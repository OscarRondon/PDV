var listaCajaSegmento = [];

$(document).ready(function () {
    AsignarCajaSegmento.Init();
});

var AsignarCajaSegmento = {
    Init: function () {
        $('#modalAsignarCajaSegmento').on('hidden.bs.modal', function (e) {
            AsignarCajaSegmento.LoadSegmentoFolio();
            $("#hdnIdFolioAsignar").val("");
        });
    },
    ObtenerSegmentosFolio: function () {
        var postData = { IdFolio: $("#hdnIdFolioAsignar").val() };
        $.ajax({
            url: '/Folio/BuscarSegmentosPorId',
            type: 'POST',
            cache: false,
            data: JSON.stringify(postData),
            contentType: 'application/json; charset=utf-8',
            success: function (respuesta) {
                if (respuesta.EsError) {
                    Mensaje.Alerta(respuesta.Mensaje);
                }
                else {
                    AsignarCajaSegmento.LoadSegmentoFolio(respuesta.Data);
                }
            }
        });
    },
    LoadSegmentoFolio: function (data) {
        listaCajaSegmento = [];

        $('#listaSegmentos').DataTable().clear();
        $('#listaSegmentos').DataTable().destroy();
        $('#listaSegmentos').DataTable({
            "language": dataTableEspanol,
            "bAutoWidth": false,
            "bFilter": false,
            "bInfo": true,
            "bLengthChange": false,
            "bPaginate": true,
            "bProcessing": false,
            "bSort": true,
            "bDestroy": true,

            "order": [[0, 'asc']],

            "aaData": data,
            "aoColumns":
                [

                    { mDataProp: "LineaSegmento", sType: "numeric", sClass: "text-right", "width": "5%" },
                    { mDataProp: "FolioInicial", sType: "numeric", sClass: "text-right", "width": "5%" },
                    { mDataProp: "FolioFinal", sType: "numeric", sClass: "text-right", "width": "5%" },
                    { mDataProp: "FolioActual", sType: "numeric", sClass: "text-right", "width": "5%" },
                    { mDataProp: "TotalFoliosAnulados", sType: "numeric", sClass: "text-right", "width": "5%" },
                    { mDataProp: "TotalFoliosUsados", sType: "numeric", sClass: "text-right", "width": "5%" },
                    { mDataProp: "TotalFoliosDisponibles", sType: "numeric", sClass: "text-right", "width": "5%" },
                    { mDataProp: "TotalFolios", sType: "numeric", sClass: "text-right", "width": "5%" },
                    {
                        sDefaultContent: "",
                        "width": "60%",
                        bSortable: false,
                        mRender: function (data, type, row) {
                            var cajasClon = $("#ddlCajasSupervisor").clone();

                            cajasClon[0].setAttribute("data-linea", row.LineaSegmento);
                            cajasClon[0].setAttribute("data-disponibles", row.TotalFoliosDisponibles);
                            cajasClon[0].setAttribute("data-idcaja", "");
                            cajasClon[0].style.display = "";

                            var resultado = '';
                            if (row.IdCaja == null) {
                                resultado = cajasClon[0].outerHTML;
                            }
                            else if (row.FolioInicial == row.FolioActual) {
                                cajasClon[0].setAttribute("data-idcaja", row.IdCaja);

                                for (var i = 0; i < cajasClon[0].options.length; i++) {
                                    if (cajasClon[0].options[i].value == row.IdCaja)
                                        cajasClon[0].options[i].setAttribute("selected", "selected");
                                }
                                resultado = cajasClon[0].outerHTML;
                            }
                            else {
                                resultado = row.NombreCaja;
                            }

                            return resultado;
                        }
                    }
                ]
        });
    },
    EditarFormulario: function (item) {
        $("#hdnIdFolioAsignar").val(item.dataset.idfolio);

        AsignarCajaSegmento.LoadSegmentoFolio();
        AsignarCajaSegmento.ObtenerSegmentosFolio();

        $('#modalAsignarCajaSegmento').modal("show");
    },
    SeleccionarCaja: function (item) {
        if (item.value != "") {
            //Eliminamos la caja del listado si ya fue agregada, antes de agregar la nueva.
            for (var i = 0; i < listaCajaSegmento.length; i++) {
                if (listaCajaSegmento[i].IdCaja == item.dataset.idcaja) {
                    listaCajaSegmento.splice(i, 1);
                }
            }
            if (item.value != "" && parseInt(item.dataset.disponibles) > 0) {
                var segmento = {
                    IdFolio: $("#hdnIdFolioAsignar").val(),
                    IdCaja: item.value,
                    LineaSegmento: item.dataset.linea
                };
                listaCajaSegmento.push(segmento);
            } else {
                Mensaje.Alerta("Segmento sin folios disponibles.");
                $(item).val(item.dataset.idcaja);
            }
            item.setAttribute("data-idcaja", item.value);
        }
        else {
            Mensaje.Alerta("Debe seleccionar una caja válida.");
            $(item).val(item.dataset.idcaja);
        }
    },
    ValidarGuardarCajas: function () {
        if (listaCajaSegmento.length > 0) {
            Mensaje.Confirmar("¿Esta seguro de guardar las asignaciones realizadas?", AsignarCajaSegmento.GuardarFormulario);
        }
        else {
            Mensaje.Alerta("Debe asignar al menos una caja para guardar.");
        }
    },
    GuardarFormulario: function () {
        var postData = { lista: listaCajaSegmento };
        $.ajax({
            url: '/Folio/AsignacionCajaSegmentoMasivo',
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
                    $('#modalAsignarCajaSegmento').modal("hide");
                    Mensaje.CorrectoF(mensaje, FolioSupervisor.BuscarFoliosSupervisor);
                }
            }
        });
    }
}