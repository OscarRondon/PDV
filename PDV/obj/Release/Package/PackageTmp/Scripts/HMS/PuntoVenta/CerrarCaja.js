$(document).ready(function () {
    Cierre.Init();
});
var Cierre = {
    Init: function () {
        $(document).on("blur", "#txtMontoRecaudado", function () {
            if ($(this).val() == "") $(this).val(0);
            var montoRecaudado = $(this).val();
            while (montoRecaudado.includes(".")) {
                montoRecaudado = montoRecaudado.replace(".", "");
            }
            var montoFinal = parseInt(montoRecaudado);
            var montoDiferencia = montoFinal - (parseInt($("#hdnMontoEfectivo").val()) + parseInt($("#hdnMontoAjuste").val()));

            if (montoDiferencia > 0) {
                $("#txtMontoDiferencia").css('color', 'green');
            }
            else if (montoDiferencia < 0) {
                $("#txtMontoDiferencia").css('color', 'red');
            }
            else if (montoDiferencia == 0) {
                $("#txtMontoDiferencia").css('color', 'black');
            }

            if (montoDiferencia < 0) {
                montoDiferencia = montoDiferencia * -1;
                $("#txtMontoDiferencia").val("-" + Moneda(montoDiferencia.toString()));
            }
            else {
                $("#txtMontoDiferencia").val(Moneda(montoDiferencia.toString()));
            }
        });
    },

    LimpiarFormulario: function () {
        $("#hdnMontoEfectivo").val(0);
        $("#hdnMontoAjuste").val(0);

        $("#txtMontoRecaudado").val(0);
        $("#txtMontoDiferencia").val(0);

        $("#txtMontoInicial").css('color', 'black');
        $("#txtMontoRecaudado").css('color', 'black');
        $("#txtMontoDiferencia").css('color', 'black');

        Cierre.CargarGrillaContado([]);
        Cierre.CargarGrillaDocumento([]);
    },
    CargarGrillaContado: function (data) {
        var montoTotal = 0;

        var footerContadoMonto = 0;
        var footerContadoCantidad = 0;

        $('#listaTotalPagos').find("tbody").empty();
        $('#listaTotalPagos').DataTable().destroy();
        $('#listaTotalPagos').DataTable({
            "language": dataTableEspanol,
            "bFilter": false,
            "bInfo": false,
            "bLengthChange": false,
            "bPaginate": false,
            "bProcessing": false,
            "bSort": true,
            "bAutoWidth": false,

            "aaData": data,
            "aoColumns":
            [
                { mDataProp: "Descripcion", sClass: "text-nowrap" },
                {
                    mDataProp: "Monto",
                    className: "text-right text-nowrap",
                    render: $.fn.dataTable.render.number('.', ',', 0, '$')
                },
                { mDataProp: "Total", sClass: "text-center" }
            ],
            "fnInitComplete": function (oSettings, json) {
                $("#hdnMontoEfectivo").val(montoTotal);
                $("#txtMontoRecaudado").val(Moneda((montoTotal).toString()));
            },
            "fnFooterCallback": function (nFoot, aData, iStart, iEnd, aiDisplay) {
                var api = this.api(), aData;

                if (aData.length > 0) {
                    footerContadoMonto = api.column(1).data().reduce(function (a, b) { return intVal(a) + intVal(b); }, 0);
                    footerCantidadContado = api.column(2).data().reduce(function (a, b) { return intVal(a) + intVal(b); }, 0);


                    $(api.column(1).footer()).html('$' + Moneda(footerContadoMonto.toString()));
                    $(api.column(2).footer()).html(footerCantidadContado.toString());

                    montoTotal = 0;
                    for (var i = 0; i < aData.length; i++) {
                        if (aData[i].Descripcion.toUpperCase() != "LEY REDONDEO PÉRDIDA") {
                            montoTotal += aData[i].Monto;
                        }
                    }
                }
            },
        });
    },
    CargarGrillaDocumento: function (data) {
        var footerMonto = 0;

        $('#listaTotalDocumento').find("tbody").empty();
        $('#listaTotalDocumento').DataTable().destroy();
        $('#listaTotalDocumento').DataTable({
            "language": dataTableEspanol,
            "bFilter": false,
            "bInfo": false,
            "bLengthChange": false,
            "bPaginate": false,
            "bProcessing": false,
            "bSort": true,
            "bAutoWidth": false,

            "aaData": data,
            "aoColumns":
            [
                { mDataProp: "Descripcion", sClass: "text-nowrap" },
                {
                    mDataProp: "Monto",
                    className: "text-right text-nowrap",
                    render: $.fn.dataTable.render.number('.', ',', 0, '$')
                },
            ],
            "fnFooterCallback": function (nFoot, aData, iStart, iEnd, aiDisplay) {
                var api = this.api(), data;
                footerMonto = api.column(1).data().reduce(function (a, b) { return intVal(a) + intVal(b); }, 0);

                $(api.column(1).footer()).html('$' + Moneda(footerMonto.toString()));
            },
        });
    },
    BuscarMontosCierre: function () {
        Cierre.LimpiarFormulario();

        var idAsignacion = $("#idAsignacion").val();
        $.ajax({
            url: '/PuntoVenta/MontoCierreCaja',
            type: 'GET',
            data: { IdAsignacionCaja: idAsignacion },
            cache: false,
            contentType: 'application/json; charset=utf-8',
            success: function (result) {
                if (result.EsError) {
                    Mensaje.Alerta(result.Mensaje);
                }
                else {
                    var data1 = [];
                    var data2 = [];
                    result.Data.forEach(function (d) { if (d.Tipo == 1) { data1.push(d); } });
                    result.Data.forEach(function (d) { if (d.Tipo == 2) { data2.push(d); } });

                    Cierre.CargarGrillaContado(data1);
                    Cierre.CargarGrillaDocumento(data2);
                }
            }
        });
    },

    GuardarCierreEfectivo: function () {
        if (Cierre.ValidarCierreEfectivo()) {
            Mensaje.Confirmar("Al realizar el cierre contado, la caja no podrá seguir recibiendo dinero. <br/>¿Desea continuar?", Cierre.CerrarCajaEfectivo)
        }
    },
    ValidarCierreEfectivo: function () {
        var montoRecaudado = $("#txtMontoRecaudado").val();
        while (montoRecaudado.includes(".")) {
            montoRecaudado = montoRecaudado.replace(".", "");
        }
        var montoDiferencia = $("#txtMontoDiferencia").val();
        while (montoDiferencia.includes(".")) {
            montoDiferencia = montoDiferencia.replace(".", "");
        }
        var montoEfectivo = $("#hdnMontoEfectivo").val();
        var montoAjuste = $("#hdnMontoAjuste").val();

        var mRecaudado = parseInt(montoRecaudado);
        var mDiferencia = parseInt(montoDiferencia);
        var mEfectivo = parseInt(montoEfectivo);
        var mAjuste = parseInt(montoAjuste);

        var esValido = true;
        if ((mEfectivo == 0 && mRecaudado == 0) ||
            (mEfectivo == mRecaudado) ||
            (mEfectivo == (mRecaudado - mAjuste))) {
            //Continua
        }
        else if ((mRecaudado - mAjuste) > mEfectivo) {
            Mensaje.Alerta("Monto recaudado es mayor al monto en efectivo.");
            esValido = false;
        }
        else if ((mRecaudado - mAjuste) < mEfectivo) {
            Mensaje.Alerta("Monto recaudado es menor al monto en efectivo.");
            esValido = false;
        }
        return esValido;
    },
    CerrarCajaEfectivo: function () {
        var montoRecaudado = $("#txtMontoRecaudado").val();
        while (montoRecaudado.includes(".")) {
            montoRecaudado = montoRecaudado.replace(".", "");
        }
        var montoDiferencia = $("#txtMontoDiferencia").val();
        while (montoDiferencia.includes(".")) {
            montoDiferencia = montoDiferencia.replace(".", "");
        }

        var postData = {
            asignacion: {
                IdAsignacion: $("#idAsignacion").val(),
                MontoRecaudado: montoRecaudado,
                MontoDiferencia: montoDiferencia,
                MontoCierre: montoRecaudado
            }
        };

        $.ajax({
            url: '/PuntoVenta/CierreCajaEfectivo',
            type: 'POST',
            data: JSON.stringify(postData),
            cache: false,
            contentType: 'application/json; charset=utf-8',
            success: function (result) {
                if (result.EsError) {
                    Mensaje.Alerta(result.Mensaje);
                }
                else {
                    VisorReporte("ArqueoCaja2.rpt", $("#idAsignacion").val(), "IdCaja");
                    Mensaje.CorrectoF(result.Mensaje, locatioReload);
                }
            }
        });
    },

    GuardarCierreCajaFinal: function () {
        if ($("#hdnEstadoAsignacion").val() == 3) { //Ya fue ingresada el arqueo
            Mensaje.Confirmar("¿Está seguro de realizar el cierre de caja?", Cierre.CerrarCajaFinal)
        }
        else { // Validamos que los datos ingresados esten todos...
            if (Cierre.ValidarCierreEfectivo()) {
                Mensaje.Confirmar("¿Está seguro de realizar el cierre de caja?", Cierre.CerrarCajaFinal)
            }
        }
    },
    CerrarCajaFinal: function () {
        var montoRecaudado = $("#txtMontoRecaudado").val();
        while (montoRecaudado.includes(".")) {
            montoRecaudado = montoRecaudado.replace(".", "");
        }
        var montoDiferencia = $("#txtMontoDiferencia").val();
        while (montoDiferencia.includes(".")) {
            montoDiferencia = montoDiferencia.replace(".", "");
        }

        var postData = {
            asignacion: {
                IdAsignacion: $("#idAsignacion").val(),
                MontoRecaudado: montoRecaudado,
                MontoDiferencia: montoDiferencia,
                MontoCierre: montoRecaudado,
                Estado: $("#hdnEstadoAsignacion").val()
            }
        };

        $.ajax({
            url: '/PuntoVenta/CierreCajaFinal',
            type: 'POST',
            data: JSON.stringify(postData),
            cache: false,
            contentType: 'application/json; charset=utf-8',
            success: function (result) {
                if (result.EsError) {
                    Mensaje.Alerta(result.Mensaje);
                }
                else {
                    if ($("#hdnEstadoAsignacion").val() != 3) {
                        VisorReporte("ArqueoCaja2.rpt", $("#idAsignacion").val(), "IdCaja");
                    }
                    Mensaje.CorrectoF(result.Mensaje, redireccionarHome);
                }
            }
        });
    },

    MotoDiferenciaAjustar: function () {
        var montoDiferencia = $("#txtMontoDiferencia").val();
        while (montoDiferencia.includes(".")) {
            montoDiferencia = montoDiferencia.replace(".", "");
        }
        var mDiferencia = parseInt(montoDiferencia);

        if (mDiferencia > 0) {
            Mensaje.Confirmar("¿Esta seguro de realizar el ajuste cierre contado?", Cierre.GuardarAjusteDiferencia);
        }
        else {
            Mensaje.Alerta("Para realizar ajuste, es necesario que el monto diferencia sea a favor.");
        }
    },
    GuardarAjusteDiferencia: function () {
        var montoDiferencia = $("#txtMontoDiferencia").val();
        while (montoDiferencia.includes(".")) {
            montoDiferencia = montoDiferencia.replace(".", "");
        }
        var postData = {
            asignacion: {
                IdAsignacion: $("#idAsignacion").val(),
                MontoDiferencia: montoDiferencia
            }
        };
        $.ajax({
            url: '/PuntoVenta/AjusteMontoDiferencia',
            type: 'POST',
            data: JSON.stringify(postData),
            cache: false,
            contentType: 'application/json; charset=utf-8',
            success: function (data) {
                if (data.EsError) {
                    Mensaje.Alerta(data.Mensaje);
                }
                else {
                    Mensaje.CorrectoF(data.Mensaje, Cierre.BuscarMontosCierre);
                }
            }
        });
    },
}