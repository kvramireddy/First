// JavaScript Document
$(document).ready(function () {
    
    $("#collapseOne").collapse('hide');
    
    //Enabling Toot Tip
    $('a').tooltip();

    //Set Container Height
    containerHeight();




    $(window).resize(function () {
        containerHeight();
    });








    $('#myTab a').click(function (e) {
        e.preventDefault();
        $(this).tab('show');
    })

    $(".rt_list_items ul li").click(function () {
        $(".rt_list_items ul li").removeClass("active");
        $(this).addClass("active");


    });



});




function addCustomScroll(el) {
    if (el.hasClass('mCustomScrollbar')) {
        el.mCustomScrollbar('update');
    } else {
        el.mCustomScrollbar({
            mouseWheelPixels: 500,
            scrollButtons: {
                enable: true,
                scrollSpeed: 500
            },
            advanced: {
                updateOnBrowserResize: true,
                updateOnContentResize: true,
                autoScrollOnFocus: false
            },
            callbacks: {
                onScrollStart: function () { $('.datepicker').hide(); }
            }
        });
    }
    containerHeight();
}

var viewWidth = $(window).height() - 155;


function containerHeight() {

    $(".set_height").css("height", $(window).height() - 52);
    $(".MapHeight").css("height", $(window).height() - 60);

    $("#RightMapPanel").css("width", $(window).width() - 370);

    $(".MapWidth").css("width", $(window).width() - 370);

    $("#ViewListContainer,#ViewListContainer .mCustomScrollBox ").css("max-height", $(window).height() - 50);
    $("#LegendListContainer,#LegendListContainer .mCustomScrollBox ").css("max-height", $(window).height() - 110);

    //$("#ActiveLayerList-1,#ActiveLayerList-1 .mCustomScrollBox ").css("height", viewWidth / 2);
    //$("#ActiveLayerList-2,#ActiveLayerList-2 .mCustomScrollBox ").css("height", viewWidth / 2);

    //$("#LayerListContainer-2,#LayerListContainer-2 .mCustomScrollBox ").css("max-height", viewWidth / 2 - 42);
    //$("#LayerListContainer-1,#LayerListContainer-1 .mCustomScrollBox ").css("max-height", viewWidth / 2 - 42);






}