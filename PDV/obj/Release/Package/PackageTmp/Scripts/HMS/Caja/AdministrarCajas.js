$(document).ready(function () {
    mLoading.modal();
    LoadCajas();
});
function ReloadCajas() {
    $('#listadoCajas').find("tbody").empty();
    $('#listadoCajas').DataTable().destroy();

    $("#AsignarMasivamente").attr('disabled', 'disabled');

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

            "sAjaxSource": '/Caja/ListaCajasAdministrador',
            "aoColumns":
            [

                {
                    sDefaultContent: null,
                    bSortable: false,
                    mRender: function (data, type, row) { return '<input type="checkbox" id="chkSelecionarCaja" name="SeleccionarCaja" data-IdCaja="' + row.IdCaja + '" onchange="SeleccionCajas(this)"  />'; },
                },
                { mDataProp: "IdentificacionCaja", bSortable: true },
                { mDataProp: "FuncionalidadCaja", bSortable: true },
                { mDataProp: "FuncionCaja", bSortable: true },
                { mDataProp: "SeccionCaja", bSortable: true },
                { mDataProp: "PisoCaja", bSortable: true },
                {
                    sDefaultContent: "",
                    bSortable: true,
                    mRender: function (data, type, row) { return row.HabilitaCaja ? "SÍ" : "NO"; },
                },
                { sDefaultContent: "-", mDataProp: "SupervisorCaja", bSortable: true },
                {
                    sDefaultContent: "",
                    bSortable: false,
                    mRender: function (data, type, row) {
                        var dataFila = 'data-IdCaja="' + row.IdCaja + '" data-IdentificacionCaja="' + (row.IdentificacionCaja != null ? row.IdentificacionCaja : "") + '"';
                        dataFila += ' data-IdFuncionCaja="' + (row.IdFuncionCaja != null ? row.IdFuncionCaja : "") + '"';
                        dataFila += ' data-FuncionCaja="' + (row.FuncionCaja != null ? row.FuncionCaja : "") + '"';
                        dataFila += ' data-IdFuncionalidadCaja="' + (row.IdFuncionalidadCaja != null ? row.IdFuncionalidadCaja : "") + '"';
                        dataFila += ' data-FuncionalidadCaja="' + (row.FuncionalidadCaja != null ? row.FuncionalidadCaja : "") + '"';
                        dataFila += ' data-IdSeccionCaja="' + (row.IdSeccionCaja != null ? row.IdSeccionCaja : "") + '"';
                        dataFila += ' data-SeccionCaja="' + (row.SeccionCaja != null ? row.SeccionCaja : "") + '"';
                        dataFila += ' data-IdPisoCaja="' + (row.IdPisoCaja != null ? row.IdPisoCaja : "") + '"';
                        dataFila += ' data-PisoCaja="' + (row.PisoCaja != null ? row.PisoCaja : "") + '"';
                        dataFila += ' data-IpCaja="' + (row.IpCaja != null ? row.IpCaja : "") + '"';
                        dataFila += ' data-HabilitaCaja="' + row.HabilitaCaja + '"';
                        dataFila += ' data-IdSupervisorCaja="' + (row.IdSupervisorCaja != null ? row.IdSupervisorCaja : "") + '"';
                        dataFila += ' data-MontoFijo="' + (row.MontoFijo != null ? row.MontoFijo : "") + '"';

                        var botonEditar = '<a ' + dataFila + ' onclick="CreaCajaModal.EditarFormulario(this)" style="cursor: pointer" id="lnkEditar" name="lnkEditar"><span class="glyphicon glyphicon-pencil" title="Editar"></span></a>&nbsp;&nbsp;';
                        var resultado = '<center>' + botonEditar + '</center>';
                        return resultado;
                    }
                },
            ],
            "fnCreatedRow": function (nRow, aData, iDataIndex) {
                nRow.dataset.caja = aData.IdCaja;
                nRow.className = 'filaCajaAsignar';
            }
        });
}

function SeleccionCajas(item) {
    var idCaja = item.dataset.idcaja;
    var cajas = $("#cajasSeleccionadas").val();
    if (cajas.includes(idCaja)) { // existe id lo eliminamos
        cajas = cajas.replace(idCaja + ";", "");
        cajas = cajas.replace(";" + idCaja, "");
        cajas = cajas.replace(idCaja, "");
    }
    else {// No existe id, lo agregamos
        if (cajas.length > 0) {
            cajas += ";" + idCaja;
        }
        else {
            cajas = idCaja;
        }
    }

    $(".filaCajaAsignar").each(function (index, item) {
        if (item.dataset.caja == idCaja) {
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
        $("#AsignarMasivamente").removeAttr('disabled');
    }
    else {
        $("#AsignarMasivamente").attr('disabled', 'disabled');
    }

    $("#cajasSeleccionadas").val(cajas);
}