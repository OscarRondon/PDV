$(document).ready(function () {
    CrearLimiteModal.Init();
});
var CrearLimiteModal = {
    Init: function () {
        $('#modalAgregarLimite').on('hidden.bs.modal', function (e) {
            CrearLimiteModal.LimpiarFormulario();
        });

        $('input[name=rngLimiteAnulado]').on('change', function (e, item) {
            if ($("input[name=rngLimiteAnulado]:checked").val() == "S") {
                $("#divFolioLimiteMotivo").show();
            }
            else {
                $("#divFolioLimiteMotivo").hide();
            }
        });
    },
    ObtenerUltimoLimiteFolio: function () {
        $.ajax({
            url: '/Folio/ObtenerUltimoLimiteFolio',
            type: 'GET',
            cache: false,
            contentType: 'application/json; charset=utf-8',
            success:
                function (result) {
                    if (!result.EsError) {
                        $('#txtFolioInicialLimite').val(result.Data);

                        if (result.Data == 1) {
                            $('#txtFolioInicialLimite').removeAttr("readonly");
                        }
                    }
                    else {
                        var mensaje = result.Mensaje;
                        Mensaje.Alerta(mensaje);
                    }
                }
        });
    },
    EditarFormulario: function (item) {
        $("#tituloModalCrearLimite")[0].innerText = "Editar";

        $("#LimiteFolioId").val(item.dataset.idfolio);

        if (item.dataset.folioinicial !== "") $('#txtFolioInicialLimite').val(item.dataset.folioinicial);
        if (item.dataset.foliofinal !== "") $('#txtFolioFinalLimite').val(item.dataset.foliofinal);
        if (item.dataset.anulado !== "") $("input[name=rngLimiteAnulado][value=" + item.dataset.anulado + "]")[0].checked = true;
        if (item.dataset.motivo !== "") {
            $("#ddlLimiteFolioMotivoAnulacion option").each(function (index, opt) {
                if (opt.text == item.dataset.motivo) {
                    opt.selected = true;
                }
            });
        }
        if ($("input[name=rngLimiteAnulado][value=N]")[0].checked) {
            $("#divFolioLimiteMotivo").hide();
        }
        else {
            $("#divFolioLimiteMotivo").show();
        }

        if (item.dataset.ultimofolio == "N") { //Solo dejamos abierto el último folio para edición
            $('#txtFolioFinalLimite').attr("readonly", "readonly");
        }

        $('#btnLimpiarLimiteFolio').hide();

        $('#modalAgregarLimite').modal("show");
    },
    GuardarFormulario: function () {
        if (CrearLimiteModal.ValidaFormulario()) {
            Mensaje.Confirmar("¿Está seguro de guardar el rango de folio?", CrearLimiteModal.GuardarFolio);
        }
    },
    GuardarFolio: function () {
        var jsonFolio = {
            IdFolio: $('#LimiteFolioId').val(),
            FolioInicial: $('#txtFolioInicialLimite').val(),
            FolioFinal: $('#txtFolioFinalLimite').val(),

            EsAnulado: ($("input[name=rngLimiteAnulado]:checked").val() == "S"),
            MotivoAnulacion: ($("input[name=rngLimiteAnulado]:checked").val() == "S") ? $("#ddlLimiteFolioMotivoAnulacion")[0].options[$("#ddlLimiteFolioMotivoAnulacion")[0].selectedIndex].text : null,
        };
        var postData = { folio: jsonFolio };
        $.ajax({
            url: '/Folio/GuardarLimiteFolio',
            type: 'POST',
            dataType: 'json',
            cache: false,
            data: JSON.stringify(postData),
            contentType: 'application/json; charset=utf-8',
            success: function (data) {
                var mensaje = data.Mensaje;

                if (!data.EsError) {
                    $('#modalAgregarLimite').modal("hide");
                    Mensaje.CorrectoF(mensaje, LimiteFolio.BuscarLimiteFolios);
                }
                else {
                    Mensaje.Alerta(mensaje);
                }
            }
        });
    },
    ValidaFormulario: function () {
        var valido = false;
        if ($('#txtFolioInicialLimite').val() === "" || isNaN($('#txtFolioInicialLimite').val())) {
            Mensaje.Alerta('Debe ingresar folio inicial válido.');
        }
        else if ($('#txtFolioFinalLimite').val() === "" || isNaN($('#txtFolioFinalLimite').val())) {
            Mensaje.Alerta('Debe ingresar folio final válido.');
        }
        else if ($("input[name=rngLimiteAnulado][value=S]")[0].checked && $("#ddlLimiteFolioMotivoAnulacion").val() == "") {
            Mensaje.Alerta("Debe seleccionar un motivo por el cual desea anular el rango de folio ingresado.");
        }
        else {
            var fIni = parseInt($('#txtFolioInicialLimite').val());
            var fFin = parseInt($('#txtFolioFinalLimite').val());
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
        CrearLimiteModal.LimpiarFormulario();
        CrearLimiteModal.ObtenerUltimoLimiteFolio();
    },
    LimpiarFormulario: function () {
        $("#tituloModalCrearLimite")[0].innerText = "Agregar";
        $('#LimiteFolioId').val("");
        $('#LimiteFolioGuardado').val("0");

        $('#txtFolioInicialLimite').val("");
        $('#txtFolioInicialLimite').attr("readonly", "readonly");
        $('#txtFolioFinalLimite').val("");
        $("input[name=rngLimiteAnulado][value=N]")[0].checked = true;
        $("#ddlLimiteFolioMotivoAnulacion").val("");

        $('#btnLimpiarLimiteFolio').show();
        $("#divFolioLimiteMotivo").hide();
    },
}