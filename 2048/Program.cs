using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;

namespace _2048
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Game game = new Game(4, 4);
            game.Run();

            Console.WriteLine("Finish");
            Console.ReadKey();
        }

    }

}
