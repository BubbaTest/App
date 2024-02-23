var bandera;
//Tipo de acontecimientos que cargan el formulario de asignación de entrevista
//var TipoAcontecimientoIdEntrevista = ["COMPENT001", "ENTRTES002", "ENTRUSU001", "ENTVIFA001", "ENTVITE001", "ENTVIUS001"];

$(document).ready(function () {
    limpiarDexieAD();
});

function limpiarDexieAD()
{
    db.VALIDACION.clear();
    db.ASUNTOS.clear();
    db.INTERVINIENTES.clear();
    db.INTERVINIENTE_FAMILIAR.clear();
    db.INTERVINIENTES_ABOGADOS.clear();
    db.INTERVINIENTE_CAUCION_ECONOMICA.clear();
    db.INTERVINIENTE_CENTRO_PENITENCIARIO.clear();
    db.INTERVINIENTE_APELATIVO.clear();
    db.INTERVINIENTE_PARENTESCO.clear();
    db.ACONTECIMIENTOS.clear();
    db.jsAsuntodes.clear();
    db.ValidacionControles.clear();
}

function inputControlRol(pestana) {
    db.ValidacionControles.where("tabPestana").equals(pestana).toArray(function (i) {
        $.each(i, function (e, item) {
            $("#" + item.inputControl).attr("disabled", "disabled");
        });
    });
}

function tabControlRol(pestana) {
    db.ValidacionControles.where("tabPestana").equals(pestana).toArray(function (i) {
        $.each(i, function (e, item) {
            if (item.inputControl === "button")
            {
                $("button").addClass("no-visible");
            } else
            {
                $("#" + item.inputControl).parents("li").addClass("disabled");
            }
        });
    });
}

