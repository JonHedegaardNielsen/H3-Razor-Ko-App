using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RazorTest.Data;
using RazorTest.Models;

namespace RazorTest.Controllers;

public class StudentsController : Controller
{
	private readonly ApplicationDbContext _context;

	public StudentsController(ApplicationDbContext context)
	{
		_context = context;
	}

	// GET: Students
	public async Task<IActionResult> Index()
	{
		return View(await _context.Students.ToListAsync());
	}

	// GET: Students/Details/{id}
	public async Task<IActionResult> Details(Guid id)
	{
		var student = await _context.Students
			.Include(s => s.QueueEntries)
			.ThenInclude(qe => qe.Queue)
			.FirstOrDefaultAsync(s => s.Id == id);

		if (student == null) return NotFound();

		return View(student);
	}

	// GET: Students/Create
	public IActionResult Create() => View();

	// POST: Students/Create
	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> Create(Student student)
	{
		if (!ModelState.IsValid) return View(student);

		_context.Add(student);
		await _context.SaveChangesAsync();
		return RedirectToAction(nameof(Index));
	}

	// GET: Students/Delete/{id}
	public async Task<IActionResult> Delete(Guid id)
	{
		var student = await _context.Students.FindAsync(id);
		if (student == null) return NotFound();

		return View(student);
	}

	// POST: Students/DeleteConfirmed/{id}
	[HttpPost, ActionName("Delete")]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> DeleteConfirmed(Guid id)
	{
		var student = await _context.Students.FindAsync(id);
		if (student != null)
		{
			_context.Students.Remove(student);
			await _context.SaveChangesAsync();
		}

		return RedirectToAction(nameof(Index));
	}
}
