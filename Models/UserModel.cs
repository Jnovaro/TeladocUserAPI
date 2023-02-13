using TeladocUserAPI.Controllers;

namespace TeladocUserAPI.Models
{
    public class UserModel
    {
        const int MinimumAge = 18;

        private string firstName;
        private string lastName;
        private string email;
        private DateTime dateOfBirth;

        public string FirstName { get { return firstName; } }
        public string LastName { get { return lastName; } }
        public string Email { get { return email; } }
        public DateTime DateOfBirth { get { return dateOfBirth; } }

        public int Age { get { return CalculateAge(); } }

        public UserModel(string firstName, string lastName, string email, DateTime dateOfBirth)
        {
            this.firstName = firstName;
            this.lastName = lastName;
            this.email = email == null ? null : email.Replace(" ", "");
            this.dateOfBirth = dateOfBirth.Date;
        }

        public bool IsValid(bool newUser, ref string message, string oldEmail = "")
        {
            if (string.IsNullOrWhiteSpace(this.firstName)) {
                message = "First Name can't be empty.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(this.firstName))
            {
                message = "Last Name can't be empty.";
                return false;
            }

            if (!ValidateEmail(ref message, newUser, oldEmail))
            {
                return false;
            }

            if (!ValidateAge())
            {
                message = "User must be at least 18 years old to subscribe.";
                return false;
            }
            return true;
        }

        public bool ValidateEmail(ref string message, bool newUser, string oldEmail)
        {
            if (!ValidateEmailFormat())
            {
                message = "Verify that the email is correct. It should contain one '@' and a '.' after it.";
                return false;
            }
            try
            {
                if (!CheckEmailUniqueness(newUser, oldEmail))
                {
                    message = "The email alreay belongs to a registered user.";
                    return false;
                }
            }
            catch (Exception ex)
            {
                message = ex.Message;
                return false;
            }
            return true;
        }

        public bool ValidateAge()
        {
            return this.dateOfBirth.AddYears(MinimumAge) <= DateTime.Today;
        }

        public bool ValidateEmailFormat()
        {
            if (!string.IsNullOrWhiteSpace(this.email) &&
                    email.Count(x => x == '@') == 1 &&
                    email.Substring(this.email.IndexOf('@')).Contains('.'))
                return true;
            return false;
        }

        public bool CheckEmailUniqueness(bool newUser, string oldEmail)
        {
            try
            {
                UserController controller = new();
                List<UserModel>? userData = controller.GetUserData();
                if (newUser)
                {
                    if (userData != null)
                        return !userData.Any(x => x.Email == this.email);
                    else return true;
                }
                else
                {
                    if (this.email == oldEmail) return true;
                    return !userData.Any(x => x.Email == this.email);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public int CalculateAge()
        {
            int age = DateTime.Today.Year - this.dateOfBirth.Year;
            if (DateTime.Today.Month < this.dateOfBirth.Month ||
                (DateTime.Today.Month == this.dateOfBirth.Month && DateTime.Today.Day < this.dateOfBirth.Day))
                age--;
            return age;
        }

    }

}