//Guardar/crear json
function AsuntoJsonString() {
    var Asunto = $("#txtAsuntoNoAsunto").val();
    var Nig = $("#txtAsuntoNoAsuntoPrincipal").val();
    var Materia = $("#ddlAsuntoMateria").val();
    var Clase = $("#ddlAsuntoClase").val();
    var Motivo = $("#ddlAsuntoMotivo").val();
    var Organo = $("#ddlAsuntoOrgano").val();
    //var Organo = '0201'; $.formatDateTime('dd/mm/y g:ii a', new Date())
    //var FechaCreacion = new Date($("#dptAsuntoFechaCreacion").val()).toISOString().slice(0, 10);
    var FechaCreacion = ConvertFecha($("#dptAsuntoFechaCreacion").val());
    var FechaEntrada = ConvertFecha($("#txtAsuntoFechaEntrada").val());
    //var FechaCreacion = ($("#dptAsuntoFechaCreacion").val()==="" ? $("#dptAsuntoFechaCreacion").val() : $.formatDateTime('yy-mm-dd g:ii a', new Date($("#dptAsuntoFechaCreacion").val())));
    //var FechaEntrada = ($("#txtAsuntoFechaEntrada").val()==="" ? $("#txtAsuntoFechaEntrada").val() : $.formatDateTime('yy-mm-dd g:ii a', new Date($("#txtAsuntoFechaEntrada").val())));
    var Fase = $("#ddlAsuntoFase").val();
    var FechaFase = ConvertFecha($("#txtAsuntoFechaFase").val());
    //var FechaFase = ($("#txtAsuntoFechaFase").val()==="" ? $("#txtAsuntoFechaFase").val() : $.formatDateTime('yy-mm-dd g:ii a', new Date($("#txtAsuntoFechaFase").val())));
    var Estado = $("#ddlAsuntoEstado").val();
    var FechaEstado = ConvertFecha($("#txtAsuntoFechaFase").val());
    //var FechaEstado = ($("#txtAsuntoFechaFase").val()==="" ? $("#txtAsuntoFechaFase").val() : $.formatDateTime('yy-mm-dd g:ii a', new Date($("#txtAsuntoFechaEstado").val())));
    var MotivoEstado = $("#ddlAsuntoEstadoTerminacion").val();
    var FechaUltimaActuacion = ConvertFecha($("#txtAsuntoFechaUltimaActuacion").val());
    //var FechaUltimaActuacion = ($("#txtAsuntoFechaUltimaActuacion").val()==="" ? $("#txtAsuntoFechaUltimaActuacion").val() : $.formatDateTime('yy-mm-dd g:ii a', new Date($("#txtAsuntoFechaUltimaActuacion").val())));
    var Procedimiento = $("#ddlAsuntoProcedimiento").val();
    var FechaUbicacion = ConvertFecha($("#txtAsuntoFechaUbicacion").val());
    //var FechaUbicacion = ($("#txtAsuntoFechaUbicacion").val()==="" ? $("#txtAsuntoFechaUbicacion").val() : $.formatDateTime('yy-mm-dd g:ii a', new Date($("#txtAsuntoFechaUbicacion").val())));
    var OrganoUbicacion = $("#ddlAsuntoOrganoUbicacion").val();
    var TipoUbicacion = $("#ddlAsuntoTipoUbicacion").val();
    var OrganoDestino = $("#ddlAsuntoOrganoDestino").val();
    var TipoUbicaionDestino = $("#ddlAsuntoTipoUbicacionDestino").val();
    //var a = $("#").val();
    //var a = $("#").val(); 
    var Descripcion = $("#txaAsuntoDescripcion").val();
    
    db.ASUNTOS.where("ASUNTO").equals(Asunto.replace(/-/g, "")).first(function (ad) {
        if (ad === null || ad === "" || ad === undefined)
        {
            db.ASUNTOS.add({ ASUNTO: Asunto.replace(/-/g, ""), NIG: Nig.replace(/-/g, ""), FECHA_ENTRADA: FechaEntrada, FECHA_ACTUACION: FechaUltimaActuacion, CLASE: Clase, ASUNTO_TIPO: "", DESCRIPCION: Descripcion, ORGANO: Organo, PONENTE: "", PROCEDIMIENTO_TIPO: Procedimiento, FASE: Fase, FASE_FECHA: FechaFase, ESTADO: Estado, ESTADO_FECHA: FechaEstado, NUM_FOLIOS_TOTAL: "", PROCEDIMIENTO_NUMERO: "", FECHA_ENTRADA_ORG: FechaCreacion, ORDEN_UBICACION: "", RESERVADO: "", JURISDICCION: "", PONENCIA: "", MOTIVO_TERMINACION: MotivoEstado, TIUF_ASIGNADO: "", SEDE_ASIGNADA: "", ASUNTO_ANTIGUO: "", MOTIVO: Motivo, MATERIA: Materia, IN_ACCIDENTAL: "", IN_CAMBIO_JUEZ: "", IN_ITINERAR: "", FISCALIA: "", NIG_ACUMULADO: "", MOTIVO_ITINERACION: "", POLICIA: "", PROCURADOR: "" });
        }else
        {
            db.ASUNTOS.update(Asunto.replace(/-/g, ""), { FECHA_ENTRADA: FechaEntrada, FECHA_ACTUACION: FechaUltimaActuacion, CLASE: Clase, ASUNTO_TIPO: "", DESCRIPCION: Descripcion, ORGANO: Organo, PONENTE: "", PROCEDIMIENTO_TIPO: Procedimiento, FASE: Fase, FASE_FECHA: FechaFase, ESTADO: Estado, ESTADO_FECHA: FechaEstado, NUM_FOLIOS_TOTAL: "", PROCEDIMIENTO_NUMERO: "", FECHA_ENTRADA_ORG: FechaCreacion, ORDEN_UBICACION: "", RESERVADO: "", JURISDICCION: "", PONENCIA: "", MOTIVO_TERMINACION: MotivoEstado, TIUF_ASIGNADO: "", SEDE_ASIGNADA: "", ASUNTO_ANTIGUO: "", MOTIVO: Motivo, MATERIA: Materia, IN_ACCIDENTAL: "", IN_CAMBIO_JUEZ: "", IN_ITINERAR: "", FISCALIA: "", NIG_ACUMULADO: "", MOTIVO_ITINERACION: "", POLICIA: "", PROCURADOR: "" })
        }
    });
    //ASUNTO, NIG, FECHA_ENTRADA, FECHA_ACTUACION, CLASE, ASUNTO_TIPO, DESCRIPCION, ORGANO, PONENTE, PROCEDIMIENTO_TIPO, FASE, FASE_FECHA, ESTADO, ESTADO_FECHA, NUM_FOLIOS_TOTAL, PROCEDIMIENTO_NUMERO, FECHA_ENTRADA_ORG, ORDEN_UBICACION, RESERVADO, JURISDICCION, PONENCIA, MOTIVO_TERMINACION, TIUF_ASIGNADO, SEDE_ASIGNADA, ASUNTO_ANTIGUO, MOTIVO, MATERIA, IN_ACCIDENTAL, IN_CAMBIO_JUEZ, IN_ITINERAR, FISCALIA, NIG_ACUMULADO, MOTIVO_ITINERACION, POLICIA, PROCURADOR
};

