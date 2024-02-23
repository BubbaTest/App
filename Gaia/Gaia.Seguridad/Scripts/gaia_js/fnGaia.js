$(document).ready(main);
function main() {
    $.fn.select2.defaults.set('language', 'es');

    /*
     * Función para mostrar mensaje de espera propio 
     */
    if (!alertify.alertwaiting) {
        //define a new dialog
        alertify.dialog('alertwaiting', function () {
            return {
                main: function (message) {
                    this.message = message;
                },
                setup: function () {
                    return {
                        options: { closable: 'false' }
                    };
                },
                prepare: function () {
                    this.setContent(this.message);
                }
            }
        });
    }
};

/**
*
*  Base64 encode / decode
*  http://www.webtoolkit.info/
*
**/

var Base64 = {

    // private property
    _keyStr: "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/=",

    // public method for encoding
    encode: function (input) {
        var output = "";
        var chr1, chr2, chr3, enc1, enc2, enc3, enc4;
        var i = 0;

        input = Base64._utf8_encode(input);

        while (i < input.length) {

            chr1 = input.charCodeAt(i++);
            chr2 = input.charCodeAt(i++);
            chr3 = input.charCodeAt(i++);

            enc1 = chr1 >> 2;
            enc2 = ((chr1 & 3) << 4) | (chr2 >> 4);
            enc3 = ((chr2 & 15) << 2) | (chr3 >> 6);
            enc4 = chr3 & 63;

            if (isNaN(chr2)) {
                enc3 = enc4 = 64;
            } else if (isNaN(chr3)) {
                enc4 = 64;
            }

            output = output +
                this._keyStr.charAt(enc1) + this._keyStr.charAt(enc2) +
                this._keyStr.charAt(enc3) + this._keyStr.charAt(enc4);

        }

        return output;
    },

    // public method for decoding
    decode: function (input) {
        var output = "";
        var chr1, chr2, chr3;
        var enc1, enc2, enc3, enc4;
        var i = 0;

        input = input.replace(/[^A-Za-z0-9\+\/\=]/g, "");

        while (i < input.length) {

            enc1 = this._keyStr.indexOf(input.charAt(i++));
            enc2 = this._keyStr.indexOf(input.charAt(i++));
            enc3 = this._keyStr.indexOf(input.charAt(i++));
            enc4 = this._keyStr.indexOf(input.charAt(i++));

            chr1 = (enc1 << 2) | (enc2 >> 4);
            chr2 = ((enc2 & 15) << 4) | (enc3 >> 2);
            chr3 = ((enc3 & 3) << 6) | enc4;

            output = output + String.fromCharCode(chr1);

            if (enc3 != 64) {
                output = output + String.fromCharCode(chr2);
            }
            if (enc4 != 64) {
                output = output + String.fromCharCode(chr3);
            }

        }

        output = Base64._utf8_decode(output);

        return output;

    },

    // private method for UTF-8 encoding
    _utf8_encode: function (string) {
        string = string.replace(/\r\n/g, "\n");
        var utftext = "";

        for (var n = 0; n < string.length; n++) {

            var c = string.charCodeAt(n);

            if (c < 128) {
                utftext += String.fromCharCode(c);
            }
            else if ((c > 127) && (c < 2048)) {
                utftext += String.fromCharCode((c >> 6) | 192);
                utftext += String.fromCharCode((c & 63) | 128);
            }
            else {
                utftext += String.fromCharCode((c >> 12) | 224);
                utftext += String.fromCharCode(((c >> 6) & 63) | 128);
                utftext += String.fromCharCode((c & 63) | 128);
            }

        }

        return utftext;
    },

    // private method for UTF-8 decoding
    _utf8_decode: function (utftext) {
        var string = "";
        var i = 0;
        var c = c1 = c2 = 0;

        while (i < utftext.length) {

            c = utftext.charCodeAt(i);

            if (c < 128) {
                string += String.fromCharCode(c);
                i++;
            }
            else if ((c > 191) && (c < 224)) {
                c2 = utftext.charCodeAt(i + 1);
                string += String.fromCharCode(((c & 31) << 6) | (c2 & 63));
                i += 2;
            }
            else {
                c2 = utftext.charCodeAt(i + 1);
                c3 = utftext.charCodeAt(i + 2);
                string += String.fromCharCode(((c & 15) << 12) | ((c2 & 63) << 6) | (c3 & 63));
                i += 3;
            }

        }

        return string;
    }

}

