using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RazorTest.Data;
using RazorTest.Models;

namespace RazorTest.Controllers;

public class QueueEntriesController : Controller
{
	private readonly ApplicationDbContext _context;

	public QueueEntriesController(ApplicationDbContext context)
	{
		_context = context;
	}

	public async Task<IActionResult> Index()
	{
		return View(await _context.QueueEntries
			.Include(qe => qe.Queue)
			.Include(qe => qe.Student)
			.Include(qe => qe.Teacher)
			.ToListAsync());
	}

	public IActionResult Create()
	{
		ViewBag.Queues = _context.Queues.ToList();
		ViewBag.Students = _context.Students.ToList();
		ViewBag.Teachers = _context.Teachers.ToList();
		return View();
	}

	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> Create(QueueEntry entry)
	{
		if (!ModelState.IsValid)
		{
			ViewBag.Queues = _context.Queues.ToList();
			ViewBag.Students = _context.Students.ToList();
			ViewBag.Teachers = _context.Teachers.ToList();
			return View(entry);
		}

		_context.Add(entry);
		await _context.SaveChangesAsync();
		return RedirectToAction(nameof(Index));
	}

	public async Task<IActionResult> Delete(int id)
	{
		var entry = await _context.QueueEntries
			.Include(qe => qe.Queue)
			.Include(qe => qe.Student)
			.FirstOrDefaultAsync(qe => qe.Id == id);

		if (entry == null) return NotFound();

		return View(entry);
	}

	[HttpPost, ActionName("Delete")]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> DeleteConfirmed(int id)
	{
		var entry = await _context.QueueEntries.FindAsync(id);
		if (entry != null)
		{
			_context.QueueEntries.Remove(entry);
			await _context.SaveChangesAsync();
		}

		return RedirectToAction(nameof(Index));
	}
}