function GeneralJsonString() {
    var Asunto = $("#txtAsuntoNoAsunto").val();
    var dtIntDetalle = $('#dtIntDetalle').DataTable();
    var dtObject = dtIntDetalle.row(".selected").data();
    var PreNombre = $("#txtIntervinientePreNombre").val();
    var Nombre = $("#txtIntervinienteNombre").val();
    var Personeria = $("#dllPersoneria").val();
    var Entidad = $("#dllEntidad").val();
    var Intervencion = $("#ddlIntervencion").val();
    var Genero = $("#ddlGenero").val();
    var IdentificacionTipo = $("#ddlIdentificativoTipo").val();
    var Identificacion = $("#txtIntervinienteIdentificativo").val();
    var Edad = $("#ddlEdad").val();
    var Menor = $("#chkIntervinienteMenor").val();
    var Orden;

    if (dtObject === undefined || dtObject === null)
    {
        Orden = parseInt($("#txtOrdenInsert").val());
        //if (parseInt(Orden) !== 0)
        //{
        //    db.INTERVINIENTES.put({ ORDEN: parseInt(Orden), ASUNTO: Asunto.replace(/-/g, ""), PRENOMBRE: PreNombre, NOMBRE: Nombre, INTERVENCION: Intervencion, IDENTIFICATIVO: Identificacion.replace(/-/g, ""), DOMICILIO: "", TELEFONOS: "", SITUACION_LIBERTAD: "", SITUACION_LIB_FECHA: "", OBSERVACIONES: "", FAX: "", E_MAIL: "", BARRIO: "", CIUDAD: "", ESTADO: "", REGION: "", TIPO_IDENTIFICATIVO: IdentificacionTipo, FECHA_NACIMIENTO: "", ESTADO_CIVIL: "", PROFESION: "", CENTRO_PENITENCIARIO: "", SEXO: Genero, NACIONALIDAD: "", RESULTADO: "", RESULTADO_RECURSO: "", DEFENSOR_RESOLUCION: "", IN_MENOR: 0, EQUIVALENTE: "", EDAD: Edad, PERSONERIA: Personeria, FECHA_RESULTADO: "", FECHA_RESULTADO_RECURSO: "", ESTADO_NACIMIENTO: "", CIUDAD_NACIMIENTO: "", ESCOLARIDAD: "", TIPO_JUICIO: "" });
        //}
    }else
    {
        Orden = parseInt(dtObject["Orden"]);
        //db.INTERVINIENTES.update(parseInt(Orden), { ASUNTO: Asunto.replace(/-/g, ""), PRENOMBRE: PreNombre, NOMBRE: Nombre, INTERVENCION: Intervencion, IDENTIFICATIVO: Identificacion.replace(/-/g, ""), TIPO_IDENTIFICATIVO: IdentificacionTipo, SEXO: Genero, PERSONERIA: Personeria });
    }

    db.INTERVINIENTES.where("ORDEN").equals(parseInt(Orden)).first(function (i) {
        if (i === null || i === "" || i === undefined) {
            if (Orden !== 0)
            {
                db.INTERVINIENTES.put({ ORDEN: parseInt(Orden), ASUNTO: Asunto.replace(/-/g, ""), PRENOMBRE: PreNombre, NOMBRE: Nombre, INTERVENCION: Intervencion, IDENTIFICATIVO: Identificacion.replace(/-/g, ""), DOMICILIO: "", TELEFONOS: "", SITUACION_LIBERTAD: "", SITUACION_LIB_FECHA: "", OBSERVACIONES: "", FAX: "", E_MAIL: "", BARRIO: "", CIUDAD: "", ESTADO: "", REGION: "", TIPO_IDENTIFICATIVO: IdentificacionTipo, FECHA_NACIMIENTO: "", ESTADO_CIVIL: "", PROFESION: "", CENTRO_PENITENCIARIO: "", SEXO: Genero, NACIONALIDAD: "", RESULTADO: "", RESULTADO_RECURSO: "", DEFENSOR_RESOLUCION: "", IN_MENOR: 0, EQUIVALENTE: "", EDAD: Edad, PERSONERIA: Personeria, FECHA_RESULTADO: "", FECHA_RESULTADO_RECURSO: "", ESTADO_NACIMIENTO: "", CIUDAD_NACIMIENTO: "", ESCOLARIDAD: "", TIPO_JUICIO: "" });
            }
        } else {
            db.INTERVINIENTES.update(parseInt(Orden), { ASUNTO: Asunto.replace(/-/g, ""), PRENOMBRE: PreNombre, NOMBRE: Nombre, INTERVENCION: Intervencion, IDENTIFICATIVO: Identificacion.replace(/-/g, ""), TIPO_IDENTIFICATIVO: IdentificacionTipo, SEXO: Genero, PERSONERIA: Personeria });
        }
    });
}

function DireccionJsonString() {
    var dtIntDetalle = $('#dtIntDetalle').DataTable();
    var dtObject = dtIntDetalle.row(".selected").data();
    var table = $('#dtIntDetalle').DataTable();
    var Orden = table.row(".selected").data();
    var Domicilio = $("#txaIntervinienteDomicilio").val();
    var Telefono = $("#txaIntervinienteTelefonos").val();
    var Municipio = $("#ddlIntDireccionMunicipio").val();
    var Departamento = $("#ddlIntDireccionMunicipio :selected").parents("optgroup").attr("id");
    var Barrio = $("#ddlIntDireccionBarrio").val();
    var Email = $("#txtIntervinienteEmail").val();
    var Orden;

    if (dtObject === undefined || dtObject === null)
    { Orden = parseInt($("#txtOrdenInsert").val()); } else { Orden = parseInt(dtObject["Orden"]); }
    //ESTADO: Departamento,
    db.INTERVINIENTES.update(Orden, { DOMICILIO: Domicilio, TELEFONOS: Telefono, CIUDAD: Municipio, BARRIO: Barrio, E_MAIL: Email });
}