function addRequestVerificationToken(data) {
    data.__RequestVerificationToken = $('input[name=__RequestVerificationToken]').val();
    return data;
};

function showPreloader(id) {
    var dialog = $('#' + id).data('dialog');
    dialog.open();
}

function closePreloader(id) {
    var dialog = $('#' + id).data('dialog');
    dialog.close();
}

function showDialog(id) {
    var dialog = $(id).data('dialog');
    dialog.open();
}

function obtenerURL(sUrl) {
    var path_name = "";
    if (sUrl.length <= 0) {
        console.log("obtenerURL: la URL es un parametro requerido");
        return;
    } //Agregar slash en caso de que la url no lo contenga 
    if (sUrl.substr(0, 1) != "/") {
        sUrl = "/" + sUrl;
    }
    // Cuando el sitio este alojado en una ruta distinta al nodo raiz 
    if (window.location.pathname != "/") {
        var href = window.location.href.split('/');
        path_name = window.location.pathname;
        //path_name = href[0] + '//' + href[2] + '/' + href[3];
    }
    return (path_name + sUrl);
}

function getParameterByName(name, url) {
    if (!url) url = window.location.href;
    url = url.toLowerCase(); // This is just to avoid case sensitiveness  
    name = name.replace(/[\[\]]/g, "\\$&").toLowerCase();// This is just to avoid case sensitiveness for query parameter name
    var regex = new RegExp("[?&]" + name + "(=([^&#]*)|&|#|$)"),
        results = regex.exec(url);
    if (!results) return null;
    if (!results[2]) return '';
    return decodeURIComponent(results[2].replace(/\+/g, " "));
}

