$(document).ready(function () {
    AnularPago.Init();
});
var tablaPagos;
function CreateRowDataPago(d) {
    var filas = '<div style="text-align: center; font-weight: bold;width: 100%"> Pago sin datos.</div>';
    if (d != null && (d.MontoEfectivo > 0 || d.MontoTransferencia > 0 || d.ListaPago.length > 0)) {
        filas = '<div class="col-sm-1"></div><div class="col-sm-11"><table class="table table-hover table-condensed compact">' +
            '<thead><tr style="background-color: #d9edf7;">' +
            '<th width="20%">Medio de Pago</th>' +
            '<th>Tarjeta</th>' +
            '<th>Banco</th>' +
            '<th>Fecha</th>' +
            '<th>Documento</th>' +
            '<th>Monto</th>' +
            '</tr>' +
            '</thead>';
        filas += '<tbody>';

        if (d.MontoEfectivo > 0) {
            filas += '<tr>' +
                '<td>Efectivo</td>' +
                '<td>--</td>' +
                '<td>--</td>' +
                '<td>--</td>' +
                '<td>--</td>' +
                '<td style="text-align: right" class="text-nowrap"> $ ' + Moneda(d.MontoEfectivo.toString()) + '</td>' +
                '</tr>';
        }
        if (d.MontoRedondeo != 0) {
            filas += '<tr>' +
                '<td>Ley de Redondeo</td>' +
                '<td>--</td>' +
                '<td>--</td>' +
                '<td>--</td>' +
                '<td>--</td>' +
                '<td style="text-align: right" class="text-nowrap"> $ ' + Moneda(d.MontoRedondeo.toString()) + '</td>' +
                '</tr>';
        }

        if (d.MontoTransferencia > 0) {
            filas += '<tr>' +
                '<td>Transferencia Bancaria</td>' +
                '<td>--</td>' +
                '<td>' + d.BancoTransferenciaNombre + '</td>' +
                '<td class="text-nowrap">' + d.FechaTransferenciaTexto + '</td>' +
                '<td>' + d.NumeroTransferencia + '</td>' +
                '<td style="text-align: right" class="text-nowrap">$' + Moneda(d.MontoTransferencia.toString()) + '</td>' +
                '</tr>';
        }
        if (d.ListaPago != null && d.ListaPago.length > 0) {

            for (var i = 0; i < d.ListaPago.length; i++) {
                var p = d.ListaPago[i];
                if (p.MontoCheque > 0) {
                    filas += '<tr>' +
                        '<td>' + (p.EsValeVista ? 'Vale Vista' : 'Cheque') + '</td>' +
                        '<td>--</td>' +
                        '<td>' + p.NombreBanco + '</td>' +
                        '<td class="text-nowrap">' + p.FechaVencimientoChequeTexto + '</td>' +
                        '<td>' + p.NumeroCheque + '</td>' +
                        '<td style="text-align: right" class="text-nowrap">$' + Moneda(p.MontoCheque.toString()) + '</td>' +
                        '</tr>';
                }
                else if (p.MontoTarjeta > 0) {
                    filas += '<tr>' +
                        '<td>' + (p.CodigoTarjeta == 4 ? 'Tarjeta de Débito' : 'Tarjeta de Crédito') + '</td>' +
                        '<td>' + p.NombreTarjeta + '</td>' +
                        '<td>' + p.NombreBanco + '</td>' +
                        '<td>--</td>' +
                        '<td>' + p.NumeroOperacionTarjeta + '</td>' +
                        '<td style="text-align: right" class="text-nowrap">$' + Moneda(p.MontoTarjeta.toString()) + '</td>' +
                        '</tr>';
                }

            }
        }
        filas += '</tbody></table></div>';
    }

    return filas;
}

