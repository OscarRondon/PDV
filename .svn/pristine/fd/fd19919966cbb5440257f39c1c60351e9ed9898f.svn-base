$(document).ready(function () {
    CrearFolioModal.Init();
});
var CrearFolioModal = {
    Init: function () {
        $('#modalAgregarFolio').on('hidden.bs.modal', function (e) {
            CrearFolioModal.LimpiarFormulario();
            if ($("#folioGuardado").val() == "1") {
                $("#folioGuardado").val("0");
                Folio.BuscarFolios();
            }
        });
    },
    ObtenerUltimoFolio: function () {
        $.ajax({
            url: '/Folio/ObtenerUltimoFolio',
            type: 'GET',
            cache: false,
            contentType: 'application/json; charset=utf-8',
            success:
                function (result) {
                    if (!result.EsError) {
                        $('#txtFolioInicialNuevo').val(result.Data);
                    }
                    else {
                        var mensaje = result.Mensaje;
                        Mensaje.Alerta(mensaje);
                    }
                }
        });
    },
    GuardarFormulario: function () {
        if (CrearFolioModal.ValidaFormulario()) {
            Mensaje.Confirmar("¿Está seguro de guardar el rango de folio?", CrearFolioModal.GuardarFolio);
        }
    },
    GuardarFolio: function () {
        var jsonFolio = {
            IdFolio: $('#idFolio').val(),
            TipoDocumento: $('#ddlTipoDocumento').val(),
            IdUsuarioSupervisor: $('#ddlUsuarioAsignado').val(),
            NombreUsuarioSupervisor: $('#ddlUsuarioAsignado')[0].options[$('#ddlUsuarioAsignado')[0].selectedIndex].text.toUpperCase(),
            FolioInicial: $('#txtFolioInicialNuevo').val(),
            FolioFinal: $('#txtFolioFinalNuevo').val(),
        };
        var postData = { folio: jsonFolio };
        $.ajax({
            url: '/Folio/GuardarFolioNuevo',
            type: 'POST',
            dataType: 'json',
            cache: false,
            data: JSON.stringify(postData),
            contentType: 'application/json; charset=utf-8',
            success: function (data) {
                var mensaje = data.Mensaje;

                if (!data.EsError) {
                    if ($('#idFolio').val() == "") {
                        Mensaje.Correcto(mensaje);
                        $("#folioGuardado").val("1");
                        CrearFolioModal.Limpiar();
                    }
                    else {
                        $('#modalAgregarFolio').modal("hide");
                        Mensaje.CorrectoF(mensaje, Folio.BuscarFolios);
                    }
                }
                else {
                    Mensaje.Alerta(mensaje);
                }
            }
        });
    },
    ValidaFormulario: function () {
        var valido = false;
        if ($('#ddlTipoDocumento').val() === "") {
            Mensaje.Alerta('Debe seleccionar documento.');
        }
        else if ($('#txtFolioInicialNuevo').val() === "" || isNaN($('#txtFolioInicialNuevo').val())) {
            Mensaje.Alerta('Debe ingresar folio inicial válido.');
        }
        else if ($('#txtFolioFinalNuevo').val() === "" || isNaN($('#txtFolioFinalNuevo').val())) {
            Mensaje.Alerta('Debe ingresar folio final válido.');
        }
        else if ($('#ddlUsuarioAsignado').val() === "") {
            Mensaje.Alerta('Debe seleccionar usuario supervisor.');
        }
        else {
            var fIni = parseInt($('#txtFolioInicialNuevo').val());
            var fFin = parseInt($('#txtFolioFinalNuevo').val());
            if (fIni == fFin || fIni > fFin) {
                Mensaje.Alerta('Folio final debe ser mayor a folio inicial.');
            }
            else {
                valido = true;
            }
        }
        return valido;


    },
    Limpiar: function () {
        CrearFolioModal.LimpiarFormulario();
        CrearFolioModal.ObtenerUltimoFolio();
    },
    LimpiarFormulario: function () {
        $("#tituloModalCrearFolio")[0].innerText = "Agregar";
        $('#idFolio').val("");
        $('#txtFolioInicialNuevo').val("");
        $('#txtFolioFinalNuevo').val("");

        $('#ddlTipoDocumento').val("");
        $('#ddlUsuarioAsignado').val("");

        $('#ddlTipoDocumento').removeAttr("disabled");
        $('#txtFolioFinalNuevo').removeAttr("readonly");

        $('#txtFolioInicialNuevo').attr("readonly", "readonly");

        $('#btnLimpiarFolioNuevo').show();
    },
    EditarFormulario: function (item) {
        $("#idFolio").val(item.dataset.idfolio);

        $("#tituloModalCrearFolio")[0].innerText = "Editar";

        if (item.dataset.folioinicial !== "") $('#txtFolioInicialNuevo').val(item.dataset.folioinicial);
        if (item.dataset.foliofinal !== "") $('#txtFolioFinalNuevo').val(item.dataset.foliofinal);
        if (item.dataset.tipodocumento !== "") $('#ddlTipoDocumento').val(item.dataset.tipodocumento);
        if (item.dataset.idusuariosupervisor !== "") $('#ddlUsuarioAsignado').val(item.dataset.idusuariosupervisor);

        $('#ddlTipoDocumento').attr("disabled", "disabled");
        if (item.dataset.ultimofolio == "N") { //Solo dejamos abierto el último folio para edición
            $('#txtFolioFinalNuevo').attr("readonly", "readonly");
        }

        $('#btnLimpiarFolioNuevo').hide();

        $('#modalAgregarFolio').modal("show");
    }
}