function AlertWaiting() {
    this.show = function () {
        if (alertify.alertwaiting().isOpen()) { alertify.alertwaiting().close(); }

        var dialog = '<img src="data:image/gif;base64,R0lGODlhIAAgAMMAAP///5qamgQEBB4eHtjY2OTk5La2toSEhMbGxlZWVry8vDY2NgAAAAAAAAAAAAAAACH+IyhjKSAxOTk1LTIwMTMgaHR0cDovL3d3dy5naWZkZS5jb20vACH/C05FVFNDQVBFMi4wAwEAAAAh+QQBCgAMACwAAAAAIAAgAAAE5xDISWkQoerN5xBDRRSdZhyVoFKIYZRUMQwJpQpT4SowNQ8vyW2icBF6k8NsMRkCCDskZTFDAZwuA0kqMfxIQ6gBQRFvAYbFIjNJVDW6XNE4CagXiSBiwO60smQUCXd4Rz1ZBgpnFAaDd0hihh12C0E9kjAFVlycXIg7BQgBB6SlnJ87paqbSKiKoqusnbMdmDC2tXQlkUhziYtyWTxIfy6BE8WJt5YFvpJivxNaGmLHT0VnOgaYf0dZXS7APdpB309RnHOG5gDqXGLDaC457D1zZ/V/nmOM82XiHQrYKhKP1oZmADdEAAAh+QQBCgAMACwAAAAAGAAXAAAEcpDJSeswNeu5xqpHsInUYFKLIGjLV55SoA5sq0wmLQ3qoQWtBA52mG0YiRYpxyCoBKRNy8VMqFzNw+GGwlJkiIlCqyVICAvMkWIghwrrDcHti2/Gh7D9qN774wUKBoOEfwyChIV/gYmDho+QkZKTR3p7EQAh+QQBCgAMACwBAAAAHQAOAAAEcpDJSadZNevN0kqZwU2EUi0olQzDKB2HOKGYZLD1CB/EnEoLlkui2PlyuKEkADMxaIwCayCbFDYE4zN1YIEmB4EgUDAoehNmTNNaDsQCnmGuuEYPiBFiARfUEHNzeUp9VBQFCoFOLmFxWHNoQwmRWEocEQAh+QQBCgAMACwHAAAAGQARAAAEaZDJydBBNOsdTsjfxhREdpzZsoiGgU3nQaki0xoFjEqJGmqElkInYwRUiRpD0SoxYhLVSlm4SaAMWmYwMFAQTY1h4RxzB2JnLXEeJJWb9pTihRu5dvghl+/7NQKBggM/fYKHAn8MiAKEEQAh+QQBCgAMACwOAAAAEgAYAAAEZZAEwqq92ATMFOVMIF7BcYDhWBHmCYpb1SooXCktmsbt944GU6wSWCxQiEPBkjAuhiCK86irTBO0qvWp7Xq/lYF4TNWNz4MqerAwgL0HX/cgELi69foAilzkBWVVAQN5d1p0Am4RACH5BAEKAAwALA4AAAASAB4AAASAsKjCqr1YGYURvkYIBsFXhcZFpiZqGaTXigxClibjMnd+irYAq6I4HAgmwpBgNHJ8hUDzgPNNjz4LwpnFDLvg7GJMTnw/5PRCrE6E3xbKOzAYwOv1xTmZwA/cJgcCMgYLeCYJAgJrF4YmBYoCVV2CAnZvA4oHbwaRcAuKcmFUJhEAIfkEAQoADAAsDwABABEAHwAABHuQyclIoRgjY7IvnOJlCkeMFBGiFGdcbMUhKQdTN1OUJru5NJTLoMh5VIZTTKJcOj2HqDQQhEqvqGuU+uw6DYtFwBkOJ57lxSgwoCTKY8phMDhPxmqGYC+hDzoeewITdHkZghMLdCOIhIuHfBMJjxiNLR4DAm1OAQJxSxEAIfkEAQoADAAsCAAOABgAEgAABGyQyUkrMzirYvvMoOJ5ILaNaIoaKooch9hacE3MVBHURzAbixuDwJMxEIJDZ7GoKGBCxkIgSDAGWAmzOQpQBRzsQBJgWj0DqvKalSSYPhp1LBFTtp20IM6mT5gdVFx1bRN8FTsVCwODOB9+KhEAIfkEAQoADAAsAgASAB0ADgAABHiQyUmrZeRSkrQlhpFdhyAcHqWEhlJQxmCaKYWwBiIts7DoHsThxSisQoSSaRCQEJgWw+HQnBBWhYAJNUkMBqiFWDI9jCzESey7GzMU5dpkwW4vJoGpQs743u1WcTV0AwZzbhJ5XClfHYd/EwpnHoYVCQuOfHKQNREAIfkEAQoADAAsAAAPABkAEQAABGcwDEEruzjrW7vYYCZ5X2ie6FkcKToMQQsu7ytnST0kKGFstcXvItz4DIiMwaZZLC6H6MVALaCcT0aUxTgqTglnTCu9KKiEUMDJg5YZBepwlnVzLwhqyKnZaghWahoBB2M3GggHSRsRACH5BAEKAAwALAEACAARABgAAARc0AhxmL34nilGyCCzcMKChFgwkGi2UW1GVHFt33iu72iwLDZF4verEYGElu+XuBQOJ9DvcwkcDgaGYYuaXS3bbOh6UIC5IAX5Eh5fk2exC4tpgwhyi8JgvhUUBREAIfkEBQoADAAsAAACAA4AHQAABHKQycnCoXgKsTLeguFNxzaM07BdaAASaUctW5IOwyIyiMBKCRwuAfMYFsITioELLBmF59KQWFivmatWRqFuudLwDqU4HBAjgnntsawDUUzZHEBLDHGG+qCgIAwGRR4FgGMeCoCCGQSAfWSAeUYGdigFihEAOw=="><br><div>Por favor, espere un momento...</div>';
        //var dialog = '<div id="dialogPreloader"><div data-role="preloader" data-type="square" data-style="color" class="preloader-square color-style"><div class="square"></div><div class="square"></div><div class="square"></div><div class="square"></div></div>Por favor, espere un momento...</div>';
        alertify.alertwaiting().destroy();
        alertify.alertwaiting().set('defaultFocus', 'ok').set('basic', true).setting({ 'closable': false, 'maximizable': false }).setContent(dialog).show();
    };

    this.close = function () {
        if (alertify.alertwaiting().isOpen()) { alertify.alertwaiting().close(); }
    };
}

