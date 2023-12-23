using BPA.AppConfig.BusinessEntity.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace BPA.AppConfig.BusinessEntity
{
    [ServiceContract]
    public interface IDataOperation<T> : IDisposable
    {

        /// <summary>
        /// Inserts the data.
        /// </summary>
        /// <param name="ClObject">The object.</param>
        /// <param name="iFormID">The form ID.</param>
        //[OperationContract]
        void InsertData(T ClObject, int iFormID, BETenant oTenant);

        /// <summary>
        /// Updates the data.
        /// </summary>
        /// <param name="ClObject">The object.</param>
        /// <param name="iFormID">The form ID.</param>
       //[OperationContract]
        void UpdateData(T ClObject, int iFormID, BETenant oTenant);

        /// <summary>
        /// Deletes the data.
        /// </summary>
        /// <param name="ClObject">The object.</param>
        /// <param name="iFormID">The form ID.</param>
       //[OperationContract]
        void DeleteData(T ClObject, int iFormID, BETenant oTenant);
    }
}
