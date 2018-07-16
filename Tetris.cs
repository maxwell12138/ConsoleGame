using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ConsoleGame.Tetris;
//using ConsoleGame.Tetris;
namespace ConsoleGame
{
    
    class Block : Element
    {
        public bool isStop;
        public int[,] data;
        public int[] offset;

        public int typeIndex;
        public int nextTypeIndex;
        public int poseIndex;    //1 2 3 4
        public int nextPoseIndex;
        public const int poseNum = 4;
        private Random rd;
        private List<int[,]> types;
        private Tetris parent;

        public Block(Tetris parent):base(parent)
        {
            this.parent = parent;
        }
        
        private void initType()
        {
            int[,] points = new int[4, 8] { { -1, 1, 0, 1, 0, 0, 1, 1 }
                                            , { -1,1,0,1,0,0,0,2}
                                            , { -1,0,0,0,1,0,0,1}
                                            , { 0,0,0,1,1,1,0,2} };
            types.Add(points);
            points = new int[4, 8] { { -2,0,-1,0,0,0,1,0}
                                    , { 0,-2,0,-1,0,0,0,1}
                                    , { -1,0,0,0,1,0,2,0}
                                    , { 0,-1,0,0,0,1,0,2} };
            types.Add(points);
            points = new int[4, 8] { { -1,0,0,0,-1,1,0,1}
                                    ,{ -1,0,0,0,-1,1,0,1}
                                    ,{ -1,0,0,0,-1,1,0,1}
                                    ,{ -1,0,0,0,-1,1,0,1}};
            types.Add(points);
            points = new int[4, 8] { { -1,0,0,0,0,-1,-1,1}
                                    ,{ -1,-1,0,-1,0,0,1,0}
                                    ,{ 0,0,1,0,1,-1,0,1}
                                    ,{ -1,0,0,0,0,1,1,1}};
            types.Add(points);
            points = new int[4, 8] { {-2,0,-1,0,0,0,0,1}
                                    ,{-1,0,0,0,0,-1,0,-2}
                                    ,{0,-1,0,0,1,0,2,0}
                                    ,{0,0,1,0,0,1,0,2}};
            types.Add(points);
            points = new int[4, 8] { {0,-1,0,0,1,0,2,0}
                                  , {0,0,1,0,0,1,0,2}
                                  , {-2,0,-1,0,0,0,0,1}
                                  , {-1,0,0,0,0,-1,0,-2}};
        }
        protected override void init()
        {
            base.init();
            isStop = false;
            data = new int[4, 2];
            offset = new int[2] { base.parent.width / 2, 2 };
            types = new List<int[,]>();
            initType();

            rd = new Random();
            //nextPoseIndex = rd.Next(types.Count);
            //nextPoseIndex = rd.Next(poseNum);
            typeIndex = rd.Next(types.Count);
            poseIndex = rd.Next(poseNum);

            data = getPose(typeIndex, poseIndex);
            display();
        }

        public void reset(int typeIndex = -1, int poseIndex = -1)
        {
            if (typeIndex < 0 || typeIndex >= types.Count
                || poseIndex < 0 || poseIndex >= poseNum)
            {
                //typeIndex = nextTypeIndex;
                //poseIndex = nextPoseIndex;
                //nextTypeIndex = rd.Next(types.Count);
                //nextPoseIndex = rd.Next(poseNum);
                
                offset[0] = parent.width/2;
                offset[1] = 2;
                //data = getPose(typeIndex, poseIndex);
                data = getPose(rd.Next(types.Count), rd.Next(poseNum));
                display();
            }

        }
        public int[,] getPose(int typeIndex, int poseIndex)
        {
            int[,] result = new int[4, 2];
            int[,] data = types[typeIndex];
            for (int index = 0; index < 4; index++)
            {
                result[index, 0] = data[poseIndex, index * 2];
                result[index, 1] = data[poseIndex, index * 2 + 1];
            }
            return result;
        }
        public int[,] getPosition()
        {
            int[,] points = new int[4, 2];
            for (int i = 0; i < 4; i++)
            {
                points[i, 0] = data[i, 0] + offset[0];
                points[i, 1] = data[i, 1] + offset[1];
            }
            return points;
        }

