// Popup Management //
var popuplistArray = [];
var currentPopupCounter = 0; // Only add to this counter, never subtract
var useBackgroundDarkener = true; // Set to false if blurring is enabled

// Close a single popup window
// @elementID  : Element ID of the parent element for a popup
function closepopup(elementID) {
    // Check if ElementID exists
    if (popuplistArray.includes(elementID)) {
        var popupElement = document.getElementById(elementID);
        if (!isEmpty(popupElement)) {

            // Get identifier before closing 
            const identifierElement = popupElement.getElementsByClassName('popupIdentifier');
            let popupIdentifier;
            if (!isEmpty(identifierElement)) popupIdentifier = identifierElement[0].value;

            // Show close animation, and then remove the element
            $(popupElement).addClass('popup-closeanimation');
            setTimeout(() => {
                $(popupElement).remove();
            }, 200);

            let arrayIndex = popuplistArray.indexOf(elementID);
            popuplistArray.splice(arrayIndex, 1);

            // Get last element if it exists
            if (popuplistArray.length > 0) {
                let nextPopupElement = document.getElementById(popuplistArray[popuplistArray.length - 1]);
                if (!isEmpty(nextPopupElement)) {
                    $(nextPopupElement).addClass('unblurred');
                }
                currentPopupCounter -= 1;
                if (useBackgroundDarkener) setBackgroundZindex();

            } else { // no more elements exists
                currentPopupCounter = 0;
                let mainpageElement = document.getElementById('mainpage-element');
                $(mainpageElement).addClass('unblurred');
                if (useBackgroundDarkener)
                    $('#popup-background').removeClass('popup-background-active').css("z-index", ""); // Remove dark background and z-index
            }

            // Run post-close script
            if (!isEmpty(popupIdentifier)) postClosePopup(popupIdentifier);
        }
    }
}

// Close ALL popups in a page
function closeAllPopups() {
    // Copy the array first, since the original array is altered along the way
    // when popups starts closing
    var copyArray = popuplistArray.slice();
    copyArray.forEach(function (elementID, index) {
        closepopup(elementID);
    });
    currentPopupCounter = 0;
}

// Returns the next popup ID available, based on the current popup counter
// NOTE: The popup id is NOT reserved. If this function is called twice without openen a pop up,
// both times the same ID will be returned.
function getNextPopupID() {
    const ZIndex = currentPopupCounter + 5000 + 1; // length + 5000 + 1 as it's the next in line
    currentPopupCounter += 1;
    return 'PopupID-' + ZIndex;
}

// Creates a popup, and starts opening animation
// If width or height is less than 100, it's considered to be percent. Above 100 is pixels.
// NOTE: @sourceIdf and @idf might be removed in the future.
// @w          : Width
// @h          : Height
// @sourceIdf  : (Only for automated scripts) Source element Identifier (used to know where this popup came from)
// @idf        : (Only for automated scripts) This popup identifer (if needed to use in the backend)
// @popupElementID : If an ID already exists for the popup (leave blank for automatic)
function openpopup(w, h, sourceIdf, idf, popupElementID) {
    // If no popupElementID has been entered
    if (isEmpty(popupElementID)) popupElementID = getNextPopupID();
    const ZIndex = popupElementID.split('-')[1]; // The number after hyphn is the z-index.

    // Add to array
    popuplistArray.push(popupElementID);

    // Get the popup
    $.ajax({
        type: "POST",
        async: false,
        data: {
            ElementID: popupElementID, ZIndex: ZIndex, Width: w, Height: h,
            sourceElementIdentifier: sourceIdf, identifier: idf
        },
        url: "/Popup/GetPopup",
        success: function (returndata) {

            // Blur everything
            var unblurredElements = $('.unblurred').map(function () {
                $(this).removeClass("unblurred");
            }).get();

            // Get popup
            $('body').append(returndata);

            $(document.getElementById(popupElementID)).addClass('unblurred');

            // Dark background
            if (useBackgroundDarkener) {
                $('#popup-background').addClass('popup-background-active') // Activate dark background
                setBackgroundZindex();
            }
        },
        error: function (returndata) {
        }
    });
    return popupElementID;
}

// Manually set the background index, if it's activated
function setBackgroundZindex() {
    if (!useBackgroundDarkener) return;
    const BackgroundZIndex = currentPopupCounter + 5000; // Find the largest z-index possible
    $('#popup-background').css('z-index', BackgroundZIndex); // Change z-index of the background darkener to match top-most popup
}

// Global keydown event for popup frames
function popupFrameKeydown(e, listIdentifier, popupElementID, identifier) {
    // Only affect the top popupFrame 
    if ((popuplistArray[popuplistArray.length - 1]) == popupElementID) {
        switch (e.keyCode) {
            case 27: // Escape

                switch (identifier) {

                }
                closepopup(popupElementID);
                break;
        }
    }
}

// HELPERS

// When the popup closes, a script can be run here.
// If no script is found for identifier, the default is that identifier = elementID to focus
function postClosePopup(identifier) {
    if (isEmpty(identifier)) return;
    switch (identifier) {
        // Window specific code run here 
        default:
            // focus on the element in the popup that is on top
            var topPopupID = popuplistArray[popuplistArray.length - 1]; // Find ID of the most top frame
            var popupFrameElement = document.getElementById(topPopupID); // Get element of the top popup frame
            var el = $(popupFrameElement).find("#" + identifier) // Find the element we're looking for inside popup frame
            if (!isEmpty(el)) el.focus(); // If found, focus!
            break;
    }
}