using System;

namespace Samurai.Client.Wp7
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (SamuraiGame game = new SamuraiGame())
            {
                game.Run();
            }
        }
    }
#endif
}

