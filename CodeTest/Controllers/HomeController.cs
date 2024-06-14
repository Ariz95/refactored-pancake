using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Mvc;
using Quote.Contracts;
using Quote.Models;
using Newtonsoft.Json;
using System.Net;


namespace PruebaIngreso.Controllers
{
    public class HomeController : Controller
    {
        private readonly IQuoteEngine quote;

        public HomeController(IQuoteEngine quote)
        {
            this.quote = quote;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Test()
        {
            var request = new TourQuoteRequest
            {
                adults = 1,
                ArrivalDate = DateTime.Now.AddDays(1),
                DepartingDate = DateTime.Now.AddDays(2),
                getAllRates = true,
                GetQuotes = true,
                RetrieveOptions = new TourQuoteRequestOptions
                {
                    GetContracts = true,
                    GetCalculatedQuote = true,
                },
                TourCode = "E-U10-PRVPARKTRF",
                Language = Language.Spanish
            };

            var result = this.quote.Quote(request);
            var tour = result.Tours.FirstOrDefault();
            ViewBag.Message = "Test 1 Correcto";
            return View(tour);
        }

        public ActionResult Test2()
        {
            ViewBag.Message = "Test 2 Correcto";
            return View();
        }


        public async Task<ActionResult>Test3()
        {

            var code = "E-U10-UNILATIN";
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            using (HttpClient client = new HttpClient())
            {
                try
                {
                    string url = $"https://refactored-pancake.free.beeceptor.com/margin/{code}";
                    HttpResponseMessage response = new HttpResponseMessage();
                    var responseBody = string.Empty;

                    Task.Run(async () =>
                    {
                        response = await client.GetAsync(url);
                    }).GetAwaiter().GetResult();

                    if(response.StatusCode == HttpStatusCode.OK)
                    {
                       responseBody = await response.Content.ReadAsStringAsync();

                    } else
                    {
                        responseBody = JsonConvert.SerializeObject(new { margin = 0.0 });
                    }

                    ViewBag.Message = $"{responseBody}";
                }
                catch (Exception ex)
                {
                    ViewBag.Message = "pelas";

                }
                    return View();

            }
        }

        public ActionResult Test4()
        {
            var request = new TourQuoteRequest
            {
                adults = 1,
                ArrivalDate = DateTime.Now.AddDays(1),
                DepartingDate = DateTime.Now.AddDays(2),
                getAllRates = true, 
                GetQuotes = true,
                RetrieveOptions = new TourQuoteRequestOptions
                {
                    GetContracts = true,
                    GetCalculatedQuote = true,
                },
                Language = Language.Spanish
            };

            var result = this.quote.Quote(request);
            return View(result.TourQuotes);
        }
    }
}