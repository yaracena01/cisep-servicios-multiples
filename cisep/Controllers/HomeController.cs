using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Net.Mail;
using cisep.Models;
using AutoMapper;
using cisep.interfaces;
using cisep.ViewModels.ServicesViewModel;
using System.IO;
using System.Text;
using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json;
using ClosedXML.Excel;
using Ganss.Excel;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Microsoft.Extensions.Localization;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Http;
using DocumentFormat.OpenXml.Drawing.Charts;

namespace cisep.Controllers
{
    public class HomeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private IMapper _mapper;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IStringLocalizer<HomeController> _localizer;

        public HomeController(IUnitOfWork unitOfWork, IMapper mapper, IWebHostEnvironment webHostEnvironment, IStringLocalizer<HomeController> localizer)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _webHostEnvironment = webHostEnvironment;
            _localizer = localizer;
        }

        public ActionResult Index()
        {
            var model = _unitOfWork.Services.GetAll();

            var vw = _mapper.Map<List<ServicesViewModel>>(model);
            foreach (var x in vw)
            {
                x.Name = _localizer.GetString(x.Name);
                x.Description = _localizer.GetString(x.Description);
                x.UrlName = _localizer.GetString(x.UrlName);

                foreach (var x2 in x.Services_Details)
                {
                    x2.Name = _localizer.GetString(x2.Name);
                }
            }
            ViewBag.services = vw;       
            ViewBag.idiom = Request.Cookies["Idiom"] == null ? "English" : Request.Cookies["Idiom"];        
            return View();
        }
        [HttpPost]
        public IActionResult SetLanguage(string culture, string returnUrl)
        {  
            try
            {
                if (culture == "en")
                {
                    Response.Cookies.Append("Idiom", "English");
                }
                else
                {
                    Response.Cookies.Append("Idiom", "Español");
                }

                CookieOptions cookies = new CookieOptions();
                cookies.Expires = DateTime.Now.AddDays(1);
               
                Response.Cookies.Append(
                    CookieRequestCultureProvider.DefaultCookieName,
                    CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),                  
                    new CookieOptions { Expires = DateTimeOffset.UtcNow.AddDays(1) }
                );              
                return Json(new { success = true});
            }
            catch (Exception ex)
            {
                string messageError = _localizer["The language was changed to english."];
                return Json(new { success = false, error = ex.Message.ToString(), message = "messageError" });
            }
            //return LocalRedirect(returnUrl);
        }

        [HttpGet]
        public IActionResult SetSpanish()
        {
            Response.Cookies.Append("Idiom", "Español");
            CookieOptions cookies = new CookieOptions();
            cookies.Expires = DateTime.Now.AddDays(1);
            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture("es")),
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddDays(1) }
            );
            return RedirectToAction("Index","Home");
        }
        private string CreateBody(string name, string message)
        {
            string body = string.Empty;

            string rootFolder = Path.Combine(_webHostEnvironment.ContentRootPath, "Views", "Home");
            string fileName = "EmailTemplate.cshtml";
            string filePath = Path.Combine(rootFolder, fileName);
            using (StreamReader reader = new StreamReader(filePath))
            {
                body = reader.ReadToEnd();
            }
           
            body = body.Replace("{subject}", name);
            body = body.Replace("{message}", message);
            return body;
        }
       
        [HttpPost]
        public IActionResult Execl()
        {
            var excelCarrier = new ExcelMapper(@"C:\Temp\CarrierPhone.xlsx").Fetch<Carrier>();
            var count = excelCarrier.Count();
            List<Carrier> lCarriers = new List<Carrier>();
            var webClient = new System.Net.WebClient();
            string json = string.Empty;
        
            foreach (var item in excelCarrier)
            {        
                json = webClient.DownloadString("http://www.carrierlookup.com/index.php/api/lookup?key=bda67624bc1026d9f1f83c98faca2d363e5be4f9&number=" + item.number);
                var carrier = JsonConvert.DeserializeObject<Carrier>(json);
                lCarriers.Add(carrier);
            }

            //List<Carrier> lCarriers = new List<Carrier>();
            //string access_key = "62280dd7ba2060067dfde424c9ac5c78";
            //string number = "18099659415";
            //string country_code = "+13463318984";
            //string format = "1";
            //string url = "http://apilayer.net/api/validate?";
            //strGet = "http://apilayer.net/api/validate?access_key=62280dd7ba2060067dfde424c9ac5c78&number=18099659415&country_code=&format=1";
            //string strGet = url + "access_key=" + access_key + "&number=" + number + "&country_code=" + "&format=" + format;

            //var webClient = new System.Net.WebClient();
            //string json = webClient.DownloadString(strGet);
            //var carrier = JsonConvert.DeserializeObject<Carrier>(json);  
            //lCarriers.Add(carrier);

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Phone Carrier");
                var currentRow = 1;
                worksheet.Cell(currentRow, 1).Value = "Phone";
                worksheet.Cell(currentRow, 2).Value = "Country";
                worksheet.Cell(currentRow, 3).Value = "Carrier";
                worksheet.Cell(currentRow, 4).Value = "Line Type";

                foreach (Carrier item in lCarriers)
                {
                    currentRow++;
                    worksheet.Cell(currentRow, 1).Value = item.number;
                    worksheet.Cell(currentRow, 2).Value = item.country_name;
                    worksheet.Cell(currentRow, 3).Value = item.carrier;
                    worksheet.Cell(currentRow, 4).Value = item.line_type;
                }

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();
                    return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        "CarrierPhoneInfo.xlsx");
                }
            }

        }
        public IActionResult Email([Bind("To,Subject,Body")] Email em)
        {
            string to = em.To;
            string subject = em.Subject;
            string message = em.Body;
            string body = CreateBody(subject, message);

            MailMessage mm = new MailMessage();
            //MailMessage mm = new MailMessage(new MailAddress("palyfreya@gmail.com"), new MailAddress("palyfreya@gmail.com"));
            mm.BodyEncoding = Encoding.Default;
            mm.To.Add(to);
            mm.Subject = subject;
            mm.Body = body;
            mm.Priority = MailPriority.High;
            mm.From = new MailAddress("palyfreya@gmail.com");
            mm.IsBodyHtml = true;

            SmtpClient smtp = new SmtpClient();
      
            smtp.Host = "smtp.gmail.com";
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new System.Net.NetworkCredential("palyfreya@gmail.com", "Abcd1234.Abcd1234.");
            smtp.Port = 25; //alternative port number is 8889
            //smtp.UseDefaultCredentials = true;      
            smtp.EnableSsl = true;
            try
            {
                smtp.Send(mm);
               /* smtp.Dispose()*/;
                return Json(new { success = true, message = "Your email has been sent successfully." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, error= ex.Message.ToString(), message = "El correo no pudo ser enviado" });
            }
            
        }
        public IActionResult SendSMS()
        {
            const string accountSid = "ACd7ebdc5eddafc559f28f71f5a88b6c81";
            const string authToken = "15a981d9e1ee67c3353bc3d9e0a8ed98";

            TwilioClient.Init(accountSid, authToken);

            var message = MessageResource.Create(
                body: "Muy buenas Sr Martinez, quisieramos ofrecerle nuestros servicios, puede visitar nuestra pagina http://yaracena-001-site1.gtempurl.com/  siempre pensando en ustedes",
                from: new Twilio.Types.PhoneNumber("+14092351836"),
                to: new Twilio.Types.PhoneNumber("+13463318984")
            );
            return Json(new { success = true, message = "Su SMS fue enviado satisfactoriamente " + message.Sid +"." });
        }

        public ActionResult PayServices(int id)
        {
            var folderDetailsStates = Path.Combine(Directory.GetCurrentDirectory(), $"wwwroot\\{"jsonFiles\\states_titlecase.json"}");
            var folderDetailsMonths = Path.Combine(Directory.GetCurrentDirectory(), $"wwwroot\\{"jsonFiles\\Months.json"}");
            var JSONStates = System.IO.File.ReadAllText(folderDetailsStates);
            var JSONMonths = System.IO.File.ReadAllText(folderDetailsMonths);
            ViewBag.state =  Newtonsoft.Json.JsonConvert.DeserializeObject<States>(JSONStates);
            ViewBag.month = Newtonsoft.Json.JsonConvert.DeserializeObject<Months>(JSONMonths);
            ViewBag.service = _unitOfWork.Services.GetById(id);
            return View();
        }

        public ActionResult Login(int id)
        {
           
            return Json(new { success = true });
        }

        [HttpPost]
        public ActionResult AddClient(Clients client)
        {
            _unitOfWork.Clients.Insert(client);
            _unitOfWork.Save();
            return Json(new { success = true, message = "You can choose more services on our home portal",  clientName = _localizer["Thanks from CISEP"] + " " + client.First_name + " " + client.Last_name });
        }

        public ActionResult flexPay(string code)
        {
            try
            {
                var flex_Pay = _unitOfWork.Flex_Pay.GetById(code);
                _unitOfWork.Flex_Pay.Delete(flex_Pay);
                _unitOfWork.Save();
                return Json(new { data = flex_Pay.Amount, success=true, message= "You can now make your flexible payment." });
            }
            catch (Exception)
            {
                return Json(new { success = false, message = "You can now make your flexible payment." });
            }
            
        }
      
    }
}
