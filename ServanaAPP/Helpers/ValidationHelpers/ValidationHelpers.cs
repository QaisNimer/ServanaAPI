namespace ServanaAPP.Helpers.ValidationHelpers
{
    public class ValidationHelpers
    {
        public static bool IsValidPassword(string password)
        {
            if (string.IsNullOrEmpty(password) || password.Length < 6)
                return false;

            bool hasUpper = false;
            bool hasLower = false;
            bool hasDigit = false;
            bool hasSymbol = false;

            string allowedSymbols = "!@#$%^&*()_+-=[]{}|;:'\",.<>?/~`";

            foreach (char c in password)
            {
                if (char.IsUpper(c))
                    hasUpper = true;
                else if (char.IsLower(c))
                    hasLower = true;
                else if (char.IsDigit(c))
                    hasDigit = true;
                else if (allowedSymbols.Contains(c))
                    hasSymbol = true;
            }

            return hasUpper && hasLower && hasDigit && hasSymbol;
        }


        public static bool IsValidEmail(string Email)
        {

            if (string.IsNullOrWhiteSpace(Email))
                return false;

            // Must contain exactly one '@'
            var parts = Email.Split('@');
            // Must contain exactly one '@' and something before it
            if (parts.Length != 2 || string.IsNullOrWhiteSpace(parts[0]))
                return false;

            string domain = parts[1].ToLower();
            string[] allowedDomains = { "gmail.com", "yahoo.com", "outlook.com", "hotmail.com" };

            return allowedDomains.Contains(domain);
        }

        public static bool IsValidName(string FullName)
        {
            if (string.IsNullOrWhiteSpace(FullName))
                return false;

            foreach (char c in FullName)
            {
                if (!char.IsLetter(c) && c != ' ')
                    return false;
            }

            return true;
        }

        public static bool IsValidPhone(string phoneNumber)
        {
            if (string.IsNullOrWhiteSpace(phoneNumber))
                return false;

            phoneNumber = phoneNumber.Trim();

            // Case 1: Local format (starts with 07 and length == 10)
            if (phoneNumber.StartsWith("07") && phoneNumber.Length == 10)
            {
                if ((phoneNumber.StartsWith("078") || phoneNumber.StartsWith("077") || phoneNumber.StartsWith("079")) &&
                    phoneNumber.All(char.IsDigit))
                {
                    return true;
                }
            }

            // Case 2: International format (starts with +962 and length == 13)
            if (phoneNumber.StartsWith("+962") && phoneNumber.Length == 13)
            {
                string rest = phoneNumber.Substring(4); // e.g., 78xxxxxxx

                if ((rest.StartsWith("78") || rest.StartsWith("77") || rest.StartsWith("79")) &&
                    rest.All(char.IsDigit))
                {
                    return true;
                }
            }

            throw new Exception("Invalid phone number");
        }


    }
}

