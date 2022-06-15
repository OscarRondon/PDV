$(document).ready(function () {
    GarantiaListado.CargarListaGarantias([]);
});
var GarantiaListado = {
    BuscarListaGarantias: function () {
        $.ajax({
            url: '/Garantia/ListadoGarantiasCaja',
            type: 'GET',
            cache: false,
            data: { asignacion: $("#idAsignacion").val() },
            contentType: 'application/json; charset=utf-8',
            success: function (respuesta) {
                if (respuesta.EsError) {
                    Mensaje.Alerta(respuesta.Mensaje);
                }
                else {
                    GarantiaListado.CargarListaGarantias(respuesta.Data);
                }
            }
        });
    },
    CargarListaGarantias: function (data) {
        $('#listadoGarantias').DataTable().destroy();
        $('#listadoGarantias').DataTable({
            "language": dataTableEspanol,
            "bAutoWidth": false,
            "bFilter": true,
            "bInfo": true,
            "bLengthChange": false,
            "bPaginate": true,
            "bProcessing": false,
            "bSort": true,

            "order": [[0, 'desc']],

            "aaData": data,
            "aoColumns":
            [
                    { mDataProp: "TipoDocumentoTexto" },
                { mDataProp: "GarFechaIngresoTexto" },
                { mDataProp: "GarRutResponsable" },
                { mDataProp: "NombreResponsable" },
                { mDataProp: "GarRutPaciente" },
                { mDataProp: "NombrePaciente" },
                { mDataProp: "GarFichaClinica" },
                { mDataProp: "GarMontoTexto" },
                { mDataProp: "GarIndicadorTexto" },
                { mDataProp: "GarEntidad" },
                { mDataProp: "GarNumeroCheque" },

            ],
        });
    },
}