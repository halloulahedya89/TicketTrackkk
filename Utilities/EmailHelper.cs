namespace TheSupportTicketSystem.Web.Utilities
{
    public class EmailHelper
    {
        public static string RemoveEmailDomain(string email)
        {
            // Check if the email is null or empty to avoid exceptions
            if (string.IsNullOrEmpty(email))
            {
                return email;
            }

            // Find the index of '@' symbol
            int atIndex = email.IndexOf('@');
            if (atIndex > -1)
            {
                // Return the substring from the beginning to the '@' symbol
                return email.Substring(0, atIndex);
            }

            // Return the original email if '@' symbol is not found
            return email;
        }
    }
}
