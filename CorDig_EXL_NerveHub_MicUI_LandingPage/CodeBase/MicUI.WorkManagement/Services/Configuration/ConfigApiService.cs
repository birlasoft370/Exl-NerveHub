using Microsoft.AspNetCore.Mvc;
using MicUI.WorkManagement.Models.Response;
using MicUI.WorkManagement.Models.ViewModels;
using MicUI.WorkManagement.Services.ServiceModel;
using System;
using System.Net.Http.Json;
using Telerik.SvgIcons;

namespace MicUI.WorkManagement.Services.Configuration
{
  
        public class ConfigApiService : BaseApiService, IConfigApiService
        {
            public ConfigApiService(HttpClient client, IHttpContextAccessor httpContextAccessor) : base(client, httpContextAccessor)
            {

            }
        
     
        public async Task<MessageResponse<string>> GetFirstLastDayOfCalenderAsync(int processId, int month, int year)
        {
            var content = await _client.GetFromJsonAsync<MessageResponse<string>>($"ProcessOffs/GetFirstLastDayOfCalender?processId={processId}&month={month}&year={year}");
            return content;
        }
        public async Task<MessageResponse<string>> UpdateProcessOffsAsync(ProcessOffModel objProcessOff)
        {
            var response = await _client.PutAsJsonAsync("ProcessOffs/UpdateProcessOffs", objProcessOff);
            return await response.Content.ReadFromJsonAsync<MessageResponse<string>>();
           
        }
        public async Task<MessageResponse<string>> AddProcessOffsAsync(ProcessOffModel objProcessOff)
        {
            var response = await _client.PostAsJsonAsync("ProcessOffs/AddProcessOffs", objProcessOff);
            return await response.Content.ReadFromJsonAsync<MessageResponse<string>>();

        }
        public async Task<MessageResponse<IList<BEProcessOff>>> GetProcessOffListAsync(int processId, int month, int year)
        {
            var content = await _client.GetFromJsonAsync<MessageResponse<IList<BEProcessOff>>>($"ProcessOffs/GetProcessOffList?processId={processId}&month={month}&year={year}");
            return content;
        }
        public async Task<MessageResponse<string>> AddCalendarAsync(CalendarInfoMasterModel objProcessOff)
        {
            var response = await _client.PostAsJsonAsync("CalendarMaster/AddCalendar", objProcessOff);
            return await response.Content.ReadFromJsonAsync<MessageResponse<string>>();

        }
        public async Task<MessageResponse<string>> UpdateCalendarAsync(CalendarInfoMasterModel objProcessOff)
        {
            var response = await _client.PutAsJsonAsync("CalendarMaster/UpdateCalendar", objProcessOff);
            return await response.Content.ReadFromJsonAsync<MessageResponse<string>>();

        }
        public async Task<MessageResponse<List<CalendarInfoMasterModel>>> GetCalendarListAsync(string CalenderName, bool IsActive)
        {
            var content = await _client.GetFromJsonAsync<MessageResponse<List<CalendarInfoMasterModel>>>($"CalendarMaster/GetCalendarList?calendarSearchName={CalenderName}&IsActive{IsActive}");
            return content;
        }
        public async Task<MessageResponse<CalendarInfoMasterModel>> GetCalendarByIdAsync(int CalenderId)
        {
            var content = await _client.GetFromJsonAsync<MessageResponse<CalendarInfoMasterModel>>($"CalendarMaster/GetCalendarById?calendarID={CalenderId}");
            return content;
        }
        public async Task<MessageResponse<int>> GetMaxWeekAsync( int calanderId, int Year)
        {
            var content = await _client.GetFromJsonAsync<MessageResponse<int>>($"CalendarData/GetMaxWeek?calanderId={calanderId}&Year={Year}");
            return content;
        }
        public async Task<MessageResponse<List<CalendarDataModel>>> GetCalendarDataListAsync(int CalendarID, int Month, int Year)
        {
            var content = await _client.GetFromJsonAsync<MessageResponse<List<CalendarDataModel>>>($"CalendarData/GetCalendarDataList?CalendarID={CalendarID}&Month={Month}&Year={Year}");
            return content;
        }
        public async Task<MessageResponse<CalendarDataDetails>> GetCalendarDataIDAsync(int CalendarID, int Month, int Year)
        {
            var content = await _client.GetFromJsonAsync<MessageResponse<CalendarDataDetails>>($"CalendarData/GetCalendarDataById?CalendarID={CalendarID}&Month={Month}&Year={Year}");
            return content;
        }
        public async Task<MessageResponse<string>> AddCalenderDataAsync(CalendarDataDetails strCalendarDataModel)
        {
           
            var response = await _client.PostAsJsonAsync("CalendarData/AddCalenderData", strCalendarDataModel);
            return await response.Content.ReadFromJsonAsync<MessageResponse<string>>();
        }
        public async Task<MessageResponse<string>> UpdateCalenderDataAsync(CalendarDataDetails strCalendarDataModel)
        {
            var response = await _client.PutAsJsonAsync("CalendarData/UpdateCalenderData", strCalendarDataModel);
            return await response.Content.ReadFromJsonAsync<MessageResponse<string>>();
        }
        public async Task<MessageResponse<List<ShiftMasterModel>>> GetShiftListAsync(string? searchShiftName)
        {
            var content = await _client.GetFromJsonAsync<MessageResponse<List<ShiftMasterModel>>>($"ShiftMaster/GetShiftList?searchShiftName={searchShiftName}");
            return content;
        }
        public async Task<MessageResponse<ShiftMasterModel>> GetShiftByIdAsync(int shiftId)
        {
            var content = await _client.GetFromJsonAsync<MessageResponse<ShiftMasterModel>>($"ShiftMaster/GetShiftById?shiftID={shiftId}");
            return content;
        }
        public async Task<MessageResponse<string>> UpdateShiftAsync(ShiftMasterModel ShiftMasterModel)
        {
            var response = await _client.PutAsJsonAsync("ShiftMaster/UpdateShift", ShiftMasterModel);
            return await response.Content.ReadFromJsonAsync<MessageResponse<string>>();
        }
        public async Task<MessageResponse<string>> AddShiftAsync(ShiftMasterModel ShiftMasterModel)
        {
            var response = await _client.PostAsJsonAsync("ShiftMaster/AddShift", ShiftMasterModel);
            return await response.Content.ReadFromJsonAsync<MessageResponse<string>>();

        }
        public async Task<MessageResponse<TerminationCodeModel>> GetTerminationCodeByIdAsync(int terminationCodeID)
        {
            var content = await _client.GetFromJsonAsync<MessageResponse<TerminationCodeModel>>($"TerminationCode/GetTerminationCodeById?terminationCodeID={terminationCodeID}");
            return content;

        }
        public async Task<MessageResponse<List<TerminationCodeModel>>> GetTerminationCodeListAsync(string? searchTerminationCode)
        {
            var content = await _client.GetFromJsonAsync<MessageResponse<List<TerminationCodeModel>>>($"TerminationCode/GetTerminationCodeList?searchTerminationCode={searchTerminationCode}");
            return content;

        }
        public async Task<MessageResponse<string>> AddTerminationCodeAsync(TerminationCodeModel TerminationCodeModel)
        {
            var response = await _client.PostAsJsonAsync("TerminationCode/AddTerminationCode", TerminationCodeModel);
            return await response.Content.ReadFromJsonAsync<MessageResponse<string>>();

        }
        public async Task<MessageResponse<string>> UpdateTerminationCodeAsync(TerminationCodeModel TerminationCodeModel)
        {
            var response = await _client.PutAsJsonAsync("TerminationCode/UpdateTerminationCode", TerminationCodeModel);
            return await response.Content.ReadFromJsonAsync<MessageResponse<string>>();

        }
        public async Task<MessageResponse<List<BreakMasterModel>>> GetBreakInfoListAsync(string? breakName)
        {
            var content = await _client.GetFromJsonAsync<MessageResponse<List<BreakMasterModel>>>($"BreakMaster/GetBreakInfoList?breakName={breakName}");
            return content;
        }
        public async Task<MessageResponse<BreakMasterModel>> GetBreakMasterByIdAsync(int breakID)
        {
            var content = await _client.GetFromJsonAsync<MessageResponse<BreakMasterModel>>($"BreakMaster/GetBreakInfoById?breakID={breakID}");
            return content;
        }
        public async Task<MessageResponse<string>> AddBreakMasterAsync(BreakMasterModel BreakMasterModel)
        {
            var response = await _client.PostAsJsonAsync("BreakMaster/AddBreakInfo", BreakMasterModel);
            return await response.Content.ReadFromJsonAsync<MessageResponse<string>>();

        }
        public async Task<MessageResponse<string>> UpdateBreakMasterAsync(BreakMasterModel BreakMasterModel)
        {
            var response = await _client.PutAsJsonAsync("BreakMaster/UpdateBreakInfo", BreakMasterModel);
            return await response.Content.ReadFromJsonAsync<MessageResponse<string>>();
        }

    
        public async Task<MessageResponse<List<BEClientInfo>>> GetClientAsync(string? searchText, bool isActive)
        {
            var content = await _client.GetFromJsonAsync<MessageResponse<List<BEClientInfo>>>($"Client/GetClient?searchText={searchText}&isActive={isActive}");
            return content ?? new MessageResponse<List<BEClientInfo>>();
        }
        public async Task<MessageResponse<List<BEProcessInfo>>> GetProcessListSearchAsync(int clientId, string? processName)
        {
            var content = await _client.GetFromJsonAsync<MessageResponse<List<BEProcessInfo>>>($"Process/GetProcessListSearch?clientId={clientId}&processName={processName}");
            return content ?? new MessageResponse<List<BEProcessInfo>>();
        }

