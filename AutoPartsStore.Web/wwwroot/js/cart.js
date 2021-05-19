function addCommas(nStr) {
    nStr += '';
    var rgx = /(\d+)(\d{3})/;
    while (rgx.test(nStr)) {
        nStr = nStr.replace(rgx, '$1' + ',' + '$2');
    }
    return nStr;
}
function addToCart(id,price) {
    fetch('/cart/add/' + id)
        .then(response => response.json())
        .then(data => {
            if (data.success === true) {
                const countElem = $('#cart-count');
                const valueElem = $('#cart-value b');
                const value = Number(valueElem.html().replaceAll(',', ''));
                const count = Number(countElem.html());
                countElem.html(count + 1);
                let newValue = value + price;
                valueElem.html(addCommas(newValue));
            }
            else
                alert('مشکلی در افزودن محصول به سبد خرید پیش اومد');
        }).catch(err => console.log(err));
}