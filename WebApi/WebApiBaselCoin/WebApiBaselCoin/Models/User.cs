namespace WebApiBaselCoin.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password_Hash { get; set; }
        public string Role { get; set; }
        public decimal Balance { get; set; }
    }
}
