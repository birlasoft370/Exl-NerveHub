using BPA.EmailManagement.BusinessEntity;
using BPA.EmailManagement.BusinessEntity.ExternalRef.Application;
using BPA.EmailManagement.BusinessLayer.Dominos;
using BPA.EmailManagement.ServiceContract.ServiceContracts;
using Microsoft.AspNetCore.Mvc;

namespace BPA.EmailConfiguration.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IMailServicTest mailServicTest;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IMailServicTest mailServicTest)
        {
            _logger = logger;
            this.mailServicTest = mailServicTest;
        }

        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [HttpPost]
        [Route("InsertLogger")]
        public async Task<MessageResponse<string>> InsertLogger([FromBody] MessageResponse<string> loggerObject)
        {
            var result = new MessageResponse<string>();
            try
            {
                result.Data = await Task.FromResult("Ok");
            }
            catch (Exception ex)
            {
                result.Message = $"{ex?.Message} {ex?.InnerException?.Message} {ex?.StackTrace}";
            }
            return result;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {

            var obj1 = new LotusNotes();
            var mailFoldersTest = obj1.test();
            BEMailConfiguration bEMail = new();
            BETenant bETenant = new BETenant();
            var mailFolders1 = obj1.GetMailFolderList(bEMail, bETenant);
            /* var res = mailServicTest.GetFolder();
            */

            return Enumerable.Range(1, 5).Select(index => new WeatherForecast()
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }
    }

    public class WeatherForecast
    {
        public DateTime Date { get; set; }
        public int TemperatureC { get; set; }
        public string Summary { get; set; }
    }

    public class BLEmailFactoryTest : IMailServicTest
    {
        IMailServicTest IMService = null;

        private IMailServicTest getclass(int Exchangeserverversion)
        {
            switch (Exchangeserverversion)
            {
                case 0:
                    IMService = new LotusNotesTest();
                    break;

                case 1:
                    IMService = new Office2007Test();
                    break;
            }
            return IMService;
        }

        public string GetFolder()
        {
            var result = getclass(0).GetFolder();
            return result;
        }
    }
    public interface IMailServicTest
    {
        string GetFolder();
    }

    public class LotusNotesTest : IMailServicTest
    {
        public string GetFolder()
        {
            return "LotusNotes";
        }
    }

    public class Office2007Test : IMailServicTest
    {
        public string GetFolder()
        {
            return "Office2007";
        }
    }

    public class Office2010Test : IMailServicTest
    {
        public string GetFolder()
        {
            return "Office2010";
        }
    }
}