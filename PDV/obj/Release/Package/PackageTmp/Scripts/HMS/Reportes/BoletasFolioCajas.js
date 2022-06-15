$(document).ready(function () {
    BoletasFolioCaja.Init();
});

var BoletasFolioCaja = {
    Init: function () {
        $('#txtFechaDesde').datetimepicker({
            format: "DD/MM/YYYY",
            ignoreReadonly: true,
            showTodayButton: true,
            allowInputToggle: true,
            locale: "es"
        });
        $('#txtFechaHasta').datetimepicker({
            format: "DD/MM/YYYY",
            ignoreReadonly: true,
            showTodayButton: true,
            allowInputToggle: true,
            //JMuseCurrent: false,//Important! See issue #1075, se usa solo cuanto esta vacia la fecha
            locale: "es"
        });
        $('#txtFechaDesde').on('dp.change', function (e) {
            $('#txtFechaHasta').data('DateTimePicker').minDate(e.date);
        });
        $('#txtFechaHasta').on('dp.change', function (e) {
            $('#txtFechaDesde').data('DateTimePicker').maxDate(e.date);
        });

        $('#btnFechaDesde').click(function () {
            $('#txtFechaDesde').focus();
        });
        $('#btnFechaHasta').click(function () {
            $('#txtFechaHasta').focus();
        });

        $('#panelFiltro').on('hide.bs.collapse', function (e) {
            $("#iconoFiltro").removeClass("glyphicon-menu-up");
            $("#iconoFiltro").addClass("glyphicon-menu-down");
        });
        $('#panelFiltro').on('show.bs.collapse', function (e) {
            $("#iconoFiltro").removeClass("glyphicon-menu-down");
            $("#iconoFiltro").addClass("glyphicon-menu-up");
        });

        BoletasFolioCaja.CargaGrillaCajasAsignadas([]);
    },
    ObtenerCajas: function () {
        var fechaInicio, fechaFinal;
        fechaInicio = $("#txtFechaDesde").val();
        fechaFinal = $("#txtFechaHasta").val();

        $.ajax({
            url: '/Reports/ListaCajasAsignadas',
            type: 'GET',
            cache: false,
            data: { FechaInicio: fechaInicio, FechaFin: fechaFinal },
            contentType: 'application/json; charset=utf-8',
            success:
            function (result) {
                if (!result.EsError) {
                    BoletasFolioCaja.CargaGrillaCajasAsignadas(result.Data);
                }
                else {
                    Mensaje.Alerta(result.Mensaje);
                }
            }
        });
    },
    CargaGrillaCajasAsignadas: function (data) {
        $('#listCajasAsignadas').DataTable().destroy();
        $('#listCajasAsignadas').DataTable(
            {
                "language": dataTableEspanol,
                "bFilter": true,
                "bInfo": true,
                "bLengthChange": false,
                "bPaginate": true,
                "bProcessing": false,
                "bSort": true,

                "order": [[2, 'desc']],
                "aaData": data,
                "aoColumns":
                [
                    { mDataProp: "CajaAsignada.IdentificacionCaja" },
                    { mDataProp: "UsuarioAsignado" },
                    {
                        sDefaultContent: "-",
                        mRender: function (data, type, full) {
                            return '<span style="display: none">' + full.FechaCreacionFiltro + '</span>' + full.FechaCreacionTexto;
                        }
                    },
                    {
                        sDefaultContent: "-",
                        bSortable: false,
                        mRender: function (data, type, full) {
                            var resultado = "";
                            if (full.Estado == 1) {
                                resultado = full.FechaRechazoTexto;
                            }
                            else if (full.Estado == 2) {
                                resultado = full.FechaInicioTexto;
                            }
                            else if (full.Estado == 3) {
                                resultado = full.FechaCierreEfectivoTexto;
                            }
                            else if (full.Estado == 4) {
                                resultado = full.FechaCierreCajaTexto;
                            }

                            return resultado;
                        }
                    },
                    {
                        sDefaultContent: "-",
                        mRender: function (data, type, full) {
                            var color = 'gray';
                            var texto = full.EstadoDescripcion;
                            if (full.Estado == 1) {
                                color = 'Red';
                                texto = '<a onclick="VerObservacion(' + "'" + full.ObservacionRechazo + "'" + ')" style="cursor: pointer" title="Motivo de rechazo">' + full.EstadoDescripcion + '</a>';
                            }
                            else if (full.Estado == 2) {
                                color = 'Orange';
                            }
                            else if (full.Estado == 3) {
                                color = 'Yellow';
                            }
                            else if (full.Estado == 4) {
                                color = 'Green';
                            }

                            var resultado = '<span class="fa fa-circle" style="color: ' + color + '"></span>&nbsp;' + texto;
                            return resultado;
                        }
                    },
                    {
                        sDefaultContent: "-",
                        bSortable: false,
                        mRender: function (data, type, full) {
                            var resultado = "";
                            if (full.Estado != 0 && full.Estado != 1) {
                                var btnEmitidas = '<div class="btn-group"><button type="button" class="btn btn-primary btn-sm" title="Imprimir Boletas Emitidas" onclick="BoletasFolioCaja.ImprimirBoletasEmitidas(' + "'" + full.IdAsignacion + "'" + ')"><span class="glyphicon glyphicon-print"></span>&nbsp;Emitidas</button></div>';
                                var btnAnuladas = '<div class="btn-group"><button type="button" class="btn btn-danger btn-sm" title="Imprimir Boletas Anuladas" onclick="BoletasFolioCaja.ImprimirBoletasAnuladas(' + "'" + full.IdAsignacion + "'" + ')"><span class="glyphicon glyphicon-print"></span>&nbsp;Anuladas</button></div>';
                                resultado = '<div class="btn-group btn-group-justified" style="width: 170px">' + btnEmitidas + btnAnuladas + '</div>';
                            }
                            return resultado;
                        }
                    }
                ],
            });
    },
    ImprimirBoletasEmitidas: function (_idAsignacion) {
        VisorReporte("EstadisticaBoletasEmitidas.rpt", _idAsignacion, "IdAsignacion");
    },
    ImprimirBoletasAnuladas: function (_idAsignacion) {
        VisorReporte("EstadisticaBoletasAnuladas.rpt", _idAsignacion, "IdAsignacion");
    }
}