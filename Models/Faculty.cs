namespace MauiApp1.Models
{
    public class Faculty : User
    {
        public string EmployeeId { get; set; }

        public override int BorrowLimit => 6;
        public override int ReturnDays => 21;

        public Faculty(string name, string email = "", string phoneNumber = "", string employeeId = "")
            : base(name, email, phoneNumber)
        {
            EmployeeId = employeeId;
        }
    }
}