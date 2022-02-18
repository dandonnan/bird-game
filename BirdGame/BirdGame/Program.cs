namespace BirdGame
{
    using System;

    /// <summary>
    /// The Program file. This is auto-generated and makes
    /// the game run when the program starts.
    /// </summary>
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            using (var game = new MainGame())
                game.Run();
        }
    }
}
