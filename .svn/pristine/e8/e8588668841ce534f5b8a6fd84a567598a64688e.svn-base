$(document).ready(function () {
    PagosAnulados.Init();
});

var PagosAnulados = {
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

        $('#ddlUsuarioSupervisor').on('change', function (e) {
            $('#ddlUsuarioAdmisionista').val("");
            $('#ddlCajas').val("");
        });
        $('#ddlUsuarioAdmisionista').on('change', function (e) {
            $('#ddlUsuarioSupervisor').val("");
            $('#ddlCajas').val("");
        });
        $('#ddlCajas').on('change', function (e) {
            $('#ddlUsuarioSupervisor').val("");
            $('#ddlUsuarioAdmisionista').val("");
        });
    },
    ImprimirReporte: function (reporte) {
        var usuario = $('#hdnUsuario').val();
        var perfil = $('#hdnTipoPerfil').val();

        var fechaDesde = $('#txtFechaDesde').val();
        var fechaHasta = $('#txtFechaHasta').val();
        
        var supervisor = $('#ddlUsuarioSupervisor').val();
        var admisionista = $('#ddlUsuarioAdmisionista').val();
        var caja = $('#ddlCajas').val() != null ? $('#ddlCajas').val() : "";


        if (supervisor != null && supervisor != "") {
            usuario = supervisor;
            perfil = "2";
        }
        else if (admisionista != null && admisionista != "") {
            usuario = admisionista;
            perfil = "1";
        }

        while (fechaDesde.includes("/")) { fechaDesde = fechaDesde.replace("/", "-"); }
        while (fechaHasta.includes("/")) { fechaHasta = fechaHasta.replace("/", "-"); }
        fechaDesde = fechaDesde.split("-")[2] + '-' + fechaDesde.split("-")[1] + '-' + fechaDesde.split("-")[0];
        fechaHasta = fechaHasta.split("-")[2] + '-' + fechaHasta.split("-")[1] + '-' + fechaHasta.split("-")[0];


        var listParametros = [
            { value: fechaDesde, text: 'FechaDesde' },
            { value: fechaHasta, text: 'FechaHasta' },
            { value: usuario, text: 'Usuario' },
            { value: perfil, text: 'TipoPerfil' },
            { value: caja, text: 'CajaId' },
        ];
        VisorReporteMultipleParametros(reporte, listParametros);
    }
}