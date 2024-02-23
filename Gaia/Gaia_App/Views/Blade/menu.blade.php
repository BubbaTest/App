<?php

ob_start();

echo '
<div class="menugaia">
    <nav>
        <label for="drop-x" class="toggleee">MENU</label>
        <input type="checkbox" id="drop-x" />
        <ul class="menuResponsive">
';  

function recursive($opciones, $parent, $primera)
{

    $has_children = false;

    foreach ($opciones as $key => $opcion) {

        if ($opcion->PadreId == $parent) {

            if ($has_children == false && $parent) {

                if (!$primera) {

                    $has_children = true;

                    echo '<ul>';
                }

                $primera = true;

            }

            echo '<li>';

            if ($opcion->OpcionTipo == 'link') {
                echo '<a href="#" onclick="loadContenido(\'' . $opcion->Mapeo . '\');">' . $opcion->Opcion . '</a>';
            }

            // De EON
            if ($opcion->OpcionTipo == 'linkResumenEjecutivo') {
                echo '<a href="#" onclick="resumenEjecutivo(\'' . $opcion->Mapeo . '\');">' . $opcion->Opcion . '</a>';
            }

            // De EON
            if ($opcion->OpcionTipo == 'link2') {
                echo '<a href="#" onclick="loadContenido2(\'' . $opcion->Mapeo . '\');">' . $opcion->Opcion . '</a>';
            }

            if ($opcion->OpcionTipo == '') {
                echo '
                    <label for="drop-' . $opcion->OpcionId . '" class="toggleee">' . $opcion->Opcion . '</label>
                    <a href="#">' . $opcion->Opcion . '</a>
                    <input type="checkbox" id="drop-' . $opcion->OpcionId . '"/>';

                recursive($opciones, $opcion->OpcionId, false);
            }

            echo "</li>";

        } // Fin if

    } // Fin foreach

    if ($has_children === true && $parent) {
        echo "</ul>";
    }

}

// La función generarMenu() devuelve un arreglo con los menú
recursive(generarMenu(), Session::get('menuraiz'), true);

// Si tiene varios roles mostrar el botón de cambiar rol
if(Auth::User()->getRoles()->count() > 1){
    $btnRoles = '<button onclick="cambiarRol(); return false" class="btnMenu">Cambiar de Rol</button>';
}else{
    $btnRoles = '';
}

// Si está en algún ambiente distinto al de PRODUCCIÓN
$valor_ambiente = Config::get('app.env');
if($valor_ambiente != 'PRODUCCION'){
    $ambiente = '<li id="ambiente"><a href="#">AMBIENTE DE ' . $valor_ambiente . '</a></li>';
}else{
    $ambiente = '';
}

echo '
        </ul>
        <ul id="ulPerfil">
            ' . $ambiente . '
            <li id="liPerfil">
                <label for="drop-perfil" class="toggleee">' . Session::get('UsuarioId') . '</label>
                <a href="#">' . Session::get('UsuarioId') . '</a>
                <input type="checkbox" id="drop-perfil"/>
                <ul>
                    <li id="menuperfil">
                        <table>
                            <tr>
                                <td id="tdFoto">
                                    <img src="img/anonymous.png">
                                </td>
                                <td id="tdBotones">
                                    <div>' . $btnRoles .'
                                        <button onclick="modalPassword(); return false" class="btnMenu">Cambiar Clave</button>
                                        <button onclick="salir(); return false;" class="btnMenu">Salir</button>
                                    </div>
                                </td>
                            </tr>
                        </table>
                        <div id="datosPerfil">
                            <hr>
                            <p><b>' . Session::get('NombreCompleto') . '</b></p>
                            <br>
                            <p><b>Rol:</b><br><i>' . Session::get('rolTemporalDescripcion') . '</i><br></p>
                            <p id="menucorreo"><b>Correo:</b> <br><i>' . Session::get('correo') . '</i></p>
                        </div>
                    </li>
                </ul>
            </li>
        </ul>
    </nav>
</div>
';

echo ob_get_clean();