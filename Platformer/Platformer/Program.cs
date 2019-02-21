using System;
using System.Windows.Forms;

namespace Platformer
{
    class Program
    {
        static void Main(string[] args)
        {
            var gameWindow = new Window();
            new System.Threading.Thread(() => Application.Run(gameWindow)).Start();
        }
    }
}
