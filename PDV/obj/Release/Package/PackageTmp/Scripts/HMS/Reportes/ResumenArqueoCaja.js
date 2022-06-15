$(document).ready(function () {
    ResumenArqueoCaja.Init();
});

var ResumenArqueoCaja = {
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

        ResumenArqueoCaja.CargaGrillaCajasAsignadas([]);
    },
    ObtenerCajas: function () {
        var fechaInicio, fechaFinal;
        fechaInicio = $("#txtFechaDesde").val();
        fechaFinal = $("#txtFechaHasta").val();

        $.ajax({
            url: '/Reports/ListaCajasCerradas',
            type: 'GET',
            cache: false,
            data: { FechaInicio: fechaInicio, FechaFin: fechaFinal },
            contentType: 'application/json; charset=utf-8',
            success:
            function (result) {
                if (!result.EsError) {
                    ResumenArqueoCaja.CargaGrillaCajasAsignadas(result.Data);
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
                            if (full.Estado == 3 || full.Estado == 4) {
                                var btnImprimir = '<div class="btn-group"><button type="button" class="btn btn-primary btn-sm" title="Imprimir Arqueo Caja" onclick="ResumenArqueoCaja.ImprimirArqueoCaja(' + "'" + full.IdAsignacion + "'" + ')"><span class="glyphicon glyphicon-print"></span>&nbsp;Arqueo Caja</button></div>';
                                resultado = '<div class="btn-group btn-group-justified" style="width: 170px">' + btnImprimir + '</div>';
                            }
                            return resultado;
                        }
                    }
                ],
            });
    },
    ImprimirArqueoCaja: function (_idAsignacion) {
        VisorReporte("ArqueoCaja2.rpt", _idAsignacion, "IdCaja");
    }
}