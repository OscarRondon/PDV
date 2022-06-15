$(document).ready(function () {
    Folio.Init();

});
var Folio = {
    Init: function () {
        $('#txtFechaInicio').datetimepicker({
            format: "DD/MM/YYYY",
            ignoreReadonly: true,
            showTodayButton: true,
            allowInputToggle: true,
            locale: "es"
        });
        $('#txtFechaFinal').datetimepicker({
            format: "DD/MM/YYYY",
            ignoreReadonly: true,
            showTodayButton: true,
            allowInputToggle: true,
            //JMuseCurrent: false,//Important! See issue #1075, se usa solo cuanto esta vacia la fecha
            locale: "es"
        });
        $("#txtFechaInicio").on("dp.change", function (e) {
            $('#txtFechaFinal').data("DateTimePicker").minDate(e.date);
        });
        $("#txtFechaFinal").on("dp.change", function (e) {
            $('#txtFechaInicio').data("DateTimePicker").maxDate(e.date);
        });

        $('#btnFechaInicio').click(function () {
            $('#txtFechaInicio').focus();
        });
        $('#btnFechaFinal').click(function () {
            $('#txtFechaFinal').focus();
        });

        $('#panelFiltro').on('hide.bs.collapse', function (e) {
            $("#iconoFiltro").removeClass("glyphicon-menu-up");
            $("#iconoFiltro").addClass("glyphicon-menu-down");
        });
        $('#panelFiltro').on('show.bs.collapse', function (e) {
            $("#iconoFiltro").removeClass("glyphicon-menu-down");
            $("#iconoFiltro").addClass("glyphicon-menu-up");
        });
        Folio.LoadFolios();
        mLoading.modal();
        Folio.BuscarFolios();
    },
    BuscarFolios: function () {
        var jsonFolio = {
            FolioInicio: $('#txtFolioDesde').val(),
            FolioFin: $('#txtFolioFinal').val(),
            FechaInicio: $('#txtFechaInicio').val(),
            FechaFin: $('#txtFechaFinal').val()
        };
        var postData = { folio: jsonFolio };
        $.ajax({
            url: '/Folio/BuscarFolioPor',
            type: 'POST',
            cache: false,
            data: JSON.stringify(postData),
            contentType: 'application/json; charset=utf-8',
            success: function (respuesta) {
                if (respuesta.EsError) {
                    Mensaje.Alerta(respuesta.Mensaje);
                }
                else {
                    Folio.LoadFolios(respuesta.Data);
                }
            }
        });
    },
    LoadFolios: function (data) {
        $('#listaFolios').DataTable().destroy();
        $('#listaFolios').DataTable({
            "language": dataTableEspanol,
            "bAutoWidth": false,
            "bFilter": false,
            "bInfo": true,
            "bLengthChange": false,
            "bPaginate": true,
            "bProcessing": false,
            "bSort": true,
            "bDestroy": true,

            "order": [[1, 'desc']],

            "aaData": data,
            "aoColumns":
                [
                    { mDataProp: "DocumentoDescripcion" },
                    { mDataProp: "FolioInicial", sType: "numeric", sClass: "text-right" },
                    { mDataProp: "FolioFinal", sType: "numeric", sClass: "text-right" },
                    { mDataProp: "TotalFoliosAnulados", sType: "numeric", sClass: "text-right" },
                    { mDataProp: "TotalFoliosUsados", sType: "numeric", sClass: "text-right" },
                    { mDataProp: "TotalFoliosDisponibles", sType: "numeric", sClass: "text-right" },
                    { mDataProp: "TotalFolios", sType: "numeric", sClass: "text-right" },
                    { mDataProp: "FechaRegistroTexto", sType: "date", sClass: "text-center" },
                    { mDataProp: "CantidadSegmento", sType: "numeric", sClass: "text-right" },
                    { mDataProp: "NombreUsuarioSupervisor" },
                    {
                        sDefaultContent: "",
                        bSortable: false,
                        mRender: function (data, type, row) {
                            var dataFila = 'data-IdFolio="' + row.IdFolio + '"';
                            dataFila += ' data-FolioInicial="' + (row.FolioInicial != null ? row.FolioInicial : "") + '"';
                            dataFila += ' data-FolioFinal="' + (row.FolioFinal != null ? row.FolioFinal : "") + '"';
                            dataFila += ' data-TipoDocumento="' + (row.TipoDocumento != null ? row.TipoDocumento : "") + '"';
                            dataFila += ' data-IdUsuarioSupervisor="' + (row.IdUsuarioSupervisor != null ? row.IdUsuarioSupervisor : "") + '"';
                            dataFila += ' data-UltimoFolio="' + (row.EsUltimoFolio ? "S" : "N") + '"';

                            var btnEditar = '<span class="glyphicon glyphicon-pencil" style="cursor: pointer" title="Folio ya segmentado, no se puede editar"></span>';
                            if (row.CantidadSegmento == null) {
                                btnEditar = '<a ' + dataFila + ' onclick="CrearFolioModal.EditarFormulario(this)" style="cursor: pointer" id="lnkEditarFolio" name="lnkEditarFolio"><span class="glyphicon glyphicon-pencil" title="Editar folio"></span></a>';
                            }
                            var btnAnular = '<a ' + dataFila + ' data-anular="1" onclick="AnularFolioRango.AbrirFormulario(this)" style="cursor: pointer; color: red" id="lnkAnularFolio" name="lnkAnularFolio"><span class="glyphicon glyphicon-trash" title="Anular folio"></span></a>';

                            return '<center>' + btnEditar + '&nbsp;&nbsp;&nbsp;' + btnAnular + '</center>';
                        }
                    }
                ]
        });
    },

}

