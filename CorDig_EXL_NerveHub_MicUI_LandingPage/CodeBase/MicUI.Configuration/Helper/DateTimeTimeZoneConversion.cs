using System.Data;

namespace MicUI.Configuration.Helper
{
    public class DateTimeTimeZoneConversion
    {
        static DateTime dtDate = DateTime.UtcNow;

        /// <summary>
        /// Get the current datetime
        /// </summary>
        /// <param name="isServerTime">current datetime is in client Timezone or server Timezone.</param>
        /// <param name="sUserTimeZone">The user time zone.</param>
        /// <param name="sServerTimeZone">The server time zone.</param>
        /// <returns></returns>
        public static DateTime GetCurrentDateTime(bool isServerTime, string sUserTimeZone, string sServerTimeZone)
        {
            string[] aUserTimeZone = sUserTimeZone.Split('/');

            TimeZoneInfo UserTz = TimeZoneInfo.FindSystemTimeZoneById(aUserTimeZone[0]);

            TimeZoneInfo ServerTz = TimeZoneInfo.FindSystemTimeZoneById(sServerTimeZone);

            DateTime dt;
            if (isServerTime)
            {
                dt = TimeZoneInfo.ConvertTime(dtDate, ServerTz);
                AddDaylightShift(dt, ServerTz);

            }
            else
            {
                dt = TimeZoneInfo.ConvertTime(dtDate, UserTz);
                AddDaylightShift(dt, UserTz);
            }
            UserTz = null;
            ServerTz = null;
            return dt;
        }

        /// <summary>
        /// Convert User Date to server Datetime.
        /// </summary>
        /// <param name="dt">The dt.</param>
        /// <param name="sUserTimeZone">The s user time zone.</param>
        /// <param name="sServerTimeZone">The s server time zone.</param>
        /// <param name="IsDateLong">date return as short or long.</param>
        /// <returns></returns>
        static DateTime ConverttoServerTime(DateTime dt, string sUserTimeZone, string sServerTimeZone, bool isLongDate)
        {
            DateTime serverDate = new DateTime(dt.Ticks, DateTimeKind.Unspecified);
            string[] aUserTimeZone = sUserTimeZone.Split('/');

            TimeZoneInfo userTz = TimeZoneInfo.FindSystemTimeZoneById(aUserTimeZone[0]);
            TimeZoneInfo serverTz = TimeZoneInfo.FindSystemTimeZoneById(sServerTimeZone);
            if (userTz.DisplayName != serverTz.DisplayName)
            {
                if (!isLongDate)
                {
                    DateTime userDate = TimeZoneInfo.ConvertTime(dtDate, userTz);
                    TimeSpan i = userDate.TimeOfDay;
                    serverDate = AddDaylightShift(TimeZoneInfo.ConvertTime(serverDate.Add(i), userTz, serverTz), serverTz);
                    serverDate = Convert.ToDateTime(serverDate.ToShortDateString());

                }
                else
                {
                    serverDate = TimeZoneInfo.ConvertTime(serverDate, userTz, serverTz);
                    AddDaylightShift(serverDate, serverTz);
                }

            }
            userTz = null;
            serverTz = null;
            return serverDate;
        }
        public static T ConverttoServerTime<T>(T item, string sUserTimeZone, string sServerTimeZone, bool isLongDate)
        {

            DateTime currentDate;
            var properties = item.GetType().GetProperties();
            foreach (var property in properties)
            {
                if (property.PropertyType == typeof(DateTime))
                {
                    currentDate = (DateTime)property.GetValue(item, null);
                    if (currentDate != DateTime.MinValue)
                    {
                        currentDate = ConverttoServerTime(currentDate, sUserTimeZone, sServerTimeZone, isLongDate);
                        if (property.CanWrite)
                            property.SetValue(item, currentDate, null);
                    }
                }
            }

            return item;
        }

        public static DateTime UserTime(string sTimeZone)
        {
            string[] aUserTimeZone = sTimeZone.Split('/');
            TimeZoneInfo tz = TimeZoneInfo.FindSystemTimeZoneById(aUserTimeZone[0]);
            DateTime serverDate = TimeZoneInfo.ConvertTime(DateTime.UtcNow, tz);
            AddDaylightShift(serverDate, tz);
            return serverDate;
        }

        /// <summary>
        /// Adjusts the time zone.
        /// </summary>
        /// <typeparam name="T">The type of the T.</typeparam>
        /// <param name="lListData">The list data.</param>
        /// <param name="sUserTimeZone">The  user time zone.</param>
        /// <param name="sServerTimeZone">The  server time zone.</param>
        /// <param name="toServerTime">To server time.</param>
        /// <returns></returns>
        public static IList<T> AdjustTimeZone<T>(IList<T> lListData, string sUserTimeZone, string sServerTimeZone)
        {
            if (lListData == null)
                return lListData;
            if (lListData.Count > 0)
            {


                foreach (var item in lListData)
                {
                    var properties = item.GetType().GetProperties();
                    foreach (var property in properties)
                    {
                        if (property.PropertyType == typeof(DateTime))
                        {
                            if ((DateTime)property.GetValue(item, null) != DateTime.MinValue)
                            {
                                if (property.CanWrite)
                                {
                                    DateTime MyDateTime = ConverttoServerTime((DateTime)property.GetValue(item, null), sUserTimeZone, sServerTimeZone, true);
                                    property.SetValue(item, MyDateTime, null);
                                }
                            }
                        }
                    }
                }
            }
            return lListData;
        }


