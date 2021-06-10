function addCommas(nStr) {
    nStr += '';
    var rgx = /(\d+)(\d{3})/;
    while (rgx.test(nStr)) {
        nStr = nStr.replace(rgx, '$1' + ',' + '$2');
    }
    return nStr;
}
function addToCart(id, price) {
    if (isLogin === "true") {
        fetch('/cart/add/' + id)
            .then(response => response.json())
            .then(data => {
                if (data.success === true) {
                    const valueElem = $('#cart-value b');
                    const value = Number(valueElem.html().replaceAll(',', ''));
                    if (data.firstTimeAdd === true) {
                        const countElem = $('#cart-count');
                        const count = Number(countElem.html());
                        countElem.html(count + 1);
                    }
                    if (typeof price === "string")
                        price = Number(price.replaceAll(',', ''));
                    let newValue = value + price;
                    valueElem.html(addCommas(newValue));
                }
                else
                    alert('مشکلی در افزودن محصول به سبد خرید پیش اومد');
            }).catch(err => console.log(err));
    }
    else
        location.replace('/login');
}