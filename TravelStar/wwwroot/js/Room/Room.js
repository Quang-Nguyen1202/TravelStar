const Room = {
    showModalAddRoom: () => {
        $('#add_room_modal').modal('show');
    },

    showBookingModal: (roomId, price) => {
        $('#room_id_txb').val(roomId);
        $('#total_price_txb').val(price);
        $('#booking-room-price').text('$ ' + price)
        $('#booking_room_modal').modal('show');
    },

    save: () => {
        let $form = $('#create_room_form');
        let formData = new FormData($form.get(0));
        let url = $form.attr('action');
        if ($form.valid()) {
            $.ajax({
                type: "POST",
                url: url,
                data: formData,
                processData: false,
                contentType: false,
                success: function (data) {
                    if (data) {
                        $("#add_room_modal .close").click();
                        location.reload();
                    } else {
                        console.log("goi khong thanh cong");
                    }
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    console.log("Loi he thong");
                }
            });
        }

    },

    processBooking: () => {
        let $form = $('#booking_room_form');
        let url = $form.attr('action');
        $.ajax({
            type: "POST",
            url: url,
            data: $form.serialize(),
            success: function (data) {
                if (data.status) {
                    $('#booking_id').val(data.bookingId);
                    $('#customer_email').val(data.customerEmail);
                    $("#booking_room_modal .close").click();

                    $('#confirm_booking_modal').modal('show');
                } else {
                    console.log("goi khoong thanh cong");
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
                console.log("Loi he thong");
            }
        });
    },

    checkBookingConfirm: () => {
        let $form = $('#confirm_booking_form');
        let url = $form.attr('action');
        $.ajax({
            type: "POST",
            url: url,
            data: $form.serialize(),
            success: function (data) {
                if (data) {
                    $('#confirm_booking_modal').modal('hide');
                    swal({
                        title: "Booking successfully.",
                        type: "success",
                        confirmButtonClass: "btn btn-info",
                        confirmButtonText: "Ok",
                    });
                } else {
                    swal({
                        title: "Booking unsuccessfully.",
                        text: "Mã xác nhận booking chưa đúng, vui lòng kiểm tra và nhập lại",
                        type: "error",
                        confirmButtonClass: "btn btn-info",
                        confirmButtonText: "Ok",
                    });
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
                console.log("Loi he thong");
            }
        });
    },

    uploadImages: (event) => {
        let files = event.target.files;
        if (files.length > 0) {
            $("#upload-error").text("");
            [].forEach.call(files, Room.previewUpload);
        } else {
            $('.btn-clear').click();
        }

    },

    previewUpload: (file) => {
        if (/\.(jpe?g|png|gif)$/i.test(file.name)) {
            var reader = new FileReader();
            reader.addEventListener('load', function () {
                const html =
                    '<div class="form-upload__item mb-2">' +
                    '<div class="form-upload__item-thumbnail" style="background-image: url(' + this.result + ')"></div>' +
                    '</div>';
                $('.form-upload__preview').append(html);
                $('.btn-clear').show();
            }, false);
            reader.readAsDataURL(file);
        } else {
            $('.btn-clear').click();

            swal({
                title: "",
                text: "Please upload image only.",
                type: "error",
                confirmButtonClass: "btn btn-info",
                confirmButtonText: "Ok",
            });
        }
    },

    clearImages: (element) => {
        $('.form-upload__item').remove();
        $('.form-upload__control').val('');
        $(element).hide();
    },

    removeRoom: (id) => {
        var title = "Do you want to remove this room ?";
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
                    url: "/removeroom",
                    data: data,
                    success: function (data) {
                        if (data) {
                            swal({
                                title: "Remove room successfully.",
                                type: "success",
                                confirmButtonClass: "btn btn-info",
                                confirmButtonText: "Ok",
                            });

                            location.reload();
                        } else {
                            swal({
                                title: "Remove room unsuccessfully.",
                                text: "",
                                type: "error",
                                confirmButtonClass: "btn btn-info",
                                confirmButtonText: "Ok",
                            });
                        }
                    },
                    error: function (jqXHR, textStatus, errorThrown) {
                        swal({
                            title: "Remove room unsuccessfully.",
                            text: "",
                            type: "error",
                            confirmButtonClass: "btn btn-info",
                            confirmButtonText: "Ok",
                        });
                    }
                });
            }
        );
    }
};

$(() => {
    $("#create_room_form").validate({
        rules: {
            name: "required",
            description: "required",
            type: "required",
            beds: "required",
            Price: "required",
            Images: "required",
            CustomerEmail: "required",
            CustomerPhone: "required",

        },

        messages: {
            name: "Please enter name room",
            description: "Please enter description",
            type: "Please enter type",
            beds: "Please choose number of bed",
            Price: "Please enter price ",
            Images: "Please select photo",
            CustomerEmail: "Please enter your email",
            CustomerPhone: "Please enter your phone number",
        }
    });
   
});