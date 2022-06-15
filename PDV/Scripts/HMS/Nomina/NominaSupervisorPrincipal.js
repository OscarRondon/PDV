$(document).ready(function () {
    NominaPrincipalSupervisor.Init();
});

var NominaPrincipalSupervisor = {
    Init: function () {
        $(".nav-tabs a[data-toggle=tab][class=tabDetalle]").on("click", function (e) {
            if ($("#DocEntryNomina").val() == "") {
                Mensaje.Alerta("Debe seleccionar una Nómina para continuar.");
                e.preventDefault();
                return false;
            }
        });

        $('#txtFechaDesde').datetimepicker({
            format: "DD/MM/YYYY",
            ignoreReadonly: true,
            showTodayButton: true,
            allowInputToggle: true,
            locale: "es"
        });
        $('#txtFechaHasta').datetimepicker({
            format: "DD/MM/YYYY",
            ignoreReadonly: true,
            showTodayButton: true,
            allowInputToggle: true,
            //JMuseCurrent: false,//Important! See issue #1075, se usa solo cuanto esta vacia la fecha
            locale: "es"
        });
        $('#txtFechaDesde').on('dp.change', function (e) {
            $('#txtFechaHasta').data('DateTimePicker').minDate(e.date);
        });
        $('#txtFechaHasta').on('dp.change', function (e) {
            $('#txtFechaDesde').data('DateTimePicker').maxDate(e.date);
        });

        $('#btnFechaDesde').click(function () {
            $('#txtFechaDesde').focus();
        });
        $('#btnFechaHasta').click(function () {
            $('#txtFechaHasta').focus();
        });

        $('#panelFiltro').on('hide.bs.collapse', function (e) {
            $("#iconoFiltro").removeClass("glyphicon-menu-up");
            $("#iconoFiltro").addClass("glyphicon-menu-down");
        });
        $('#panelFiltro').on('show.bs.collapse', function (e) {
            $("#iconoFiltro").removeClass("glyphicon-menu-down");
            $("#iconoFiltro").addClass("glyphicon-menu-up");
        });
        mLoading.modal();
        NominaPrincipalSupervisor.BuscarNominas();
    },
    BuscarNominas: function () {
        NominaPrincipalSupervisor.LimpiarNomina();
        NominaPrincipalSupervisor.CargarNominas([]);

        var jsonNomina = {
            NumeroDesde: $('#txtNominaDesde').val(),
            NumeroHasta: $('#txtNominaHasta').val(),
            FechaDesde: $('#txtFechaDesde').val(),
            FechaHasta: $('#txtFechaHasta').val(),
            TipoNomina: $('#ddlTipoNomina').val(),
            EstadoDetalleNomina: $('#ddlEstado').val(),
        };
        var postData = { filtro: jsonNomina };
        $.ajax({
            url: '/Nomina/BuscarNominasSupervisor',
            type: 'POST',
            cache: false,
            data: JSON.stringify(postData),
            contentType: 'application/json; charset=utf-8',
            success: function (respuesta) {
                if (respuesta.EsError) {
                    Mensaje.Alerta(respuesta.Mensaje);
                }
                else {
                    NominaPrincipalSupervisor.CargarNominas(respuesta.Data);
                }
            }
        });
    },
    CargarNominas: function (data) {
        $('#listaNominas').DataTable().destroy();
        $('#listaNominas').DataTable({
            "language": dataTableEspanol,
            "bAutoWidth": false,
            "bFilter": true,
            "bInfo": true,
            "bLengthChange": false,
            "bPaginate": true,
            "bProcessing": false,
            "bSort": true,
            "bDestroy": true,

            "order": [[0, 'desc']],

            "aaData": data,
            "aoColumns":
            [
                { mDataProp: "NumeroNomina", sClass: "text-right", },
                { mDataProp: "FechaTexto", sClass: "text-center" },
                { mDataProp: "TipoNominaDescripcion" },
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
                        var btnImprimir = '&nbsp;&nbsp;<a title="Imprimir Nómina" onclick="NominaPrincipalSupervisor.ImprimirNomina(' + row.DocEntry + ')"><span class="glyphicon glyphicon-print" style="cursor: pointer"  ></span></a>';
                        if (row.TotalDetalle > 0) {
                            btnDetalle = '<a onclick="NominaPrincipalSupervisor.VerDetalleNomina(this)" data-docentry="' + row.DocEntry + '" data-tipo="' + row.TipoNominaDescripcion + '" data-nomina="' + row.NumeroNomina + '" style="cursor: pointer" id="lnkDetalleNomina" name="lnkDetalleNomina"><span class="glyphicon glyphicon-list-alt" title="Ver detalle Nómina"></span></a>';
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

        NominaDetalleSupervisor.LimpiarDetalleNomina();
    },
    VerDetalleNomina: function (link) {
        var nomina = {
            docentry: link.dataset.docentry,
            numero: link.dataset.nomina,
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

            NominaDetalleSupervisor.LimpiarDetalleNomina();
            NominaDetalleSupervisor.BuscarNominaDetalle(nomina);
        }
        else {
            $('a[href="#tabDetalle"]').tab('show');
        }
    },
    ImprimirNomina: function (_docentry) {
        VisorReporte("NominaDetalleSupervisor.rpt", _docentry, "DocEntry");
    }
}