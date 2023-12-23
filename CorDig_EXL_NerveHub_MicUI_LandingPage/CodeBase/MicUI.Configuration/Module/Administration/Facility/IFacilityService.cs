using Microsoft.AspNetCore.Mvc.Rendering;
using MicUI.Configuration.Services.ServiceModel;

namespace MicUI.Configuration.Module.Administration.Facility
{
    public interface IFacilityService
    {
        List<BEFacility> GetFacilityList(string searchFacility, bool bActiveFacility);
        FacilityModel GetFacilityById(int searchFacility);
        SelectList GetLocations();
        string InsertData(FacilityModel facilityModel);
        string UpdateData(FacilityModel facilityModel);
    }
}
