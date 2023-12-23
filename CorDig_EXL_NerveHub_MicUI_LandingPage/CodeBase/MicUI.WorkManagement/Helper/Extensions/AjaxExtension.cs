namespace MicUI.WorkManagement.Helper.Extensions
{
    public static class AjaxExtension
    {
        //HttpRequest Extension method to 
        //check if the incoming request is an AJAX call 
        public static bool IsAjaxRequest(this HttpRequest request)
        {
            if (request == null)
                throw new ArgumentNullException("request");

            if (request.Headers != null)
                return request.Headers["X-Requested-With"] == "XMLHttpRequest";
            return false;
        }
    }
}
