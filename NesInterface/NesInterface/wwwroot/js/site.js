window.toastInstance = null;

window.showToast = function (message, type) {
    if (toastInstance) {
        toastInstance.hide();
    }

    const $toast = $('#ajaxToast');
    const toast = $toast.get(0);

    $toast.removeClass('bg-success bg-danger bg-warning bg-info');

    switch (type) {
        case 'success':
            $toast.addClass('bg-success');
            break;
        case 'danger':
            $toast.addClass('bg-danger');
            break;
        case 'warning':
            $toast.addClass('bg-warning');
            break;
        case 'info':
            $toast.addClass('bg-info');
            break;
        default:
            $toast.addClass('bg-success');
    }

    $('#ajaxToastBody').text(message);

    toastInstance = new bootstrap.Toast(toast, {
        autohide: true,
        delay: 3000
    });

    toastInstance.show();

    $toast.on('hidden.bs.toast', function () {
        toastInstance = null;
    });
}