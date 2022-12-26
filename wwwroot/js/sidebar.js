$(".bars-icon").click(function () {
  $(".sidebar").toggleClass("width-reduce");
  $(".bars-icon").toggleClass("add-fa-bars");
});

$(".side-nav-link.nav-link").click(function () {
  
  if ($(window).width() > 991) {
    $(".sidebar").removeClass("width-reduce");
  }
// $(this).toggleClass("active");
   if($(this).parent().hasClass("active")){
    $(this).parent().removeClass("active");
   
  }else{
    
    $(".side-nav-item").removeClass("active");
    $(this).parent().addClass("active");
  }


});