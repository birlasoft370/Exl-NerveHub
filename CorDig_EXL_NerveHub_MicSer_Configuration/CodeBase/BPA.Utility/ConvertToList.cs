using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BPA.Utility
{
    public static class ConvertDataTableToList
    {
        public static List<T> ConvertToList<T>(this DataTable dt)
        {
            const BindingFlags flags = BindingFlags.Public | BindingFlags.Instance;
            var columnNames = dt.Columns.Cast<DataColumn>()
                .Select(c => c.ColumnName)
                .ToList();
            var objectProperties = typeof(T).GetProperties(flags);
            var targetList = dt.AsEnumerable().Select(dataRow =>
            {
                var instanceOfT = Activator.CreateInstance<T>();

                foreach (var properties in objectProperties.Where(properties => columnNames.Contains(properties.Name) && dataRow[properties.Name] != DBNull.Value))
                {
                    if (properties.PropertyType.Name.Equals("Int32"))
                    {
                        properties.SetValue(instanceOfT, Convert.ToInt32(dataRow[properties.Name]), null);
                    }
                    else
                    {
                        properties.SetValue(instanceOfT, dataRow[properties.Name], null);
                    }
                }
                return instanceOfT;
            }).ToList();

            return targetList;
        }
    }
}