function Alertify(url_plus_data) {

    alertify.confirm().destroy();

    var contenido;
    var div = $("#ModalGenerico");

    url = url_plus_data.split("?", 2)[0];
    data = url_plus_data.split("?", 2)[1];

    decode_data = Base64.decode(data);
    json_data_config = JSON.parse(decode_data);
    //console.log(decode_data)
    //alertify.defaults.theme.ok = "success button";
    //alertify.defaults.theme.cancel = "danger button";

    div.load(obtenerURL(url), function () {
        contenido = div.html();
        div.empty();
        var modal = alertify.confirm()
            .setHeader((typeof (json_data_config.setHeader) === "undefined" ? '' : '<strong>' + json_data_config.setHeader + '</strong>'))
            .setContent(contenido)
            .set({
                'startMaximized': (typeof (json_data_config.startMaximized) === "undefined" ? true : json_data_config.startMaximized),
                'resizable': (typeof (json_data_config.resizable) === "undefined" ? true : json_data_config.resizable),
                'basic': (typeof (json_data_config.basic) === "undefined" ? true : json_data_config.basic),
                'closable': (typeof (json_data_config.closable) === "undefined" ? true : json_data_config.closable),
                'onok': function () { eval(json_data_config.onok); }
            })
            .resizeTo((typeof (json_data_config.width) === "undefined" ? '50%' : json_data_config.width), (typeof (json_data_config.height) === "undefined" ? '50%' : json_data_config.height))
            .show();
        //PreventKeyEnterOnAlertify();
    });
}

function PreventKeyEnterOnAlertify() {
    $(document).on('keyup keydown', 'input, textarea, .select2-search__field', function (ev) {
        console.log(1)
        isPressEnter = false;
        var me = $(this);
        if (ev.which === 13) { console.log(2); return (isPressEnter = true); }
        if (me.data('listening') != 1) {
            me.data('listening', 1).keydown(function (ev) {
                if ((ev.keyCode ? ev.keyCode : ev.which) === 13) { console.log(3); return (isPressEnter = true); }
            });
        }
    });
}

//z-index select2 en Alertify
function fn_renderSelect2Alert(root) {
    var select = $(root).find("select");

    select.select2({ match: matchCustom, placeholder: "Seleccione un registro", language: "es" }).on("select2:open", function (e) {
        var id = e.target.id;
        var div_results = "#select2-" + id + "-results"; //obtener dinamicamente el id del select2
        $(div_results).parents(".select2-container").css({ 'z-index': '3000' }); //buscar el select2 y aplicar un index mayor a los demas controles
    });
}

function matchCustom(params, data) {
    // If there are no search terms, return all of the data
    if ($.trim(params.term) === '') { return data; }

    // Do not display the item if there is no 'text' property
    if (typeof data.text === 'undefined') { return null; }

    // `params.term` is the user's search term
    // `data.id` should be checked against
    // `data.text` should be checked against
    var q = params.term.toLowerCase();
    var cdn = new Array();
    var valor = true;

    cdn = q.split(" ");
    $.each(cdn, function (i, item) {
        if (valor !== false) {
            if ($.trim(item) !== '') {
                if (data.text.toLowerCase().indexOf(item) > -1 || data.id.toLowerCase().indexOf(item) > -1) {
                    valor = true;
                } else {
                    valor = false;
                }
            }
        }
    });

    if (valor === true) {
        return $.extend({}, data, true);
    }

    // Return `null` if the term should not be displayed
    return null;
}

function showalertify() {
    //    if (alertify.alertwaiting().isOpen())
    //    { alertify.alertwaiting().close(); }

    //    //var dialog = '<div id="dialogPreloader"><div data-role="preloader" data-type="square" data-style="color" class="preloader-square color-style"><div class="square"></div><div class="square"></div><div class="square"></div><div class="square"></div></div>Por favor, espere un momento...</div>';
    //    //alertify.alert().destroy();
    //    //alertify.alert().set('basic', true).setContent(dialog).show();
    //    var dialog = '<div id="dialogPreloader"><div data-role="preloader" data-type="square" data-style="color" class="preloader-square color-style"><div class="square"></div><div class="square"></div><div class="square"></div><div class="square"></div></div>Por favor, espere un momento...</div>';
    //    alertify.alertwaiting().destroy();
    //    alertify.alertwaiting().set('basic', true).setting({ 'closable': false }).setContent(dialog).show();
}

function closealertify() {
    //    if (alertify.alertwaiting().isOpen())
    //    { alertify.alertwaiting().close(); }
}

