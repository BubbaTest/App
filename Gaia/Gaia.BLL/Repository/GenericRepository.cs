using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Data;

using System.Linq.Expressions;
using Gaia.BLL.Interface;
using LinqKit;

using Gaia.BLL.Model;


//using System.Web;
//using Newtonsoft.Json;
//using Newtonsoft.Json.Linq;
//using System.Text;
//using System.Drawing;
using System.IO;
using Microsoft.SqlServer;


namespace Gaia.BLL.Repository
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        protected DbSet<TEntity> DbSet;
        public readonly DbContext _dbContext;
        public Type _dbContext2;
        
        //***Constructor generado por el asistente
        //////private GaiaDbContext gaiaDbContext;

        public GenericRepository(DbContext dbContext)
        {
            _dbContext = dbContext;
            DbSet = _dbContext.Set<TEntity>();
        }

        public GenericRepository(Type dbContext)
        {
            this._dbContext2 = dbContext;
        }
        
        T UnProxy<T>(DbContext context, T proxyObject) where T : class
        {
            var proxyCreationEnabled = context.Configuration.ProxyCreationEnabled;
            try
            {
                context.Configuration.ProxyCreationEnabled = false;
                T poco = context.Entry(proxyObject).CurrentValues.ToObject() as T;
                return poco;
            }
            finally
            {
                context.Configuration.ProxyCreationEnabled = proxyCreationEnabled;
            }
        }
        //***Constructor generado por el asistente
        ////public GenericRepository(GaiaDbContext gaiaDbContext)
        ////{
        ////    // TODO: Complete member initialization
        ////    this.gaiaDbContext = gaiaDbContext;
        ////}

        public TEntity SelectByID(object id)
        {
            return DbSet.Find(id);
            //throw new NotImplementedException();
        }

        public IQueryable<TEntity> SearchFor(System.Linq.Expressions.Expression<Func<TEntity, bool>> predicate)
        {
             //DbSet.Where(predicate);
            return DbSet.Where(predicate);
            //throw new NotImplementedException();
        }

        public IQueryable<TEntity> SelectAll()
        {
            return DbSet;
            //throw new NotImplementedException();
        }
        
        public IQueryable<TEntity> ExecStoreProcedure(string sql, object[] parameters)
        {
            if (!sql.Contains("EXEC") || !sql.Contains("EXECUTE")) { sql = "EXECUTE " + sql; }
            _dbContext.Database.CommandTimeout = 0;
            return _dbContext.Database.SqlQuery<TEntity>(sql, parameters).AsQueryable();
        }
        
        public object ExecuteScalar<TEntity>(string sql, object[] parameters)
        {
            if (!sql.Contains("EXEC") || !sql.Contains("EXECUTE")) { sql = "EXECUTE " + sql; }
            //var value = _dbContext.Database.SqlQuery<TEntity>(sql, parameters).FirstOrDefault();
            var value = this.ExecuteStoreProcedure(sql, parameters).FirstOrDefault();

            object parsedValue = default(TEntity);
            try
            {
                parsedValue = Convert.ChangeType(value, typeof(TEntity));
            }
            catch (InvalidCastException)
            {
                parsedValue = null;
            }
            catch (ArgumentException)
            {
                parsedValue = null;
            }
            return (TEntity)parsedValue;
        }

        public object ExecuteFunction<TEntity>(string sql)
        {
            var value = _dbContext.Database.SqlQuery<TEntity>(sql).FirstOrDefault();

            object parsedValue = default(TEntity);
            try
            {
                parsedValue = Convert.ChangeType(value, typeof(TEntity));
            }
            catch (InvalidCastException)
            {
                parsedValue = null;
            }
            catch (ArgumentException)
            {
                parsedValue = null;
            }
            return (TEntity)parsedValue;
        }

        public void Update(TEntity entity)
        {
            //_dbContext.Entry(entity).State = System.Data.EntityState.Modified;
            _dbContext.Entry(entity).State = System.Data.Entity.EntityState.Modified;
            Save();
            //_dbContext.SaveChanges();
            //throw new NotImplementedException();
        }

        public void Update(TEntity entity, params Expression<Func<TEntity, object>>[] modifiedProperties)
        {
            DbSet.Attach(entity);
            _dbContext.Configuration.ValidateOnSaveEnabled = false;

            //foreach (var property in modifiedProperties)
            //{

            //    //System.Reflection.PropertyInfo prop = property;
            //    string propertyName = ((MemberExpression)property.Body).Member.Name; //GetPropertyName<property>(property); //GeneralExtensions.GetPropertyName<TEntity, object>(property);
            //    _dbContext.Entry<TEntity>(entity).Property(propertyName).IsModified = true;
            //}
            foreach (var property in modifiedProperties)
            {
                string propertyName = "";
                Expression bodyExpression = property.Body;
                if (bodyExpression.NodeType == ExpressionType.Convert && bodyExpression is UnaryExpression)
                {
                    Expression operand = ((UnaryExpression)property.Body).Operand;
                    propertyName = ((MemberExpression)operand).Member.Name;
                }
                else { propertyName = ((MemberExpression)property.Body).Member.Name; }

                _dbContext.Entry<TEntity>(entity).Property(propertyName).IsModified = true;
            }

            _dbContext.SaveChanges();
            _dbContext.Configuration.ValidateOnSaveEnabled = true;
        }

        public void Insert(TEntity entity)
        {
            DbSet.Add(entity);
            Save();
            //throw new NotImplementedException();
        }

        public void Insert(ICollection<TEntity> entity)
        {
            DbSet.AddRange(entity);
            Save();
            //throw new NotImplementedException();
        }

        public void Delete(TEntity entity)
        {
            DbSet.Remove(entity);
            Save();
            //throw new NotImplementedException();
        }

        public void Save()
        {
            _dbContext.SaveChanges();
        }

        public void Dispose()
        {
            _dbContext.Dispose();
        }

        //serverSide Datatable
        public IQueryable<TEntity> fnServerSide(string searchValue, string searchColum, string orderBy, int NPagina, int NRegTotal, out int filteredResultsCount, out int totalResultsCount)
        {
            var tabla = typeof(TEntity);
            var whereClause = PredicateBuilder.True<TEntity>();
            if (searchValue != null)
            {
                //var vbArrayss = searchValue.Split(' '.ToCharArray());

                var vbArrays = searchValue.Split(' ').ToList(); //.ConvertAll(x => x.ToLower());
                var sc = searchColum.Split(',').ToList().ConvertAll(x => x);
                var sv = searchValue.Split(',').ToList().ConvertAll(x => x);

                //Si necesitan anexar una consulta especifica, favor realizar otro if similar 
                //if (tabla.Name == "vwDetalleIndiceProtocoloObtenerListado")
                if (tabla.Name == "vwDetalleIndiceProtocoloObtenerListado" || tabla.Name == "vwDetalleIndiceMatrimonioDivorcioObtenerLista")
                {   
                    for (int i = 0; i < sc.Count(); i++)
                    {
                        whereClause = whereClause.And(EqualsPredicate<TEntity>(sc[i], sv[i]));
                    }
                }else if (tabla.Name == "VariableControl")
                {
                    for (int i = 0; i < sc.Count(); i++)
                    {
                        //if (sc[i] != "UsuarioId")
                        //{
                        //    whereClause = whereClause.And(ContainsPredicate<TEntity>(sc[i], sv[i]));
                        //}else
                        //{
                            //whereClause = whereClause.And(EqualsPredicate<TEntity>(sc[i], sv[i]));
                            whereClause = whereClause.And(ContainsPredicate<TEntity>(sc[i], sv[i]));
                            //foreach (var value in vbArrayss)
                            //    whereClause = whereClause.And(ContainsPredicate<TEntity>(sc[i], sv[i]));
                        //}
                    }
                }
                //else if (tabla.Name == "VariableControl")
                //{
                //    for (int i = 0; i < sc.Count(); i++)
                //    {
                //        //if (sc[i] != "UsuarioId")
                //        //{
                //        //    whereClause = whereClause.And(ContainsPredicate<TEntity>(sc[i], sv[i]));
                //        //}else
                //        //{
                //        //whereClause = whereClause.And(EqualsPredicate<TEntity>(sc[i], sv[i]));
                //        whereClause = whereClause.And(ContainsPredicate<TEntity>(sc[i], sv[i]));
                //        //foreach (var value in vbArrayss)
                //        //    whereClause = whereClause.And(ContainsPredicate<TEntity>(sc[i], sv[i]));
                //        //}
                //    }
                //}
                if (tabla.Name == "vwAuditoria")
                {
                    for (int i = 0; i < sc.Count(); i++)
                    {
                        //if (sc[i] != "UsuarioId")
                        //{
                        //    whereClause = whereClause.And(ContainsPredicate<TEntity>(sc[i], sv[i]));
                        //}else
                        //{
                        //whereClause = whereClause.And(EqualsPredicate<TEntity>(sc[i], sv[i]));
                        whereClause = whereClause.And(ContainsPredicate<TEntity>(sc[i], sv[i]));
                        //foreach (var value in vbArrayss)
                        //    whereClause = whereClause.And(ContainsPredicate<TEntity>(sc[i], sv[i]));
                        //}
                    }
                }
                else
                {
                    foreach (var filter in vbArrays)
                    {
                        whereClause = whereClause.And(ContainsPredicate<TEntity>(searchColum, filter));
                    }
                }
            }

            var result = DbSet
                           .AsExpandable()
                           .Where(whereClause)
                           .OrderBy(GetPropertyExpression<TEntity>(orderBy))
                           .Skip(NPagina)
                           .Take(NRegTotal);

            // now just get the count of items (without the skip and take) - eg how many could be returned with filtering
            filteredResultsCount = DbSet.AsExpandable().Where(whereClause).Count();
            totalResultsCount = DbSet.AsExpandable().Where(whereClause).Count();//DbSet.Count();

            return result;
        }

        public Expression<Func<T, bool>> ContainsPredicate<T>(string NColumna, string VBusqueda)
        {
            var parameter = Expression.Parameter(typeof(T), "m");
            var member = Expression.PropertyOrField(parameter, NColumna);
            var body = Expression.Call(member, "Contains", Type.EmptyTypes, Expression.Constant(VBusqueda));

            return Expression.Lambda<Func<T, bool>>(body, parameter);
        }

        public Expression<Func<T, bool>> EqualsPredicate<T>(string NColumna, string VBusqueda)
        {
            var parameter = Expression.Parameter(typeof(T), "m");
            var member = Expression.PropertyOrField(parameter, NColumna);
            var body = Expression.Equal(member, Expression.Constant(VBusqueda));

            return Expression.Lambda<Func<T, bool>>(body, parameter);
        }

        public Expression<Func<T, string>> GetPropertyExpression<T>(string NColumna)
        {
            var paramterExpression = Expression.Parameter(typeof(T), "m");

            return (Expression<Func<T, string>>)Expression.Lambda(Expression.
                PropertyOrField(paramterExpression, NColumna), paramterExpression);
        }
        
        public DataTable ToDataTable<T>(T entity) where T : class
        {
            var properties = typeof(T).GetProperties();
            var table = new DataTable();
            string[] nameSpace = new string[] { "System.Collections.Generic", "CATI.DAL.Model" };
            
            foreach (var property in properties)
            {
                if (!nameSpace.Contains((property.PropertyType).Namespace))
                {
                    System.Diagnostics.Debug.Print((property.PropertyType).Namespace.ToString() + "-" + property.Name.ToString() + "-" + entity.ToString());

                    table.Columns.Add(property.Name, property.PropertyType);
                }
            }
            
            table.Rows.Add(properties.Where(p => !nameSpace.Contains((p.PropertyType).Namespace)).Select(p => p.GetValue(entity, null)).ToArray());

            return table;
        }
        
        private DataTable ListToDatatable<T>(List<T> items) where T : class
        {
            // Instancia del objeto a devolver
            DataTable dataTable = new DataTable();
            
            // Informacin del tipo de datos de los elementos del List
            Type itemsType = typeof(T);
            
            // Recorremos las propiedades para crear las columnas del datatable
            foreach (System.Reflection.PropertyInfo prop in itemsType.GetProperties())
            {
                // Crearmos y agregamos una columna por cada propiedad de la entidad
                DataColumn column = new DataColumn(prop.Name);
                column.DataType = prop.PropertyType;
                dataTable.Columns.Add(column);
            }
            
            int j;
            
            // ahora recorremos la coleccin para guardar los datos en el DataTable
            foreach (T item in items)
            {
                j = 0;
                object[] newRow = new object[dataTable.Columns.Count];
                
                // Volvemos a recorrer las propiedades de cada item para
                // obtener su valor guardarlo en la fila de la tabla
                foreach (System.Reflection.PropertyInfo prop in itemsType.GetProperties())
                {
                    newRow[j] = prop.GetValue(item, null); j++;
                }
                
                dataTable.Rows.Add(newRow);
            }
            
            // Devolver el objeto creado
            return dataTable;
        }

        public HelpersDataTable createColumnsTable<T>(dynamic selectType, bool info, bool paging, bool search, int[] lengthMenu,
                                       string dom, bool lengthChange, List<ButtonsTable> buttons, List<T> ent, List<columnsDefTable> def,
                                       string initComplete, string[,] order, string url) where T : class
        {
            Type itemsType = typeof(T);
            List<columnsTable> list = new List<columnsTable>();

            foreach (System.Reflection.PropertyInfo prop in itemsType.GetProperties())
            {
                //if (prop.CustomAttributes.Any(r => r.AttributeType.Name.Contains("DisplayAttribute")) &&
                //    prop.CustomAttributes.Any(r => r.AttributeType.Name.Contains("JsonPropertyAttribute")))
                //{
                list.Add(new columnsTable
                {
                    data = prop.Name, //prop.CustomAttributes.Where(r => r.AttributeType.Name.Contains("JsonPropertyAttribute")).FirstOrDefault().NamedArguments.FirstOrDefault().TypedValue.Value.ToString(),
                    title = SplitCamelCase(prop.Name) //prop.CustomAttributes.Where(r => r.AttributeType.Name.Contains("DisplayAttribute")).FirstOrDefault().NamedArguments.FirstOrDefault().TypedValue.Value.ToString()
                });
                //}
            }

            var obj = new HelpersDataTable
            { select = selectType, info = info, paging = paging, search = search, lengthMenu = lengthMenu, dom = dom, lengthChange = lengthChange,
                buttons = buttons, columnDefs = def, columns = list, data = ent, InitComplete = initComplete, order = order };

            return obj;
        }

        public static string SplitCamelCase(string input)
        {
            return System.Text.RegularExpressions.Regex.Replace(input, "([A-Z])", " $1", System.Text.RegularExpressions.RegexOptions.Compiled).Trim();
        }

        public IQueryable<TEntity> searchDynamic(List<PredicateWhere> searchColum)
        {
            var whereClause = PredicateBuilder.True<TEntity>();

            if (searchColum != null)
            {
                Expression<Func<TEntity, bool>> wher = PredicateBuilder.True<TEntity>();

                 foreach (var whr in searchColum)
                {
                    if (whr.FiltroTipo == "split" && string.IsNullOrWhiteSpace(whr.Valor) == false)
                    {
                        var splt = whr.Valor.Split(whr.CaraterSplit);

                        for (int i = 0; i < splt.Count(); i++)
                        {
                            if (string.IsNullOrWhiteSpace(splt[i]) == false)
                            {
                                if (i == 0)
                                { whereClause = ContainsPredicate<TEntity>(whr.Columna, splt[i]); }
                                else { whereClause = whereClause.And(ContainsPredicate<TEntity>(whr.Columna, splt[i])); }
                            }
                        }
                    }else if (whr.FiltroTipo == "contains")
                    {
                        wher = ContainsPredicate<TEntity>(whr.Columna, whr.Valor);

                        if (whr.OperadorTipo == "or")
                        { whereClause = whereClause.Or(wher); }
                        else
                        { whereClause = whereClause.And(wher); }
                    }else
                    {
                        wher = EqualsPredicate<TEntity>(whr.Columna, whr.Valor);

                        if (whr.OperadorTipo == "or")
                        { whereClause = whereClause.Or(wher); }
                        else
                        { whereClause = whereClause.And(wher); }
                    }
                }
            }

            var result = DbSet.AsExpandable().Where(whereClause);
            return result;
        }


        //public Expression<Func<T, bool>> ContainsPredicate<T>(string NColumna, string VBusqueda)
        //{
        //    var vbArray = VBusqueda.Split(' ').ToList().ConvertAll(x => x.ToLower());
        //    var methodInfo = typeof(List<string>).GetMethod("Contains", new Type[] { typeof(string) });
        //    var list = Expression.Constant(vbArray);

        //    var parameter = Expression.Parameter(typeof(T), "m");
        //    var member = Expression.Property(parameter, NColumna);
        //    var body = Expression.Call(list, methodInfo, member);

        //    return Expression.Lambda<Func<T, bool>>(body, parameter);
        //}

        public IList<TEntity> ExecuteStoreProcedure(string sql, object[] parameters, string cnn)
        {
            try
            {
                IList<TEntity> result = null;
                using (var db = (DbContext)Activator.CreateInstance(_dbContext2, new object[] { cnn }))
                {
                    try
                    {
                        using (var transaction = db.Database.BeginTransaction())
                        {
                            try
                            {
                                if (!sql.Contains("EXEC") || !sql.Contains("EXECUTE")) { sql = "EXECUTE " + sql; }
                                result = db.Database.SqlQuery<TEntity>(sql, parameters).ToList();
                                transaction.Commit();
                            }
                            catch (Exception ex) { transaction.Rollback(); }
                            finally { transaction.Dispose(); }
                        }
                    }
                    catch (Exception ex) { if (db.Database.CurrentTransaction != null) { db.Database.CurrentTransaction.Rollback(); } db.Dispose(); throw ex; }
                    finally { db.Dispose(); }
                }
                return result;
            }
            catch (Exception ex) { throw ex; }
        }

        public IList<TEntity> ExecuteStoreProcedure(string sql, object[] parameters)
        {
            try
            {
                IList<TEntity> result = null;
                using (var db = (DbContext)Activator.CreateInstance(_dbContext2))
                {
                    try
                    {
                        using (var transaction = db.Database.BeginTransaction())
                        {
                            try
                            {
                                if (!sql.Contains("EXEC") || !sql.Contains("EXECUTE")) { sql = "EXECUTE " + sql; }
                                
                                result = db.Database.SqlQuery<TEntity>(sql, parameters).ToList();
                                transaction.Commit();
                                
                            }
                            catch (Exception ex) { transaction.Rollback(); }
                            finally { transaction.Dispose(); }
                        }
                    }
                    catch (Exception ex) { if (db.Database.CurrentTransaction != null) { db.Database.CurrentTransaction.Rollback(); } db.Dispose(); throw ex; }
                    finally { db.Dispose(); }
                }
                return result;
            }
            catch (Exception ex) { throw ex; }
        }

        public IList<TEntity> ExecuteStoredProcedureWithTimeOut(string sql, object[] parameters,int seconds)
        {
            try
            {
                IList<TEntity> result = null;
                using (var db = (DbContext)Activator.CreateInstance(_dbContext2))
                {
                    try
                    {
                        using (var transaction = db.Database.BeginTransaction())
                        {
                            try
                            {
                                if (!sql.Contains("EXEC") || !sql.Contains("EXECUTE")) { sql = "EXECUTE " + sql; }
                                db.Database.CommandTimeout = seconds;
                                result = db.Database.SqlQuery<TEntity>(sql, parameters).ToList();
                                transaction.Commit();

                            }
                            catch (Exception ex) { transaction.Rollback(); }
                            finally { transaction.Dispose(); }
                        }
                    }
                    catch (Exception ex) { if (db.Database.CurrentTransaction != null) { db.Database.CurrentTransaction.Rollback(); } db.Dispose(); throw ex; }
                    finally { db.Dispose(); }
                }
                return result;
            }
            catch (Exception ex) { throw ex; }
        }


        public IList<TEntity> ExecuteStoreProcedure2(string sql, object[] parameters)
        {
            try
            {
                IList<TEntity> result = null;
                using (var db = (DbContext)Activator.CreateInstance(_dbContext2))
                {
                    try
                    {
                        using (var transaction = db.Database.BeginTransaction())
                        {
                            try
                            {
                                if (!sql.Contains("EXEC") || !sql.Contains("EXECUTE")) { sql = "EXECUTE " + sql; }
                                result = db.Database.SqlQuery<TEntity>(sql, parameters).ToList();
                                transaction.Rollback();

                            }
                            catch (Exception ex) { transaction.Rollback(); }
                            finally { transaction.Dispose(); }
                        }
                    }
                    catch (Exception ex) { if (db.Database.CurrentTransaction != null) { db.Database.CurrentTransaction.Rollback(); } db.Dispose(); throw ex; }
                    finally { db.Dispose(); }
                }
                return result;
            }
            catch (Exception ex) { throw ex; }
        }

        public void ExecuteNonQuery(string sql, object[] parameters)
        {
            try
            {
                //IList<TEntity> result = null;
                using (var db = (DbContext)Activator.CreateInstance(_dbContext2))
                {
                    try
                    {
                        using (var transaction = db.Database.BeginTransaction())
                        {
                            try
                            {
                                if (!sql.Contains("EXEC") || !sql.Contains("EXECUTE")) { sql = "EXECUTE " + sql; }
                                var result = db.Database.SqlQuery<dynamic>(sql, parameters).ToList();
                                transaction.Commit();
                            }
                            catch (Exception ex) { transaction.Rollback(); }
                            finally { transaction.Dispose(); }
                        }
                    }
                    catch (Exception ex) { if (db.Database.CurrentTransaction != null) { db.Database.CurrentTransaction.Rollback(); } db.Dispose(); throw ex; }
                    finally { db.Dispose(); }
                }
                //return result;
            }
            catch (Exception ex) { throw ex; }
        }

        public void ExecuteNonQuery(string sql, object[] parameters, string cnn)
        {
            try
            {
                //IList<TEntity> result = null;
                using (var db = (DbContext)Activator.CreateInstance(_dbContext2, new object[] { cnn }))
                {
                    try
                    {
                        using (var transaction = db.Database.BeginTransaction())
                        {
                            try
                            {
                                if (!sql.Contains("EXEC") || !sql.Contains("EXECUTE")) { sql = "EXECUTE " + sql; }
                                var result = db.Database.SqlQuery<dynamic>(sql, parameters).ToList();
                                transaction.Commit();
                            }
                            catch (Exception ex) { transaction.Rollback(); }
                            finally { transaction.Dispose(); }
                        }
                    }
                    catch (Exception ex) { if (db.Database.CurrentTransaction != null) { db.Database.CurrentTransaction.Rollback(); } db.Dispose(); throw ex; }
                    finally { db.Dispose(); }
                }
                //return result;
            }
            catch (Exception ex) { throw ex; }
        }
    }
}
