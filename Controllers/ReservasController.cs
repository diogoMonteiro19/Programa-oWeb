using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TP.Data;
using TP.Models;

namespace TP.Controllers
{
    public class ReservasController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ReservasController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Reservas
        [Authorize(Roles ="Cliente,Gestor,Funcionario")]
        public async Task<IActionResult> Index()
        {
            

            if (User.IsInRole("Cliente"))
            {
                var applicationDbContext=_context.Reservas.Include(r => r.Imovel).Include(r => r.User).Where(r => r.User.Email == User.Identity.Name);
                return View(await applicationDbContext.ToListAsync());
            }
            else if(User.IsInRole("Gestor"))
            {
                var gestorID = _context.Users.Where(u => u.UserName.CompareTo(User.Identity.Name) == 0).Select(g => g.Id).First();
                var listImov = _context.Imoveis.Where(i => i.GestorId == gestorID).ToList();
                List<Reserva> listaR = new List<Reserva>();
                
                foreach (Imovel im in listImov )
                {
                    var alguma = _context.Reservas.Where(r => r.ImovelId == im.ImovelId).ToList();
                    listaR.AddRange(alguma);
                }
                List<IdentityUser> ListU = new List<IdentityUser>();
                foreach (Reserva reserva in listaR)
                {
                    var aux = _context.Users.Where(u => u.Id.CompareTo(reserva.IdentityUserId) == 0).First();
                    ListU.Add(aux);
                }
                ViewBag.listaUsers = ListU;
                return View(listaR);
            }
            else if (User.IsInRole("Funcionario"))
            {
                var func_atual = _context.Funcionarios.Where(u => u.email.CompareTo(User.Identity.Name) == 0).First();

                var listImov = _context.Imoveis.Where(i => i.GestorId == func_atual.GestorId).ToList();
                List<Reserva> listaR = new List<Reserva>();

                foreach (Imovel im in listImov)
                {
                    var alguma = _context.Reservas.Where(r => r.ImovelId == im.ImovelId).ToList();
                    listaR.AddRange(alguma);
                }
                List<IdentityUser> ListU = new List<IdentityUser>();
                foreach (Reserva reserva in listaR)
                {
                    var aux = _context.Users.Where(u => u.Id.CompareTo(reserva.IdentityUserId) == 0).First();
                    ListU.Add(aux);
                }
                ViewBag.listaUsers = ListU;
                return View(listaR);
            }
            return View();
        }

        // GET: Reservas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reserva = await _context.Reservas
                .Include(r => r.Imovel)
                .Include(r => r.User)
                .FirstOrDefaultAsync(m => m.ReservaId == id);
            if (reserva == null)
            {
                return NotFound();
            }

            return View(reserva);
        }

        // GET: Reservas/Create
        //public IActionResult Create(int? id)
        //{
        //    ViewData["ImovelId"] = new SelectList(_context.Imoveis, "ImovelId", "ImovelId");
        //    ViewData["IdentityUserId"] = new SelectList(_context.Users, "Id", "Id");
        //    return View();
        //}
        [Authorize(Roles = "Cliente,Admin")]
        public IActionResult Create(int? id)
        {
            ViewBag.imovDescr = _context.Imoveis.Where(i=> i.ImovelId == id).First().Descricao;
            ViewBag.ImovelId = id;
            return View();
        }

        // POST: Reservas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ReservaId,dataEntrada,dataSaida,ImovelId,IdentityUserId")] Reserva reserva)
        {
            if (ModelState.IsValid)
            {
                var aux = _context.Users.Where(u => u.UserName.CompareTo(User.Identity.Name) == 0).First();
                reserva.User = aux;
                var reservas = _context.Reservas.Where(r => r.ImovelId == reserva.ImovelId);
                foreach(Reserva r in reservas)
                {
                    if(r.dataEntrada <= reserva.dataSaida && r.dataSaida >= reserva.dataEntrada)
                    {
                        ViewBag.ImovelId = reserva.ImovelId;
                        ModelState.AddModelError(nameof(Reserva.dataEntrada), "Já existe reserva para essas datas");
                        return View();
                    }
                }
                _context.Add(reserva);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ImovelId"] = new SelectList(_context.Imoveis, "ImovelId", "Descricao", reserva.ImovelId);
            ViewData["IdentityUserId"] = new SelectList(_context.Users, "Id", "Id", reserva.IdentityUserId);
            return View(reserva);
        }

        // GET: Reservas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reserva = await _context.Reservas.FindAsync(id);
            if (reserva == null)
            {
                return NotFound();
            }
            ViewData["ImovelId"] = new SelectList(_context.Imoveis, "ImovelId", "Descricao", reserva.ImovelId);
            ViewData["IdentityUserId"] = new SelectList(_context.Users, "Id", "Id", reserva.IdentityUserId);
            return View(reserva);
        }

        // POST: Reservas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ReservaId,dataEntrada,dataSaida,ImovelId,IdentityUserId,Confirmar")] Reserva reserva)
        {
            if (id != reserva.ReservaId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(reserva);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReservaExists(reserva.ReservaId))
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
            ViewData["ImovelId"] = new SelectList(_context.Imoveis, "ImovelId", "ImovelId", reserva.ImovelId);
            ViewData["IdentityUserId"] = new SelectList(_context.Users, "Id", "Id", reserva.IdentityUserId);
            return View(reserva);
        }

        // GET: Reservas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reserva = await _context.Reservas
                .Include(r => r.Imovel)
                .Include(r => r.User)
                .FirstOrDefaultAsync(m => m.ReservaId == id);
            if (reserva == null)
            {
                return NotFound();
            }

            return View(reserva);
        }

        // POST: Reservas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var reserva = await _context.Reservas.FindAsync(id);
            _context.Reservas.Remove(reserva);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ReservaExists(int id)
        {
            return _context.Reservas.Any(e => e.ReservaId == id);
        }
    }
}
