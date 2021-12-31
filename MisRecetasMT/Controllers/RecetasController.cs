using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MisRecetasMT.Models.AccesoDB;
using MisRecetasMT.Models.Entidades;
using MisRecetasMT.ViewModels.Receta;


namespace MisRecetasMT.Controllers
{
    //[Authorize]
    public class RecetasController : Controller
    {
        private readonly Contexto _context;
        private readonly IWebHostEnvironment _hostEnvironment;

        public RecetasController(Contexto context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
        }

        // GET: Recetas
        
        public async Task<IActionResult> Index()
        {
            
            if((HttpContext.Session.GetInt32("_Rol")==1))            
                return View(await _context.Recetas.Include("Usuario").ToListAsync());
            else
                return View(await _context.Recetas.Where(x => x.UsuarioId.Equals(HttpContext.Session.GetString("_Id"))).Include("Usuario").ToListAsync());
            
            //return View(await _context.Recetas.Include("Usuario").ToListAsync());
        }

        public async Task<IActionResult> RecetasCompartidas()
        {            
            return View(await _context.Recetas.Include("Usuario").Where(x=>x.Estado==true).ToListAsync());            
        }

        // GET: Recetas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var receta = await _context.Recetas.Include(e=>e.Usuario)
                .FirstOrDefaultAsync(m => m.RecetaId == id);
            if (receta == null)
            {
                return NotFound();
            }

            ViewData["RecetaDetalle"] = await _context.RecetaDetalle.Where(x => x.RecetaId == id).Include(y => y.Ingrediente).ToListAsync();

            return View(receta);
        }

        // GET: Recetas/Create
        public IActionResult Create()
        {
            ViewBag.Ingredientes = _context.Ingredientes.Where(x=>x.EstadoIngrediente==true).ToList();
            return View();
        }

        // POST: Recetas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        //public IActionResult Create([FromBody] RecetaViewModel recetaViewModel)
        //public IActionResult Create([FromBody] IFormFile file)
        //public IActionResult Create (IFormFile file)
        //public IActionResult Create(RecetaViewModel recetaViewModel)
        //public JsonResult Create(string Nombre, string Descripcion, IFormFile Imagen, List<RecetaDetalle> Ingredientes)
        public async Task<IActionResult> Create(string Nombre, string Descripcion, IFormFile Imagen, bool Estado)
        {                        
            
            if (Nombre != null && Descripcion != null && Imagen != null)
            {
                Receta receta = new Receta();
                string wwwRootPath = _hostEnvironment.WebRootPath;
                string nombreImagen = Path.GetFileNameWithoutExtension(Imagen.FileName);
                string extension = Path.GetExtension(Imagen.FileName);
                receta.NombreImagen = nombreImagen = DateTime.Now.ToString("yymmssfff") + extension;
                receta.Nombre = Nombre;
                receta.Estado = Estado;
                receta.Descripcion = Descripcion;
                receta.Fecha = DateTime.Now;
                receta.UsuarioId = HttpContext.Session.GetString("_Id");                        

                string path = Path.Combine(wwwRootPath + "/image/" + nombreImagen);

                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    await Imagen.CopyToAsync(fileStream);
                }

                _context.Add(receta);

                if (_context.SaveChanges() > 0)
                {
                    return Json(new { data = "ok" });
                }
                else
                {
                    return Json(new { data = "error" });

                }
            }
            else            

                return Json(new { data = "error" });  

        }       

        [HttpPost]
        public IActionResult CreateDetails([FromBody] RecetaViewModel recetaViewModel)
        {
            if (recetaViewModel.Ingredientes != null)
            {
                int RecetaId = _context.Recetas.Max(r => r.RecetaId);

                foreach (var ingrediente in recetaViewModel.Ingredientes)
                {
                    RecetaDetalle recetaDetalle = new RecetaDetalle()
                    {
                        RecetaId = RecetaId,
                        IngredienteId = ingrediente.IngredienteId,
                        Cantidad = ingrediente.Cantidad
                    };
                    _context.Add(recetaDetalle);
                }
                if (_context.SaveChanges() > 0)
                {
                    return Json(new { data = "ok" });
                   
                }
                else
                {
                    return Json(new { data = "no" });
                }
            }
            else
            {
                return Json(new { data = "no" });
            }
         
        }
            // GET: Recetas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var receta = await _context.Recetas.FindAsync(id);
            if (receta == null)
            {
                return NotFound();
            }
            RecetaViewModel recetaViewModel = new RecetaViewModel
            {
                RecetaId = receta.RecetaId,
                Nombre = receta.Nombre,
                Descripcion = receta.Descripcion,
                NombreImagen = receta.NombreImagen,
                Estado = receta.Estado,
                UsuarioId = receta.UsuarioId

            };

            ViewData["RecetaDetalle"] = await _context.RecetaDetalle.Where(x => x.RecetaId == id).Include(y => y.Ingrediente).ToListAsync();
            ViewData["Ingredientes"] = await _context.Ingredientes.ToListAsync();
            
            return View(recetaViewModel);


            /*
            if (id == null)
            {
                return NotFound();
            }

            var receta = await _context.Recetas.FindAsync(id);
            if (receta == null)
            {
                return NotFound();
            }
            return View(receta);
            */
        }

        // POST: Recetas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, [Bind("RecetaId,Nombre,Descripcion,Fecha,Estado,UsuarioId")] Receta receta)
        public async Task<IActionResult> Edit(string Nombre, string Descripcion, IFormFile Imagen, bool Estado, string NombreImagen, string UsuarioId, int RecetaId)
        {

            if (Nombre != null && Descripcion != null)
            {
                Receta receta = new Receta();
                if (Imagen != null)
                {                    
                    string wwwRootPath = _hostEnvironment.WebRootPath;
                    string nombreImagen = Path.GetFileNameWithoutExtension(Imagen.FileName);
                    string extension = Path.GetExtension(Imagen.FileName);
                    receta.NombreImagen = nombreImagen = DateTime.Now.ToString("yymmssfff") + extension;                  

                    string path = Path.Combine(wwwRootPath + "/image/" + nombreImagen);

                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        await Imagen.CopyToAsync(fileStream);
                    }

                }
                else
                {
                    receta.NombreImagen = NombreImagen; 

                }

                receta.Nombre = Nombre;
                receta.Estado = Estado;
                receta.Descripcion = Descripcion;
                receta.Fecha = DateTime.Now;
                receta.UsuarioId = UsuarioId;
                receta.RecetaId = RecetaId;

                var recetaId = receta.RecetaId;

                try
                {
                    _context.Update(receta);                    

                    if (await _context.SaveChangesAsync() > 0)
                    {
                        var listaRecetaDetalle = _context.RecetaDetalle.Where(x => x.RecetaId == recetaId);
                        foreach (var recetaDetalle in listaRecetaDetalle)
                        {
                            _context.RecetaDetalle.Remove(recetaDetalle);
                            
                        }
                        await _context.SaveChangesAsync();

                        return Json(new { data = "ok" });
                    }
                    else
                    {
                        return Json(new { data = "error" });
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RecetaExists(receta.RecetaId))
                    {
                        return Json(new { data = "error" });
                    }
                    else
                    {
                        return Json(new { data = "error" });
                    }
                    
                }    
                
            }
            else

                return Json(new { data = "error" });
        }

        

        // POST: Recetas/Delete/5
        [HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var receta = await _context.Recetas.FindAsync(id);
            _context.Recetas.Remove(receta);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RecetaExists(int id)
        {
            return _context.Recetas.Any(e => e.RecetaId == id);
        }
    }
}
