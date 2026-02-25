using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using RazorTest.Data;

public class AuthController : Controller
{
	private readonly ApplicationDbContext _context;

	public AuthController(ApplicationDbContext context)
	{
		_context = context;
	}

	// GET: Auth/Login
	public IActionResult Login()
	{
		return View();
	}

	// POST: Auth/Login
	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> Login(string email, string password)
	{
		// 🔐 hash the incoming password
		var hashedPassword = HashPassword(password);

		// Check Student
		var student = await _context.Students
			.FirstOrDefaultAsync(s => s.Email == email && s.PasswordHash == hashedPassword);

		if (student != null)
		{
			await SignInUser(student.Id, student.Name, "Student");
			return RedirectToAction("Index", "Home");
		}

		// Check Teacher
		var teacher = await _context.Teachers
			.FirstOrDefaultAsync(t => t.Email == email && t.PasswordHash == hashedPassword);

		if (teacher != null)
		{
			await SignInUser(teacher.Id, teacher.Name, "Teacher");
			return RedirectToAction("Index", "Home");
		}

		ModelState.AddModelError("", "Invalid login attempt");
		return View();
	}

	public async Task<IActionResult> Logout()
	{
		await HttpContext.SignOutAsync();
		return RedirectToAction("Login");
	}

	private async Task SignInUser(Guid id, string name, string role)
	{
		var claims = new List<Claim>
		{
			new Claim(ClaimTypes.NameIdentifier, id.ToString()),
			new Claim(ClaimTypes.Name, name),
			new Claim(ClaimTypes.Role, role)
		};

		var identity = new ClaimsIdentity(
			claims,
			CookieAuthenticationDefaults.AuthenticationScheme);

		var principal = new ClaimsPrincipal(identity);

		await HttpContext.SignInAsync(
			CookieAuthenticationDefaults.AuthenticationScheme,
			principal);
	}

	// ⚠️ Simple example only (NOT production secure)
	private string HashPassword(string password)
	{
		return Convert.ToBase64String(
			System.Text.Encoding.UTF8.GetBytes(password));
	}

	public IActionResult AccessDenied()
	{
		return View();
	}
}
