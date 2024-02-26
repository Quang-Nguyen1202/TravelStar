function DeleteConfirm(url, id, title, table) {
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
                        if (table) {
                            table.ajax.reload();
                        }
                        
                        swal({
                            title: "Remove successfully.",
                            type: "success",
                            confirmButtonClass: "btn btn-info",
                            confirmButtonText: "Ok",
                        });
                    } else {
                        swal({
                            title: "Remove unsuccessfully.",
                            text: "",
                            type: "error",
                            confirmButtonClass: "btn btn-info",
                            confirmButtonText: "Ok",
                        });
                    }
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    swal({
                        title: "Remove unsuccessfully.",
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

$("[id^=carousel-thumbs]").carousel({
    interval: false
});

/** Pause/Play Button **/
$(".carousel-pause").click(function () {
    var id = $(this).attr("href");
    if ($(this).hasClass("pause")) {
        $(this).removeClass("pause").toggleClass("play");
        $(this).children(".sr-only").text("Play");
        $(id).carousel("pause");
    } else {
        $(this).removeClass("play").toggleClass("pause");
        $(this).children(".sr-only").text("Pause");
        $(id).carousel("cycle");
    }
    $(id).carousel;
});

/** Fullscreen Buttun **/
$(".carousel-fullscreen").click(function () {
    var id = $(this).attr("href");
    $(id).find(".active").ekkoLightbox({
        type: "image"
    });
});

if ($("[id^=carousel-thumbs] .carousel-item").length < 2) {
    $("#carousel-thumbs [class^=carousel-control-]").remove();
    $("#carousel-thumbs").css("padding", "0 5px");
}

$("#carousel").on("slide.bs.carousel", function (e) {
    var id = parseInt($(e.relatedTarget).attr("data-slide-number"));
    var thumbNum = parseInt(
        $("[id=carousel-selector-" + id + "]")
            .parent()
            .parent()
            .attr("data-slide-number")
    );
    $("[id^=carousel-selector-]").removeClass("selected");
    $("[id=carousel-selector-" + id + "]").addClass("selected");
    $("#carousel-thumbs").carousel(thumbNum);
});

////
var successMessage = function (message) {
    toastr.options = {
        "positionClass": "toast-top-center",
        "closeButton": true,
        "timeOut": "2000",
        "newestOnTop": true
    };
    toastr.success(message);
};

var errorMessage = function (message) {
    toastr.options = {
        "positionClass": "toast-top-center",
        "closeButton": true,
        "timeOut": "2000",
        "newestOnTop": true
    };
    toastr.error(message);
};