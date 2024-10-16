using BlazorBuilds.Components.Common.Extensions;
using BlazorBuilds.Components.Common.Seeds;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace BlazorBuilds.Components.Accordion;

public partial class Accordion
{
    [Parameter] public RenderFragment?   AccordionItems { get; set; }
    [Parameter] public HeadingLevelValue HeadingSize    { get; set; } = HeadingLevelValue.h3;
    [Parameter] public ExpandMode        ExpandMode     { get; set; } = ExpandMode.Multiple;

    [Parameter] public EventCallback<ItemHeadingClickedData> OnAccordionItemHeadingClicked { get; set; }

    private readonly List<AccordionItem> _accordionItems = [];
    private int                 _focusIndex     = 0;

    internal void AddAccordionItem(AccordionItem accordionItem)

         => _accordionItems.Contains(accordionItem).WhenFalse(() => _accordionItems.Add(accordionItem));
   
    internal void RemoveAccordionItem(AccordionItem accordionItem)
    
        => _accordionItems.Contains(accordionItem).WhenTrue(() => _accordionItems.Remove(accordionItem));

    private async ValueTask SetItemFocus(int itemIndex)

        => await _accordionItems[itemIndex].SetFocus();

    internal void UpdateFocusIndex(Guid accordionItemID)
    {
        var accordionItem = _accordionItems.Where(a => a.AccordionItemID == accordionItemID).SingleOrDefault();

        (accordionItem is not null).WhenTrue(() => _focusIndex = _accordionItems.IndexOf(accordionItem!));
    }

    private async Task CheckSetStartState(ExpandMode expandMode)
    {
        if (expandMode != ExpandMode.Single) return;

        int openCount = 0;

        foreach (var accordionItem in _accordionItems)
        {
            openCount = accordionItem.Expanded ? openCount + 1 : openCount;

            await (openCount > 1 && true == accordionItem.Expanded).WhenTrue(() => accordionItem.ToggleExpandedState());
        }
    }
    protected override async Task OnAfterRenderAsync(bool firstRender)

        => await firstRender.WhenTrue(() => CheckSetStartState(ExpandMode));
 
    public async Task CollapseAllPanels(bool keepFocus = true)
    {
        foreach(var accordionItem in _accordionItems) await accordionItem.Expanded.WhenTrue(() => accordionItem.ToggleExpandedState());

        await (_focusIndex >= 0 && true == keepFocus).WhenTrue(() => SetItemFocus(_focusIndex));
    }

    public async Task ExpandAllPanels(bool keepFocus = true)
    {
        foreach (var accordionItem in _accordionItems) await accordionItem.Expanded.WhenFalse(() => accordionItem.ToggleExpandedState());

        await (_focusIndex >= 0 && true == keepFocus).WhenTrue(() => SetItemFocus(_focusIndex));
    }

    public async Task ExpandPanelWithFocus(int panelIndex)
    {
        if (panelIndex < 0 && panelIndex >= _accordionItems.Count) return;

        if (ExpandMode == ExpandMode.Single) await CollapseAllPanels(false);

        var accordionItem = _accordionItems[panelIndex];

        await accordionItem.Expanded.WhenFalse(() => accordionItem.ToggleExpandedState());
        await SetItemFocus(panelIndex);

    }
    
    private async Task RaiseOnAccordionItemHeadingClicked(int itemIndex, string headingText, bool isExpanded)

        => await (OnAccordionItemHeadingClicked.HasDelegate).WhenTrue(() => OnAccordionItemHeadingClicked.InvokeAsync(new(itemIndex, headingText, isExpanded)));

    internal async Task NotifyHeadingClicked(Guid accordionItemID)
    {
        var accordionItem = _accordionItems.Where(a => a.AccordionItemID == accordionItemID).SingleOrDefault();

        if (accordionItem is null) return;

        var itemWasExpanded = accordionItem.Expanded;

        _focusIndex =  _accordionItems.IndexOf(accordionItem);

        if (ExpandMode == ExpandMode.Single) await CollapseAllPanels(false);

        if ((false == itemWasExpanded && ExpandMode == ExpandMode.Single) || (ExpandMode == ExpandMode.Multiple)) await accordionItem.ToggleExpandedState();

        await RaiseOnAccordionItemHeadingClicked(_focusIndex, accordionItem.HeadingText, accordionItem.Expanded);
    }

    private async Task HandleHomeEndKeys(bool isHome)
    {
        var focusIndex = isHome ? 0 : _accordionItems.Count -1;

        await SetItemFocus(focusIndex);
    }
    private async Task HandleUpDownArrowKeys(Direction direction, int focusIndex)
    {
        int maxIndex = _accordionItems.Count -1;

        focusIndex = direction switch
        {
            Direction.Down when focusIndex < maxIndex => focusIndex + 1,
            Direction.Down when focusIndex >= maxIndex => 0,
            Direction.Up when focusIndex > 0 => focusIndex - 1,
            Direction.Up when focusIndex <= 0 => maxIndex,
            _ => focusIndex
        };

        await SetItemFocus(focusIndex);
    }

    private async Task HandleKeyPress(KeyboardEventArgs keyArgs)
    {
        var keyboardTask = keyArgs.Key switch
        {
            GlobalStrings.KeyBoard_Down_Arrow_Key => HandleUpDownArrowKeys(Direction.Down, _focusIndex),
            GlobalStrings.KeyBoard_Up_Arrow_Key => HandleUpDownArrowKeys(Direction.Up, _focusIndex),
            GlobalStrings.KeyBoard_Home_Key => HandleHomeEndKeys(true),
            GlobalStrings.KeyBoard_End_Key => HandleHomeEndKeys(false),
            _ => Task.CompletedTask
        };
        await keyboardTask;
    }
}
