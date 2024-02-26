const Booking = {
    checkCustomerConfirm: (email) => {
        let data = null;

        if (email) {
            data = { email: email };
        } else {
            data = { email: $('#confirm_email').val() };
        }
        
        let url = "/booking/confirmcustomer";
        $.ajax({
            type: "GET",
            url: url,
            data: data,
            success: function (data) {
                $('#container_booking').html(data);
                Booking.closeCustomerConfirm();
            },
            error: function (jqXHR, textStatus, errorThrown) {
                console.log("Loi he thong");
            }
        });
    },

    closeCustomerConfirm: () => {
        $('#confirm_customer_modal').modal('hide');
    },

    cancelBooking: (btnCancel, id) => {
        var url = "/cancelbooking";
        var title = "Do you want to cancel booking ?";
        swal({
            title: title,
            type: "warning",
            showCancelButton: true,
            cancelButtonText: "No",
            cancelButtonClass: "btn btn-secondary",
            confirmButtonClass: "btn btn-danger",
            confirmButtonText: "Yes",
            closeOnConfirm: false,
            showLoaderOnConfirm: true
        },
            function () {
                let data = {
                    id: id
                }

                $.ajax({
                    type: "POST",
                    url: url,
                    data: data,
                    success: function (data) {
                        if (data) {
                            $(btnCancel).attr('disabled', 'disabled');
                            $(btnCancel).text('Canceled');
                            $(btnCancel).removeClass('btn-outline-info');
                            $(btnCancel).addClass('btn-outline-danger');

                            swal({
                                title: "Cancel booking successfully.",
                                type: "success",
                                confirmButtonClass: "btn btn-info",
                                confirmButtonText: "Ok",
                            });
                        } else {
                            swal({
                                title: "Cancel booking unsuccessfully.",
                                text: "",
                                type: "error",
                                confirmButtonClass: "btn btn-info",
                                confirmButtonText: "Ok",
                            });
                        }
                    },
                    error: function (jqXHR, textStatus, errorThrown) {
                        swal({
                            title: "Cancel booking unsuccessfully.",
                            text: "",
                            type: "error",
                            confirmButtonClass: "btn btn-info",
                            confirmButtonText: "Ok",
                        });
                    }
                });
            }
        );
    },
};

$(() => {
    $("#booking_room_form").validate({
        rules: {
            CustomerEmail: "required",
            CustumerPhone: "required",
        },
        messages: {
            CustomerEmail: "Please enter your email",
            CustomerPhone: "Please enter your phone number",
        }
    });
});