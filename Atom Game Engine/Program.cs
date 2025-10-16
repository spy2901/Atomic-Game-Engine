using Atom_Game_Engine;

namespace AtomGameEngine
{
    class Program
    {
        static void Main()
        {
            using (Game game = new Game(1920, 1080, "Atom Game Engine"))
            {
                game.Run();
                
            }
        }
    }
}
