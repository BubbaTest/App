<!DOCTYPE html>
<html>
    <head>
        <title>Sistema de Información Estadístico Integral del Poder Judicial</title>

        <link rel="shortcut icon" href="favicon.ico" type="image/x-icon">
        <link rel="icon" href="{{ asset('favicon.ico') }}" type="image/x-icon">

        <link href="{{ asset('css/metro.css') }}" rel="stylesheet">
        <link href="{{ asset('css/metro-icons.css') }}" rel="stylesheet">
        <link href="{{ asset('css/metro-responsive.css') }}" rel="stylesheet">

        <script src="{{ asset('js/jquery.js') }}"></script>
        <script src="{{ asset('js/metro.js') }}"></script>

        <style>

        .login-form {
        width: 25rem;
        height: 18.75rem;
        /*position: fixed;*/
        /*top: 50%;
        margin-top: -9.375rem;
        left: 50%;*/
        /*margin-left: -12.5rem;*/
        background-color: #ffffff;
        opacity: 0;
        -webkit-transform: scale(.8);
        transform: scale(.8);
        }
        .encabezado {

        background-image: url("{{ asset('img/bg_header.png') }}");
        background-repeat: repeat-y;
        }
        img {
        display: block;
        margin-left: auto;
        margin-right: auto;
        }
        </style>
        <script>
        $(function () {
        var form = $(".login-form");
        form.css({
        opacity: 1,
        "-webkit-transform": "scale(1)",
        "transform": "scale(1)",
        "-webkit-transition": ".5s",
        "transition": ".5s"
        });
        });
        </script>
    </head>
    <body style="background-image: url('{{ asset('img/bg_header.png') }}'); background-repeat: repeat">
        <div class="flex-grid">
            <div class="row padding20" >
                <div class="place-left">
                    <img src="{{ asset('img/logo_poderjudicial.png') }}">
                </div>
                <div class="place-right fg-white">
                    <h1>Sistema de Información Estadístico Integral del Poder Judicial</h1>
                    {{-- <p><h4>Aplicación Informática que permite obtener las estadísticas e indicadores de los órganos judiciales, auxiliares, de apoyo y administrativos.</h4></p> --}}
                </div>
            </div>
            <div class="row flex-just-sb padding20 " >
                <div class="cell auto-size">
                    <div class="carousel" data-role="carousel" data-height="100%" data-control-next="<span class='mif-chevron-right'></span>"
                    data-control-prev="<span class='mif-chevron-left'></span>" style="width: 100%; height: 100%;">
{{--                     <div class="slide align-center fg-yellow">
                        <h3>
                        <p>Definir los criterios de validación Legal y Lógico-Estadístico para los Reportes diseñados por las direcciones asignadas a esta labor con la participación de las áreas usuarias (judiciales, auxiliares, de apoyo y administrativas).</p>
                        </h3>
                        <img src="./img/InstrumentosMedicionEstadisticos.png" alt="">
                    </div>
                    <div class="slide align-center fg-yellow">
                        <h3>
                        <p>Proponer al Consejo un Proyecto de Reglamento sobre el funcionamiento del Sistema de Información de Estadísticas e Indicadores.</p>
                        </h3>
                        <img src="./img/PropuestaProyecto.png" alt="">
                    </div>
                    <div class="slide align-center fg-yellow">
                        <h3>
                        <p>Objetivos del Módulo de Órganos Judiciales</p>
                        <p>Diseñar y crear reportes estadísticos e indicadores del ámbito Judicial</p>
                        </h3>
                        <img src="./img/ReportesNicarao.png" alt="">
                    </div> --}}
                </div>
            </div>
            <div class="cell padding20">
                <div class="login-form padding20 block-shadow">
                    <form method="POST" action="{{ url('/login') }}">
                        <h1 class="text-light">Inicio de sesión</h1>
                        <hr class="thin" />
                        <br />
                        <div class="input-control text full-size" data-role="input">
                            <label for="usuario">Usuario:</label>
                            <input type="text" name="UsuarioId" id="user_login" value="{{ old('name') }}" required="">
                        </div>
                        <br />
                        <br />
                        <div class="input-control password full-size" data-role="input">
                            <label for="password">Contraseña:</label>
                            <input type="password" name="password" id="user_password" required="">
                        </div>
                        <br /> @if($errors->any())
                        <div class="fg-red">
                            <b>{{ $errors->first('personalizado') }}</b>
                        </div>
                        @endif
                        <br />
                        <div class="form-actions">
                            <button type="submit" class="button primary">Ingresar</button>
                        </div>
                        <input type="hidden" name="_token" value="{{ csrf_token() }}">
                    </form>
                </div>
            </div>
        </div>
    </div>
</div>

</br></br></br></br></br>

<div id="count-up" class="align-center">
    <div style="float:center; width:98%; color:#FFFFFF">
        Dirección: Km 7 ½ Carretera Norte · Código Postal: 11025.  Teléfonos: ++505 2233 2128 / ++505 2233 0004 / ++505 22330003 -
        Corte Suprema de Justicia  -         República de Nicaragua · América Central <br>
        Todos los Derechos Reservados 2005 - 2017 ®.
    </div>
</div>

</body>
</html>
