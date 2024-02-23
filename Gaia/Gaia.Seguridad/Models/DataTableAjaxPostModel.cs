using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Gaia.Seguridad.Model
{
    public class DataTableAjaxPostModel
    {
        // properties are not capital due to json mapping
        public int draw { get; set; }
        public int start { get; set; }
        public int length { get; set; }
        public List<Column> columns { get; set; }
        public Search search { get; set; }
        public List<Order> order { get; set; }
    
        public class Column
        {
            public string data { get; set; }
            public string name { get; set; }
            public bool searchable { get; set; }
            public bool orderable { get; set; }
            public Search search { get; set; }
        }

        public class Search
        {
            public string value { get; set; }
            public string regex { get; set; }
        }

        public class Order
        {
            public int column { get; set; }
            public string dir { get; set; }
        }

        //DiceaDbContext Db = new DiceaDbContext();

        //public static IList<vwBusquedaAbogadoNotario> YourCustomSearchFunc(DataTableAjaxPostModel model, out int filteredResultsCount, out int totalResultsCount)
        //{
        //    var searchBy = (model.search != null) ? model.search.value : null;
        //    var take = model.length;
        //    var skip = model.start;

        //    string sortBy = "";
        //    bool sortDir = true;

        //    if (model.order != null)
        //    {
        //        // in this example we just default sort on the 1st column
        //        sortBy = model.columns[model.order[0].column].data;
        //        sortDir = model.order[0].dir.ToLower() == "asc";
        //    }

        //    // search the dbase taking into consideration table sorting and paging
        //    var result = GetDataFromDbase(searchBy, take, skip, sortBy, sortDir, out filteredResultsCount, out totalResultsCount);
        //    if (result == null)
        //    {
        //        // empty collection...
        //        return new List<vwBusquedaAbogadoNotario>();
        //    }

        //    return result;
        //}

        //public static List<vwBusquedaAbogadoNotario> GetDataFromDbase(string searchBy, int take, int skip, string sortBy, bool sortDir, out int filteredResultsCount, out int totalResultsCount)
        //{
        //    // the example datatable used is not supporting multi column ordering
        //    // so we only need get the column order from the first column passed to us.
        //    DiceaDbContext Db = new DiceaDbContext();
        //    var whereClause = BuildDynamicWhereClause(Db, searchBy);

        //    if (String.IsNullOrEmpty(searchBy))
        //    {
        //        // if we have an empty search then just order the results by Id ascending
        //        sortBy = "AbogadoNotarioId";
        //        sortDir = true;
        //    }

        //    var result = Db.vwBusquedaAbogadoNotario
        //                   .AsExpandable()
        //                   .Where(whereClause)
        //                   //.OrderBy(sortBy, sortDir)
        //                   .OrderBy(m => m.AbogadoNotarioId) // have to give a default order when skipping .. so use the PK
        //                   .Skip(skip)
        //                   .Take(take)
        //                   .ToList();

        //    // now just get the count of items (without the skip and take) - eg how many could be returned with filtering
        //    filteredResultsCount = Db.vwBusquedaAbogadoNotario.AsExpandable().Where(whereClause).Count();
        //    totalResultsCount = Db.AbogadoNotarios.Count();

        //    return result;
        //}

        //private static Expression<Func<vwBusquedaAbogadoNotario, bool>> BuildDynamicWhereClause(DiceaDbContext entities, string searchValue)
        //{
        //    // simple method to dynamically plugin a where clause
        //    var predicate = PredicateBuilder.True<vwBusquedaAbogadoNotario>(); // true -where(true) return all
        //    if (String.IsNullOrWhiteSpace(searchValue) == false)
        //    {
        //        // as we only have 2 cols allow the user type in name 'firstname lastname' then use the list to search the first and last name of dbase
        //        var searchTerms = searchValue.Split(' ').ToList().ConvertAll(x => x.ToLower());

        //        predicate = predicate.Or(s => searchTerms.Any(srch => s.Busqueda.ToLower().Contains(srch)));
        //        //predicate = predicate.Or(s => searchTerms.Any(srch => s.Lastname.ToLower().Contains(srch)));
        //    }
        //    return predicate;
        //}
    }
}