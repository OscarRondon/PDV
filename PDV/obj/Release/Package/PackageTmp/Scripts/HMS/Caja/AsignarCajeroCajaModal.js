$(document).ready(function () {
    AsignarCajeroModal.Init();
});

var AsignarCajeroModal = {
    Init: function () {
        $('#btnAsignarFolio').click(function () {
            mLoading.modal();
            window.location.href = "/Folio/AdministracionFolioSupervisor";
        });

        $('#modalAsignarCajeroCaja').on('hidden.bs.modal', function (e) {
            AsignarCajeroModal.LimpiarFormulario();
        })
        $('#modalAsignarCajeroCaja').on('shown.bs.modal', function (e) {
            var idCaja = $("#idCaja").val();
            if (idCaja != "") {
                AsignarCajeroModal.BuscarFolioCaja(idCaja);
            }
            else {
                Mensaje.Alerta("Ocurrió un problema al obtener la caja. Vuelva a intentarlo mas tarde.");
            }
        });
        $('#ddlFuncionalidadCaja').change(function (e) {
            $(".spnAsterisco").each(function (index, item) { item.hidden = ($('#ddlFuncionalidadCaja').val() != "C"); });
        });
    },
    BuscarFolioCaja: function (idCaja) {
        $.ajax({
            url: '/Caja/TraerFolioCaja',
            type: 'GET',
            cache: false,
            data: { idCaja: idCaja },
            contentType: 'application/json; charset=utf-8',
            success: function (data) {
                if (data.EsError) {
                    Mensaje.Alerta("La caja seleccionada no tiene folio asignado.");
                }
                else {
                    $("#tieneFolio").val("1");

                    var foliosCaja = (data.Data.FolioInicial + ' - ' + data.Data.FolioFinal);
                    var foliosRestantes = ((parseInt(data.Data.FolioFinal) - parseInt(data.Data.FolioActual)) + 1);
                    $('#txtFolioAsignados').val(foliosCaja);
                    $('#txtFolioRestantes').val(foliosRestantes);
                }
            }
        });
    },
    GuardarFormulario: function () {
        if (AsignarCajeroModal.ValidarFormulario()) {
            Mensaje.Confirmar("¿Está seguro de guardar los cambios?", AsignarCajeroModal.GuardarAsignacion);
        }
    },
    GuardarAsignacion: function () {
        var monto = "0";
        if ($('#txtMontoFijo').val() != "") {
            monto = $('#txtMontoFijo').val();
            while (monto.includes(".")) {
                monto = monto.replace(".", "");
            }
        }
        var funcionalidadId = null;
        var funcionalidadNombre = null;
        if ($("#ddlFuncionalidadCaja").val() != $('#hdnFuncionalidadCajaCambio').val()) {
            funcionalidadId = $("#ddlFuncionalidadCaja").val();
            funcionalidadNombre = $("#ddlFuncionalidadCaja")[0].options[$("#ddlFuncionalidadCaja")[0].selectedIndex].text;
        }

        var jsonCaja = {
            CajaAsignada: {
                IdCaja: $('#idCaja').val(),
                IdFuncionalidadCaja: funcionalidadId,
                FuncionalidadCaja: funcionalidadNombre
            },
            IdAsignacion: $("#idAsignacion").val(),
            IdUsuarioAsignado: $('#UsuarioAsignado').val(),
            UsuarioAsignado: $('#UsuarioAsignado')[0].options[$('#UsuarioAsignado')[0].selectedIndex].text.toUpperCase(),
            MontoFijo: monto
        };
        var postData = { asignacion: jsonCaja };
        $.ajax({
            url: '/Caja/GuardarCajaAsignadaCajero',
            type: 'POST',
            dataType: 'json',
            cache: false,
            data: JSON.stringify(postData),
            contentType: 'application/json; charset=utf-8',
            success: function (data) {
                var mensaje = data.Mensaje;
                if (data.EsError) {
                    Mensaje.Alerta(mensaje);
                }
                else {
                    $("#modalAsignarCajeroCaja").modal("hide");
                    Mensaje.CorrectoF(mensaje, AdministraCajaSupervisor.ReloadCajas);
                }
            }
        });
    },
    ValidarFormulario: function () {
        if ($("#idCaja").val().length == 0) {
            Mensaje.Alerta('Debe ingresar identificación de caja.');
            return false;
        }
        if ($("#ddlFuncionalidadCaja").val() === "" || $("#ddlFuncionalidadCaja").val() == null) {
            Mensaje.Alerta('Debe seleccionar una funcionalidad.');
            return false;
        }
        if ($("#UsuarioAsignado").val() === "") {
            Mensaje.Alerta('Debe seleccionar un usuario.');
            return false;
        }
        if (AsignarCajeroModal.ValidarFormularioMonto()) {
            return false;
        }
        if ($("#ddlFuncionalidadCaja").val() == "C") { // Si es Caja valida Folios.
            if ($("#tieneFolio").val() == "0") {
                Mensaje.Alerta('Debe asignar folio a la caja antes de continuar.');
            }
            else if ($('#txtFolioAsignados').val() == "" || $('#txtFolioAsignados').val().trim() == "" || $('#txtFolioAsignados').val() == "0") {
                Mensaje.Alerta('Debe asignar folio a la caja antes de continuar.');
            }
            else if ($('#txtFolioRestantes').val() == "" || $('#txtFolioRestantes').val().trim() == "" || $('#txtFolioRestantes').val() == "0") {
                Mensaje.Alerta('Debe asignar folios restantes a la caja antes de continuar.');
            }
            else {
                return true;
            }

            return false;
        }

        return true;
    },
    ValidarFormularioMonto: function () {
        if ($('#txtMontoFijo').val() == "" || $('#txtMontoFijo').val().trim() == "") {
            if ($("#ddlFuncionalidadCaja").val() == "C") { // Si es caja validamos monto fijo...
                Mensaje.Alerta('Debe ingresar monto fijo.');
                return true;
            }
        }
        else {
            var valor = $('#txtMontoFijo').val();
            while (valor.includes(".")) {
                valor = valor.replace(".", "");
            }
            if (isNaN(valor)) {
                Mensaje.Alerta('El monto fijo ingresado no es válido.');
                return true;
            }
            else {
                var monto = Number(valor);
                if (monto == NaN) {
                    Mensaje.Alerta("El monto fijo ingresado es superior a $ 999.999.999.999.");
                    return true;
                }
            }
        }
        return false;
    },
    LimpiarFormulario: function () {
        $("#idCaja").val("");
        $("#idAsignacion").val("");
        $("#tieneFolio").val("0");

        $('#txtNombreCaja').val("");
        $('#ddlFuncionalidadCaja').val("");
        $('#hdnFuncionalidadCajaCambio').val("");
        $('#txtFuncionCaja').val("");
        $('#txtSeccionCaja').val("");

        $('#txtMontoFijo').val("");
        $('#txtFolioAsignados').val("");
        $('#txtFolioRestantes').val("");

        $('#UsuarioAsignado').val("");

        $('#hdnFuncionCaja').val("");

        $(".spnAsterisco").each(function (index, item) { item.hidden = true; });
    },
    EditarFormulario: function (item) {

        AsignarCajeroModal.LimpiarFormulario();
        $("#idCaja").val(item.dataset.idcaja);
        $("#idAsignacion").val(item.dataset.idasignacion);
        $('#txtNombreCaja').val(item.dataset.identificacioncaja);
        $('#hdnFuncionCaja').val(item.dataset.idfuncioncaja);

        if (item.dataset.idfuncionalidadcaja !== "") {
            $('#ddlFuncionalidadCaja').val(item.dataset.idfuncionalidadcaja);
            $('#hdnFuncionalidadCajaCambio').val(item.dataset.idfuncionalidadcaja);

            if (item.dataset.idfuncionalidadcaja == "C") {
                $(".spnAsterisco").each(function (index, item) { item.hidden = false; });
            }
            else {
                $(".spnAsterisco").each(function (index, item) { item.hidden = true; });
            }
        }
        if (item.dataset.funcioncaja !== "") $('#txtFuncionCaja').val(item.dataset.funcioncaja);
        if (item.dataset.seccioncaja !== "") $('#txtSeccionCaja').val(item.dataset.seccioncaja);
        if (item.dataset.montofijo !== "") {
            $('#txtMontoFijo').val(item.dataset.montofijo);
            PDV.SoloNumeroFormato($("#txtMontoFijo")[0]);
        }
        if (item.dataset.usuario !== "") $('#UsuarioAsignado').val(item.dataset.usuario);

        $('#modalAsignarCajeroCaja').modal("show");
    }
}