        /// <summary>
        /// Adjusts the time zone.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="sUserTimeZone">The s user time zone.</param>
        /// <param name="sServerTimeZone">The s server time zone.</param>
        /// <param name="toServerTime">To server time.</param>
        /// <returns></returns>
        public static DateTime AdjustTimeZone(DateTime item, string sUserTimeZone, string sServerTimeZone)
        {
            if (item == null)
                return item;
            item = ConverttoServerTime(item, sUserTimeZone, sServerTimeZone, true);
            return item;
        }

        /// <summary>
        /// Adjusts the time zone.
        /// </summary>
        /// <typeparam name="T">The type of the T.</typeparam>
        /// <param name="item">The item.</param>
        /// <param name="sUserTimeZone">The s user time zone.</param>
        /// <param name="sServerTimeZone">The s server time zone.</param>
        /// <param name="toServerTime">To server time.</param>
        /// <returns></returns>
        public static T AdjustTimeZone<T>(T item, string sUserTimeZone, string sServerTimeZone, bool toServerTime)
        {
            if (item == null)
                return item;
            var properties = item.GetType().GetProperties();
            foreach (var property in properties)
            {
                if (property.PropertyType == typeof(DateTime))
                {
                    if ((DateTime)property.GetValue(item, null) != DateTime.MinValue)
                    {
                        if (property.CanWrite)
                        {
                            if (toServerTime)
                            {
                                property.SetValue(item, ((DateTime)property.GetValue(ConverttoServerTime(item, sServerTimeZone, sUserTimeZone, true), null)), null);
                            }
                            else
                            {
                                property.SetValue(item, ((DateTime)property.GetValue(ConverttoServerTime(item, sUserTimeZone, sServerTimeZone, true), null)), null);
                            }
                        }
                    }
                }
            }

            return item;
        }

        static DateTime AddDaylightShift(DateTime currentDate, TimeZoneInfo serverTimeZone)
        {
            if (serverTimeZone.IsDaylightSavingTime(currentDate))
            {
                foreach (TimeZoneInfo.AdjustmentRule adjustment in serverTimeZone.GetAdjustmentRules())
                {
                    currentDate = currentDate.Add(adjustment.DaylightDelta);
                }
            }
            return currentDate;
        }


