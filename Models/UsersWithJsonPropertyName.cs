using System.Text.Json.Serialization;

namespace FitnessTracker.Models;

public class UsersWithJsonPropertyName
{
    // these attributes tell the deserializer to map the values in json to the properties in our model
    [JsonPropertyName("user_id")] public int UserId { get; set; }
    [JsonPropertyName("user_name")] public string UserName { get; set; } = "";
    [JsonPropertyName("email")] public string Email { get; set; } = "";
    [JsonPropertyName("password_hash")] public string PasswordHash { get; set; } = "";
    [JsonPropertyName("created_at")] public DateTime CreatedAt { get; set; }
}