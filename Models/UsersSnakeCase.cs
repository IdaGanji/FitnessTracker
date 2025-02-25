namespace FitnessTracker.Models;

public class UsersSnakeCase
{
    public int user_id { get; set; }
    public string user_name { get; set; } = "";
    public string email { get; set; } = "";
    public string password_hash { get; set; } = "";
    public DateTime created_at { get; set; }
}