var LimiteFolio = {
    Init: function () {
        LimiteFolio.BuscarLimiteFolios();
    },
    BuscarLimiteFolios: function () {
        $.ajax({
            url: '/Folio/ObtenerListadoLimiteFolio',
            type: 'GET',
            cache: false,
            //JMdata: JSON.stringify(postData),
            contentType: 'application/json; charset=utf-8',
            success: function (respuesta) {
                if (respuesta.EsError) {
                    Mensaje.Alerta(respuesta.Mensaje);
                }
                else {
                    LimiteFolio.CargarGrillaFolio(respuesta.Data);
                }
            }
        });
    },
    CargarGrillaFolio: function (data) {
        $('#listLimiteFolio').DataTable().destroy();
        $('#listLimiteFolio').DataTable({
            "language": dataTableEspanol,
            "bAutoWidth": false,
            "bFilter": false,
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
                    { mDataProp: "FolioInicial", sType: "numeric", sClass: "text-right" },
                    { mDataProp: "FolioFinal", sType: "numeric", sClass: "text-right" },
                    { mDataProp: "FechaRegistroTexto", sType: "date", sClass: "text-center" },
                    {
                        mDataProp: "EsAnulado",
                        sClass: "text-center",
                        mRender: function (data, type, row) {

                            var resultado = '';
                            if (row.EsAnulado) {
                                resultado = '<span class="fa fa-circle" style="color: red; cursor: pointer"></span>&nbsp;';
                            }
                            else {
                                resultado = '<span class="fa fa-circle" style="color: green; cursor: pointer"></span>&nbsp;';
                            }


                            return resultado + (row.EsAnulado ? "SI" : "NO");
                        }
                    },
                    { mDataProp: "NombreUsuarioCreacion", sDefaultContent: "-", },
                    { mDataProp: "MotivoAnulacion", sDefaultContent: "-", },
                    {
                        sDefaultContent: "",
                        bSortable: false,
                        mRender: function (data, type, row) {
                            var dataFila = 'data-IdFolio="' + row.IdFolio + '"';
                            dataFila += ' data-FolioInicial="' + (row.FolioInicial != null ? row.FolioInicial : "") + '"';
                            dataFila += ' data-FolioFinal="' + (row.FolioFinal != null ? row.FolioFinal : "") + '"';
                            dataFila += ' data-anulado="' + (row.EsAnulado ? "S" : "N") + '"';
                            dataFila += ' data-motivo="' + row.MotivoAnulacion + '"';


                            var btnEditar = '<span class="glyphicon glyphicon-pencil" style="cursor: pointer" title="Rango de folio, no es el último ingresado."></span>';
                            if (row.EsUltimoFolio) {
                                btnEditar = '<a ' + dataFila + ' onclick="CrearLimiteModal.EditarFormulario(this)" style="cursor: pointer" id="lnkEditarLimite" name="lnkEditarLimite"><span class="glyphicon glyphicon-pencil" title="Editar folio"></span></a>';
                            }
                            return '<center>' + btnEditar + '</center>';
                        }
                    }
                ]
        });
    },

}

