using ImageWebService.Communication;
using ImageWebService.Models;
using System.Web.Mvc;

namespace ImageWebService.Controllers
{
    public class HomeController : Controller
    {
        
        private static LogsModel l_model = new LogsModel();
        private static ConfigModel c_model = new ConfigModel();
        private static MainModel m_model = new MainModel();

        [HttpGet]
        public ActionResult Index()
        {
            return View(m_model);
        }

        [HttpGet]
        public ActionResult Logs()
        {
            return View(l_model.ServiceLogs);
        }

        [HttpPost]
        public ActionResult LogsByType(FormCollection fc)
        {
            string log_type = fc["LogType"].ToString();
            l_model.FilterLogsByType(log_type);
            return RedirectToAction("Logs");
        }

        public ActionResult RemoveConfirm(string handler)
        {
            ViewBag.handler = handler;
            return View();
        }

        [HttpPost]
        public ActionResult RemoveHandler(string handler)
        {
            c_model.RemoveHandler(handler);
            return RedirectToAction("Config");
        }

        [HttpGet]
        public ActionResult Config()
        {
            return View(c_model);
        }
    }
}