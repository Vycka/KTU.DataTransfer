window.SelectedItemId = -1;

function ItemClick(itemId) {
    window.SelectedItemId = itemId;
    $('[id^=itemlist-name-]').css('font-weight', '');
    $('#itemlist-name-' + itemId).css('font-weight', 'bold');

    $('#selected-item-controls').css('display', '');

    if (window.ItemClickCallBack != null) {
        window.ItemClickCallBack(itemId);
    }
}