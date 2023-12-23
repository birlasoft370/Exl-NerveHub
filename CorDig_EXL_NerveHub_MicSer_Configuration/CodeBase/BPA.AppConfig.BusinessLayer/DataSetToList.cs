using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BPA.AppConfig.BusinessLayer
{
    static class DataSetToList
    {
        /// <summary>
        /// Converts to.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="datatable">The datatable.</param>
        /// <returns></returns>
        public static List<T> ConvertTo<T>(DataTable datatable) where T : new()
        {
            List<T> Temp = new List<T>();
            try
            {
                List<string> columnsNames = new List<string>();
                foreach (DataColumn DataColumn in datatable.Columns)
                    columnsNames.Add(DataColumn.ColumnName.Replace(" ", "").Replace("-", ""));
                Temp = datatable.AsEnumerable().ToList().ConvertAll<T>(row => getObject<T>(row, columnsNames));
                return Temp;
            }
            catch { return Temp; }
        }
        /// <summary>
        /// Gets the object.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="row">The row.</param>
        /// <param name="columnsName">Name of the columns.</param>
        /// <returns></returns>
        public static T getObject<T>(DataRow row, List<string> columnsName) where T : new()
        {
            T obj = new T();
            try
            {
                //string columnname = "";
                int columnindex;
                string value = "";
                PropertyInfo[] Properties; Properties = typeof(T).GetProperties();


                foreach (PropertyInfo objProperty in Properties)
                {
                    columnindex = columnsName.FindIndex(name => name.ToLower() == objProperty.Name.ToLower());
                    //if (!string.IsNullOrEmpty(columnname))
                    if (columnindex != -1)
                    {
                        value = row[columnindex].ToString();
                        if (!string.IsNullOrEmpty(value))
                        {
                            if (Nullable.GetUnderlyingType(objProperty.PropertyType) != null)
                            {
                                value = row[columnindex].ToString().Replace("$", "").Replace(",", "");
                                objProperty.SetValue(obj, Convert.ChangeType(value, Type.GetType(Nullable.GetUnderlyingType(objProperty.PropertyType).ToString())), null);
                            }
                            else
                            {
                                value = row[columnindex].ToString().Replace("%", "");
                                objProperty.SetValue(obj, Convert.ChangeType(value, Type.GetType(objProperty.PropertyType.ToString())), null);
                            }
                        }
                    }
                }
                return obj;
            }
            catch { return obj; }
        }
    }
}
