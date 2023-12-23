using System.ComponentModel;
using System.Data;
using System.Reflection;

namespace MicUI.Configuration.Helper
{
    public static class UISharedLayer
    {
        public static DataTable ToDataTable<T>(List<T> iList)
        {
            DataTable dataTable = new DataTable();
            PropertyDescriptorCollection propertyDescriptorCollection =
                TypeDescriptor.GetProperties(typeof(T));
            for (int i = 0; i < propertyDescriptorCollection.Count; i++)
            {
                PropertyDescriptor propertyDescriptor = propertyDescriptorCollection[i];
                Type type = propertyDescriptor.PropertyType;

                if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
                    type = Nullable.GetUnderlyingType(type);


                dataTable.Columns.Add(propertyDescriptor.Name, type);
            }
            object[] values = new object[propertyDescriptorCollection.Count];
            foreach (T iListItem in iList)
            {
                for (int i = 0; i < values.Length; i++)
                {
                    values[i] = propertyDescriptorCollection[i].GetValue(iListItem);
                }
                dataTable.Rows.Add(values);
            }
            return dataTable;
        }
        public static DataTable ValidateDate(DataTable Table)
        {
            DataTable dt;
            dt = Table;
            DataTable tempDataTable = Table.Clone();
            int t = 0;
            for (int k = 0; k < dt.Columns.Count; k++)
            {
                int j = 0;
                foreach (DataColumn dc in Table.Columns)

                    if (dc.DataType.Name.ToString() == "DateTime")
                    {
                        DataTable dtCloned = dt.Clone();
                        dtCloned.Columns[k].DataType = typeof(string);
                        foreach (DataRow row in dt.Rows)
                        {
                            dtCloned.ImportRow(row);
                        }
                        dt = dtCloned;
                        dt.AcceptChanges();
                    }
            }
            return dt;
        }

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
