using Sentinela.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Sentinela.Controllers
{
    public class ContaController : MasterController
    {
        //
        // GET: /Conta/

        [HttpGet]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.returnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        public ActionResult Login(Usuario model, string returnUrl)
        {
            model = model.Autentica(_Contexto);
            if (model != null)
            {
                FormsAuthentication.SetAuthCookie(model.Login, false);
                if (Url.IsLocalUrl(returnUrl))
                {
                    return Redirect(returnUrl);
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }

            ViewBag.returnUrl = returnUrl;
            ModelState.AddModelError("", "Login ou senha incorretos.");
            return View(model);
        }

        [HttpGet]
        [Authorize]
        public ActionResult Logoff()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }
    }
}
