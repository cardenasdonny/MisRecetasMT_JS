using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MisRecetasMT.Models.AccesoDB;
using MisRecetasMT.Models.Entidades;
using MisRecetasMT.Models.Usuario;
using MisRecetasMT.ViewModels.Usuario;
using MisRecetasMT.ViewModels;
using Newtonsoft.Json.Linq;
using MisRecetasMT.ViewModels.Receta;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace MisRecetasMT.Controllers
{
    public class UsuariosController : Controller
    {
        private readonly UserManager<UsuarioIdentity> _userManager;
        private readonly SignInManager<UsuarioIdentity> _singInManager;        

        private readonly Contexto _context;
        const string SesionNombre = "_Nombre";
        const string SesionRol = "_Rol";
        const string SesionId = "_Id";

        public UsuariosController(Contexto context, UserManager<UsuarioIdentity> userManager, SignInManager<UsuarioIdentity> signInManager, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _userManager = userManager;
            _singInManager = signInManager;  
            
        }
        //[Authorize]

        //Principal Usuarios
        public async Task<IActionResult> Index()
        {
            var usuarios = await _userManager.Users.ToListAsync();
            return View(usuarios);
        }              
        

        //Crear Usuarios
        //[Authorize]
        public IActionResult Crear()
        {
            return View();
        }

        public IActionResult CrearUsuario()
        {
            UsuarioModel usuarioModel = new UsuarioModel()
            {
                Registro = "2"
            };
            return View(usuarioModel);
        }


        //[Authorize]
        [HttpPost]       
        public async Task<IActionResult> Crear([Bind("Cedula,Nombre,Email,Password,Registro")] UsuarioModel usuarioModel)
        {

            if (ModelState.IsValid)
            {
                UsuarioIdentity usuario = new UsuarioIdentity()
                {
                    UserName = usuarioModel.Email,
                    Email = usuarioModel.Email,
                    Nombre = usuarioModel.Nombre,
                    Cedula = usuarioModel.Cedula,
                    Rol = 2
                };

                try
                {
                    TempData["Accion"] = "Crear";
                    TempData["Mensaje"] = "El usuario se creó con éxito";
                    var result = await _userManager.CreateAsync(usuario, usuarioModel.Password).ConfigureAwait(false);
                    if(usuarioModel.Registro!="1")
                        return RedirectToAction(nameof(Index));
                    else
                        return RedirectToAction(nameof(Login));

                }
                catch (Exception)
                {

                    //throw;
                    return View(usuario);
                }
            }
            return View(usuarioModel);
        }
        //[Authorize]
        // Eliminar Usuarios
        [HttpPost, ActionName("Eliminar")]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Eliminar(string id)
        {
            var usuario = await _userManager.FindByIdAsync(id);

            if (usuario != null)
            {
                await _userManager.DeleteAsync(usuario);                
                TempData["Accion"] = "Crear";
                TempData["Mensaje"] = "Usuario eliminado con éxito";
                return RedirectToAction(nameof(Index));  
            }
            return View("Index");

        }
     

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login([Bind("Email,Password,RecordarMe")] LoginModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _singInManager.PasswordSignInAsync(
                    model.Email, model.Password, model.RecordarMe, false);

                if (result.Succeeded)
                {                    
                    var usuario = _userManager.Users.FirstOrDefault(x=>x.Email==model.Email);                    
                    HttpContext.Session.SetString(SesionNombre, usuario.Nombre);
                    HttpContext.Session.SetInt32(SesionRol, usuario.Rol);
                    HttpContext.Session.SetString(SesionId, usuario.Id);

                    return RedirectToAction("Index", "Recetas");
                }

                //ModelState.AddModelError(string.Empty, "Error login");
                return RedirectToAction("Login", "Usuarios");
            }

            return View(model);
        }
        //[Authorize]
        public async Task<IActionResult> CerrarSesion()
        {
            await _singInManager.SignOutAsync();
            return RedirectToAction("Login", "Usuarios");
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult OlvidePassword()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> OlvidePassword(OlvidePasswordModel model)
        {
            if (ModelState.IsValid)
            {
                // Find the user by email
                var user = await _userManager.FindByEmailAsync(model.Email);
                // If the user is found AND Email is confirmed
                //if (user != null && await _userManager.IsEmailConfirmedAsync(user))
                if (user != null)
                {
                    // Generate the reset password token
                    var token = await _userManager.GeneratePasswordResetTokenAsync(user);

                    // Build the password reset link
                    var passwordResetLink = Url.Action("ResetearPassword", "Usuarios",
                            new { email = model.Email, token = token }, Request.Scheme);

                    MailMessage mensaje = new MailMessage();
                    mensaje.To.Add(model.Email);
                    mensaje.Subject = "Recetas resetear password";
                    mensaje.Body = passwordResetLink;
                    mensaje.From = new MailAddress("mariaisabel200.trivio@gmail.com");
                    mensaje.IsBodyHtml = false;
                    SmtpClient smtp = new SmtpClient("smtp.gmail.com");
                    smtp.Port = 587;
                    smtp.UseDefaultCredentials = true;
                    smtp.EnableSsl = true;
                    smtp.Credentials = new System.Net.NetworkCredential("mariaisabel200.trivio@gmail.com", "mariaisabeltrivino");
                    smtp.Send(mensaje);

                    // Log the password reset link
                    //_logger.Log(LogLevel.Warning, passwordResetLink);

                    // Send the user to Forgot Password Confirmation view
                    return View("OlvidePasswordConfirmacion");
                }

                // To avoid account enumeration and brute force attacks, don't
                // reveal that the user does not exist or is not confirmed
                return View("OlvidePasswordConfirmacion");
            }

            return View(model);
        }      

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ResetearPassword(string token, string email)
        {
            // If password reset token or email is null, most likely the
            // user tried to tamper the password reset link
            if (token == null || email == null)
            {
                ModelState.AddModelError("", "Token invalido");
            }
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> ResetearPassword(ResetearPasswordModel model)
        {
            if (ModelState.IsValid)
            {
                // Find the user by email
                var user = await _userManager.FindByEmailAsync(model.Email);

                if (user != null)
                {
                    // reset the user password
                    var result = await _userManager.ResetPasswordAsync(user, model.Token, model.Password);
                    if (result.Succeeded)
                    {
                        return View("ResetearPasswordConfirmacion");
                    }
                    // Display validation errors. For example, password reset token already
                    // used to change the password or password complexity rules not met
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                    return View(model);
                }

                // To avoid account enumeration and brute force attacks, don't
                // reveal that the user does not exist
                return View("ResetPasswordConfirmation");
            }
            // Display validation errors if model state is not valid
            return View(model);
        }   
       
    }
}
