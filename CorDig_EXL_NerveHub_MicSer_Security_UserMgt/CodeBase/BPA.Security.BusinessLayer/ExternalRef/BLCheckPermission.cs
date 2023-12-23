using BPA.Security.BusinessEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BPA.Security.BusinessLayer.ExtrernalRefre
{
    public static class BLCheckPermission
    {

        public static bool hasPermission(int iFormID, int iUserID, PermissionSet ePermission)
        {
            return true;
        }
    }
}
