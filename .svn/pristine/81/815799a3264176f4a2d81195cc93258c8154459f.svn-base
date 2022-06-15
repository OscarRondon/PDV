$(document).ready(function () {

    IngresoGarantiaModal.Init();
    $('#FichaClinica').mask('999?99999');
    $('#GarEpisodio').mask('a999?9999999');;
    $('#divCheque').hide();
    $("#GarRutResponsable").Rut({
        on_error: function () {
            Mensaje.Alerta("Rut ingresado es inválido. Intente nuevamente.");
            $("#GarRutResponsable").val("");
        },
        on_empty: function () {
            $("#GarRutResponsable").val("");
        }
    });
    $("#GarRutPaciente").Rut({
        on_error: function () {
            Mensaje.Alerta("Rut ingresado es inválido. Intente nuevamente.");
            $("#GarRutPaciente").val("");
        },
        on_empty: function () {
            $("#GarRutPaciente").val("");
        }
    });



    //Manejo Campos Formulario
    //Campos Respaldo
    $('#Monto').blur(function () {
        this.value = (this.value + '').replace(/[^0-9]/g, '');
        inputMonto = $('#Monto').val();
        var resultado = "";
        $('#Monto').val(resultado.concat('$ ', Moneda(inputMonto)));
    });
    //Campos Responsable
    $('#GarRutResponsable').blur(function () {
        inputRutResp = $('#GarRutResponsable').val();
        if (inputRutResp !== "") {
            $('#GarRutResponsable').val(inputRutResp.toUpperCase());
        }
    });
    $('#PasResponsable').blur(function () {
        inputPasResp = $('#PasResponsable').val();
        if (inputPasResp !== "") {
            $('#PasResponsable').val(inputPasResp.toUpperCase());
        }
    });
    $('#NombreResponsable').blur(function () {
        inputNomResp = $('#NombreResponsable').val();
        if (inputNomResp !== "") {
            $('#NombreResponsable').val(inputNomResp.toUpperCase());
        }
    });
    $('#DireccionResponsable').blur(function () {
        inputDirResp = $('#DireccionResponsable').val();
        if (inputDirResp !== "") {
            $('#DireccionResponsable').val(inputDirResp.toUpperCase());
        }
    });
    $('#ListadoComunasResponsable').change(function () {
        inputComResp = $('#ListadoComunasResponsable').val();
        if (inputComResp !== "") {
            $('#ListadoComunasResponsable').val(inputComResp.toUpperCase());
        }
    });
    //Campos Paciente
    $('#FichaClinica').blur(function () {
        inputFicCli = $('#FichaClinica').val();
        if (inputFicCli !== "") {
            $('#FichaClinica').val(inputFicCli.toUpperCase());
        }
    });
    $('#GarEpisodio').blur(function () {
        inputEpisodio = $('#GarEpisodio').val();
        if (inputEpisodio !== "") {
            $('#GarEpisodio').val(inputEpisodio.toUpperCase());
        }
    });
    $('#GarRutPaciente').blur(function () {
        inputRutPac = $('#GarRutPaciente').val();
        if (inputRutPac !== "") {
            $('#GarRutPaciente').val(inputRutPac.toUpperCase());
        }
    });
    $('#PasPaciente').blur(function () {
        inputPasPac = $('#PasPaciente').val();
        if (inputPasPac !== "") {
            $('#PasPaciente').val(inputPasPac.toUpperCase());
        }
    });
    $('#NombrePaciente').blur(function () {
        inputNomPac = $('#NombrePaciente').val();
        if (inputNomPac !== "") {
            $('#NombrePaciente').val(inputNomPac.toUpperCase());
        }
    });
    $('#DireccionPaciente').blur(function () {
        inputDirPac = $('#DireccionPaciente').val();
        if (inputDirPac !== "") {
            $('#DireccionPaciente').val(inputDirPac.toUpperCase());
        }
    });
    $('#ListadoComunasPaciente').change(function () {
        inputComPac = $('#ListadoComunasPaciente').val();
        if (inputComPac !== "") {
            $('#ListadoComunasPaciente').val(inputComPac.toUpperCase());
        }
    });
});

