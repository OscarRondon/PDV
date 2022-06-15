var listaSegmentos = [];

$(document).ready(function () {
    FolioSupervisor.Init();
});

var FolioSupervisor = {
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
        FolioSupervisor.LoadFoliosSupervisor();
        mLoading.modal();
        FolioSupervisor.BuscarFoliosSupervisor();

        $(document).on("blur", ".segmento", function () {
            FolioSupervisor.ValidarSegmentoIngresado(this);
        });
    },
    BuscarFoliosSupervisor: function () {
        var jsonFolio = {
            FolioInicio: $('#txtFolioDesde').val(),
            FolioFin: $('#txtFolioFinal').val(),
            FechaInicio: $('#txtFechaInicio').val(),
            FechaFin: $('#txtFechaFinal').val()
        };
        var postData = { folio: jsonFolio };
        $.ajax({
            url: '/Folio/BuscarFolioPorSupervisor',
            type: 'POST',
            cache: false,
            data: JSON.stringify(postData),
            contentType: 'application/json; charset=utf-8',
            success: function (respuesta) {
                if (respuesta.EsError) {
                    Mensaje.Alerta(respuesta.Mensaje);
                }
                else {
                    FolioSupervisor.LoadFoliosSupervisor(respuesta.Data);
                }
            }
        });
    },
    LoadFoliosSupervisor: function (data) {
        listaSegmentos = [];
        $("#btnGuardarSegmento").attr("disabled", "disabled");

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

            "order": [[2, 'asc']],

            "aaData": data,
            "aoColumns":
            [
                { mDataProp: "DocumentoDescripcion" },
                { sDefaultContent: "-", mDataProp: "FechaRegistroTexto", sType: "date" },
                { mDataProp: "FolioInicial", sType: "numeric", sClass: "text-right" },
                { mDataProp: "FolioFinal", sType: "numeric", sClass: "text-right" },
                { mDataProp: "TotalFoliosAnulados", sType: "numeric", sClass: "text-right" },
                { mDataProp: "TotalFoliosUsados", sType: "numeric", sClass: "text-right" },
                { mDataProp: "TotalFoliosDisponibles", sType: "numeric", sClass: "text-right" },
                { mDataProp: "TotalFolios", sType: "numeric", sClass: "text-right" },
                {
                    sClass: "text-right",
                    bSortable: true,
                    mRender: function (data, type, row) {
                        var resultado = '';
                        if (row.CantidadSegmento == null) {
                            resultado = '<input type="text" data-segmentoactual="0" data-IdFolio="' + row.IdFolio + '" data-FolioFinal="' + row.FolioFinal + '"  data-FolioInicial="' + row.FolioInicial + '"  class="form-control text-right segmento" onkeyup="PDV.SoloNumero(this);"  value=""  maxlength="10" placeholder="Ingresar segmentos" />';
                        }
                        else {
                            resultado = '<div>' + row.CantidadSegmento + '</div>';
                        }
                        return resultado;
                    }
                },
                {
                    sDefaultContent: "-",
                    sClass: "text-center",
                    bSortable: false,
                    mRender: function (data, type, row) {
                        var color = '';
                        var title = '';

                        if (row.CantidadSegmentoPendiente == null) {
                            color = 'gray';
                            title = 'Sin segmento asignado';
                        }
                        else if (row.CantidadSegmentoPendiente > 0) {
                            color = 'Red';
                            title = row.CantidadSegmentoPendiente + ' Segmentos pendientes';
                        }
                        else if (row.CantidadSegmentoPendiente == 0) {
                            color = 'Green';
                            title = 'Sin segmentos pendientes';
                        }

                        var resultado = '<span class="fa fa-circle" title="' + title + '" style="color: ' + color + '; cursor: pointer"></span>';
                        return resultado;
                    }
                },
                {
                    sDefaultContent: "",
                    bSortable: false,
                    mRender: function (data, type, row) {
                        var dataFila = 'data-IdFolio="' + row.IdFolio + '"';
                        dataFila += ' data-FolioInicial="' + (row.FolioInicial != null ? row.FolioInicial : "") + '"';
                        dataFila += ' data-FolioFinal="' + (row.FolioFinal != null ? row.FolioFinal : "") + '"';

                        var btnSegmentos = '<span class="glyphicon glyphicon-th-list" style="cursor: pointer" title="Folio aun no tiene segmentos para asignar"></span>';
                        if (row.CantidadSegmento != null) {
                            btnSegmentos = '<a ' + dataFila + ' onclick="AsignarCajaSegmento.EditarFormulario(this)" style="cursor: pointer" ><span class="glyphicon glyphicon-th-list" title="Asignar caja a los segmentos de este rango"></span></a>';
                        }
                        var btnAnular = '<a ' + dataFila + ' data-anular="2" onclick="AnularFolioRango.AbrirFormulario(this)" style="cursor: pointer; color: red" id="lnkAnularFolio" name="lnkAnularFolio"><span class="glyphicon glyphicon-trash" title="Anular folio"></span></a>';

                        return '<center>' + btnSegmentos + '&nbsp;&nbsp;&nbsp;' + btnAnular + '</center>';
                    }
                }
            ]
        });
    },

    ValidarSegmentoIngresado: function (item) {
        if (item.dataset.segmentoactual != item.value) {
            var idfolio = item.dataset.idfolio;
            if (item.value != "") {
                var fIni = parseInt(item.dataset.folioinicial);
                var fFin = parseInt(item.dataset.foliofinal);
                var fSeg = parseInt(item.value);

                if ((fFin - fIni) <= fSeg) {
                    Mensaje.Alerta("Número de segmento debe ser menor a la cantidad de folios del rango.");
                    item.value = "";
                    for (var i = 0; i < listaSegmentos.length; i++) {
                        if (listaSegmentos[i].IdFolio == idfolio) {
                            listaSegmentos.splice(i, 1);
                        }
                    }
                }
                else {
                    if (listaSegmentos.length > 0) {
                        for (var i = 0; i < listaSegmentos.length; i++) {
                            if (listaSegmentos[i].IdFolio == idfolio) {
                                listaSegmentos.splice(i, 1);
                                break;
                            }
                        }
                    }

                    var fTotal = (fFin - fIni) + 1;
                    var fRestante = fTotal % fSeg;

                    if (fRestante > 0) {
                        bootbox.confirm({
                            message: '<span class="glyphicon glyphicon-question-sign" style="color: blue"></span>&nbsp;&nbsp;La división entre la cantidad de folios y el segmento ingresado, no da un número entero como resultado. Existen ' + fRestante + ' folios que serán agregados al último segmento.<br/> ¿Desea continuar?',
                            buttons: {
                                confirm: { label: 'Aceptar', className: 'btn-primary' },
                                cancel: { label: 'Cancelar' }
                            },
                            callback: function (result) {
                                if (result) {
                                    var segmento = {
                                        IdFolio: idfolio,
                                        CantidadSegmento: fSeg,
                                    }
                                    listaSegmentos.push(segmento);
                                    $("#btnGuardarSegmento").removeAttr("disabled");

                                    item.dataset.segmentoactual = item.value;
                                }
                                else {
                                    item.value = "";
                                }
                            }
                        });
                    }
                    else {
                        var segmento = {
                            IdFolio: idfolio,
                            CantidadSegmento: fSeg,
                        }
                        listaSegmentos.push(segmento);

                        item.dataset.segmentoactual = item.value;
                    }
                }
            }
            else {
                for (var i = 0; i < listaSegmentos.length; i++) {
                    if (listaSegmentos[i].IdFolio == idfolio) {
                        listaSegmentos.splice(i, 1);
                        break;
                    }
                }
            }

            if (listaSegmentos.length > 0) {
                $("#btnGuardarSegmento").removeAttr("disabled");
            }
            else {
                $("#btnGuardarSegmento").attr("disabled", "disabled");
            }
        }
    },
    ValidarGuardarSegmento: function () {
        if (listaSegmentos.length > 0) {
            Mensaje.Confirmar("¿Esta seguro de guardar todos los segmentos ingresados?", FolioSupervisor.GuardarSegmento);
        }
        else {
            Mensaje.Alerta("Debe ingresar al menos un segmento para guardar.");
        }
    },
    GuardarSegmento: function () {
        var postData = { lista: listaSegmentos }
        $.ajax({
            url: '/Folio/GuardarFolioSegmentoMasivo',
            type: 'POST',
            cache: false,
            data: JSON.stringify(postData),
            contentType: 'application/json; charset=utf-8',
            success: function (respuesta) {
                var mensaje = respuesta.Mensaje;
                if (respuesta.EsError) {
                    Mensaje.Alerta(mensaje);
                }
                else {
                    Mensaje.CorrectoF(mensaje, FolioSupervisor.BuscarFoliosSupervisor);
                }
            }
        });
    }
}