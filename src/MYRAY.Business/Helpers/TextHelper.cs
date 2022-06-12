namespace MYRAY.Business.Helpers;
/// <summary>
/// Helper class for operation related to string
/// </summary>
public static class TextHelper
{
    /// <summary>
    /// Validate phone number
    /// </summary>
    /// <param name="phoneNumber">The string to be validated.</param>
    /// <param name="minlength">Minimum phone length limit</param>
    /// <param name="maxlength">Maximum phone length limit</param>
    /// <returns>True if the string is a valid phone number</returns>
    public static bool IsValidPhoneNumber(this string phoneNumber, int minlength = 10, int maxlength = 11)
    {
        if (string.IsNullOrWhiteSpace(phoneNumber)) return false;

        phoneNumber = phoneNumber.Trim();

        if (phoneNumber[0] == '+')
        {
            phoneNumber = phoneNumber.Remove(0, 1);
        }

        if (phoneNumber.Length < minlength || phoneNumber.Length > maxlength)
        {
            return false;
        }

        return phoneNumber.All(char.IsDigit);
    }
    
    public static string ConvertVNPhoneNumber(this string phoneNumber)
    {
        string phone = phoneNumber;
        phone = phone.Trim();
        if (phone[0] == '+')
        {
            phone = phone.Remove(0,1);
        }

        if (phone[0] == '0')
        {
            phone = phone.Remove(0, 1);
        }

        if (phone.IndexOf("84", StringComparison.Ordinal) == 0)
        {
            phone = "+" + phone;
        }
        else
        {
            phone = "+84" + phone;
        }

        return phone;

    }
}