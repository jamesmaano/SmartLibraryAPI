using System.ComponentModel.DataAnnotations;

namespace SmartLibraryAPI.Models
{
    public class Faculty : User
    {
        public override int BorrowLimit => 5;
        public override int ReturnDays => 30;

        public Faculty() { }

        public Faculty(string name, string email, string phoneNumber, string employeeId)
            : base(name, email, phoneNumber)
        {
            EmployeeId = employeeId;
        }
    }
}
