const Revenue = {

    initialise: () => {
        Revenue.table = $('#revenue_table').DataTable({
            ajax: {
                url: '/Revenue/GetBookingList',
                data: function (data) {
                    data.fromDate = $('#from-date').val();
                    data.toDate = $('#to-date').val();
                },
                complete: function (data) {
                    console.log(data);
                    let totalPrice = 0;
                    data.responseJSON.data.forEach(function (item, index) {
                        totalPrice = totalPrice + item.totalPrice;
                    });

                    $('#total-price').text(`Tổng doanh thu: $${totalPrice}`);
                } 
            },
            columns: [
                { data: "id", width: "4%" },

                { data: "customerEmail", width: "15%" },
                { data: "hotelName" , width : "15%" },
                { data: "roomName", width: "10%" },
                { data: "checkInDate", width: "20%" },
                { data: "checkOutDate", width: "20%" },
                { data: "totalPrice", width: "10%" },
            ],
            "scrollX": true,
            responsive: true
        });
    },

    search: () => {
        Revenue.table.ajax.reload();
    },

    exportExcel: () => {
        let fromDate = $('#from-date').val();
        let toDate = $('#to-date').val();
        window.location.href = `/Revenue/ExportRevenueInExcel?fromDate=${fromDate}&toDate=${toDate}`;
    }
}
$(() => {
    Revenue.initialise();
    $(".search-date-input").datepicker({
        dateFormat: 'dd MM, yy'
    });
});