$("#login").click(function () {
    //orangeForm - pass
    var usuario = $("#Usuario").val();
    var pass = $("#Contrasenia").val();
    login.Usuario = usuario;
    login.Contrasenia = pass;
    ShowLoader();
    $.ajax({
        url: "api/Sesion/",
        type: "POST",
        data: JSON.stringify(login),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            localStorage.setItem('ACCESS_TOKEN', "Bearer " + data);
            toastr["success"]("Se ha autenticado correctamente!");
            HideLoader();
            $(location).attr('href', "/Home/Index");
            $.cookie('token', data);
        },
        fail: function () {
            toastr["error"]("El usuario o la contraseña son incorrectos, vuelva a intentar nuevamente!");
            HideLoader();
        },
        error: function () {
            toastr["error"]("El usuario o la contraseña son incorrectos, vuelva a intentar nuevamente!");
            HideLoader();
        }
    })
})
var login = {
    Usuario: "John",
    Contrasenia: "Doe",
};