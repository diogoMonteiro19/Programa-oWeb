using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TP.Data;
using TP.Models;

namespace TP.Controllers
{
    public class GestoresController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private List<Funcionario> funcionarios;
        private readonly ApplicationDbContext _context;
        public GestoresController(UserManager<IdentityUser> userManager,ApplicationDbContext context)
        {
            _userManager = userManager;
            _context =context;
            funcionarios = new List<Funcionario>();
            
        }
        // GET: GestoresController
        public ActionResult Index()
        {
            var userID = _context.Users.Where(u => u.Email.CompareTo(User.Identity.Name) == 0).First().Id;

            funcionarios = _context.Funcionarios.Where(f => f.GestorId == userID).ToList();

            ViewBag.listaFunc = funcionarios;
            return View();
        }

        // GET: GestoresController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: GestoresController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: GestoresController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateAsync([Bind("FuncionarioId,email,IdentityUserId,GestorId")] Funcionario funcionario)
        {
            try
            {
                var user = new IdentityUser
                {
                    UserName = funcionario.email,
                    Email = funcionario.email,
                };

                
                var result = await _userManager.CreateAsync(user, "Password1$");
                await _userManager.AddToRoleAsync(user, "Funcionario");
                var aux = _context.Users.Where(u => u.UserName.CompareTo(User.Identity.Name) == 0).First();
                funcionario.GestorId = aux.Id;
                _context.Add(funcionario);
                await _context.SaveChangesAsync();
                funcionarios.Add(funcionario);
                ViewBag.listaFunc = funcionarios;
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: GestoresController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: GestoresController/Edit/5
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

        // GET: GestoresController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: GestoresController/Delete/5
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
