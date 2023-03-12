function randomizeString(length = 10, specialChars = false) {
    let result = '';
    let characters = 'ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789';
    if (specialChars) characters += '!@#$%&*()_+{}|<>[]';
    const charactersLength = characters.length;
    let counter = 0;
    while (counter < length) {
        result += characters.charAt(Math.floor(Math.random() * charactersLength));
        counter += 1;
    }
    return result;
}


function changeTheme() {
    let chkbox = document.getElementById('color-theme-changer');
    if(chkbox.checked) {
        trans()
        document.documentElement.setAttribute('color-theme', 'dark')
    } else {
        trans()
        document.documentElement.setAttribute('color-theme', 'light')
    }
}