function GeneralleyJsonString() {
    var dtIntDetalle = $('#dtIntDetalle').DataTable();
    var dtObject = dtIntDetalle.row(".selected").data();
    var table = $('#dtIntDetalle').DataTable();
    var Orden = table.row(".selected").data();
    var FechaNacimiento = ConvertFecha($("#dtpFechaNacimiento").val());
    //var FechaNacimiento = ($("#dtpFechaNacimiento").val()==="" ? $("#dtpFechaNacimiento").val() : $.formatDateTime('yy-mm-dd g:ii a', new Date($("#dtpFechaNacimiento").val())));
    var EstadoCivil = $("#ddlEstadoCivil").val();
    var deptoNacimiento = $("#ddlDepartamentoNac").val();
    var mupioNacimiento = $("#ddlMunicipioNac").val();
    var Profesion = $("#ddlProfesion").val();
    var Escolaridad = $("#ddlEscolaridad").val();
    var Nacionalidad = $("#ddlNacionalidad").val();
    var Orden;

    if (dtObject === undefined || dtObject === null)
    { Orden = parseInt($("#txtOrdenInsert").val()); } else { Orden = parseInt(dtObject["Orden"]); }

    db.INTERVINIENTES.update(Orden, { FECHA_NACIMIENTO: FechaNacimiento, ESTADO_CIVIL: EstadoCivil, ESTADO_NACIMIENTO: deptoNacimiento, PROFESION: Profesion, CIUDAD_NACIMIENTO: mupioNacimiento, ESCOLARIDAD: Escolaridad, NACIONALIDAD: Nacionalidad });
}

//funcion para recorrer los campos de una tabla
function FamiliaresJsonString() {
    var JsondtIntervinienteFamiliar = [];
    var $headers = $("#dtIntervinienteFamiliar th");
    var $rows = $("#dtIntervinienteFamiliar tbody tr").each(function (index) {
        $cells = $(this).find("td");
        JsondtIntervinienteFamiliar[index] = {};
        $cells.each(function (cellIndex) {
            JsondtIntervinienteFamiliar[index][$($headers[cellIndex]).html()] = $(this).html();
        });
    });
}

//Funciones para mostar el dialog de las datatables
function showDialog(id) {
    var dialog = $(id).data('dialog');
    dialog.open();
}
//Funciones para cerrar el dialog de las datatables
function cerrarDialog(id) {
    var dialog = $("#" + id).data('dialog');
    $("#DivDetalle").empty();
    dialog.close();

}

function notificacionDialog(scaption, stype, scontent, b_keepOpen, t_timeout) {
    if (b_keepOpen === undefined) { b_keepOpen = false; }
    if (t_timeout === undefined) { t_timeout = 3000; }

    $.Notify({
        caption: scaption,
        content: scontent,
        type: stype,
        shadow: true,
        keepOpen: b_keepOpen,
        timeout: t_timeout
    });
}

//function mostrarDetalle(id) {
function mostrarDetalle() {
    var dialog = $("#ModalEdicionDatos").data('dialog');

    if (!dialog.element.data('opened')) {
        dialog.open();
    } else {
        dialog.close();
    }
}

//Funcion para cambiar de tab en base al id del frame. Cuando a la tab no se lo coloca la propieda id
function moverTab(idFrame) {
    var tabs = $(idFrame).parent().parent().children('.tabs'); //Obtener todas tabs que contiene el controlTab en el frame seleccionado
    var frames = $(idFrame).parent().children('.frame')          //Obtener los frames del controlTab

    tabs.children('li').removeClass('active');   //Remover la clase active a todas las tabs
    frames.css({ display: "none" });              //Ocultar todos lo frames

    //frames.empty();     //En caso de que las llamadas sean mediante ajax, limpiar los frame para no sobrecargar la pagina, el moverTab tiene que ser antes del load
    var newTab = tabs.children('li:has(a[href="' + idFrame + '"])');    //Obtener el tab que sera seleccionado a partir del id de idFrame
    newTab.addClass('active');

    $(idFrame).css({ display: "block" }); //mostramos el frame del tab seleccionado
};

function clickIntGenerales(tab) {
    var tabActive = $('ul[id="IntGenerales"] li[class="active"] a').attr('id');
    var tabotro = $('ul[id="IntOtros"] li[class="active"] a').attr('id');
    var dtIntDetalle = $('#dtIntDetalle').DataTable();
    var dtObject = dtIntDetalle.row(".selected").data();
    var Orden;

    if (dtObject === undefined || dtObject === null)
    { Orden = parseInt($("#txtOrdenInsert").val()) } else { Orden = parseInt(dtObject["Orden"]) }

    if (Orden === undefined || Orden === 0) {
        alertify.alert().destroy();
        alertify.alert('Advertencia', 'Debe seleccionar un registro de la tabla');
        alertify.alert().setting('modal', true);

        return false;
    } else {
        if (tabActive === "_01IntDatosGenerales") {
            if (ctrInterGeneral() === false) {
                alertify.alert().destroy();
                alertify.alert('Advertencia', 'Llenar todos los campos obligatorios');
                alertify.alert().setting('modal', true);

                return false;
            } else {
                GeneralJsonString();

                return true;
            }
        } else if (tabActive === "_02IntDireccion") {
            if (ctrInterDireccion() === false) {
                alertify.alert().destroy();
                alertify.alert('Advertencia', 'Llenar todos los campos obligatorios');
                alertify.alert().setting('modal', true);

                return false;
            } else {
                DireccionJsonString();

                return true;
            }
        } else {
            return true;
        }
    }
}

