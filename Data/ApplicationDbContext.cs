
using Microsoft.EntityFrameworkCore;
using RazorTest.Models;

namespace RazorTest.Data;

public class ApplicationDbContext : DbContext
{
	public DbSet<User> Users => Set<User>();
	public DbSet<Teacher> Teachers => Set<Teacher>();
	public DbSet<Student> Students => Set<Student>();
	public DbSet<Queue> Queues => Set<Queue>();
	public DbSet<QueueEntry> QueueEntries => Set<QueueEntry>();
	public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
	{

	}

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		base.OnModelCreating(modelBuilder);

		// TPT
		modelBuilder.Entity<User>().UseTptMappingStrategy();

		// Mange-til-mange: Teacher <-> Queue
		
		// 1-til-mange: Queue -> QueueEntries
		modelBuilder.Entity<QueueEntry>()
			.HasOne(e => e.Queue)
			.WithMany(q => q.QueueEntries)
			.HasForeignKey(e => e.QueueId);

		// 1-til-mange: Student -> QueueEntries
		modelBuilder.Entity<QueueEntry>()
			.HasOne(e => e.Student)
			.WithMany(s => s.QueueEntries)
			.HasForeignKey(e => e.StudentId);

		// valgfri: Teacher -> QueueEntries (assigned)
		modelBuilder.Entity<QueueEntry>()
			.HasOne(e => e.Teacher)
			.WithMany(t => t.QueueEntries)
			.HasForeignKey(e => e.TeacherId)
			.OnDelete(DeleteBehavior.SetNull);

		// forhindrer dobbelt-tilmelding til samme kø
		modelBuilder.Entity<QueueEntry>()
			.HasIndex(e => new { e.QueueId, e.StudentId })
			.IsUnique();

		// ✅ FASTE GUIDS (så migrations bliver stabile)

		var teacher1Id = Guid.Parse("81cce9d1-dd44-4754-b3b7-f150cdd4ff11"); // jeres eksisterende seed
		var teacher2Id = Guid.Parse("11111111-1111-1111-1111-111111111111");

		Teacher[] teachers = [
			new Teacher
			{
				Id = teacher1Id,
				Name = "Test",
				Email = "test@gmail.com",
				PasswordHash = Auth.HashPassword("Test1234")
			},
			new Teacher
			{
				Id = teacher2Id,
				Name = "Amalie",
				Email = "amalie@techcollege.dk",
				PasswordHash = Auth.HashPassword("Teacher1234")
			}];

		var s1 = Guid.Parse("22222222-2222-2222-2222-222222222222");
		var s2 = Guid.Parse("33333333-3333-3333-3333-333333333333");
		var s3 = Guid.Parse("44444444-4444-4444-4444-444444444444");
		var s4 = Guid.Parse("55555555-5555-5555-5555-555555555555");
		var s5 = Guid.Parse("66666666-6666-6666-6666-666666666666");

		// Seed TEACHERS
		modelBuilder.Entity<Teacher>().HasData(
			teachers
		);

		modelBuilder.Entity<Teacher>()
    .HasMany(t => t.Queues)
    .WithMany(q => q.Teachers)
    .UsingEntity<Dictionary<string, object>>(
        "TeacherQueues",
        j => j.HasOne<Queue>()
              .WithMany()
              .HasForeignKey("QueueId")
              .OnDelete(DeleteBehavior.Cascade),
        j => j.HasOne<Teacher>()
              .WithMany()
              .HasForeignKey("TeacherId")
              .OnDelete(DeleteBehavior.Cascade),
        j =>
        {
            j.ToTable("TeacherQueues");
            j.HasKey("QueueId", "TeacherId"); // ✅ vigtigt!
        });

		Student[] students = [
			new Student { Id = s1, Name = "Elev 1", Email = "elev1@techcollege.dk", PasswordHash = Auth.HashPassword("Student1234") },
			new Student { Id = s2, Name = "Elev 2", Email = "elev2@techcollege.dk", PasswordHash = Auth.HashPassword("Student1234") },
			new Student { Id = s3, Name = "Elev 3", Email = "elev3@techcollege.dk", PasswordHash = Auth.HashPassword("Student1234") },
			new Student { Id = s4, Name = "Elev 4", Email = "elev4@techcollege.dk", PasswordHash = Auth.HashPassword("Student1234") },
			new Student { Id = s5, Name = "Elev 5", Email = "elev5@techcollege.dk", PasswordHash = Auth.HashPassword("Student1234") }
		];

		//  Seed STUDENTS
		modelBuilder.Entity<Student>().HasData(
			students
		);


		Queue[] queues = [
			new Queue { Id = 1, QueueName = "Serverside" },
			new Queue { Id = 2, QueueName = "Gui-programmering" },
			new Queue { Id = 3, QueueName = "Intro" }
		];

		modelBuilder.Entity<Queue>().HasData(
			queues
		);

		modelBuilder.Entity("TeacherQueues").HasData(
		new { QueueId = 1, TeacherId = teacher1Id },
		new { QueueId = 2, TeacherId = teacher2Id },
		new { QueueId = 3, TeacherId = teacher1Id },
		new { QueueId = 3, TeacherId = teacher2Id }
		);

	}

	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
	{
		var folder = Environment.SpecialFolder.LocalApplicationData;
		var path = Environment.GetFolderPath(folder);
		string dbPath = Path.Join(path, "queues.db");
		optionsBuilder.UseSqlite($"Data Source={dbPath}");
		base.OnConfiguring(optionsBuilder);
		Console.WriteLine(dbPath);
	}
}
