namespace MauiApp1.Models
{
    public class Student : User
    {
        public string StudentId { get; set; }

        public override int BorrowLimit => 3;
        public override int ReturnDays => 14;

        public Student(string name, string email = "", string phoneNumber = "", string studentId = "")
            : base(name, email, phoneNumber)
        {
            StudentId = studentId;
        }
    }
}