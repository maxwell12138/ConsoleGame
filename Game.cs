using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Threading;

namespace ConsoleGame
{
    public abstract class Game
    {
        protected AutoResetEvent autoEventInput, autoEventUpdate;

        public readonly int width;
        public readonly int height;
        public readonly int offsetX;
        public readonly int offsetY;
        public int deltaT;

        protected bool isPause;
        protected bool isEnd;
        protected bool reload;
        //protected bool isOver;


        public Game(int w = 18, int h = 18, int x = 6, int y = 3,int t=500)
        {
            autoEventInput = new AutoResetEvent(false);
            autoEventUpdate = new AutoResetEvent(false);
            width = w;
            height = h;
            offsetX = x;
            offsetY = y;
            deltaT = t;
            
            init();
        }
        
        protected virtual void init()
        {
            isPause = false;
            isEnd = false;
            reload = false;
            Thread checkKeyPress = new Thread(onKeyPressed);
            checkKeyPress.Start();
            Thread ondraw = new Thread(update);
            ondraw.Start();

            // draw borders
            Console.ForegroundColor = ConsoleColor.Blue;
            for (int i = -1; i < width + 1; i++)
            {
                Console.SetCursorPosition(offsetX + i * 2, offsetY - 1);
                Console.Write("■");
                Console.SetCursorPosition(offsetX + i * 2, offsetY + height);
                Console.Write("■");
            }
            for (int j = -1; j < height + 1; j++)
            {
                Console.SetCursorPosition(offsetX - 2, offsetY + j);
                Console.Write("■");
                Console.SetCursorPosition(offsetX + width * 2, offsetY + j);
                Console.Write("■");
            }
            Console.ForegroundColor = ConsoleColor.White;
        }

        public static Type showMenu()
        {
            List<Type> games = new List<Type>();

            //search every subclasses of the substract parent class Game
            //display as a menu list and wait for selected
            Assembly assembly = Assembly.GetExecutingAssembly();
            Type[] allType = assembly.GetTypes();

            Type baseType = Type.GetType("ConsoleGame.Game");

            foreach (Type t in allType)
            {
                if (t.BaseType == baseType)
                {
                    games.Add(t);
                    Console.WriteLine("  " + t.Name);
                }
            }
            Console.SetCursorPosition(0, 0);
            Console.WriteLine("※");

            int currentRow = 0;
            while (true)
            {
                Console.SetCursorPosition(0, games.Count + 1);
                ConsoleKeyInfo key = Console.ReadKey();
                Console.SetCursorPosition(0, games.Count + 1);
                Console.WriteLine(" ");
                switch (key.Key)
                {
                    case ConsoleKey.Enter:
                        return games[currentRow];
                    case ConsoleKey.W:
                    case ConsoleKey.UpArrow:
                        if (currentRow < 1) continue;
                        Console.SetCursorPosition(0, currentRow - 1);
                        Console.WriteLine("※");
                        Console.WriteLine(" ");
                        currentRow--;
                        break;
                    case ConsoleKey.S:
                    case ConsoleKey.DownArrow:
                        if (currentRow + 1 >= games.Count) continue;
                        Console.SetCursorPosition(0, currentRow);
                        Console.WriteLine(" ");
                        Console.WriteLine("※");
                        currentRow++;
                        break;
                    case ConsoleKey.Escape:
                        return null;
                    default:
                        break;
                }
            }
        }
        public void pause()
        {
            Console.SetCursorPosition(0, 0);
            Console.WriteLine("Pause. Press Space to continue...");
            while (Console.ReadKey().Key != ConsoleKey.Spacebar)
            {
                Console.SetCursorPosition(0, 1);
                Console.WriteLine(" ");
                Console.SetCursorPosition(0, 0);
            }
            Console.SetCursorPosition(0, 0);
            Console.WriteLine("                                 ");
            Console.WriteLine(" ");
            isPause = !isPause;
        }


        public abstract void exec();
        protected abstract void update();
        protected abstract void onKeyPressed();

        public bool confirm(string info,int row=0,int col=0)
        {
            while (true)
            {
                Console.SetCursorPosition(row, row);
                Console.Write(info + "(Y/N): ");
                ConsoleKeyInfo key = Console.ReadKey();
                switch (key.Key)
                {
                    case ConsoleKey.Y:
                        return true;
                    case ConsoleKey.N:
                        Console.SetCursorPosition(row, col);
                        for (int i = 0; i < info.Length + 8; i++)
                        {
                            Console.Write(" ");
                        }
                        return false;
                    default:
                        Console.SetCursorPosition(info.Length + 7, col);
                        Console.WriteLine(" ");
                        break;
                }
            }

        }
    }
}
