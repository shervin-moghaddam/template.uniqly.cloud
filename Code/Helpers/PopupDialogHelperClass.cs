using template.Models;

namespace template.Code.Helpers;

public enum PopupDialogTypes
{
    Default = 0,
    Confirmation = 1,
    Error = 2,
    Warning = 3,
    Question = 4
}

public class PopupDialogHelperClass
{
    /// <summary>
    /// Creates and returns a PopupDialogModel with pre-defined options
    /// when given an identifier, and optionally an already existing model to add to.
    /// </summary>
    /// <param name="Identifier"></param>
    /// <returns></returns>
    public static PopupDialogModel? CreateDialogOptions(int DialogNo)
    {
        PopupDialogModel m;

        return DialogNo switch
        {
            100 => new PopupDialogModel // Common - On close - Changes not saved
            {
                DialogType = PopupDialogModel.DialogTypeEnum.Question, //(int)PopupDialogTypes.Question,
                ShowCloseButton = true,
                ElementID = $"dlg_{DialogNo}",
                DialogMaxWidth = 450,
                TitleTextLangKey = "Common_DialogTitle_OnClose_ChangesNotSaved",
                ContentTextLangKey = "Common_DialogText_OnClose_ChangesNotSaved",
                ButtonList = new List<PopupDialogModel.PopupDialogButton>
                {
                    CreateDialogButton("yes"), CreateDialogButton("cancel")
                }
            },
            1001 => new PopupDialogModel // PLE Changes not saved
            {
                DialogType = PopupDialogModel.DialogTypeEnum.Question ,
                ShowCloseButton = true,
                ElementID = $"pledlg_{DialogNo}",
                DialogMaxWidth = 450,
                TitleTextLangKey = "POSLayoutEditor_DialogTitle_ChangesNotSaved",
                ContentTextLangKey = "POSLayoutEditor_DialogText_ChangesNotSaved",
                ButtonList = new List<PopupDialogModel.PopupDialogButton>
                {
                    CreateDialogButton("yes"), CreateDialogButton("cancel")
                }
            },
            1009 => new PopupDialogModel // Auto-align items in AdvGrid (compact())
            {
                DialogType = PopupDialogModel.DialogTypeEnum.Question,
                ShowCloseButton = true,
                ElementID = $"pledlg_{DialogNo}",
                DialogMaxWidth = 450,
                TitleTextLangKey = "POSLayoutEditor_DialogTitle_AutoAlign",
                ContentTextLangKey = "POSLayoutEditor_DialogText_ChangesNotSaved",
                ButtonList = new List<PopupDialogModel.PopupDialogButton>
                {
                    CreateDialogButton("yes"), CreateDialogButton("no")
                }
            },
            1010 => new PopupDialogModel // Delete page
            {
                DialogType = PopupDialogModel.DialogTypeEnum.Warning,
                ShowCloseButton = true,
                ElementID = $"pledlg_{DialogNo}",
                DialogMaxWidth = 450,
                TitleTextLangKey = "POSLayoutEditor_DialogTitle_DeletePageQuestion",
                ContentTextLangKey = "POSLayoutEditor_DialogText_DeletePageQuestion",
                ButtonList = new List<PopupDialogModel.PopupDialogButton>
                {
                    CreateDialogButton("yes"), CreateDialogButton("no")
                }
            },
            1050 => new PopupDialogModel // Delete variant
            {
                DialogType = PopupDialogModel.DialogTypeEnum.Warning,
                ShowCloseButton = true,
                ElementID = $"varedlg_{DialogNo}",
                DialogMaxWidth = 450,
                TitleTextLangKey = "VariantEditor_DialogTitle_DeleteVariantConfirmation",
                ContentTextLangKey = "VariantEditor_DialogText_DeleteVariantConfirmation",
                ButtonList = new List<PopupDialogModel.PopupDialogButton>
                {
                    CreateDialogButton("yes"), CreateDialogButton("no")
                }
            },
            _ => null // Returning null by default (but shouldn't happen)
        };
    }

    /// <summary>
    /// For just creating simple ok,cancel,yes,no buttons
    /// </summary>
    /// <param name="Identifier"></param>
    /// <returns></returns>
    private static PopupDialogModel.PopupDialogButton CreateDialogButton(string? Identifier)
    {
        if (string.IsNullOrEmpty(Identifier)) return null!;
        
        return Identifier.ToLower() switch
        {
            "ok" => new PopupDialogModel.PopupDialogButton
            {
                ColorClass = "green", TextLangKey = "Button_OK", ElementID = "dialog_ok"
            },
            "cancel" => new PopupDialogModel.PopupDialogButton
            {
                ColorClass = "red", TextLangKey = "Button_Cancel", ElementID = "dialog_cancel"
            },
            "yes" => new PopupDialogModel.PopupDialogButton
            {
                ColorClass = "green", TextLangKey = "Button_Yes", ElementID = "dialog_ok"
            },
            "no" => new PopupDialogModel.PopupDialogButton
            {
                ColorClass = "red", TextLangKey = "Button_No", ElementID = "dialog_no"
            }
        };
    }
}