using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList.Mvc;
using PagedList;
using System.Configuration;
using Sentinela.Models;
using Sentinela.Core;

namespace Sentinela.Controllers
{
    [Authorize]
    public class AdicionalController : MasterController
    {
        

        //
        // GET: /Adicional/

        public ActionResult Index(int? page)
        {
            

            int pageSize =  Convert.ToInt32(ConfigurationManager.AppSettings["PageSize"]);
            int pageNumber = (page ?? 1);


            return View(_Contexto.Adicional.OrderBy(a => a.AdicionalId).ToPagedList(pageNumber,pageSize));
        }

        //
        // GET: /Adicional/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Adicional/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Adicional adicional)
        {
            if (ModelState.IsValid)
            {
                _Contexto.Adicional.Add(adicional);
                _Contexto.SaveChanges();
                TempData["message"] = "Adicionado com sucesso!";
                return RedirectToAction("Index");
            }

            return View(adicional);
        }

        //
        // GET: /Adicional/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Adicional adicional = _Contexto.Adicional.Find(id);
            if (adicional == null)
            {
                return HttpNotFound();
            }
            return View(adicional);
        }

        //
        // POST: /Adicional/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Adicional adicional)
        {
            if (ModelState.IsValid)
            {
                _Contexto.Entry(adicional).State = EntityState.Modified;
                _Contexto.SaveChanges();
                TempData["message"] = "Alteração feita com sucesso!";
                return RedirectToAction("Index");
            }
            return View(adicional);
        }

        //
        // GET: /Adicional/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Adicional adicional = _Contexto.Adicional.Find(id);
            if (adicional == null)
            {
                return HttpNotFound();
            }
            return View(adicional);
        }

        //
        // POST: /Adicional/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Adicional adicional = _Contexto.Adicional.Find(id);
            _Contexto.Adicional.Remove(adicional);
            _Contexto.SaveChanges();
            TempData["message"] = "Exclusão feita com sucesso!";
            return RedirectToAction("Index");
        }

        
    }
}