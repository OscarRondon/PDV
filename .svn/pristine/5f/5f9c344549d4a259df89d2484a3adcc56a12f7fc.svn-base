$(document).ready(function () {
    CreaCajaModal.Init();
});
var CreaCajaModal = {
    Init: function () {
        $('#modalAgregarCaja').on('hidden.bs.modal', function (e) {
            CreaCajaModal.LimpiarFormulario();
            if ($("#cajaGuardado").val() == "1") {
                $("#cajaGuardado").val("0");
                ReloadCajas();
            }
        });
    },
    GuardarFormulario: function () {
        if (CreaCajaModal.ValidaFormulario()) {
            Mensaje.Confirmar("¿Está seguro de guardar los cambios?", CreaCajaModal.GuardarCaja);
        }
    },
    GuardarCaja: function () {
        var jsonCaja = {
            IdCaja: $('#idCaja').val() == "" ? null : $('#idCaja').val(),
            IdentificacionCaja: $('#NombreCaja').val().toUpperCase().trim(),
            IdFuncionalidadCaja: $('#FuncionalidadCaja').val(),
            FuncionalidadCaja: CreaCajaModal.GetTextSelected($('#FuncionalidadCaja')[0]),
            IdFuncionCaja: $('#FuncionCaja').val(),
            FuncionCaja: CreaCajaModal.GetTextSelected($('#FuncionCaja')[0]),
            IdSeccionCaja: $('#SeccionCaja').val(),
            SeccionCaja: CreaCajaModal.GetTextSelected($('#SeccionCaja')[0]),
            IdPisoCaja: $('#PisoCaja').val(),
            PisoCaja: CreaCajaModal.GetTextSelected($('#PisoCaja')[0]),
            HabilitaCaja: $('input[name=habilitaCaja]:checked').val(),
            IpCaja: $('#IpCaja').val().trim(),
            MacCaja: $('#MacCaja').val().trim(),

            IdSupervisorCaja: $('#UsuarioAsignado').val() == "" ? null : $('#UsuarioAsignado').val(),
            SupervisorCaja: $('#UsuarioAsignado').val() == "" ? null : CreaCajaModal.GetTextSelected($('#UsuarioAsignado')[0])
        };
        var postData = { caja: jsonCaja };
        $.ajax({
            url: '/Caja/GuardarCajaModal',
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
                    if ($('#idCaja').val() == "") {
                        Mensaje.Correcto(mensaje);
                        $("#cajaGuardado").val("1");
                        CreaCajaModal.LimpiarFormulario();
                    }
                    else {
                        $('#modalAgregarCaja').modal("hide");
                        Mensaje.CorrectoF(mensaje, ReloadCajas);
                    }
                }
            }
        });
    },
    ValidaFormulario: function () {
        var IpRegEx = /^(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)$/;
        if ($('#NombreCaja').val() === "" || $('#NombreCaja').val().trim() === "") {
            Mensaje.Alerta('Debe ingresar identificación de caja.');
        }
        else if ($('#FuncionalidadCaja').val() === "") {
            Mensaje.Alerta('Debe seleccionar funcionalidad de caja.');
        }
        else if ($('#FuncionCaja').val() === "") {
            Mensaje.Alerta('Debe seleccionar función de caja.');
        }
        else if ($('#SeccionCaja').val() === "") {
            Mensaje.Alerta('Debe seleccionar sección de caja.');
        }
        else if ($('#PisoCaja').val() === "") {
            Mensaje.Alerta('Debe seleccionar piso de caja.');
        }
        else if ($('#IpCaja').val() === "" || $('#IpCaja').val().trim === "") {
            Mensaje.Alerta('Debe ingresar IP de caja.');
        }
        else if (!$("#IpCaja").val().match(IpRegEx)) {
            Mensaje.Alerta('Formato de IP es incorrecto.');
        }
        //else if ($('#MacCaja').val() === "" || $('#MacCaja').val().trim === "") {
        //    Mensaje.Alerta('Debe ingresar MAC de caja.');
        //}
        else if ($('input[name=habilitaCaja]:checked').val() == null) {
            Mensaje.Alerta('Debe indicar si habilita caja.');
        }
        else {
            return true;
        }

        return false;
    },
    LimpiarFormulario: function () {
        $("#tituloModalCreaCaja")[0].innerText = "Agregar";
        $("#idCaja").val("");
        $('#NombreCaja').val("");
        $('#IpCaja').val("");
        $('#MacCaja').val("");

        $('input[name="habilitaCaja"]').each(function (index, item) {
            item.checked = false;
        });

        $('#FuncionalidadCaja').val("");
        $('#FuncionCaja').val("");
        $('#SeccionCaja').val("");
        $('#PisoCaja').val("");
        $('#UsuarioAsignado').val("");
    },
    EditarFormulario: function (item) {
        CreaCajaModal.LimpiarFormulario();

        $("#tituloModalCreaCaja")[0].innerText = "Editar";

        $("#idCaja").val(item.dataset.idcaja);

        $('#NombreCaja').val(item.dataset.identificacioncaja);
        $('#IpCaja').val(item.dataset.ipcaja);
        $('#MacCaja').val(item.dataset.maccaja);

        $('input[name="habilitaCaja"]').each(function (index, check) {
            if (check.value == item.dataset.habilitacaja) {
                check.checked = true;
            }
        });

        if (item.dataset.idfuncionalidadcaja != "") $('#FuncionalidadCaja').val(item.dataset.idfuncionalidadcaja);
        if (item.dataset.idfuncioncaja != "") $('#FuncionCaja').val(item.dataset.idfuncioncaja);
        if (item.dataset.idseccioncaja != "") $('#SeccionCaja').val(item.dataset.idseccioncaja);
        if (item.dataset.idpisocaja != "") $('#PisoCaja').val(item.dataset.idpisocaja);
        if (item.dataset.idsupervisorcaja !== "") $('#UsuarioAsignado').val(item.dataset.idsupervisorcaja);

        if ($('#FuncionalidadCaja')[0].selectedIndex == -1) $('#FuncionalidadCaja')[0].selectedIndex = 0;
        if ($('#FuncionCaja')[0].selectedIndex == -1) $('#FuncionCaja')[0].selectedIndex = 0;
        if ($('#SeccionCaja')[0].selectedIndex == -1) $('#SeccionCaja')[0].selectedIndex = 0;
        if ($('#PisoCaja')[0].selectedIndex == -1) $('#PisoCaja')[0].selectedIndex = 0;
        if ($('#UsuarioAsignado')[0].selectedIndex == -1) $('#UsuarioAsignado')[0].selectedIndex = 0;

        $('#modalAgregarCaja').modal("show");
    },
    GetTextSelected: function (sel) {
        return sel.options[sel.selectedIndex].text.toUpperCase();
    }
}