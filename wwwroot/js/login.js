function toggleShowPassword(el) {
    const $passinput = $(el).closest('div').find(':input');
    const type = $($passinput).attr('type') === 'password' ? 'text' : 'password';
    $passinput[0].type = type;
    if ($(el).hasClass('fa-eye-slash'))
        $(el).addClass('fa-eye');
    else
        $(el).addClass('fa-eye-slash');
};