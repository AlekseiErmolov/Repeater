using System.IO;
using System.Text.RegularExpressions;

namespace Repeater.Infrastructure.Helpers
{
    internal static class CommonHelpers
    {
        public static bool IsValidFilename(string testName)
        {
            var containsABadCharacter = new Regex("[" + Regex.Escape(Path.GetInvalidFileNameChars().ToString()) + "]");
            return !containsABadCharacter.IsMatch(testName);
        }
    }
}