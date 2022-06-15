var pagoBase = null;
var lstPagoBase = []

var panelActivo = "#ninguno";

$('#txtFechaPago').datetimepicker({
    format: "DD/MM/YYYY",
    ignoreReadonly: true,
    showTodayButton: true,
    allowInputToggle: true,
    locale: "es"
});
$('#btnFechaPago').click(function () {
    $('#txtFechaPago').focus();
});

function GetPanelActivo() {
    panelActivo = "#" + $("#pnlPagoActivo").val() + " ";

    var agregar = true;

    if (lstPagoBase.length > 0) {
        for (var i = 0; i < lstPagoBase.length; i++) {
            if (lstPagoBase[i].Panel == $("#pnlPagoActivo").val()) {
                agregar = false;
                pagoBase = lstPagoBase[i];
                return false;
            }
        }
    }

    if (agregar) {
        var pagoNuevo = {
            Panel: $("#pnlPagoActivo").val(),
            pagoNombreCompletoPaciente: "",
            pagoNombreCompletoResponsable: "",
            pagoMontoBonificacionFinanciador: "",
            pagoMontoCopagoBeneficiario: "",
            pagoMontoCopagoBeneficiarioFinal: 0,
            pagoIniciado: false,

            pagoTotal: 0,
            pagoDiferencia: 0,

            montoFinalEfectivo: 0,
            montoFinalCheque: 0,
            montoFinalTarjeta: 0,
            montoFinalTransferencia: 0,
            fechaTransferencia: "",
            numeroTransferencia: "",
            bancoTransferencia: "",

            lstPagos: [],

            tablaMedioPago: null
        }
        pagoBase = pagoNuevo;
        lstPagoBase.push(pagoNuevo);
    }
}
$(document).on("blur", "#txtMontoPagar", function () {
    if ($(panelActivo + "#formaPagoSeleccionada").val() == 1) {
        MedioPago.MedioPagoRedondeo(this);
    }
});

