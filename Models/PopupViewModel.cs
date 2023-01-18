namespace template.Models;

public class PopupViewModel
{
    public string WidthStyle { get; set; }
    public string HeightStyle { get; set; }
    public int ZIndex { get; set; }
    public string ElementID { get; set; }
    public string Identifier { get; set; }
    public string sourceElementIdentifier { get; set; }
    public bool EmptyFrame { get; set; } = false; // Set to true if the popup frame should be empty
}