        #region AdjustTimeZone
        /// <summary>
        /// Adjusts the time zone.
        /// </summary>
        /// <param name="dataSet">The data set.</param>
        /// <param name="sUserTimeZone">The s user time zone.</param>
        /// <param name="sServerTimeZone">The s server time zone.</param>
        /// <param name="toServerTime">To server time.</param>
        /// <returns></returns>
        public static DataSet AdjustTimeZone(DataSet dataSet, string sUserTimeZone, string sServerTimeZone)
        {
            if (dataSet == null)
                return null;
            try
            {
                if (dataSet.Tables.Count == 0)
                    return dataSet;
                // The following code iterates through each table, and find all the columns that are 
                // DateTime columns. After identifying the columns that have to be adjusted,
                // it traverses the data in the table and adjusts the DateTime columns back to their 
                // original values. You must leave the RowState of the DataRow in the same state 
                //after making the adjustments.
                foreach (DataTable table in dataSet.Tables)
                {
                    AdjustTimeZone(table, sUserTimeZone, sServerTimeZone);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
            return dataSet;
        }

        #endregion

        #region AdjustTimeZone Dataset
        /// <summary>
        /// Adjusts the time zone.
        /// </summary>
        /// <param name="dataSet">The data set.</param>
        /// <param name="sUserTimeZone">The s user time zone.</param>
        /// <param name="sServerTimeZone">The s server time zone.</param>
        /// <param name="toServerTime">To server time.</param>
        /// <returns></returns>
        public static DataTable AdjustTimeZone(DataTable dataTable, string sUserTimeZone, string sServerTimeZone)
        {
            if (dataTable == null)
                return null;
            // Obtains the time difference on the sender computer that
            //remoted this dataset to the Web service.
            string str;



            // The following code iterates through each table, and find all the columns that are 
            // DateTime columns. After identifying the columns that have to be adjusted,
            // it traverses the data in the table and adjusts the DateTime columns back to their 
            // original values. You must leave the RowState of the DataRow in the same state 
            //after making the adjustments.
            DataTable table = dataTable;
            DataColumnCollection columns = table.Columns;
            int[] ColumnNumbers = new int[columns.Count];
            int ColumnNumbersIndex = 0;
            for (int i = 0; i < columns.Count; i++)
            {
                DataColumn col = columns[i];
                if (col.DataType == typeof(DateTime))
                {
                    ColumnNumbers[ColumnNumbersIndex] = i;
                    ColumnNumbersIndex++;
                }
            }
            foreach (DataRow row in table.Rows)
            {
                switch (row.RowState)
                {
                    case DataRowState.Unchanged:
                        AdjustDateTimeValues(row, ColumnNumbers, ColumnNumbersIndex, sUserTimeZone, sServerTimeZone);
                        row.AcceptChanges();	// This is to make sure that the
                        // row appears to be unchanged again.
                        //Debug.Assert(row.RowState == DataRowState.Unchanged);
                        break;
                    case DataRowState.Added:
                        AdjustDateTimeValues(row, ColumnNumbers, ColumnNumbersIndex, sUserTimeZone, sServerTimeZone);
                        // The row is still in a DataRowState.Added state.
                        //Debug.Assert(row.RowState == DataRowState.Added);
                        break;
                    case DataRowState.Modified:
                        AdjustDateTimeValues(row, ColumnNumbers, ColumnNumbersIndex, sUserTimeZone, sServerTimeZone);
                        // The row is a still DataRowState.Modified.
                        //Debug.Assert(row.RowState == DataRowState.Modified);
                        break;
                    case DataRowState.Deleted:
                        //   This is to make sure that you obtain the right results if 
                        //the .RejectChanges()method is called.
                        row.RejectChanges();	// This is to "undo" the delete.
                        AdjustDateTimeValues(row, ColumnNumbers, ColumnNumbersIndex, sUserTimeZone, sServerTimeZone);
                        // To adjust the datatime values.
                        // The row is now in DataRowState.Modified state.
                        //Debug.Assert(row.RowState == DataRowState.Modified);
                        row.AcceptChanges();	// This is to mark the changes as permanent.
                        //Debug.Assert(row.RowState == DataRowState.Unchanged);
                        row.Delete();
                        // Delete the row. Now, it has the same state as it started.
                        // Debug.Assert(row.RowState == DataRowState.Deleted);
                        break;
                    default:
                        throw new ApplicationException
                        ("You must add a case statement that handles the new version of the dataset.");
                }
            }

            //str = dataSet.Tables["MyTable"].Rows[0][1].ToString();
            return dataTable;
        }

        #endregion
        static void AdjustDateTimeValues(DataRow row, int[] columnNumbers, int columnCount, string sUserTimeZone, string sServerTimeZone)
        {

            for (int i = 0; i < columnCount; i++)
            {
                int columnIndex = columnNumbers[i];
                if (row[columnIndex].ToString() != "")
                {
                    DateTime original = (DateTime)row[columnIndex];
                    DateTime modifiedDateTime = ConverttoServerTime(original, sUserTimeZone, sServerTimeZone, true);
                    row[columnIndex] = modifiedDateTime;
                }
            }
        }

        public static DataTable MakeClientData(DataTable dsData)
        {
            var dsClientData = dsData.Clone();
            try
            {


                foreach (DataColumn dc in dsClientData.Columns)
                {
                    if (dc.DataType == typeof(DateTime))
                    {
                        dc.DataType = typeof(string);
                    }
                }
                dsClientData.AcceptChanges();

                using (DataTableReader rdr = dsData.CreateDataReader())
                {
                    if (rdr.HasRows)
                    {
                        while (rdr.Read())
                        {
                            object[] vals = new object[dsData.Columns.Count];
                            rdr.GetValues(vals);
                            dsClientData.Rows.Add(vals);
                        }
                        dsClientData.AcceptChanges();
                    }
                }
                dsClientData.AcceptChanges();

            }
            catch (Exception)
            {

            }
            return dsClientData;
        }

        public static DataSet MakeClientData(DataSet dsData)
        {
            var dsClientData = dsData.Clone();
            foreach (DataTable dt in dsClientData.Tables)
            {
                foreach (DataColumn dc in dt.Columns)
                {
                    if (dc.DataType == typeof(DateTime))
                    {
                        dc.DataType = typeof(string);
                    }
                }
                dt.AcceptChanges();
            }
            dsClientData.AcceptChanges();
            for (int i = 0; i < dsData.Tables.Count; i++)
            {
                using (DataTableReader rdr = dsData.Tables[i].CreateDataReader())
                {
                    if (rdr.HasRows)
                    {
                        while (rdr.Read())
                        {
                            object[] vals = new object[dsData.Tables[i].Columns.Count];
                            rdr.GetValues(vals);
                            dsClientData.Tables[i].Rows.Add(vals);
                        }
                        dsClientData.Tables[i].AcceptChanges();
                    }
                }
            }
            dsClientData.AcceptChanges();
            return dsClientData;
        }
    }
}
