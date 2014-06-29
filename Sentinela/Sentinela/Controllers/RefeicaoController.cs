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

namespace Sentinela.Controllers
{
    [Authorize]
    public class RefeicaoController : MasterController
    {
        

        //
        // GET: /Refeicao/

        public ActionResult Index(int? page)
        {


            int pageSize =  Convert.ToInt32(ConfigurationManager.AppSettings["PageSize"]);
            int pageNumber = (page ?? 1);



            var refeicao = _Contexto.Refeicao.OrderBy(r => r.RefeicaoId);
            return View(refeicao.ToPagedList(pageNumber,pageSize));
        }

        //
        // GET: /Refeicao/Create

        public ActionResult Create()
        {
            ViewBag.CardapioId = new SelectList(_Contexto.Cardapio, "CardapioId", "Nome");
            return View();
        }

        //
        // POST: /Refeicao/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Refeicao refeicao)
        {
            if (ModelState.IsValid)
            {
                _Contexto.Refeicao.Add(refeicao);
                _Contexto.SaveChanges();
                TempData["message"] = "Adicionado com sucesso!";
                return RedirectToAction("Index");
            }

            
            return View(refeicao);
        }

        //
        // GET: /Refeicao/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Refeicao refeicao = _Contexto.Refeicao.Find(id);
            if (refeicao == null)
            {
                return HttpNotFound();
            }
            return View(refeicao);
        }

        //
        // POST: /Refeicao/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Refeicao refeicao)
        {
            if (ModelState.IsValid)
            {
                _Contexto.Entry(refeicao).State = EntityState.Modified;
                _Contexto.SaveChanges();
                TempData["message"] = "Alteração feita com sucesso!";
                return RedirectToAction("Index");
            }
            return View(refeicao);
        }

        //
        // GET: /Refeicao/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Refeicao refeicao = _Contexto.Refeicao.Find(id);
            if (refeicao == null)
            {
                return HttpNotFound();
            }
            return View(refeicao);
        }

        //
        // POST: /Refeicao/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Refeicao refeicao = _Contexto.Refeicao.Find(id);
            _Contexto.Refeicao.Remove(refeicao);
            _Contexto.SaveChanges();
            TempData["message"] = "Exclusão feita com sucesso!";
            return RedirectToAction("Index");
        }

        
    }
}