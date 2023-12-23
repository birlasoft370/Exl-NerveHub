using MicUI.WorkManagement.Services.Configuration;
using MicUI.WorkManagement.Services.ServiceModel;

namespace MicUI.WorkManagement.Module.Administration
{
    public class ShiftService:IShiftService
    {
        private readonly IConfigApiService _configService;
        public ShiftService(IConfigApiService configService)
        {
            _configService = configService;
        }
        public List<ShiftMasterModel> GetShiftList(string? searchShiftName)
        {
            var result = _configService.GetShiftListAsync(searchShiftName).GetAwaiter().GetResult();
            return result.data;
        }
        public ShiftMasterModel GetShiftById(int ShiftId)
        {
            var result = _configService.GetShiftByIdAsync(ShiftId).GetAwaiter().GetResult();
            return result.data;
        }
        public string UpdateShift(ShiftMasterModel ShiftMasterModel)
        {
            var result = _configService.UpdateShiftAsync(ShiftMasterModel).GetAwaiter().GetResult();
            return result.data;

        }
        public string AddShift(ShiftMasterModel ShiftMasterModel)
        {
            var result = _configService.AddShiftAsync(ShiftMasterModel).GetAwaiter().GetResult();
            return result.data;
        }
    }
}
