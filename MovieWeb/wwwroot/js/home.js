$(document).ready(function () {
    $(".submenu1").hide();
    var movie = new Movie();
})
class Movie {
    constructor() {
        this.initEvents();
    }
    initEvents() {
        $(".text-search").on("input", this.textChanged);
        $(".text-search").on("remove", this.textChanged);
    }
    textChanged() {
        var data = $(".text-search").val();
        if (data == "") {

            $(".submenu1").hide();
            return;
        } 
        var str=''
        $(".submenu1").show();
        $.ajax({
            type: 'GET',
            url: '/api/SearchFilm?name='+data,
            dataType: 'json',
            success: function (data) {
                $('.dialog').empty();
                $.each(data, function (key, val) {
                    str += `<li class="item-search">
                    <img src="${val.anh}" alt="">
                    <a href="/Film/InfoFilm?filmCode=${val.maPhim}">${val.tenPhim}</a>
                    </li>`
                })
                $('.submenu1').html(str)
            }
        })

    }
}
