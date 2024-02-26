var table;
$(document).ready(function () {
    table = $('#account_table').DataTable({
        ajax: {
            url: '/Account/GetAccountList'
        },
        columns: [
            
            { data: "email"},
            {
                data: "createdDate",
                render: function (data, type, row) {
                    var date = new Date(row.createdDate)
                    var temp = new Date(date.getFullYear(), date.getMonth(), date.getDate() + 1).toISOString().substr(0, 10).split('-');
                    var formatDate = temp[1] + '/' + temp[2] + '/' + temp[0];
                    return formatDate;
                }
            },
            {
                data: "id",
                render: function (data, type, row) {
                    var manageRolesURL = '/accountrole?userId=' + data;
                    var html = '<div class="text-right"><button type="button" class="btn btn-info btn-in-cell" onclick="ShowAccountEditModal(this)" title="Chỉnh sửa tài khoản" style="font-size: 0.7rem"> \
                                    <i class="fas fa-edit" ></i> \
                                </button> \
                                <a href="' + manageRolesURL + `" class="btn btn-info btn-in-cell" title="Quản lý quyền tài khoản" style="font-size: 0.7rem">\
                                    <i class="fa fa-wrench"></i> \
                                </a> \
                                <button type="button" class="btn btn-info btn-in-cell" onclick="ShowDeleteConfirm('${data}', '${row.email}')" title="Xóa tài khoản" style = "font-size: 0.7rem"> \
                                    <i class="fa fa-trash"></i> \
                                </button></div>`;
                    return html;
                },
                orderable: false
            }
        ],
        "order": [[0, "desc"]],
        "scrollX": true,
        "responsive": true,
        "paging": false,
        "info": false
    });
    
    //validation
    //validatorAdd = $('#add_account_form').validate({
    //    rules: {
    //        "UserFullName": {
    //            required: true
    //        },
    //        "Email": {
    //            required: true
    //        }
    //    },
    //    messages: {
    //        "UserFullName": {
    //            required: 'Họ và tên không được để trống'
    //        },
    //        "Email": {
    //            required: 'Email không được để trống'
    //        }
    //    }
    //});

    //validatorEdit = $('#edit_account_form').validate({
    //    rules: {
    //        "UserFullName": {
    //            required: true
    //        },
    //        "Email": {
    //            required: true
    //        }
    //    },
    //    messages: {
    //        "UserFullName": {
    //            required: 'Họ và tên không được để trống'
    //        },
    //        "Email": {
    //            required: 'Email không được để trống'
    //        }
    //    }
    //});

    $('#add_account_btn').on('click', function () {
        $('#add_account_modal').modal('show');
    });

    $('.close_modal').on('click', function () {
        $('#add_account_form').trigger("reset");
        validatorAdd.resetForm();
        validatorEdit.resetForm();
    });

    $('#add_account_form').submit(function (e) {
        e.preventDefault();
        var $form = $(this);
        var url = $form.attr('action');
        if ($form.valid()) {
            $('#add_account_savebtn').html('<i class="fa fa-spinner fa-spin"></i> Lưu ');
            $('#add_account_savebtn').prop('disabled', true);
            $('.close_modal').prop('disabled', true);

            $.ajax({
                type: "POST",
                url: url,
                data: $form.serialize(), // serializes the form's elements.
                success: function (data) {
                    $('#add_account_savebtn').html('Lưu');
                    $('#add_account_savebtn').prop('disabled', false);
                    $('.close_modal').prop('disabled', false);

                    if (data.status) {
                        if (!data.isExist) {
                            $('#add_account_modal').modal('hide');
                            $('#add_account_form').trigger("reset");
                            table.ajax.reload();
                            swal({
                                title: "Thêm tài khoản thành công",
                                text: "Password: 123Pa$$word!",
                                type: "success",
                                confirmButtonClass: "btn btn-info",
                                confirmButtonText: "Ok",
                            });
                        } else {
                            swal({
                                title: "Thêm tài khoản không thành công",
                                text: "Email đã tồn tại, vui lòng đăng ký một email khác.",
                                type: "error",
                                confirmButtonClass: "btn btn-info",
                                confirmButtonText: "Ok",
                            });
                        }
                    } else {
                        swal({
                            title: "Thêm tài khoản không thành công",
                            text: "Đã có lỗi từ hệ thống, vui lòng liên hệ kỹ thuật viên để biết thêm chi tiết." + data.error,
                            type: "error",
                            confirmButtonClass: "btn btn-info",
                            confirmButtonText: "Ok",
                        });
                    }
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    swal({
                        title: "Thêm tài khoản không thành công",
                        text: "Đã có lỗi từ hệ thống, vui lòng liên hệ kỹ thuật viên để biết thêm chi tiết.",
                        type: "error",
                        confirmButtonClass: "btn btn-info",
                        confirmButtonText: "Ok",
                    });
                }
            });
        }
    });

    $('#edit_account_form').submit(function (e) {
        e.preventDefault();
        var $form = $(this);
        var url = $form.attr('action');

        if ($form.valid()) {
            $.ajax({
                type: "POST",
                url: url,
                data: $form.serialize(), // serializes the form's elements.
                success: function (data) {
                    if (data.status) {
                        table.ajax.reload();
                        successMessage('Lưu thành công.');

                        $('#edit_account_modal').modal('hide');
                    } else {
                        errorMessage('Lỗi thệ thống.');
                    }
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    errorMessage('Lỗi thệ thống.');
                }
            });
        }
    });
});

function ShowAccountEditModal(button) {
    var tr = $(button).closest('tr');
    var data = table.row(tr).data();
    console.log(data);
    $('#edit_account_form #user_full_name').val(data.userFullName);
    $('#edit_account_form #email').val(data.email);
    $('#edit_account_form #Id').val(data.id);

    $('#edit_account_modal').modal('show');

    table.$('tr.rowTableSelected').removeClass('rowTableSelected');
    $(tr).addClass('rowTableSelected');
}

function ShowDeleteConfirm(id, email) {
    var url = "/Account/DeleteAccount";
    var title = "Bạn muốn xóa tài khoản " + email + "?";
    DeleteConfirm(url, id, title);
}