function clickIntOtros(tab) {
    var tabotro = $('ul[id="IntOtros"] li[class="active"] a').attr('id');

    if (tabotro === "_0901IntOtrosGenLey") {
        if (ctrInterLey() === false) {
            alertify.alert().destroy();
            alertify.alert('Advertencia', 'Llenar todos los campos obligatorios');
            alertify.alert().setting('modal', true);

            return false;
        } else {
            GeneralleyJsonString();

            return true;
        }
    } else {
        return true;
    }
}

function ConvertFecha(FechaString) {
    if (FechaString == "") {
        return FechaString;
    } else {
        var FechaSplit = FechaString.split("/");

        var FechaDate = $.formatDateTime('yy/mm/dd g:ii a', new Date(FechaSplit[2], (FechaSplit[1] - 1), FechaSplit[0]));

        return FechaDate;
    }
}

function cargardtAcontecimientos(AsuntoNicarao, AsuntoDefensoria, Procede) {
    $('#dtAcontecimientos').DataTable({
        ajax: {
            url: "/Nicarao/ActuacionObtenerListado",
            type: "get",
            data: { "AsuntoNicarao": AsuntoNicarao, "AsuntoDefensoria": AsuntoDefensoria, "OrdenActuacion": 0, "SedeId": SedeId, "Procede": Procede },
            dataSrc: ""
        },
        rowId: "RowId",
        columns: [
                  { data: "ORDEN", className: 'details-control' },
                  {
                      orderable: false, data: "TieneAsociado",
                      render: function (data) {
                          if (data == true) {
                              return '<span class="icon mif-plus fg-white bg-lightGreen" id="iconDetails"></span>';
                          } return '';
                      },
                      className: 'details-control'
                  },
                  { orderable: false, defaultContent: '<span id="iconDetails" class="icon mif-enlarge"></span>', className: 'dt-body-center' },
                  { data: "ACONTECIMIENTO_TIPO", className: 'details-control' },
                  { data: "DESCRIPCION", className: 'details-control' },
                  { data: "FECHA", className: 'details-control' }
        ],
        columnDefs: [{ targets: [0], visible: false, searchable: false },
                    { width: "25%", targets: 3 }, { width: "65%", targets: 4 }, { width: "8%", targets: 5 }
        ],
        order: [[0, 'desc']],
        fnRowCallback: function (row, data, index) {
            if (data["ProcedeAsunto"] === "NI") {
                $(row).addClass("fg-darkIndigo");
            } else {
                $(row).addClass("fg-darkEmerald");
            }
        },
        language: { url: "/Json/Spanish.json" },
        lengthMenu: [5],
        bLengthChange: false
    });
}

function CargardtAcontecimientosDoc(OrdenId) {
    $('#dtAcontecimientosDoc').DataTable({
        ajax: {
            url: "/Nicarao/ActuacionObtenerDocumento",
            type: "get",
            data: { "OrdenId": OrdenId, "SedeId": SedeId },
            dataSrc: ""
        },
        columns: [{ data: "ORDEN_DOC" },
                    { orderable: false, defaultContent: '<span class="icon mif-tablet"></span>', className: 'dt-body-center' },
                    { data: "TITULO" },
                    { data: "FOLIO_INICIAL" },
                    { data: "FOLIOS_NUM" }],
        order: [[1, 'asc']],
        columnDefs: [{ targets: [0], visible: false, searchable: false }],
        language: { url: "/Json/Spanish.json" },
        lengthMenu: [5],
        bLengthChange: false,
        searching: false
    });
}

function cargarAconteciminetoDetalle(AsuntoNicarao, AsuntoDefensoria, OrdenId, Procede) {
    $("#FrmAcontecimientosDetalle").load("/Nicarao/ActuacionObtenerDetalle?AsuntoNicarao=" + AsuntoNicarao + "&AsuntoDefensoria=" + AsuntoDefensoria + "&OrdenId=" + OrdenId + "&Procede=" + Procede + "&SedeId=" + SedeId, function () {
        $('#dtAcontecimientosDoc').dataTable().fnDestroy();
        CargardtAcontecimientosDoc(OrdenId);
    });
}

function CragarAcontecimientoAsociado(OrdenId, Procede, row, tr) {
    $.ajax({
        url: '/Nicarao/ActuacionesAsociadas',
        contentType: 'application/html ; charset:utf-8',
        data: { "OrdenId": OrdenId, "Procede": Procede, "SedeId": SedeId },
        type: 'GET',
        dataType: 'html'
    }).success(function (result) {

        row.child(result).show();
        tr.addClass('shown');
        //tr.find('#iconDetails').;
    }).error(function () {
        console.log('Ocurrio un error');
    });
}

