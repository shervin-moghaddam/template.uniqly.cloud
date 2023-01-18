// Caller -- await --> createDialog -- await --> responseFromDialog
// Caller must close the dialog
// Returns: {dialogNo: 0, buttonIDF: 'btn_yes', popupElementID: '4510-1'}
async function dialogTest() {
    let newDialog = new dialogResponseClass(1001);
    var resVar = newDialog.responseFromDialog().then((response) => {
        console.log(response);
        newDialog.closeDialog();
    })

    //newDialog.resolve('response');

}

// Show a simple OK dialog with a text (a light version of dialogResponseClass)
// No response is assigned.
function showDlg(dialogNo) {
    const popupElementID = getNextPopupID();
    $.ajax({
        type: 'post',
        async: false,
        url: '/dlg/getDlg',
        data: {dlgNo: dialogNo, popupElementID: popupElementID},
        success: function (returndata) {
            const jsonReturn = JSON.parse(returndata);
            const popupElement = $('#' + popupElementID + '-content');
            $(popupElement).on('click', '#btn_ok', function () {
                closepopup(popupElementID)
            });
            openpopup(0, 0, 'dlgframe', '', popupElementID); // Create a popup
            $(popupElement).html(jsonReturn.dlg);
        }
    });
}


class popupResponseClass {
    constructor(popupElementID,w,h) {
        this.popupElementID = popupElementID;
        this.w = w;
        this.h = h;
    }

    // Awaits a response from a manually created popup frame
    responseFromPopup() {
        this.pro = new Promise((resolve, reject) => {
            this.resolve = resolve;
            this.reject = reject;
            openpopup(this.w, this.h, '', '', this.popupElementID); // Create a popup
        })
        return this.pro;
    }

    closeDialog() {
        closepopup(this.popupElementID);
    }
}


class dialogResponseClass {
    constructor(dialogNo) {
        this.dialogNo = dialogNo;
    }

    responseFromDialog() {
        this.pro = new Promise((resolve, reject) => {
            this.resolve = resolve;
            this.reject = reject;
            const popupElementID = getNextPopupID();
            this.popupElementID = popupElementID;
            $.ajax({
                type: 'post',
                async: false,
                url: '/dlg/getDlg',
                data: {dlgNo: this.dialogNo, popupElementID: popupElementID},
                success: function (returndata) {
                    const jsonReturn = JSON.parse(returndata);

                    // @returndata contains two JSON objects:
                    // @dlg     : Dialog window rendereded HTML
                    // @btnLst  : Button list to 
                    openpopup(0, 0, 'dlgframe', '', popupElementID); // Create a popupz
                    const popupElement = $('#' + popupElementID + '-content');

                    // Assign a click event for each button element.
                    // When the button is clicked, resolve this promise.
                    $.each(jsonReturn.btnLst.split(','), function (index, btnID) {
                        $(popupElement).on('click', '#' + btnID, function () {
                            const responseValue = '{"dialogNo": ' + jsonReturn.dialogNo + ', "btnID": "' + btnID + '", "PopupElementID": "' + popupElementID + '"}';
                            resolve(JSON.parse(responseValue));
                        });
                    });
                    $(popupElement).html(jsonReturn.dlg);
                }
            })
        })
        return this.pro;

    }


    closeDialog() {
        closepopup(this.popupElementID);
    }
}