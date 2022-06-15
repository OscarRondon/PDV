$(document).ready(function () {
    $(document).on('blur', '.textUpperTrim', function () {
        PDV.TextoUpperTrim(this);
    });
});

var dataTableEspanol = {
    "sProcessing": "Procesando...",
    "sLengthMenu": "Mostrar _MENU_ registros",
    "sZeroRecords": "No se encontraron resultados",
    "sEmptyTable": "Ningún dato disponible en esta tabla",
    "sInfo": "Mostrando registros del _START_ al _END_ de un total de _TOTAL_ registros",
    "sInfoEmpty": "Mostrando registros del 0 al 0 de un total de 0 registros",
    "sInfoFiltered": "(filtrado de un total de _MAX_ registros)",
    "sInfoPostFix": "",
    "sInfoThousands": ",",
    "sSearch": "Buscar: ",
    "sUrl": "",
    "sLoadingRecords": "Cargando...",
    "oPaginate": {
        "sFirst": "Primero",
        "sLast": "Último",
        "sNext": "Siguiente",
        "sPrevious": "Anterior",
    },
    "oAria": {
        "sSortAscending": ": Activar para ordenar la columna de manera ascendente",
        "sSortDescending": ": Activar para ordenar la columna de manera descendente"
    }
};

var intVal = function (i) {
    if (typeof i === 'string') {
        let multiplier = /[\(\)]/g.test(i) ? -1 : 1;

        return (i.replace(/[\$,\(\)]/g, '') * multiplier)
    }
    return typeof i === 'number' ? i : 0;
};

function VisorReporte(reportName, paramValue, paramName) {
    window.open("../Reports/AspReportViewer.aspx?reportName=" + reportName + "&paramValue=" + paramValue + "&paramName=" + paramName);
    window.onafterprint = function () {
        window.close();
    };
}
function VisorReporteBoleta(reportName, paramValue, paramName) {
    window.open("../Reports/DescargarReporteForPrint?reportName=" + reportName + "&paramValue=" + paramValue + "&paramName=" + paramName);
}
function VisorReporteMultipleParametros(reportName, reportParam) {
    if (reportParam.length > 0) {
        var link = "../Reports/AspReportViewer.aspx?reportName=" + reportName;

        for (var i = 0; i < reportParam.length; i++) {
            link += "&paramValue=" + reportParam[i].value + "&paramName=" + reportParam[i].text;
        }

        window.open(link);
    }
}
function VerObservacion(texto) {
    bootbox.alert(texto);
}
function GenerarPDFBase64(stringBase64, nombreArchivo) {
    if (window.navigator && window.navigator.msSaveOrOpenBlob) {
        var byteCharacters = atob(stringBase64);
        var byteNumbers = new Array(byteCharacters.length);
        for (var i = 0; i < byteCharacters.length; i++) {
            byteNumbers[i] = byteCharacters.charCodeAt(i);
        }
        var byteArray = new Uint8Array(byteNumbers);
        var blob = new Blob([byteArray], {
            type: 'application/pdf'
        });
        window.navigator.msSaveOrOpenBlob(blob, nombreArchivo);
    } else { // Directly use base 64 encoded data for rest browsers (not IE)
        var base64EncodedPDF = stringBase64;
        var dataURI = "data:application/pdf;base64," + base64EncodedPDF;
        window.open(dataURI, '_blank');

    }
}
function redireccionarHome() {
    mLoading.modal();
    window.location.href = "/Home/Index";
};
function locatioReload() {
    mLoading.modal();
    location.reload();
};

function Moneda(entrada) {
    var num = entrada.replace(/\./g, "");
    if (!isNaN(num)) {
        num = num.toString().split('').reverse().join('').replace(/(?=\d*\.?)(\d{3})/g, '$1.');
        num = num.split("").reverse().join("").replace(/^[\.]/, "");
        entrada = num;
    } else {
        entrada = input.value.replace(/[^\d\.]*/g, "");
    }
    return entrada;
};

function Redondeo(monto) {
    var ultimo = monto.charAt(monto.length - 1)
    var unidad = monto.slice(0, -1);
    if (ultimo == 0) {
        unidad = monto;
    }
    else if (1 <= ultimo && ultimo <= 5) {
        unidad = unidad.concat('0');
    }
    else {
        var nuevoMonto = unidad.slice(0, -1);
        var nuevoUltimo = unidad.charAt(unidad.length - 1);
        var ajuste = parseInt(nuevoUltimo) + 1;
        unidad = nuevoMonto.concat(ajuste, '0');
    }
    return unidad;
};

function soloNumeros(e) {
    var key = window.Event ? e.which : e.keyCode
    return ((key >= 48 && key <= 57) || (key == 8))
}
var PDV = {
    CalculoMultiplo: function (valor, multiplo) {
        var resto = valor % multiplo;
        if (resto == 0)
            return true;
        else
            return false;
    },
    SoloNumero: function (objeto) {
        objeto.value = objeto.value.replace(/[^0-9]/g, '');
        //Si es cero se deja vacio
        objeto.value = Number(objeto.value).toString();
        if (objeto.value == "0") objeto.value = "";
    },
    SoloNumeroFormato: function (objeto) {
        objeto.value = objeto.value.replace(/[^0-9]/g, '');
        //Si es cero se deja vacio
        objeto.value = Number(objeto.value).toString();
        if (objeto.value == "0") objeto.value = "";
        //Aplica formato numero
        objeto.value = objeto.value.toString().replace(/(\d)(?=(\d{3})+(?!\d))/g, "$1.");
    },
    Restar: function (obj) {
        //HDR
        obj.value = obj.value.replace(/[^0-9]/g, '');
        //Si es cero se deja vacio
        obj.value = Number(obj.value).toString();
        if (obj.value == "0") obj.value = "";
        var monto = $("#txtMontoPagar").val();
        monto = monto.replace(/[^0-9]/g, '');
        monto.value = Number(monto.value).toString();
        var dif = obj.value - monto;
        if (dif == 0 || dif < 0) dif = "0";
        dif = dif.toString().replace(/(\d)(?=(\d{3})+(?!\d))/g, "$1.");
        $("#txtMontoPagoVuelto").val(dif);
        obj.value = obj.value.toString().replace(/(\d)(?=(\d{3})+(?!\d))/g, "$1.");
    },
    TextoUpperTrim: function (objeto) {
        objeto.value = objeto.value.trim().toUpperCase();

        while (objeto.value.includes("  ")) {
            objeto.value = objeto.value.replace("  ", " ");
        }
    },
}