function showalertifyfail(error_number, error_message) {
    //destruimos el último objeto instanciado de alertify
    alertify.alert().destroy();
    //verificamos si el error corresponde a que la sesión del usuario ha finalizada
    //entonces redireccionamos al usuario al login
    if (error_number === 887) {

        alertify.alert().setting({
            'title': 'Advertencia'
            , 'modal': true
            , 'label': 'OK'
            , 'message': error_message
            , 'onok': function () {
                var oaut = new UICredenciales();
                oaut.mostrar();
                //url = $(location).attr("origin") + obtenerURL("/Usuario/Login");
                //$(location).attr("href", url);
            }
        }).show();
    }
    else if (error_number === 888) {

        alertify.alert().setting({
            'title': 'Advertencia'
            , 'modal': true
            , 'label': 'OK'
            , 'message': error_message
            , 'onok': function () {
                var oaut = new UICredenciales();
                oaut.mostrar();
                //url = $(location).attr("origin") + obtenerURL("/Usuario/Login");
                //$(location).attr("href", url);
            }
        }).show();
    }
    else {

        alertify.alert().setting({
            'title': 'Advertencia'
            , 'modal': true
            , 'label': 'OK'
            , 'message': error_message
        }).show();
    }
}

function SiguientePaso() {
    console.log('Siguiente paso...');
    //$.get(obtenerURL("/Mantenimiento/_mttoSistema"), function (data) {
    //    $("#cell-content").html(data);
    //    //  alert( "Load was performed." );
    //});
}

/*
 * UIAutorizacion: implementación
 var oaut = new UIAutorizacion()
 oaut.mostrar('PruebaAutorizacion', 'SiguientePaso()')
 */
function UIAutorizacion() {
    //this.accionmodulo;
    //this.ejecutarfuncion;
    this.mostrar = function (AccionModulo, EjecutarFuncion) {
        //var AccionModulo = this.accionmodulo;
        //var EjecutarFuncion  = this.ejecutarfuncion;
        $.ajax({
            url: obtenerURL("Usuario/_FormularioAutorizacion?AccionModulo=" + AccionModulo),
            type: "get",
            dataType: "html",
            //beforeSend: function () { showalertify(); },
            success: function (result) {
                //closealertify();
                var modal = alertify.confirm().setHeader("<h2>Autorización</h2>")
                    .set({
                        "labels": { ok: "Aceptar", cancel: "Cancelar" },
                        "modal": true,
                        'resizable': false,
                        'closable': false,
                        "onok": function () {
                            //var root = modal.elements.root;
                            var validar = true;
                            var usuarioid = $("#UsuarioAutorizacion").val();
                            var password = $("#PasswordAutorizacion").val();
                            //console.log(usuarioid)
                            if (usuarioid === "" || password === "") {
                                validar = false;
                                alertify.alert().destroy();
                                alertify.set('notifier', 'position', 'top-right');
                                alertify.error("Es obligatorio llenar todos los campos.");
                            }
                            //console.log(validar)
                            if (validar === true) {
                                password = Base64.encode(password);
                                //console.log(password)
                                $.post(obtenerURL("/Usuario/TieneAutorizacion")
                                    , { "UsuarioId": usuarioid, "Password": password, "Modulo": AccionModulo }
                                    , function (data) {
                                        var r = JSON.parse(data)
                                        //console.log(data)
                                        if (r.Retorno === "0") {
                                            alertify.alert().destroy();
                                            alertify.set('notifier', 'position', 'top-right');
                                            alertify.success(r.Mensaje);
                                            alertify.confirm().destroy();
                                            alertify.confirm().close();
                                            $(Function(EjecutarFuncion));
                                        }
                                        else {
                                            alertify.alert().destroy();
                                            alertify.set('notifier', 'position', 'top-right');
                                            alertify.error(r.Mensaje);
                                            return false;
                                        }
                                    });
                            }
                            return false;
                        }
                    }).resizeTo("500", "500").setContent(result).show();
            }
        });
    };
}

/*
 *UIAutorizacion: implementación
 var oaut = new UICredenciales()
 oaut.mostrar('SiguientePaso()')
 */
