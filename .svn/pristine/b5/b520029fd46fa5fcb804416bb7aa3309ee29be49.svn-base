var aprueba = "";

$(document).ready(function () {
    CierreCajaModal.Init();
    $('#BotonApruebaCierre').click(function () {
        aprueba = "Y";
    });
    $('#BotonRechazaCierre').click(function () {
        aprueba = "N";
    });
});

var CierreCajaModal = {
    Init: function () {
        $('#modalCierreCaja').on('hidden.bs.modal', function (e) {
            CierreCajaModal.LimpiarFormulario();
        })
    },
    GuardarFormulario: function () {
        Mensaje.Confirmar("¿Confirma el envío de la información?", CierreCajaModal.CerrarCaja);
    },
    CerrarCaja: function () {
        var postData = { asignacion: $('#asignacionSeleccionada').val(), apruebaAsignacion: aprueba };
        $.ajax({
            url: '/Caja/ProcesoCierreCaja',
            type: 'GET',
            dataType: 'json',
            cache: false,
            data: postData,
            contentType: 'application/json; charset=utf-8',
            success: function (data) {
                var mensaje = data.Mensaje;
                if (data.EsError) {
                    Mensaje.Alerta(mensaje);
                }
                else {
                    $('#modalCierreCaja').modal("hide");
                    Mensaje.Correcto(mensaje, ReloadCajas());
                }
            }
        });
    },
    LimpiarFormulario: function () {
        $("#tituloModalCierreCaja").val("");
        $('#NombreCaja').val("");
        $('#UsuarioCaja').val("");
        $('#FechaInicioCaja').val("");
        $('#FechaCierreCaja').val("");
        $('#MontoInicioCaja').val("");
        $('#MontoCierreCaja').val("");
        $('#MontoDiferenciaCaja').val("");
        $('#ObservacionDiferenciaCaja').val("");
    },
    AbrirFormulario: function (item) {
        CierreCajaModal.LimpiarFormulario();

        $("#tituloModalCierreCaja").innerText = "Cierre Caja";
        $('#asignacionSeleccionada').val(item.dataset.idasignacion);
        $('#NombreCaja').val(item.dataset.cajaasignada);
        $('#UsuarioCaja').val(item.dataset.usuarioasignado);
        $('#FechaInicioCaja').val(item.dataset.fechainicioasignaciontexto);
        $('#FechaCierreCaja').val(item.dataset.fechacierreasignaciontexto);
        $('#MontoInicioCaja').val(item.dataset.montofijoasignacion);
        $('#MontoCierreCaja').val(item.dataset.montocierre);
        $('#MontoDiferenciaCaja').val(item.dataset.montodiferencia);
        $('#ObservacionDiferenciaCaja').val(item.dataset.observaciondiferencia);

        $('#modalCierreCaja').modal("show");
    }
}