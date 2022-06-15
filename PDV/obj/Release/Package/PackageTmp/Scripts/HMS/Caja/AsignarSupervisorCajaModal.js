$(document).ready(function () {
    AsignarSupervisorModal.Init();
});

var AsignarSupervisorModal = {
    Init: function () {
        $('#modalAsignaSupervisor').on('hidden.bs.modal', function (e) {
            $("#UsuarioSupervisor").val("");
        })
    },
    GuardarFormulario: function () {
        if ($("#cajasSeleccionadas").val().length == 0) {
            Mensaje.Alerta('No se ha seleccionado ninguna caja para asignar.');
        }
        else if ($("#UsuarioSupervisor").val() === "") {
            Mensaje.Alerta('Debe seleccionar un usuario supervisor.');
        }
        else {
            Mensaje.Confirmar("¿Está seguro de asignar el supervisor a las cajas seleccionadas?", AsignarSupervisorModal.GuardarMultiplesCajas);
        }
    },
    GuardarMultiplesCajas: function () {
        var listCajas = $("#cajasSeleccionadas").val().split(";");

        var postData = {
            "lstCajasId": listCajas,
            "idSupervisorCaja": $("#UsuarioSupervisor").val(),
            "supervisorCaja": $("#UsuarioSupervisor")[0].options[$("#UsuarioSupervisor")[0].selectedIndex].text.toUpperCase()
        };
        $.ajax({
            url: '/Caja/AsignarSupervisorMasivoModal',
            type: 'POST',
            dataType: 'json',
            cache: false,
            data: JSON.stringify(postData),
            contentType: 'application/json; charset=utf-8',
            success: function (data) {
                var mensaje = data.Mensaje;
                if (data.EsError) {
                    Mensaje.Alerta(mensaje);
                }
                else {
                    $("#cajasSeleccionadas").val("");

                    $("#modalAsignaSupervisor").modal("hide");
                    Mensaje.CorrectoF(mensaje, ReloadCajas);
                }
            }
        });
    }
}