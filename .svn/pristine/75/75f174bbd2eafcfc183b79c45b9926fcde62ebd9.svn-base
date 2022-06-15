$(document).ready(function () {
    BoletasEmitidas.Init();
});

var BoletasEmitidas = {
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
    },
    ImprimirBoletas: function (reporte) {
        var fechaDesde = $('#txtFechaDesde').val();
        var fechaHasta = $('#txtFechaHasta').val();

        while (fechaDesde.includes("/")) { fechaDesde = fechaDesde.replace("/", "-"); }
        while (fechaHasta.includes("/")) { fechaHasta = fechaHasta.replace("/", "-"); }
        fechaDesde = fechaDesde.split("-")[2] + '-' + fechaDesde.split("-")[1] + '-' + fechaDesde.split("-")[0];
        fechaHasta = fechaHasta.split("-")[2] + '-' + fechaHasta.split("-")[1] + '-' + fechaHasta.split("-")[0];


        var listParametros = [
                { value: fechaDesde, text: 'FechaDesde' },
                { value: fechaHasta, text: 'FechaHasta' },
                { value: $('#hdnUsuario').val(), text: 'Usuario' },
                { value: $('#hdnTipoPerfil').val(), text: 'TipoPerfil' },
        ];
        VisorReporteMultipleParametros(reporte, listParametros);
    }
}