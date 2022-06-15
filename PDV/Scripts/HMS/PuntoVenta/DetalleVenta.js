var footerSubTotal = 0;
var footerBonificacionFinanciador = 0;
var footerBonificacionComplementaria = 0;
var footerCopagoBeneficiario = 0;

var pagoMontoBonificacionFinanciador = "";
var pagoMontoCopagoBeneficiario = 0;

var DetalleVenta = {
    CargaDetalle: function (idTransaccion, monto) {
        DetalleVenta.LimpiarFormulario();

        $.ajax({
            url: '/PuntoVenta/TraerDatosVenta',
            type: 'GET',
            cache: false,
            data: { idTransaccion: idTransaccion },
            contentType: 'application/json; charset=utf-8',
            success: function (result) {
                if (!result.EsError) {
                    var info = result.Data;

                    if (info.NroGarantia != "") {
                        Mensaje.Alerta("Para procesar el pago, se debe Devolver o Anular la Garantía " + result.Data.NroGarantia);
                        PendientePago.LimpiarPendientePago();
                    }
                    else {
                        if (info.Tipo == 2) {
                            $("#btnAgregarGarantiaVenta").show();
                        }
                        else {
                            $("#btnAgregarGarantiaVenta").hide();
                        }
                        PendientePago.ActivarTab("tabDetalle");

                        var DateTransaccion = (info.FechaTransaccionTexto).split(' ');
                        var DateTransaccionFormato = DateTransaccion[0].split('-');
                        var fechaFinal = DateTransaccionFormato[2] + '/' + DateTransaccionFormato[1] + '/' + DateTransaccionFormato[0];

                        $('#TxtFechaEmisionDetalleVenta').val(fechaFinal);
                        $('#TxtFichaClinicaDetalleVenta').val(info.FichaClinica);
                        $('#TxtEpisodioDetalleVenta').val(info.Episodio);
                        $('#TxtNombrePacienteDetalleVenta').val(info.PacienteNombre);
                        $('#TxtRutPacienteDetalleVenta').val(info.RutPaciente);
                        $('#TxtBono').val(info.NumeroBono);
                        $('#TxtNombreResponsableDetalleVenta').val(info.ResponsableNombre);
                        $('#TxtRutResponsableDetalleVenta').val(info.RutResponsable);
                        $('#TxtCategoriaConvenio').val((info.CategoriaConvenio).toUpperCase());
                        $('#TxtTarifaDetalleVenta').val((info.TarifaTransaccion).toUpperCase());
                        $('#TxtPrevisionDetalleVenta').val((info.PrevisionResponsable).toUpperCase());
                        $('#TxtNombreMedicoDetalleVenta').val(info.NombreMedico);
                        $('#TxtEspecialidadMedicoDetalleVenta').val((info.EspecialidadMedico).toUpperCase());
                        $('#TxtServicioMedicoDetalleVenta').val((info.ServicioAtencion).toUpperCase());
                        $('#TxtCodigoUnidadDetalleVenta').val(info.CodigoUnidad);
                        $('#BoletaImedAsociado').val(info.BoletaAsociada);
                        $('#hdnPacienteDireccion').val(info.DireccionPaciente);
                        $('#hdnPacienteComuna').val(info.ComunaPaciente);
                        $('#hdnTotalDocumento').val(monto);
                        DetalleVenta.CargaGrilla(info.DetalleAtencion);

                        MedioPago.PagoDetalleNormal(
                            pagoMontoBonificacionFinanciador,
                            monto,
                            info.PacienteNombre,
                            info.ResponsableNombre);
                    }
                }
                else {
                    Mensaje.Alerta(result.Mensaje);
                }
            }
        });
    },
    CargaGrilla: function (data) {
        $('#listDetalleVenta').DataTable().clear();
        $('#listDetalleVenta').DataTable({
            "language": dataTableEspanol,
            "bFilter": false,
            "bInfo": false,
            "bLengthChange": false,
            "bPaginate": false,
            "bProcessing": false,
            "bSort": false,
            "bDestroy": true,
            "bAutoWidth": false,

            "aaData": data,
            "aoColumns":
                [
                    { mDataProp: "CodigoPrestacion" },
                    { mDataProp: "DescripcionPrestacion" },
                    { mDataProp: "CentroCosto" },
                    { mDataProp: "Cantidad" },
                    {
                        sClass: "text-right",
                        mRender: function (data, type, full) {
                            return "$ " + Moneda(full.ValorUnitario.toString());
                        }
                    },
                    {
                        sClass: "text-right",
                        mRender: function (data, type, full) {
                            footerSubTotal += intVal(full.SubTotal);
                            return "$ " + Moneda(full.SubTotal.toString());
                        }
                    },
                    {
                        sClass: "text-right",
                        mRender: function (data, type, full) {
                            footerBonificacionFinanciador += intVal(full.BonificacionFinanciador);
                            return "$ " + Moneda(full.BonificacionFinanciador.toString());
                        }
                    },
                    {
                        sClass: "text-right",
                        mRender: function (data, type, full) {
                            footerBonificacionComplementaria += intVal(full.BonificacionComplemnentaria);
                            return "$ " + Moneda(full.BonificacionComplemnentaria.toString());
                        }
                    },
                    {
                        sClass: "text-right",
                        mRender: function (data, type, full) {
                            footerCopagoBeneficiario += (full.CopagoBeneficiario);
                            return "$ " + Moneda(full.CopagoBeneficiario.toString());
                        }
                    }
                ],
            "fnFooterCallback": function (nFoot, aData, iStart, iEnd, aiDisplay) {
                var api = this.api(), data;
                $(api.column(5).footer()).html('$ ' + Moneda(footerSubTotal.toString()));
                $(api.column(6).footer()).html('$ ' + Moneda(footerBonificacionFinanciador.toString()));
                $(api.column(7).footer()).html('$ ' + Moneda(footerBonificacionComplementaria.toString()));
                $(api.column(8).footer()).html('$ ' + Moneda(footerCopagoBeneficiario.toString()));

                pagoMontoBonificacionFinanciador = footerBonificacionFinanciador.toString();
                pagoMontoCopagoBeneficiario = footerCopagoBeneficiario;
            },
        });
    },
    LimpiarFormulario: function () {
        pagoMontoBonificacionFinanciador = "";
        pagoMontoCopagoBeneficiario = 0;

        footerSubTotal = 0;
        footerBonificacionFinanciador = 0;
        footerBonificacionComplementaria = 0;
        footerCopagoBeneficiario = 0;

        $('#TxtFechaEmisionDetalleVenta').val("");
        $('#TxtFichaClinicaDetalleVenta').val("");
        $('#TxtEpisodioDetalleVenta').val("");
        $('#TxtNombrePacienteDetalleVenta').val("");
        $('#TxtRutPacienteDetalleVenta').val("");
        $('#TxtBono').val("");
        $('#TxtNombreResponsableDetalleVenta').val("");
        $('#TxtRutResponsableDetalleVenta').val("");
        $('#TxtCategoriaConvenio').val("");
        $('#TxtTarifaDetalleVenta').val("");
        $('#TxtPrevisionDetalleVenta').val("");
        $('#TxtNombreMedicoDetalleVenta').val("");
        $('#TxtEspecialidadMedicoDetalleVenta').val("");
        $('#TxtServicioMedicoDetalleVenta').val("");
        $('#TxtCodigoUnidadDetalleVenta').val("");

        $('#hdnPacienteDireccion').val("");
        $('#hdnPacienteComuna').val("");
        $('#hdnTotalDocumento').val("");
        DetalleVenta.CargaGrilla([]);
    },
    IngresarGarantia: function () {
        NuevaGarantia.AbrirFormulario(pagoMontoCopagoBeneficiario);
    },
    AplicarAnticipo: function () {
        AplicarAnticipo.AbrirFormulario($('#TxtEpisodioDetalleVenta').val(), $('#TxtRutResponsableDetalleVenta').val(), $('#TxtNombreResponsableDetalleVenta').val(), $('#hdnTotalDocumento').val());
    }
}