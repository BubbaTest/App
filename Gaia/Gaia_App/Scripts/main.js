function iniciarValoresUsuario() {
    listado = JSON.parse(encabezdo);    
    $.each(listado, function (i, item) {
        $('#txtUsuarioId').prop("disabled", true);
        $("#txtUsuarioId").val(item.UsuarioId);
        $('#txtEmpleadoId').prop("disabled", true);
        $("#ddlProcedencia").val(item.TipoIdentificadorId).trigger("change");        
            var sssUrl = obtenerURL("../Mantenimiento/txtEmpleadoId?EmpleadoId=" + item.Identificador + '&tipo=' + item.TipoIdentificadorId);  //.EmpleadoId
            $("#txtEmpleadoId").load(sssUrl, function (response, status, xhr) {
                if (status === "success") {
                    $("#txtEmpleadoId").val(item.Identificador).trigger("change"); 
                }
            });        
        //$('#txtPassword').prop("disabled", true);
        //$("#txtPassword").val(item.Password);        
        $("#txtCorreo").val(item.Correo);
        if (item.Avatar != null) {
            $("#Avatar").empty();
            var uavatars = obtenerURL("/Mantenimiento/GetAvatars?UsuarioId=" + item.UsuarioId);
            $("#Avatar").load(uavatars, function (response, status, xhr) {
            });        
        }                
        $("#ddlUEstadoId").val(item.EstadoId).trigger("change");        
    });

    $('#btnGuardaUsuario').hide();
    $('#btnActualizarUsuario').show();
    //$('#btnActualizarPassword').show();    

    listado = "";
    encabezdo = "";
};

function iniciarValoresUsuarioSAP(obj) {    
    $.each(obj, function (i, item) {
        $("#hfidUsuario").val(item.id_usuario);
        $("#txtLoginId").val(item.NombreUsuario);
        $('#txtLoginId').prop("disabled", true);
        $("#txtPassword").val(item.Contrasenna);
        $("#txtCorreo").val(item.email);
        $('#txtCorreo').prop("disabled", true);
        $("#txtIdentificadorId").val(item.IdTipoIdentificacion).trigger("change");
        $('#txtIdentificadorId').prop("disabled", true);
        $("#txtIdentificador").val(item.Identificacion);
        $('#txtIdentificador').prop("disabled", true);
        if (item.blnActivo === "1") {
            document.getElementById("activoblnActivo").checked = true;
            document.getElementById("inactivoblnActivo").checked = false;
        }
        else {
            document.getElementById("activoblnActivo").checked = false;
            document.getElementById("inactivoblnActivo").checked = true;
        }
        $("#txtintCodEntidadPJ").val(item.intCodEntidadPJ).trigger("change");
        $('#txtintCodEntidadPJ').prop("disabled", true);
        $("#txtNombre1").val(item.PrimerNombre);  
        $('#txtNombre1').prop("disabled", true);
        $("#txtNombre2").val(item.SegundoNombre);
        $('#txtNombre2').prop("disabled", true);
        $("#txtApellido1").val(item.PrimerApellido);
        $('#txtApellido1').prop("disabled", true);
        $("#txtApellido2").val(item.SegundoApellido);
        $('#txtApellido2').prop("disabled", true);
        if (item.DeBaja === "1") {
            document.getElementById("activoblnDeBaja").checked = true;
            document.getElementById("inactivoblnDeBaja").checked = false;
        }
        else {
            document.getElementById("activoblnDeBaja").checked = false;
            document.getElementById("inactivoblnDeBaja").checked = true;
        }
        if (item.Avatar != null) {
            $("#Avatar").empty();
            var uavatars = obtenerURL("/Mantenimiento/GetAvatarsSap?UsuarioId=" + item.id_usuario);
            $("#Avatar").load(uavatars, function (response, status, xhr) {
            });
        }
    });

    $('#btnGuardaUsuarioSap').hide();
    $('#btnActualizarUsuarioSap').show();

    listado = "";
    encabezdo = "";
}

