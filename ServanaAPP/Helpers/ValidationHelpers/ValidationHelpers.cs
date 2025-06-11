namespace ServanaAPP.Helpers.ValidationHelpers
{
    public class ValidationHelpers
    {
        public static bool IsValidatePassword(string Password)
        {
            bool hasSymbole = false;
            bool hasUpper = false;
            bool hasLower = false;
            bool hasDigit = false;

            if (!string.IsNullOrEmpty(Password) && Password.Length >= 6)
            {
                foreach (char c in Password)
                {
                    if (char.IsUpper(c))
                        hasUpper = true;
                    else if (char.IsLower(c))
                        hasLower = true;
                    else if (char.IsDigit(c))
                        hasDigit = true;
                    else
                        hasSymbole = true;
                }
            }
            else
            {
                return false;
            }

            return hasUpper && hasLower && hasDigit && hasSymbole;
        }

        public static bool IsValidEmail(string Email)
        {

            if (!string.IsNullOrEmpty(Email))
            {
                if (Email.Contains("gmail") ||
                    Email.Contains("yahoo") ||
                    Email.Contains("outlook") ||
                    Email.Contains("hotmail"))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

            return false;
        }

        public static bool IsValidName(string FullName)
        {
            if (string.IsNullOrEmpty(FullName))
                return false;

            foreach (char c in FullName)
            {
                if (!char.IsLetter(c))
                    return false;
            }

            return true;
        }
        
        public static bool IsValidPhone(string PhoneNumber)
        {
            try
            {
                foreach (char c in PhoneNumber)
                {
                    //
                    if (!char.IsDigit(c))
                    {
                        return false;
                    }
                }
                if (!string.IsNullOrWhiteSpace(PhoneNumber))
                {
                    if (PhoneNumber.StartsWith("+962") && PhoneNumber.Length == 13)
                        return true;
                }
                throw new Exception("Invalid Phone Number");
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }

        }

    }
}

