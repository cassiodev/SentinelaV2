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
    public class ItemController : MasterController
    {
        

        //
        // GET: /Item/

        public ActionResult Index(int? page)
        {


            int pageSize =  Convert.ToInt32(ConfigurationManager.AppSettings["PageSize"]);
            int pageNumber = (page ?? 1);



            return View(_Contexto.Item.OrderBy(i => i.ItemId).ToPagedList(pageNumber,pageSize));
        }

        //
        // GET: /Item/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Item/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Item item)
        {
            if (ModelState.IsValid)
            {
                _Contexto.Item.Add(item);
                _Contexto.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(item);
        }

        //
        // GET: /Item/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Item item = _Contexto.Item.Find(id);
            if (item == null)
            {
                return HttpNotFound();
            }
            return View(item);
        }

        //
        // POST: /Item/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Item item)
        {
            if (ModelState.IsValid)
            {
                _Contexto.Entry(item).State = EntityState.Modified;
                _Contexto.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(item);
        }

        //
        // GET: /Item/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Item item = _Contexto.Item.Find(id);
            if (item == null)
            {
                return HttpNotFound();
            }
            return View(item);
        }

        //
        // POST: /Item/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Item item = _Contexto.Item.Find(id);
            _Contexto.Item.Remove(item);
            _Contexto.SaveChanges();
            return RedirectToAction("Index");
        }

        
    }
}