function cargardtAcontecimientosAsociados(ordenId) {
    $("#dtAcontecimientosAsociados").DataTable({
        ajax: {
            url: "/Nicarao/ActuacionesObtenerListadoAsociado",
            type: "get",
            data: { "OrdenId": parseInt(ordenId), "SedeId": SedeId },
            dataSrc: ""
        },
        rowId: "RowId",
        columns: [
                  { data: "ORDEN", className: "tablesolicitud" },
                  {
                      orderable: false, data: "TieneAsociado",
                      render: function (data) {
                          if (data == true) {
                              return '<span class="icon mif-plus fg-white bg-lightGreen"></span>';
                          } return '';
                      },
                      className: 'tablesolicitud'
                  },
                  { orderable: false, defaultContent: '<span id="iconDetails" class="icon mif-enlarge" onclick="ampliarAsociado(this)"></span>', className: 'dt-body-center' },
                  { data: "ACONTECIMIENTO_TIPO", className: 'tablesolicitud' },
                  { data: "DESCRIPCION", className: 'tablesolicitud' },
                  { data: "FECHA", className: 'tablesolicitud' }
        ],
        columnDefs: [{ targets: [0], visible: false, searchable: false },
                    { width: "25%", targets: 3 }, { width: "65%", targets: 4 }, { width: "8%", targets: 5 }],
        fnRowCallback: function (row, data, index) {
            if (data["ProcedeAsunto"] === "NI") {
                $(row).addClass("fg-darkIndigo");
            } else {
                $(row).addClass("fg-darkEmerald");
            }
        },
        language: { url: "/Json/Spanish.json" },
        lengthMenu: [10],
        bLengthChange: false,
        searching: false,
        //bPaginate: false,
        //bInfo: false
    });
}

function AmpliarAcontecimientos(AsuntoNicarao, AsuntoDefensoria, OrdenId, TipoAcontecimiento, Procede) {
    var contenido;
    var div = $("#ModalGenerico");

    alertify.defaults.theme.ok = "success button";
    alertify.defaults.theme.cancel = "danger button";

    alertify.confirm().destroy();

    div.load("/Nicarao/AmpliarActuacion?AsuntoNicarao=" + AsuntoNicarao + "&AsuntoDefensoria=" + AsuntoDefensoria + "&OrdenId=" + OrdenId + "&TipoAcontecimiento=" + TipoAcontecimiento + "&Procede=" + Procede + "&SedeId=" + SedeId, function () {
        contenido = div.html();
        div.empty();
        var modal = alertify.confirm()
        .setHeader('<h4> <span class="mif-search mif-ani-float mif-ani-slow"></span>Busqueda de Expedientes</h4> ')
        .setContent(contenido)
        .set({ 'startMaximized': false, 'maximizable': true, 'resizable': true, 'labels': { ok: '< Ok >', cancel: '< Salir >' } })
        .resizeTo(1000, 500)
        .set('oncancel', function (closeEvent) {
        })
        .set('onok', function (closeEvent) {

        })
        .show();
    });
}

function cargarddlIntervencion(Clase, Intervencion) {
    $.getJSON("/Nicarao/IntervecionCargar?Clase=" + Clase).done(function (datajson) {
        var datoObject = JSON.parse(datajson);

        var Intervenciondll = $("#ddlIntervencion");

        $.each(datoObject, function (i, itemData) {
            Intervenciondll.append($('<option/>', {
                value: itemData.INTERVENCION,
                text: itemData.DESCRIPCION
            }));
        });
        Intervenciondll.val(Intervencion).trigger("change");
    });
}

function cargarSelect2(url, Select2Id, value) {
    $.ajax({
        url: url,
        type: "get",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (jsonData) {
            var obj = JSON.parse(jsonData);
            console.log(obj)
            $("#" + Select2Id + " option").each(function () { $(this).remove(); });
            $.each(obj, function (key, idata) {
                $("#" + Select2Id).append($('<option/>', {
                    value: idata.CodigoId,
                    text: idata.Descripcion
                }));
            });
            //$("#" + Select2Id).val(value).trigger("change");
        },
        error: function (result) { alertify.alert("Advertencia", "Error llenado: #" + Select2Id); }
    });
}

