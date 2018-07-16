using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleGame
{
    /*
    public interface Element
    {
        bool keyPressEnable
        {
            get;
            set;
        }
        
        void onKeyPressEvent(ConsoleKeyInfo key);
        void update();
        
    }
    */
    public abstract class Element
    {
        protected Game parent;
        public abstract void onKeyPressedEvent(ConsoleKeyInfo key);
        public abstract void update();

        public Element(Game parent)
        {
            this.parent = parent;
            init();
        }

        public Element()
        {
            this.parent = null;
            init();
        }

        protected virtual void init()
        {

        }
        protected void output(int x,int y,String symbol= "■")
        {
            Console.SetCursorPosition(x * 2 + parent.offsetX, y + parent.offsetY);
            Console.WriteLine(symbol);
        }
        protected void output(Point p, String symbol = "■")
        {
            output(p.x, p.y, symbol);
        }
    }
    /*
    class Sprite : Element
    {
        public String[,] pic;
        private Point _center, _size;

        public override void update()
        {
            int w = _size.x / 2;
            int h = _size.y / 2;
            for(int y = 0; y < _size.y; y++)
            {
                for(int x = 0; x < _size.x; x++)
                {
                    Console.SetCursorPosition(_center.x - w,y);
                    Console.Write(pic[x,y]);
                }
            }
            
        }

        protected override void init()
        {
            _size = new Point(1, 1);
            _center = new Point(0, 0);
            pic = new string[1,1];
            pic[0, 0] = "■";
        }

        public override void onKeyPressedEvent(ConsoleKeyInfo key)
        {
            
        }
    }


    class Scene : Element
    {
        public virtual void enter()
        {
            Console.Clear();
        }
        public virtual void exit()
        {
            Console.Clear();
        }
        public override void update()
        {

        }
        public override void onKeyPressedEvent(ConsoleKeyInfo key)
        {
            switch (key.Key)
            {
                case ConsoleKey.Escape:
                    break;
                case ConsoleKey.Spacebar:
                    break;
                default:
                    break;
            }
        }
        
    }
    */
}
