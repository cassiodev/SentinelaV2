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
    public class UsuarioController : MasterController
    {
        

        //
        // GET: /Usuario/

        public ActionResult Index(int? page)
        {


            int pageSize =  Convert.ToInt32(ConfigurationManager.AppSettings["PageSize"]);
            int pageNumber = (page ?? 1);



            var usuario = _Contexto.Usuario.OrderBy(u => u.UsuarioId).Include(u => u.Pessoa);
            return View(usuario.ToPagedList(pageNumber,pageSize));
        }

        //
        // GET: /Usuario/Details/5

        public ActionResult Details(int id = 0)
        {
            Usuario usuario = _Contexto.Usuario.Find(id);
            if (usuario == null)
            {
                return HttpNotFound();
            }
            return View(usuario);
        }

        //
        // GET: /Usuario/Create

        public ActionResult Create()
        {
            ViewBag.UsuarioId = new SelectList(_Contexto.Pessoa, "PessoaId", "Email");
            return View();
        }

        //
        // POST: /Usuario/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                _Contexto.Usuario.Add(usuario);
                _Contexto.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.UsuarioId = new SelectList(_Contexto.Pessoa, "PessoaId", "Email", usuario.UsuarioId);
            return View(usuario);
        }

        //
        // GET: /Usuario/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Usuario usuario = _Contexto.Usuario.Find(id);
            if (usuario == null)
            {
                return HttpNotFound();
            }
            ViewBag.UsuarioId = new SelectList(_Contexto.Pessoa, "PessoaId", "Email", usuario.UsuarioId);
            return View(usuario);
        }

        //
        // POST: /Usuario/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Usuario usuario)
        {

            ModelState["Senha"].Errors.Clear();
            
            if (ModelState.IsValid)
            {
                var original = _Contexto.Usuario.Find(usuario.UsuarioId);

                original.Login = usuario.Login;
                original.Ativo = usuario.Ativo;
                original.Pessoa.Nome = usuario.Pessoa.Nome;
                original.Pessoa.Email = usuario.Pessoa.Email;

                _Contexto.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.UsuarioId = new SelectList(_Contexto.Pessoa, "PessoaId", "Email", usuario.UsuarioId);
            return View(usuario);
        }

        //
        // GET: /Usuario/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Usuario usuario = _Contexto.Usuario.Find(id);
            if (usuario == null)
            {
                return HttpNotFound();
            }
            return View(usuario);
        }

        //
        // POST: /Usuario/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Usuario usuario = _Contexto.Usuario.Find(id);
            _Contexto.Usuario.Remove(usuario);
            _Contexto.SaveChanges();
            return RedirectToAction("Index");
        }

        
    }
}