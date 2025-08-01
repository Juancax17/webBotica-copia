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
    public class RolsController : Controller
    {
        private readonly MiAngelitoContext _context;

        public RolsController(MiAngelitoContext context)
        {
            _context = context;
        }

        // GET: Rols
        public async Task<IActionResult> Index()
        { 
            
            return View(await _context.Rols.OrderByDescending(c => c.Estado)
                .ThenBy(c => c.Rol1)
                .ToListAsync());
        }

        
        // GET: Rols/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Rols/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdRol,Rol1,Descripcion,Estado")] Rol rol)
        {
            if (ModelState.IsValid)
            {
                bool rolExiste = _context.Rols.Any(r => r.Rol1.ToLower().Trim() == rol.Rol1.ToLower().Trim());

                if (rolExiste)
                {
                    ModelState.AddModelError("Rol1", "El nombre del rol ya existe.");
                    return View(rol); 
                }
                _context.Add(rol);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(rol);
        }

        // GET: Rols/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rol = await _context.Rols.FindAsync(id);
            if (rol == null)
            {
                return NotFound();
            }
            return View(rol);
        }

        // POST: Rols/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdRol,Rol1,Descripcion,Estado")] Rol rol)
        {
            if (id != rol.IdRol)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    bool rolExiste = _context.Rols.Any(r => r.Rol1.ToLower().Trim() == rol.Rol1.ToLower().Trim() && r.IdRol != rol.IdRol);

                    if (rolExiste)
                    {
                        ModelState.AddModelError("Rol1", "El nombre del rol ya existe.");
                        return View(rol);
                    }
                    _context.Update(rol);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RolExists(rol.IdRol))
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
            return View(rol);
        }

        // GET: Rols/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rol = await _context.Rols
                .FirstOrDefaultAsync(m => m.IdRol == id);
            if (rol == null)
            {
                return NotFound();
            }

            return View(rol);
        }

        // POST: Rols/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var rol = await _context.Rols.FindAsync(id);
            if (rol != null)
            {
                rol.Estado = !rol.Estado;
                await _context.SaveChangesAsync();
            }

            
            return RedirectToAction(nameof(Index));
        }

        private bool RolExists(int id)
        {
            return _context.Rols.Any(e => e.IdRol == id);
        }
    }
}
