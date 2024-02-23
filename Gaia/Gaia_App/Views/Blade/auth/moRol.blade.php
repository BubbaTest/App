<div data-role="dialog" id="dgRolPorUsuario" data-close-button="false" data-overlay="true" data-overlay-color="op-dark">
    <div class="window">
        <div class="window-caption">
            <span class="window-caption-icon"><span class="mif-users"></span></span>
            <span class="window-caption-title">Seleccione un rol:</span>
        </div>
        <div class="window-content" style="height: 20%">
            <div id="seleccioneRol" class="listview">

                @foreach (Auth::User()->getRoles() as $rol)

                    <a id="{{ $rol->RolId }}" onclick="goToIndex(this.id); return null">
                        <div class="list">
                            <span class="list-title">{{ $rol->getRolDescripcion()->Descripcion }}</span>
                        </div>
                    </a>
                @endforeach

            </div>
        </div>
    </div>
</div>
