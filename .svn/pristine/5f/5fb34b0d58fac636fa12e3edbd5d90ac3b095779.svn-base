var documentosModificados = [];
var documentosPendientes = [];

var documentosFinal = [];
$(document).ready(function () {
    NominaDetalleSupervisor.Init();
});

var NominaDetalleSupervisor = {
    Init: function () {
        NominaDetalleSupervisor.CargarDetalleNomina([]);

        $(document).on("blur", ".comentario", function () {
            NominaDetalleSupervisor.AgregarComentario(this);
        });
    },
    BuscarNominaDetalle: function (nomina) {
        var postData = { docEntryNomina: nomina.docentry };
        $.ajax({
            url: '/Nomina/BuscarDetalleNominaSupervisor',
            type: 'POST',
            cache: false,
            data: JSON.stringify(postData),
            contentType: 'application/json; charset=utf-8',
            success: function (respuesta) {
                if (respuesta.EsError) {
                    Mensaje.Alerta(respuesta.Mensaje);
                    NominaPrincipalSupervisor.LimpiarNomina();
                }
                else {
                    $("#spnNumeroNomina")[0].innerHTML = nomina.numero;
                    $("#spnTipoNomina")[0].innerHTML = nomina.tipo;

                    if (respuesta.Data != null && respuesta.Data.DetalleNomina != null && respuesta.Data.DetalleNomina.length > 0) {
                        NominaDetalleSupervisor.CargarDetalleNomina(respuesta.Data.DetalleNomina);

                        for (var i = 0; i < respuesta.Data.DetalleNomina.length; i++) {
                            var detalle = respuesta.Data.DetalleNomina[i];
                            if (detalle.EstadoResponsable == "N" && detalle.EstadoSupervisor != "N" && detalle.EstadoSupervisor != "P") {
                                documentosPendientes.push(detalle);
                            }
                        }
                    }
                }
            }
        });
    },
    CargarDetalleNomina: function (data) {
        var modificar = false;
        $('#listaDetalleNomina').DataTable().destroy();
        $('#listaDetalleNomina').DataTable({
            "language": dataTableEspanol,
            "bFilter": true,
            "bInfo": true,
            "bLengthChange": true,
            "bPaginate": true,
            "bProcessing": false,
            "bSort": true,
            "bDestroy": true,
            "sScrollX": "100%",
            "bScrollCollapse": true,

            "order": [[0, 'asc']],

            "aaData": data,
            "aoColumns":
            [
                { mDataProp: "NumeroOrdenAtencion", sClass: "text-right" },
                { mDataProp: "FechaOrdenAtencionTexto" },
                { mDataProp: "TipoDocumentoDescripcion" },
                { mDataProp: "NumeroDocumento", sClass: "text-right" },
                { mDataProp: "PacienteRut", sClass: "text-right" },
                { mDataProp: "PacienteNombre" },
                { mDataProp: "TitularRut", sClass: "text-right" },
                { mDataProp: "TitularNombre" },
                { mDataProp: "FichaClinica" },
                { mDataProp: "Episodio" },
                {
                    sDefaultContent: "",
                    sClass: "text-right",
                    mRender: function (data, type, row) {
                        return "$ " + Moneda(row.MontoPago.toString());
                    }
                },
                {
                    sDefaultContent: "-",
                    mRender: function (data, type, row) {
                        var resultado = "";
                        if (row.EstadoResponsable == "P") {
                            resultado = '<span class="fa fa-circle" style="color: yellow; cursor: pointer" title="Pendiente"></span>';
                        }
                        else if (row.EstadoResponsable == "Y") {
                            resultado = '<span class="fa fa-circle" style="color: green; cursor: pointer" title="Aprobado"></span>';
                        }
                        else if (row.EstadoResponsable == "N" && row.EstadoSupervisor == "N") {
                            resultado = '<span class="fa fa-circle" style="color: red; cursor: pointer" title="Rechazado, pendiente revisión admisionista"></span>';
                        }
                        else if (row.EstadoResponsable == "N" && row.EstadoSupervisor == "P") {
                            resultado = '<span class="fa fa-circle" style="color: red; cursor: pointer" title="Rechazado, pendiente aprobación supervisor"></span>';
                        }
                        else if (row.EstadoResponsable == "N") {
                            var btnReenviar = '<div class="btn-group"><button onclick="NominaDetalleSupervisor.CambiarEstadoDocumento(this)" data-docentry="' + row.DocEntry + '" data-linea="' + row.LineaDetalle + '" data-tipo="P" type="button" class="btn btn-primary btn-sm" title="Reenviar"><span class="glyphicon glyphicon-share-alt"></span></button></div>';
                            var btnRechazar = '<div class="btn-group"><button onclick="NominaDetalleSupervisor.CambiarEstadoDocumento(this)" data-docentry="' + row.DocEntry + '" data-linea="' + row.LineaDetalle + '" data-tipo="N" type="button" class="btn btn-primary btn-sm" title="Rechazar"><span class="glyphicon glyphicon-remove"></span></button></div>';
                            resultado = '<div class="btn-group btn-group-justified"">' + btnReenviar + btnRechazar + '</div>';
                            modificar = true;
                        }
                        else if (row.EstadoResponsable == "A") {
                            resultado = '<span class="fa fa-circle" style="color: gray; cursor: pointer" title="Anulado"></span>';
                        }

                        return '<span style="display: none">' + row.EstadoResponsable + '</span><center>' + resultado + '</center>';
                    }
                },
                {
                    sDefaultContent: "",
                    bSortable: false,
                    mRender: function (data, type, row) {
                        var resultado = row.ObservacionResponsable;
                        if (row.EstadoResponsable == "N" && row.EstadoSupervisor != "N" && row.EstadoSupervisor != "P") { //Rechazado
                            resultado = '<input type="text" class="form-control comentario" maxlength="250" style="width: 300px" data-linea="' + row.LineaDetalle + '" data-docentry="' + row.DocEntry + '" value="' + row.ObservacionResponsable + '"/>';
                        }

                        return resultado;
                    }
                },
            ],
            "fnInitComplete": function (oSettings, json) {
                if (modificar) {
                    $("#pnlBotones").show();
                }
            }
        });
    },
    CambiarEstadoDocumento: function (item) {
        var eliminar = false;

        if (item.dataset.tipo == "N") {
            if (item.className.includes("btn-danger")) {
                eliminar = true;
                $(item).removeClass("btn-danger");
                $(item).addClass("btn-primary");
            }
            else {
                $('button[data-linea="' + item.dataset.linea + '"][data-docentry="' + item.dataset.docentry + '"][data-tipo="P"]').removeClass("btn-success").addClass("btn-primary");
                $(item).removeClass("btn-primary");
                $(item).addClass("btn-danger");
            }
        }
        else if (item.dataset.tipo == "P") {
            if (item.className.includes("btn-success")) {
                eliminar = true;
                $(item).removeClass("btn-success");
                $(item).addClass("btn-primary");
            }
            else {
                $('button[data-linea="' + item.dataset.linea + '"][data-docentry="' + item.dataset.docentry + '"][data-tipo="N"]').removeClass("btn-danger").addClass("btn-primary");
                $(item).removeClass("btn-primary");
                $(item).addClass("btn-success");
            }
        }

        var observacion = $('input[type="text"][data-linea="' + item.dataset.linea + '"][data-docentry="' + item.dataset.docentry + '"]').val();
        if (documentosModificados.length > 0) {
            for (var i = 0; i < documentosModificados.length; i++) {
                var detalle = documentosModificados[i];
                if (detalle.LineaDetalle == item.dataset.linea && detalle.DocEntry == item.dataset.docentry) {
                    documentosModificados.splice(i, 1);
                    break;
                }
            }
        }
        if (!eliminar) {
            var detalle = {
                DocEntry: item.dataset.docentry,
                LineaDetalle: item.dataset.linea
            }

            if (item.dataset.tipo == "P") {
                detalle.EstadoResponsable = item.dataset.tipo;
                detalle.ObservacionResponsable = observacion;
            }
            else if (item.dataset.tipo == "N") {
                detalle.EstadoSupervisor = item.dataset.tipo;
                detalle.ObservacionSupervisor = observacion;
            }

            documentosModificados.push(detalle);
        }
    },
    AgregarComentario: function (item) {
        if (documentosModificados.length > 0) {
            for (var i = 0; i < documentosModificados.length; i++) {
                var detalle = documentosModificados[i];
                if (detalle.LineaDetalle == item.dataset.linea && detalle.DocEntry == item.dataset.docentry) {
                    documentosModificados.splice(i, 1);

                    if (detalle.EstadoResponsable == "P") {
                        detalle.ObservacionResponsable = item.value;
                    }
                    else if (detalle.EstadoSupervisor == "N") {
                        detalle.ObservacionSupervisor = item.value;
                    }

                    documentosModificados.push(detalle);
                    break;
                }
            }
        }
    },
    LimpiarDetalleNomina: function () {
        documentosModificados = [];
        documentosPendientes = [];

        $("#pnlBotones").hide();

        NominaDetalleSupervisor.CargarDetalleNomina([]);
    },
    ValidarDocumentos: function () {
        if (documentosModificados.length > 0) {
            var valido = true;
            for (var i = 0; i < documentosModificados.length; i++) {
                if (documentosModificados[i].EstadoSupervisor == "N" && // Es Rechazado y sin observacion
                    documentosModificados[i].ObservacionSupervisor == "") {
                    valido = false;
                    Mensaje.Alerta("Existen documentos Rechazados, que no tienen un comentario. Favor ingresar.");
                    break;
                }
            }
            if (valido) {
                documentosFinal = documentosModificados;
                Mensaje.Confirmar("¿Está seguro de guardar los cambios?", NominaDetalleSupervisor.GuardarDocumentos);
            }
        }
        else {
            Mensaje.Alerta("Debe modificar el estado de al menos uno de los documentos de la lista para continuar.");
        }
    },
    GuardarDocumentos: function () {
        var postData = { listaDetalle: documentosFinal };
        $.ajax({
            url: '/Nomina/GuardarDetalleNomina',
            type: 'POST',
            cache: false,
            data: JSON.stringify(postData),
            contentType: 'application/json; charset=utf-8',
            success: function (respuesta) {
                Mensaje.Alerta(respuesta.Mensaje);

                if (!respuesta.EsError) {
                    NominaPrincipalSupervisor.BuscarNominas();
                }
            }
        });
    },
    ConfirmarReenviarTodas: function () {
        if (documentosPendientes.length > 0) {
            bootbox.confirm({
                message: '<span class="glyphicon glyphicon-question-sign" style="color: blue"></span>&nbsp;&nbsp;¿Está seguro de reenviar todos los documentos pendientes?',
                buttons: {
                    confirm: { label: 'Aceptar', className: 'btn-primary' },
                    cancel: { label: 'Cancelar' }
                },
                callback: function (result) {
                    if (result) {
                        for (var i = 0; i < documentosPendientes.length; i++) {
                            documentosPendientes[i].EstadoResponsable = "P";
                            documentosPendientes[i].EstadoSupervisor = null;
                        }

                        documentosFinal = documentosPendientes;
                        NominaDetalleSupervisor.GuardarDocumentos();
                    }
                }
            });
        }
        else {
            Mensaje.Alerta("No existen documentos pendientes para aprobar.");
        }
    },
    ConfirmarRechazarTodas: function () {
        if (documentosPendientes.length > 0) {
            bootbox.dialog({
                message: '<span class="glyphicon glyphicon-question-sign" style="color: blue"></span>&nbsp;&nbsp;¿Está seguro de rechazar todos los documentos pendientes?<br/><br/><div class="form-group" ><label class="control-label">Ingrese motivo de rechazo:</label><textarea id="txtObservacionRechazo" rows="3" class="form-control" style="max-width: none" maxlength="250"></textarea></div>',
                buttons: {
                    cancel: {
                        label: 'Cancelar',
                        className: 'btn-default'
                    },
                    confirm: {
                        label: 'Aceptar',
                        className: 'btn-primary',
                        callback: function (data) {
                            $("#txtObservacionRechazo").focus();
                            if ($("#txtObservacionRechazo").val() == "") {
                                Mensaje.Alerta("Debe ingresar el motivo de rechazo.");
                                return false;
                            }
                            else {
                                for (var i = 0; i < documentosPendientes.length; i++) {
                                    documentosPendientes[i].EstadoResponsable = null;
                                    documentosPendientes[i].EstadoSupervisor = "N";
                                    documentosPendientes[i].ObservacionSupervisor = $("#txtObservacionRechazo").val();
                                }

                                documentosFinal = documentosPendientes;
                                NominaDetalleSupervisor.GuardarDocumentos();
                            }
                        }
                    }
                }
            });
        }
        else {
            Mensaje.Alerta("No existen documentos pendientes para aprobar.");
        }
    },
}