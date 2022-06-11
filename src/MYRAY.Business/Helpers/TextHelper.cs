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

        return phoneNumber.All(c => char.IsDigit(c));
    }
}