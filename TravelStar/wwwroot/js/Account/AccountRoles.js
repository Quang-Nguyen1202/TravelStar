
$(document).ready(function () {
    $('#account_roles_form').submit(function (e) {
        e.preventDefault();
        var $form = $(this);
        var url = $form.attr('action');

        $.ajax({
            type: "POST",
            url: url,
            data: $form.serialize(), // serializes the form's elements.
            success: function (data) {
                if (data.status) {
                    successMessage('Lưu thành công.');
                } else {
                    errorMessage('Lỗi thệ thống.');
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
                errorMessage('Lỗi thệ thống.');
            }
        });
    });
});

