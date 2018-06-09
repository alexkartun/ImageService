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
        private static PhotosModel p_model = new PhotosModel();

        private WebChannel channel;

        public HomeController()
        {
            channel = new WebChannel();
        }
        /// <summary>
        /// Set all main window data. [HttpGet] Request.
        /// </summary>
        /// <returns>Index view.</returns>
        public ActionResult Index()
        {
            m_model.SetData(channel);
            return View(m_model);
        }

        /// <summary>
        /// Set all service photos. [HttpGet] Request.
        /// </summary>
        /// <returns>Photos view.</returns>
        public ActionResult Photos()
        {
            p_model.SetServicePhotos(channel);
            return View(p_model);
        }

        /// <summary>
        /// Set all logs. [HttpGet] Request.
        /// </summary>
        /// <returns>Logs view.</returns>
        public ActionResult Logs()
        {
            l_model.SetLogs(channel);
            return View(l_model.ServiceLogs);
        }

        /// <summary>
        /// Set all logs by type. [HttpPost] Request.
        /// </summary>
        /// <param name="fc">Form collection that include the chosen type.</param>
        /// <returns>Logs view with filtered logs.</returns>
        [HttpPost]
        public ActionResult LogsByType(FormCollection fc)
        {
            string log_type = fc["LogType"].ToString();
            l_model.FilterLogsByType(log_type, channel);
            return View("Logs", l_model.ServiceLogs);
        }

        /// <summary>
        /// Remove handler confirmation. [HttpGet] Request.
        /// </summary>
        /// <param name="handler">Handler to be removed.</param>
        /// <returns>Remove handler confirmation view.</returns>
        public ActionResult RemoveHandlerConfirm(string handler)
        {
            ViewBag.handler = handler;
            return View();
        }

        /// <summary>
        /// Redirect to ViewPhoto view. [HttpGet] Request.
        /// </summary>
        /// <param name="path">Photo path.</param>
        /// <param name="name">Photo name.</param>
        /// <param name="month">Photo month.</param>
        /// <param name="year">Photo year.</param>
        /// <returns></returns>
        public ActionResult ViewPhoto(string path, string name, string month, string year)
        {
            ViewBag.path = path;
            ViewBag.name = name;
            ViewBag.month = month;
            ViewBag.year = year;
            return View();
        }

        /// <summary>
        /// Redirect to DeletePhotoConfirm view. [HttpGet] Request.
        /// </summary>
        /// <param name="iPath">Image path.</param>
        /// <param name="tPath">Thumbnail path.</param>
        /// <param name="name">Image name.</param>
        /// <returns>DeletePhotoConfirm view.</returns>
        public ActionResult DeletePhotoConfirm(string iPath, string tPath, string name)
        {
            ViewBag.imagePath = iPath;
            ViewBag.thumbnailPath = tPath;
            ViewBag.name = name;
            return View();
        }

        /// <summary>
        /// Remove handler. [HttpPost] Request.
        /// </summary>
        /// <param name="handler">Handler to remove.</param>
        [HttpPost]
        public void RemoveHandler(string handler)
        {
            c_model.RemoveHandler(handler, channel);
        }

        /// <summary>
        /// Remove image & thumbnail. [HttpPost] Request.
        /// </summary>
        /// <param name="iPath">Image path</param>
        /// <param name="tPath">Thumbnail path.</param>
        [HttpPost]
        public void RemoveImage(string iPath, string tPath)
        {
            p_model.RemoveImage(iPath, tPath, channel);
        }

        /// <summary>
        /// Set all config data. [HttpGet] Request.
        /// </summary>
        /// <returns>Config view.</returns>
        public ActionResult Config()
        {
            c_model.SetConfigData(channel);
            return View(c_model);
        }
    }
}