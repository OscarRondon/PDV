$(document).ready(function () {
    DetallesGarantia.Init();
    $('#divCheque').hide();
    $('#btnPagare').hide();

});
var DetallesGarantia = {
    Init: function () {
        $('#modalDetallesGarantia').on('hidden.bs.modal', function (e) {
            DetallesGarantia.LimpiarFormulario();
        })
    },
    AbrirFormulario: function (item) {
        DetallesGarantia.LimpiarFormulario();

        var texto = $('#ta_' + item.dataset.docentry + '_' + item.dataset.docnum).val();
        var objeto = JSON.parse(texto);

        DetallesGarantia.CargarDatosGarantia(objeto);
        $('#idDoc').val(item.dataset.docentry);


        $('#modalDetallesGarantia').modal("show");
    },

    CargarDatosGarantia: function (garantia) {
        $('#tituloModalDetallesGarantia').html("Detalles Garantía Nº " + garantia.GarDocNum);

        $('#inputDoc').val(garantia.TipoDocumentoTexto.toUpperCase());

        var tipoDoc = $('#inputDoc').val();
        var respaldo = garantia.GarIndicadorTexto;

        if (garantia.GarEstado != "3" && garantia.GarEstado != "2") {
            if (garantia.DiasVencimiento != null && garantia.DiasVencimiento < 0) {
                $('#btnAplicar').show();
                $("#Comentario").prop("disabled", false);
                $("#btnGuardar").show();
            }

            if (garantia.GarEstado == "1") {
                if (tipoDoc != "PAGARÉ") {
                    $('#btnDevolucion').show();
                }
            }
        }

        $('#Monto').val(garantia.GarMontoTexto);
        $('#respaldoDoc').val(respaldo);

        if (tipoDoc == "GARANTÍA") {
            $('#divRespaldo').show();

            if (respaldo == "CHEQUE") {
                $('#divCheque').show();
                $('#Banco').val(garantia.GarEntidadDescripcion.toUpperCase());
                $('#GarNumeroCheque').val(garantia.GarNumeroCheque);
            }
        }

        $('#GarRutResponsable').val(garantia.GarRutResponsable);
        $('#PasResponsable').val(garantia.PasResponsable);
        $('#NombreResponsable').val(garantia.NombreResponsable);
        $('#DireccionResponsable').val(garantia.DireccionResponsable);
        $('#ComunaResponsable').val(garantia.ComunaResponsable);
        $('#FichaClinica').val(garantia.GarFichaClinica);
        $('#GarEpisodio').val(garantia.GarEpisodio);
        $('#GarRutPaciente').val(garantia.GarRutPaciente);
        $('#PasPaciente').val(garantia.PasPaciente);
        $('#NombrePaciente').val(garantia.NombrePaciente);
        $('#DireccionPaciente').val(garantia.DireccionPaciente);
        $('#ComunaPaciente').val(garantia.ComunaPaciente);
        $('#Comentario').val(garantia.Comentario);
    },
    DevolverGarantia: function () {
        var docentry = $("#idDoc").val();
        $.ajax({
            url: '/Garantia/DevolverGarantia',
            type: 'GET',
            cache: false,
            data: { DocEntry: docentry, estado: "2" }, // Devolucion
            contentType: 'application/json; charset=utf-8',
            success: function (data) {
                if (data.resultGarantia.EsError) {
                    Mensaje.Alerta(data.resultGarantia.Mensaje);
                }
                else {
                    $('#modalDetallesGarantia').modal("hide");
                    Mensaje.Correcto(data.resultGarantia.Mensaje);

                    Anticipo.LimpiarGarantia();
                }
            }
        });
    },
    AplicarGarantia: function () {
        var docentry = $("#idDoc").val();
        $.ajax({
            url: '/Garantia/DevolverGarantia',
            type: 'GET',
            cache: false,
            data: { DocEntry: docentry, estado: "3" }, // Aplicada
            contentType: 'application/json; charset=utf-8',
            success: function (data) {
                if (data.resultGarantia.EsError) {
                    Mensaje.Alerta(data.resultGarantia.Mensaje);
                } else {
                    $('#modalDetallesGarantia').modal("hide");
                    Mensaje.Correcto(data.resultGarantia.Mensaje);

                    Anticipo.LimpiarGarantia();
                }
            }
        });
    },
    LimpiarFormulario: function () {
        $('#inputDoc').val("");
        $('#Monto').val("");
        $('#respaldoDoc').val("");
        $('#Banco').val("");
        $('#GarNumeroCheque').val("");
        $('#GarRutResponsable').val("");
        $('#PasResponsable').val("");
        $('#NombreResponsable').val("");
        $('#DireccionResponsable').val("");
        $('#ComunaResponsable').val("");
        $('#FichaClinica').val("");
        $('#GarEpisodio').val("");
        $('#GarRutPaciente').val("");
        $('#PasPaciente').val("");
        $('#NombrePaciente').val("");
        $('#DireccionPaciente').val("");
        $('#ComunaPaciente').val("");
        $('#Comentario').val("");

        $('#divRespaldo').hide();
        $('#divCheque').hide();

        $('#btnDevolucion').hide();
        $('#btnAplicar').hide();
        $("#btnGuardar").hide();

        $("#Comentario").prop("disabled", true);
    },
    Imprimir: function () {
        var docentry = $("#idDoc").val();

        if ($('#inputDoc').val() == "ANTICIPO") {
            VisorReporte("ComprobantePagoAnticipo.rpt", docentry, "DocEntry");
        }
        else {
            $.ajax({
                url: '/Garantia/Imprimir',
                type: 'GET',
                cache: false,
                data: { DocEntry: docentry },
                contentType: 'application/json; charset=utf-8',
                success: function (data) {
                    if (data.resultGarantia.EsError) {
                        Mensaje.Alerta(data.resultGarantia.Mensaje);
                    } else {
                        GenerarPDFBase64(data.resultGarantia.Data, "Garantia.pdf");
                    }
                }
            });
        }
    },
    AgregarComentario: function () {
        var docentry = $("#idDoc").val();
        var Comentario = $("#Comentario").val();
        $.ajax({
            url: '/Garantia/AgregarComentario',
            type: 'GET',
            cache: false,
            data: { DocEntry: docentry, Comentario: Comentario },
            contentType: 'application/json; charset=utf-8',
            success: function (data) {
                Mensaje.Alerta(data.resultGarantia.Mensaje);

            }
        });
    }
};