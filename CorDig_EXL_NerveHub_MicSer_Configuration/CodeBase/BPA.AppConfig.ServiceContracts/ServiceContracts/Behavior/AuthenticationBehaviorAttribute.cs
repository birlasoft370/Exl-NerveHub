using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Description;
using System.Text;
using System.Threading.Tasks;

namespace BPA.AppConfig.ServiceContracts.ServiceContracts.Behavior
{
    [AttributeUsage(AttributeTargets.Method)]
    public class AuthenticationBehaviorAttribute : Attribute, IOperationBehavior
    {
        #region IOperationBehavior Members

        public void AddBindingParameters(OperationDescription operationDescription, System.ServiceModel.Channels.BindingParameterCollection bindingParameters)
        {
            //
        }

        public void ApplyClientBehavior(OperationDescription operationDescription, System.ServiceModel.Dispatcher.ClientOperation clientOperation)
        {
            //
        }

        public void ApplyDispatchBehavior(OperationDescription operationDescription, System.ServiceModel.Dispatcher.DispatchOperation dispatchOperation)
        {
            // apply invoker (this is our custom AuthenticationInvoker).
          //  dispatchOperation.Invoker = new AuthenticationInvoker(dispatchOperation.Invoker);

        }

        public void Validate(OperationDescription operationDescription)
        {
            //
        }

        #endregion
    }
}
