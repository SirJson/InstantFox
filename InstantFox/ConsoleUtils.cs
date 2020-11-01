using System;

namespace InstantFox
{
    internal static class ConsoleUtils
    {
        private static int groupLevel = 0;

        public static void Group() => groupLevel++;

        public static void EndGroup() => groupLevel--;

        public static void Print(params object[] data)
        {
            var tabs = new string('\t', groupLevel);
            var msg = string.Format("{1} [{0}] > {2}", DateTime.Now, tabs, string.Join(", ", data));
            Console.WriteLine(msg);
        }

        public static bool Confirm(string title)
        {
            ConsoleKey response;
            do
            {
                Console.Write($"{ title } [y/n] ");
                response = Console.ReadKey(false).Key;
                if (response != ConsoleKey.Enter)
                {
                    Console.WriteLine();
                }
            } while (response != ConsoleKey.Y && response != ConsoleKey.N);

            return (response == ConsoleKey.Y);
        }
    }
}