function iniciarValoresUsuarioGaleno(obj) {    
    $.each(obj, function (i, item) {
        $("#hfidUsuario").val(item.idUsuario);
        
        var sssUrl = obtenerURL("../Mantenimiento/txtEmpleadoIdGaleno?EmpleadoId=" + item.chrCodEmpleado);  
        $("#txtEmpleadoId").load(sssUrl, function (response, status, xhr) {
            if (status === "success") {
                $("#txtEmpleadoId").val(item.chrCodEmpleado).trigger("change");
            }
        });        
        $("#txtchrcodempleado").val(item.chrCodEmpleado);
        $("#txtlogin").val(item.strLogin);
        $("#ddldeliml").val(item.intCodDelegacionIML).trigger("change");
        
        if (item.blnEstado === "1") {
            document.getElementById("activoblnEstado").checked = true;
            document.getElementById("inactivoblnEstado").checked = false;
        }
        else {
            document.getElementById("activoblnEstado").checked = false;
            document.getElementById("inactivoblnEstado").checked = true;
        }
        if (item.Delegaciones === "1") {
            $('#switch-h').prop('checked', true);   
        }
    });

    $('#btnGuardaUsuarioGaleno').hide();
    $('#btnActualizarUsuarioGaleno').show();

    listado = "";
    encabezdo = "";
};


function iniciarValoresPermisoSap(obj) {
    $.each(obj, function (i, item) {
        $("#hfidPermiso").val(item.id_permiso);        
        $("#ddltablarolid").val(item.id_rol).trigger("change");
        $("#ddlcatmenuid").val(item.id_menu).trigger("change");
        if (item.DeBaja === "1") {
            document.getElementById("activoblnEstadoper").checked = true;
            document.getElementById("inactivoblnEstadoper").checked = false;
        }
        else {
            document.getElementById("activoblnEstadoper").checked = false;
            document.getElementById("inactivoblnEstadoper").checked = true;
        }        
        if (item.blnActivo === "1") {
            document.getElementById("activoblnActivoper").checked = true;
            document.getElementById("inactivoblnActivoper").checked = false;
        }
        else {
            document.getElementById("activoblnActivoper").checked = false;
            document.getElementById("inactivoblnActivoper").checked = true;
        }        
    });

    $('#btnGuardaPermisoSap').hide();
    $('#btnActualizarPermisoSap').show();

    listado = "";
    encabezdo = "";
};

function iniciarValoresRolGaleno(obj) {
    $.each(obj, function (i, item) {
        $("#hfidRol").val(item.id_rol);
        $("#txtNombre").val(item.nombre);        
        $("#txtDescripcion").val(item.descripcion);        
        if (item.DeBaja === "1") {
            document.getElementById("activoblnBajaRol").checked = true;
            document.getElementById("inactivoblnBajaRol").checked = false;
        }
        else {
            document.getElementById("activoblnBajaRol").checked = false;
            document.getElementById("inactivoblnBajaRol").checked = true;
        }
        if (item.blnActivo === "1") {
            document.getElementById("activoblnActivoRol").checked = true;
            document.getElementById("inactivoblnActivoRol").checked = false;
        }
        else {
            document.getElementById("activoblnActivoRol").checked = false;
            document.getElementById("inactivoblnActivoRol").checked = true;
        }
        $("#txtFDesde").val(item.FechaInicioRol);        
        if (item.esIndefinido === "1") { $('#switch-h').prop('checked', true); $("#txtFHasta").val(item.FechaFinRol); }
        else { $('#switch-h').prop('checked', false);}        
    });

    $('#btnGuardaRolSap').hide();
    $('#btnActualizarRolSap').show();

    listado = "";
    encabezdo = "";
};

