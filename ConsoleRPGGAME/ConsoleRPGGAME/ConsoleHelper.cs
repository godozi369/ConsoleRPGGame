using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game.Util;

namespace Game.Util
{
    public static class Helper
    {
        public static void ClearFromLine(int startY)           
        {
            int windowWidth = Console.WindowWidth;
            int windowHeight = Console.WindowHeight;

            for (int y = startY; y < windowHeight; y++)
            {
                Console.SetCursorPosition(0, y);
                Console.Write(new string(' ', windowWidth));
            }
            Console.SetCursorPosition(0, startY);
        }
    }
}
// Helper.ClearFromLine(15);
// Console.SetCursorPosition(0, 15);