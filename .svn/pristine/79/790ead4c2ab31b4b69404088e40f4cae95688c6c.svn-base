$(document).ready(function () {
    GlobalFolio.Init();
});

var GlobalFolio = {
    Init: function () {
        GlobalFolio.CargarListaFolio([]);
    },
    BuscarListaFolio: function () {
        GlobalFolio.CargarListaFolio([]);

        if ($('#txtFolioGloblalDesde').val() == "" ||
            $('#txtFolioGloblalHasta').val() == "") {
            Mensaje.Alerta("Debe ingresar N° folio desde y hasta para buscar.");
        }
        else {
            $.ajax({
                url: '/Reports/ListaGlobalFolio',
                type: 'GET',
                cache: false,
                data: {
                    folioInicio: $('#txtFolioGloblalDesde').val(),
                    folioFin: $('#txtFolioGloblalHasta').val()
                },
                contentType: 'application/json; charset=utf-8',
                success: function (respuesta) {
                    if (respuesta.EsError) {
                        Mensaje.Alerta(respuesta.Mensaje);
                    }
                    else {
                        GlobalFolio.CargarListaFolio(respuesta.Data);
                    }
                }
            });
        }
    },
    CargarListaFolio: function (data) {
        $('#listaFolios').DataTable().destroy();
        $('#listaFolios').DataTable({
            "language": dataTableEspanol,
            "bAutoWidth": false,
            "bFilter": false,
            "bInfo": true,
            "bLengthChange": false,
            "bPaginate": true,
            "bProcessing": false,
            "bSort": true,

            "order": [[0, 'asc']],

            "aaData": data,
            "aoColumns":
                [
                    { mDataProp: "NumeroFolio" },
                    { mDataProp: "Estado" },
                    {
                        sDefaultContent: "--",
                        className: "text-nowrap",
                        mRender: function (data, type, full) {
                            return '<span style="display:none">' + full.FechaCorrelativo + '</span>' + full.FechaTexto;
                        }
                    },
                    { mDataProp: "Ficha" },
                    { mDataProp: "Episodio" },
                    { mDataProp: "Titular" },
                    { mDataProp: "Usuario" },
                    {
                        mDataProp: "Monto",
                        className: "text-right text-nowrap",
                        render: $.fn.dataTable.render.number('.', ',', 0, '$')
                    },
                    { mDataProp: "CajaNombre" },
                    {
                        className: "text-center text-nowrap",
                        mRender: function (data, type, full) {
                            var texto = '';
                            if (full.Observacion != null) {
                                texto = '<a onclick="VerObservacion(' + "'" + full.Observacion + "'" + ')" style="cursor: pointer" title="Observación" class="glyphicon glyphicon-comment"></a>';
                            }
                            return texto;
                        }
                    },
                ],
        });
    },
    ImprimirReporte: function () {
        if ($('#txtFolioGloblalDesde').val() == "" ||
            $('#txtFolioGloblalHasta').val() == "") {
            Mensaje.Alerta("Debe ingresar N° folio desde y hasta para buscar.");
        }
        else {
            var listParametros = [
                { value: $('#txtFolioGloblalDesde').val(), text: 'FolioDesde' },
                { value: $('#txtFolioGloblalHasta').val(), text: 'FolioHasta' },
            ];

            VisorReporteMultipleParametros("FoliosGlobal.rpt", listParametros);
        }
    },
}