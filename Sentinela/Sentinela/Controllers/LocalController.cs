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
using System.IO;

namespace Sentinela.Controllers
{
    [Authorize]
    public class LocalController : MasterController
    {
        

        //
        // GET: /Local/
        
        public ActionResult Index(int? page)
        {


            int pageSize =  Convert.ToInt32(ConfigurationManager.AppSettings["PageSize"]);
            int pageNumber = (page ?? 1);



            var local = _Contexto.Local.OrderBy(l => l.LocalId).Include(l => l.Cidade).Include("Imagem");
            return View(local.ToPagedList(pageNumber,pageSize));
        }

        //
        // GET: /Local/Detalhes/5

        public ActionResult Detalhes(int id = 0)
        {
            Local local = _Contexto.Local.Find(id);
            if (local == null)
            {
                return HttpNotFound();
            }
            return View(local);
        }

        //
        // GET: /Local/Create

        public ActionResult Create()
        {
            ViewBag.EstadoId = new SelectList(_Contexto.Estado, "EstadoId", "UF");
            return View();
        }

        //
        // POST: /Local/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Local local)
        {
            if (ModelState.IsValid)
            {
                _Contexto.Local.Add(local);
                _Contexto.SaveChanges();
                TempData["message"] = "Adicionado com sucesso!";
                return RedirectToAction("Index");
            }

            ViewBag.CidadeId = new SelectList(_Contexto.Cidade, "CidadeId", "Nome", local.CidadeId);
            return View(local);
        }

        //
        // GET: /Local/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Local local = _Contexto.Local.Find(id);
            if (local == null)
            {
                return HttpNotFound();
            }

            ViewBag.CidadeId = new SelectList(_Contexto.Cidade.Where(e => e.EstadoId == local.Cidade.EstadoId), "CidadeId", "Nome", local.CidadeId);
            ViewBag.EstadoId = new SelectList(_Contexto.Estado, "EstadoId", "UF", local.Cidade.EstadoId);
            return View(local);
        }

        //
        // POST: /Local/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Local local)
        {
            if (ModelState.IsValid)
            {
                _Contexto.Entry(local).State = EntityState.Modified;
                _Contexto.SaveChanges();
                TempData["message"] = "Alteração feita com sucesso!";
                return RedirectToAction("Index");
            }
            ViewBag.CidadeId = new SelectList(_Contexto.Cidade, "CidadeId", "Nome", local.CidadeId);
            return View(local);
        }

        //
        // GET: /Local/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Local local = _Contexto.Local.Find(id);
            if (local == null)
            {
                return HttpNotFound();
            }
            return View(local);
        }

        //
        // POST: /Local/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Local local = _Contexto.Local.Include("Imagem").FirstOrDefault(f=>f.LocalId==id);
            
            foreach (var imagem in local.Imagem.ToList())
            {
                _Contexto.Imagem.Remove(imagem);
                string FileToDelete;
                // Set full path to file 
                string filename = imagem.Url.Split('/').Last();
                FileToDelete = Path.Combine(Server.MapPath("~/Images/"), Path.GetFileName(filename));
                // Delete a file
                System.IO.File.Delete(FileToDelete);
            }

            _Contexto.Local.Remove(local);
            
            _Contexto.SaveChanges();
            TempData["message"] = "Exclusão feita com sucesso!";
            return RedirectToAction("Index");
        }

        
    }
}