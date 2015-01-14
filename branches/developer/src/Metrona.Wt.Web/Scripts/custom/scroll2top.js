(function ($) {
    $.extend({

        scrollToTop: function () {

            var isScrolling = false;

            // Append Button
            $("body").append($("<a />")
				.addClass("scroll-to-top")
				.attr({
				    "href": "#",
				    "id": "scrollToTop",
                    "title": "nach oben"
				})
				.append(
					$("<i aria-hidden='true'/>")
						.addClass("glyphicon glyphicon-upload")
				));

            $("#scrollToTop").click(function (e) {

                e.preventDefault();
                $("body, html").animate({ scrollTop: 0 }, 500);
                return false;

            });

            // Show/Hide Button on Window Scroll event.
            $(window).scroll(function () {

                if (!isScrolling) {

                    isScrolling = true;

                    if ($(window).scrollTop() > 150) {

                        $("#scrollToTop").stop(true, true).addClass("visible");
                        isScrolling = false;

                        $(".arrow-one").css("display", "none");
                        $(".arrow-two").css("display", "none");

                    } else {

                        $("#scrollToTop").stop(true, true).removeClass("visible");
                        isScrolling = false;

                        $(".arrow-one").css("display", "block");
                        $(".arrow-two").css("display", "block");

                    }

                }

            });

        }

    });
})(jQuery);