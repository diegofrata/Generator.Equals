namespace Generator.Equals
{
    public static class StringHelpers
    {
        public static string[] ToLines(
            this string text)
        {
            return text.Split(
                new[] { "\r\n", "\r", "\n" },
                System.StringSplitOptions.RemoveEmptyEntries);
        }
    }
}