function UICredenciales() {
    //this.accionmodulo;
    //this.ejecutarfuncion;
    this.mostrar = function () {
        //var AccionModulo = this.accionmodulo;
        //var EjecutarFuncion  = this.ejecutarfuncion;
        $.ajax({
            url: obtenerURL("Usuario/_FormularioCredenciales"),
            type: "get",
            dataType: "html",
            //beforeSend: function () { showalertify(); },
            success: function (result) {
                //closealertify();
                var modal = alertify.confirm().setHeader("<h2>Autenticación</h2>")
                    .set({
                        "labels": { ok: "Aceptar", cancel: "Cancelar" },
                        "modal": true,
                        'closable': false,
                        'resizable': false,
                        "oncancel": function (closeEvent) {
                            //console.log("oncancel")
                            var url = $(location).attr("origin") + obtenerURL("/Usuario/Login");
                            $(location).attr("href", url);
                        },
                        //"onclose": function (closeEvent) {
                        //    console.log("onclose")
                        //    var url = $(location).attr("origin") + obtenerURL("/Usuario/Login");
                        //    $(location).attr("href", url);
                        //},
                        "onshow": function () {
                            //Una vez que carga la vista en el modal confirm
                            var root = this.elements.root;
                            var usuariocredenciales = $(root).find("#UsuarioCredenciales");
                            //console.log(usuariocredenciales);
                            //console.log($("#hUsuarioId").val());
                            usuariocredenciales.val($("#hUsuarioId").val());
                        },
                        "onok": function () {
                            //var root = modal.elements.root;
                            var validar = true;
                            var usuarioid = $("#hUsuarioId").val();
                            var password = $("#PasswordCredenciales").val();
                            var rolid = $("#hRolId").val();
                            var entidadid = $("#hEntidadId").val();

                            //console.log(usuarioid)
                            if (usuarioid === "" || password === "") {
                                validar = false;
                                alertify.alert().destroy();
                                alertify.set('notifier', 'position', 'top-right');
                                alertify.error("Es obligatorio llenar todos los campos.");
                            }
                            //console.log(validar)
                            if (validar === true) {
                                password = Base64.encode(password);
                                rolid = Base64.encode(rolid);
                                entidadid = Base64.encode(entidadid);
                                //console.log(password)
                                $.post(obtenerURL("/Usuario/CredencialesCorrectas")
                                    , { "UsuarioId": usuarioid, "Password": password, "RolId": rolid, "EntidadId": entidadid }
                                    , function (data) {
                                        //console.log(data)
                                        if (data === "True") {
                                            alertify.alert().destroy();
                                            alertify.set('notifier', 'position', 'top-right');
                                            alertify.success("Datos correctos!");
                                            alertify.confirm().destroy();
                                            alertify.confirm().close();
                                            //$(Function(EjecutarFuncion));
                                        }
                                        else {
                                            $("#PasswordCredenciales").val("");
                                            alertify.alert().destroy();
                                            alertify.set('notifier', 'position', 'top-right');
                                            alertify.error("Datos incorrectos. Verificar credenciales|permisos");
                                            return false;
                                        }
                                    });
                            }
                            return false;
                        }
                    }).resizeTo("500", "500").setContent(result).show();
            }
        });
    };
}

//Llamado
/*
Forma 1:
//la variable "resultado" que está dentro de la promesa, devuelve valores "true" o "false", 
//hasta que el valor devuelto por la promesa sea "true" el usuario podrá continuar con la gestión

//Usuario autenticado mediante Gaia
UIConfirmarCodigoSMS().then( (resultado) => console.log("Resultado:",resultado) );

//Usuario no autenticado (CERBEROS)
UIConfirmarCodigoSMS('{"celular":"58830858","correo":"vflores@poderjudicial.gob.ni"}').then( (resultado) => console.log("Resultado:",resultado) );

*/

