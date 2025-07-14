﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using webBotica2.Models;

namespace webBotica2.Controllers
{
    public class ProveedoresController : Controller
    {
        private readonly MiAngelitoContext _context;

        public ProveedoresController(MiAngelitoContext context)
        {
            _context = context;
        }

        // GET: Proveedores
        public async Task<IActionResult> Index()
        {
            var miAngelitoContext = _context.Proveedores.Include(p => p.IdLaboratorioNavigation);
            return View(await miAngelitoContext.ToListAsync());
        }

        
        // GET: Proveedores/Create
        public IActionResult Create()
        {
            ViewData["IdLaboratorio"] = new SelectList(_context.Laboratorios.Where(l=>l.Estado==true), "IdLaboratorio", "Nombre");
            return View();
        }

        // POST: Proveedores/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdProveedor,Ruc,RazonSocial,Tipo,Telefono,Correo,Estado,IdLaboratorio")] Proveedore proveedore)
        {
            if (ModelState.IsValid)
            {
                bool ExistProv= _context.Proveedores.Any(r=>r.Ruc == proveedore.Ruc);

                if (ExistProv) {
                    ModelState.AddModelError("Ruc", "El RUC ya se encuentra registrado");
                    ViewData["IdLaboratorio"] = new SelectList(_context.Laboratorios.Where(l => l.Estado == true), "IdLaboratorio", "Nombre", proveedore.IdLaboratorio);
                    return View(proveedore);
                }
                _context.Add(proveedore);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdLaboratorio"] = new SelectList(_context.Laboratorios.Where(l => l.Estado == true),"IdLaboratorio", "Nombre", proveedore.IdLaboratorio);
            return View(proveedore);
        }

        // GET: Proveedores/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var proveedore = await _context.Proveedores.FindAsync(id);
            if (proveedore == null)
            {
                return NotFound();
            }
            ViewData["IdLaboratorio"] = new SelectList(_context.Laboratorios.Where(l => l.Estado == true), "IdLaboratorio", "Nombre", proveedore.IdLaboratorio);
            return View(proveedore);
        }

        // POST: Proveedores/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdProveedor,Ruc,RazonSocial,Tipo,Telefono,Correo,Estado,IdLaboratorio")] Proveedore proveedore)
        {
            if (id != proveedore.IdProveedor)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    bool ExistProv = _context.Proveedores.Any(r => r.Ruc == proveedore.Ruc && r.IdProveedor!=proveedore.IdProveedor);
                     
                   if (ExistProv) {
                        ModelState.AddModelError("Ruc", "El RUC ya se encuentra registrado");
                        ViewData["IdLaboratorio"] = new SelectList(_context.Laboratorios.Where(l => l.Estado == true), "IdLaboratorio", "Nombre", proveedore.IdLaboratorio);
                        return View(proveedore);
                        
                    }
                    _context.Update(proveedore);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProveedoreExists(proveedore.IdProveedor))
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
            ViewData["IdLaboratorio"] = new SelectList(_context.Laboratorios.Where(l => l.Estado == true), "IdLaboratorio", "Nombre", proveedore.IdLaboratorio);
            return View(proveedore);
        }

        // GET: Proveedores/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var proveedore = await _context.Proveedores
                .Include(p => p.IdLaboratorioNavigation)
                .FirstOrDefaultAsync(m => m.IdProveedor == id);
            if (proveedore == null)
            {
                return NotFound();
            }

            return View(proveedore);
        }

        // POST: Proveedores/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var proveedore = await _context.Proveedores.FindAsync(id);
            if (proveedore != null)
            {
                proveedore.Estado = false;
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProveedoreExists(int id)
        {
            return _context.Proveedores.Any(e => e.IdProveedor == id);
        }
    }
}
