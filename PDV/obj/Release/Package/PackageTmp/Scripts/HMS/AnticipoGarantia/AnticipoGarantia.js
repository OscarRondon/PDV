$(document).ready(function () {
    Anticipo.Init();
});
var Anticipo = {
    Init: function () {
        $('#txtGarantiaFechaDesde').datetimepicker({
            format: "DD/MM/YYYY",
            ignoreReadonly: true,
            showTodayButton: true,
            allowInputToggle: true,
            locale: "es"
        });
        $('#txtGarantiaFechaHasta').datetimepicker({
            format: "DD/MM/YYYY",
            ignoreReadonly: true,
            showTodayButton: true,
            allowInputToggle: true,
            //JMuseCurrent: false,//Important! See issue #1075, se usa solo cuanto esta vacia la fecha
            locale: "es"
        });
        $("#txtGarantiaFechaDesde").on("dp.change", function (e) {
            $('#txtGarantiaFechaHasta').data("DateTimePicker").minDate(e.date);
        });
        $("#txtGarantiaFechaHasta").on("dp.change", function (e) {
            $('#txtGarantiaFechaDesde').data("DateTimePicker").maxDate(e.date);
        });

        $('#btnGarantiaFechaDesde').click(function () {
            $('#txtGarantiaFechaDesde').focus();
        });
        $('#btnGarantiaFechaHasta').click(function () {
            $('#txtGarantiaFechaHasta').focus();
        });

        Anticipo.CargaListadoGarantia([]);
    },
    BuscarGarantias: function () {
        if ($("#txtGarantiaFechaDesde").val() == "" &&
            $("#txtGarantiaFechaHasta").val() == "" &&
            $("#ddlGarantiaEstado").val() == "" &&
            $("#txtGarantiaNumero").val() == "" &&
            $("#txtGarantiaResponsable").val() == "" &&
            $("#txtGarantiaPaciente").val() == "" &&
            $("#txtGarantiaEpisodio").val() == "" &&
            $("#ddlGarantiaTipo").val() == "") {
            Mensaje.Alerta("Debe ingresar al menos un filtro para la búsqueda.");
        }
        else {
            var parametros = {
                fechaInicio: $("#txtGarantiaFechaDesde").val(),
                fechaFin: $("#txtGarantiaFechaHasta").val(),
                estado: $("#ddlGarantiaEstado").val(),
                numero: $("#txtGarantiaNumero").val(),
                responsable: $("#txtGarantiaResponsable").val(),
                paciente: $("#txtGarantiaPaciente").val(),
                episodio: $("#txtGarantiaEpisodio").val(),
                tipo: $("#ddlGarantiaTipo").val(),
            }

            $.ajax({
                url: '/Garantia/ListadoGarantiasFechas',
                type: 'GET',
                cache: false,
                data: parametros,
                contentType: 'application/json; charset=utf-8',
                success: function (respuesta) {
                    if (respuesta.EsError) {
                        Mensaje.Alerta(respuesta.Mensaje);
                    }
                    else {
                        Anticipo.CargaListadoGarantia(respuesta.Data);
                    }
                }
            });
        }
    },
    CargaListadoGarantia: function (data) {
        $('#listaGarantias').DataTable().destroy();
        $('#listaGarantias').DataTable({
            "language": dataTableEspanol,
            "bFilter": true,
            "bInfo": true,
            "bLengthChange": false,
            "bPaginate": true,
            "bProcessing": false,
            "bSort": true,
            "bDestroy": true,
            "sScrollX": "100%",
            "bScrollCollapse": true,

            "order": [[0, 'desc']],

            "aaData": data,
            "aoColumns":
                [
                    { mDataProp: "GarDocNum" },
                    {
                        className: "text-nowrap",
                        mRender: function (data, type, row) {
                            return '<span style="display:none" >' + row.GarFechaIngresoNumero + '</span>' + row.GarFechaIngresoTexto;
                        },
                    },
                    { mDataProp: "GarFechaFinTexto" },
                    { mDataProp: "GarRutResponsable" },
                    { mDataProp: "NombreResponsable" },
                    { mDataProp: "GarEstadoTexto" },
                    { mDataProp: "TipoDocumentoTexto" },
                    {
                        mDataProp: "GarMonto",
                        className: "text-right text-nowrap",
                        render: $.fn.dataTable.render.number('.', ',', 0, '$')
                    },
                    {
                        bSortable: false,
                        mRender: function (data, type, row) {
                            var resultado = "";
                            if (row.DiasVencimiento == null) {
                                resultado = '--';
                            }
                            else if (row.DiasVencimiento > 0) {
                                resultado = '<span class="fa fa-circle" style="color:green"></span> Vence en ' + row.DiasVencimiento + ' días';
                            }
                            else if (row.DiasVencimiento == 0) {
                                resultado = '<span class="fa fa-circle" style="color:green"></span> Vence hoy';
                            }
                            else {
                                resultado = '<span class="fa fa-circle" style="color:red"></span> Vencida ' + (row.DiasVencimiento * -1) + ' días';
                            }
                            return '<center>' + resultado + '</center>';
                        },
                    },
                    { mDataProp: "Comentario" },
                    {
                        bSortable: false,
                        mRender: function (data, type, row) {
                            var stringObjeto = JSON.stringify(row);
                            var textArea = '<textarea style="display:none" id="ta_' + row.GarDocEntry + '_' + row.GarDocNum + '">' + stringObjeto + '</textarea>';
                            var botonEditar = '<a data-docentry="' + row.GarDocEntry + '" data-docnum="' + row.GarDocNum + '"  onclick="DetallesGarantia.AbrirFormulario(this)" style="cursor: pointer"><span class="glyphicon glyphicon-eye-open" aria-hidden="true"></span></a>';

                            var resultado = '<center>' + botonEditar + '</center>' + textArea;
                            return resultado;
                        },
                    }
                ],
        });
    },

    LimpiarGarantia: function () {
        Anticipo.CargaListadoGarantia([]);

        $("#txtGarantiaFechaDesde").val("");
        $("#txtGarantiaFechaHasta").val("");
        $("#ddlGarantiaEstado").val("");
        $("#txtGarantiaNumero").val("");
        $("#txtGarantiaResponsable").val("");
        $("#txtGarantiaPaciente").val("");
        $("#txtGarantiaEpisodio").val("");
        $("#ddlGarantiaTipo").val("");
    },
}