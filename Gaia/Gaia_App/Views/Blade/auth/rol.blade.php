<!DOCTYPE html>
<html>
<head>
    <title>Sistema de Información Estadístico Integral del Poder Judicial</title>

    <link rel="shortcut icon" href="favicon.ico" type="image/x-icon">
    <link rel="icon" href="{{ asset('favicon.ico') }}" type="image/x-icon">

    <link rel="stylesheet" href="{{ asset('css/select2.css') }}">
    <link rel="stylesheet" href="{{ asset('css/alertify.css') }}">
    <link rel="stylesheet" href="{{ asset('css/metro.css') }}">
    <link rel="stylesheet" href="{{ asset('css/metro-icons.css') }}">
    <link rel="stylesheet" href="{{ asset('css/metro-responsive.css') }}">
    <link rel="stylesheet" href="{{ asset('css/metro-rtl.css') }}">
    <link rel="stylesheet" href="{{ asset('css/metro-schemes.css') }}">

    <script src="{{ asset('js/jquery.js') }}"></script>
    <script src="{{ asset('js/select2.js') }}"></script>
    <script src="{{ asset('js/select2es.js') }}"></script>
    <script src="{{ asset('js/alertify.js') }}"></script>
    <script src="{{ asset('js/main.js') }}"></script>

    <style>
        .select2-container { z-index:2016; }
    </style>

</head>

<body>

<script type="text/javascript">

    // LLamar al modal para seleccionar el Rol
    $.ajax({
        url: 'moRol',
        type: 'GET',
        dataType: 'HTML'
    }).success(function (result) {

        alertify.confirm("").setContent(result).set('closable', false).set('basic', true);

    }).error(function (xhr, ajaxOptions, thrownError) {
        console.log(xhr);
        console.log(thrownError);
    });


    function goToIndex(rol){

        $('#seleccioneRol').empty().append('');
        $("#seleccioneRol").empty().append('<div class="flex-grid padding20"><div class="row flex-just-center">Cargando...</div></div>');

        $.ajax({
            url: 'setRolTemporal/' + rol,
            type: 'GET',
            dataType: 'HTML'
        }).success(function (result) {

            var path_sin_dominio = window.location.pathname;

            path_sin_dominio = path_sin_dominio.replace("/rol","");

            //console.log(path_sin_dominio);

            window.location.href = path_sin_dominio;

        }).error(function (xhr, ajaxOptions, thrownError) {
            console.log(xhr);
            console.log(thrownError);
        })

    }


</script>

</body>
</html>




