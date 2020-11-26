using static InstantFox.ConsoleUtils;

namespace InstantFox
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var loader = new Loader();
            loader.LoadSetup();
            loader.UnpackLZMAUtility();
            loader.UnpackFirefox();
            loader.InstallExtensions();
            loader.ExecuteResult(args);
            if (Confirm("Cleanup firefox instance?"))
            {
                loader.Cleanup();
            }
            else if (Confirm("Open instance folder instead?"))
            {
                loader.OpenWorkFolder();
            }
            Print("Goodbye!");
        }
    }
}
