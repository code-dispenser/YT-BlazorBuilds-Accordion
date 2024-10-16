namespace BlazorBuilds.Components.Accordion;

public class ItemHeadingClickedData(int itemIndex, string headingText, bool isExpanded)
{
    public int ItemIndex      { get; set; } = itemIndex;
    public string HeadingText { get; set; } = String.IsNullOrWhiteSpace(headingText) ? String.Empty : headingText;
    public bool IsExpanded    { get; set; } = isExpanded;
}
