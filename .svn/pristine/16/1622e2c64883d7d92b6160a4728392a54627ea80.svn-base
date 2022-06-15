var tipoAnular = "";

$(document).ready(function () {
    AnularFolioRango.Init();
});
var AnularFolioRango = {
    Init: function () {
        $('#modalAnularFolioRango').on('hidden.bs.modal', function (e) {
            $("#hdnIdFolioAnulacion").val("");
            $("#hdnFolioIniAnu").val("");
            $("#hdnFolioFinAnu").val("");
            $("#hdnIdCajaAnulacion").val("");

            $("#txtFolioInicialAnular").val("");
            $("#txtFolioFinalAnular").val("");
            $("#ddlMotivoAnulacion").val("");
            tipoAnular = "";
        });
        AnularFolioRango.ObtenerMotivosAnulacion();

    },
    AbrirFormulario: function (item) {
        $("#hdnIdFolioAnulacion").val(item.dataset.idfolio);
        if (item.dataset.idcaja !== "") $('#hdnIdCajaAnulacion').val(item.dataset.idcaja);

        $("#hdnFolioIniAnu").val(item.dataset.folioinicial);
        $("#hdnFolioFinAnu").val(item.dataset.foliofinal);

        $("#txtFolioInicialAnular").val(item.dataset.folioinicial);
        $("#txtFolioFinalAnular").val(item.dataset.foliofinal);

        tipoAnular = item.dataset.anular;

        $('#modalAnularFolioRango').modal("show");
    },
    AnularConfirmacion: function () {
        if (AnularFolioRango.ValidarAnualacionFolio()) {
            Mensaje.Confirmar("¿Esta seguro que desea anular el rango de folio seleccionado?", AnularFolioRango.GuardarAnulacionFolio);
        }
    },
    ValidarAnualacionFolio: function () {
        var valido = false;
        if ($('#txtFolioInicialAnular').val() === "" || isNaN($('#txtFolioInicialAnular').val())) {
            Mensaje.Alerta('Debe ingresar folio inicial válido.');
        }
        else if ($('#txtFolioFinalAnular').val() === "" || isNaN($('#txtFolioFinalAnular').val())) {
            Mensaje.Alerta('Debe ingresar folio final válido.');
        }
        else if ($("#ddlMotivoAnulacion").val() == "") {
            Mensaje.Alerta("Debe seleccionar un motivo por el cual desea anular el rango de folio seleccionado.");
        }
        else {
            var fIniP = parseInt($('#hdnFolioIniAnu').val());
            var fFinP = parseInt($('#hdnFolioFinAnu').val());

            var fIni = parseInt($('#txtFolioInicialAnular').val());
            var fFin = parseInt($('#txtFolioFinalAnular').val());
            if (fIni > fFin) {
                Mensaje.Alerta('Folio final debe ser mayor a folio final.');
            }
            else if (fIni < fIniP || fIni > fFinP) {
                Mensaje.Alerta('Debe ingresar un folio inicial dentro del rango ' + fIniP + '-' + fFinP + '.');
            }
            else if (fFin < fIniP || fFin > fFinP) {
                Mensaje.Alerta('Debe ingresar un folio final dentro del rango ' + fIniP + '-' + fFinP + '.');
            }
            else {
                valido = true;
            }
        }
        return valido;
    },
    GuardarAnulacionFolio: function () {
        var jsonFolio = {
            IdFolio: $('#hdnIdFolioAnulacion').val(),
            Motivo: $("#ddlMotivoAnulacion")[0].options[$("#ddlMotivoAnulacion")[0].selectedIndex].text,
            FolioInicial: $('#txtFolioInicialAnular').val(),
            FolioFinal: $('#txtFolioFinalAnular').val(),
            IdCaja: $('#hdnIdCajaAnulacion').val()
        };
        var postData = { folio: jsonFolio };
        $.ajax({
            url: '/Folio/AnularFolio',
            type: 'POST',
            dataType: 'json',
            cache: false,
            data: JSON.stringify(postData),
            contentType: 'application/json; charset=utf-8',
            success: function (data) {
                var mensaje = data.Mensaje;
                if (!data.EsError) {
                    if (tipoAnular == "1") { // Jefe de Admision - Folio
                        Mensaje.CorrectoF(mensaje, Folio.BuscarFolios());
                    }
                    else if (tipoAnular == "2") { // Supervisor - Segmento
                        Mensaje.CorrectoF(mensaje, FolioSupervisor.BuscarFoliosSupervisor());
                    }
                    else if (tipoAnular == "3") { // Admisionista - Segmento
                        Mensaje.CorrectoF(mensaje, FolioAdmisionistaAnular.Init());
                    }
                    $('#modalAnularFolioRango').modal("hide");
                }
                else {
                    Mensaje.Alerta(mensaje);
                }
            }
        });
    },
    ObtenerMotivosAnulacion: function () {
        $.ajax({
            url: '/Folio/ObtenerMotivoAnulacion',
            type: 'GET',
            dataType: 'json',
            cache: false,
            contentType: 'application/json; charset=utf-8',
            success: function (result) {
                if (!result.EsError) {
                    var info = result.Data;
                    if (info.length > 0) {
                        $("#ddlMotivoAnulacion").append('<option value="">Seleccione...</option>');
                        $(info).each(function (index, item) {
                            $("#ddlMotivoAnulacion").append('<option value="' + item.Codigo + '">' + item.Descripcion + '</option>');
                        });
                    }
                    else {
                        $("#ddlMotivoAnulacion").append('<option value="">Sin datos...</option>');
                    }
                }
                else {
                    $("#ddlMotivoAnulacion").append('<option value="">Sin datos...</option>');
                }
            }
        });
    },
}