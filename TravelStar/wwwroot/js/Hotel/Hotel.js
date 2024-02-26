const Hotel = {
    dataCityList: [],
    table: null,
    isSuperAdmin: null,

    initialise: () => {
        Hotel.table = $('#hotel_table').DataTable({
            ajax: {
                url: '/Hotel/GetHotelList'
            },
            columns: [
                { data: "id", width: "4%" },
                {
                    data: "emailOwner",
                    width: "15%",
                    render: function (data, type, row) {
                        return row['emailOwner'];
                    }
                },
                { data: "name", width: "10%" },
                { data: "phone", width: "10%" },
                { data: "address", width: "25%" },
                { data: "ward", width: "10%" },
                { data: "district", width: "10%" },
                { data: "city", width: "10%" },
                {
                    data: "id",
                    render: function (data, type, row) {
                        return '<button type="button" class="btn btn-primary" onclick="Hotel.selectRow(' + row['id'] + ')" style="font-size: 0.7rem" title="Detail"><i class="fa-solid fa-align-justify"></i></button>\
                                <button type = "button" class="btn btn-secondary" onclick = "Hotel.remove(' + row['id'] + ')" style="font-size: 0.7rem" title="Remove"><i class="fa-solid fa-trash-can"></i></button >';
                    },
                    orderable: false,
                    width: "21%"
                }
            ],
            "order": [[1, "desc"]],
            "scrollX": true,
            responsive: true
        });
    },

    showModalAddHotel: () => {
        Hotel.getCity();

        $('#add_hotel_modal').modal('show');
    },

    getCity: () => {
        $('#city').val('');
        $('#city').html('');
        $('#city').append('<option value="" selected>Choose...</option>');

        $.ajax({
            type: "Get",
            url: '/City',
            async: true,
            success: function (data) {
                Hotel.dataCityList = data.cityList;
                data.cityList.forEach(function (item) {
                    $('#city').append('<option value="' + item.Id + '">' + item.Name + '</option>')
                });
            },
            error: function (jqXHR, textStatus, errorThrown) {
                console.log("loi 404");
            }
        });
    },

    changeCity: () => {
        let id = $('#city').val();
        $('#district').val('');
        $('#district').html('');
        $('#district').append('<option value="" selected>Choose...</option>');

        let city = Hotel.dataCityList.filter(function (item) {
            if (item.Id == id) {
                item.Districts.forEach(function (item) {
                    $('#district').append('<option value="' + item.Id + '">' + item.Name + '</option>')
                });
                return item
            }
        });

        $('#district').trigger("change");
    },

    changeDistrict: () => {
        let cityId = $('#city').val();
        let districtId = $('#district').val();
        $('#ward').val('');
        $('#ward').html('');
        $('#ward').append('<option value="" selected>Choose...</option>');

        Hotel.dataCityList.forEach(function (item) {
            if (item.Id == cityId) {
                item.Districts.forEach(function (item) {
                    if (item.Id == districtId) {
                        item.Wards.forEach(function (item) {
                            $('#ward').append('<option value="' + item.Id + '">' + item.Name + '</option>')
                        });
                        return;
                    }
                });
            }
        });
    },

    save: () => {
        let $form = $('#create_hotel_form');
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
                        $("#add_hotel_modal .close").click();
                        Hotel.table.ajax.reload();
                    } else {
                        console.log("goi không thanh cong");
                    }
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    console.log("Loi he thong");
                }
            });
        }
    },

    remove: (id) => {
        var url = "/remove";
        var title = "Do you want to remove ?";
        DeleteConfirm(url, id, title, Hotel.table);
    },

    selectRow: (id) => {
        let checkInDate = $('#date-in').val();
        let checkOutDate = $('#date-out').val();
        window.location.href = `/hotel/rooms?hotelId=${id}&checkInDate=${checkInDate}&checkOutDate=${checkOutDate}`;
    },

    uploadImages: (event) => {
        let files = event.target.files;
        if (files.length > 0) {
            $("#upload-error").text("");
            [].forEach.call(files, Hotel.previewUpload);
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
};

$(() => {
    Hotel.initialise();

    $("#create_hotel_form").validate({
        rules: {
            name: "required",
            phone: "required",
            address: "required",
            Images: "required",
            city: "required",
            district: "required",
            wardId: "required",
            
        },

        messages: {
            name: "Please enter name of hotel",
            phone: "Please enter phone number",
            address: "Please enter address",
            Images: "Please upload images",
            city: "Please select an option",
            district: "Please select an option",
            wardId: "Please select an option",
            
        }
    });
});