function HighlightMenuItemAndRemoveLink(name) {
    window.LastHighlightedMenuItem = name;
    $("#menu-" + name).css("font-weight", "bold");
    $("#menu-" + name).attr("href", "#");
}