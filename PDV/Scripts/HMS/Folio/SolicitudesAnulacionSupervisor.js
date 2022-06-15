var tablaAnulados;
$(document).ready(function () {
    SolicitudesAnulacionSupervisor.Init();

    $('#listaFolioAnulados').on('click', '.filaGrupo', function () {
       
        var currentOrder = tablaAnulados.order()[0];
        if (currentOrder[0] === 0 && currentOrder[1] === 'asc') {
            tablaAnulados.order([0, 'desc']).draw();
        }
        else {
            tablaAnulados.order([0, 'asc']).draw();
        }
    });
});
var SolicitudesAnulacionSupervisor = {
    Init: function () {
        // Order by the grouping


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

        //SolicitudesAnulacionSupervisor.LoadFolios();
        mLoading.modal();
        SolicitudesAnulacionSupervisor.BuscarFolios();
    },
    BuscarFolios: function () {
        var jsonFolio = {
            FechaInicio: $('#txtFechaInicio').val(),
            FechaFin: $('#txtFechaFinal').val()
        };
        var postData = { folio: jsonFolio };
        $.ajax({
            url: '/Folio/BuscarFolioAnuladosPorSupervisor',
            type: 'POST',
            cache: false,
            data: JSON.stringify(postData),
            contentType: 'application/json; charset=utf-8',
            success: function (respuesta) {
                if (respuesta.EsError) {
                    Mensaje.Alerta(respuesta.Mensaje);
                }
                else {
                    SolicitudesAnulacionSupervisor.LoadFolios(respuesta.Data);
                }
            }
        });
    },
    LoadFolios: function (data) {
        tablaAnulados = $('#listaFolioAnulados').DataTable({
            "language": dataTableEspanol,
            "bAutoWidth": false,
            "bFilter": false,
            "bInfo": true,
            "bLengthChange": false,
            "bPaginate": true,
            "bProcessing": false,
            "bSort": true,
            "bDestroy": true,
            "aaData": data,
            "order": [[0, 'asc']],
            "aoColumns":
            [
                { mDataProp: "NombreCaja", bVisible: false },
                { mDataProp: "FolioInicial", sType: "numeric", sClass: "text-right" },
                { mDataProp: "FolioFinal", sType: "numeric", sClass: "text-right" },
                { mDataProp: "FechaAnulacionTexto", sType: "date", sClass: "text-center" },
                { mDataProp: "NombreUsuarioAnulacion", sDefaultContent: "-" },
                { mDataProp: "Motivo", sDefaultContent: "-" },
            ],
            "drawCallback": function (settings) {
                var api = this.api();
                var rows = api.rows({ page: 'current' }).nodes();
                var last = null;

                api.column(0, { page: 'current' }).data().each(function (group, i) {
                    if (last !== group) {
                        $(rows).eq(i).before(
                            '<tr class="group filaGrupo" style="cursor: pointer"><td colspan="5"><strong>' + group + '</strong></td></tr>'
                        );
                        last = group;
                    }
                });
            }
        });
    }
}