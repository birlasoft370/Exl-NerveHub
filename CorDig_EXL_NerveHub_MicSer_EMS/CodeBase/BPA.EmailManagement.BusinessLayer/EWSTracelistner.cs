using Microsoft.Exchange.WebServices.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BPA.EmailManagement.BusinessLayer
{
    public class EwsTraceListener : ITraceListener
    {
        private const string XDiagInfoHeaderName = "X-DiagInfo: ";
        private const string RequestIdHeaderName = "RequestId: ";

        public string LastDiagInfoHeader { get; private set; }
        public string LastRequestIdHeader { get; private set; }

        public void Trace(string traceType, string traceMessage)
        {
            if (traceType == "EwsResponseHttpHeaders")
            {
                string diagInfo = null;
                if (TryGetResponseHeader(traceMessage, XDiagInfoHeaderName, out diagInfo))
                {
                    // DebugLog.WriteInfo("X-DiagInfo", diagInfo);
                    this.LastDiagInfoHeader = diagInfo;
                }

                string requestId = null;
                if (TryGetResponseHeader(traceMessage, RequestIdHeaderName, out requestId))
                {
                    // DebugLog.WriteInfo("RequestId", requestId);
                    this.LastRequestIdHeader = requestId;
                }
            }

            //DebugLog.WriteEwsLog(traceType, traceMessage);//issue
        }

        private static bool TryGetResponseHeader(string traceMessage, string headerName, out string headerValue)
        {
            headerValue = null;

            if (!traceMessage.Contains(headerName))
            {
                return false;

            }

            int start = traceMessage.IndexOf(headerName) + headerName.Length;
            int end = traceMessage.IndexOf("\n", start);

            headerValue = traceMessage.Substring(start, end - start);

            return true;
        }
    }
}