        public bool isBottom(int[,] p)
        {
            for(int i = 0; i < 4; i++)
            {
                //int[,] p = getPosition();//runAction(Actions.down);
                if (p[i, 1] >= parent.height) return true;
                if (parent.points[p[i,0],p[i,1]] != 0) return true;
            }
            return false;
        }
        public bool isOverlap(int[,] points)
        {
            for (int i = 0; i < 4; i++)
            {
                if (points[i, 0] < 0 || points[i, 0] >= parent.width) return true; //no overboard
                if (points[i, 1] < 0 || points[i, 1] >= parent.height) return true; 
                if (parent.points[points[i, 0], points[i, 1]] != 0) return true; //no overlap
            }
            return false;
        }
        /*
        public bool checkEnable(Actions action)
        {
            int[,] points = runAction(action);
            //if (touchBottom()) return false;
            for (int i = 0; i < 4; i++)
            {
                if (points[i, 0] < 0 || points[i, 0] >= parent.width) return false; //no overboard
                if (parent.points[points[i, 0], points[i, 1]+1] != 0) return false; //no overlap
            }
            return true;
        }
        */
        public int[,] runAction(Actions action= Actions.none)
        {
            int[,] points;
            points = getPosition();
            switch (action)
            {
                case Actions.left:
                    for(int i = 0; i < 4; i++)
                    {
                        points[i, 0] = points[i,0] - 1;
                    }
                    break;
                case Actions.right:
                    for (int i = 0; i < 4; i++)
                    {
                        points[i, 0] = points[i,0] + 1 ;
                    }
                    break;
                case Actions.down:
                    for (int i = 0; i < 4; i++)
                    {
                        points[i, 1] = points[i, 1] + 1 ;
                    }
                    break;
                case Actions.transform_cw:
                    points = getPose(typeIndex, (poseIndex +1)% poseNum);
                    for (int i = 0; i < 4; i++)
                    {
                        points[i, 0] += offset[0];
                        points[i, 1] += offset[1];
                    }
                    break;
                case Actions.transform_anticw:
                    points = getPose(typeIndex, (poseIndex -1+ poseNum) % poseNum);
                    for (int i = 0; i < 4; i++)
                    {
                        points[i, 0] += offset[0];
                        points[i, 1] += offset[1];
                    }
                    break;
                case Actions.none:
                    points = getPosition();
                    break;
                default:
                    points = getPosition();
                    break;
            }
            return points;
        }
        
        public override void onKeyPressedEvent(ConsoleKeyInfo key)
        {
            /*
            switch (key.Key)
            {
                case ConsoleKey.A:
                case ConsoleKey.LeftArrow:
                    if (!isOverlap(runAction(Actions.left)))
                    {
                        offset[0]--;
                    }
                    break;
                case ConsoleKey.D:
                case ConsoleKey.RightArrow:
                    //moveRight();
                    if (!isOverlap(runAction(Actions.right)))
                    {
                        offset[0]++;
                    }
                    break;
                case ConsoleKey.W:
                case ConsoleKey.UpArrow:
                    if (!isOverlap(runAction(Actions.transform_cw)))
                    {
                        poseIndex = (poseIndex + 1) % poseNum;
                        data = getPose(typeIndex,poseIndex);
                    }
                    break;
                case ConsoleKey.S:
                case ConsoleKey.DownArrow:
                    if (!isOverlap(runAction(Actions.transform_anticw)))
                    {
                        poseIndex = (poseIndex - 1+poseNum) % poseNum;
                        data = getPose(typeIndex, poseIndex);
                    }
                    break;
                default:
                    break;
            }
            */
        }
        public override void update()
        {
            int[,] nextPosition = runAction(Actions.down);
            if (!isOverlap(nextPosition) && !isBottom(nextPosition))
            {
                erase();
                offset[1]++;
                display();
                isStop = false;
            }
            else
            {
                int[,] position = getPosition();
                for(int i = 0; i < 4; i++)
                {
                    parent.points[position[i, 0], position[i, 1]]=1;
                }
                reset();
                isStop = true;
            }
        }
        public void display()
        {
            for (int i = 0; i < 4; i++)
            {
                output(data[i, 0]+offset[0], data[i, 1]+offset[1], "■");
            }
        }
        public void erase()
        {
            for(int i = 0; i < 4; i++)
            {
                output(data[i, 0]+offset[0], data[i, 1]+offset[1], " ");
            }
        }

    }

