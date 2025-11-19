namespace MauiApp1.Factories
{
    using MauiApp1.Models;

    /// <summary>
    /// Factory Pattern: Creates User objects based on type
    /// </summary>
    public class UserFactory
    {
        public static User CreateUser(
            string userType,
            string name,
            string email = "",
            string phoneNumber = "",
            string idNumber = "")
        {
            return userType.ToLower() switch
            {
                "student" => new Student(name, email, phoneNumber, idNumber),
                "faculty" => new Faculty(name, email, phoneNumber, idNumber),
                _ => throw new ArgumentException($"Invalid user type: {userType}")
            };
        }

        public static User CreateStudent(string name, string email = "", string phoneNumber = "", string studentId = "")
        {
            return new Student(name, email, phoneNumber, studentId);
        }

        public static User CreateFaculty(string name, string email = "", string phoneNumber = "", string employeeId = "")
        {
            return new Faculty(name, email, phoneNumber, employeeId);
        }
    }
}