function iniciarValoresAccSap(obj) {
    $.each(obj, function (i, item) {
        $("#hfIdAccion").val(item.IdAccion);
        $("#ddlTipoAccionId").val(item.IdTipoAccion).trigger("change");
        $("#txtDescripcionAcc").val(item.DescpAccion);
        if (item.DeBaja === "1") {
            document.getElementById("inactivoblnDeBajaAcc").checked = true;
            document.getElementById("activoblnDeBajaAcc").checked = false;
        }
        else {
            document.getElementById("inactivoblnDeBajaAcc").checked = false;
            document.getElementById("activoblnDeBajaAcc").checked = true;
        }
        $("#txtPorDefecto").val(item.PorDefecto);
        $("#txtGrupo").val(item.Grupo);
        $("#txtOrden").val(item.Orden);
        if (item.blnActivo === "1") {
            document.getElementById("activoblnActivoAcc").checked = true;
            document.getElementById("inactivoblnActivoAcc").checked = false;
        }
        else {
            document.getElementById("activoblnActivoAcc").checked = false;
            document.getElementById("inactivoblnActivoAcc").checked = true;
        }
    });

    $('#btnGuardaAccionSap').hide();
    $('#btnActualizarAccionSap').show();

    listado = "";
    encabezdo = "";
};

function iniciarValoresMenSap(obj) {    
    $.each(obj, function (i, item) {        
        $("#hfIdMenu").val(item.id_menu);
        $("#ddlModuloId").val(item.IdModulo).trigger("change");
        $("#txtmenupadre").val(item.menu_padre);
        $("#txtcodigomen").val(item.codigo);
        $("#txtnombremen").val(item.nombre);
        $("#txtdescripcionmen").val(item.descripcion);        
        $("#txturl").val(item.url);
        $("#txttipo").val(item.tipo);
        $("#txtorden").val(item.orden);
        $("#txtnivel").val(item.nivel);
        if (item.estado === "1") {
            document.getElementById("activoblnEstado").checked = true;
            document.getElementById("inactivoblnEstado").checked = false;
        }
        else {
            document.getElementById("activoblnEstado").checked = false;
            document.getElementById("inactivoblnEstado").checked = true;
        }
    });

    $('#btnGuardaMenuSap').hide();
    $('#btnActualizarMenuSap').show();

    listado = "";
    encabezdo = "";
};

function iniciarValoresSistema() {
    listado = JSON.parse(encabezdo);
    $.each(listado, function (i, item) {        
        $("#txtSistemaId").val(item.SistemaId);
        $("#txtDescripcion").val(item.Descripcion);
        $("#txtLlaveCifrado").val(item.LlaveCifrado);
        $("#ddlrelalgoritmo").val(item.AlgoritmoCifrado).trigger("change");
        if (item.Accion === true) {
            document.getElementById("activosis").checked = true;
            document.getElementById("inactivosis").checked = false;
        }
        else {
            document.getElementById("activosis").checked = false;
            document.getElementById("inactivosis").checked = true;
        }
    });

    $('#btnGuardaSis').hide();
    $('#btnActualizarSis').show();    

    listado = "";
    encabezdo = "";
};

function iniciarValoresRol() {
    listado = JSON.parse(encabezdo);    

    $.each(listado, function (i, item) {
        $('#txtRolId').prop("disabled", true);
        $("#txtRolId").val(item.RolId);        
        $("#txtDescripcionR").val(item.Descripcion);        
        if (item.Activo === true) {            
            document.getElementById("activorol").checked = true;
            document.getElementById("inactivorol").checked = false;
        }
        else {
            document.getElementById("activorol").checked = false;
            document.getElementById("inactivorol").checked = true;
        }
    });

    $('#btnGuardaRol').hide();
    $('#btnActualizarRol').show();

    listado = "";
    encabezdo = "";
};

function iniciarValoresOpcAso() {
    listado = JSON.parse(encabezdo);    

    $.each(listado, function (i, item) {
        $('#txtOpcionId').prop("disabled", true);
        $("#txtOpcionId").val(item.OpcionId);                
        $("#txtNivel").val(item.Nivel);
        $("#txtPadreId").val(item.PadreId);
        $("#txtOpcion").val(item.Opcion);
        $("#txtOpcionTipo").val(item.OpcionTipo);
        $("#txtMapeo").val(item.Mapeo);
        $("#txtNombreIcono").val(item.NombreIcono);
        $("#txtDesGen").val(item.DescripcionGeneral);
        if (item.Activo === true) {
            document.getElementById("activoopc").checked = true;
            document.getElementById("inactivoopc").checked = false;
        }
        else {
            document.getElementById("activoopc").checked = false;
            document.getElementById("inactivoopc").checked = true;
        }
    });

    $('#btnGuardaOpcAso').hide();
    $('#btnActualizarOpcAso').show();

    listado = "";
    encabezdo = "";
};

