var documentoAprobados = [];
var busquedaFiltro;

$(document).ready(function () {
    GenerarNomina.Init();
});

var GenerarNomina = {
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

        GenerarNomina.CargarNominas([]);
    },
    BuscarNominas: function () {
        GenerarNomina.CargarNominas([]);

        if ($('#ddlTipoNomina').val() == "") {
            Mensaje.Alerta("Debe seleccionar tipo nómina.");
        }
        else {

            var jsonNomina = {
                FechaDesde: $('#txtFechaDesde').val(),
                FechaHasta: $('#txtFechaHasta').val(),
                TipoNomina: $('#ddlTipoNomina').val(),
                TipoAtencion: $('#ddlFuncion').val(),
            };
            busquedaFiltro = jsonNomina;

            var postData = { filtro: jsonNomina };
            $.ajax({
                url: '/Nomina/ListadoNominasAprobadas',
                type: 'POST',
                cache: false,
                data: JSON.stringify(postData),
                contentType: 'application/json; charset=utf-8',
                success: function (respuesta) {
                    if (respuesta.EsError) {
                        Mensaje.Alerta(respuesta.Mensaje);
                    }
                    else {
                        documentoAprobados = respuesta.Data;
                        GenerarNomina.CargarNominas(respuesta.Data);
                    }
                }
            });
        }
    },
    CargarNominas: function (data) {
        $('#listaNominas').DataTable().destroy();
        $('#listaNominas').DataTable({
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

            "order": [[0, 'asc']],
            "aaData": data,
            "aoColumns":
            [
                { mDataProp: "NumeroNomina", sClass: "text-right", },
                { mDataProp: "TipoDocumentoDescripcion" },
                { mDataProp: "NumeroDocumento" },
                { mDataProp: "FechaOrdenAtencionTexto" },
                { mDataProp: "AdmisionistaNombre" },
                { mDataProp: "NumeroOrdenAtencion", sClass: "text-right" },
                { mDataProp: "FechaOrdenAtencionTexto", sClass: "text-center" },
                { mDataProp: "PacienteRut", sClass: "text-right" },
                { mDataProp: "PacienteNombre" },
                { mDataProp: "TitularNombre" },
                { mDataProp: "PacienteCategoria" },
                { mDataProp: "UnidadCobroPaciente" },
                { mDataProp: "UnidadCobroEpisodio" },
                {
                    sDefaultContent: "",
                    sClass: "text-right",
                    mRender: function (data, type, full) {
                        return "$ " + Moneda(full.MontoPago.toString());
                    }
                }
            ]
        });
    },
    ValidarGenerarNomina: function () {
        if (documentoAprobados.length > 0) {
            if ($('#ddlTipoNomina').val() == "") {
                Mensaje.Alerta("Debe seleccionar tipo nómina.");
            }
            else {
                Mensaje.Confirmar("¿Está seguro de generar la Nómina?", GenerarNomina.GeneracionNominaFinal);
            }
        }
        else {
            Mensaje.Alerta("No existen documentos para generar la nomina.");
        }
    },
    GeneracionNominaFinal: function () {
        var postData = { filtro: busquedaFiltro };

        $.ajax({
            url: '/Nomina/GenerarNominaSupervisor',
            type: 'POST',
            cache: false,
            data: JSON.stringify(postData),
            contentType: 'application/json; charset=utf-8',
            success: function (respuesta) {
                Mensaje.Alerta(respuesta.Mensaje);

                if (!respuesta.EsError) {
                    GenerarNomina.CargarNominas([]);
                }
            }
        });
    }
}