var IngresoGarantiaModal = {
    Init: function () {
        $('#modalIngresoGarantia').on('hidden.bs.modal', function (e) {
            $('#inputDoc').val("0");
            IngresoGarantiaModal.LimpiarFormulario();
        });
    },
    GuardarFormulario: function () {
        if (IngresoGarantiaModal.ValidarFormulario()) {
            Mensaje.Confirmar("¿Confirma el envío de la información?", IngresoGarantiaModal.IngresarGarantia);
        }
    },
    //Guardar Garantía
    IngresarGarantia: function () {

        var txtMontoFinal = $('#Monto').val();
        var MontoFinal = txtMontoFinal.replace("$", "");
        var Monto = MontoFinal.replace(".", "");
        var Entidad = $('#ListaBancos option:selected').html();
        if (Entidad == "Seleccione Banco..." || Entidad == "") {
            Entidad = "";
        }
        else {
            Entidad = Entidad;
        };
        Entidad = $('#ListaBancos').val();
        var jsonAntGar = {
            GarIdAsignacion: $('#idAsignacion').val(),
            IdTipoDocumento: $('#inputDoc').val(),
            GarMonto: parseInt(Monto.trim()),
            GarMontoTexto: Monto,
            GarIndicador: $('#respaldoDoc').val(),
            GarIndicadorTexto: $('#respaldoDoc option:selected').text(),
            GarEntidad: Entidad,
            GarNumeroCheque: $('#GarNumeroCheque').val(),

            GarRutResponsable: $('#GarRutResponsable').val(),
            PasResponsable: $('#PasResponsable').val(),
            NombreResponsable: $('#NombreResponsable').val(),
            DireccionResponsable: $('#DireccionResponsable').val(),
            ComunaResponsable: $('#ListadoComunasResponsable').val(),

            GarFichaClinica: $('#FichaClinica').val(),
            GarEpisodio: $('#GarEpisodio').val(),
            GarRutPaciente: $('#GarRutPaciente').val(),
            PasaportePaciente: $('#PasPaciente').val(),
            NombrePaciente: $('#NombrePaciente').val(),
            DireccionPaciente: $('#DireccionPaciente').val(),
            ComunaPaciente: $('#ListadoComunasPaciente').val(),
            IdTrack: $('#ListadoBoletasAbiertas').val()
        };
        var postData = { antGar: jsonAntGar };
        $.ajax({
            url: '/Garantia/IngresoGarantia',
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
                    var docentry = data.DocEntry;
                    $.ajax({
                        url: '/Garantia/Imprimir',
                        type: 'GET',
                        cache: false,
                        data: { DocEntry: docentry },
                        contentType: 'application/json; charset=utf-8',
                        success: function (data) {
                            if (data.resultGarantia.EsError) {
                                Mensaje.Alerta(data.resultGarantia.Mensaje);
                            } else {
                                GenerarPDFBase64(data.resultGarantia.Data, "Garantia.pdf");
                                $('#modalIngresoGarantia').modal("hide");
                                Mensaje.CorrectoF(mensaje, GarantiaListado.BuscarListaGarantias);
                            }
                        }
                    });



                }
            }
        });
    },
    ValidarFormulario: function () {
        var textoInicio = "<dl><dt>Debe revisar los siguientes datos:</dt>";
        var textoFinal = "</dl>";
        var esError = false;



        if ($('#inputDoc').val() === "" || $('#inputDoc').val() === "Seleccione Tipo Documento..." || $('#inputDoc').val() === "0") {
            textoInicio = textoInicio + "<dd> &#8226;  Tipo de documento.</dd>";
            esError = true;
        }
        else {
            if ($('#inputDoc').val() == "G") {
                if ($('#ListadoBoletasAbiertas').val() === "" || $('#ListadoBoletasAbiertas').val() === "Seleccione Tipo Documento..." || $('#ListadoBoletasAbiertas').val() === "0") {
                    textoInicio = textoInicio + "<dd> &#8226;  Número.</dd>";
                    esError = true;
                }
            }
            //if ($('#inputDoc').val() === "A") {
            if ($('#Monto').val() == "" || $('#Monto').val().trim() == "") {
                textoInicio = textoInicio + '<dd> &#8226;  Monto a cancelar.</dd>';
                esError = true;
            }
            var seleccionRespaldo = $('#respaldoDoc').val();
            if (seleccionRespaldo != "") {
                if (seleccionRespaldo == "1") {
                    if ($('#ListaBancos').val() == "" || $('#ListaBancos').val().trim() == "") {
                        textoInicio = textoInicio + '<dd> &#8226;  Banco emisor.</dd>';
                        esError = true;
                    }

                    if ($('#GarNumeroCheque').val() == "" || $('#GarNumeroCheque').val().trim() == "") {
                        textoInicio = textoInicio + '<dd> &#8226;  Número de cheque.</dd>';
                        esError = true;
                    }
                }
            }
            else {
                textoInicio = textoInicio + '<dd> &#8226;  Medio de Pago.</dd>';
                esError = true;
            }
            //}

            if ($('#GarRutResponsable').val() == "" || $('#GarRutResponsable').val().trim() == "") {
                textoInicio = textoInicio + '<dd> &#8226;  RUT del responsable.</dd>';
                esError = true;
            }

            if ($('#NombreResponsable').val() == "" || $('#NombreResponsable').val().trim() == "") {
                textoInicio = textoInicio + '<dd> &#8226;  Nombre del responsable.</dd>';
                esError = true;
            }

            if ($('#DireccionResponsable').val() == "" || $('#DireccionResponsable').val().trim() == "") {
                textoInicio = textoInicio + '<dd> &#8226;  Direccion del responsable.</dd>';
                esError = true;
            }

            if ($('#ListadoComunasResponsable').val() == "" || $('#ListadoComunasResponsable').val().trim() == "") {
                textoInicio = textoInicio + '<dd> &#8226;  Comuna del responsable.</dd>';
                esError = true;
            }

            if ($('#FichaClinica').val() == "" || $('#FichaClinica').val().trim() == "") {
                textoInicio = textoInicio + '<dd> &#8226;  Ficha Clínica.</dd>';
                esError = true;
            }
            if ($('#GarEpisodio').val() == "" || $('#GarEpisodio').val().trim() == "") {
                textoInicio = textoInicio + '<dd> &#8226;  Episodio.</dd>';
                esError = true;
            }
            if ($('#GarRutPaciente').val() == "" || $('#GarRutPaciente').val().trim() == "") {
                textoInicio = textoInicio + '<dd> &#8226;  RUT del Paciente.</dd>';
                esError = true;
            }

            if ($('#NombrePaciente').val() == "" || $('#NombrePaciente').val().trim() == "") {
                textoInicio = textoInicio + '<dd> &#8226;  Nombre del Paciente.</dd>';
                esError = true;
            }

            if ($('#DireccionPaciente').val() == "" || $('#DireccionPaciente').val().trim() == "") {
                textoInicio = textoInicio + '<dd> &#8226;  Direccion del Paciente.</dd>';
                esError = true;
            }

            if ($('#ListadoComunasPaciente').val() == "" || $('#ListadoComunasPaciente').val().trim() == "") {
                textoInicio = textoInicio + '<dd> &#8226;  Comuna del Paciente.</dd>';
                esError = true;
            }
        }
        if (esError == true) {
            bootbox.alert(textoInicio + textoFinal);
        } else {
            return true;
        }
    },
    LimpiarFormulario: function () {
        $("#tituloModalIngresoGarantia").val("");

        $('#ListadoBoletasAbiertas').val("");
        $('#Monto').val("");
        $('#respaldoDoc').val("");
        $('#respaldoDoc')[0].onchange();
        $('#ListaBancos option:selected').val("");
        $('#GarNumeroCheque').val("");

        $('#GarRutResponsable').val("");
        $('#PasResponsable').val("");
        $('#NombreResponsable').val("");
        $('#DireccionResponsable').val("");
        $('#ListadoComunasResponsable option:selected').html("");

        $('#FichaClinica').val("");
        $('#GarEpisodio').val("");
        $('#GarRutPaciente').val("");
        $('#PasPaciente').val("");
        $('#NombrePaciente').val("");
        $('#DireccionPaciente').val("");
        $('#ListadoComunasPaciente option:selected').html("");
        $('#ListadoBoletasAbiertas option:selected').html("");
    },
    AbrirFormulario: function () {
        IngresoGarantiaModal.LimpiarFormulario();
        IngresoGarantiaModal.CargarListaBoletasActivas('dataBoletasAbiertas', 'ListadoBoletasAbiertas');
        $('#modalIngresoGarantia').modal("show");
        IngresoGarantiaModal.CargarListaBancos();
        IngresoGarantiaModal.CargarListaComuna('datalistResponsable', 'ListadoComunasResponsable');
        IngresoGarantiaModal.CargarListaComuna('datalistPaciente', 'ListadoComunasPaciente');
    },
    CargarListaBancos: function () {
        $('#ListaBancos option').remove();
        $.ajax({
            url: '/Garantia/TraerDatosBanco',
            type: 'GET',
            cache: false,
            success: function (data) {
                if (data != null) {
                    $.each(data.data, function () {
                        $('#ListaBancos').append($("<option></option>").val(this['Value']).html(this['Text']));
                    });
                    $("#ListaBancos").prepend("<option value=''>Seleccione Banco...</option>");
                    $("#ListaBancos option:first").attr("selected", "selected");
                }
            }
        });
    },
    CargarListaComuna: function (listaDatos, selector) {
        var datalist = document.getElementById(listaDatos);
        var input = document.getElementById(selector);
        var request = new XMLHttpRequest();
        request.onreadystatechange = function (response) {
            if (request.readyState === 4) {
                if (request.status === 200) {
                    var jsonOptions = JSON.parse(request.responseText);

                    $.each(jsonOptions.data, function (indice, item) {
                        var option = document.createElement('option');
                        option.value = item.Value;
                        option.text = item.Text;
                        datalist.appendChild(option);
                    });
                    input.placeholder = "Indique comuna...";
                } else {
                    input.placeholder = "No se ha podido cargar la lista de comunas...";
                }
            }
        };
        input.placeholder = "Cargando Opciones...";
        request.open('GET', '/Garantia/TraerDatosComunas', true);
        request.send();
    },
    CargarListaBoletasActivas: function (listaDatos, selector) {
        var datalist = document.getElementById(listaDatos);
        var input = document.getElementById(selector);
        var request = new XMLHttpRequest();
        request.onreadystatechange = function (response) {
            if (request.readyState === 4) {
                if (request.status === 200) {
                    var jsonOptions = JSON.parse(request.responseText);

                    $.each(jsonOptions.data, function (indice, item) {
                        var option = document.createElement('option');
                        option.value = item.Text;
                        datalist.appendChild(option);
                    });
                    input.placeholder = "Indique Boleta...";
                } else {
                    input.placeholder = "No se ha podido cargar la lista de boletas...";
                }
            }
        };
        input.placeholder = "Cargando Opciones...";
        request.open('GET', '/Garantia/ListaBoletasActivas', true);
        request.send();
    },
    HabilitarControlesDocumento: function (tipoDocumento) {
        IngresoGarantiaModal.LimpiarFormulario();
        if (tipoDocumento == "A") {
            $("#asteriscoNumero")[0].style.display = "none";
            $("#Monto")[0].readOnly = false;
            $("#FichaClinica")[0].readOnly = false;
            $("#GarEpisodio")[0].readOnly = false;
            $("#GarRutPaciente")[0].readOnly = false;
            $("#NombrePaciente")[0].readOnly = false;
        }
        else if (tipoDocumento == "G") {
            $("#asteriscoNumero")[0].style.display = "inherit";
            $("#Monto")[0].readOnly = true;
            $("#FichaClinica")[0].readOnly = true;
            $("#GarEpisodio")[0].readOnly = true;
            $("#GarRutPaciente")[0].readOnly = true;
            $("#NombrePaciente")[0].readOnly = true;
        }
    },

    mostraDivCheque: function (tipoRespaldo) {
        if (tipoRespaldo == '1') {
            $('#divCheque').show();
            IngresoGarantiaModal.CargarListaBancos();
        } else {
            $('#divCheque').hide();
        }
    },
    BuscarInfoBoleta: function () {
        var doc = $("#ListadoBoletasAbiertas").val();
        var vacio = "";
        var fafsa = $('#ListadoBoletasAbiertas option:selected').html();
        $.ajax({
            url: '/Garantia/TraerDatosBoleta',
            type: 'GET',
            data: { idTrans: vacio, idTrack: doc },
            cache: false,
            success: function (data) {
                if (data.data != null) {
                    var resultado = "";
                    resultado = resultado.concat('$ ', data.data.MontoPago);
                    $('#Monto').val(resultado);

                    $('#GarRutResponsable').val(data.data.RutResponsable);
                    $('#PasResponsable').val("");
                    $('#NombreResponsable').val(data.data.NombresResponsable);
                    $('#DireccionResponsable').val(data.data.DireccionResponsable);
                    $('#ListadoComunasResponsable option:selected').html(data.data.ComunaResponsable);

                    $('#FichaClinica').val(data.data.FichaClinica);
                    $('#GarEpisodio').val(data.data.Episodio);
                    $('#GarRutPaciente').val(data.data.RutPaciente);
                    $('#PasPaciente').val("");
                    $('#NombrePaciente').val(data.data.NombresPaciente);
                    $('#ListadoComunasPaciente option:selected').html(data.data.ComunaPaciente);
                }
            }
        });
    },
}