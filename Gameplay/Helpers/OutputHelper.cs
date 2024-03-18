namespace Gameplay.Helpers
{
    internal static class OutputHelper
    {
        public static void Write()
        {
            Console.WriteLine();
        }

        public static void Write(string s, bool writeEmptyLineAfter = false)
        {
            Console.WriteLine(s);
            if (writeEmptyLineAfter)
            {
                Console.WriteLine();
            }
        }

        public static void Write(string pattern, params object[] values)
        {
            Console.WriteLine(string.Format(pattern, values));
        }

        public static void WriteDivider(string pattern)
        {
            var p = pattern.Where(_ => _ == '{').Select(_ => string.Empty).ToArray();
            Console.WriteLine(string.Format(pattern, p).Replace(" ", "-") + "-");
        }
    }
}
