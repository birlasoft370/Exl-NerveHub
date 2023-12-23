using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;
using MicUI.Configuration.Helper;
using MicUI.Configuration.Models.ViewModels;
using MicUI.Configuration.Module.UserPreference;
using Newtonsoft.Json;
using System.Data;

namespace MicUI.Configuration.Controllers
{
    public class UsageReportController : BaseController
    {
        private readonly IUserPreferenceService _UserPreferenceService;

        public UsageReportController(IUserPreferenceService userPreferenceService)
        {
            _UserPreferenceService = userPreferenceService;
        }

        public ActionResult Index()
        {
            return View(new UsageReportViewModel());
        }
        public JsonResult ChartGetBatchStatusReportData([DataSourceRequest] DataSourceRequest request, string FromDate, string Todate)
        {
            object DQAdata = null;
            object SWMDATA = null;
            object SDQAPerData = null;
            object SDQAAllData = null;
            JsonResult jsonResult = Json(null);
            DataSet? dsReportData = new();

            DateTime fromDate = DateTimeTimeZoneConversion.ConverttoServerTime(DateTime.Parse(FromDate), base.oUser.sUserTimeZone, base.oUser.sServerTimeZone, false);
            DateTime toDate = DateTimeTimeZoneConversion.ConverttoServerTime(DateTime.Parse(Todate), base.oUser.sUserTimeZone, base.oUser.sServerTimeZone, false);

            string jsonStringReport = _UserPreferenceService.GetMonthlyUsageReports(fromDate, toDate);

            dsReportData = !string.IsNullOrWhiteSpace(jsonStringReport) ? JsonConvert.DeserializeObject<DataSet>(jsonStringReport) : null;

            if (dsReportData != null && dsReportData.Tables.Count > 0)
            {
                SWMDATA = dsReportData.Tables[0].AsEnumerable().Select(r => r.Table.Columns.Cast<DataColumn>()
                .Select(c => new KeyValuePair<string, string>(c.ColumnName.Replace("[^a-zA-Z0-9]+", "").Replace(" ", ""), r[c.Ordinal].ToString())).ToDictionary(z => z.Key, z => z.Value)).ToList();

                DQAdata = dsReportData.Tables[1].AsEnumerable().Select(r => r.Table.Columns.Cast<DataColumn>()
              .Select(c => new KeyValuePair<string, string>(c.ColumnName.Replace("[^a-zA-Z0-9]+", "").Replace(" ", ""), r[c.Ordinal].ToString())).ToDictionary(z => z.Key, z => z.Value)).ToList();

                SDQAPerData = dsReportData.Tables[2].AsEnumerable().Select(r => r.Table.Columns.Cast<DataColumn>()
                              .Select(c => new KeyValuePair<string, string>(c.ColumnName.Replace("[^a-zA-Z0-9]+", "").Replace(" ", ""), r[c.Ordinal].ToString())).ToDictionary(z => z.Key, z => z.Value)).ToList();

                SDQAAllData = dsReportData.Tables[3].AsEnumerable().Select(r => r.Table.Columns.Cast<DataColumn>()
                            .Select(c => new KeyValuePair<string, string>(c.ColumnName.Replace("[^a-zA-Z0-9]+", "").Replace(" ", ""), r[c.Ordinal].ToString())).ToDictionary(z => z.Key, z => z.Value)).ToList();
            }

            jsonResult = Json(new
            {

                lastSWMData = SWMDATA,
                lstDQAData = DQAdata,
                lstSDALLDATA = SDQAPerData,
                lstAllData = SDQAAllData

            });
            return jsonResult;
        }
    }
}
