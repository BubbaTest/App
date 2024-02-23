using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Gaia.BLL.Repository
{
    public static class ReflectionDatasetEntity
    {
        public static IList<T> ToListReflection<T>(DataTable table) where T : new()
        {
            List<T> result = new List<T>();
            IList<PropertyInfo> properties = typeof(T).GetProperties().ToList();

            foreach (var row in table.Rows)
            {
                result.Add(CreateItemFromRow<T>((DataRow)row, properties));
            }

            return (IList<T>)result;
        }

        private static T CreateItemFromRow<T>(DataRow row, IList<PropertyInfo> properties) where T : new()
        {
            T item = new T();

            foreach (var property in properties)
            {
                if (property.PropertyType == typeof(DayOfWeek))
                {
                    DayOfWeek day = (DayOfWeek)Enum.Parse(typeof(DayOfWeek), row[property.Name].ToString());
                    property.SetValue(item, day, null);
                }
                else
                {
                    if (row.Table.Columns[property.Name] != null)
                    {
                        if (row[property.Name] == DBNull.Value)
                            property.SetValue(item, null, null);
                        else
                            property.SetValue(item, row[property.Name], null);
                    }
                }
            }

            return item;
        }
    }
}