        public async Task<MessageResponse<List<BEProcessInfo>>> GetProcessListByClientIdAsync(int clientId, string? processName, bool activeProcess)
        {
            var content = await _client.GetFromJsonAsync<MessageResponse<List<BEProcessInfo>>>($"Process/GetProcessListByClientId?clientId={clientId}&processName={processName}&activeProcess={activeProcess}");
            return content ?? new MessageResponse<List<BEProcessInfo>>();
        }
        public async Task<MessageResponse<List<BECampaignInfo>>> GetProcessWiseCampaignListAsync(int processId)
        {
            var content = await _client.GetFromJsonAsync<MessageResponse<List<BECampaignInfo>>>($"Campaign/GetProcessWiseCampaignList?processId={processId}");
            return content ?? new MessageResponse<List<BECampaignInfo>>();
        }
        public async Task<MessageResponse<IList<BELanguages>>> GetLanguageListAsync(bool IsActiveLanguages)
        {
            var content = await _client.GetFromJsonAsync<MessageResponse<IList<BELanguages>>>($"LanguageConfig/GetLanguages?IsActiveLanguages={IsActiveLanguages}");
            return content ?? new MessageResponse<IList<BELanguages>>();
        }
        public async Task<MessageResponse<IList<BEMailTranslatorConfiguration>>> GetCampaignWiseLangList(int storeId, int formId)
        {
            var content = await _client.GetFromJsonAsync<MessageResponse<IList<BEMailTranslatorConfiguration>>>($"LanguageConfig/GetCampaignWiseLangList?storeId={storeId}&formId={formId}");
            return content ?? new MessageResponse<IList<BEMailTranslatorConfiguration>>();
        }
        public async Task<MessageResponse<int>> GetCheckUserIsSuperOrFunctionalAdminAsync()
        {
            var content = await _client.GetFromJsonAsync<MessageResponse<int>>("Campaign/GetCheckUserIsSuperOrFunctionalAdmin");
            return content ?? new MessageResponse<int>();
        }
        public async Task<MessageResponse<ProcessOffDisplayDetail>> GetProcessOffsByIdAsync(int processId, int month, int year)
        {
            var content = await _client.GetFromJsonAsync<MessageResponse<ProcessOffDisplayDetail>>($"ProcessOffs/GetProcessOffsById?processId={processId}&month={month}&year={year}");
            return content;
        }
    }

}

