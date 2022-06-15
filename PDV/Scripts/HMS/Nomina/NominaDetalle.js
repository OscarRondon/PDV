var documentosModificados = [];
var documentosPendientes = [];
var documentosFinal = [];
$(document).ready(function () {
    NominaDetalle.Init();
});

var NominaDetalle = {
    Init: function () {
        NominaDetalle.CargarDetalleNomina([], false);

        $(document).on("blur", ".comentario", function () {
            NominaDetalle.AgregarComentario(this);
        });
    },
    BuscarNominaDetalle: function (nomina) {
        var postData = { docEntryNomina: nomina.docentry };
        $.ajax({
            url: '/Nomina/BuscarDetalleNomina',
            type: 'POST',
            cache: false,
            data: JSON.stringify(postData),
            contentType: 'application/json; charset=utf-8',
            success: function (respuesta) {
                if (respuesta.EsError) {
                    Mensaje.Alerta(respuesta.Mensaje);
                    NominaPrincipal.LimpiarNomina();
                }
                else {
                    $("#spnNumeroNomina")[0].innerHTML = nomina.numero;
                    $("#spnTipoNomina")[0].innerHTML = nomina.tipo;
                    $("#spnAdmisionista")[0].innerHTML = nomina.admisionista;
                    $("#spnCaja")[0].innerHTML = nomina.caja;

                    if (nomina.tipo == "Contado") {
                        $("#spnNumeroDocumento")[0].innerHTML = "N° Operación/Cheque";
                    }
                    else {
                        $("#spnNumeroDocumento")[0].innerHTML = "N° Documento";
                    }

                    if (respuesta.Data != null && respuesta.Data.DetalleNomina != null && respuesta.Data.DetalleNomina.length > 0) {
                        NominaDetalle.CargarDetalleNomina(respuesta.Data.DetalleNomina, (nomina.tipo == "Contado"));
                        for (var i = 0; i < respuesta.Data.DetalleNomina.length; i++) {
                            var detalle = respuesta.Data.DetalleNomina[i];
                            if ($("#EsRevisor").val() == 1) { //Es Revisor
                                if (detalle.EstadoSupervisor == "P") {
                                    documentosPendientes.push(detalle);
                                }
                            }
                            else { //Es Admisionista
                                if (detalle.EstadoSupervisor == "N") {
                                    documentosPendientes.push(detalle);
                                }
                            }
                        }
                    }
                }
            }
        });
    },
    CargarDetalleNomina: function (data, tieneMedioPago) {
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
                    {
                        sDefaultContent: "",
                        mRender: function (data, type, row) {
                            var resultado = "";
                            if (row.TipoDocumento == "BX") { //Contado
                                resultado = row.TipoDocumentoSAP;
                            }
                            else {
                                resultado = row.TipoDocumentoDescripcion
                            }
                            return resultado;
                        }
                    },
                    {
                        bVisible: tieneMedioPago,
                        sDefaultContent: "",
                        mRender: function (data, type, row) {
                            return (row.SubTipoDocumento == null || row.SubTipoDocumento == "") ? row.TipoDocumentoDescripcion : row.SubTipoDocumento;
                        }
                    },
                    { mDataProp: "NumeroDocumento", sClass: "text-right" },
                    { mDataProp: "PacienteRut", sClass: "text-right" },
                    { mDataProp: "PacienteNombre" },
                    { mDataProp: "TitularRut", sClass: "text-right" },
                    { mDataProp: "TitularNombre" },
                    { mDataProp: "FichaClinica" },
                    { mDataProp: "Episodio" },
                    {
                        mDataProp: "MontoPago",
                        className: "text-right",
                        render: $.fn.dataTable.render.number('.', ',', 0, '$'),
                    },
                    {
                        sDefaultContent: "",
                        mRender: function (data, type, row) {
                            var resultado = "-";
                            if ($("#EsRevisor").val() == 1) { //Es Revisor
                                if (row.EstadoSupervisor == "P") {
                                    var btnAprobar = '<div class="btn-group"><button onclick="NominaDetalle.CambiarEstadoDocumento(this)" data-docentry="' + row.DocEntry + '" data-linea="' + row.LineaDetalle + '" data-tipo="Y" type="button" class="btn btn-primary btn-sm" title="Aprobar"><span class="glyphicon glyphicon glyphicon-ok"></span></button></div>';
                                    var btnRechazar = '<div class="btn-group"><button onclick="NominaDetalle.CambiarEstadoDocumento(this)" data-docentry="' + row.DocEntry + '" data-linea="' + row.LineaDetalle + '"  data-tipo="N" type="button" class="btn btn-primary btn-sm" title="Rechazar"><span class="glyphicon glyphicon glyphicon-remove"></span></button></div>';
                                    resultado = '<div class="btn-group btn-group-justified"">' + btnAprobar + btnRechazar + '</div>';
                                    modificar = true;
                                }
                                else if (row.EstadoSupervisor == "Y") {
                                    resultado = '<span class="fa fa-circle" style="color: green; cursor: pointer" title="Aprobado"></span>';
                                }
                                else if (row.EstadoSupervisor == "N") {
                                    resultado = '<span class="fa fa-circle" style="color: red; cursor: pointer" title="Rechazado"></span>';
                                }
                                else if (row.EstadoSupervisor == "A") {
                                    resultado = '<span class="fa fa-circle" style="color: gray; cursor: pointer" title="Anulado"></span>';
                                }
                            }
                            else { //Es Admisionista
                                if (row.EstadoSupervisor == "A") {
                                    resultado = '<span class="fa fa-circle" style="color: gray; cursor: pointer" title="Anulado"></span>';
                                }
                                else if (row.EstadoSupervisor == "P") {
                                    resultado = '<span class="fa fa-circle" style="color: yellow; cursor: pointer" title="Pendiente"></span>';
                                }
                                else if (row.EstadoSupervisor == "Y") {
                                    resultado = '<span class="fa fa-circle" style="color: green; cursor: pointer" title="Aprobado"></span>';
                                }
                                else if (row.EstadoSupervisor == "N") {
                                    var btnReenviar = '<div class="btn-group"><button onclick="NominaDetalle.CambiarEstadoDocumento(this)" data-docentry="' + row.DocEntry + '" data-linea="' + row.LineaDetalle + '" data-tipo="P" type="button" class="btn btn-primary btn-sm" title="Reenviar"><span class="glyphicon glyphicon-share-alt"></span></button></div>';
                                    resultado = '<div class="btn-group btn-group-justified"">' + btnReenviar + '</div>';

                                    modificar = true;
                                }
                            }
                            return '<span style="display: none">' + row.EstadoSupervisor + '</span><center>' + resultado + '</center>';
                        }
                    },
                    {
                        sDefaultContent: "",
                        bSortable: false,
                        mRender: function (data, type, row) {
                            var resultado = row.ObservacionSupervisor;
                            if ($("#EsRevisor").val() == 1) {
                                if (row.EstadoSupervisor == "P") { //Pendiente
                                    resultado = '<input type="text" class="form-control comentario" maxlength="250" style="width: 300px" data-linea="' + row.LineaDetalle + '" data-docentry="' + row.DocEntry + '"/>';
                                }
                            }
                            else {
                                if (row.EstadoSupervisor == "N") { //Rechazado
                                    resultado = '<input type="text" class="form-control comentario" maxlength="250" style="width: 300px" data-linea="' + row.LineaDetalle + '" data-docentry="' + row.DocEntry + '" value="' + row.ObservacionSupervisor + '"/>';
                                }
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
                $('button[data-linea="' + item.dataset.linea + '"][data-tipo="Y"]').removeClass("btn-success").addClass("btn-primary");
                $(item).removeClass("btn-primary");
                $(item).addClass("btn-danger");
            }
        }
        else if (item.dataset.tipo == "Y") {
            if (item.className.includes("btn-success")) {
                eliminar = true;
                $(item).removeClass("btn-success");
                $(item).addClass("btn-primary");
            }
            else {
                $('button[data-linea="' + item.dataset.linea + '"][data-tipo="N"]').removeClass("btn-danger").addClass("btn-primary");
                $(item).removeClass("btn-primary");
                $(item).addClass("btn-success");
            }
        }
        else if (item.dataset.tipo == "P") {
            if (item.className.includes("btn-success")) {
                eliminar = true;
                $(item).removeClass("btn-success");
                $(item).addClass("btn-primary");
            }
            else {
                $(item).removeClass("btn-primary");
                $(item).addClass("btn-success");
            }
        }

        var observacion = $('input[type="text"][data-linea="' + item.dataset.linea + '"]').val();
        var noExiste = true;
        if (documentosModificados.length > 0) {
            for (var i = 0; i < documentosModificados.length; i++) {
                var detalle = documentosModificados[i];
                if (detalle.LineaDetalle == item.dataset.linea) {
                    noExiste = false;

                    documentosModificados.splice(i, 1);
                    if (!eliminar) {
                        detalle.EstadoSupervisor = item.dataset.tipo;
                        detalle.ObservacionSupervisor = observacion;

                        documentosModificados.push(detalle);
                    }
                    break;
                }
            }
        }
        if (noExiste) {
            var detalle = {
                DocEntry: item.dataset.docentry,
                LineaDetalle: item.dataset.linea,
                EstadoSupervisor: item.dataset.tipo,
                ObservacionSupervisor: observacion
            };

            documentosModificados.push(detalle);
        }
    },
    AgregarComentario: function (item) {
        if (documentosModificados.length > 0) {
            for (var i = 0; i < documentosModificados.length; i++) {
                var detalle = documentosModificados[i];
                if (detalle.LineaDetalle == item.dataset.linea) {
                    documentosModificados.splice(i, 1);

                    detalle.ObservacionSupervisor = item.value;

                    documentosModificados.push(detalle);
                    break;
                }
            }
        }
    },
    LimpiarDetalleNomina: function () {
        documentosModificados = [];
        documentosPendientes = [];

        $("#spnNumeroNomina")[0].innerHTML = "";
        $("#spnTipoNomina")[0].innerHTML = "";
        $("#spnAdmisionista")[0].innerHTML = "";
        $("#spnCaja")[0].innerHTML = "";

        $("#pnlBotones").hide();

        NominaDetalle.CargarDetalleNomina([], false);

        if ($("#EsRevisor").val() != 1) {
            if ($("#btnAprobarTodos")[0] != null) $("#btnAprobarTodos")[0].outerHTML = "";
            if ($("#btnRechazarTodos")[0] != null) $("#btnRechazarTodos")[0].outerHTML = "";
        }
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
                Mensaje.Confirmar("¿Está seguro de guardar los cambios?", NominaDetalle.GuardarDocumentos);
            }
        }
        else {
            Mensaje.Alerta("Debe modificar el estado de al menos uno de los documentos de la lista para continuar.");
        }
    },
    GuardarDocumentos: function () {
        $.ajax({
            url: '/Nomina/GuardarDetalleNomina',
            type: 'POST',
            cache: false,
            data: JSON.stringify({ listaDetalle: JSON.stringify(documentosFinal) }),
            contentType: 'application/json; charset=utf-8',
            success: function (respuesta) {
                Mensaje.Alerta(respuesta.Mensaje);

                if (!respuesta.EsError) {
                    NominaPrincipal.BuscarNominas();
                }
            },
            error: function (xhr, ajaxOptions, thrownError) {
            }
        });
    },
    ConfirmarAprobarTodas: function () {
        if (documentosPendientes.length > 0) {
            bootbox.confirm({
                message: '<span class="glyphicon glyphicon-question-sign" style="color: blue"></span>&nbsp;&nbsp;¿Está seguro de aprobar todos los documentos pendientes?',
                buttons: {
                    confirm: { label: 'Aceptar', className: 'btn-primary' },
                    cancel: { label: 'Cancelar' }
                },
                callback: function (result) {
                    if (result) {
                        for (var i = 0; i < documentosPendientes.length; i++) {
                            documentosPendientes[i].EstadoSupervisor = "Y"; //Aprobar
                        }

                        documentosFinal = documentosPendientes;
                        NominaDetalle.GuardarDocumentos();
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
                                    documentosPendientes[i].ObservacionSupervisor = $("#txtObservacionRechazo").val();
                                    documentosPendientes[i].EstadoSupervisor = "N";
                                }

                                documentosFinal = documentosPendientes;
                                NominaDetalle.GuardarDocumentos();
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