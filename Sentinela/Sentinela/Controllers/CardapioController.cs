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
    public class CardapioController : MasterController
    {
        

        //
        // GET: /Cardapio/

        public ActionResult Index(int? page)
        {


            int pageSize =  Convert.ToInt32(ConfigurationManager.AppSettings["PageSize"]);
            int pageNumber = (page ?? 1);



            return View(_Contexto.Cardapio.Include("Imagem").OrderBy(c => c.CardapioId).ToPagedList(pageNumber,pageSize));
        }

        //
        // GET: /Cardapio/Create

        public ActionResult Create()
        {
            ViewBag.Refeicoes = _Contexto.Refeicao.Where(f => f.Ativo).ToList();
            ViewBag.Itens = _Contexto.Item.Where(f => f.Ativo).OrderBy(i => i.Nome).ToList();


            return View();
        }

        //
        // POST: /Cardapio/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Cardapio cardapio, FormCollection form)
        {

            cardapio.SetRefeicoes(_Contexto, form);

            if (ModelState.IsValid)
            {
                _Contexto.Cardapio.Add(cardapio);
                _Contexto.SaveChanges();
                TempData["message"] = "Adicionado com sucesso!";
                return RedirectToAction("Index");
            }

            return View(cardapio);
        }

        //
        // GET: /Cardapio/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Cardapio cardapio = _Contexto.Cardapio.Find(id);
            if (cardapio == null)
            {
                return HttpNotFound();
            }
            ViewBag.Refeicoes = _Contexto.Refeicao.Where(f => f.Ativo).ToList();
            ViewBag.Itens = _Contexto.Item.Where(f => f.Ativo).OrderBy(i => i.Nome).ToList();

            return View(cardapio);
        }

        //
        // POST: /Cardapio/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Cardapio cardapio, FormCollection form)
        {
            var _cardapio = _Contexto.Cardapio.Find(cardapio.CardapioId);

            _cardapio.CardapioRefeicaoItem.Clear();
            _cardapio.Ativo = cardapio.Ativo;
            _cardapio.Descricao = cardapio.Descricao;
            _cardapio.Nome = cardapio.Nome;

            _cardapio.SetRefeicoes(_Contexto, form);

            if (ModelState.IsValid)
            {
                _Contexto.SaveChanges();
                TempData["message"] = "Alteração feita com sucesso!";
                return RedirectToAction("Index");
            }
            return View(cardapio);
        }

        //
        // GET: /Cardapio/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Cardapio cardapio = _Contexto.Cardapio.Find(id);
            if (cardapio == null)
            {
                return HttpNotFound();
            }
            return View(cardapio);
        }

        //
        // POST: /Cardapio/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {


            Cardapio cardapio = _Contexto.Cardapio.Find(id);


            foreach (var imagem in cardapio.Imagem.ToList())
            {
                _Contexto.Imagem.Remove(imagem);
                string FileToDelete;
                // Set full path to file 
                string filename = imagem.Url.Split('/').Last();
                FileToDelete = Path.Combine(Server.MapPath("~/Images/"), Path.GetFileName(filename));
                // Delete a file
                System.IO.File.Delete(FileToDelete);
            }
            
            
            _Contexto.Cardapio.Remove(cardapio);
            _Contexto.SaveChanges();
            TempData["message"] = "Exclusão feita com sucesso!";
            return RedirectToAction("Index");
        }

        
    }
}