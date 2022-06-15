//$(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
$(document).ready(function () {
    $('#btnBuscarReporte').click(function () {
        PrintReporte("1","");
    });
});

function PrintReporte(IdBoleta,impresora) {
    var jsonBoleta = {
        IdBoleta: IdBoleta,
        impresora: impresora
    };
    var postData = { boleta: jsonBoleta };
    $.ajax({
        url: '/Reports/ListaBoletabyRut',
        type: 'GET',
        dataType: 'json',
        data: JSON.stringify(postData),
        contentType: 'application/json; charset=utf-8',
        success: function (data) {
            if (data != "" && data != null) {
                if (data.includes("Imprimiendo")) {
                    Mensaje.Correcto(data);
                } else {
                    Mensaje.Alerta(data);
                }
            }
        }
    });
}