function RegistrarAcont(tab) {
    if (tab === "frmAcontecimiento") {
        alertify.defaults.theme.ok = "success button";
        alertify.defaults.theme.cancel = "danger button";

        alertify.confirm().destroy();
        var modal = alertify.confirm()
                .setHeader('<h4><span class="icon mif-list2 bg-white mif-ani-float mif-ani-slow"></span>Registrar Actuaciones</h4>')
                .setContent('<div class="flex-grid"><div class="row cell-auto-size"><div class="cell padding10 no-padding-bottom"><table id="dtAcontecimientosTipos" class="hovered cell-border" width="100%"><thead><tr><th class="order-column">Tipo</th><th class="order-column">Acontecimiento</th></tr></thead></table></div></div><div class="row cell-auto-size"><div class="cell padding10 no-padding-bottom">Descripción:<div class="input-control textarea full-size"><textarea id="txtTipoAcontecimientoDescripcion" placeholder="Descripción Opcional"></textarea></div></div></div></div>')
                .set({ 'startMaximized': false, 'maximizable': false, 'resizable': true, 'labels': { ok: '< Continuar >', cancel: '< Salir >' } })
                .resizeTo(800, 550)
                .set('oncancel', function (closeEvent) { })
                .set('onok', function (closeEvent) {
                    var root = this.elements.root;
                    var Objetdt = $(root).find("#dtAcontecimientosTipos");
                    var dtAcontecimientosTipos = Objetdt.DataTable();
                    var dataObject = dtAcontecimientosTipos.row(".selected").data();
                    var Descripcion = $(root).find("#txtTipoAcontecimientoDescripcion");
                    var CampoFiltro = "EXPEDIENTEDEFENSORIA";
                    var UsuarioId = $("#UsuarioId").text();
                    var DatosUsuario = JSON.parse($("#sUsuario").val());
                    var EntidadId = DatosUsuario.EntidadId;
                    var AsuntoNicarao = $("#txtAsuntoNoAsunto").val();
                    var AsuntoDefensoria = $("#txtAsuntoContenedoraId").val();
                    var Fecha = $.formatDateTime('dd/mm/yy', new Date());

                    if (dataObject === undefined) {
                        alertify.alert().destroy();
                        alertify.alert("Advertencia", "Debe seleccionar un registro de la tabla");

                        return false;
                    } else {
                        var TipoAcontecimientoId = dataObject["TipoAcontecimiento"];

                        var JsonActuacion = '[{ "ASUNTO": "' + AsuntoDefensoria.replace(/-/g, "") + '", "ORDEN": ' + parseInt(1) + ', "ORDEN_ASOCIACION": "", "ORGANO": "' + $("#ddlAsuntoOrgano").val() + '", "FECHA": "' + ConvertFecha("01/01/1990") + '", "DESCRIPCION": "' + Descripcion.val() + '", "ACONTECIMIENTO_TIPO": "' + TipoAcontecimientoId + '", "ACONTECIMIENTO_NUMERO": "", "PROCEDIMIENTO_TIPO": "", "FECHA_SISTEMA": "", "USUARIO": "' + UsuarioId + '", "ORGANO_USUARIO": "' + EntidadId + '", "ESTADO": "ACT", "FECHA_ESTADO": "", "FOLIOS_NUM": "", "FOLIO_INICIAL": "", "PIEZA": "", "PROCEDIMIENTO_NUMERO": "", "JUEZ": "", "DIARIZADO": "", "FECHA_DIARIZADO": "", "FECHA_CIERRE_DIARIO": "", "FH_ACONTECIMIENTO": "", "IMPRESO": "", "HORA_DIARIZADO": "", "SECRETARIO": "", "ASUNTO_ACUMULADO": "", "ID_IMPRESION": "", "PONENCIA": "", "USUARIO_MODIFICA": "", "ORGANO_USUARIO_INICIAL": "" }]'

                        db.ACONTECIMIENTOS.clear();
                        db.transaction("rw", db.ACONTECIMIENTOS, function () {
                            db.ACONTECIMIENTOS.bulkAdd(JSON.parse(JsonActuacion));
                        }).then(function () {
                            //Buscamos si el tipo de acontecimiento corresponde a una entrevista
                            $.get(obtenerURL("/Nicarao/CargarAsignacionEntrevista"), { TipoAcontecimientoId: TipoAcontecimientoId })
                                .done(function (resultado) {
                                    if (resultado === "True") { CargarEntrevistaDefensor(); }
                                    else { ObtenerFormato(TipoAcontecimientoId, modal); }
                                })
                                .fail(function (jqxhr, textStatus, error) {
                                    var err = textStatus + ", " + error;
                                    alertify.alert("Advertencia", err);
                                });

                            return true;
                        });
                    }
                })
                .show();

        $("#dtAcontecimientosTipos").DataTable({
            ajax: {
                url: "/Nicarao/RegitrarActuacion",
                dataSrc: ""
            },
            columns: [{ data: "TipoAcontecimiento" }, { data: "Acontecimiento" }],
            columnDefs: [{ width: "15%", targets: 0 }],
            language: { url: "/Json/Spanish.json", select: { "rows": { "_": "Ha seleccionado %d filas" } } },
            lengthMenu: [5], bLengthChange: false, info: false, searching: true,
            scrollY: 160, scrollCollapse: true, paging: false, select: 'single'
        });
    }
}

function ObtenerFormato(TipoAcontecimientoId) {
    $.ajax({
        url: "/Nicarao/ObtenerFormato",
        type: 'get',
        dataType: 'json',
        data: { "TipoAcontecimientoId": TipoAcontecimientoId },
    }).done(function (data) {
        var ObjectData = JSON.parse(data);
        
        if (ObjectData.Retorno === "0") {
            alertify.alert().destroy();
            alertify.alert("Advertencia", ObjectData.Mensaje);
        } else {
            dbDF.Formato.clear();
            dbDF.Formato.add(ObjectData.objData).then(function () {
                $("#frmDocumento").parents("li").removeClass("disabled");
                moverTab("#frmDocumentoDetalle");
                $("#btnGuardarAsunto").addClass("no-visible");
                $("#btnAcontecimientoRegistrar").addClass("no-visible");

                alertify.confirm().close();

                CargarDocumento(JSON.stringify(ObjectData.objData));
            });
        }
    });
}

