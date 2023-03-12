let trans = () => {
    document.documentElement.classList.add('transition');
    window.setTimeout(() => {
        document.documentElement.classList.remove('transition');
    }, 1000)
}

function tabSwitcher() {
    const $tabButton = $('.tab-nav');

    $($tabButton).each(function() {
        // Which tab is active by default?
        $('.tab-button.active').each(function() {
            const $subID = $(this).attr('subid');
            $('#' + $subID).addClass('active');
        });

        $('.tab-nav').each(function() {
            $(this).on('click', 'a', function(e){
                $(this).addClass('active').siblings().removeClass('active');
            });
        });

        
        $(this).on('click', 'a', function(e){
            const subID = $(this).attr('subid');
            const $parentTabEl = $(this).closest('.tab-nav-wrapper');

            const currentSubEl = $($parentTabEl).find('.tab-sub.active');
            const nextSubEl = $($parentTabEl).find('#' + subID);
            const serverLink = $(this).attr('serverlink');

            //$(subEl).removeClass('tab-transition-in');
            if (typeof serverLink !== 'undefined' && serverLink !== false) {
                // $.ajax({
                //                type: "POST", url: "/home/getpage", data: {idf: 'contact', uid: uid},
                //                beforeSend: function () {
                //                    $($mainpage).addClass('tab-transition-out');
                //                },
                //                success: function (returndata) {
                //                    setTimeout(
                //                        function () {
                //                            //$($mainpage).removeClass('tab-transition-out');
                //                            //$($mainpage).addClass('tab-transition-in');
                //                            //$mainpage.html(returndata);
                //                            //runFunction(uid + '_func');
                //                        }, 200); // Same time as animation effect
                //                }
                //            });    
            }
            else {
                $(currentSubEl).removeClass('tab-transition-in');
                $(currentSubEl).addClass('tab-transition-out');
                setTimeout(
                    function () {
                        $(currentSubEl).removeClass('active');
                        $(nextSubEl).removeClass('tab-transition-out');
                        $(nextSubEl).addClass('tab-transition-in');
                        $(nextSubEl).addClass('active');

                        //$mainpage.html(returndata);
                        //runFunction(uid + '_func');
                    }, 60); // Same time as animation effect
            }

        });
    });
}

function tabSwitcherStartup() {
   
}

// Run scripts on startup
$(document).ready(function() {
    tabSwitcher();
    alerts();
    tabSwitcherStartup();
});