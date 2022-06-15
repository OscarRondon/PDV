$(document).ready(function () {
    AplicarAnticipo.Init();
});


var AplicarAnticipo = {
    Init: function () {
        $('#ModalNuevaGarantia').on('hidden.bs.modal', function (e) {
            AplicarAnticipo.LimpiarFormulario();
        });
    },
    AbrirFormulario: function (episodio, rutRespon, nombreRespon, montoFactura) {
        $('#txtAplicarResponsableRut').val(rutRespon);
        $('#txtAplicarResponsableNombre').val(nombreRespon);
        $('#txtAplicarEpisodio').val(episodio);
        $('#ModalAplicarAnticipo').modal("show");
        $('#txtMontoFactura').val(montoFactura);
        AplicarAnticipo.CargarMatrix(episodio, rutRespon);
    },
    LimpiarFormulario: function () {
        $('#txtAplicarResponsableRut').val("");
        $('#txtAplicarResponsableNombre').val("");
        $('#txtAplicarEpisodio').val("");
        $('#txtMontoFactura').val("");
        AplicarAnticipo.CargaGrilla([]);
        $('#ModalAplicarAnticipo').modal("hide");
    },

    GuardarFormulario: function () {
        if (NuevaGarantia.ValidarFormulario()) {
            Mensaje.Confirmar("¿Esta seguro de guardar la garantía?", NuevaGarantia.IngresarGarantia);
        }
    },
    IngresarAnticipo: function (DocEntryGar, MontoAnticipo, MontoFactura, DocEntryPago) {
        var jsonAntGar = {
            GarDocEntry: DocEntryGar,
            NumPagRecib: DocEntryPago,
            NumDocBoleta: $('#PagoSeleccionadoId').val(),
            IdTrack: $('#PagoSeleccionadoTrack').val(),
            IdAsignacion: $("#idAsignacion").val(),
            IdCaja: $("#idCaja").val(),
            GarMonto: MontoFactura
        };

        var postData = { antGar: jsonAntGar };
        $.ajax({
            url: '/PuntoVenta/AplicarAnticipoBoleta',
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
                    AplicarAnticipo.LimpiarFormulario();

                    if (result.DocNum == "2") {
                        PendientePago.ImprimirDocumentoPago();
                    }

                    Mensaje.Correcto(mensaje);

                    $('#ModalAplicarAnticipo').modal("hide");
                    PendientePago.LimpiarFormularioPendiente();
                }
            },
        });
    },
    CargarMatrix: function (episodio, rutResp) {
        $.ajax({
            url: '/Garantia/BuscarAnticipoEpisodio',
            type: 'GET',
            cache: false,
            data: { episodio: episodio, rutRes: rutResp },
            contentType: 'application/json; charset=utf-8',
            success: function (result) {
                if (!result.EsError) {
                    var info = result.data;
                    AplicarAnticipo.CargaGrilla(result.data);
                }
                else {
                    Mensaje.Alerta(result.Mensaje);
                }
            }
        });
    },
    CargaGrilla: function (data) {
        $('#listaAnticipos').DataTable().clear();
        $('#listaAnticipos').DataTable({
            "language": dataTableEspanol,
            "bFilter": false,
            "bInfo": false,
            "bLengthChange": false,
            "bPaginate": false,
            "bProcessing": false,
            "bSort": false,
            "bDestroy": true,
            "bAutoWidth": false,

            "aaData": data,
            "aoColumns":
            [
                { mDataProp: "GarDocNum", className: "text-center" },
                { mDataProp: "GarFechaIngresoTexto", className: "text-center" },
                { mDataProp: "GarMonto", className: "text-center" },
                {
                    sDefaultContent: "",
                    bSortable: false,
                    mRender: function (data, type, full, meta) {
                        var dataFila = ' data-docentry="' + full.GarDocEntry + '"';
                        dataFila += ' data-numpagrecib="' + full.NumPagRecib + '"';
                        dataFila += ' data-MontoAPagar="' + full.GarMonto + '"';
                        dataFila += ' data-MontoFactura="' + $('#txtMontoFactura').val() + '"';
                        var resultado = "";
                        var btnPag = '<div class="btn-group"><button type="button" style="padding-left: 0px; padding-right: 0px;" class="btn btn-sm btn-success" title="Aplicar Anticipo" ' + dataFila + ' onclick="AplicarAnticipo.AsociarAnticipo(' + full.GarDocEntry + ',' + full.GarMonto + ',' + $('#txtMontoFactura').val() + ',' + "'" + full.NumPagRecib + "'" + ');" ><span class="glyphicon glyphicon-ok-sign"></span></button></div>';
                        resultado = btnPag;
                        return '<div class="btn-group btn-group-justified" style="width: 80px">' + resultado + '</div>';
                    },
                }
            ]
        });
    },
    AsociarAnticipo: function (DocEntryGar, MontoAnticipo, MontoFactura, DocEntryPago) {
        if (DocEntryPago == "null") {
            Mensaje.Alerta("El anticipo no contiene un pago asociado");
            return false;
        } else {
            if (parseFloat(MontoAnticipo) > parseFloat(MontoFactura)) {
                Mensaje.Alerta("El monto del anticipo es superior al monto de la factura");
            } else {
                AplicarAnticipo.IngresarAnticipo(DocEntryGar, MontoAnticipo, MontoFactura, DocEntryPago);
            }
        }
    },
}