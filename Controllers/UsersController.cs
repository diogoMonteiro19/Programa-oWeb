using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TP.Data;

namespace TP.Controllers
{
    public class UsersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UsersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: UsersController
        public ActionResult Index()
        {
            var Gestores_funcs = _context.UserRoles.Where(i => i.RoleId.CompareTo("45698b3a-8dfe-4377-8950-3575fc3e1b10") == 0 || 
            (i.RoleId.CompareTo("e914d48a-8ca9-4643-86d3-e22b273ad63a") ==0)).Select(u => u.UserId);

            List<IdentityUser> proprietarios = new List<IdentityUser>();

            foreach (string n in Gestores_funcs)
            {
                proprietarios.Add(_context.Users.Where(u => u.Id.CompareTo(n) == 0).First());
            }
            var clientes = _context.UserRoles.Where(i => i.RoleId.CompareTo("2f1752f9-cb14-49fa-870e-9938c86d5c21") == 0).Select(u => u.UserId);
            List<IdentityUser> clis = new List<IdentityUser>();

            foreach (string n in clientes)
            {
                clis.Add(_context.Users.Where(u => u.Id.CompareTo(n) == 0).First());
            }

            ViewBag.listaclis = clis;
            ViewBag.listaProp = proprietarios;
            return View();
        }

        // GET: UsersController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: UsersController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: UsersController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: UsersController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: UsersController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: UsersController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: UsersController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
