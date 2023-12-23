using Microsoft.AspNetCore.Mvc;
using MicUI.WorkManagement.Models.Response;
using MicUI.WorkManagement.Models.ViewModels;
using MicUI.WorkManagement.Services.ServiceModel;
using System.Net.Http;

namespace MicUI.WorkManagement.Services.Configuration
{
    public interface IConfigApiService
    { 
        Task<MessageResponse<int>> GetMaxWeekAsync(int calanderId, int Year);
        Task<MessageResponse<List<BEProcessInfo>>> GetProcessListSearchAsync(int clientId, string? processName);
        Task<MessageResponse<List<BEProcessInfo>>> GetProcessListByClientIdAsync(int clientId, string? processName, bool activeProcess);
        Task<MessageResponse<List<BECampaignInfo>>> GetProcessWiseCampaignListAsync(int processId);
        Task<MessageResponse<IList<BELanguages>>> GetLanguageListAsync(bool IsActiveLanguages);
        Task<MessageResponse<IList<BEMailTranslatorConfiguration>>> GetCampaignWiseLangList(int storeId, int formId);
        Task<MessageResponse<int>> GetCheckUserIsSuperOrFunctionalAdminAsync();

        Task<MessageResponse<List<BEClientInfo>>> GetClientAsync(string? searchText, bool isActive);
        Task<MessageResponse<string>> GetFirstLastDayOfCalenderAsync(int processId, int month, int year);
        Task<MessageResponse<string>> UpdateProcessOffsAsync(ProcessOffModel objProcessOff);
        Task<MessageResponse<string>> AddProcessOffsAsync(ProcessOffModel objProcessOff);
        Task<MessageResponse<IList<BEProcessOff>>> GetProcessOffListAsync(int processId, int month, int year);
        Task<MessageResponse<string>> AddCalendarAsync(CalendarInfoMasterModel objProcessOff);
        Task<MessageResponse<string>> UpdateCalendarAsync(CalendarInfoMasterModel objProcessOff);
        Task<MessageResponse<List<CalendarInfoMasterModel>>> GetCalendarListAsync(string CalenderName, bool IsActive);
        Task<MessageResponse<CalendarInfoMasterModel>> GetCalendarByIdAsync(int CalenderId);
        Task<MessageResponse<List<CalendarDataModel>>> GetCalendarDataListAsync(int CalendarID, int Month, int Year);
        Task<MessageResponse<CalendarDataDetails>> GetCalendarDataIDAsync(int CalendarID, int Month, int Year);
        Task<MessageResponse<string>> AddCalenderDataAsync(CalendarDataDetails strCalendarDataModel);
        Task<MessageResponse<List<BreakMasterModel>>> GetBreakInfoListAsync(string? breakName);
        Task<MessageResponse<BreakMasterModel>> GetBreakMasterByIdAsync(int breakID);
        Task<MessageResponse<string>> AddBreakMasterAsync(BreakMasterModel BreakMasterModel);
        Task<MessageResponse<string>> UpdateBreakMasterAsync(BreakMasterModel BreakMasterModel);
        Task<MessageResponse<TerminationCodeModel>> GetTerminationCodeByIdAsync(int terminationCodeID);
        Task<MessageResponse<List<TerminationCodeModel>>> GetTerminationCodeListAsync(string? searchTerminationCode);
        Task<MessageResponse<string>> UpdateTerminationCodeAsync(TerminationCodeModel TerminationCodeModel);
        Task<MessageResponse<string>> AddTerminationCodeAsync(TerminationCodeModel TerminationCodeModel);
        Task<MessageResponse<List<ShiftMasterModel>>> GetShiftListAsync(string? searchShiftName);
        Task<MessageResponse<ShiftMasterModel>> GetShiftByIdAsync(int shiftId);
        Task<MessageResponse<string>> UpdateShiftAsync(ShiftMasterModel ShiftMasterModel);
        Task<MessageResponse<string>> AddShiftAsync(ShiftMasterModel ShiftMasterModel);
        Task<MessageResponse<string>> UpdateCalenderDataAsync(CalendarDataDetails strCalendarDataModel);
        Task<MessageResponse<ProcessOffDisplayDetail>> GetProcessOffsByIdAsync(int processId, int month, int year);
    }
    
}
