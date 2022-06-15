
$(document).ready(function () {
    ReporteDocumentosAbiertos.Init();
});

var ReporteDocumentosAbiertos = {
    Init: function () {
        $('#txtFechaDesde').datetimepicker({
            format: "DD/MM/YYYY",
            ignoreReadonly: true,
            showTodayButton: true,
            allowInputToggle: true,
            locale: "es"
        });
        $('#txtFechaDesde').on('dp.change', function (e) {
            $('#txtFechaDesde').data('DateTimePicker').minDate(e.date);
        });

        $('#btnFechaDesde').click(function () {
            $('#txtFechaDesde').focus();
        });
        

        $('#panelFiltro').on('hide.bs.collapse', function (e) {
            $("#iconoFiltro").removeClass("glyphicon-menu-up");
            $("#iconoFiltro").addClass("glyphicon-menu-down");
        });
        $('#panelFiltro').on('show.bs.collapse', function (e) {
            $("#iconoFiltro").removeClass("glyphicon-menu-down");
            $("#iconoFiltro").addClass("glyphicon-menu-up");
        });

        
    },
    Buscar: function () {       
        $("#PanelBodyReporteDocumento").empty();
        $.ajax({
            url: '/Reports/ReporteDocumentosAbiertos',
            type: 'GET',
            cache: false,
            data: { fecha: $("#txtFechaDesde").val() },
            contentType: 'application/json; charset=utf-8',
            success: function (data) {
                if (data.EsError) {
                    Mensaje.Alerta(data.result.Mensaje);
                } else {
                    GenerarPDFBase64(data.result.Data, "DocumentosAbiertos.pdf");      
                    
                }
            }
        });
    }
}