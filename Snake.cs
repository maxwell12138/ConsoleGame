using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleGame;
namespace Snake
{
    enum Direction { up, down, left, right };
    /*
    class SnakeScene : Element
    {
        public SnakeScene() : base()
        {
            //Element snake = new SnakeBody();
            //Element food=
        }
        public override void update()
        {
            
        }
        public override void onKeyPressedEvent(ConsoleKeyInfo key)
        {
            while (true)
            {
                switch (key.Key)
                {
                    case ConsoleKey.Escape:
                        break;
                    case ConsoleKey.Spacebar:
                        break;
                    case ConsoleKey.R:
                        break;
                    default:

                        break;
                }
            }

        }
    }
    */
    class Food : Element
    {
        public Point pos;

        public Food(Game parent) : base(parent)
        {
            pos = new Point(0, 0);
            //update();
        }

        public override void onKeyPressedEvent(ConsoleKeyInfo key)
        {
            
        }

        public override void update()
        {
            output(pos, "☆");
        }
    }

    class SnakeBody : Element
    {
        public List<Point> body;
        public Direction dir;
        public Food food;
        public bool isDead;
        public Point getHead()
        {
            return body[body.Count - 1];
        }
        public Point getNeck()
        {
            return body[body.Count - 2];
        }
        public Point getTail()
        {
            return body[0];
        }
        public SnakeBody(Game parent):base(parent)
        {
            
        }
        protected override void init()
        {
            base.init();
            dir = Direction.right;
            isDead = false;

            body = new List<Point>();
            Point p = new Point(parent.width / 2, parent.height/2);
            output(p, "■");
            body.Add(p);
            p = new Point(parent.width / 2 + 1, parent.height/2);
            output(p, "■");
            body.Add(p);
            p = new Point(parent.width / 2 + 2, parent.height/2);
            output(p, "□");
            body.Add(p);
        }
        public override void onKeyPressedEvent(ConsoleKeyInfo key)
        {
            //ConsoleKeyInfo key = Console.ReadKey();
            //Console.SetCursorPosition(Math.Max(Console.CursorLeft - 1, 0), Console.CursorTop);
            //Console.WriteLine(" ");
            switch (key.Key)
            {
                case ConsoleKey.W:
                case ConsoleKey.UpArrow:
                    if (getHead().y - 1 != getNeck().y) dir = Direction.up;
                    break;
                case ConsoleKey.A:
                case ConsoleKey.LeftArrow:
                    if (getHead().x - 1 != getNeck().x) dir = Direction.left;
                    break;
                case ConsoleKey.S:
                case ConsoleKey.DownArrow:
                    if (getHead().y + 1 != getNeck().y) dir = Direction.down;
                    break;
                case ConsoleKey.D:
                case ConsoleKey.RightArrow:
                    if (getHead().x + 1 != getNeck().x) dir = Direction.right;
                    break;
                default:
                    break;
            }
        }

        public override void update()
        {
            int x, y;
            switch (dir)
            {
                case Direction.up:
                    x = getHead().x;
                    y=getHead().y - 1;
                    break;
                case Direction.down:
                    x = getHead().x;
                    y = getHead().y + 1;
                    break;
                case Direction.left:
                    x = getHead().x-1;
                    y = getHead().y;
                    break;
                case Direction.right:
                    x = getHead().x+1;
                    y = getHead().y;
                    break;
                default:
                    x = 0;
                    y = 0;
                    break;
            }
            output(getHead(), "■");
            output(x, y, "□");
            body.Add(new ConsoleGame.Point(x, y));
            
            if (getHead() == food.pos)
            {
                ((Snake)parent).putFood();
                //return true;
            }
            else
            {
                output(getTail(), " ");
                body.RemoveAt(0);
                //return false;
            }
        }

        private void gameOver()
        {
            Console.SetCursorPosition(0, 0);
            Console.WriteLine("Game Over. Score: {0}. Press R to reload.", body.Count - 3);
        }
        public bool isOver()
        {
            int x = getHead().x;
            int y = getHead().y;
            //if snake go out of border, fail
            if (x >=parent.width || x < 0 || y >=parent.height || y < 0)
            {
                gameOver();
                return true;
            }
            //if snake eat it self, fail
            for (int i = 0; i < body.Count - 1; i++)
            {
                if (getHead() == body[i])
                {
                    gameOver();
                    return true;
                }
            }
            return false;
        }
    }

    class Snake : Game
    {
        SnakeBody snake;
        Food food;
        public Snake(int w = 18, int h = 25, int x = 6, int y = 3, int t = 800) : base(w, h, x, y, t)
        {
        }
        protected override void init()
        {
            Console.Clear();
            food = new Food(this);
            snake = new SnakeBody(this);
            putFood();
            snake.food = food;
            base.init();
        }
        
        public override void exec()
        {
            
            while (true)
            {
                if (isPause)
                {
                    pause();
                    autoEventInput.Set();
                    autoEventUpdate.Set();
                }
                if (isEnd)
                {
                    if (confirm("Do you want to leave?"))
                    {
                        return;
                    }
                }
                if (reload)
                {
                    if (confirm("Do you want to reload the game?"))
                    {
                        Console.Clear();
                        init(); // include autoEvent1.Set() autoEvent2.Set()
                    }
                }
            }
        }

        protected override void onKeyPressed()
        {
            while (true)
            {
                ConsoleKeyInfo key = Console.ReadKey();
                Console.SetCursorPosition(Console.CursorLeft-1, Console.CursorTop);
                Console.Write(" ");
                switch (key.Key)
                {
                    case ConsoleKey.Escape:
                        isEnd = true;
                        autoEventInput.Reset();
                        break;
                    case ConsoleKey.R:
                        reload = true;
                        autoEventInput.Reset();
                        break;
                    case ConsoleKey.Spacebar:
                        isPause = true;
                        autoEventInput.Reset();
                        break;
                    case ConsoleKey.PageDown:
                        if (deltaT < 900) deltaT += 100;
                        break;
                    case ConsoleKey.PageUp:
                        if (deltaT > 100) deltaT -= 100;
                        break;
                    default:
                        snake.onKeyPressedEvent(key);
                        break;
                }
            }
        }

        protected override void update()
        {
            while (true)
            {
                DateTime now = DateTime.Now;
                while (true)
                {
                    TimeSpan dt = DateTime.Now - now;
                    if (dt.TotalSeconds * 1000 >= deltaT) break;
                    if (isEnd || isPause || reload)
                    {
                        autoEventUpdate.Reset();
                        now = DateTime.Now;
                    }
                }

                food.update();
                snake.update();
                if (snake.isOver()) return;//isOver = true;
            }
        }

        public bool putFood()
        {
            int empty = height * width - snake.body.Count;
            if (empty <= 0) return false;

            //select a random index inside of the rest empty boxes
            //iterate all boxes and ignore the snake body, until the selected empty box
            Random random = new Random();
            int index = random.Next(0, empty);

            Point temp = new Point(0, 0);
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    temp.x = i;
                    temp.x = j;
                    if (!snake.body.Contains(temp))
                    {
                        if (index == 0)
                        {
                            food.pos.setPoint(i, j);
                            return true;
                        }
                        else
                        {
                            index--;
                        }
                    }
                }
            }
            return false;
        }
    }

}
