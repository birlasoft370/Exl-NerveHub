using MicUI.WorkManagement.Services.ServiceModel;

namespace MicUI.WorkManagement.Module.Administration
{
    public interface IShiftService
    {
        List<ShiftMasterModel> GetShiftList(string? searchShiftName);
        ShiftMasterModel GetShiftById(int ShiftId);
        string UpdateShift(ShiftMasterModel ShiftMasterModel);
       string AddShift(ShiftMasterModel ShiftMasterModel);
    }
    
         
}
