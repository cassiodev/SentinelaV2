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
        // GET: /Usuario/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Usuario/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(UsuarioModelView usuario)
        {
            if (_Contexto.Pessoa.Any(c => c.Email.Equals(usuario.Email)))
                ModelState.AddModelError("Email", "E-mail já cadastrado!");
            if (_Contexto.Usuario.Any(c => c.Login.ToLower().Equals(usuario.Login.ToLower())))
                ModelState.AddModelError("Login", "Login já cadastrado!");

            if (ModelState.IsValid)
            {

                var original = new Usuario();
                original.Pessoa = new Pessoa();

                original.Login = usuario.Login.ToLower();
                original.Ativo = usuario.Ativo;
                original.Pessoa.Nome = usuario.Nome;
                original.Pessoa.Email = usuario.Email;
                original.Senha = usuario.Senha;
                original.Pessoa.PessoaId = usuario.PessoaId;
                
                _Contexto.Usuario.Add(original);
                _Contexto.SaveChanges();
                TempData["message"] = "Adicionado com sucesso!";
                return RedirectToAction("Index");
            }

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

            var original = new UsuarioModelView();

            original.Login = usuario.Login;
            original.Ativo = usuario.Ativo;
            original.Nome = usuario.Pessoa.Nome;
            original.Email = usuario.Pessoa.Email;
            original.Senha = usuario.Senha;
            original.ConfirmaSenha = usuario.Senha;
            original.UsuarioId = usuario.UsuarioId;
            original.PessoaId = usuario.UsuarioId;

            return View(original);
        }

        //
        // POST: /Usuario/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(UsuarioModelView usuario)
        {

            if (_Contexto.Pessoa.Any(c => c.Email.Equals(usuario.Email) && c.PessoaId != usuario.UsuarioId))
                ModelState.AddModelError("Email", "E-mail já cadastrado!");
            if (_Contexto.Usuario.Any(c => c.Login.ToLower().Equals(usuario.Login.ToLower()) && c.Pessoa.PessoaId != usuario.UsuarioId))
                ModelState.AddModelError("Login", "Login já cadastrado!");

            if (ModelState.IsValid)
            {
                var original = _Contexto.Usuario.Find(usuario.UsuarioId);

                original.Login = usuario.Login.ToLower();
                original.Ativo = usuario.Ativo;
                original.Pessoa.Nome = usuario.Nome;
                original.Pessoa.Email = usuario.Email;
                original.Senha = usuario.Senha;
                

                _Contexto.SaveChanges();
                TempData["message"] = "Alteração feita com sucesso!";
                return RedirectToAction("Index");
            }
            return View(usuario);
        }

        //
        // GET: /Usuario/Delete/5

        public ActionResult Delete(int id = 0)
        {
            ViewBag.Usuarios = _Contexto.Usuario.Count();
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
            TempData["message"] = "Exclusão feita com sucesso!";
            return RedirectToAction("Index");
        }

        
    }
}