var AnularPago = {
    Init: function () {
        mLoading.modal();

        AnularPago.CargarPagos([]);

        $('#listPagos tbody').on('click', '.filaPago', function () {
            var tr = $(this).closest('tr');
            var row = tablaPagos.row(tr);

            if (row.child.isShown()) {
                // This row is already open - close it
                row.child.hide();
                tr.removeClass('shown');

                $("#iconFilaPlus_" + tr[0].dataset.fila).show();
                $("#iconFilaMinus_" + tr[0].dataset.fila).hide();
            }
            else {
                // Open this row
                row.child(CreateRowDataPago(row.data())).show();
                tr.addClass('shown');

                $("#iconFilaPlus_" + tr[0].dataset.fila).hide();
                $("#iconFilaMinus_" + tr[0].dataset.fila).show();
            }
        });

        $('#ModalAnularPago').on('hidden.bs.modal', function (e) {
            AnularPago.LimpiarPagos();
        })
    },
    BuscarPagosRecibidos: function (tipo, docId, docLinea, trackId) {
        var postData = {
            tipoDocumento: tipo,
            DocumentoId: docId,
            DocumentoLinea: docLinea,
            TrackId: trackId
        };
        $.ajax({
            url: '/PuntoVenta/ObtenerListaPagos',
            type: 'POST',
            cache: false,
            data: JSON.stringify(postData),
            contentType: 'application/json; charset=utf-8',
            success: function (respuesta) {
                if (respuesta.EsError) {
                    Mensaje.Alerta(respuesta.Mensaje);
                }
                else {
                    if (respuesta.Data.length == 0) {
                        Mensaje.Alerta("Documento no tiene pago asociado");
                    }
                    else {
                        AnularPago.CargarPagos(respuesta.Data);
                        $('#ModalAnularPago').modal();
                    }
                }
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                //JM
                alert("Status: " + textStatus); alert("Error: " + errorThrown);
            }
        });
    },
    BuscarPagoRecibido: function (pagoId) {
        var postData = { PagoId: pagoId };
        $.ajax({
            url: '/PuntoVenta/ObtenerPagoRecibido',
            type: 'POST',
            cache: false,
            data: JSON.stringify(postData),
            contentType: 'application/json; charset=utf-8',
            success: function (respuesta) {
                if (respuesta.EsError) {
                    Mensaje.Alerta(respuesta.Mensaje);
                }
                else {
                    if (respuesta.Data.length == 0) {
                        Mensaje.Alerta("Documento no tiene pago asociado");
                    }
                    else {
                        AnularPago.CargarPagos(respuesta.Data);
                        $('#ModalAnularPago').modal();
                    }
                }
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                //JM
                alert("Status: " + textStatus); alert("Error: " + errorThrown);
            }
        });
    },
    CargarPagos: function (data) {
        if (tablaPagos != null) {
            tablaPagos.clear();
            tablaPagos.destroy();
        }

        tablaPagos = $('#listPagos').DataTable({
            "language": dataTableEspanol,
            "bAutoWidth": false,
            "bFilter": false,
            "bInfo": true,
            "bLengthChange": false,
            "bPaginate": true,
            "bProcessing": false,
            "bSort": true,
            "pageLength": 5,

            "order": [[1, 'desc']],

            "aaData": data,
            "aoColumns":
                [
                    {
                        className: 'filaPago text-center',
                        sDefaultContent: "",
                        bSortable: false,
                        "width": "5%",
                        mRender: function (data, type, full, meta) {
                            return '<span id="iconFilaPlus_' + meta.row + '" class="glyphicon glyphicon-plus"  style="color: green; width: 10px; hight: 10px"></span>' +
                                '<span id="iconFilaMinus_' + meta.row + '" class="glyphicon glyphicon-minus"  style="color: red; width: 10px; hight: 10px; display: none"></span>';
                        },
                    },
                    {
                        className: 'filaPago',
                        mDataProp: "DocNum",
                        "width": "30%",
                    },
                    {
                        sDefaultContent: "",
                        className: "text-nowrap filaPago",
                        "width": "30%",
                        mRender: function (data, type, full, meta) {
                            return '<span style="display:none">' + full.FechaDocumentoCorrelativo + '</span>' + full.FechaDocumentoTexto;
                        }
                    },
                    {
                        mDataProp: "MontoTotal",
                        "width": "30%",
                        className: "text-right text-nowrap filaPago",
                        render: $.fn.dataTable.render.number('.', ',', 0, '$')
                    },
                    {
                        className: 'text-center',
                        sDefaultContent: "",
                        bSortable: false,
                        "width": "5%",
                        mRender: function (data, type, full, meta) {
                            return '<button type="button" class="btn btn-xs btn-danger" title="Anular pago recibido" onclick="AnularPago.ConfirmacionAnularPago(' + full.DocEntry + ')" ><span class="glyphicon glyphicon-remove"></span></button></div>';
                        },
                    },

                ],
            "fnCreatedRow": function (nRow, aData, iDataIndex) {
                nRow.dataset.fila = iDataIndex;
                nRow.style.cursor = "pointer";
            },
        });
    },

    LimpiarPagos: function () {
        $("#hdnAnulaPagoTipo").val(""),
            AnularPago.CargarPagos([]);
    },

    AbrirFormularioLista: function (tipo, docId, docLinea, trackId) {
        AnularPago.BuscarPagosRecibidos(tipo, docId, docLinea, trackId);

        $("#hdnAnulaPagoTipo").val(tipo);
    },
    AbrirFormularioPago: function (tipo, pagoId) {
        AnularPago.BuscarPagoRecibido(pagoId);

        $("#hdnAnulaPagoTipo").val(tipo);
    },

    ConfirmacionAnularPago: function (pagoId) {
        bootbox.dialog({
            message: '<span class="glyphicon glyphicon-question-sign" style="color: blue"></span>&nbsp;&nbsp;¿Está seguro de anular este pago recibido?<br/><br/><div class="form-group" ><label class="control-label">Ingrese motivo de anulación:</label><textarea id="txtObservacionCancelacionPago" rows="3" class="form-control textUpperTrim" style="max-width: none" maxlength="250"></textarea></div>',
            buttons: {
                cancel: {
                    label: 'Cancelar',
                    className: 'btn-default'
                },
                confirm: {
                    label: 'Aceptar',
                    className: 'btn-primary',
                    callback: function (data) {
                        $("#txtObservacionCancelacionPago").focus();
                        if ($("#txtObservacionCancelacionPago").val() == "") {
                            Mensaje.Alerta("Debe ingresar el motivo de anulación.");
                            return false;
                        }
                        else {
                            AnularPago.CancelarPagoRecibido(pagoId, $("#txtObservacionCancelacionPago").val());
                        }
                    }
                }
            }
        });
    },
    CancelarPagoRecibido: function (pagoId, observacion) {
        $.ajax({
            url: '/PuntoVenta/AnularPagoRecibido',
            type: 'GET',
            cache: false,
            data: {
                tipoDocumento: $("#hdnAnulaPagoTipo").val(),
                pagoId: pagoId,
                observacion: observacion,
                asignacionId: $("#idAsignacion").val()
            },
            contentType: 'application/json; charset=utf-8',
            success: function (result) {
                if (!result.EsError) {
                    Mensaje.Correcto(result.Mensaje);
                    AnularPago.LimpiarPagos();
                    $('#ModalAnularPago').modal('hide');
                    PendientePago.LimpiarFormularioPendiente();
                }
                else {
                    Mensaje.Alerta(result.Mensaje);
                }
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                //JM
                alert("Status: " + textStatus); alert("Error: " + errorThrown);
            }
        });
    },
}
