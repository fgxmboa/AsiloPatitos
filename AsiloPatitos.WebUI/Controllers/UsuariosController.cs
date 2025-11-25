using System;
using System.Security.Cryptography;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AsiloPatitos.Domain.Entities;
using AsiloPatitos.Infrastructure;

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
        public IActionResult Logout()
        {
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
    }
}
