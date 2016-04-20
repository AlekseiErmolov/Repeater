using System.Text.RegularExpressions;

namespace Repeater.Classes
{
    static class CommonHelpers
    {
        public static bool IsValidFilename(string testName)
        {
            Regex containsABadCharacter = new Regex("[" + Regex.Escape(System.IO.Path.GetInvalidFileNameChars().ToString()) + "]");
            if (containsABadCharacter.IsMatch(testName))
                return false;

            return true;
        }
    }
}
