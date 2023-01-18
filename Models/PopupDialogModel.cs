namespace template.Models;
public class PopupDialogModel
    {
        // Setup
        public int DialogType { get; set; } // 0: Default, 1: Confirmation, 2: Error, 3: Warning, 4: Question 
        public string TitleCustomText { get; set; }
        public string TitleTextLangKey { get; set; }
        public bool ShowCloseButton { get; set; }
        public string ElementID { get; set; } // Used to identify, for instance after onclick(this)
        public string PopupElementID { get; set; } // Used to identify the popup element contenting this dialog

        // Events
        public string OnClickFunctionName { get; set; } // Calls the function with the parameter "this" 

        // Style
        // All sizes <= 100 is percent, > 100 is pixels
        public int DialogMaxWidth { get; set; } // Default is 80%
        public int ContentMaxHeight { get; set; } // Default is auto

        // Custom HTML content
        // If HTML is entered, everything else will be ignored
        public string HTMLContent { get; set; }

        // Text-only function (only show a text as the dialog content)
        public string CustomText { get; set; } // Ignores LangText if has value 
        public string ContentTextLangKey { get; set; } // Get multilingual text from DialogResource

        // Bottom bar
        public bool ShowBottomBar { get; set; }
        public List<PopupDialogButton> ButtonList { get; set; }

        public class PopupDialogButton
        {
            public string ElementID { get; set; }
            public List<string> ElementClassList { get; set; } // Manually define classes
            public string ColorClass { get; set; } // Green, yellow, grey, red
            public string FontAwesomeClass { get; set; } // For icons
            public string TooltipText { get; set; } // When user hovers over the button
            public bool AlignRight { get; set; } // Align the button to the right side

            // Text or content inside dialog box
            public string CustomText { get; set; } // Ignores LangText if has value 
            public string TextLangKey { get; set; } // Resource, Key
            public string HTMLContent { get; set; } // If HTML is entered, everything else will be ignored
        }

        public class JsonResponseModel
        {
            public string dlg { get; set; }
            public string btnLst { get; set; }
            public int dialogNo { get; set; }
        }
    }