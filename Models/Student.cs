using System.ComponentModel.DataAnnotations;

namespace SmartLibraryAPI.Models
{
    public class Student : User
    {
        public override int BorrowLimit => 3;
        public override int ReturnDays => 14;

        public Student() { }

        public Student(string name, string email, string phoneNumber, string studentId)
            : base(name, email, phoneNumber)
        {
            StudentId = studentId;
        }
    }
}
