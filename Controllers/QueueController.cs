using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RazorTest.Data;
using RazorTest.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

public class QueuesController : Controller
{
	private readonly ApplicationDbContext _context;

	public QueuesController(ApplicationDbContext context)
	{
		_context = context;
	}

	// =========================
	// Helpers
	// =========================
	private bool TryGetUserId(out Guid userId)
	{
		userId = Guid.Empty;
		var idStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
		return Guid.TryParse(idStr, out userId);
	}

	// =========================
	// Public / Common views
	// (kan bruges af både elev og lærer)
	// =========================

	// Liste over køer
	public async Task<IActionResult> Index()
	{
		var queues = await _context.Queues
			.Include(q => q.Teachers)
			.AsNoTracking()
			.ToListAsync();

		return View(queues);
	}

	// Detaljeside for en kø + elever i køen
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

	// =========================
	// Student actions
	// =========================

	// Elev joiner en kø (1 klik)
	[Authorize(Roles = "Student")]
	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> Join(int queueId, string description, string location)
	{
		if (!TryGetUserId(out var studentId))
		{
			TempData["Error"] = "Din bruger kunne ikke findes. Log ind igen.";
			return RedirectToAction(nameof(Student));
		}

		// basic validation
		if (queueId <= 0) ModelState.AddModelError("", "Vælg en kø.");
		if (string.IsNullOrWhiteSpace(description)) ModelState.AddModelError("", "Beskrivelse er påkrævet.");
		if (string.IsNullOrWhiteSpace(location)) ModelState.AddModelError("", "Lokation er påkrævet.");

		if (!ModelState.IsValid)
		{
			// fyld dropdown igen
			var queues = await _context.Queues.AsNoTracking().OrderBy(q => q.QueueName).ToListAsync();
			ViewBag.Queues = queues;
			return View("Student");
		}

		// ✅ Kun 1 kø ad gangen: fjern evt. eksisterende entry
		var existing = await _context.QueueEntries.FirstOrDefaultAsync(e => e.StudentId == studentId);
		if (existing != null)
			_context.QueueEntries.Remove(existing);

		_context.QueueEntries.Add(new QueueEntry
		{
			QueueId = queueId,
			StudentId = studentId,
			TeacherId = null,
			CreatedAt = DateTime.UtcNow,
			Description = description.Trim(),
			Location = location.Trim()
		});

		await _context.SaveChangesAsync();

		TempData["Success"] = "Du er nu sat i kø.";
		return RedirectToAction(nameof(Details), new { id = queueId });
	}

	[Authorize(Roles = "Student")]
	[HttpGet]
	public async Task<IActionResult> Student()
	{
		var queues = await _context.Queues
			.AsNoTracking()
			.OrderBy(q => q.QueueName)
			.ToListAsync();

		ViewBag.Queues = queues; // bruges i dropdown
		return View();
	}

	// (Valgfrit senere) Hvis I vil: 1 kø ad gangen
	// Lav en JoinOne(...) som fjerner eksisterende entry før den opretter en ny.

	// =========================
	// Teacher actions
	// =========================

	// Lærer ser kun de køer han/hun er med i + elever i kø
	public async Task<IActionResult> MyQueues()
	{
		if (!User.IsInRole("Teacher"))
			return Forbid();

		if (!TryGetUserId(out var teacherId))
			return RedirectToAction("Login", "Auth");

		var queues = await _context.Queues
			.Include(q => q.Teachers)
			.Include(q => q.QueueEntries)
				.ThenInclude(e => e.Student)
			.Where(q => q.Teachers.Any(t => t.Id == teacherId))
			.AsNoTracking()
			.ToListAsync();

		return View(queues);
	}

	// =========================
	// Admin / CRUD (hvis I bruger det)
	// =========================

	public IActionResult Create() => View();

	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> Create(Queue queue)
	{
		if (!ModelState.IsValid) return View(queue);

		_context.Queues.Add(queue);
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