function CargarDocumento(strData) {
    $.ajax({
        url: "/Nicarao/CargarDocumento",
        type: 'post',
        dataType: 'html',
        data: { "strData": strData },
        beforeSend: function () { showalertify(); },
        complete: function () { closealertify(); }
    }).done(function (data) {
        $(".frame").empty();
        $("#frmDocumentoDetalle").empty().append(data);
    });
}

function ObtenerDataAsunto(SedeId) {
    var tabActivo = $('ul[class="tabs"] li[class="active"] a').attr('id');
    var UsuarioAccion = $("#UsuarioId").text();
    var DatosUsuario = JSON.parse($("#sUsuario").val());
    var EntidadUsuarioId = DatosUsuario.EntidadId;
    var AsuntoDefensoria = $("#txtAsuntoContenedoraId").val();
    var strData = { VALIDACION: $("#txtBanderaAccion").val(), ASUNTOS: "", jsAsuntodes: "", INTERVINIENTES: "", SOLICITUD: "", IPARTICIPACION: "" };

    if (tabActivo === "frmAsunto") {
        alertify.alert().destroy();
        alertify.alert("Advertencia", "Dede de Guardar al menos un Interviniente")
    } else if (tabActivo === "frmInterviniente") {
        db.transaction("rw", db.ASUNTOS, db.INTERVINIENTES, db.jsAsuntodes, function () {
            db.ASUNTOS.toArray().then(function (a) { strData.ASUNTOS = JSON.stringify(a[0]); });
            db.jsAsuntodes.toArray().then(function (j) { strData.jsAsuntodes = JSON.stringify(j[0]) });
            db.INTERVINIENTES.toArray().then(function (i) { strData.INTERVINIENTES = JSON.stringify(i); });
            dbDF.SOLICITUD.toArray().then(function (s) { strData.SOLICITUD = (JSON.stringify(s) === "[]" ? 0 : parseInt(s[0]["SolicitudId"])); });
            dbDF.IntervinienteParticipacion.toArray().then(function (ip) { strData.IPARTICIPACION = JSON.stringify(ip); });
        }).then(function () {
            AgregarAsunto(SedeId, AsuntoDefensoria, UsuarioAccion, EntidadUsuarioId, strData);
        });
    } else if (tabActivo === "frmAcontecimiento") {
        alertify.alert().destroy();
        alertify.alert("Advertencia", "Debe guardar los datos")
    }
}

function AgregarAsunto(SedeId, AsuntoDefensoria, UsuarioAccion, EntidadUsuarioId, strData)
{
    $.ajax({
        url: "/Nicarao/GuardarAsuntoDefensoria",
        type: 'post',
        dataType: 'json',
        data: {
            "SedeId": SedeId, "AsuntoDefensoria": AsuntoDefensoria, "UsuarioAccion": UsuarioAccion, "EntidadUsuarioId": EntidadUsuarioId,
            "SolicitudId": parseInt(strData.SOLICITUD), "strAsunto": strData.ASUNTOS, "strInterviniente": strData.INTERVINIENTES, 
            "strParticipacion": strData.IPARTICIPACION, "strAsuntoDes": strData.jsAsuntodes, "Bandera": strData.VALIDACION
        },
        beforeSend: function () { showalertify(); },
        //complete: function () { closealertify(); }
    }).done(function (result) {
        var resultdata = JSON.parse(result);

        if (resultdata.Retorno === "0") {
            ObtenerInterviniente(AsuntoDefensoria, strData.VALIDACION, resultdata.Mensaje);
        } else
        {
            alertify.alert("Advertencia", resultdata.Mensaje).set('basic', false);
        }
    });
}

function ObtenerInterviniente(AsuntoDefensoria, BanderaId, Mensaje)
{
    $.getJSON("/Nicarao/TempAsuntoObtener?AsuntoDefensoria=" + AsuntoDefensoria).done(function (strData) {
        db.INTERVINIENTES.clear();
        db.jsAsuntodes.clear();

        var objData = JSON.parse(strData);
        var objInterviniente = objData.INTERVINIENTES;
        var OrdenId = [];

        $.each(objInterviniente, function (i, item) {
            objInterviniente[i]["ORDEN"] = parseInt(objInterviniente[i]["ORDEN"]);
            OrdenId.push(parseInt(objInterviniente[i]["ORDEN"]));
        });

        db.transaction('rw', db.INTERVINIENTES, db.jsAsuntodes, function () {
            if (jQuery.isEmptyObject(objData) === false) {
                db.jsAsuntodes.add({ "Asunto": AsuntoDefensoria, "Orden": OrdenId });
                db.INTERVINIENTES.bulkAdd(objInterviniente);
            }
        }).then(function () {
            if (BanderaId === "insert") {
                $("#frmAcontecimiento").click();
                $("#txtBanderaAccion").val("update");
            } else
            {
                db.INTERVINIENTES.toArray().then(function (inte) {
                    $('#dtIntDetalle').dataTable().fnDestroy();
                    dtIntDetalleCargar(JSON.stringify(inte));
                });
            }
        });
    });
}
