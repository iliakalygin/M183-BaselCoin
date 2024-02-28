namespace WebApiBaselCoin.Models
{
    public class UserSession
    {
        public string SessionId { get; set; } 
        public int UserId { get; set; } 
        public DateTime CreatedAt { get; set; } 
        public DateTime LastAccessed { get; set; }
        public DateTime ExpiresAt { get; set; } 

        // Navigationseigenschaft zu User
        public User User { get; set; }
    }
}
