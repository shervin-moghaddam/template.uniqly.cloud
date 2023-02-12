// Show a message on screen
// @Type        : 0: Message, 1: Confirmation, 2: Error, 3: Warning
// @Timer       : Milliseconds for staying alive (2 seconds default)
// @Text        : Text to be displayed
// @Position    : 0,1,2: Upper left, center right, 3 : center, 4-6 Bottom left,center,right)
var messageBoxes = [] // Array of message boxes (future use)
function showMessage(options) {

    // Defaults if nothing is entered
    if (options.timer === 0 || options.timer === undefined) options.timer = 3000; // 3 seconds
    if (options.type === undefined) options.type = 0; // default message
    if (options.position === undefined) options.position = 5; // center bottom

    // Prepare elements
    let messageEl = document.createElement('div');
    $(messageEl).click(function () { closeMessage(messageEl) });
    let iconEl = document.createElement('div')
    let textEl = document.createElement('div');

    // Text
    $(textEl).addClass('message-text').html('<a>' + options.text + '</a>')

    // Type
    $(messageEl).addClass("notification-popup-wrapper show-message"); // show-message starts the animation
    switch (options.type) {
        case 0:
        case 1:
            $(messageEl).addClass("confirmation");
            $(iconEl).addClass('icon').html('<i class="far fa-check"></i>');
            break;
        case 2:
            $(messageEl).addClass("error");
            $(iconEl).addClass('icon').html('<i class="far fa-times"></i>');
            break;
        case 3:
            $(messageEl).addClass("warning");
            $(iconEl).addClass('icon').html('<i class="fas fa-exclamation"></i>');
            break;
    };

    // Position
    let positionClass;
    switch (options.position) {
        case 0: positionClass = 'top-left'; break;
        case 1: positionClass = 'top-center'; break;
        case 2: positionClass = 'top-right'; break;
        case 3: positionClass = 'center'; break;
        case 4: positionClass = 'bottom-left'; break;
        case 5: positionClass = 'bottom-center'; break;
        case 6: positionClass = 'bottom-right'; break;
    };

    // for animation (only when bottom)
    switch (options.position) {
        case 4:
        case 5:
        case 6: $(messageEl).animate({ bottom: "15px" }, 300); break;
    }

    // Add the elements to parent
    $('body').append(
        $(messageEl).addClass(positionClass).append(iconEl).append(textEl));
    setTimeout(() => {
        closeMessage(messageEl);
    }, options.timer);
}

// Activating closeMessage
function closeMessage(messageEl) {
    $(messageEl).addClass('close-message').removeClass('show-message');
    setTimeout(() => {
        $(messageEl).remove();
    }, 400); // Same time span as close-notification-popup animation in css
}