
using RazorTest.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RazorTest.Models;
namespace RazorTest.Controllers;

public class TeachersController : Controller
{
	private readonly ApplicationDbContext _context;

	public TeachersController(ApplicationDbContext context)
	{
		_context = context;
	}

	public async Task<IActionResult> Index()
	{
		return View(await _context.Teachers
			.Include(t => t.Queues)
			.ToListAsync());
	}

	public async Task<IActionResult> Details(Guid id)
	{
		var teacher = await _context.Teachers
			.Include(t => t.Queues)
			.Include(t => t.QueueEntries)
			.ThenInclude(qe => qe.Queue)
			.FirstOrDefaultAsync(t => t.Id == id);

		if (teacher == null) return NotFound();

		return View(teacher);
	}

	public IActionResult Create() => View();

	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> Create(Teacher teacher)
	{
		if (!ModelState.IsValid) return View(teacher);

		_context.Add(teacher);
		await _context.SaveChangesAsync();
		return RedirectToAction(nameof(Index));
	}

	public async Task<IActionResult> Delete(Guid id)
	{
		var teacher = await _context.Teachers.FindAsync(id);
		if (teacher == null) return NotFound();

		return View(teacher);
	}

	[HttpPost, ActionName("Delete")]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> DeleteConfirmed(Guid id)
	{
		var teacher = await _context.Teachers.FindAsync(id);
		if (teacher != null)
		{
			_context.Teachers.Remove(teacher);
			await _context.SaveChangesAsync();
		}

		return RedirectToAction(nameof(Index));
	}
}
