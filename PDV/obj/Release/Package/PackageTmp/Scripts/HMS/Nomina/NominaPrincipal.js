$(document).ready(function () {
    NominaPrincipal.Init();
});

var NominaPrincipal = {
    Init: function () {
        $(".nav-tabs a[data-toggle=tab][class=tabDetalle]").on("click", function (e) {
            if ($("#DocEntryNomina").val() == "") {
                Mensaje.Alerta("Debe seleccionar una Nómina para continuar.");
                e.preventDefault();
                return false;
            }
        });

        mLoading.modal();
        NominaPrincipal.BuscarNominas();
    },
    LimpiarBusquedaNomina: function () {
        $('#txtNominaDesde').val("");
        $('#txtNominaHasta').val("");
        $('#txtFechaDesde').val("");
        $('#txtFechaHasta').val("");
        $('#ddlTipoNomina').val("");
        $('#ddlFuncion').val("");
        $('#ddlCajas').val("");
        $('#ddlEstado').val("");
        $('#ddlPiso').val("");
        $('#ddlSeccion').val("");
    },
    BuscarNominas: function () {
        NominaPrincipal.LimpiarNomina();
        NominaPrincipal.CargarNominas([]);

        var jsonNomina = {
            NumeroDesde: $('#txtNominaDesde').val(),
            NumeroHasta: $('#txtNominaHasta').val(),
            FechaDesde: $('#txtFechaDesde').val(),
            FechaHasta: $('#txtFechaHasta').val(),
            TipoNomina: $('#ddlTipoNomina').val(),
            TipoAtencion: $('#ddlFuncion').val(),
            EstadoDetalleNomina: $('#ddlEstado').val(),
            CajaId: $('#ddlCajas').val(),
            PisoId: $('#ddlPiso').val(),
            SeccionId: $('#ddlSeccion').val(),
        };
        var postData = { filtro: jsonNomina };
        $.ajax({
            url: '/Nomina/BuscarNominas',
            type: 'POST',
            cache: false,
            data: JSON.stringify(postData),
            contentType: 'application/json; charset=utf-8',
            success: function (respuesta) {
                if (respuesta.EsError) {
                    Mensaje.Alerta(respuesta.Mensaje);
                }
                else {
                    NominaPrincipal.CargarNominas(respuesta.Data);
                }
            }
        });
    },
    CargarNominas: function (data) {
        $('#listaNominas').DataTable().destroy();
        $('#listaNominas').DataTable({
            "language": dataTableEspanol,
            "bAutoWidth": true,
            "bFilter": true,
            "bInfo": true,
            "bLengthChange": false,
            "bPaginate": true,
            "bProcessing": false,
            "bSort": true,
            "scrollX": true,
            "order": [[0, 'desc']],

            "aaData": data,
            "aoColumns":
                [
                    { mDataProp: "NumeroNomina", sClass: "text-right", },
                    { mDataProp: "FechaTexto", sClass: "text-center" },
                    { mDataProp: "TipoNominaDescripcion" },
                    { mDataProp: "TipoAntencionDescripcion" },
                    { mDataProp: "UsuarioNombre" },
                    { mDataProp: "SupervisorNombre" },
                    { mDataProp: "CajaNombre" },
                    { mDataProp: "TotalDetalle", sClass: "text-center" },
                    {
                        sDefaultContent: "",
                        bSortable: false,
                        mRender: function (data, type, row) {
                            var resultado = '';
                            if (row.TotalDetalleAprobado > 0) {
                                resultado += '<div><span class="fa fa-circle" style="color: green; cursor: pointer"></span>&nbsp;' + row.TotalDetalleAprobado + " " + (row.TotalDetalleAprobado == 1 ? "aprobado" : "aprobados") + "</div>";
                            }
                            if (row.TotalDetalleRechazado > 0) {
                                resultado += '<div><span class="fa fa-circle" style="color: red; cursor: pointer"></span>&nbsp;' + row.TotalDetalleRechazado + " " + (row.TotalDetalleRechazado == 1 ? "rechazado" : "rechazados") + "</div>";
                            }
                            if (row.TotalDetallePendiente > 0) {
                                resultado += '<div><span class="fa fa-circle" style="color: yellow; cursor: pointer"></span>&nbsp;' + row.TotalDetallePendiente + " " + (row.TotalDetallePendiente == 1 ? "pendiente" : "pendientes") + "</div>";
                            }
                            if (row.TotalDetalleAnulado > 0) {
                                resultado += '<div><span class="fa fa-circle" style="color: gray; cursor: pointer"></span>&nbsp;' + row.TotalDetalleAnulado + " " + (row.TotalDetalleAnulado == 1 ? "anulado" : "anulados") + "</div>";
                            }
                            return resultado;
                        }
                    },
                    {
                        sDefaultContent: "",
                        bSortable: false,
                        mRender: function (data, type, row) {
                            var btnDetalle = '<span class="glyphicon glyphicon-list-alt" style="cursor: pointer" title="Nómina sin documentos para visualizar"></span>';
                            var btnImprimir = '&nbsp;&nbsp;<a title="Imprimir Nómina" onclick="NominaPrincipal.ImprimirNomina(' + row.DocEntry + ',' + "'" + row.TipoNomina + "'" + ')"><span class="glyphicon glyphicon-print" style="cursor: pointer"  ></span></a>';
                            if (row.TotalDetalle > 0) {
                                btnDetalle = '<a onclick="NominaPrincipal.VerDetalleNomina(this)" data-nomina="' + row.NumeroNomina + '" data-tipo="' + row.TipoNominaDescripcion + '"  data-docentry="' + row.DocEntry + '" data-caja="' + row.CajaNombre + '" data-admisionista="' + row.UsuarioNombre + '"  style="cursor: pointer" id="lnkDetalleNomina" name="lnkDetalleNomina"><span class="glyphicon glyphicon-list-alt" title="Ver detalle Nómina"></span></a>';
                            }
                            return '<center>' + btnDetalle + btnImprimir + '</center>';
                        }
                    }
                ],
            "fnCreatedRow": function (nRow, aData, iDataIndex) {
                nRow.dataset.docentry = aData.DocEntry;
                nRow.className = 'filaNomina';
            },
            "fnRowCallback": function (nRow, aData, iDisplayIndex, iDisplayIndexFull) {
                if (nRow.dataset.docentry != $("#DocEntryNomina").val()) {
                    nRow.className = nRow.className.replace("selected", "");
                }
            }
        });
    },
    LimpiarNomina: function () {
        $("#DocEntryNomina").val("");
        $(".filaNomina").each(function (index, item) {
            $(item).removeClass('selected');
        });
        $('a[href="#tabNominas"]').tab('show');

        NominaDetalle.LimpiarDetalleNomina();
    },
    VerDetalleNomina: function (link) {
        var nomina = {
            docentry: link.dataset.docentry,
            numero: link.dataset.nomina,
            admisionista: link.dataset.admisionista,
            caja: link.dataset.caja,
            tipo: link.dataset.tipo
        };

        $(".filaNomina").each(function (index, item) {
            $(item).removeClass('selected');

            if (item.dataset.docentry == nomina.docentry) {
                $(item).addClass('selected');
            }
        });
        if ($("#DocEntryNomina").val() != nomina.docentry) {
            $('#DocEntryNomina').val(nomina.docentry);

            $('a[href="#tabDetalle"]').tab('show');

            NominaDetalle.LimpiarDetalleNomina();
            NominaDetalle.BuscarNominaDetalle(nomina);
        }
        else {
            $('a[href="#tabDetalle"]').tab('show');
        }
    },
    ImprimirNomina: function (_docentry, tipo) {
        if (tipo == "NC") {
            VisorReporte("NominaDetalleContado.rpt", _docentry, "DocEntry");
        }
        else {
            VisorReporte("NominaDetalle.rpt", _docentry, "DocEntry");
        }
    },
}