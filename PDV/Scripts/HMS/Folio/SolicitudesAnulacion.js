$(document).ready(function () {
    SolicitudesAnulacion.Init();
});
var SolicitudesAnulacion = {
    Init: function () {
        $('#txtFechaInicio').datetimepicker({
            format: "DD/MM/YYYY",
            ignoreReadonly: true,
            showTodayButton: true,
            allowInputToggle: true,
            locale: "es"
        });
        $('#txtFechaFinal').datetimepicker({
            format: "DD/MM/YYYY",
            ignoreReadonly: true,
            showTodayButton: true,
            allowInputToggle: true,
            //JMuseCurrent: false,//Important! See issue #1075, se usa solo cuanto esta vacia la fecha
            locale: "es"
        });
        $("#txtFechaInicio").on("dp.change", function (e) {
            $('#txtFechaFinal').data("DateTimePicker").minDate(e.date);
        });
        $("#txtFechaFinal").on("dp.change", function (e) {
            $('#txtFechaInicio').data("DateTimePicker").maxDate(e.date);
        });

        $('#btnFechaInicio').click(function () {
            $('#txtFechaInicio').focus();
        });
        $('#btnFechaFinal').click(function () {
            $('#txtFechaFinal').focus();
        });

        SolicitudesAnulacion.LoadFolios();
        mLoading.modal();
        SolicitudesAnulacion.BuscarFolios();
    },
    BuscarFolios: function () {
        var jsonFolio = {
            FechaInicio: $('#txtFechaInicio').val(),
            FechaFin: $('#txtFechaFinal').val()
        };
        var postData = { folio: jsonFolio };
        $.ajax({
            url: '/Folio/BuscarFolioAnulados',
            type: 'POST',
            cache: false,
            data: JSON.stringify(postData),
            contentType: 'application/json; charset=utf-8',
            success: function (respuesta) {
                if (respuesta.EsError) {
                    Mensaje.Alerta(respuesta.Mensaje);
                }
                else {
                    SolicitudesAnulacion.LoadFolios(respuesta.Data);
                }
            }
        });
    },
    LoadFolios: function (data) {
        $('#listaFolioAnulados').DataTable().destroy();
        $('#listaFolioAnulados').DataTable({
            "language": dataTableEspanol,
            "bAutoWidth": false,
            "bFilter": false,
            "bInfo": true,
            "bLengthChange": false,
            "bPaginate": true,
            "bProcessing": false,
            "bSort": true,
            "bDestroy": true,

            "order": [[0, 'asc']],

            "aaData": data,
            "aoColumns":
            [
                { mDataProp: "FolioInicial", sType: "numeric", sClass: "text-right" },
                { mDataProp: "FolioFinal", sType: "numeric", sClass: "text-right" },
                { mDataProp: "FechaAnulacionTexto", sType: "date", sClass: "text-center" },
                { mDataProp: "NombreCaja", sDefaultContent: "-" },
                { mDataProp: "NombreUsuarioAnulacion", sDefaultContent: "-" },
                { mDataProp: "Motivo", sDefaultContent: "-" },
            ]
        });
    }
}