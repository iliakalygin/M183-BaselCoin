namespace WebApiBaselCoin.Models
{
    public class AppLog
    {
        public int Id { get; set; }
        public DateTime EventDate { get; set; }
        public string EventType { get; set; }
        public int UserId { get; set; }
        public string Action { get; set; }

        // Navigation Property für die Beziehung zu User
        public User User { get; set; }
    }
}