var UIConfirmarCodigoSMS = function (parametros) {
    return new Promise((resolve, rejection) => {

        //UIConfirmarCodigoSMS(resolve, rejection);
        //inicio
        console.log("parametros: " + parametros);
        var celular = "";
        var correo = "";

        if (!(typeof (parametros) === "undefined")) {
            var celular = JSON.parse(parametros).celular;
            var correo = JSON.parse(parametros).correo;
        }

        $.ajax({
            url: obtenerURL("Usuario/_FormularioEnviarPIN"),
            data: { "celular": celular, "correo": correo },
            type: "get",
            dataType: "html",
            //beforeSend: function () { showalertify(); },
            success: function (result) {
                //closealertify();
                var modal = alertify.confirm().setHeader("")
                    .set({
                        "labels": { ok: "Confirmar", cancel: "Cancelar" },
                        "modal": true,
                        'closable': false,
                        'resizable': false,
                        "oncancel": function (closeEvent) {
                            alertify.alert().destroy();
                            alertify.set('notifier', 'position', 'top-right');
                            alertify.error("El usuario a cancelado la operación");
                            //resultadoConfirmacion = false;
                            resolve(false);
                            return true;
                        },
                        //"onclose": function (closeEvent) {
                        //    console.log("onclose")
                        //},
                        "onshow": function () {
                            //Una vez que carga la vista en el modal confirm
                            var root = this.elements.root;
                        },
                        "onok": function () {

                            var root = this.elements.root;
                            var validar = true;

                            var txtCodigo = $(root).find("#txtCodigoPIN");

                            if (txtCodigo === "") {
                                validar = false;
                                alertify.alert().destroy();
                                alertify.set('notifier', 'position', 'top-right');
                                alertify.error("Es requerido ingresar el código de verificación.");
                            }

                            if (validar === true) {

                                $.post(obtenerURL("/Usuario/ConfirmarPIN")
                                    , { "codigo": txtCodigo.val() }
                                    , function (data) {
                                        console.log(data);
                                        var r = JSON.parse(data)

                                        if (r.Retorno === "0") {
                                            alertify.alert().destroy();
                                            alertify.set('notifier', 'position', 'top-right');
                                            alertify.success(r.Mensaje);
                                            alertify.confirm().destroy();
                                            alertify.confirm().close();
                                            resolve(true);

                                            return true;
                                        }
                                        else {
                                            alertify.alert().destroy();
                                            alertify.set('notifier', 'position', 'top-right');
                                            alertify.error(r.Mensaje);

                                            return false;
                                        }
                                    });
                            }
                            return false;
                        }
                    }).resizeTo("250", "350").setContent(result).show();
            }
            , error: function (xhr, status, errorThrown) {
                //Here the status code can be retrieved like;
                //xhr.status;
                //The message added to Response object in Controller can be retrieved as following.
                //xhr.responseText;
                alertify.alert().destroy();
                alertify.set('notifier', 'position', 'top-right');
                alertify.error("Ha ocurrido un problema al cargar la ventana de confirmación");
            }
        });
        //fin
    });
}
//function ()

function EnviarCodigoSMS() {
    var strCelularPIN = ($("#hTipoIdentificadorId").val() === "ANON" ? $("#hCelularPIN").val() : "");
    var strCorreoPIN = ($("#hTipoIdentificadorId").val() === "ANON" ? $("#hCorreoPIN").val() : "");

    $.post(obtenerURL("/Usuario/EnviarPIN")
        , { "celular": strCelularPIN, "correo": strCorreoPIN }
        , function (data) {
            console.log(data);
            var r = JSON.parse(data)

            if (r.Retorno === "0") {
                alertify.alert().destroy();
                alertify.set('notifier', 'position', 'top-right');
                alertify.success(r.Mensaje);

                $("#txtCodigoPIN").attr('disabled', false);
                $("#btnEnviarCodigo").attr('value', "Reenviar PIN");

                return false;
            }
            else {

                alertify.alert().destroy();
                alertify.set('notifier', 'position', 'top-right');
                alertify.error(r.Mensaje);

                return false;
            }
        });
}

//actualmente no funcional
/*
function FormularioAutorizacion(AccionModulo, EjecutarFuncion) {
    $.ajax({
        url: obtenerURL("Usuario/_FormularioAutorizacion?AccionModulo=" + AccionModulo),
        type: "get",
        dataType: "html",
        //beforeSend: function () { showalertify(); },
        success: function (result) {
            //closealertify();
            var modal = alertify.confirm().setHeader("<h2>Autorización</h2>")
                .set({
                    "labels": { ok: "Aceptar", cancel: "Cancelar" },
                    "modal": true,
                    'resizable': false,
                    "onok": function () {
                        //var root = modal.elements.root;
                        var validar = true;
                        var usuarioid = $("#UsuarioAutorizacion").val();
                        var password = $("#PasswordAutorizacion").val();

                        if (usuarioid === "" || password === "") {
                            validar = false;
                            alertify.alert().destroy();
                            alertify.set('notifier', 'position', 'top-right');
                            alertify.error("Es obligatorio llenar todos los campos.");
                        }

                        if (validar === true) {
                            password = Base64.encode(password);

                            $.post(obtenerURL("/Usuario/TieneAutorizacion")
                                    , { "UsuarioId": usuarioid, "Password": password, "Modulo": AccionModulo }
                                    , function (data) {
                                        console.log(data)
                                        if (data === "True") {
                                            alertify.alert().destroy();
                                            alertify.set('notifier', 'position', 'top-right');
                                            alertify.success("Datos correctos!");
                                            alertify.confirm().destroy();
                                            alertify.confirm().close();
                                            $(Function(EjecutarFuncion));
                                        }
                                        else {
                                            alertify.alert().destroy();
                                            alertify.set('notifier', 'position', 'top-right');
                                            alertify.error("Datos incorrectos. Verificar credenciales|permisos");
                                            return false;
                                        }
                                    });
                        }
                        return false;
                    }
                }).resizeTo("500", "500").setContent(result).show();
        }
    });
}
*/

