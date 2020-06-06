using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Education.Data;
using Education.Models;
using Microsoft.AspNetCore.Authorization;

namespace Education.Controllers
{
    //--BACKEND--

    [Authorize(Roles = "Admins")]
    public class TasksController : Controller
    {
        private readonly EducationDbContext _context;

        public TasksController(EducationDbContext context)
        {
            _context = context;
        }

        // GET: Tasks
        public async Task<IActionResult> Index()
        {
            var educationDbContext = _context.Task.Include(t => t.Project);
            return View(await educationDbContext.ToListAsync());
        }

        // GET: Tasks/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var task = await _context.Task
                .Include(t => t.Project)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (task == null)
            {
                return NotFound();
            }

            return View(task);
        }

        // GET: Tasks/Create
        public IActionResult Create()
        {
            //ViewData["ProjectId"] = new SelectList(_context.Project, "ID", "ID");

            //ViewData["ProjectId"] = _context.Task.Include(t => t.Project).Select(t => new SelectListItem() { Text = t.Project.Name, Value = t.Project.Name })
            //.Distinct()
            //.ToList();

            ViewData["ProjectId"] = _context.Project.Select(t => new SelectListItem() { Text = t.Name, Value = t.Name })
            .Distinct()
            .ToList();

            return View();
        }

        // POST: Tasks/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("ID,Title,Note,Video,Header,Tag,ProjectId,CreatedAt,UpdatedAt")] Education.Models.Task task)
        public async Task<IActionResult> Create([Bind("ID,Title,Note,Video,Header,Tag,ProjectId")] Education.Models.Task task)
        {
            if (ModelState.IsValid)
            {
                _context.Add(task);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProjectId"] = new SelectList(_context.Project, "ID", "ID", task.ProjectId);
            return View(task);
        }

        // GET: Tasks/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var task = await _context.Task.Include(t => t.Project).FirstOrDefaultAsync(t => t.ID == id);
            var project = task.Project;

            if (task == null)
            {
                return NotFound();
            }

            ViewData["ProjectId"] = _context.Project
                .Where(p => p.ID != project.ID)
                .Select(p => new SelectListItem() { Text = p.Name, Value = p.Name })
                .Distinct()
                .ToList();


            ViewData["SelectedProjectId"] = project.Name;

            return View(task);
        }

        // POST: Tasks/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, [Bind("ID,Title,Note,Video,Header,Tag,ProjectId,CreatedAt,UpdatedAt")] Education.Models.Task task)
        public async Task<IActionResult> Edit(int id, [Bind("ID,Title,Note,Video,Header,Tag,ProjectId,CreatedAt")] Education.Models.Task task)
        {
            if (id != task.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(task);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TaskExists(task.ID))
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
            ViewData["ProjectId"] = new SelectList(_context.Project, "ID", "ID", task.ProjectId);
            return View(task);
        }

        // GET: Tasks/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var task = await _context.Task
                .Include(t => t.Project)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (task == null)
            {
                return NotFound();
            }

            return View(task);
        }

        // POST: Tasks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var task = await _context.Task.FindAsync(id);
            _context.Task.Remove(task);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TaskExists(int id)
        {
            return _context.Task.Any(e => e.ID == id);
        }
        //--BACKEND--

        //--FRONTEND--
      
        //--FRONTEND--

    }
}