var MedioPago = {
    PagoDetalleNormal: function (montoCubierto, montoPendiente, paciente, responsable) {
        if (!pagoBase.pagoIniciado) {
            pagoBase.pagoMontoBonificacionFinanciador = montoCubierto;
            pagoBase.pagoMontoCopagoBeneficiarioFinal = montoPendiente;
            pagoBase.pagoNombreCompletoPaciente = paciente;
            pagoBase.pagoNombreCompletoResponsable = responsable;

            pagoBase.pagoIniciado = true;

            $(panelActivo + "#txtMontoCubierto").val(Moneda(pagoBase.pagoMontoBonificacionFinanciador));
            $(panelActivo + "#txtMontoPago").val(Moneda(pagoBase.pagoMontoCopagoBeneficiarioFinal.toString()));

            $(panelActivo + "#txtNombreCompletoPaciente").val(pagoBase.pagoNombreCompletoPaciente);
            $(panelActivo + "#txtNombreCompletoResponsable").val(pagoBase.pagoNombreCompletoResponsable);

            MedioPago.PagoCargaGrilla();
            MedioPago.PagoCargaFooter();
        }
    },
    PagoLimpiar: function () {
        $(panelActivo + "#txtNombreCompletoPaciente").val("");
        $(panelActivo + "#txtNombreCompletoResponsable").val("");
        $(panelActivo + "#txtMontoCubierto").val("");
        $(panelActivo + "#txtMontoPago").val("");

        $(panelActivo + "#ddlFormaPago").val("");

        //Limpiar Parametros
        pagoBase.pagoNombreCompletoPaciente = "";
        pagoBase.pagoNombreCompletoResponsable = "";
        pagoBase.pagoMontoBonificacionFinanciador = "";
        pagoBase.pagoMontoCopagoBeneficiario = "";
        pagoBase.pagoMontoCopagoBeneficiarioFinal = 0;
        pagoBase.pagoIniciado = false;

        pagoBase.pagoTotal = 0;
        pagoBase.pagoDiferencia = 0;

        pagoBase.montoFinalEfectivo = 0;
        pagoBase.montoFinalCheque = 0;
        pagoBase.montoFinalTarjeta = 0;
        pagoBase.montoFinalTransferencia = 0;
        pagoBase.fechaTransferencia = "";
        pagoBase.numeroTransferencia = "";
        pagoBase.bancoTransferencia = "";

        pagoBase.lstPagos = [];

        if (pagoBase.tablaMedioPago != null) {
            pagoBase.tablaMedioPago
                .rows()
                .remove()
                .draw();
        }
    },
    PagoAgregarMedioPago: function () {
        if ($(panelActivo + "#ddlFormaPago").val() == "") {
            Mensaje.Alerta("Debe seleccionar una forma de pago para continuar.");
        }
        else {
            if (pagoBase.pagoDiferencia != 0) {
                var item = $(panelActivo + '#ddlFormaPago')[0].options[$(panelActivo + '#ddlFormaPago')[0].selectedIndex];

                var esError = false;
                var panel = item.dataset.panel;
                pagoBase.tablaMedioPago.rows().every(function () {
                    if ((panel == 1 && this.data()[0] == "Efectivo") ||
                        (panel == 4 && this.data()[0] == "Transferencia Bancaria")) {
                        Mensaje.Alerta("Ya existe un pago con este medio de pago.");
                        esError = true;
                    }
                });

                if (!esError) {
                    MedioPago.MedioPagoMostrar(item.dataset.panel, item.text);
                }
            }
            else {
                Mensaje.Alerta("No existe monto pendiente de pago.");
            }
        }
    },
    PagoCargaGrilla: function () {
        if (pagoBase != null && pagoBase.tablaMedioPago == null) {
            pagoBase.tablaMedioPago = $(panelActivo + '#listPagos').DataTable(
                {
                    "language": dataTableEspanol,
                    "bFilter": false,
                    "bInfo": false,
                    "bLengthChange": false,
                    "bPaginate": false,
                    "bProcessing": false,
                    "bSort": false,
                    //"bDestroy": true,
                    "bAutoWidth": false,

                    "columnDefs": [
                        { className: "text-right", "targets": [3, 4] }
                    ],
                });
        }
    },
    PagoCargaFooter: function (monto, esSuma, esRedondeo) {
        if (esSuma == null) {
            pagoBase.pagoDiferencia = (pagoBase.pagoMontoCopagoBeneficiarioFinal - pagoBase.pagoTotal);
            if (isNaN(pagoBase.pagoDiferencia)) {
                pagoBase.pagoDiferencia = pagoBase.pagoMontoCopagoBeneficiarioFinal;
            } else {
                pagoBase.pagoDiferencia = pagoBase.pagoDiferencia;
            }
        }
        else if (esSuma) {
            if (esRedondeo != null && esRedondeo) {
                pagoBase.pagoTotal = pagoBase.pagoTotal + (monto * -1);
            }
            else {
                pagoBase.pagoTotal = pagoBase.pagoTotal + monto;
            }
            pagoBase.pagoDiferencia = (pagoBase.pagoMontoCopagoBeneficiarioFinal - pagoBase.pagoTotal);
        }
        else {
            pagoBase.pagoTotal = pagoBase.pagoTotal - monto;

            if (esRedondeo != null && esRedondeo) {
                pagoBase.pagoTotal = pagoBase.pagoTotal + (pagoBase.pagoTotal * -1);
            }

            pagoBase.pagoDiferencia = (pagoBase.pagoMontoCopagoBeneficiarioFinal - pagoBase.pagoTotal);
        }
        $(panelActivo + '#lblPagoTotal').val('$ ' + Moneda(pagoBase.pagoTotal.toString()));
        $(panelActivo + '#lblPagoPendi').val('$ ' + Moneda(pagoBase.pagoDiferencia.toString()));

        //JM"footerCallback": function (row, data, start, end, display) {
        //    var api = this.api(), data;

        //    // Calculo Total
        //    pagoBase.pagoTotal = 0;
        //    for (var i = 0; i < data.length; i++) {

        //        var m1 = data[i][5];
        //        var m2 = 0;
        //        if (isNaN(m1)) {
        //            while (m1.includes(".") || m1.includes(",")) {
        //                m1 = m1.replace(".", "");
        //                m1 = m1.replace(",", "");
        //            }
        //            m2 = intVal(m1);
        //        }

        //        if (data[i][0] == "Ley de Redondeo") {
        //            pagoBase.pagoTotal = pagoBase.pagoTotal + (m2 * -1);
        //        }
        //        else {
        //            pagoBase.pagoTotal = pagoBase.pagoTotal + m2;
        //        }
        //    }

        //    //Calculo Diferencia por Pagar
        //    pagoBase.pagoDiferencia = (pagoBase.pagoMontoCopagoBeneficiarioFinal - pagoBase.pagoTotal);
        //    if (isNaN(pagoBase.pagoDiferencia)) {
        //        pagoBase.pagoDiferencia = pagoBase.pagoMontoCopagoBeneficiarioFinal;
        //    } else {
        //        pagoBase.pagoDiferencia = pagoBase.pagoDiferencia;
        //    }
        //    // Actualizar footer
        //    $(api.column(5).footer()).html(
        //        '$ ' + Moneda(pagoBase.pagoTotal.toString()) + '<br />$ ' + Moneda(pagoBase.pagoDiferencia.toString())
        //    );
        //}
    },
    PagoValidarConfirmarVenta: function () {
        if ($("#PagoSeleccionadoTipo").val() == "2" || // Boleta
            $("#PagoSeleccionadoTipo").val() == "6" || // Anticipo
            $("#PagoSeleccionadoTipo").val() == "7")  // IMED
        {
            var texto = '';
            var folio = '';//$('#FolioAsignado').val().trim();
            // Funcionalidad en construcción (Crear nuevo procedimiento que me traiga el ultimo folio por caja)
            // Si no tiene un folio asignado se consulta por ultimo folio para mostrar al usuario antes de crearlo
            //if ($("#PagoSeleccionadoTipo").val() == "2") {// Boleta
            //    if (folio == '') {
            //        $.ajax({
            //            url: '/Caja/TraerFolioCaja',
            //            type: 'GET',
            //            cache: false,
            //            async: false,
            //            data: { idCaja: $("#idCaja").val() },
            //            contentType: 'application/json; charset=utf-8',
            //            success: function (data) {
            //                if (data.EsError) {
            //                    Mensaje.Alerta("Ocurrio un error al obtener folio");
            //                }
            //                else {
            //                    folio = data.Data.FolioActual;
            //                }
            //            }
            //        });
            //    }

            //    if (folio != null) {
            //        texto = ", se realizará venta con Folio: <b>" + folio + "</b>";
            //    }
            //}
            //Verificamos que el pago sea por el total
            if (pagoBase.pagoDiferencia > 0) {
                Mensaje.Alerta("Existe un monto pendiente de pago $ " + Moneda(pagoBase.pagoDiferencia.toString()) + ". Ingrese el monto faltante para confirmar el pago.");
            }
            else {
                Mensaje.Confirmar("¿Está seguro de confirmar la venta?" + texto, MedioPago.PagoConfirmarVenta);
            }
        }
        else {
            var montoFinalParcial = pagoBase.pagoMontoCopagoBeneficiarioFinal - pagoBase.pagoDiferencia;
            if (montoFinalParcial == 0) {
                Mensaje.Alerta("Existe un monto pendiente de pago $ " + Moneda(pagoBase.pagoDiferencia.toString()) + ". Ingrese el monto faltante para confirmar el pago.");
            }
            else {
                var mensaje = (pagoBase.pagoDiferencia == 0 ? "¿Está seguro de confirmar la venta?" : "¿Está seguro de confirmar el pago parcial?");
                Mensaje.Confirmar(mensaje, MedioPago.PagoConfirmarVenta);
            }
        }
    },

    PagoConfirmarVenta: function () {
        var montoFinalParcial = pagoBase.pagoMontoCopagoBeneficiarioFinal - pagoBase.pagoDiferencia;

        var pagoNuevo = {
            DocEntry: $("#PagoSeleccionadoId").val(),
            IdTrack: $('#PagoSeleccionadoTrack').val(),
            TipoPago: parseInt($("#PagoSeleccionadoTipo").val()),

            IdAsignacion: $("#idAsignacion").val(),
            IdCaja: $("#idCaja").val(),
            CajaNombre: $("#nombreCaja").val(),

            FechaTransferencia: pagoBase.fechaTransferencia,
            NumeroTransferencia: pagoBase.numeroTransferencia,
            BancoTransferencia: pagoBase.bancoTransferencia,
            MontoTransferencia: pagoBase.montoFinalTransferencia,
            MontoCheque: pagoBase.montoFinalCheque,
            MontoEfectivo: pagoBase.montoFinalEfectivo,
            MontoTarjeta: pagoBase.montoFinalTarjeta,
            MontoTotal: montoFinalParcial,
            ListaPago: pagoBase.lstPagos,

            AsientoId: 0,
            AsientoLinea: 0,
            ResponsableRut: "",
            ResponsableNombre: "",
            FolioAsignado: ""
        };

        if ($("#PagoSeleccionadoTipo").val() == "3" || $("#PagoSeleccionadoTipo").val() == "4") // Convenio | Pagare
        {
            pagoNuevo.AsientoId = $("#AsientoSeleccionadoId").val();
            pagoNuevo.AsientoLinea = $("#AsientoSeleccionadoLinea").val();
            pagoNuevo.ResponsableRut = $("#SocioNegocioCode").val();
        }
        else if ($("#PagoSeleccionadoTipo").val() == "2")//Boleta
        {
            pagoNuevo.ResponsableNombre = $('#TxtNombreResponsableDetalleVenta').val();
            pagoNuevo.ResponsableRut = $('#TxtRutResponsableDetalleVenta').val();
            pagoNuevo.FolioAsignado = $('#FolioAsignado').val().trim();
        }
        else if ($("#PagoSeleccionadoTipo").val() == "5")//Factura
        {
            pagoNuevo.ResponsableRut = $("#SocioNegocioCode").val();
        }
        else if ($("#PagoSeleccionadoTipo").val() == "6") //Anticipo
        {
            pagoNuevo.ResponsableRut = $("#SocioNegocioCode").val();
        }
        else if ($("#PagoSeleccionadoTipo").val() == "7") //IMED
        {

        }

        var postData = { pago: pagoNuevo };
        $.ajax({
            url: '/PuntoVenta/FinalizarPago',
            type: 'POST',
            cache: false,
            data: JSON.stringify(postData),
            contentType: 'application/json; charset=utf-8',
            success: function (result) {
                if (!result.EsError) {
                    Mensaje.Correcto(result.Mensaje);
                    MedioPago.PagoLimpiar();

                    if ($("#PagoSeleccionadoTipo").val() == "2") {
                        PendientePago.ImprimirDocumentoPago();
                    }
                    else if ($("#PagoSeleccionadoTipo").val() == "3" ||
                        $("#PagoSeleccionadoTipo").val() == "4" ||
                        $("#PagoSeleccionadoTipo").val() == "5" ||
                        $("#PagoSeleccionadoTipo").val() == "6") {
                        PendientePago.ImprimirPagoRecibido();
                    }
                    else if ($("#PagoSeleccionadoTipo").val() == "7") {
                        PendientePago.ImprimirOrden($('#PagoSeleccionadoTrack').val());
                    }
                }
                else {
                    Mensaje.Alerta(result.Mensaje);
                }
            }
        });
    },

    MedioPagoMostrar: function (panel, pago) {
        MedioPago.MedioPagoLimpiar();

        $(panelActivo + "#formaPagoSeleccionada").val(panel);

        var esError = false
        $(panelActivo + "#tituloMedioPago")[0].innerText = "pago con " + pago.toLowerCase();

        if (panel == 1) {//Efectivo
            $(panelActivo + "#divMontoRedondeo").show();
            $(panelActivo + "#divMontoPagoEfectivo").show();
            $(panelActivo + "#divMontoPagoVuelto").show();
        }
        else if (panel == 2) {//Credito
            $(panelActivo + "#divBanco").show();
            $(panelActivo + "#divTarjeta").show();
            $(panelActivo + "#ddlTarjetaCredito option").each(function (index, item) {
                if (item.text.toUpperCase() == "REDCOMPRA") {
                    item.disabled = true;
                }
            });
            $(panelActivo + "#divNumero").show();
        }
        else if (panel == 3) { //Cheque
            $(panelActivo + "#divBanco").show();
            $(panelActivo + "#divNumero").show();
            $(panelActivo + "#divFecha").show();
            //JM$(panelActivo + "#tituloFecha")[0].innerText = "vencimiento";
            $(panelActivo + "#tituloFecha")[0].innerText = "";
        }
        else if (panel == 4) {//Transferencia
            $(panelActivo + "#divBanco").show();
            $(panelActivo + "#divFecha").show();
            $(panelActivo + "#divNumero").show();
            $(panelActivo + "#tituloFecha")[0].innerText = "transferencia";
        }
        else if (panel == 5) {//Debito
            $(panelActivo + "#divBanco").show();
            $(panelActivo + "#divTarjeta").show();
            $(panelActivo + "#ddlTarjetaCredito option").each(function (index, item) {
                if (item.text.toUpperCase() != "REDCOMPRA")
                    item.disabled = true;
                else
                    item.selected = true;
            });
            $(panelActivo + "#divNumero").show();
        }
        else if (panel == 6) { //Vale Vista
            $(panelActivo + "#divBanco").show();
            $(panelActivo + "#ddlBanco").val("012");//Banco Estado de Chile por defecto
            $(panelActivo + "#divNumero").show();
        }
        else {
            esError = true;
            Mensaje.Alerta("Ocurrió un problema. Vuelva a intentarlo mas tarde.");
        }

        if (!esError) {
            $(panelActivo + "#txtMontoPagar").val(pagoBase.pagoDiferencia);
            PDV.SoloNumeroFormato($(panelActivo + "#txtMontoPagar")[0]);
            $(panelActivo + "#txtMontoPagar").blur()

            $(panelActivo + "#ModalMedioPago").modal();
        }
    },
    MedioPagoLimpiar: function () {
        var fecha = $(panelActivo + "#txtFechaPago")[0].dataset.fecha;
        while (fecha.includes("-")) { fecha = fecha.replace("-", "/"); }

        $(panelActivo + "#txtFechaPago").val(fecha);
        $(panelActivo + "#txtMontoPagar").val("");
        $(panelActivo + "#txtMontoPagar")[0].dataset.valor = "0";

        $(panelActivo + "#txtNumero").val("");
        $(panelActivo + "#ddlTarjetaCredito").val("");
        $(panelActivo + "#ddlTarjetaCredito option").each(function (index, item) {
            item.disabled = false;
        });
        $(panelActivo + "#ddlBanco").val("");
        $(panelActivo + "#txtMontoRedondeo").val("0");
        $(panelActivo + "#txtMontoPagoEfectivo").val("0");
        $(panelActivo + "#txtMontoPagoVuelto").val("0");

        $(panelActivo + "#divMontoRedondeo").hide();
        $(panelActivo + "#divMontoPagoEfectivo").hide();
        $(panelActivo + "#divMontoPagoVuelto").hide();
        $(panelActivo + "#divTarjeta").hide();
        $(panelActivo + "#divBanco").hide();
        $(panelActivo + "#divNumero").hide();
        $(panelActivo + "#divFecha").hide();

        $(panelActivo + "#formaPagoSeleccionada").val("");
    },
    MedioPagoAgregar: function () {
        var panel = $(panelActivo + "#formaPagoSeleccionada").val();
        if (!MedioPago.MedioPagoAgregarValidar(panel)) {
            var filaFormaPago = panel == 1 ? "Efectivo" :
                (panel == 2 ? "Tarjeta de Crédito" :
                    (panel == 3 ? "Cheque" :
                        (panel == 4 ? "Transferencia Bancaria" :
                            (panel == 5 ? "Tarjeta de Débito" : "Vale Vista"))));

            var filaOrigen = $(panelActivo + "#ddlTarjetaCredito").val() == "" ? "--" : $(panelActivo + '#ddlTarjetaCredito')[0].options[$(panelActivo + '#ddlTarjetaCredito')[0].selectedIndex].text;
            var filaBanco = $(panelActivo + "#ddlBanco").val() == "" ? "--" : $(panelActivo + '#ddlBanco')[0].options[$(panelActivo + '#ddlBanco')[0].selectedIndex].text
            var filaNumero = $(panelActivo + "#txtNumero").val() == "" ? "--" : $(panelActivo + "#txtNumero").val();
            var filaFecha = (panel == 1 || panel == 2 || panel == 5) || $(panelActivo + "#txtFechaPago").val() == "" ? "--" : $(panelActivo + "#txtFechaPago").val();

            var montoPagar = $(panelActivo + '#txtMontoPagar').val();
            var montoIngresado = "$ " + montoPagar;

            while (montoPagar.includes(".") || montoPagar.includes(",")) {
                montoPagar = montoPagar.replace(".", "");
                montoPagar = montoPagar.replace(",", "");
            }

            var fecha = $(panelActivo + "#txtFechaPago").val();
            var numero = $(panelActivo + "#txtNumero").val();
            var codigoBanco = $(panelActivo + "#ddlBanco").val();
            var codigoTarjeta = $(panelActivo + "#ddlTarjetaCredito").val();
            var montoFinal = parseInt(montoPagar);
            var montoRedondeo = parseInt($(panelActivo + "#txtMontoRedondeo").val());

            var fila = pagoBase.tablaMedioPago.data().length + 1;
            var valorOculto = 'data-panel="' + panel + '" ';
            valorOculto += 'data-monto="' + montoFinal + '" ';
            valorOculto += 'data-fila="' + fila + '" ';

            if (montoRedondeo != 0 && panel == 1) { //Redondeo
                valorOculto += 'data-filaRedondeo="' + fila + '" ';
            }

            var accionEliminar = '<button type="button" class="eliminar_registro btn btn-primary btn-xs" id="btnEliminarRegistro" ' + valorOculto + ' onclick="MedioPago.MedioPagoEliminar(this);">Eliminar</button>';

            if (panel == 1) {
                pagoBase.montoFinalEfectivo += montoFinal;
            }
            else if (panel == 2 || panel == 5) {
                pagoBase.montoFinalTarjeta += montoFinal;
            }
            else if (panel == 3 || panel == 6) {
                pagoBase.montoFinalCheque += montoFinal;
            }
            else if (panel == 4) {
                pagoBase.fechaTransferencia = fecha;
                pagoBase.numeroTransferencia = numero;
                pagoBase.bancoTransferencia = codigoBanco,
                    pagoBase.montoFinalTransferencia += montoFinal;
            }

            if (panel == 2 || panel == 3 || panel == 5 || panel == 6) {
                var transaccion = {
                    fila: fila,

                    //Cheque
                    NumeroCheque: (panel == 3 || panel == 6 ? parseInt(numero) : 0),
                    MontoCheque: (panel == 3 || panel == 6 ? montoFinal : 0),
                    FechaVencimientoCheque: (panel == 3 || panel == 6) ? fecha : null,
                    EsValeVista: (panel == 6),

                    //Tarjeta
                    CodigoTarjeta: codigoTarjeta,
                    NumeroOperacionTarjeta: (panel == 2 || panel == 5 ? numero : 0),
                    NumeroTajeta: 0,
                    MontoTarjeta: (panel == 2 || panel == 5 ? montoFinal : 0),
                    NumeroCuotasTarjeta: 1,

                    //Ambos
                    CodigoBanco: codigoBanco,
                }

                pagoBase.lstPagos.push(transaccion);
            }

            pagoBase.tablaMedioPago.row.add([filaFormaPago, filaOrigen, filaBanco, filaFecha, filaNumero, montoIngresado, accionEliminar]).draw();
            MedioPago.PagoCargaFooter(montoFinal, true, false);

            if (montoRedondeo != 0 && panel == 1) { //Redondeo
                pagoBase.tablaMedioPago.row.add(["Ley de Redondeo", filaOrigen, filaBanco, filaFecha, filaNumero, "$ " + montoRedondeo, '']).draw();
                MedioPago.PagoCargaFooter(montoRedondeo, true, true);
            }

            $(panelActivo + "#ModalMedioPago").modal('hide');
        }
    },
    MedioPagoEliminar: function (item) {
        var fila = item.dataset.fila;

        var panel = item.dataset.panel;
        var monto = parseInt(item.dataset.monto);

        if (panel == 1) {
            pagoBase.montoFinalEfectivo = pagoBase.montoFinalEfectivo - monto;
        }
        else if (panel == 2 || panel == 5) {
            pagoBase.montoFinalTarjeta = pagoBase.montoFinalTarjeta - monto;
        }
        else if (panel == 3 || panel == 6) {
            pagoBase.montoFinalCheque = pagoBase.montoFinalCheque - monto;
        }
        else if (panel == 4) {
            pagoBase.fechaTransferencia = "";
            pagoBase.numeroTransferencia = "";
            pagoBase.bancoTransferencia = "";
            pagoBase.montoFinalTransferencia = pagoBase.montoFinalTransferencia - monto;
        }

        for (var i = 0, j = 0; i < pagoBase.lstPagos.length; i++) {
            if (pagoBase.lstPagos[i].fila == fila) {
                pagoBase.lstPagos.splice(i, 1);
            }
        }
        var esRedondeo = false;
        if (item.dataset.filaredondeo != undefined || item.dataset.filaredondeo != null) { //Redondeo
            pagoBase.tablaMedioPago
                .row(item.dataset.filaredondeo)
                .remove()
                .draw();
            esRedondeo = true;
        }
        pagoBase.tablaMedioPago
            .row($(item).parents('tr'))
            .remove()
            .draw();

        MedioPago.PagoCargaFooter(monto, false, esRedondeo);
    },
    MedioPagoAgregarValidar: function (panel) {
        var esError = false;

        var montoRedondeo = parseInt($(panelActivo + "#txtMontoRedondeo").val());
        var montoPagar = $(panelActivo + "#txtMontoPagar").val();
        while (montoPagar.includes(".") || montoPagar.includes(",")) {
            montoPagar = montoPagar.replace(".", "");
            montoPagar = montoPagar.replace(",", "");
        }

        if ((panel == 2 || panel == 5) && $(panelActivo + "#ddlTarjetaCredito").val() == "") {
            Mensaje.Alerta("Debe seleccionar una tarjeta antes de continuar.");
            esError = true;
        }
        else if ((panel == 2 || panel == 3 || panel == 4 || panel == 5 || panel == 6) && $(panelActivo + "#ddlBanco").val() == "") {
            Mensaje.Alerta("Debe seleccionar un banco antes de continuar.");
            esError = true;
        }
        else if ((panel == 2 || panel == 3 || panel == 4 || panel == 5 || panel == 6) && $(panelActivo + "#txtNumero").val() == "") {
            Mensaje.Alerta("Debe ingresar el n° solicitado antes de continuar.");
            esError = true;
        }
        else if (panel == 3 && $(panelActivo + "#txtFechaPago").val() == "") {
            Mensaje.Alerta("Debe ingresar fecha de vencimiento antes de continuar.");
            esError = true;
        }
        else if (panel == 4 && $(panelActivo + "#txtFechaPago").val() == "") {
            Mensaje.Alerta("Debe ingresar fecha de la transferencia antes de continuar.");
            esError = true;
        }
        else if (montoPagar == "") {
            Mensaje.Alerta("Debe ingresar el monto a pagar antes de continuar.");
            esError = true;
        }
        else if (isNaN(parseInt(montoPagar))) {
            Mensaje.Alerta("Debe ingresar un monto a pagar válido.");
            esError = true;
        }
        else if (parseInt(montoPagar) == 0) {
            Mensaje.Alerta("Debe ingresar un monto a pagar mayor a cero.");
            esError = true;
        }
        else {
            var montoIngresado = parseInt(montoPagar);
            if ((montoRedondeo == 0 && montoIngresado > pagoBase.pagoDiferencia) ||
                (montoRedondeo != 0 && montoIngresado > (pagoBase.pagoDiferencia + montoRedondeo))) {
                Mensaje.Alerta("El monto a pagar no puede ser mayor al pago pendiente.");
                esError = true;
            }
        }

        return esError;
    },
    MedioPagoRedondeo: function (item) { //Redondeo
        var esError = false;
        var montoPagar = 0;
        var ultimoDigito = 0;

        if (item.dataset.valor != item.value) {
            while (item.value.includes(".") || item.value.includes(",")) {
                item.value = item.value.replace(".", "");
                item.value = item.value.replace(",", "");
            }
            montoPagar = parseInt(item.value);
            ultimoDigito = parseInt(item.value.substring(item.value.length - 1));


            if (ultimoDigito != 0) {
                if (montoPagar != pagoBase.pagoDiferencia) {
                    Mensaje.Alerta("Debe ingresar un monto con el redondeo aplicado.");
                    item.value = pagoBase.pagoDiferencia;
                    PDV.SoloNumeroFormato(item);

                    MedioPago.MedioPagoRedondeo(item);
                    esError = true;
                }
                else if (pagoBase.lstPagos.length > 0) {
                    Mensaje.Alerta("Debe ingresar el monto $ " + ultimoDigito + " al medio de pago agregado anteriormente.");
                    item.value = montoPagar - ultimoDigito;
                    PDV.SoloNumeroFormato(item);
                    esError = true;
                }

                if (ultimoDigito <= 5) {
                    ultimoDigito = -1 * ultimoDigito;
                }
                else {
                    ultimoDigito = (10 - ultimoDigito);
                }
            }

            if (!esError) {
                montoPagar = montoPagar + ultimoDigito;

                item.value = montoPagar;
                PDV.SoloNumeroFormato(item);

                item.dataset.valor = item.value;

                $(panelActivo + "#txtMontoRedondeo").val(ultimoDigito);
            }
        }
    },
}
