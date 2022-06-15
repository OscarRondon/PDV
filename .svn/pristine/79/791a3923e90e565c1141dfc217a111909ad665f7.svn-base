$(document).ready(function () {
    LoadCajas();
});

function ReloadCajas() {
    $('#listadoCajas').find("tbody").empty();
    $('#listadoCajas').DataTable().destroy();
    LoadCajas();
}

function LoadCajas() {
    $('#listadoCajas').DataTable(
        {
            "language": dataTableEspanol,
            "bFilter": true,
            "bInfo": true,
            "bLengthChange": false,
            "bPaginate": true,
            "bProcessing": false,
            "bSort": true,

            "order": [[1, 'asc']],

            "sAjaxSource": '/Caja/ListaCajasCierre',
            "aoColumns":
            [
                { mDataProp: "CajaAsignada.IdentificacionCaja", bSortable: true },
                { mDataProp: "UsuarioAsignado", bSortable: true },
                { mDataProp: "FechaCreacionAsignacionTexto", bSortable: true },
                { mDataProp: "FechaInicioAsignacionTexto", bSortable: true },
                { mDataProp: "FechaCierreAsignacionTexto", bSortable: true },
                {
                    sDefaultContent: "",
                    bSortable: false,
                    mRender: function (data, type, row) {
                        var dataFila = 'data-IdAsignacion ="' + row.IdAsignacion + '"';
                        dataFila += 'data-CajaAsignada ="' + (row.CajaAsignada.IdentificacionCaja != null ? row.CajaAsignada.IdentificacionCaja : "Sin Datos") + '"';
                        dataFila += 'data-UsuarioAsignado ="' + (row.UsuarioAsignado != null ? row.UsuarioAsignado : "Sin Datos") + '"';
                        dataFila += 'data-FechaInicioAsignacionTexto ="' + (row.FechaInicioAsignacionTexto != null ? row.FechaInicioAsignacionTexto : "Sin Datos") + '"';
                        dataFila += 'data-FechaCierreAsignacionTexto ="' + (row.FechaCierreAsignacionTexto != null ? row.FechaCierreAsignacionTexto : "Sin Datos") + '"';
                        dataFila += 'data-MontoFijoAsignacion ="' + (row.MontoFijo != null ? row.MontoFijo : 0) + '"';
                        dataFila += 'data-MontoCierre ="' + (row.MontoCierre != null ? row.MontoCierre : 0) + '"';
                        dataFila += 'data-MontoDiferencia ="' + (row.MontoDiferencia != null ? row.MontoDiferencia : 0) + '"';
                        dataFila += 'data-ObservacionDiferencia ="' + (row.ObservacionDiferencia != null ? row.ObservacionDiferencia : "Sin Observación.") + '"';
                        //var botonListado = '<a ' + dataFila + ' onclick="TransaccionesCajaModal(this)" style="cursor: pointer"><span class="glyphicon glyphicon-list-alt" title="Ver Transacciones"></span></a>&nbsp;&nbsp;';
                        var botonCerrar = '<a ' + dataFila + ' onclick="CierreCajaModal.AbrirFormulario(this)" style="cursor: pointer"><span class="glyphicon glyphicon-off" title="Cerrar Caja"></span></a>&nbsp;&nbsp;';
                        //var resultado = '<center>' + botonListado + ' ' + botonCerrar + '</center>';
                        var resultado = '<center>' + botonCerrar + '</center>';
                        return resultado;
                    }
                },
            ],
            "fnCreatedRow": function (nRow, aData, iDataIndex) {
                nRow.dataset.asignacion = aData.IdAsignacion;
                nRow.className = 'filaAsignacion';
            }
        });
}

function SeleccionAsignacion(item) {
    var idAsignacion = item.dataset.idAsignacion;
    var asignacion = $("#asignacionSeleccionada").val();
    if (asignacion.includes(idAsignacion)) { // existe id lo eliminamos
        asignacion = asignacion.replace(idAsignacion + ";", "");
        asignacion = asignacion.replace(";" + idAsignacion, "");
        asignacion = asignacion.replace(idAsignacion, "");
    }
    else {// No existe id, lo agregamos
        if (asignacion.length > 0) {
            asignacion += ";" + idAsignacion;
        }
        else {
            asignacion = idAsignacion;
        }
    }

    $(".filaAsignacion").each(function (index, item) {
        if (item.dataset.asignacion == idAsignacion) {
            if ($(item).hasClass('selected')) {
                $(item).removeClass('selected');
            }
            else {
                $(item).addClass('selected');
            }
            return false;
        }
    });

    $("#asignacionSeleccionada").val(asignacion);
}