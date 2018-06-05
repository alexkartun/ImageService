using ImageWebService.Communication;
using ImageWebService.Models;
using System.Web.Mvc;

namespace ImageWebService.Controllers
{
    public class HomeController : Controller
    {
        
        private static Logs l_model = new Logs();
        private static Config c_model = new Config();
        private static ImageWebModel w_model = new ImageWebModel();

        public ActionResult Index()
        {
            w_model.SetServiceStatus();
            w_model.SetStudents();
            w_model.SetNumberOfPhotos();
            return View(w_model);
        }

        public ActionResult Logs()
        {
            return View(l_model);
        }

        public ActionResult Config()
        {
            c_model.SetConfigData();
            return View(c_model);
        }

        public ActionResult LogsByType(Logs model)
        {
            model.SetLogsByType();
            l_model.ServiceLogs = model.ServiceLogs;
            return RedirectToAction("Logs");
        }
    }
}