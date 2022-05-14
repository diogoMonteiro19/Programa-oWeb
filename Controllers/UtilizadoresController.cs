using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TP.Data;
using TP.Models;

namespace TP.Controllers
{
    public class UtilizadoresController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UtilizadoresController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Utilizadores
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Utilizadores.Include(u => u.User);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Utilizadores/Details/5
        public async Task<IActionResult> Details(string? id)
        {

            if (id == null)
            {
                return NotFound();
            }
            Utilizador utilizador = new Utilizador();

            if (User.IsInRole("Gestor") || User.IsInRole("Funcionario") || User.IsInRole("Admin"))
            {
                 utilizador = await _context.Utilizadores
                    .FirstOrDefaultAsync(m => m.IdentityUserId.CompareTo(id) == 0);
                if (utilizador == null)
                {
                    return NotFound();
                }

                var avaliacoes = await _context.AvalicoesClientes.Where(a => a.ClienteId.CompareTo(utilizador.IdentityUserId) == 0).ToListAsync();
                ViewBag.la = avaliacoes;
            }
            if (User.IsInRole("Cliente"))
            {
                utilizador = await _context.Utilizadores
                    .FirstOrDefaultAsync(m => m.email.CompareTo(id) == 0);
                if (utilizador == null)
                {
                    return NotFound();
                }

                var avaliacoes = await _context.AvalicoesClientes.Where(a => a.ClienteId.CompareTo(utilizador.IdentityUserId) == 0).ToListAsync();
                ViewBag.la = avaliacoes;
                List<string> listaprop = new List<string>();

                foreach(var item in avaliacoes)
                {
                    var gestor = _context.Users.Where(u => u.Id.CompareTo(item.GestorId)==0).First().UserName;
                    listaprop.Add(gestor);
                }
                ViewBag.lg = listaprop;
            }


            return View(utilizador);
        }

        // GET: Utilizadores/Create
        public IActionResult Create()
        {
            ViewData["IdentityUserId"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }

        // POST: Utilizadores/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UtilizadorId,email,IdentityUserId")] Utilizador utilizador)
        {
            if (ModelState.IsValid)
            {
                _context.Add(utilizador);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdentityUserId"] = new SelectList(_context.Users, "Id", "Id", utilizador.IdentityUserId);
            return View(utilizador);
        }

        // GET: Utilizadores/Edit/5
        public async Task<IActionResult> Edit(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var utilizador = await _context.Utilizadores.Where(u=> u.IdentityUserId.CompareTo(id)==0).FirstAsync();
            if (utilizador == null)
            {
                return NotFound();
            }
            ViewData["IdentityUserId"] = new SelectList(_context.Users, "Id", "Id", utilizador.IdentityUserId);
            return View(utilizador);
        }

        // POST: Utilizadores/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("UtilizadorId,email,IdentityUserId")] Utilizador utilizador)
        {
            if (id != utilizador.UtilizadorId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (Utilizador.IsValidEmail(utilizador.email))
                    {
                        _context.Update(utilizador);
                        var user = _context.Users.Where(u => u.Id.CompareTo(utilizador.IdentityUserId) == 0).First();
                        user.UserName = utilizador.email;
                        user.Email = utilizador.email;
                        _context.Update(user);
                        await _context.SaveChangesAsync();
                    }
                    
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UtilizadorExists(utilizador.UtilizadorId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdentityUserId"] = new SelectList(_context.Users, "Id", "Id", utilizador.IdentityUserId);
            return View(utilizador);
        }

        // GET: Utilizadores/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var utilizador = await _context.Utilizadores
                .Include(u => u.User)
                .FirstOrDefaultAsync(m => m.UtilizadorId == id);
            if (utilizador == null)
            {
                return NotFound();
            }

            return View(utilizador);
        }

        // POST: Utilizadores/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var utilizador = await _context.Utilizadores.FindAsync(id);
            _context.Utilizadores.Remove(utilizador);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UtilizadorExists(int id)
        {
            return _context.Utilizadores.Any(e => e.UtilizadorId == id);
        }
    }
}
