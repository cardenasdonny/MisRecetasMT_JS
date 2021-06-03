using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MisRecetasMT.Models.AccesoDB;
using MisRecetasMT.Models.Entidades;

namespace MisRecetasMT.Controllers
{
    
    public class TipoIngredientesController : Controller
    {
        private readonly Contexto _context;

        public TipoIngredientesController(Contexto context)
        {
            _context = context;
        }
        [Authorize]

        // GET: TipoIngredientes
        public async Task<IActionResult> Index()
        {
            var tipoIngrediente = await _context.TipoIngrediente.Where(x=>x.EstadoTipoIngrediente==true).ToListAsync();
            return View(tipoIngrediente);
            //return View(await _context.TipoIngrediente.ToListAsync());
        }

        // GET: TipoIngredientes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tipoIngrediente = await _context.TipoIngrediente
                .FirstOrDefaultAsync(m => m.TipoIngredienteId == id);
            if (tipoIngrediente == null)
            {
                return NotFound();
            }

            return View(tipoIngrediente);
        }

        // GET: TipoIngredientes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: TipoIngredientes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TipoIngredienteId,NombreTipoIngrediente,EstadoTipoIngrediente")] TipoIngrediente tipoIngrediente)
        {
            if (ModelState.IsValid)
            {
                TempData["Accion"] = "Crear";
                TempData["Mensaje"] = "El tipo de ingrediente se creó con éxito";

                _context.Add(tipoIngrediente);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(tipoIngrediente);
        }

        // GET: TipoIngredientes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tipoIngrediente = await _context.TipoIngrediente.FindAsync(id);
            if (tipoIngrediente == null)
            {
                return NotFound();
            }
            return View(tipoIngrediente);
        }

        // POST: TipoIngredientes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TipoIngredienteId,NombreTipoIngrediente,EstadoTipoIngrediente")] TipoIngrediente tipoIngrediente)
        {
            if (id != tipoIngrediente.TipoIngredienteId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tipoIngrediente);
                    await _context.SaveChangesAsync();
                    TempData["Accion"] = "Editar";
                    TempData["Mensaje"] = "El tipo de ingrediente se editó con éxito";
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TipoIngredienteExists(tipoIngrediente.TipoIngredienteId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
               
            }
            return View(tipoIngrediente);
        }

        

        // POST: TipoIngredientes/Delete/5
        [HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]        
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tipoIngrediente = await _context.TipoIngrediente.FindAsync(id);
            _context.TipoIngrediente.Remove(tipoIngrediente);
            await _context.SaveChangesAsync();
            TempData["Accion"] = "Eliminar";
            TempData["Mensaje"] = "El tipo de ingrediente se eliminó con éxito";
            return RedirectToAction(nameof(Index));

        }

        private bool TipoIngredienteExists(int id)
        {
            return _context.TipoIngrediente.Any(e => e.TipoIngredienteId == id);
        }
    }
}
