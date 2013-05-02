using System;

namespace SquareFighter
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (SFGame game = new SFGame())
            {
                game.Run();
            }
        }
    }
#endif
}

