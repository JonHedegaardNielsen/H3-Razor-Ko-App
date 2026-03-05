public class Auth
{
    public static string HashPassword(string password)
	{
		return Convert.ToBase64String(
			System.Text.Encoding.UTF8.GetBytes(password));
	}

}
