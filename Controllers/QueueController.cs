using RazorTest.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;



public class QueuesController : Controller
{
	private readonly ApplicationDbContext _context;

	public QueuesController(ApplicationDbContext context)
	{
		_context = context;
	}

	public async Task<IActionResult> Index()
	{
		return View(await _context.Queues
			.Include(q => q.Teachers)
			.ToListAsync());
	}

	public async Task<IActionResult> Details(int id)
	{
		var queue = await _context.Queues
			.Include(q => q.Teachers)
			.Include(q => q.QueueEntries)
			.ThenInclude(qe => qe.Student)
			.FirstOrDefaultAsync(q => q.Id == id);

		if (queue == null) return NotFound();

		return View(queue);
	}

	public IActionResult Create() => View();

	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> Create(RazorTest.Models.Queue queue)
	{
		if (!ModelState.IsValid) return View(queue);

		_context.Add(queue);
		await _context.SaveChangesAsync();
		return RedirectToAction(nameof(Index));
	}

	public async Task<IActionResult> Delete(int id)
	{
		var queue = await _context.Queues.FindAsync(id);
		if (queue == null) return NotFound();

		return View(queue);
	}

	[HttpPost, ActionName("Delete")]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> DeleteConfirmed(int id)
	{
		var queue = await _context.Queues.FindAsync(id);
		if (queue != null)
		{
			_context.Queues.Remove(queue);
			await _context.SaveChangesAsync();
		}

		return RedirectToAction(nameof(Index));
	}
}
