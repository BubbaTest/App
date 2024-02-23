using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Gaia.BLL.Model
{
    public class HelpersDataTable
    {
        [Key]
        public bool search { get; set; }
        public dynamic select { get; set; }
        public bool paging { get; set; }
        public int[] lengthMenu { get; set; }
        public bool info { get; set; }
        public string dom { get; set; }
        public bool lengthChange { get; set; }
        public string InitComplete { get; set; }
        public List<ButtonsTable> buttons { get; set; }
        public LenguajeTable language { get { return new LenguajeTable() { sProcessing = "Procesando...",
            sLengthMenu = "Mostrar _MENU_ registros", sZeroRecords = "No se encontraron resultados",
            sEmptyTable = "Ningún dato disponible en esta tabla", sInfo = "Registros _START_ al _END_ de _TOTAL_ registros",
            sInfoEmpty = "Registros del 0 al 0 de 0 registros", sInfoFiltered = "(filtrado de un total de _MAX_ registros)",
            sInfoPostFix = "", sSearch = "Buscar:", sUrl = "", sInfoThousands = ",", sLoadingRecords = "Cargando...",
            oPaginate = new oPaginate() { sFirst = "Primero", sLast = "Último", sNext = "Siguiente", sPrevious = "Anterior" },
            oAria = new oAria() { sSortAscending = ": Activar para ordenar la columna de manera ascendente",
                sSortDescending = ": Activar para ordenar la columna de manera descendente" },
            select = new LenguajeSelectTable() { rows = "%d registro seleccionado" } }; } }
        public List<columnsDefTable> columnDefs { get; set; }
        public List<columnsTable> columns { get; set; }
        public dynamic data { get; set; }
        public string[,] order { get; set; }
    }

    public class columnsDefTable
    {
        [Key]
        public int targets { get; set; }
        public bool orderable { get; set; }
        public bool visible { get; set; }
        public string className { get; set; }
        public string defaultContent { get; set; }
        public bool searchable { get; set; }
        public string width { get; set; }
    }

    public class columnsTable
    {
        [Key]
        public string data { get; set; }
        public string title { get; set; }
    }

    //public class LenguajeTable
    //{
    //    [Key]
    //    public string url { get; set; }
    //    public LenguajeSelectTable select { get; set; }
    //}

    public class LenguajeSelectTable
    {
        [Key]
        public string rows { get; set; }
    }

    public class ButtonsTable
    {
        [Key]
        public string extend { get; set; }
        public string title { get; set; }
        public string text { get; set; }
        public string className { get; set; }
        public List<ButtonsTable> buttons { get; set; }
    }

    public class OrderTable
    {
        [Key]
        public int columns { get; set; }
        public string orderType { get; set; }
    }

    public class LenguajeTable
    {
        [Key]
        public string sProcessing { get; set; }
        public string sLengthMenu { get; set; }
        public string sZeroRecords { get; set; }
        public string sEmptyTable { get; set; }
        public string sInfo { get; set; }
        public string sInfoEmpty { get; set; }
        public string sInfoFiltered { get; set; }
        public string sInfoPostFix { get; set; }
        public string sSearch { get; set; }
        public string sUrl { get; set; }
        public string sInfoThousands { get; set; }
        public string sLoadingRecords { get; set; }
        public oPaginate oPaginate { get; set; }
        public oAria oAria { get; set; }
        public LenguajeSelectTable select { get; set; }
    }

    public class oPaginate
    {
        [Key]
        public string sFirst { get; set; }
        public string sLast { get; set; }
        public string sNext { get; set; }
        public string sPrevious { get; set; }
    }

    public class oAria
    {
        [Key]
        public string sSortAscending { get; set; }
        public string sSortDescending { get; set; }
    }

    //public partial LenguajeTable jsonLenguaje()
    //{
    //    var _obj = new LenguajeTable() { sProcessing = "Procesando...", sLengthMenu = "Mostrar _MENU_ registros", sZeroRecords = "No se encontraron resultados", sEmptyTable = "Ningún dato disponible en esta tabla", sInfo = "Registros _START_ al _END_ de _TOTAL_ registros", sInfoEmpty = "Registros del 0 al 0 de 0 registros", sInfoFiltered = "(filtrado de un total de _MAX_ registros)", sInfoPostFix = "", sSearch = "Buscar:", sUrl = "", sInfoThousands = ",", sLoadingRecords = "Cargando...", oPaginate = new oPaginate() { sFirst = "Primero", sLast = "Último", sNext = "Siguiente", sPrevious = "Anterior" }, oAria = new oAria() { sSortAscending = ": Activar para ordenar la columna de manera ascendente", sSortDescending = ": Activar para ordenar la columna de manera descendente" }, select = new LenguajeSelectTable() { rows = "%d registro seleccionado" } };

    //    return _obj;
    //}
}
