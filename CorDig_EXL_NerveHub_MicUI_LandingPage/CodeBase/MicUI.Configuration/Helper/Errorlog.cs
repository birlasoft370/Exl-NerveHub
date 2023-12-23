using MicUI.Configuration.Helper.Sessions;
using System.Data.Common;

namespace MicUI.Configuration.Helper
{
    public static class Errlog
    {
        static string _ErrorLogDir = AppSettingsHelper.Configuration.GetValue<string>("DeveloperLogFilePath");

        private static string _Heading()
        {
            return string.Format("{0}", DateTime.Now.ToString("[dd/MM/yyyy hh:mm:ss:fff]"));
        }

        public static void ErrorLogFile(string className, string methodName, string message)
        {
            try
            {
                if (!Directory.Exists(string.Format(@"{0}\", _ErrorLogDir)))
                {
                    Directory.CreateDirectory(string.Format(@"{0}\", _ErrorLogDir));
                }

                string strFileName = string.Format(@"{0}\{1}.txt", _ErrorLogDir, DateTime.Now.ToString("ddMMMyyyy"));

                StreamWriter sw = new StreamWriter(strFileName, true);
                sw.WriteLine(_Heading());
                if (className.Length > 0)
                    sw.WriteLine("Class Name: " + className);//Environment.NewLine
                if (methodName.Length > 0)
                    sw.WriteLine("Method Name: " + methodName);
                sw.WriteLine("Message: " + message);
                sw.WriteLine("------------------------------");
                sw.Flush();
                sw.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }

    public static class AppSettingsHelper
    {
        private static IConfiguration config;
        public static IConfiguration Configuration
        {
            get
            {
                var builder = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json");
                config = builder.Build();
                return config;
            }
        }
    }
}
