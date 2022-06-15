var Mensaje = {
    Alerta: function (mensaje) {
        bootbox.alert({ message: '<span class="glyphicon glyphicon-info-sign" style="color: blue"></span>&nbsp;&nbsp;' + mensaje });
    },
    AlertaF: function (mensaje, funcion) {
        bootbox.alert({
            message: '<span class="glyphicon glyphicon-info-sign" style="color: blue"></span>&nbsp;&nbsp;' + mensaje,
            callback: function () {
                if (funcion != null) {
                    funcion();
                }
            }
        });
    },
    Error: function (mensaje) {
        bootbox.alert({ message: '<span class="glyphicon glyphicon-remove-sign" style="color: red"></span>&nbsp;&nbsp;' + mensaje });
    },
    Correcto: function (mensaje) {
        bootbox.alert({ message: '<span class="glyphicon glyphicon-ok-sign" style="color: green"></span>&nbsp;&nbsp;' + mensaje });
    },
    CorrectoF: function (mensaje, funcion) {
        bootbox.alert({
            message: '<span class="glyphicon glyphicon-ok-sign" style="color: green"></span>&nbsp;&nbsp;' + mensaje,
            callback: function () {
                if (funcion != null) {
                    funcion();
                }
            }
        });
    },
    Confirmar: function (mensaje, funcion) {
        bootbox.confirm({
            message: '<span class="glyphicon glyphicon-question-sign" style="color: blue"></span>&nbsp;&nbsp;' + mensaje,
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
                    funcion();
                }
            }
        });
    },
}