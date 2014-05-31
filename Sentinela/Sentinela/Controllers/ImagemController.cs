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
    public class ImagemController : MasterController
    {
        //
        // GET: /Imagem/
        [HttpGet]
        public ActionResult Index(int? id, string entidade)
        {
            switch (entidade)
            {
                case "local":
                    var local = _Contexto.Local.Include("Imagem").FirstOrDefault(i => i.LocalId == id);
                    ViewBag.Classe = entidade;
                    ViewBag.Titulo = local.Nome;
                    ViewBag.ItemId = local.LocalId;
                    ViewBag.Qtd = local.Imagem.Count;
                    return View(local.Imagem.ToList());
                case "cardapio":
                    var cardapio = _Contexto.Cardapio.Include("Imagem").FirstOrDefault(i => i.CardapioId == id);
                    ViewBag.Classe = entidade;
                    ViewBag.Titulo = cardapio.Nome;
                    ViewBag.ItemId = cardapio.CardapioId;
                    ViewBag.Qtd = cardapio.Imagem.Count;
                    return View(cardapio.Imagem.ToList());
                default:
                    return new EmptyResult();
            }
        }

        //
        // POST: /Imagem/Create

        [HttpPost]
       
        public ActionResult Create(int? id, string entidade)
        {
            foreach (string file in Request.Files)
            {
                HttpPostedFileBase arquivo = Request.Files[file] as HttpPostedFileBase;
                string savedFileName = Path.Combine(Server.MapPath("~/Images/"), Path.GetFileName(arquivo.FileName));
                Imagem amostra = new Imagem() { Url = "/Images/" + Path.GetFileName(arquivo.FileName) };
                switch (entidade)
                {
                    case "local":
                        amostra.LocalId = id;
                        
                        break;
                    case "cardapio":
                        amostra.CardapioId = id;
                        break;
                    default:
                        break;
                }

                _Contexto.Imagem.Add(amostra);
                arquivo.SaveAs(savedFileName);
            }
            _Contexto.SaveChanges();
            return RedirectToAction("Index", new { id = id, entidade = entidade });
        }

        //
        // POST: /Imagem/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Imagem imagem = _Contexto.Imagem.Find(id);
            _Contexto.Imagem.Remove(imagem);
            _Contexto.SaveChanges();

            string FileToDelete;
            // Set full path to file 
            string filename = imagem.Url.Split('/').Last();
            FileToDelete = Path.Combine(Server.MapPath("~/Images/"), Path.GetFileName(filename));
            // Delete a file
            System.IO.File.Delete(FileToDelete);
            return Json(new {msg = "Foto removida com sucesso", erro = false });
        }

        
    }
}