    class Tetris : Game
    {
        public enum Actions { left, right, down, transform_cw,transform_anticw, none };
        public int[,] points;
        private Block block;
        private int _score;
        public Tetris(int w=18,int h=25,int x=6,int y=3,int t=800): base(w,h,x,y,t)
        {
            
        }
        protected override void init()
        {
            Console.Clear();
            _score = 0;
            points = new int[this.width,this.height];
            block = new Block(this);
            showScore();
            showNextBlock();
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
                else if (isEnd)
                {
                    if (confirm("Do you want to leave?"))
                    {
                        Console.Clear();
                        return;
                    }
                    autoEventInput.Set();
                    autoEventUpdate.Set();
                    isEnd = false;
                }
                else if (reload)
                {
                    if (confirm("Do you want to reload the game?"))
                    {
                        Console.Clear();
                        init(); // include autoEvent1.Set() autoEvent2.Set()
                    }
                    autoEventInput.Set();
                    autoEventUpdate.Set();
                    reload = false;
                }
            }
        }

        protected override void onKeyPressed()
        {
            while (true)
            {
                ConsoleKeyInfo key = Console.ReadKey();
                Console.SetCursorPosition(Console.CursorLeft - 1, Console.CursorTop);
                Console.WriteLine(" ");
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
                        if (deltaT <= 900) deltaT += 100;
                        break;
                    case ConsoleKey.PageUp:
                        if (deltaT > 100) deltaT -= 100;
                        break;

                    case ConsoleKey.A:
                    case ConsoleKey.LeftArrow:
                        lock (block)
                        {
                            if (!block.isOverlap(block.runAction(Actions.left)))
                            {
                                block.erase();
                                block.offset[0]--;
                                block.display();
                            }
                        }
                        break;
                    case ConsoleKey.D:
                    case ConsoleKey.RightArrow:
                        lock (block)
                        {
                            if (!block.isOverlap(block.runAction(Actions.right)))
                            {
                                block.erase();
                                block.offset[0]++;
                                block.display();
                            } 
                        }
                        break;
                    case ConsoleKey.W:
                    case ConsoleKey.UpArrow:
                        lock (block)
                        {
                            
                            if (!block.isOverlap(block.runAction(Actions.transform_cw)))
                            {
                                block.erase();
                                block.poseIndex = (block.poseIndex + 1) % Block.poseNum;
                                block.data = block.getPose(block.typeIndex, block.poseIndex);
                                block.display();
                            }
                        }
                        break;
                    case ConsoleKey.S:
                    case ConsoleKey.DownArrow:
                        lock (block)
                        {
                            block.update();
                        }
                        break;
                    default:
                        break;
                }
            }
        }
        private int eraseLine()
        {
            int score = 0;
            for(int i = height - 1; i >= 1; i--)
            {
                int sum = 0;
                for(int j = 0; j < width; j++)
                {
                    if (points[j,i] != 0) sum++;
                }
                if (sum < width) continue;  //if the floor is not full, turn to the upper floor
                score++;

                for (int k = i; k >= 1; k--)
                {
                    for (int j = 0; j < width; j++) 
                    {
                        points[j,k] = points[j,k-1];
                    }
                }
                i++;
            }
            if (score > 0)
            {
                //clear();
                display();
            }
            return score;
        }
        private void display(string symbol= "■")
        {
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    Console.SetCursorPosition(i * 2 + offsetX, j + offsetY);
                    if (points[i, j] == 0)
                    {
                        Console.WriteLine(" ");
                    }
                    else
                    {
                        Console.WriteLine(symbol);
                    }
                }
            }
        }
        private void clear()
        {
            for(int i = 0; i < width; i++)
            {
                for(int j = 0; j < height; j++)
                {
                    Console.SetCursorPosition(offsetX+i*2,offsetY+j);
                    Console.WriteLine(" ");
                }
            }
        }
        private void showScore()
        {
            Console.SetCursorPosition(offsetX + width * 2 + 10, offsetY);
            Console.WriteLine("score : {0}", _score);
        }
        private void showNextBlock()
        {
            int x = offsetX + width * 2 + 10;
            int y = offsetY + 2;
            Console.SetCursorPosition(x, y);
            Console.WriteLine("Next Block");
            x += 5;
            y += 3;
            int[,] pose = block.getPose(block.nextTypeIndex, block.nextPoseIndex);
            
            for(int i = -2; i < 3; i++)
            {
                for(int j = -2; j < 3; j++)
                {
                    Console.SetCursorPosition(x + i * 2, y + j);
                    Console.WriteLine(" ");
                }
            }
            for (int i = 0; i < 4; i++)
            {
                Console.SetCursorPosition(x + pose[i, 0] * 2, y + pose[i, 1]);
                Console.WriteLine("■");
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
                lock (block)
                {
                    block.update();
                    if (block.isStop)
                    {
                        _score += eraseLine();
                        showScore();
                        block.isStop = false;
                    }
                    showNextBlock();
                }
            }
        }
    }
}
