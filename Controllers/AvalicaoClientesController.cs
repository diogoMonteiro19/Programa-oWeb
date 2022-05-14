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
    public class AvalicaoClientesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AvalicaoClientesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: AvalicaoClientes
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.AvalicoesClientes.Include(a => a.Cliente).Include(a => a.Gestor);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: AvalicaoClientes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var avalicaoCliente = await _context.AvalicoesClientes
                .Include(a => a.Cliente)
                .Include(a => a.Gestor)
                .FirstOrDefaultAsync(m => m.AvalicaoClienteId == id);
            if (avalicaoCliente == null)
            {
                return NotFound();
            }

            return View(avalicaoCliente);
        }

        // GET: AvalicaoClientes/Create
        public IActionResult Create(string? id)
        {
            ViewBag.Gestor = _context.Users.Where(u => u.UserName.CompareTo(User.Identity.Name) == 0).First().Id;
            ViewBag.User = _context.Users.Where(u=> u.Id.CompareTo(id)==0).First().Id;
            return View();
        }

        // POST: AvalicaoClientes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AvalicaoClienteId,Comentario,Pontuacao,ClienteId,GestorId")] AvalicaoCliente avalicaoCliente)
        {
            if (ModelState.IsValid)
            {
                var gestorID = _context.Users.Where(u => u.UserName.CompareTo(User.Identity.Name) == 0).First().Id;
                avalicaoCliente.GestorId = gestorID;
                _context.Add(avalicaoCliente);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ClienteId"] = new SelectList(_context.Users, "Id", "Id", avalicaoCliente.ClienteId);
            ViewData["GestorId"] = new SelectList(_context.Users, "Id", "Id", avalicaoCliente.GestorId);
            return View(avalicaoCliente);
        }

        // GET: AvalicaoClientes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var avalicaoCliente = await _context.AvalicoesClientes.FindAsync(id);
            if (avalicaoCliente == null)
            {
                return NotFound();
            }
            ViewData["ClienteId"] = new SelectList(_context.Users, "Id", "Id", avalicaoCliente.ClienteId);
            ViewData["GestorId"] = new SelectList(_context.Users, "Id", "Id", avalicaoCliente.GestorId);
            return View(avalicaoCliente);
        }

        // POST: AvalicaoClientes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("AvalicaoClienteId,Comentario,Pontuacao,ClienteId,GestorId")] AvalicaoCliente avalicaoCliente)
        {
            if (id != avalicaoCliente.AvalicaoClienteId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(avalicaoCliente);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AvalicaoClienteExists(avalicaoCliente.AvalicaoClienteId))
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
            ViewData["ClienteId"] = new SelectList(_context.Users, "Id", "Id", avalicaoCliente.ClienteId);
            ViewData["GestorId"] = new SelectList(_context.Users, "Id", "Id", avalicaoCliente.GestorId);
            return View(avalicaoCliente);
        }

        // GET: AvalicaoClientes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var avalicaoCliente = await _context.AvalicoesClientes
                .Include(a => a.Cliente)
                .Include(a => a.Gestor)
                .FirstOrDefaultAsync(m => m.AvalicaoClienteId == id);
            if (avalicaoCliente == null)
            {
                return NotFound();
            }

            return View(avalicaoCliente);
        }

        // POST: AvalicaoClientes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var avalicaoCliente = await _context.AvalicoesClientes.FindAsync(id);
            _context.AvalicoesClientes.Remove(avalicaoCliente);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AvalicaoClienteExists(int id)
        {
            return _context.AvalicoesClientes.Any(e => e.AvalicaoClienteId == id);
        }
    }
}
