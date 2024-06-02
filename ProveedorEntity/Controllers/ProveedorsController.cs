using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using ProveedorEntity.Models;

namespace ProveedorEntity.Controllers
{
    public class ProveedorsController : Controller
    {
        private readonly ProveedorDatosContext _context;

        public ProveedorsController(ProveedorDatosContext context)
        {
            _context = context;
        }

        // GET: Proveedors
        public async Task<IActionResult> Index()
        {
            return _context.Proveedors != null ?
                        View(await _context.Proveedors.ToListAsync()) :
                        Problem("Entity set 'ProveedorDatosContext.Proveedors'  is null.");
        }

        public IActionResult ExportarCSV()
        {
            var proveedores = _context.Proveedors.ToList(); // Obtén los datos de la base de datos

            var csv = new StringBuilder();
            csv.AppendLine("Id,Nombre,Email,Direccion,Telefono"); // Encabezados del archivo CSV

            foreach (var proveedor in proveedores)
            {
                csv.AppendLine($"{proveedor.Id},{proveedor.Nombre},{proveedor.Email},{proveedor.Direccion},{proveedor.Telefono ?? ""}");
            }

            byte[] byteArray = Encoding.UTF8.GetBytes(csv.ToString());
            MemoryStream stream = new MemoryStream(byteArray);

            // Devuelve el archivo CSV para descargar
            return File(stream, "text/csv", "proveedores.csv");
        }

        public IActionResult ExportarPDF()
        {
            var proveedores = _context.Proveedors.ToList(); // Obtén los datos de la base de datos

            MemoryStream memoryStream = new MemoryStream();
            PdfWriter writer = new PdfWriter(memoryStream);
            PdfDocument pdfDocument = new PdfDocument(writer);
            Document document = new Document(pdfDocument);

            // Agrega un título al documento PDF
            document.Add(new Paragraph("Lista de Proveedores"));

            // Agrega los datos de los proveedores al documento PDF
            foreach (var proveedor in proveedores)
            {
                document.Add(new Paragraph($"ID: {proveedor.Id}, Nombre: {proveedor.Nombre}, Email: {proveedor.Email}, Dirección: {proveedor.Direccion}, Teléfono: {proveedor.Telefono ?? "N/A"}"));
            }

            // Cierra el documento PDF
            document.Close();

            // Establece el tipo de contenido y el nombre del archivo
            return File(memoryStream.ToArray(), "application/pdf", "proveedores.pdf");
        }

// GET: Proveedors/Details/5
public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Proveedors == null)
            {
                return NotFound();
            }

            var proveedor = await _context.Proveedors
                .FirstOrDefaultAsync(m => m.Id == id);
            if (proveedor == null)
            {
                return NotFound();
            }

            return View(proveedor);
        }

        // GET: Proveedors/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Proveedors/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nombre,Email,Direccion,Telefono")] Proveedor proveedor, IFormFile Imagen)
        {

            var isCaptchaValid = IsReCaptchValid();

            if (!isCaptchaValid)
            {
                ViewData["Mensaje"] = "Por favor Senatori, Verifique el reCAPTCHA.";
                return View();
            }

            if (ModelState.IsValid)
            {
                if (Imagen != null && Imagen.Length > 0)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await Imagen.CopyToAsync(memoryStream);
                        proveedor.Imagen = memoryStream.ToArray();
                    }
                }

                _context.Add(proveedor);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(proveedor);
        }

        private bool IsReCaptchValid()
        {
            var captchaResponse = Request.Form["g-recaptcha-response"];
            var secretKey = "6Ld17_UnAAAAAOWs0zTT-UxMYONIZ9Eo4vdUM1aG";  // clave secreta de reCAPTCHA

            using (var httpClient = new HttpClient())
            {
                var response = httpClient.GetAsync($"https://www.google.com/recaptcha/api/siteverify?secret={secretKey}&response={captchaResponse}").Result;
                if (response.IsSuccessStatusCode)
                {
                    var jsonResponse = response.Content.ReadAsStringAsync().Result;
                    dynamic data = JObject.Parse(jsonResponse);
                    return data.success == "true";
                }
                else
                {
                    // Maneja el error de reCAPTCHA
                    return false;
                }
            }
        }

        // GET: Proveedors/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Proveedors == null)
            {
                return NotFound();
            }

            var proveedor = await _context.Proveedors.FindAsync(id);
            if (proveedor == null)
            {
                return NotFound();
            }
            return View(proveedor);
        }

        // POST: Proveedors/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nombre,Email,Direccion,Telefono")] Proveedor proveedor, IFormFile Imagen)
        {
            if (id != proveedor.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (Imagen != null && Imagen.Length > 0)
                    {
                        using (var memoryStream = new MemoryStream())
                        {
                            await Imagen.CopyToAsync(memoryStream);
                            proveedor.Imagen = memoryStream.ToArray();
                        }
                    }

                    _context.Update(proveedor);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProveedorExists(proveedor.Id))
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
            return View(proveedor);
        }

        // GET: Proveedors/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Proveedors == null)
            {
                return NotFound();
            }

            var proveedor = await _context.Proveedors
                .FirstOrDefaultAsync(m => m.Id == id);
            if (proveedor == null)
            {
                return NotFound();
            }

            return View(proveedor);
        }

        // POST: Proveedors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Proveedors == null)
            {
                return Problem("Entity set 'ProveedorDatosContext.Proveedors'  is null.");
            }
            var proveedor = await _context.Proveedors.FindAsync(id);
            if (proveedor != null)
            {
                _context.Proveedors.Remove(proveedor);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProveedorExists(int id)
        {
            return (_context.Proveedors?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}