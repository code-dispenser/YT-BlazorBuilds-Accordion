namespace BlazorBuilds.Components.Accordion;

public enum HeadingLevelValue : int { h1, h2, h3, h4, h5, h6 }
public enum ExpandMode : int { Single, Multiple }
internal enum StyleFor : int
{
    Accordion,
    AccordionHeading,
    AccordionHeadingText,
    AccordionHeadingIcon,
    AccordionPanel,
    AccordionButton,
    AccordionButtonContent,
    AccordionButtonIcon,
    ChevronIcon
}
internal enum Direction : int { Up, Down }