function iniciarValoresEntidad() {
    listado = JSON.parse(encabezdo);    
    
    $.each(listado, function (i, item) {
        $('#txtEntidadId').prop("disabled", true);
        $("#txtEntidadId").val(item.EntidadId);
        $("#txtDescripcionE").val(item.Descripcion);
        var Dept = item.CodDepartamento;
        if (Dept != null) {            
            $("#txtCodDepartamento").val(item.CodDepartamento).trigger("change");
            var sssUrl = obtenerURL("/Mantenimiento/txtCodMunicipio?chrCodDepartamento=" + $('#txtCodDepartamento').val());
            $("#txtCodMunicipio").load(sssUrl, function (response, status, xhr) {
                if (status === "success") {
                    $("#txtCodMunicipio").val(item.CodMunicipio).trigger("change");
                }
            })
        }        
        $("#txtDomicilio").val(item.Domicilio);
        $("#txtTelefonos").val(item.Telefonos);
        $("#txtEstado").val(item.Estado);
        $("#txtCircuito").val(item.Circuito);
        $("#txtDescripcionCorta").val(item.DescripcionCorta);
        var Sed = item.SedeJudicial
        if (Sed != null) {
            $("#txtSedeJudicial").val(item.SedeJudicial).trigger("change");
        }
        var Ent = item.intCodEntidadPJ
        if (Ent != null) {
            $("#txtintCodEntidadPJ").val(item.intCodEntidadPJ).trigger("change");;
        }
        var EDI = item.chrCodEdificio
        if (EDI != null) {
            $("#txtchrCodEdificio").val(item.chrCodEdificio).trigger("change");;
        }        
        if (item.Activo === true) {
            document.getElementById("activoent").checked = true;
            document.getElementById("inactivoent").checked = false;
        }
        else {
            document.getElementById("activoent").checked = false;
            document.getElementById("inactivoent").checked = true;
        }
    });

    $('#btnGuardaEntidad').hide();
    $('#btnActualizarEntidad').show();

    listado = "";
    encabezdo = "";
};

function iniciarValoresParametro() {
    listado = JSON.parse(encabezdo);

    $.each(listado, function (i, item) {
        $("#txtParametroId").val(item.ParametroId);
        $("#txtDescripcionParametro").val(item.Descripcion);
        $("#txtValorParametro").val(item.Valor);        
    });

    $('#btnGuardaParametro').hide();
    $('#btnActualizarParametro').show();
    $('#txtParametroId').prop("disabled", true);

    listado = "";
    encabezdo = "";
};

function iniciarValoresReporte() {
    listado = JSON.parse(encabezdo);

    $.each(listado, function (i, item) {
        $("#txtReporteId").val(item.ReporteId);
        $("#txtDescripcionReporte").val(item.Descripcion);        
        $("#txtNombreReporte").val(item.NombreReporte);        
        $("#txtSistemaIdReporte").val(item.SistemaId).trigger("change");
        $("#txtModuloReporte").val(item.Modulo);
        if (item.Activo === true) {
            document.getElementById("activoreporte").checked = true;
            document.getElementById("inactivoreporte").checked = false;
        }
        else {
            document.getElementById("activoreporte").checked = false;
            document.getElementById("inactivoreporte").checked = true;
        }
    });

    $('#btnGuardaReporte').hide();
    $('#btnActualizarReporte').show();
    $('#txtReporteId').prop("disabled", true);

    listado = "";
    encabezdo = "";
};

