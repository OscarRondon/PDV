$(document).ready(function () {
    $('#txtFechaIngreso').datetimepicker({ format: "DD-MM-YYYY", locale: "es" });
    $("#txtrut").Rut({
        on_error: function () {
            Mensaje.Alerta("Rut ingresado es inválido. Intente nuevamente.");
            $("#GarRutResponsable").val("");
        },
        on_empty: function () {
            $("#GarRutResponsable").val("");
        }
    });
    $('#btnBuscarGarantia').click(function () {
        LoadGarantias();
    });
});

function LoadGarantias() {
    var RutResponsableTexto = $("#txtrut").val();
    var FechaIngresoTexto = $("#txtFechaIngreso").val();
    var returnData;
    $.ajax({
        url: '/Garantia/ListadoGarantiasRut',
        type: 'GET',
        cache: false,
        data: { rutResponsable: RutResponsableTexto, fechaInicio: FechaIngresoTexto },
        contentType: 'application/json; charset=utf-8',
        success:
            function (respuesta) {
                if (respuesta.EsError) {
                    Mensaje.Alerta(respuesta.Mensaje);
                }
                else {
                    $("#divDt").show();
                    $('#tblAntGar').find("tbody").empty();
                    $('#tblAntGar').DataTable().destroy();
                    returnData = respuesta.Data;
                    $('#tblAntGar').DataTable(
                        {
                            "language": dataTableEspanol,
                            "info": false,
                            "paging": true,
                            "searching": true,
                            "ordering": true,
                            "aaData": returnData,
                            "aoColumns":
                                [
                                    {
                                        sDefaultContent: null,
                                        bSortable: true,
                                        mRender: function (data, type, row) { return '<span style="display:none" >' + row.GarFechaIngresoNumero + '</span>' + row.GarFechaIngresoTexto; },
                                    },
                                    { mDataProp: "TipoDocumentoTexto" },
                                    { mDataProp: "GarMontoTexto" },
                                    { mDataProp: "GarEntidad" },
                                    { mDataProp: "GarNumeroCheque" }
                                ]
                        });
                    $('#tblAntGar')
                        .removeClass('display')
                        .addClass('table table-striped table-bordered');
                }
            },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            alert("Status: " + textStatus); alert("Error: " + errorThrown);
        }
    });
};

function SaveIngresoDevolucion() {
    var jsonAntGar = {
        IdTipoDocumento: "D",

        RutResponsable: $('#RutResponsable').val(),
        NombreResponsable: $('#NombreResponsable').val(),
        DireccionResponsable: "",
        ComunaResponsable: "",
        CiudadResponsable: "",

        GarRutPaciente: "",
        NombrePaciente: "",
        DireccionPaciente: "",
        ComunaPaciente: "",

        CiudadPaciente: "",
        FichaClinicaPaciente: "",
        Valor: $('#txtTotal').val(),
        Entidad: "",
        Numero: 0,
        Indicador: -1
    };
    var postData = { antGar: jsonAntGar };

    $.ajax({
        url: '/Garantia/IngresoGarantia',
        type: 'POST',
        dataType: 'json',
        cache: false,
        data: JSON.stringify(postData),
        contentType: 'application/json; charset=utf-8',
        success: function (data) {
            if (data !== "" && data !== null) {
                if (data.includes("correctamente")) {
                    Mensaje.Correcto(data);
                    LimpiarCampos();
                } else {
                    Mensaje.Alerta(data);
                }
            }
        }
    });
}