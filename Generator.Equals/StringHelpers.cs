using System;

namespace Generator.Equals
{
    public static class StringHelpers
    {
        static readonly string[] _newLineChars = new[] { "\r\n", "\r", "\n" };

        public static string[] ToLines(this string text)
        {
            return text?.Split(_newLineChars, StringSplitOptions.RemoveEmptyEntries) ?? [];
        }
    }
}