function iniciarValoresTipoObjeto() {    

    $.each(encabezdo, function (i, item) {
        $("#txtTipoObjetoId").val(item.TipoObjetoId);
        $("#txtDescripcionTipoObjeto").val(item.Descripcion);
        if (item.Activo === true) {
            document.getElementById("activotipobj").checked = true;
            document.getElementById("inactivotipobj").checked = false;
        }
        else {
            document.getElementById("activotipobj").checked = false;
            document.getElementById("inactivotipobj").checked = true;
        }
    });

    $('#btnGuardaTipoObjeto').hide();
    $('#btnActualizarTipoObjeto').show();
    $('#txtTipoObjetoId').prop("disabled", true);
    
    encabezdo = "";
};

function iniciarValoresCampoReporte() {

    $.each(encabezdo, function (i, item) {
        $("#txtCampoReporteId").val(item.CampoReporteId);
        $("#ddlTipoObjetoId").val(item.TipoObjetoId).trigger("change");
        $("#txtPlaceholder").val(item.Placeholder);
        $("#txtTitulo").val(item.Titulo);
        $("#ddlParametroId").val(item.ParametroId).trigger("change");
        if (item.Activo === true) {
            document.getElementById("activocamprep").checked = true;
            document.getElementById("inactivocamprep").checked = false;
        }
        else {
            document.getElementById("activocamprep").checked = false;
            document.getElementById("inactivocamprep").checked = true;
        }
        if (item.IsPrincipal === true) {
            document.getElementById("isprincipalcamrep").checked = true;
            document.getElementById("notisprincipalcamrep").checked = false;
        }
        else {
            document.getElementById("isprincipalcamrep").checked = false;
            document.getElementById("notisprincipalcamrep").checked = true;
        }
    });

    $('#btnGuardaCampoReporte').hide();
    $('#btnActualizarCampoReporte').show();
    $('#txtCampoReporteId').prop("disabled", true);

    encabezdo = "";
};

function iniciarValoresVariableControl() {
    listado = JSON.parse(encabezdo);

    $.each(listado, function (i, item) {
        $("#hfIdVC").val(item.VariableControlId);
        $('#hfIdVC').prop("disabled", true);
        $("#txtdVC").val(item.Descripcion);
        $("#txtcVC").val(item.Cadena);
        var dateString = item.Fecha;        
        if (dateString != null) {            
            $("#txtfVC").val(dateString);
        }
        else {
            $("#txtfVC").val("");
        }
        $("#txtnVC").val(item.Numerico);
        $("#txttrVC").val(item.TablaRel);
        $("#txtcrVC").val(item.CampoRel);
        if (item.Activo === true) {
            document.getElementById("activovr").checked = true;
            document.getElementById("inactivovr").checked = false;
        }
        else {
            document.getElementById("activovr").checked = false;
            document.getElementById("inactivovr").checked = true;
        }
    });

    $('#btnGuardaVariable').hide();
    $('#btnActualizarVariable').show();

    listado = "";
    encabezdo = "";
};

function iniciarValoresMateria() {
    $.each(encabezdo, function (i, item) {
        $("#txtIdMateria").val(item.MateriaId);
        $('#txtIdMateria').prop("disabled", true);
        $("#txtDescripcionMateria").val(item.Descripcion);        
    });

    $('#btnGuardaMateria').hide();
    $('#btnActualizarMateria').show();
        
    encabezdo = "";
};

function formatFechaHoras(dates) {
    var date = new Date(dates);
    var day = date.getDate();
    var twoDigitDay = ((day < 10 ? '0' + day : '' + day));
    var twoDigitMonth = ((date.getMonth() + 1) >= 10) ? (date.getMonth() + 1) : '0' + (date.getMonth() + 1);
    var hours = date.getHours();
    var minutes = date.getMinutes();
    var ampm = hours >= 12 ? 'pm' : 'am';
    hours = hours % 12;
    hours = hours ? hours : 12; // the hour '0' should be '12'
    hours = hours < 10 ? '0' + hours : hours;
    minutes = minutes < 10 ? '0' + minutes : minutes;
    var strTime = hours + ':' + minutes + ' ' + ampm;
    return twoDigitDay + "/" + twoDigitMonth + "/" + date.getFullYear() + "  " + strTime;
};
