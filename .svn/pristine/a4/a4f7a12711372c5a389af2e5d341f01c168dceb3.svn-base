$(document).ready(function () {
    AdministraCajaSupervisor.LoadCajas();
    $('#modalAsignarCajasDiaAnterior').on('shown.bs.modal', function (e) {
        AdministraCajaSupervisor.ReloadCajasAsignadasDiaAnterior();
    })
});
var AdministraCajaSupervisor = {
    InitCajasHistorico: function () {
        $('#txtFechaInicio').datetimepicker({
            format: "DD/MM/YYYY",
            ignoreReadonly: true,
            showTodayButton: true,
            allowInputToggle: true,
            locale: "es"
        });
        $('#txtFechaFinal').datetimepicker({
            format: "DD/MM/YYYY",
            showTodayButton: true,
            ignoreReadonly: true,
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

        AdministraCajaSupervisor.LoadCajasAsignadasCargaGrilla([]);
    },
    InitCajasReasignar: function () {
        $('#txtFechaAsignada').datetimepicker({
            format: "DD/MM/YYYY",
            ignoreReadonly: true,
            showTodayButton: true,
            allowInputToggle: true,
            locale: "es"
        });
        $('#btnFechaAsignada').click(function () {
            $('#txtFechaAsignada').focus();
        });
        AdministraCajaSupervisor.BuscarCajasAsignadasSupervisor();
    },

    LoadCajas: function () {
        $('#listadoCajas').DataTable(
            {
                "language": dataTableEspanol,
                "bFilter": true,
                "bInfo": true,
                "bLengthChange": false,
                "bPaginate": true,
                "bProcessing": false,
                "bSort": true,

                "order": [[0, 'asc']],

                "sAjaxSource": '/Caja/ListaCajasSupervisor',
                "aoColumns":
                    [
                        { mDataProp: "CajaAsignada.IdentificacionCaja", bSortable: true },
                        { mDataProp: "CajaAsignada.FuncionalidadCaja", bSortable: true },
                        { mDataProp: "CajaAsignada.FuncionCaja", bSortable: true },
                        { mDataProp: "CajaAsignada.SeccionCaja", bSortable: true },
                        { mDataProp: "CajaAsignada.PisoCaja", bSortable: true },
                        { mDataProp: "UsuarioAsignado" },
                        { sDefaultContent: "-", mDataProp: "FechaCreacionTexto" },
                        { sDefaultContent: "-", mDataProp: "FechaInicioTexto" },
                        {
                            sDefaultContent: "",
                            bSortable: false,
                            mRender: function (data, type, row) {
                                var resultado = '<center><span class="glyphicon glyphicon-user" style="cursor: pointer; color: red" title="Caja iniciada no se puede editar"></span></center>';

                                if (row.FechaInicio == null) {
                                    var dataFila = 'data-IdCaja="' + row.CajaAsignada.IdCaja + '"';
                                    dataFila += ' data-IdAsignacion="' + (row.IdAsignacion != null ? row.IdAsignacion : "") + '"';
                                    dataFila += ' data-usuario="' + (row.IdUsuarioAsignado != null ? row.IdUsuarioAsignado : "") + '"';
                                    dataFila += ' data-IdentificacionCaja="' + (row.CajaAsignada.IdentificacionCaja != null ? row.CajaAsignada.IdentificacionCaja : "") + '"';
                                    dataFila += ' data-IdFuncionCaja="' + (row.CajaAsignada.IdFuncionCaja != null ? row.CajaAsignada.IdFuncionCaja : "") + '"';
                                    dataFila += ' data-FuncionCaja="' + (row.CajaAsignada.FuncionCaja != null ? row.CajaAsignada.FuncionCaja : "") + '"';
                                    dataFila += ' data-IdFuncionalidadCaja="' + (row.CajaAsignada.IdFuncionalidadCaja != null ? row.CajaAsignada.IdFuncionalidadCaja : "") + '"';
                                    dataFila += ' data-FuncionalidadCaja="' + (row.CajaAsignada.FuncionalidadCaja != null ? row.CajaAsignada.FuncionalidadCaja : "") + '"';
                                    dataFila += ' data-IdSeccionCaja="' + (row.CajaAsignada.IdSeccionCaja != null ? row.CajaAsignada.IdSeccionCaja : "") + '"';
                                    dataFila += ' data-SeccionCaja="' + (row.CajaAsignada.SeccionCaja != null ? row.CajaAsignada.SeccionCaja : "") + '"';
                                    dataFila += ' data-IdPisoCaja="' + (row.CajaAsignada.IdPisoCaja != null ? row.CajaAsignada.IdPisoCaja : "") + '"';
                                    dataFila += ' data-PisoCaja="' + (row.CajaAsignada.PisoCaja != null ? row.CajaAsignada.PisoCaja : "") + '"';
                                    dataFila += ' data-IpCaja="' + (row.CajaAsignada.IpCaja != null ? row.CajaAsignada.IpCaja : "") + '"';
                                    dataFila += ' data-HabilitaCaja="' + row.CajaAsignada.HabilitaCaja + '"';
                                    dataFila += ' data-IdSupervisorCaja="' + (row.CajaAsignada.IdSupervisorCaja != null ? row.CajaAsignada.IdSupervisorCaja : "") + '"';
                                    dataFila += ' data-MontoFijo="' + (row.MontoFijo != null ? row.MontoFijo : (row.CajaAsignada.MontoFijo != null ? row.CajaAsignada.MontoFijo : "")) + '"';

                                    resultado = '<center><a ' + dataFila + ' onclick="AsignarCajeroModal.EditarFormulario(this)" style="cursor: pointer" id="lnkAsignarUsuario" name="lnkAsignarUsuario"><span class="glyphicon glyphicon-user" style="color: green" title="Asignar usuario cajero"></span></a></center>';
                                }
                                return resultado;
                            }
                        }
                    ],
            });
    },
    ReloadCajas: function () {
        $('#listadoCajas').find("tbody").empty();
        $('#listadoCajas').DataTable().destroy();
        AdministraCajaSupervisor.LoadCajas();
    },

    BuscarCajasAsignadasHistorico: function () {
        var fechaInicio, fechaFinal;
        fechaInicio = $("#txtFechaInicio").val();
        fechaFinal = $("#txtFechaFinal").val();

        $.ajax({
            url: '/Caja/ListaCajasSupervisorAsignadas',
            type: 'GET',
            cache: false,
            data: { FechaInicio: fechaInicio, FechaFin: fechaFinal },
            contentType: 'application/json; charset=utf-8',
            success:
                function (result) {
                    if (!result.EsError) {
                        AdministraCajaSupervisor.LoadCajasAsignadasCargaGrilla(result.Data);
                    }
                    else {
                        Mensaje.Alerta(result.Mensaje);
                    }
                }
        });
    },
    LoadCajasAsignadasCargaGrilla: function (data) {
        $('#listaCajasAsignadasHistorico').DataTable().destroy();
        $('#listaCajasAsignadasHistorico').DataTable(
            {
                "language": dataTableEspanol,
                "bFilter": true,
                "bInfo": true,
                "bLengthChange": false,
                "bPaginate": true,
                "bProcessing": false,
                "bSort": true,

                "order": [[0, 'asc']],
                "aaData": data,
                "aoColumns":
                    [
                        { mDataProp: "CajaAsignada.IdentificacionCaja" },
                        { mDataProp: "CajaAsignada.FuncionCaja" },
                        { mDataProp: "CajaAsignada.SeccionCaja" },
                        { mDataProp: "CajaAsignada.PisoCaja" },
                        { mDataProp: "UsuarioAsignado" },
                        { sDefaultContent: "-", mDataProp: "FechaAsignacionTexto" },
                        {
                            sDefaultContent: "-",
                            mRender: function (data, type, full) {
                                var resultado = "";
                                if (full.Estado == 1) {
                                    resultado = full.FechaRechazoTexto;
                                }
                                else if (full.Estado == 2) {
                                    resultado = full.FechaInicioTexto;
                                }
                                else if (full.Estado == 3) {
                                    resultado = full.FechaCierreEfectivoTexto;
                                }
                                else if (full.Estado == 4) {
                                    resultado = full.FechaCierreCajaTexto;
                                }

                                return resultado;
                            }
                        },
                        {
                            sDefaultContent: "-",
                            mRender: function (data, type, full) {
                                var color = 'gray';
                                var texto = full.EstadoDescripcion;
                                if (full.Estado == 1) {
                                    color = 'Red';
                                    texto = '<a onclick="VerObservacion(' + "'" + full.ObservacionRechazo + "'" + ')" style="cursor: pointer" title="Motivo de rechazo">' + full.EstadoDescripcion + '</a>';
                                }
                                else if (full.Estado == 2) {
                                    color = 'Orange';
                                }
                                else if (full.Estado == 3) {
                                    color = 'Yellow';
                                }
                                else if (full.Estado == 4) {
                                    color = 'Green';
                                }

                                var resultado = '<span class="fa fa-circle" style="color: ' + color + '"></span>&nbsp;' + texto;
                                return resultado;
                            }
                        }
                    ],
            });
    },

    BuscarCajasAsignadasSupervisor: function () {
        //var fechaAsignacion = $("#txtFechaAsignada").val();
        $.ajax({
            url: '/Caja/ListaCajasSupervisor',
            type: 'GET',
            cache: false,
            contentType: 'application/json; charset=utf-8',
            success:
                function (data) {
                    AdministraCajaSupervisor.BuscarUsuariosCaja(data);
                }
        });
    },

    BuscarUsuariosCaja: function (data) {
        $.ajax({
            url: '/Caja/ListaUsuariosCaja',
            type: 'GET',
            cache: false,
            contentType: 'application/json; charset=utf-8',
            success:
                function (result) {
                    if (!result.EsError) {
                        AdministraCajaSupervisor.LoadCajasAsignadasSupervisor(data, result.Data);
                    }
                    else {
                        Mensaje.Alerta(result.Mensaje);
                    }
                }
        });
    },
    LoadCajasAsignadasSupervisor: function (data, users) {

        // carga de usuarios de caja disponibles 
        var usuarios = "";
        users.forEach(function (user, index) {
            usuarios += '<option value="' + user.Login + '"> ' + user.Nombre + '</option>';
        });

        $('#listadoCajasSupervisor').DataTable().destroy();
        $('#listadoCajasSupervisor').DataTable(
            {
                "language": dataTableEspanol,
                "bFilter": false,
                "bInfo": true,
                "bLengthChange": false,
                "bPaginate": false,
                "bProcessing": false,
                "bSort": true,

                "order": [[2, 'asc']],

                "aaData": data.data,
                "aoColumns":
                    [
                        {
                            sDefaultContent: null,
                            bSortable: false,
                            mRender: function (data, type, row) { return '<input type="checkbox" onclick="AdministraCajaSupervisor.SeleccionarAsignacion(this)" id="chkSelecionarCaja" name="SeleccionarAsignacion" data-IdCaja="' + row.CajaAsignada.IdCaja + '" />'; },
                        },
                        { mDataProp: "CajaAsignada.IdentificacionCaja", bSortable: true },
                        { mDataProp: "CajaAsignada.FuncionCaja", bSortable: true },
                        { mDataProp: "CajaAsignada.SeccionCaja", bSortable: true },
                        {
                            sDefaultContent: null,
                            bSortable: false,
                            mRender: function (data, type, row) {
                                return '<div class="col-sm-18"><select id="usuarioAsignado_' + row.CajaAsignada.IdCaja + '" class="form-control">' +
                                    '<option value="">Seleccione Usuario...</option>' + usuarios + '</select></div> ';
                            },
                        },
                        {
                            sDefaultContent: null,
                            bSortable: false,
                            mRender: function (data, type, row) { return '<div class="input-group"><input type="text" class="form-control datetime" style="max-width:none" id="txtFechaIni_' + row.CajaAsignada.IdCaja + '" value="" readonly /><button class="input-group-addon" id="btnFechaIni_' + row.CajaAsignada.IdCaja + '"><i class="glyphicon glyphicon-calendar"></i></button></div>'; },
                        },
                        {
                            sDefaultContent: null,
                            bSortable: false,
                            mRender: function (data, type, row) { return '<div class="input-group"><input type="text" class="form-control datetime" style="max-width:none" id="txtFechaFin_' + row.CajaAsignada.IdCaja + '" value="" readonly /><button class="input-group-addon" id="btnFechaFin_' + row.CajaAsignada.IdCaja + '"><i class="glyphicon glyphicon-calendar"></i></button></div>'; },
                        },
                    ],
                "fnCreatedRow": function (nRow, aData, iDataIndex) {
                    nRow.dataset.idcaja = aData.CajaAsignada.IdCaja;
                    nRow.className = 'filaCajaAsignada';
                },
                "fnDrawCallback": function (oSettings) {
                    var data = oSettings.aoData;
                    data.forEach(function (aData, index) {
                        var idCaja = aData._aData.CajaAsignada.IdCaja;
                        $('#txtFechaIni_' + idCaja).datetimepicker({
                            format: "DD/MM/YYYY",
                            ignoreReadonly: true,
                            showTodayButton: true,
                            allowInputToggle: true,
                            locale: "es"
                        });
                        $('#txtFechaFin_' + idCaja).datetimepicker({
                            format: "DD/MM/YYYY",
                            ignoreReadonly: true,
                            showTodayButton: true,
                            allowInputToggle: true,
                            locale: "es"
                        });
                        $('#btnFechaIni_' + idCaja).click(function () {
                            $('#txtFechaIni_' + idCaja).focus();
                        });
                        $('#btnFechaFin_' + idCaja).click(function () {
                            $('#txtFechaFin_' + idCaja).focus();
                        });
                    });
                }
            });
        //$('#listadoCajasSupervisor').DataTable().draw();
        //data.data.forEach(function (caja, index) {
        //    var idCaja = caja.CajaAsignada.IdCaja;
        //    $('#txtFechaIni_' + idCaja).datetimepicker({
        //        format: "DD/MM/YYYY",
        //        ignoreReadonly: true,
        //        showTodayButton: true,
        //        allowInputToggle: true,
        //        locale: "es"
        //    });
        //    $('#txtFechaFin_' + idCaja).datetimepicker({
        //        format: "DD/MM/YYYY",
        //        ignoreReadonly: true,
        //        showTodayButton: true,
        //        allowInputToggle: true,
        //        locale: "es"
        //    });
        //    $('#btnFechaIni_' + idCaja).click(function () {
        //        $('#txtFechaIni_' + idCaja).focus();
        //    });
        //    $('#btnFechaFin_' + idCaja).click(function () {
        //        $('#txtFechaFin_' + idCaja).focus();
        //    });
        //});

    },
    SeleccionarAsignacion: function (item) {
        var idCaja = item.dataset.idcaja;

        var cajas = $("#cajasSeleccionadas").val();
        if (cajas.includes(idCaja)) { // existe id lo eliminamos
            cajas = cajas.replace(idCaja + ";", "");
            cajas = cajas.replace(";" + idCaja, "");
            cajas = cajas.replace(idCaja, "");
            $('#txtFechaFin_' + idCaja).val("");
            $('#txtFechaIni_' + idCaja).val("");
        }
        else {// No existe id, lo agregamos
            if (cajas.length > 0) {
                cajas += ";" + idCaja;
            }
            else {
                cajas = idCaja;
            }
        }

        $(".filaCajaAsignada").each(function (index, item) {
            if (item.dataset.idcaja == idCaja) {
                if ($(item).hasClass('selected')) {
                    $(item).removeClass('selected');
                }
                else {
                    $(item).addClass('selected');
                }
                return false;
            }
        });

        if (cajas.length > 0) {
            $("#btnReasignarMasivo").removeAttr('disabled');
        }
        else {
            $("#btnReasignarMasivo").attr('disabled', 'disabled');
        }

        $("#cajasSeleccionadas").val(cajas);
    },
    GuardarCajasAsignadasFormulario: function () {
        if ($("#cajasSeleccionadas").val().length == 0) {
            Mensaje.Alerta('No se ha seleccionado ninguna caja para asignar.');
        }
        else {
            Mensaje.Confirmar("¿Está seguro de reasignar las cajas seleccionadas?", AdministraCajaSupervisor.GuardarMultiplesAsignadas);
        }
    },
    GuardarMultiplesAsignadas: function () {
        var listasignacion = $("#cajasSeleccionadas").val().split(";");
        var asignaciones = [];
        var error = false;
        var mensajeError = "";

        listasignacion.forEach(function (idCaja, index) {
            var asignacion = {
                IdCaja: "",
                FechaInicioCaja: "",
                FechaCierreCaja: "",
                IdUsuarioAsignado: ""
            };
            var ini = $('#txtFechaIni_' + idCaja).val().split("/");
            var fin = $('#txtFechaFin_' + idCaja).val().split("/");
            var user = $('#usuarioAsignado_' + idCaja).val();
            var fechaIni = new Date(ini[2], ini[1] - 1, ini[0]);
            var fechaFin = new Date(fin[2], fin[1] - 1, fin[0]);

            if (user == "") {
                error = true;
                mensajeError = "Debe seleccionar un usuario";
            }
            if (fechaIni.getTime() > fechaFin.getTime()) {
                error = true;
                mensajeError = "La fecha de inicio de la asginación no puede ser mayor a la fecha final";
            }

            if (isNaN(fechaFin.getTime())) {
                error = true;
                mensajeError = "Debe especificar una fecha final de asignación";
            }
            if (isNaN(fechaIni.getTime())) {
                error = true;
                mensajeError = "Debe especificar una fecha inicial de asignación";
            }

            asignacion.IdCaja = idCaja;
            asignacion.FechaInicioCaja = fechaIni;
            asignacion.FechaCierreCaja = fechaFin;
            asignacion.IdUsuarioAsignado = user;
            asignaciones.push(asignacion);

        });

        if (error) {
            Mensaje.Alerta(mensajeError);
        } else {
            var postData = {
                "ListasAsignaciones": asignaciones
            };
            $.ajax({
                url: '/Caja/ReAsignarCajasAsignadas',
                type: 'POST',
                dataType: 'json',
                cache: false,
                data: JSON.stringify(postData),
                contentType: 'application/json; charset=utf-8',
                success: function (data) {
                    var mensaje = data.Mensaje;
                    if (data.EsError) {
                        Mensaje.Alerta(mensaje);
                    }
                    else {
                        $("#cajasSeleccionadas").val("");
                        Mensaje.CorrectoF(mensaje, AdministraCajaSupervisor.ReloadCajas);
                        AdministraCajaSupervisor.BuscarCajasAsignadasSupervisor();
                    }
                }
            });
        }
    }
}