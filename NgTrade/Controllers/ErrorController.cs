using System;
using System.Web.Mvc;
using NgTrade.Models.Info;

namespace NgTrade.Controllers
{
    public class ErrorController : Controller
    {
        public ActionResult Index(int statusCode, Exception exception, bool isAjaxRequet)
        {
            Response.StatusCode = statusCode;
            if (!isAjaxRequet)
            {
                var model = new ErrorModel { HttpStatusCode = statusCode, Exception = exception };
                return View(model);
            }
            var errorObjet = new { message = exception.Message };
            return Json(errorObjet, JsonRequestBehavior.AllowGet);
        }
    }
}
