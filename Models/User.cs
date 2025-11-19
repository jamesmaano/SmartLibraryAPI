namespace MauiApp1.Models
{
    public abstract class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public abstract int BorrowLimit { get; }
        public abstract int ReturnDays { get; }
        public DateTime RegisteredDate { get; set; } = DateTime.Now;

        protected User(string name, string email = "", string phoneNumber = "")
        {
            Name = name;
            Email = email;
            PhoneNumber = phoneNumber;
        }
    }
}