$(document).ready(function () {
    $('.panelCaja').on('hide.bs.collapse', function (e) {
        $("#icono_" + e.currentTarget.id).removeClass("glyphicon-menu-up");
        $("#icono_" + e.currentTarget.id).addClass("glyphicon-menu-down");
    });
    $('.panelCaja').on('show.bs.collapse', function (e) {
        $("#icono_" + e.currentTarget.id).removeClass("glyphicon-menu-down");
        $("#icono_" + e.currentTarget.id).addClass("glyphicon-menu-up");
    });


    $(document).on("click", "#btnRechazar", function () {
        $("#idAsignacionRechazo").val(this.dataset.asignacion);
        $("#txtNombreCaja").val(this.dataset.nombrecaja);
        $("#txtFechaAsignacion").val(this.dataset.fechaasignacion);
    });
    $('#modalMotivoRechazo').on('hide.bs.modal', function (e) {
        $("#idAsignacionRechazo").val("");
        $("#txtMotivoRechazo").val("");
        $("#txtNombreCaja").val("");
        $("#txtFechaAsignacion").val("");
    });
});
function InicioCajaContinuar(idAsignacion) {
    var transbank = $('#NumMaqTran_' + idAsignacion)[0];
    if (transbank.value != transbank.dataset.inicio) {
        bootbox.confirm({
            message: '<span class="glyphicon glyphicon-question-sign" style="color: blue"></span>&nbsp;&nbsp;Número de máquina transbank a sido modificado.<br/>¿Desea guardar este cambio y continuar?',
            buttons: {
                confirm: { label: 'Aceptar', className: 'btn-primary' },
                cancel: { label: 'Cancelar' }
            },
            callback: function (result) {
                if (result) {
                    ModificarAsignacionCaja(idAsignacion);
                }
            }
        });
    }
    else {
        RedireccionarPuntoVenta(idAsignacion);
    }
}
function InicioConfirmacion(idAsignacion, ipCaja) {
    bootbox.confirm({
        message: '<span class="glyphicon glyphicon-question-sign" style="color: blue"></span>&nbsp;&nbsp;Al iniciar esta caja, no podra utilizar otra hasta realizar el cierre correspondiente.<br/>¿Desea continuar?',
        buttons: {
            confirm: {
                label: 'Aceptar',
                className: 'btn-primary'
            },
            cancel: {
                label: 'Cancelar'
            }
        },
        callback: function (result) {
            if (result) {
                IniciarAsignacionCaja(idAsignacion, ipCaja);
            }
        }
    });
}
function ModificarAsignacionCaja(idAsignacion) {
    $.ajax({
        url: '/Caja/ModificarAsignacionCaja',
        type: 'GET',
        data: { asignacion: idAsignacion, numeroTransbank: $('#NumMaqTran_' + idAsignacion).val() },
        cache: false,
        contentType: 'application/json; charset=utf-8',
        success: function (respuesta) {
            if (respuesta.EsError) {
                Mensaje.Alerta(respuesta.Mensaje);
            }
            else {
                bootbox.alert({
                    message: '<span class="glyphicon glyphicon-ok-sign" style="color: green"></span>&nbsp;&nbsp;' + respuesta.Mensaje,
                    callback: function () {
                        RedireccionarPuntoVenta(idAsignacion);
                    }
                });
            }
        }
    });
}
function IniciarAsignacionCaja(idAsignacion, ipCaja) {
    $.ajax({
        url: '/Caja/IniciarAsignacionCaja',
        type: 'GET',
        data: {
            asignacion: idAsignacion,
            numeroTransbank: $('#NumMaqTran_' + idAsignacion).val(),
            cajaip: ipCaja
        },
        cache: false,
        contentType: 'application/json; charset=utf-8',
        success: function (respuesta) {
            if (respuesta.EsError) {
                Mensaje.Alerta(respuesta.Mensaje);
            }
            else {
                bootbox.alert({
                    message: '<span class="glyphicon glyphicon-ok-sign" style="color: green"></span>&nbsp;&nbsp;' + respuesta.Mensaje,
                    callback: function () {
                        RedireccionarPuntoVenta(idAsignacion);
                    }
                });
            }
        }
    });
}
function RedireccionarPuntoVenta(idAsignacion) {
    mLoading.modal();
    window.location.href = "/PuntoVenta/PuntoDeVenta?asignacion=" + idAsignacion;
}
function RechazoConfirmacion() {
    if ($("#txtMotivoRechazo").val() == "") {
        Mensaje.Alerta("Debe ingresar una observación por la cual desea rechazar la asignación seleccionada.");
    }
    else {
        Mensaje.Confirmar("¿Esta seguro que desea rechazar la caja seleccionada?", RechazarAsignacionCaja);
    }
}
function RechazarAsignacionCaja() {
    $.ajax({
        url: '/Caja/RechazarAsignacionCaja',
        type: 'GET',
        data: { asignacion: $("#idAsignacionRechazo").val(), observacion: $('#txtMotivoRechazo').val() },
        cache: false,
        contentType: 'application/json; charset=utf-8',
        success: function (respuesta) {
            if (respuesta.EsError) {
                Mensaje.Alerta(respuesta.Mensaje);
            }
            else {
                Mensaje.CorrectoF(respuesta.Mensaje, RecargarInicio);
            }
        }
    });
}
function RecargarInicio() {
    mLoading.modal();
    location.reload();
}

