function onlyNumber(evt) {
    let charCode = (evt.which) ? evt.which : evt.keyCode;
    if (charCode > 47 && charCode < 58)
        return true;
    else evt.preventDefault();
}

export { onlyNumber }