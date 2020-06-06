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
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Education.ViewModels;

namespace Education.Controllers
{
    public class ProjectsController : Controller
    {
        private readonly EducationDbContext _context;
        private readonly IWebHostEnvironment webHostEnvironment;

        public ProjectsController(EducationDbContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            webHostEnvironment = hostEnvironment;
        }

        //--BACKEND--
        // GET: Projects
        [Authorize(Roles = "Admins")]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Project.ToListAsync());
        }

        // GET: Projects/Details/5
        [Authorize(Roles = "Admins")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var project = await _context.Project
                .FirstOrDefaultAsync(m => m.ID == id);
            if (project == null)
            {
                return NotFound();
            }

            return View(project);
        }

        // GET: Projects/Create
        [Authorize(Roles = "Admins")]
        public IActionResult Create()
        {
            return View();
        }

        private string UploadedFile(ProjectCreate projectCreate)
        {
            string uniqueFileName = null;

            if (projectCreate.Image != null)
            {
                string uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "images");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + projectCreate.Image.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    projectCreate.Image.CopyTo(fileStream);
                }
            }
            return uniqueFileName;
        }

        // POST: Projects/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admins")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("ID,Name,Content,Price,CreatedAt,UpdatedAt")] Project project)
        public async Task<IActionResult> Create([Bind("ID,Name,Content,Price,Image")] ProjectCreate projectCreate)
        {
            Project projectModel = null;

            if (ModelState.IsValid)
            {

                string uniqueFileName = UploadedFile(projectCreate);
                string fileLocation = HttpContext.Request.Scheme + "://" + this.Request.Host + "/images/" + uniqueFileName;

                projectModel = new Project
                {
                    Name = projectCreate.Name,
                    Content = projectCreate.Content,
                    Price = projectCreate.Price,
                    //Picture = uniqueFileName,
                    Picture = fileLocation,

                };

                _context.Add(projectModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(projectModel);
        }

        // GET: Projects/Edit/5
        [Authorize(Roles = "Admins")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var project = await _context.Project.FindAsync(id);
            if (project == null)
            {
                return NotFound();
            }

            ProjectCreate projectCreate = new ProjectCreate();
            projectCreate.ID = project.ID;
            projectCreate.Name = project.Name;
            projectCreate.Content = project.Content;
            projectCreate.Price = project.Price;
            projectCreate.Picture = project.Picture;

            return View(projectCreate);
        }

        // POST: Projects/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admins")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, [Bind("ID,Name,Content,Price,CreatedAt,UpdatedAt")] Project project)
        public async Task<IActionResult> Edit(int id, [Bind("ID,Name,Content,Price,Image,CreatedAt")] ProjectCreate projectCreate)
        {
            if (id != projectCreate.ID)
            {
                return NotFound();
            }

            Project projectModel = null;

            if (ModelState.IsValid)
            {
                try
                {

                    string uniqueFileName = UploadedFile(projectCreate);
                    string fileLocation = HttpContext.Request.Scheme + "://" + this.Request.Host + "/images/" + uniqueFileName;

                    projectModel = new Project
                    {
                        ID = projectCreate.ID,
                        Name = projectCreate.Name,
                        Content = projectCreate.Content,
                        Price = projectCreate.Price,
                        //Picture = uniqueFileName,
                        Picture = fileLocation,

                    };

                    _context.Update(projectModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProjectExists(projectModel.ID))
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
            return View(projectModel);
        }

        // GET: Projects/Delete/5
        [Authorize(Roles = "Admins")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var project = await _context.Project
                .FirstOrDefaultAsync(m => m.ID == id);
            if (project == null)
            {
                return NotFound();
            }

            return View(project);
        }

        // POST: Projects/Delete/5
        [Authorize(Roles = "Admins")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var project = await _context.Project.FindAsync(id);
            _context.Project.Remove(project);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProjectExists(int id)
        {
            return _context.Project.Any(e => e.ID == id);
        }
        //--BACKEND--

        //--FRONTEND--
        // GET: Projects
        [Authorize]
        public async Task<IActionResult> List()
        {
            return View(await _context.Project.ToListAsync());
        }


        [Authorize]
        public async Task<IActionResult> Show(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var project = await _context.Project.Include(p => p.Tasks).FirstOrDefaultAsync(m => m.ID == id);
            var tasks = project.Tasks.ToList();

            ProjectShow projectShow = new ProjectShow();
            projectShow.project = project;
            projectShow.tasks = tasks;

            return View(projectShow);
        }

        //--FRONTEND--

    }
}
