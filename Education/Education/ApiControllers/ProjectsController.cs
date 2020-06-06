using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Education.Data;
using Education.Models;
using Education.ViewModels;

namespace Education.ApiControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectsController : ControllerBase
    {
        private readonly EducationDbContext _context;

        public ProjectsController(EducationDbContext context)
        {
            _context = context;
        }

        // GET: api/Projects
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Project>>> GetProject()
        {
            //return await _context.Project.ToListAsync();

            //var companies = db.Companies.ToList();
            //return Ok(new { results = companies });

            var projects = await _context.Project.ToListAsync();
            return Ok(new { courses = projects });
        }

        // GET: api/Projects/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Project>> GetProject(int id)
        {
            var project = await _context.Project.FindAsync(id);

            if (project == null)
            {
                return NotFound();
            }

            return project;
        }

        // PUT: api/Projects/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProject(int id, Project project)
        {
            if (id != project.ID)
            {
                return BadRequest();
            }

            _context.Entry(project).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProjectExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Projects
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<Project>> PostProject(Project project)
        {
            _context.Project.Add(project);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetProject", new { id = project.ID }, project);
        }

        // DELETE: api/Projects/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Project>> DeleteProject(int id)
        {
            var project = await _context.Project.FindAsync(id);
            if (project == null)
            {
                return NotFound();
            }

            _context.Project.Remove(project);
            await _context.SaveChangesAsync();

            return project;
        }

        private bool ProjectExists(int id)
        {
            return _context.Project.Any(e => e.ID == id);
        }


        //[HttpGet("{id}")]
        //public async Task<ActionResult<Project>> GetProject(int id)
        //{
        //    var project = await _context.Project.FindAsync(id);

        //    if (project == null)
        //    {
        //        return NotFound();
        //    }

        //    return project;
        //}

        //[HttpGet("{id}")]
        //[Route("Task/{id}")]
        //public async Task<ActionResult<Project>> Show(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var project = await _context.Project.Include(p => p.Tasks).FirstOrDefaultAsync(m => m.ID == id);
        //    //var tasks = project.Tasks.ToList();

        //    //ProjectShow projectShow = new ProjectShow();
        //    //projectShow.project = project;
        //    //projectShow.tasks = tasks;

        //    //return View(projectShow);
        //    return Ok(new { projectTask = project });
        //}

        [HttpGet("{id}")]
        [Route("Task/{id}")]
        public async Task<ActionResult<Project>> ShowAPI(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var task =  _context.Task.ToList().Where(t => t.ProjectId == id);
            //var tasks = project.Tasks.ToList();

            //ProjectShow projectShow = new ProjectShow();
            //projectShow.project = project;
            //projectShow.tasks = tasks;

            //return View(projectShow);
            return Ok(new { projectTask = task });
        }
    }
}
