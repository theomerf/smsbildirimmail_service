using Autofac;
using Hangfire;
using System;

public class AutofacJobActivator : JobActivator
{
    private readonly ILifetimeScope _lifetimeScope;

    public AutofacJobActivator(ILifetimeScope lifetimeScope)
    {
        _lifetimeScope = lifetimeScope;
    }

    public override JobActivatorScope BeginScope(JobActivatorContext context)
    {
        var scope = _lifetimeScope.BeginLifetimeScope();
        return new AutofacJobActivatorScope(scope);
    }

    public override object ActivateJob(Type jobType)
    {
        return _lifetimeScope.Resolve(jobType);
    }

    private class AutofacJobActivatorScope : JobActivatorScope
    {
        private readonly ILifetimeScope _scope;

        public AutofacJobActivatorScope(ILifetimeScope scope)
        {
            _scope = scope;
        }

        public override object Resolve(Type type)
        {
            return _scope.Resolve(type);
        }

        public override void DisposeScope()
        {
            _scope.Dispose();
        }
    }
}