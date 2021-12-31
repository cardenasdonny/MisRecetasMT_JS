using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MisRecetasMT.Models.AccesoDB;
using MisRecetasMT.Models.Entidades;

namespace MisRecetasMT.Controllers
{
    //[Authorize]
    public class IngredientesController : Controller
    {
        private readonly Contexto _context;
        private readonly IWebHostEnvironment _hostEnvironment;

        public IngredientesController(Contexto context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
        }

        // GET: Ingredientes
        public async Task<IActionResult> Index()
        {
            return View(await _context.Ingredientes.Include("TipoIngrediente").ToListAsync());
        }

        // GET: Ingredientes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ingrediente = await _context.Ingredientes
                .Include(i => i.TipoIngrediente)
                .FirstOrDefaultAsync(m => m.IngredienteId == id);
            if (ingrediente == null)
            {
                return NotFound();
            }

            return View(ingrediente);
        }

        // GET: Ingredientes/Create
        public IActionResult Create()
        {            
            ViewData["TipoIngredienteId"] = new SelectList(_context.TipoIngrediente.Where(x=>x.EstadoTipoIngrediente==true).ToList(), "TipoIngredienteId", "NombreTipoIngrediente");
            return View();
        }

        // POST: Ingredientes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IngredienteId,NombreIngrediente,EstadoIngrediente,TipoIngredienteId,Imagen")] Ingrediente ingrediente)
 
        {
            if (ModelState.IsValid && ingrediente.TipoIngredienteId!=0)
            {

                string wwwRootPath = _hostEnvironment.WebRootPath;
                string nombreImagen = Path.GetFileNameWithoutExtension(ingrediente.Imagen.FileName);
                string extension = Path.GetExtension(ingrediente.Imagen.FileName);
                ingrediente.NombreImagen = nombreImagen = DateTime.Now.ToString("yymmssfff") + extension;
                var a = Path.Combine(wwwRootPath + "/image/" + nombreImagen);
                string path = Path.Combine(wwwRootPath + "/image/" + nombreImagen);

                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    await ingrediente.Imagen.CopyToAsync(fileStream);
                }

                _context.Add(ingrediente);
                await _context.SaveChangesAsync();
                TempData["Accion"] = "Crear";
                TempData["Mensaje"] = "El ingrediente se creó con éxito";
                return RedirectToAction(nameof(Index));
            }
            ViewData["TipoIngredienteId"] = new SelectList(_context.TipoIngrediente.Where(x => x.EstadoTipoIngrediente == true).ToList(), "TipoIngredienteId", "NombreTipoIngrediente", ingrediente.TipoIngredienteId);
            return View(ingrediente);
        }

        // GET: Ingredientes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {

            if (id == null)
            {
                return NotFound();
            }

            var ingrediente = await _context.Ingredientes.FindAsync(id);
            if (ingrediente == null)
            {
                return NotFound();
            }
            ViewData["TipoIngredienteId"] = new SelectList(_context.TipoIngrediente, "TipoIngredienteId", "NombreTipoIngrediente", ingrediente.TipoIngredienteId);
            return View(ingrediente);
        }

        // POST: Ingredientes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IngredienteId,NombreIngrediente,EstadoIngrediente,TipoIngredienteId")] Ingrediente ingrediente)
        {
            if (id != ingrediente.IngredienteId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(ingrediente);
                    await _context.SaveChangesAsync();
                    TempData["Accion"] = "Editar";
                    TempData["Mensaje"] = "El ingrediente se editó con éxito";
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!IngredienteExists(ingrediente.IngredienteId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }               
            }
            ViewData["TipoIngredienteId"] = new SelectList(_context.TipoIngrediente, "TipoIngredienteId", "NombreTipoIngrediente", ingrediente.TipoIngredienteId);
            return View(ingrediente);
        }

        // GET: Ingredientes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ingrediente = await _context.Ingredientes
                .Include(i => i.TipoIngrediente)
                .FirstOrDefaultAsync(m => m.IngredienteId == id);
            if (ingrediente == null)
            {
                return NotFound();
            }

            return View(ingrediente);
        }

        // POST: Ingredientes/Delete/5
        [HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var Ingrediente = await _context.Ingredientes.FindAsync(id);

                if (Ingrediente.EstadoIngrediente == true)
                {
                    Ingrediente.EstadoIngrediente = false;

                }
                else
                {
                    Ingrediente.EstadoIngrediente = true;
                }

                _context.Update(Ingrediente);
                await _context.SaveChangesAsync();
                TempData["Accion"] = "Editar";
                TempData["Mensaje"] = "Se cambió el estado con éxito";
                return RedirectToAction(nameof(Index));


            }
            catch (DbUpdateConcurrencyException)
            {


                throw;

            }
        }

        private bool IngredienteExists(int id)
        {
            return _context.Ingredientes.Any(e => e.IngredienteId == id);
        }
    }
}
