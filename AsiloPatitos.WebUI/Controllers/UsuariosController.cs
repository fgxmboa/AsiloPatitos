using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using AsiloPatitos.Domain.Entities;
using AsiloPatitos.Infrastructure;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace AsiloPatitos.WebUI.Controllers
{
    public class UsuariosController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UsuariosController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Registro
        public IActionResult Register()
        {
            return View();
        }

        //POST: Registro
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(Usuario usuario)
        {
            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Por favor complete todos los campos requeridos.";
                return View(usuario);
            }

            // Validar correo duplicado
            if (await _context.Usuarios.AnyAsync(u => u.Correo == usuario.Correo))
            {
                TempData["ErrorMessage"] = "El correo ya está registrado.";
                return View(usuario);
            }

            // Hash de contraseña
            usuario.Contrasena = HashPassword(usuario.Contrasena);

            usuario.FechaCreacion = DateTime.Now;
            usuario.Activo = true;

            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Usuario registrado correctamente.";
            return RedirectToAction("Login");
        }

        // =====================================================
        //  GET: Login
        // =====================================================
        public IActionResult Login()
        {
            return View();
        }

        // =====================================================
        //  POST: Login
        // =====================================================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string correo, string contrasena)
        {
            if (string.IsNullOrWhiteSpace(correo) || string.IsNullOrWhiteSpace(contrasena))
            {
                TempData["ErrorMessage"] = "Debe ingresar el correo y la contraseña.";
                return View();
            }

            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(u => u.Correo == correo && u.Activo);

            if (usuario == null)
            {
                TempData["ErrorMessage"] = "Credenciales inválidas.";
                return View();
            }

            if (!VerifyPassword(contrasena, usuario.Contrasena))
            {
                TempData["ErrorMessage"] = "Credenciales incorrectas.";
                return View();
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, usuario.Nombre),
                new Claim(ClaimTypes.Email, usuario.Correo),
                new Claim(ClaimTypes.Role, usuario.Rol),
                new Claim("UsuarioId", usuario.Id.ToString())
            };

            var identity = new ClaimsIdentity(claims, "Cookies");
            var claimsPrincipal = new ClaimsPrincipal(identity);

            // Crear cookie de autenticación
            await HttpContext.SignInAsync("Cookies", claimsPrincipal);

            // Crear sesión
            HttpContext.Session.SetInt32("UsuarioId", usuario.Id);
            HttpContext.Session.SetString("NombreUsuario", usuario.Nombre);
            HttpContext.Session.SetString("Rol", usuario.Rol);

            TempData["SuccessMessage"] = "Bienvenido.";
            return RedirectToAction("Index", "Home");
        }

        // =====================================================
        //  GET: Logout
        // =====================================================
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync("Cookies");
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }

        // =====================================================
        //  Hash + Validación
        // =====================================================
        private string HashPassword(string password)
        {
            using var sha = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(password);
            var hash = sha.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }

        private bool VerifyPassword(string password, string hashedPassword)
        {
            return HashPassword(password) == hashedPassword;
        }

        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(string correo)
        {
            if (string.IsNullOrWhiteSpace(correo))
            {
                TempData["ErrorMessage"] = "Debe ingresar su correo.";
                return View();
            }

            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(u => u.Correo == correo);

            if (usuario == null)
            {
                TempData["ErrorMessage"] = "No existe una cuenta con este correo.";
                return View();
            }

            // Crear token
            var token = Guid.NewGuid().ToString("N");

            usuario.ResetToken = token;
            usuario.ResetTokenExpiracion = DateTime.Now.AddMinutes(30);

            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Se generó un enlace para restablecer su contraseña.";
            TempData["ResetLink"] = $"{Request.Scheme}://{Request.Host}/Usuarios/Restablecer?token={token}";

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Restablecer(string token)
        {
            if (string.IsNullOrEmpty(token))
                return RedirectToAction("Login");

            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(u => u.ResetToken == token
                                       && u.ResetTokenExpiracion > DateTime.Now);

            if (usuario == null)
            {
                TempData["ErrorMessage"] = "Enlace inválido o expirado.";
                return RedirectToAction("ForgotPassword");
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Restablecer(string token, string nuevaContrasena)
        {
            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(u => u.ResetToken == token
                                       && u.ResetTokenExpiracion > DateTime.Now);

            if (usuario == null)
            {
                TempData["ErrorMessage"] = "Enlace inválido o expirado.";
                return RedirectToAction("ForgotPassword");
            }

            usuario.Contrasena = HashPassword(nuevaContrasena);
            usuario.ResetToken = null;
            usuario.ResetTokenExpiracion = null;

            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Contraseña actualizada correctamente.";
            return RedirectToAction("Login");
        }
    }
}
