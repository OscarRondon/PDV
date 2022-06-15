var tablaPendiente;
var PendientePago = {
    Init: function () {
        mLoading.modal();
        $('#listPendientesPago').on('click', '.filaGrupo', function () {
            var currentOrder = tablaPendiente.order()[0];
            if (currentOrder[0] === 0 && currentOrder[1] === 'asc') {
                tablaPendiente.order([0, 'desc']).draw();
            }
            else {
                tablaPendiente.order([0, 'asc']).draw();
            }
        });

        $('#txtPendienteFechaDesde').datetimepicker({
            format: "DD/MM/YYYY",
            ignoreReadonly: true,
            showTodayButton: true,
            allowInputToggle: true,
            locale: "es"
        });
        $('#txtPendienteFechaHasta').datetimepicker({
            format: "DD/MM/YYYY",
            ignoreReadonly: true,
            showTodayButton: true,
            allowInputToggle: true,
            //JMuseCurrent: false,//Important! See issue #1075, se usa solo cuanto esta vacia la fecha
            locale: "es"
        });
        $('#txtPendienteFechaDesde').on('dp.change', function (e) {
            $('#txtPendienteFechaHasta').data('DateTimePicker').minDate(e.date);
        });
        $('#txtPendienteFechaHasta').on('dp.change', function (e) {
            $('#txtPendienteFechaDesde').data('DateTimePicker').maxDate(e.date);
        });

        $('#btnPendienteFechaDesde').click(function () {
            $('#txtPendienteFechaDesde').focus();
        });
        $('#btnPendienteFechaHasta').click(function () {
            $('#txtPendienteFechaHasta').focus();
        });

        $('#panelFiltroPendiente').on('hide.bs.collapse', function (e) {
            $("#iconoFiltroPendiente").removeClass("glyphicon-menu-up");
            $("#iconoFiltroPendiente").addClass("glyphicon-menu-down");
        });
        $('#panelFiltroPendiente').on('show.bs.collapse', function (e) {
            $("#iconoFiltroPendiente").removeClass("glyphicon-menu-down");
            $("#iconoFiltroPendiente").addClass("glyphicon-menu-up");
        });
        PendientePago.CargarPendientePago([]);
        PendientePago.BuscarDocumentosUsuario();
    },
    BuscarDocumentosFiltro: function () {
        if ($('#txtPendienteFechaDesde').val() == "" &&
            $('#txtPendienteFechaHasta').val() == "" &&
            $('#txtPendienteFicha').val() == "" &&
            $('#txtPendienteEpisodio').val() == "" &&
            $('#txtPendientePaciente').val() == "" &&
            $('#txtPendienteResponsable').val() == "" &&
            $('#txtPendienteSocioNegocio').val() == "" &&
            $('#txtPendienteFolio').val() == "" &&
            $('#txtPendienteComprobante').val() == "") {
            Mensaje.Alerta("Debe ingresar al menos uno de los filtros para buscar.");
        }
        else {
            PendientePago.LimpiarPendientePago();
            PendientePago.CargarPendientePago([]);

            var estadoDocumento = $('input[type="radio"][name="estadoDocumento"]:checked').val();
            var jsonPagoPendiente = {
                FechaDesde: $('#txtPendienteFechaDesde').val(),
                FechaHasta: $('#txtPendienteFechaHasta').val(),
                FichaClinica: $('#txtPendienteFicha').val(),
                Episodio: $('#txtPendienteEpisodio').val(),
                Paciente: $('#txtPendientePaciente').val(),
                Responsable: $('#txtPendienteResponsable').val(),
                SocioNegocio: $('#txtPendienteSocioNegocio').val(),
                NumeroFolio: $('#txtPendienteFolio').val().trim(),
                NumeroComprobante: $('#txtPendienteComprobante').val().trim(),
                Estado: estadoDocumento,
                AsignacionId: $("#idAsignacion").val(),
                TipoDocumento: $('#dllTipoDocumento').val()
            };
            var postData = { filtro: jsonPagoPendiente };
            $.ajax({
                url: '/PuntoVenta/ListaDocumentosFiltro',
                type: 'POST',
                cache: false,
                data: JSON.stringify(postData),
                contentType: 'application/json; charset=utf-8',
                success: function (respuesta) {
                    if (respuesta.EsError) {
                        Mensaje.Alerta(respuesta.Mensaje);
                    }
                    else {
                        PendientePago.CargarPendientePago(respuesta.Data);
                    }
                }
            });
        }
    },
    BuscarDocumentosUsuario: function () {
        PendientePago.LimpiarPendientePago();
        PendientePago.CargarPendientePago([]);

        $.ajax({
            url: '/PuntoVenta/ListaDocumentosUsuario',
            type: 'GET',
            cache: false,
            contentType: 'application/json; charset=utf-8',
            success: function (respuesta) {
                if (respuesta.EsError) {
                    Mensaje.Alerta(respuesta.Mensaje);
                }
                else {
                    PendientePago.CargarPendientePago(respuesta.Data);
                }
            }
        });
    },
    CargarPendientePago: function (data) {

        $('#listPendientesPago').DataTable().destroy();
        tablaPendiente = $('#listPendientesPago').DataTable({
            "language": dataTableEspanol,
            "bAutoWidth": false,
            "bFilter": false,
            "bInfo": true,
            "bLengthChange": false,
            "bPaginate": true,
            "bProcessing": false,
            "bSort": true,
            "sScrollX": "100%",
            "bScrollCollapse": true,


            "order": [[0, 'asc'], [1, 'desc']],

            "aaData": data,
            "aoColumns":
                [
                    { mDataProp: "TipoTexto", bVisible: false },
                    {
                        sDefaultContent: "--",
                        className: "text-nowrap",
                        mRender: function (data, type, full) {
                            return '<span style="display:none">' + full.FechaTransaccionCorrelativo + '</span>' +
                                (full.Tipo == 4 ? full.FechaTransaccionTexto.split(' ')[0] :
                                    full.FechaTransaccionTexto.split(' ')[0] + '<br/>' +
                                    full.FechaTransaccionTexto.split(' ')[1]);
                        }
                    },
                    { mDataProp: "Episodio", sDefaultContent: "--" },
                    { mDataProp: "IdTrack", sDefaultContent: "--", className: "text-right" },
                    {
                        className: "text-right text-nowrap",
                        mRender: function (data, type, full) {
                            return '<div style="width: 85px">' + (full.RutPaciente == "" ? '--' : full.RutPaciente) + '</div>';
                        }
                    },
                    {
                        mRender: function (data, type, full) {
                            return '<div style="width: 200px; word-wrap: break-word">' + (full.PacienteNombre == "" ? '--' : full.PacienteNombre) + '</div>';
                        }
                    },
                    {
                        className: "text-right text-nowrap",
                        mRender: function (data, type, full) {
                            return '<div style="width: 85px">' + (full.RutResponsable == "" ? '--' : full.RutResponsable) + '</div>';
                        }
                    },
                    {
                        mRender: function (data, type, full) {
                            return '<div style="width: 200px; word-wrap: break-word">' + (full.ResponsableNombre == "" ? '--' : full.ResponsableNombre) + '</div>';
                        }
                    },
                    {
                        className: "text-center text-nowrap",
                        mRender: function (data, type, full) {
                            return '<span style="display: none">' + full.AsientoCuotaCorrelativo + '</span>' + (full.AsientoCuota == "" ? "N/A" : full.AsientoCuota);
                        }
                    },
                    {
                        mDataProp: "MontoPago",
                        className: "text-right text-nowrap",
                        render: $.fn.dataTable.render.number('.', ',', 0, '$')
                    },
                    {
                        sDefaultContent: "",
                        bSortable: false,
                        mRender: function (data, type, full, meta) {
                            var dataFila = ' data-docentry="' + full.DocEntry + '"';
                            dataFila += ' data-docnum="' + full.DocNum + '"';
                            dataFila += ' data-trackid="' + full.IdTrack + '"';
                            dataFila += ' data-episodio="' + full.Episodio + '"';
                            dataFila += ' data-tipo="' + full.Tipo + '"';
                            dataFila += ' data-paciente="' + (full.PacienteNombre != null ? full.PacienteNombre : "") + '"';
                            dataFila += ' data-responsable="' + (full.ResponsableNombre != null ? full.ResponsableNombre : "") + '"';
                            dataFila += ' data-socionegocio="' + (full.SocioNegocioCode != null ? full.SocioNegocioCode : full.ResponsableRut) + '"';
                            dataFila += ' data-total="' + (full.MontoPago != null ? full.MontoPago : "0") + '"';
                            dataFila += ' data-asientonumero="' + full.AsientoNumero + '"';
                            dataFila += ' data-asientolinea="' + full.AsientoLinea + '"';
                            dataFila += ' data-tienecuotapendiente="' + full.TieneCuotaPendiente + '"';
                            dataFila += ' data-fila="' + meta.row + '"';
                            dataFila += ' data-numerofolio="' + full.NumeroFolio + '"';

                            var resultado = "";

                            var btnAnuPagPayment = '<div class="btn-group"><button type="button" style="padding-left: 0px; padding-right: 0px;" class="btn btn-sm btn-danger" title="Anular pagos" onclick="AnularPago.AbrirFormularioLista(' + full.Tipo + ',' + full.DocEntry + ', null,' + full.IdTrack + ')"><span class="glyphicon glyphicon-ban-circle"></span></button></div>';
                            var btnAnuPagAsiento = '<div class="btn-group"><button type="button" style="padding-left: 0px; padding-right: 0px;" class="btn btn-sm btn-danger" title="Anular pagos" onclick="AnularPago.AbrirFormularioLista(' + full.Tipo + ',' + full.AsientoNumero + ',' + full.AsientoLinea + ', null)"><span class="glyphicon glyphicon-ban-circle"></span></button></div>';

                            var btnAnuPagRecibido = full.NumeroComprobante != null ? '<div class="btn-group"><button type="button" style="padding-left: 0px; padding-right: 0px;" class="btn btn-sm btn-danger" title="Anular pagos" onclick="AnularPago.AbrirFormularioPago(' + full.Tipo + ',' + full.NumeroComprobante + ')"><span class="glyphicon glyphicon-ban-circle"></span></button></div>' : '';

                            if (full.Pagado) {
                                if (full.Tipo == 1) {//Orden de Venta
                                    var btnImp = '<div class="btn-group"><button type="button" style="padding-left: 0px; padding-right: 0px;" class="btn btn-sm btn-info" title="Imprimir" onclick="VisorReporte(' + "'OrdenAtencionORDR.rpt'" + ',' + full.IdTrack + ',' + "'DocEntry'" + ')"><span class="glyphicon glyphicon-print"></span></button></div>';
                                    var btnAnu = '<div class="btn-group"><button type="button" style="padding-left: 0px; padding-right: 0px;" class="btn btn-sm btn-danger" title="Anular" onclick="PendientePago.ConfirmacionAnularVenta(' + full.DocEntry + ',' + full.IdTrack + ',' + "'" + full.Episodio + "'" + ')"><span class="glyphicon glyphicon-remove"></span></button></div>';
                                    resultado = btnImp + btnAnu;
                                }
                                else if (full.Tipo == 2) {//Boleta
                                    var btnImp = "";
                                    if (full.BoletaAsociada != "Y") {
                                        btnImp = '<div class="btn-group"><button type="button" style="padding-left: 0px; padding-right: 0px;" class="btn btn-sm btn-info" title="Imprimir" onclick="VisorReporte(' + "'OrdenAtencion.rpt'" + ',' + full.DocEntry + ',' + "'DocEntry'" + ')"><span class="glyphicon glyphicon-print"></span></button></div>';
                                    }
                                    var btnAnu = '<div class="btn-group"><button type="button" style="padding-left: 0px; padding-right: 0px;" class="btn btn-sm btn-danger" title="Anular documento" onclick="PendientePago.ConfirmacionAnularVenta(' + full.DocEntry + ',' + full.IdTrack + ',' + "'" + full.Episodio + "'" + ')"><span class="glyphicon glyphicon-remove"></span></button></div>';

                                    resultado = btnImp + btnAnu + btnAnuPagPayment;
                                }
                                else if (full.Tipo == 3 || full.Tipo == 4) { //Convenio || Pagaré
                                    var btnImp = '<div class="btn-group"><button type="button" style="padding-left: 0px; padding-right: 0px;" class="btn btn-sm btn-info" title="Imprimir" onclick="PendientePago.ImprimirPagoFinal(' + full.Tipo + ',' + full.AsientoNumero + ',' + full.AsientoLinea + ')"><span class="glyphicon glyphicon-print"></span></button></div>';
                                    resultado = btnImp + btnAnuPagAsiento;
                                }
                                else if (full.Tipo == 5) { //Factura
                                    var btnImp = '<div class="btn-group"><button type="button" style="padding-left: 0px; padding-right: 0px;" class="btn btn-sm btn-info" title="Imprimir" onclick="PendientePago.ImprimirPagoFinal(' + full.Tipo + ',' + full.DocEntry + ',null)"><span class="glyphicon glyphicon-print"></span></button></div>';
                                    resultado = btnImp + btnAnuPagPayment;
                                }
                                else if (full.Tipo == 6) { //Anticipo
                                    var btnImp = '<div class="btn-group"><button type="button" style="padding-left: 0px; padding-right: 0px;" class="btn btn-sm btn-info" title="Imprimir" onclick="PendientePago.ImprimirPagoFinal(' + full.Tipo + ',' + full.DocEntry + ',null)"><span class="glyphicon glyphicon-print"></span></button></div>';
                                    resultado = btnImp + btnAnuPagRecibido;
                                }
                                else if (full.Tipo == 7) {//IMED
                                    var btnImp = '<div class="btn-group"><button type="button" style="padding-left: 0px; padding-right: 0px;" class="btn btn-sm btn-info" title="Imprimir" onclick="VisorReporte(' + "'OrdenAtencionORDR.rpt'" + ',' + full.IdTrack + ',' + "'DocEntry'" + ')"><span class="glyphicon glyphicon-print"></span></button></div>';
                                    var btnAnu = '<div class="btn-group"><button type="button" style="padding-left: 0px; padding-right: 0px;" class="btn btn-sm btn-danger" title="Anular" onclick="PendientePago.ConfirmacionAnularVenta(' + full.DocEntry + ',' + full.IdTrack + ',' + "'" + full.Episodio + "'" + ')"><span class="glyphicon glyphicon-remove"></span></button></div>';
                                    resultado = btnImp + btnAnu + btnAnuPagPayment;
                                }
                            }
                            else {
                                if (full.Tipo == 1) { //Orden de Venta
                                    var btnImp = '<div class="btn-group"><button type="button" style="padding-left: 0px; padding-right: 0px;" class="btn btn-sm btn-info" title="Imprimir" onclick="PendientePago.ConfirmaImprimirOrdenAtencion(' + full.DocEntry + ',' + full.IdTrack + ')"><span class="glyphicon glyphicon-print"></span></button></div>';
                                    var btnAnu = '<div class="btn-group"><button type="button" style="padding-left: 0px; padding-right: 0px;" class="btn btn-sm btn-danger" title="Anular" onclick="PendientePago.ConfirmacionAnularVenta(' + full.DocEntry + ',' + full.IdTrack + ',' + "'" + full.Episodio + "'" + ')"><span class="glyphicon glyphicon-remove"></span></button></div>';
                                    resultado = btnImp + btnAnu;
                                }
                                else if (full.Tipo == 2) { //Boleta
                                    var btnPag = "";
                                    if ($("#estadoCaja").val() != "Cerrado Contado") {
                                        btnPag = '<div class="btn-group"><button type="button" style="padding-left: 0px; padding-right: 0px;" class="btn btn-sm btn-success" title="Completar pago" ' + dataFila + ' onclick="PendientePago.PagoSeleccionado(this);" ><span class="glyphicon glyphicon-usd"></span</button></div>';
                                    }
                                    var btnAnu = '<div class="btn-group"><button type="button" style="padding-left: 0px; padding-right: 0px;" class="btn btn-sm btn-danger" title="Anular" onclick="PendientePago.ConfirmacionAnularVenta(' + full.DocEntry + ',' + full.IdTrack + ',' + "'" + full.Episodio + "'" + ')"><span class="glyphicon glyphicon-remove"></span></button></div>';
                                    resultado = btnPag + btnAnu;
                                }
                                else if (full.Tipo == 3 || full.Tipo == 4) { //Pagare || Convenio
                                    var btnPag = "";
                                    if ($("#estadoCaja").val() != "Cerrado Contado") {
                                        btnPag = '<div class="btn-group"><button type="button" style="padding-left: 0px; padding-right: 0px;" class="btn btn-sm btn-success" title="Completar pago" ' + dataFila + ' onclick="PendientePago.PagoConfirmarCuota(this);" ><span class="glyphicon glyphicon-usd"></span></button></div>';
                                    }
                                    var btnImp = '<div class="btn-group"><button type="button" style="padding-left: 0px; padding-right: 0px;" class="btn btn-sm btn-info" title="Imprimir" onclick="PendientePago.ImprimirPagoFinal(' + full.Tipo + ',' + full.AsientoNumero + ',' + full.AsientoLinea + ')"><span class="glyphicon glyphicon-print"></span></button></div>';


                                    //JMif (full.Impreso) {
                                    //    btnImp = '<div class="btn-group"><button type="button" style="padding-left: 0px; padding-right: 0px;" class="btn btn-sm btn-info" title="Imprimir" onclick="PendientePago.ImprimirPagoFinal(' + full.Tipo + ',' + full.AsientoNumero + ',' + full.AsientoLinea + ')"><span class="glyphicon glyphicon-print"></span></button></div>';
                                    //}

                                    resultado = btnPag + (full.Impreso ? (btnImp + btnAnuPagAsiento) : "");// Si fue impreso tiene pago asociado.
                                }
                                else if (full.Tipo == 5) { // Factura
                                    var btnPag = "";
                                    if ($("#estadoCaja").val() != "Cerrado Contado") {
                                        btnPag = '<div class="btn-group"><button type="button" style="padding-left: 0px; padding-right: 0px;" class="btn btn-sm btn-success" title="Completar pago" ' + dataFila + ' onclick="PendientePago.PagoSeleccionado(this);" ><span class="glyphicon glyphicon-usd"></span></button></div>';
                                    }
                                    var btnImp = '<div class="btn-group"><button type="button" style="padding-left: 0px; padding-right: 0px;" class="btn btn-sm btn-info" title="Imprimir" onclick="PendientePago.ImprimirPagoFinal(' + full.Tipo + ',' + full.DocEntry + ',null)"><span class="glyphicon glyphicon-print"></span></button></div>';

                                    //JMif (full.Impreso) {
                                    //    btnImp = '<div class="btn-group"><button type="button" style="padding-left: 0px; padding-right: 0px;" class="btn btn-sm btn-info" title="Imprimir" onclick="PendientePago.ImprimirPagoFinal(' + full.Tipo + ',' + full.DocEntry + ',null)"><span class="glyphicon glyphicon-print"></span></button></div>';
                                    //}

                                    resultado = btnPag + (full.Impreso ? (btnImp + btnAnuPagPayment) : ""); // Si fue impreso tiene pago asociado.
                                }
                                else if (full.Tipo == 6) { //Anticipo
                                    var btnPag = "";
                                    if ($("#estadoCaja").val() != "Cerrado Contado") {
                                        btnPag = '<div class="btn-group"><button type="button" style="padding-left: 0px; padding-right: 0px;" class="btn btn-sm btn-success" title="Completar pago" ' + dataFila + ' onclick="PendientePago.PagoSeleccionado(this);" ><span class="glyphicon glyphicon-usd"></span></button></div>';
                                    }
                                    var btnDev = '<div class="btn-group"><button type="button" style="padding-left: 0px; padding-right: 0px;" class="btn btn-sm btn-danger" title="Devolver anticipo" onclick="PendientePago.ConfirmacionAnularAnticipo(' + full.DocEntry + ')"><span class="glyphicon glyphicon-share-alt"></span></button></div>';
                                    resultado = btnPag + btnDev;
                                }
                                else if (full.Tipo == 7) { //IMED

                                    var btnAnu = '<div class="btn-group"><button type="button" style="padding-left: 0px; padding-right: 0px;" class="btn btn-sm btn-danger" title="Anular" onclick="PendientePago.ConfirmacionAnularVenta(' + full.DocEntry + ',' + full.IdTrack + ',' + "'" + full.Episodio + "'" + ')"><span class="glyphicon glyphicon-remove"></span></button></div>';
                                    var btnPag = "";

                                    if ($("#estadoCaja").val() != "Cerrado Contado") {
                                        if (full.BoletaAsociada == "Y") {
                                            btnPag = '<div class="btn-group"><button type="button" style="padding-left: 0px; padding-right: 0px;" class="btn btn-sm btn-success" title="Completar pago" ' + dataFila + ' onclick="Mensaje.Alerta(\'Recuerde que debe realizar el pago de la boleta antes de imprimir la orden\');" ><span class="glyphicon glyphicon-usd"></span></button></div>';
                                        } else {
                                            btnPag = '<div class="btn-group"><button type="button" style="padding-left: 0px; padding-right: 0px;" class="btn btn-sm btn-success" title="Completar pago" ' + dataFila + ' onclick="PendientePago.PagoSeleccionado(this);" ><span class="glyphicon glyphicon-usd"></span></button></div>';
                                        }
                                    }
                                    //JMvar btnImp = '<div class="btn-group"><button type="button" style="padding-left: 0px; padding-right: 0px;" class="btn btn-sm btn-info" title="Imprimir" onclick="PendientePago.ConfirmaImprimirOrdenAtencion(' + full.DocEntry + ',' + full.IdTrack + ')"><span class="glyphicon glyphicon-print"></span></button></div>';

                                    if (full.MontoPago == 0) { // En vez de pagar cambios a Imprimir
                                        btnPag = '<div class="btn-group"><button type="button" style="padding-left: 0px; padding-right: 0px;" class="btn btn-sm btn-info" title="Imprimir" onclick="PendientePago.ConfirmaImprimirOrdenAtencion(' + full.DocEntry + ',' + full.IdTrack + ')"><span class="glyphicon glyphicon-print"></span></button></div>';
                                    }

                                    resultado = btnPag + btnAnu;
                                }
                            }
                            return '<div class="btn-group btn-group-justified" style="width: 80px">' + resultado + '</div>';
                        },
                    },
                    { mDataProp: "NumeroFolio", sDefaultContent: "--", className: "text-right" },
                    {
                        className: "text-right text-nowrap",
                        mRender: function (data, type, full) {
                            return '<div style="width: 85px">' + (full.SocioNegocioCode == "" ? '--' : full.SocioNegocioCode) + '</div>';
                        }
                    },
                    {
                        mRender: function (data, type, full) {
                            return '<div style="width: 200px; word-wrap: break-word">' + (full.SocioNegocioName == "" ? '--' : full.SocioNegocioName) + '</div>';
                        }
                    },
                ],
            "fnCreatedRow": function (nRow, aData, iDataIndex) {
                nRow.dataset.pendiente = iDataIndex;
                nRow.className = 'filaPagoPendiente';
            },
            "fnRowCallback": function (nRow, aData, iDisplayIndex, iDisplayIndexFull) {
                if (nRow.dataset.pendiente != $("#PagoSeleccionadoFila").val()) {
                    nRow.className = nRow.className.replace("selected", "");
                }
            },
            "drawCallback": function (settings) {
                var api = this.api();
                var rows = api.rows({ page: 'current' }).nodes();
                var last = null;

                api.column(0, { page: 'current' }).data().each(function (group, i) {
                    if (last !== group) {
                        $(rows).eq(i).before(
                            '<tr class="group filaGrupo" style="cursor: pointer"><td colspan="13"><strong>' + group + '</strong></td></tr>'
                        );
                        last = group;
                    }
                });
            }
        });
    },

    LimpiarBusqueda: function () {
        $('#txtPendienteFechaDesde').val("");
        $('#txtPendienteFechaHasta').val("");
        $('#txtPendienteFicha').val("");
        $('#txtPendienteEpisodio').val("");
        $('#txtPendientePaciente').val("");
        $('#txtPendienteResponsable').val("");
        $('#txtPendienteSocioNegocio').val("");
        $('#txtPendienteFolio').val("");
        $('#txtPendienteComprobante').val("");
        $('#dllTipoDocumento').val("");

        $('input[type="radio"][name="estadoDocumento"]').each(function (index, item) {
            item.checked = (item.value == "0");
        });
    },
    LimpiarPendientePago: function () {
        $("#PagoSeleccionadoTipo").val("");
        $("#PagoSeleccionadoFila").val("");
        $("#PagoSeleccionadoId").val("");
        $("#PagoSeleccionadoTrack").val("");

        $("#AsientoSeleccionadoId").val("");
        $("#AsientoSeleccionadoLinea").val("");
        $("#SocioNegocioCode").val("");
        $('#BoletaImedAsociado').val("");
        $("#FolioAsignado").val("");

        $(".filaPagoPendiente").each(function (index, item) {
            $(item).removeClass('selected');
        });
        $('a[href="#tabPendiente"]').tab('show');

        DetalleVenta.LimpiarFormulario();

        PendientePago.HabilitarTab();
    },
    LimpiarFormularioPendiente: function () {
        if ($('#txtPendienteFechaDesde').val() == "" &&
            $('#txtPendienteFechaHasta').val() == "" &&
            $('#txtPendienteFicha').val() == "" &&
            $('#txtPendienteEpisodio').val() == "" &&
            $('#txtPendientePaciente').val() == "" &&
            $('#txtPendienteResponsable').val() == "" &&
            $('#txtPendienteSocioNegocio').val() == "" &&
            $('#txtPendienteFolio').val() == "" &&
            $('#txtPendienteComprobante').val() == "") {
            PendientePago.BuscarDocumentosUsuario();
        }
        else {
            PendientePago.BuscarDocumentosFiltro();
        }
        //JMPendientePago.LimpiarBusqueda();
        //PendientePago.BuscarDocumentosUsuario();
    },

    HabilitarTab: function () {
        if (!$("#navItemDetalle").hasClass("disabled")) {
            $("#navItemDetalle").addClass("disabled");
        }
        if (!$("#navItemPago").hasClass("disabled")) {
            $("#navItemPago").addClass("disabled");
        }
        $("#navItemDetalle a").attr('disabled', true);
        $("#navItemPago a").attr('disabled', true);


        if ($("#PagoSeleccionadoTipo").val() != "") {
            if ($("#PagoSeleccionadoTipo").val() == "2") { //Boleta

                $("#navItemDetalle").removeClass("disabled");
                $("#navItemDetalle a").removeAttr("disabled");
                $("#navItemPago").removeClass("disabled");
                $("#navItemPago a").removeAttr("disabled");
            }
            else if (
                $("#PagoSeleccionadoTipo").val() == "3" || //Pagare
                $("#PagoSeleccionadoTipo").val() == "4" || //Convenio
                $("#PagoSeleccionadoTipo").val() == "5" || //Factura
                $("#PagoSeleccionadoTipo").val() == "6" || //Anticipo
                $("#PagoSeleccionadoTipo").val() == "7") { //IMED

                $("#navItemPago").removeClass("disabled");
                $("#navItemPago a").removeAttr("disabled");
            }
        }

    },

    ActivarTab: function (tab) {
        $('.tab-pane a[href="#' + tab + '"]').tab('show');

        PendientePago.HabilitarTab();
    },
    PagoConfirmarCuota: function (item) {
        if (item.dataset.tienecuotapendiente == "true") {

            bootbox.confirm({
                message: '<span class="glyphicon glyphicon-question-sign" style="color: blue"></span>&nbsp;&nbsp;Tiene cuotas anteriores pendiente de pago. ¿Desea continuar?',
                buttons: {
                    confirm: {
                        label: 'Aceptar',
                        className: 'btn-primary'
                    },
                    cancel: {
                        label: 'Cancelar'
                    }
                },
                callback: function (result) {
                    if (result) {
                        PendientePago.PagoSeleccionado(item)
                    }
                }
            });
        }
        else {
            PendientePago.PagoSeleccionado(item);
        }
    },
    PagoSeleccionado: function (item) {
        $(".filaPagoPendiente").each(function (index, fila) {
            $(fila).removeClass('selected');

            if (fila.dataset.pendiente == item.dataset.fila) {
                $(fila).addClass('selected');
            }
        });

        if ($("#PagoSeleccionadoId").val() == item.dataset.docentry &&
            $("#PagoSeleccionadoTipo").val() == item.dataset.tipo) {
            if (item.dataset.tipo == 2) {//Boleta
                PendientePago.ActivarTab("tabDetalle");
            }
            else if (item.dataset.tipo == 3 ||
                item.dataset.tipo == 4 ||
                item.dataset.tipo == 5 ||
                item.dataset.tipo == 6 ||
                item.dataset.tipo == 7) {//Pagare || Convenio || Factura || Anticipo || IMED
                PendientePago.ActivarTab("tabPago");
            }
        }
        else {
            MedioPago.PagoLimpiar();

            $("#PagoSeleccionadoTipo").val(item.dataset.tipo);
            $("#PagoSeleccionadoFila").val(item.dataset.fila);
            $("#PagoSeleccionadoId").val(item.dataset.docentry);
            $("#PagoSeleccionadoTrack").val(item.dataset.trackid);

            $("#SocioNegocioCode").val(item.dataset.socionegocio);
            $("#AsientoSeleccionadoId").val(item.dataset.asientonumero);
            $("#AsientoSeleccionadoLinea").val(item.dataset.asientolinea);
            $("#FolioAsignado").val(item.dataset.numerofolio == "null" ? "" : item.dataset.numerofolio);


            if (item.dataset.tipo == 2) {//Boleta 
                DetalleVenta.CargaDetalle(item.dataset.docentry, item.dataset.total);
            }
            else if (item.dataset.tipo == 3 ||
                item.dataset.tipo == 4 ||
                item.dataset.tipo == 5 ||
                item.dataset.tipo == 6 ||
                item.dataset.tipo == 7) {//Pagare || Convenio || Factura || Anticipo || IMED

                PendientePago.ActivarTab("tabPago");

                MedioPago.PagoDetalleNormal(
                    "0",
                    item.dataset.total,
                    item.dataset.paciente,
                    item.dataset.responsable);
            }
        }
    },

    ConfirmacionAnularVenta: function (docEntry, idTrack, episodio) {
        bootbox.dialog({
            message: '<span class="glyphicon glyphicon-question-sign" style="color: blue"></span>&nbsp;&nbsp;¿Está seguro de anular esta transacción?<br/><br/><div class="form-group" ><label class="control-label">Ingrese motivo de anulación:</label><textarea id="txtObservacionAnulacion" rows="3" class="form-control textUpperTrim" style="max-width: none" maxlength="250"></textarea></div>',
            buttons: {
                cancel: {
                    label: 'Cancelar',
                    className: 'btn-default'
                },
                confirm: {
                    label: 'Aceptar',
                    className: 'btn-primary',
                    callback: function (data) {
                        $("#txtObservacionAnulacion").focus();
                        if ($("#txtObservacionAnulacion").val() == "") {
                            Mensaje.Alerta("Debe ingresar el motivo de anulación.");
                            return false;
                        }
                        else {
                            PendientePago.AnularVenta(docEntry, idTrack, episodio, $("#txtObservacionAnulacion").val());
                        }
                    }
                }
            }
        });
    },
    AnularVenta: function (docEntry, idTrack, episodio, observacion) {
        $.ajax({
            url: '/PuntoVenta/AnularVentaTransaccion',
            type: 'GET',
            cache: false,
            data: {
                idTransaccion: docEntry,
                idTrack: idTrack,
                episodio: episodio,
                observacion: observacion,
                asignacionId: $("#idAsignacion").val()
            },
            contentType: 'application/json; charset=utf-8',
            success: function (result) {
                if (!result.EsError) {
                    Mensaje.Correcto(result.Mensaje);
                    PendientePago.LimpiarFormularioPendiente();
                }
                else {
                    Mensaje.Alerta(result.Mensaje);
                }
            }
        });
    },

    ConfirmacionAnularAnticipo: function (docEntry) {
        bootbox.dialog({
            message: '<span class="glyphicon glyphicon-question-sign" style="color: blue"></span>&nbsp;&nbsp;¿Está seguro de anular esta transacción?<br/><br/><div class="form-group" ><label class="control-label">Ingrese motivo de anulación:</label><textarea id="txtObservacionAnulacionAnticipo" rows="3" class="form-control textUpperTrim" style="max-width: none" maxlength="250"></textarea></div>',
            buttons: {
                cancel: {
                    label: 'Cancelar',
                    className: 'btn-default'
                },
                confirm: {
                    label: 'Aceptar',
                    className: 'btn-primary',
                    callback: function (data) {
                        $("#txtObservacionAnulacionAnticipo").focus();
                        if ($("#txtObservacionAnulacionAnticipo").val() == "") {
                            Mensaje.Alerta("Debe ingresar el motivo de anulación.");
                            return false;
                        }
                        else {
                            PendientePago.AnularAnticipo(docEntry, $("#txtObservacionAnulacionAnticipo").val());
                        }
                    }
                }
            }
        });
    },
    AnularAnticipo: function (docEntry, observacion) {
        $.ajax({
            url: '/PuntoVenta/AnularVentaAnticipo',
            type: 'GET',
            cache: false,
            data: {
                idTransaccion: docEntry,
                observacion: observacion
            },
            contentType: 'application/json; charset=utf-8',
            success: function (result) {
                if (!result.EsError) {
                    PendientePago.LimpiarFormularioPendiente();
                    Mensaje.Correcto(result.Mensaje);
                }
                else {
                    Mensaje.Alerta(result.Mensaje);
                }
            }
        });
    },




    ImprimirDocumentoPago: function () {
        VisorReporteBoleta("BoletaServicioOki.rpt", $("#PagoSeleccionadoId").val(), "DocEntry");
        VisorReporte("BoletaServicioOki.rpt", $("#PagoSeleccionadoId").val(), "DocEntry");
        if ($("#BoletaImedAsociado").val() != "Y") {
            VisorReporte("OrdenAtencion.rpt", $("#PagoSeleccionadoId").val(), "DocEntry"); //para los casos de IMED + Boleta
        }
        PendientePago.LimpiarFormularioPendiente();
    },
    ImprimirPagoRecibido: function () {
        if ($("#PagoSeleccionadoTipo").val() == "6") { //Anticipo
            VisorReporte("ComprobantePagoAnticipo.rpt", $("#PagoSeleccionadoId").val(), "DocEntry");
        }
        else if ($("#PagoSeleccionadoTipo").val() == "5") { //Factura
            VisorReporte("ComprobantePago.rpt", $("#PagoSeleccionadoId").val(), "DocEntry");
        }
        else if (
            $("#PagoSeleccionadoTipo").val() == "3" ||
            $("#PagoSeleccionadoTipo").val() == "4") { // Convenio | Pagare

            var listParametros = [
                { value: $("#AsientoSeleccionadoId").val(), text: 'AsientoId' },
                { value: $("#AsientoSeleccionadoLinea").val(), text: 'AsientoLinea' },
            ];

            VisorReporteMultipleParametros("ComprobantePagoAsiento.rpt", listParametros);
        }
        PendientePago.LimpiarFormularioPendiente();
    },
    ImprimirPagoFinal: function (TipoPago, Param1, Param2) {
        if (TipoPago == "6") { //Anticipo
            VisorReporte("ComprobantePagoAnticipo.rpt", Param1, "DocEntry");
        }
        else if (TipoPago == "5") { //Factura
            VisorReporte("ComprobantePago.rpt", Param1, "DocEntry");
        }
        else if (TipoPago == "3" || TipoPago == "4") { // Convenio | Pagare
            var listParametros = [
                { value: Param1, text: 'AsientoId' },
                { value: Param2, text: 'AsientoLinea' },
            ];

            VisorReporteMultipleParametros("ComprobantePagoAsiento.rpt", listParametros);
        }
    },

    ConfirmaImprimirOrdenAtencion: function (_docentry, _idtrack) {
        bootbox.confirm({
            message: '<span class="glyphicon glyphicon-question-sign" style="color: blue"></span>&nbsp;&nbsp;¿Está seguro de imprimir?',
            buttons: {
                confirm: { label: 'Aceptar', className: 'btn-primary' },
                cancel: { label: 'Cancelar' }
            },
            callback: function (result) {
                if (result) {
                    PendientePago.ImprimirOrdenAtencion(_docentry, _idtrack);
                }
            }
        });
    },
    ImprimirOrdenAtencion: function (_docentry, _idtrack) {
        var imprimir = {
            ReporteParametro: _idtrack,
            IdAsignacionCaja: $("#idAsignacion").val(),
            IdTrack: _idtrack
        };

        var impresion = JSON.stringify(imprimir);

        $.ajax({
            url: '/PuntoVenta/ImpresionOrdenAtencion',
            type: 'GET',
            cache: false,
            data: { imprimir: impresion },
            contentType: 'application/json; charset=utf-8',
            success: function (data) {
                if (data.EsError) {
                    Mensaje.Alerta(data.Mensaje);
                }
                else {
                    PendientePago.ImprimirOrden(_idtrack);
                }
            }
        });
    },
    ImprimirOrden: function (parametro) {
        VisorReporte("OrdenAtencionORDR.rpt", parametro, "DocEntry");
        PendientePago.LimpiarFormularioPendiente();
    }
}