function DisplayFecha(FechaTime) {
    var date = new Date(FechaTime);
    var month = date.getMonth() + 1;
    var day = date.getUTCDate();
    return ((day < 10 ? '0' + day : '' + day) + "/" + (month < 10 ? '0' + month : '' + month) + "/" + date.getFullYear());
}

function Select2Dinamica(url, Select2Id) {
    $.each($("#" + Select2Id + " option"), function (oi, oitem) {
        if (oi !== 0) {
            oitem.remove();
        }
    });

    $.ajax({
        url: url,
        type: "get",
        contentType: "application/json; charset=utf-8",
        //data: { "SedeId": SedeId, "UsuarioOrgano": "GAM4" },
        dataType: "json",
        success: function (jsonData) {
            var objectData = JSON.parse(jsonData);

            $.each(objectData, function (key, value) {
                //$("#" + Select2Id).append($("<option></option>").val(value.CODIGO).html(value.DESCRIPCION));
                $("#" + Select2Id).append($('<option/>', {
                    value: value.CODIGO,
                    text: value.DESCRIPCION
                }));
            });
        },
        error: function (result) { alertify.alert("Advertencia", "Error llenado: #" + Select2Id); }
    });
}

function ConvertFecha(FechaString) {
    if (FechaString === "") {
        return FechaString;
    } else {
        var FechaSplit = FechaString.split("/");

        var FechaDate = $.formatDateTime('yy/mm/dd g:ii a', new Date(FechaSplit[2], (FechaSplit[1] - 1), FechaSplit[0]));

        return FechaDate;
    }
}

//Función para igualar el alto de columnas
function EqualHeight(group) {
    tallest = 0;
    group.each(function () {
        thisHeight = $(this).height();
        if (thisHeight > tallest) {
            tallest = thisHeight;
        }
    });
    group.height(tallest);
};

function FormatoHoraAmPm(obj) {
    $(obj).inputmask({
        mask: "h:s t\\m",
        placeholder: "hh:mm xm",
        alias: "datetime",
        hourFormat: "12"
    });
}

function convertTo24Hour(time) {

    var hours = Number(time.match(/^(\d+)/)[1]);
    var minutes = Number(time.match(/:(\d+)/)[1]);
    var AMPM = time.match(/\s(.*)$/)[1];
    if ((AMPM == "pm" || AMPM == "PM") && hours < 12) hours = hours + 12;
    if ((AMPM == "am" || AMPM == "AM") && hours == 12) hours = hours - 12;
    var sHours = hours.toString();
    var sMinutes = minutes.toString();
    if (hours < 10) sHours = "0" + sHours;
    if (minutes < 10) sMinutes = "0" + sMinutes;
    return sHours + ":" + sMinutes
}

function formateaFechaHora(dates) {
    var date = new Date(dates);
    var hours = date.getHours();
    var minutes = date.getMinutes();
    var ampm = hours >= 12 ? 'pm' : 'am';
    hours = hours % 12;
    hours = hours ? hours : 12; // the hour '0' should be '12'
    minutes = minutes < 10 ? '0' + minutes : minutes;
    var strTime = hours + ':' + minutes + ' ' + ampm;
    return date.getMonth() + 1 + "/" + date.getDate() + "/" + date.getFullYear() + "  " + strTime;
}

function SumarMinutos(dt, minutes) {
    return new Date(dt.getTime() + minutes * 60000);
}
