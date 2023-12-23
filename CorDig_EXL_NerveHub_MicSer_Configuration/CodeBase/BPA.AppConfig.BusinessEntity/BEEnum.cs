using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BPA.AppConfig.BusinessEntity
{
    /// <summary>
    /// Permission set 
    /// </summary>
    public enum PermissionSet
    {
        /// <summary>
        /// 
        /// </summary>
        ADD = 1,
        /// <summary>
        /// 
        /// </summary>
        UPDATE = 2,
        /// <summary>
        /// 
        /// </summary>
        DELETE = 3,
        /// <summary>
        /// 
        /// </summary>
        VIEW = 4,
        /// <summary>
        ///  Added to check approve permission of form
        /// </summary>
        APPROVE = 5
    }

    /// <summary>
    /// 
    /// </summary>
    public enum WeekDay
    {
        /// <summary>
        /// 
        /// </summary>
        Sunday = 0,
        /// <summary>
        /// 
        /// </summary>
        Monday = 1,
        /// <summary>
        /// 
        /// </summary>
        Tuesday = 2,
        /// <summary>
        /// 
        /// </summary>
        Wednesday = 3,
        /// <summary>
        /// 
        /// </summary>
        Thrusday = 4,
        /// <summary>
        /// 
        /// </summary>
        Friday = 5,
        /// <summary>
        /// 
        /// </summary>
        Saturday = 6
    }

    /// <summary>
    /// Permission set 
    /// </summary>
    public enum RowState
    {
        /// <summary>
        /// 
        /// </summary>
        NONE = 0,
        /// <summary>
        /// 
        /// </summary>
        NEW = 1,
        /// <summary>
        /// 
        /// </summary>
        UPDATED = 2,
        /// <summary>
        /// 
        /// </summary>
        DELETED = 3
    }
}
