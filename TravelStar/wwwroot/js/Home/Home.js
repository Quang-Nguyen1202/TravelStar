const Home = {
    search: () => {
        let $form = $('#search_hotel_form');
        let url = $form.attr('action');
        if ($form.valid()) {
            $.ajax({
                type: "POST",
                url: url,
                data: $form.serialize(),
                success: function (response) {
                    $('#container_hotel_list').html(response);
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    console.log("Loi he thong");
                }
            });
        }
    },
};



$(() => {
    $("#search_hotel_form").validate({
        rules: {
            City: "required",
            CheckInDate: "required",
            CheckOutDate: {
                required: function (element) { return ($("#end_date").val() != ""); },
                greaterDate: "#date-in"
            },
        },

        messages: {
            City: "Please enter city",
            CheckInDate: "Please enter check in date",
            CheckOutDate: {
                required: "Please enter check out date",
                greaterDate: "Must be greater than check in date"
            },
        }
    });

    $.validator.addMethod("greaterDate", function (value, element, params) {
        return this.optional(element) || new Date(value) >= new Date($(params).val());
    });
});