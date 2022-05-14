using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TP.Data;
using TP.Models;

namespace TP.Controllers
{
    public class ImovelsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ImovelsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Imovels
        public async Task<IActionResult> Index()
        {
            if (User.IsInRole("Cliente"))
            {
                return View(await _context.Imoveis.ToListAsync());
            }
            else if (User.IsInRole("Gestor"))
            {
                var listImov = _context.Imoveis.Where(i => i.Gestor.UserName.CompareTo(User.Identity.Name) == 0);
                return View(await listImov.ToListAsync());
            }
            else if (User.IsInRole("Funcionario"))
            {
                var func_atual = _context.Funcionarios.Where(u => u.email.CompareTo(User.Identity.Name) == 0).First();
                var listimov = _context.Imoveis.Where(i => i.GestorId.CompareTo(func_atual.GestorId) == 0);
                return View(await listimov.ToListAsync());
            }
            return View(await _context.Imoveis.ToListAsync());
        }

        // GET: Imovels/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var imovel = await _context.Imoveis
                .FirstOrDefaultAsync(m => m.ImovelId == id);
            if (imovel == null)
            {
                return NotFound();
            }

            var listaAval = await  _context.Avaliacoes.Where(a => a.ImovelId == id).ToListAsync();
            ViewBag.listaAval = listaAval;
            return View(imovel);
        }

        // GET: Imovels/Create
        [Authorize(Roles = "Gestor,Admin")]
        public IActionResult Create()
        {
            List<string> listaC = new List<string>();
            listaC.Add("Apartamento");
            listaC.Add("Moradia");
            listaC.Add("Estudio");
            listaC.Add("Bungalow");
            ViewBag.categorias = listaC;
            return View();
        }

        // POST: Imovels/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ImovelId,Descricao,Preco,Categoria")] Imovel imovel)
        {
            if (ModelState.IsValid)
            {
                var aux = _context.Users.Where(u => u.UserName.CompareTo(User.Identity.Name) == 0).First();
                imovel.Gestor = aux;
                _context.Add(imovel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(imovel);
        }

        // GET: Imovels/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var imovel = await _context.Imoveis.FindAsync(id);
            if (imovel == null)
            {
                return NotFound();
            }
            return View(imovel);
        }

        // POST: Imovels/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ImovelId,Descricao,Preco")] Imovel imovel)
        {
            if (id != imovel.ImovelId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(imovel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ImovelExists(imovel.ImovelId))
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
            return View(imovel);
        }

        // GET: Imovels/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var imovel = await _context.Imoveis
                .FirstOrDefaultAsync(m => m.ImovelId == id);
            if (imovel == null)
            {
                return NotFound();
            }

            return View(imovel);
        }

        // POST: Imovels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var reservas = await _context.Reservas.Where(r => r.ImovelId == id).ToListAsync();
            _context.Reservas.RemoveRange(reservas);
            var avaliacoes = await _context.Avaliacoes.Where(a => a.ImovelId == id).ToListAsync();
            _context.Avaliacoes.RemoveRange(avaliacoes);
            var imovel = await _context.Imoveis.FindAsync(id);
            _context.Imoveis.Remove(imovel);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ImovelExists(int id)
        {
            return _context.Imoveis.Any(e => e.ImovelId == id);
        }
        
    }
}
