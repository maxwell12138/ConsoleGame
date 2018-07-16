using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ConsoleGame
{

    public class Point
    {
        public Point(int x = 0, int y = 0)
        {
            this.x = x;
            this.y = y;
        }
        public Point(Point p)
        {
            this.x = p.x;
            this.y = p.y;
        }
        public void setPoint(Point p)
        {
            this.x = p.x;
            this.y = p.y;
        }
        public void setPoint(int x,int y)
        {
            this.x = x;
            this.y = y;
        }
        public static Point operator +(Point p1, Point p2)
        {
            Point p = new Point(p1.x + p2.x, p1.y + p2.y);
            return p;
        }

        public static bool operator ==(Point p1, Point p2)
        {
            if (p1.x == p2.x && p1.y == p2.y)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public static bool operator !=(Point p1, Point p2)
        {
            if (p1.x == p2.x && p1.y == p2.y)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
 
        public int x;
        public int y;
        /*
        public int x,y;
        public Point(int x=0,int y=0)
        {
            this.x = x;
            this.y = y;
        }
        */

        /*
        public int x
        {
            get
            {
                return _x ;
            }
            set
            {
                if (value > 0) _x = value;
            }
        }
        public int y
        {
            get
            {
                return _y;
            }
            set
            {
                if(value > 0) _y = value;
            }
        }
        */
    }
    
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                Type t = Game.showMenu();
                if (t == null) return;

                object ob = Activator.CreateInstance(t, new object[] { 18, 20, 6, 3, 800});
                Game game = (Game)ob;
                game.exec();
            }
        }
    }
}
