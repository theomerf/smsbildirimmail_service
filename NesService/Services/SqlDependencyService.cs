using Entities.Models;
using Services.Contracts;
using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Services
{
    public class SqlDependencyService : IDisposable
    {
        private readonly ILoggerService _logger;
        private readonly Action<Guid, RequestType> _callback;
        private SqlConnection _connection;
        private SqlDependency _dependency;
        private readonly string _connectionString;
        private bool _disposed = false;
        private readonly object _lockObject = new object();

        public SqlDependencyService(ILoggerService logger, Action<Guid, RequestType> callback)
        {
            _logger = logger;
            _callback = callback;
            _connectionString = ConfigurationManager.ConnectionStrings["DatabaseString"].ConnectionString;
        }

        public void StartWatching()
        {
            try
            {
                SqlDependency.Start(_connectionString);
                SetupDependency();
            }
            catch (Exception ex)
            {
                _logger.LogError($"SQL Dependency başlatma hatası: {ex.Message}");
                throw;
            }
        }

        public void StopWatching()
        {
            try
            {
                _disposed = true;

                CleanupConnection();

                if (!string.IsNullOrEmpty(_connectionString))
                {
                    SqlDependency.Stop(_connectionString);
                }

                _logger.LogDebug("SQL Dependency durduruldu.");
            }
            catch (Exception ex)
            {
                _logger.LogError($"SQL Dependency durdurma hatası: {ex.Message}");
            }
        }

        private void SetupDependency()
        {
            lock (_lockObject)
            {
                try
                {
                    if (_connection != null)
                    {
                        if (_dependency != null)
                        {
                            _dependency.OnChange -= OnDependencyChange;
                            _dependency = null;
                        }

                        if (_connection.State != System.Data.ConnectionState.Closed)
                        {
                            _connection.Close();
                        }
                        _connection.Dispose();
                        _connection = null;
                    }

                    if (_disposed) return;

                    _connection = new SqlConnection(_connectionString);
                    _connection.Open();

                    string query = "SELECT [Id], [Type] FROM [dbo].[Requests] WHERE [Status] = 0";

                    using (var command = new SqlCommand(query, _connection))
                    {
                        _dependency = new SqlDependency(command);
                        _dependency.OnChange += OnDependencyChange;

                        command.ExecuteNonQuery();
                    }

                    _logger.LogDebug("SQL Dependency kuruldu ve aktifleştirildi.");
                }
                catch (Exception ex)
                {
                    _logger.LogError($"SQL Dependency setup hatası: {ex.Message}");
                    CleanupConnection();
                }
            }
        }

        private void OnDependencyChange(object sender, SqlNotificationEventArgs e)
        {
            try
            {
                _logger.LogDebug($"SQL Dependency tetiklendi - Type: {e.Type}, Info: {e.Info}, Source: {e.Source}");

                if (e.Type == SqlNotificationType.Change && e.Info == SqlNotificationInfo.Insert)
                {
                    Task.Run(CheckForNewRequests);
                }

                if (!_disposed)
                {
                    Task.Run(SetupDependency);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"SQL Dependency change event hatası: {ex.Message}");
            }
        }

        private async Task CheckForNewRequests()
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    // Son 30 saniyede eklenen pending request'leri bul
                    string query = @"
                        SELECT [Id], [Type] 
                        FROM [dbo].[Requests] 
                        WHERE [Status] = 0 
                        AND [CreatedAt] > DATEADD(SECOND, -30, GETDATE())
                        ORDER BY [CreatedAt] DESC";

                    using (var command = new SqlCommand(query, connection))
                    {
                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                Guid requestId = reader.GetGuid(reader.GetOrdinal("Id"));
                                RequestType requestType = (RequestType)reader.GetInt32(reader.GetOrdinal("Type"));

                                _logger.LogDebug($"Yeni request bulundu - ID: {requestId}, Type: {requestType}");

                                await Task.Run(() => _callback(requestId, requestType));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Yeni request'leri kontrol ederken hata: {ex.Message}");
            }
        }

        private void CleanupConnection()
        {
            try
            {
                if (_dependency != null)
                {
                    _dependency.OnChange -= OnDependencyChange;
                    _dependency = null;
                }

                if (_connection != null)
                {
                    if (_connection.State != System.Data.ConnectionState.Closed)
                    {
                        _connection.Close();
                    }
                    _connection.Dispose();
                    _connection = null;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Connection temizleme hatası: {ex.Message}");
            }
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                StopWatching();
            }
        }
    }
}