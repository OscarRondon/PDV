$(document).ready(function () {
    NuevaGarantia.Init();
});

var NuevaGarantia = {
    Init: function () {
        $('#ModalNuevaGarantia').on('hidden.bs.modal', function (e) {
            NuevaGarantia.LimpiarFormulario();
        });
    },
    AbrirFormulario: function (montoPago) {
        $('#txtGarantiaMonto').val(montoPago);
        PDV.SoloNumeroFormato($('#txtGarantiaMonto')[0]);

        $("#txtGarantiaResponsableRut").val($('#TxtRutResponsableDetalleVenta').val());
        $("#txtGarantiaResponsableNombre").val($('#TxtNombreResponsableDetalleVenta').val());
        $("#txtGarantiaResponsableDireccion").val($('#hdnPacienteDireccion').val());
        $("#txtGarantiaResponsableComuna").val($('#hdnPacienteComuna').val());

        $("#txtGarantiaPacienteRut").val($('#TxtRutPacienteDetalleVenta').val());
        $("#txtGarantiaPacienteNombre").val($('#TxtNombrePacienteDetalleVenta').val());
        $("#txtGarantiaPacienteDireccion").val($('#hdnPacienteDireccion').val());
        $("#txtGarantiaPacienteComuna").val($('#hdnPacienteComuna').val());

        $('#ModalNuevaGarantia').modal("show");
    },
    MostrarOcultarCheque: function (medioPago) {
        if (medioPago == '1') {
            $('#divGarantiaCheque').show();
        } else {
            $('#divGarantiaCheque').hide();

            $('#txtGarantiaNumeroCheque').val("");
            $('#ddlGarantiaBancos').val("");
        }
    },
    LimpiarFormulario: function () {
        $('#txtGarantiaMonto').val("");
        $('#txtGarantiaNumeroCheque').val("");

        $('#ddlGarantiaMedioPago').val("");
        $('#ddlGarantiaMedioPago')[0].onchange();
        $('#ddlGarantiaBancos').val("");

        $("#txtGarantiaResponsableRut").val("");
        $("#txtGarantiaResponsableNombre").val("");
        $("#txtGarantiaResponsableDireccion").val("");
        $("#txtGarantiaResponsableComuna").val("");

        $("#txtGarantiaPacienteRut").val("");
        $("#txtGarantiaPacienteNombre").val("");
        $("#txtGarantiaPacienteDireccion").val("");
        $("#txtGarantiaPacienteComuna").val("");
    },
    LimpiarFormularioResponsable: function () {
        $('#ddlGarantiaMedioPago').val("");
        $('#ddlGarantiaMedioPago')[0].onchange();
        $('#ddlGarantiaBancos').val("");

        $("#txtGarantiaResponsableRut").val("");
        $("#txtGarantiaResponsableNombre").val("");
        $("#txtGarantiaResponsableDireccion").val("");
        $("#txtGarantiaResponsableComuna").val("");
    },
    GuardarFormulario: function () {
        if (NuevaGarantia.ValidarFormulario()) {
            Mensaje.Confirmar("¿Esta seguro de guardar la garantía?", NuevaGarantia.IngresarGarantia);
        }
    },
    ValidarFormulario: function () {
        var textoInicio = "<dt>Debe revisar los siguientes datos:</dt>";
        var esError = false;

        if ($('#txtGarantiaResponsableRut').val() == "" || $('#txtGarantiaResponsableRut').val().trim() == "") {
            textoInicio = textoInicio + '<dd> &#8226;  Rut responsable.</dd>';
            esError = true;
        }
        if ($('#txtGarantiaResponsableNombre').val() == "" || $('#txtGarantiaResponsableNombre').val().trim() == "") {
            textoInicio = textoInicio + '<dd> &#8226;  Nombre responsable.</dd>';
            esError = true;
        }
        if ($('#txtGarantiaResponsableComuna').val() == "" || $('#txtGarantiaResponsableComuna').val().trim() == "") {
            textoInicio = textoInicio + '<dd> &#8226;  Comuna responsable.</dd>';
            esError = true;
        }
        if ($('#txtGarantiaResponsableDireccion').val() == "" || $('#txtGarantiaResponsableDireccion').val().trim() == "") {
            textoInicio = textoInicio + '<dd> &#8226;  Dirección responsable.</dd>';
            esError = true;
        }

        if ($('#txtGarantiaMonto').val() == "" || $('#txtGarantiaMonto').val().trim() == "") {
            textoInicio = textoInicio + '<dd> &#8226;  Monto a cancelar.</dd>';
            esError = true;
        }
        var tipoMedioPago = $('#ddlGarantiaMedioPago').val();
        if (tipoMedioPago != "") {
            if (tipoMedioPago == "1") {
                if ($('#ddlGarantiaBancos').val() == "") {
                    textoInicio = textoInicio + '<dd> &#8226;  Banco emisor.</dd>';
                    esError = true;
                }

                if ($('#txtGarantiaNumeroCheque').val() == "" || $('#txtGarantiaNumeroCheque').val().trim() == "") {
                    textoInicio = textoInicio + '<dd> &#8226;  Número de cheque.</dd>';
                    esError = true;
                }
            }
        }
        else {
            textoInicio = textoInicio + '<dd> &#8226;  Medio de Pago.</dd>';
            esError = true;
        }

        if (esError) {
            bootbox.alert("<dl>" + textoInicio + "</dl>");
        } else {
            return true;
        }
    },
    IngresarGarantia: function () {
        var montoPago = $('#txtGarantiaMonto').val();
        while (montoPago.includes(".") || montoPago.includes(",")) {
            montoPago = montoPago.replace(".", "");
            montoPago = montoPago.replace(",", "");
        }
        var montoFinal = parseInt(montoPago);

        var jsonAntGar = {
            GarIdAsignacion: $('#idAsignacion').val(),
            GarFichaClinica: $('#TxtFichaClinicaDetalleVenta').val(),
            GarEpisodio: $('#TxtEpisodioDetalleVenta').val(),
            GarIndicador: parseInt($('#ddlGarantiaMedioPago').val()),
            GarMonto: montoFinal,

            GarEntidad: $('#ddlGarantiaBancos').val(),
            GarNumeroCheque: $('#txtGarantiaNumeroCheque').val(),

            IdTipoDocumento: "G",

            GarRutResponsable: $('#txtGarantiaResponsableRut').val(),
            NombreResponsable: $('#txtGarantiaResponsableNombre').val(),
            DireccionResponsable: $('#txtGarantiaResponsableDireccion').val(),
            ComunaResponsable: $('#txtGarantiaResponsableComuna').val(),

            GarRutPaciente: $('#txtGarantiaPacienteRut').val(),
            NombrePaciente: $('#txtGarantiaPacienteNombre').val(),
            DireccionPaciente: $('#txtGarantiaPacienteDireccion').val(),
            ComunaPaciente: $('#txtGarantiaPacienteComuna').val(),

            IdTrack: $('#PagoSeleccionadoTrack').val()
        };

        var postData = { antGar: jsonAntGar };
        $.ajax({
            url: '/PuntoVenta/IngresarGarantia',
            type: 'POST',
            cache: false,
            data: JSON.stringify(postData),
            contentType: 'application/json; charset=utf-8',
            success: function (result) {
                var mensaje = result.Mensaje;
                if (result.EsError) {
                    Mensaje.Alerta(mensaje);
                }
                else {

                    VisorReporte("ReciboGarantia.rpt", result.DocEntry, "DocEntry");

                    NuevaGarantia.LimpiarFormulario();
                    $('#ModalNuevaGarantia').modal("hide");

                    PendientePago.LimpiarFormularioPendiente();

                    Mensaje.Correcto(mensaje);
                }
            },
        });
    },

}