using System.ComponentModel.DataAnnotations.Schema;

namespace 飞行棋小项目
{
    internal class Program
    {
        #region 场景选择枚举
        enum E_SceneType
        {
            Start,
            Game,
            End
        }
        #endregion

        #region 控制台初始化函数
        static void ConsoleInit(int w, int l)
        {
            Console.Clear();   //清空屏幕
            Console.SetWindowSize(w, l);   //设定控制台大小
            Console.SetBufferSize(w, l);   //设定缓冲区大小
            Console.CursorVisible = false;   //设定光标隐藏
        }
        #endregion

        #region 1.开始场景逻辑函数+结束
        static void GameScene_1(int w,ref E_SceneType nowSceneType)
        {
            #region 场景搭建
            Console.SetCursorPosition(nowSceneType == E_SceneType.Start ? w/2 - 3: w/2 - 4, 10);   //游戏标题
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write(nowSceneType == E_SceneType.Start ?"飞行棋":"游戏结束");

            int mouseicon = 0;
            bool wheterquit = false;
            while (true)
            {
                Console.SetCursorPosition(nowSceneType == E_SceneType.Start ? w/2 - 4: w/2 - 6, 25);
                Console.ForegroundColor = mouseicon == 0 ? ConsoleColor.Red : ConsoleColor.White;
                Console.Write(nowSceneType == E_SceneType.Start ? "开始游戏":"返回初始界面");

                Console.SetCursorPosition(w/2 - 4, 30);
                Console.ForegroundColor = mouseicon == 1 ? ConsoleColor.Red : ConsoleColor.White;
                Console.Write("结束游戏");
                switch (Console.ReadKey(true).Key)   //得到一个输入的枚举类型
                {
                    case ConsoleKey.W:
                        --mouseicon;
                        if (mouseicon < 0)
                        {
                            mouseicon = 0;
                        }
                        break;
                    case ConsoleKey.S:
                        ++mouseicon;
                        if (mouseicon > 1)
                        {
                            mouseicon = 1;
                        }
                        break;
                    case ConsoleKey.J:
                        if (mouseicon == 0)
                        {
                            nowSceneType = nowSceneType == E_SceneType.Start? E_SceneType.Game: E_SceneType.Start;
                            wheterquit =true;
                        }
                        else
                        {
                            Environment.Exit(0);
                        }
                        break;

                }
                if (wheterquit)
                {
                    break;
                }

            }
            #endregion
        }
        #endregion

        #region 不变的红墙绘制函数
        static void PrintWall(int w,int l)
        {
            for (int i = 0; i < w; i = i+2)
            {
                Console.SetCursorPosition(i, 0);
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("■");
                Console.SetCursorPosition(i, l-11);
                Console.Write("■");
                Console.SetCursorPosition(i, l-6);
                Console.Write("■");
                Console.SetCursorPosition(i, l-1);
                Console.Write("■");
            }
            for (int i = 0;i < l; i++)
            {
                Console.SetCursorPosition(0, i);
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("■");
                Console.SetCursorPosition(w-2, i);
                Console.Write("■");
            }
        }
        #endregion

        #region 格子内容枚举
        static void UnchangedMessage(int w , int l)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.SetCursorPosition(2,l-10);
            Console.Write("□：普通格子");
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.SetCursorPosition(2,l-9);
            Console.Write("||：暂停，一回合不动");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.SetCursorPosition(25,l-9);
            Console.Write("●：倒退五格");
            Console.ForegroundColor = ConsoleColor.White;
            Console.SetCursorPosition(2,l-8);
            Console.Write("＠：时空隧道，随机倒退，暂停，换位置");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.SetCursorPosition(2,l-7);
            Console.Write("▲：玩家");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.SetCursorPosition(12,l-7);
            Console.Write("△：电脑");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.SetCursorPosition(22,l-7);
            Console.Write("◎：电脑和玩家重合");
            Console.ForegroundColor = ConsoleColor.White;
            Console.SetCursorPosition(2,l-5);
            Console.Write("按任意键开始扔骰子");
        }
        #endregion

        #region 格子结构体和格子枚举
        enum E_Grid
        {
            normal,
            boom,
            pause,
            tunnel
        }

        struct Vector2
        {
            public int x;
            public int y;

            public Vector2(int x, int y) 
            { 
                this.x = x;
                this.y = y;
            }
        }
        struct Grid       //格子结构体
        {
            public E_Grid grid;   //格子类型
            public Vector2 Position;   //格子位置
            public Grid(int x,int y, E_Grid Grid)
            {
                Position.x = x;
                Position.y = y;
                this.grid = Grid;
            }

            public void DrawGrid()
            {
                Console.SetCursorPosition(Position.x, Position.y);   //减少代码量
                switch (grid)
                {
                    case E_Grid.normal:
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.Write("□");
                        break;
                    case E_Grid.boom:
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write("●");
                        break;
                    case E_Grid.pause:
                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.Write("||");
                        break;
                    case E_Grid.tunnel:
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.Write("＠");
                        break;
                }
            }
        }
        #endregion

        #region 地图结构体
        struct Map
        {
            public Grid[] grids;

            public Map(int x, int y ,int num)
            {
                grids = new Grid[num];
                int indexX = 0,indexY = 0 ,stepNum = 2;
                Random r = new Random();
                int randomNum;
                for (int i = 0; i < num; i++)
                {
                    randomNum = r.Next(0,101);    //初始化  格子类型
                    if (randomNum < 85  || i == 0  || i == num  - 1)            //设置类型  普通格子(首位格子必定为普通格子，其它格子有85%的概率为普通格子)
                    {
                        grids[i].grid = E_Grid.normal;
                    }
                    else if (randomNum >=85 && randomNum <90)                   //设置类型  炸弹格子（几率为5%）
                    {
                        grids[i].grid = E_Grid.boom;
                    }
                    else if (randomNum >=90 && randomNum <95)                   //设置类型  暂停格子（几率为5%）
                    {
                        grids[i].grid = E_Grid.pause;
                    }
                    else                                                        //设置类型  时空隧道格子（几率为5%）
                    {
                        grids[i].grid = E_Grid.tunnel;
                    }
                    grids[i].Position = new Vector2(x, y);                      //每次循环按一定规律排列
                    if(indexX == 15)
                    {
                        y ++;
                        indexY ++;
                        if(indexY == 2)
                        {
                            indexY = 0;
                            indexX = 0;
                            stepNum = -stepNum;
                        }
                    }
                    else
                    {
                        x += stepNum;
                        indexX++;
                    }
                }
            }

            public void Draw()
            {
                for (int i = 0;i<grids.Length;i++)
                {
                    grids[i].DrawGrid();
                }
            }
        }
        #endregion

        #region 2.游戏场景逻辑函数
        static void GameScene_2(int w,int l)
        {
            #region 场景中固定元素绘制
            PrintWall(w, l);
            UnchangedMessage(w, l);
            #endregion
        }

        #endregion

        #region 玩家和电脑枚举和结构体
        enum E_playertype
        {
            Player,
            Computer
        }
        struct Player
        {
            public E_playertype type;
            public int index;
            public bool ispause;

            public Player(E_playertype type, int index)
            {
                this.type = type;
                this.index = index;
                ispause = false;
            }
            public void Draw(Map map)
            {
                Grid grid = map.grids[index];
                Console.SetCursorPosition(grid.Position.x,grid.Position.y);
                switch (type)
                {
                    case E_playertype.Player:
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.Write("▲");
                        break;
                    case E_playertype.Computer:
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write("△");
                        break;
                    default:
                        break;
                }
            }
        }
        static void DrawPlayer(Player player, Player computer, Map map)
        {
            Grid gird = map.grids[player.index];
            if (player.index == computer.index)
            {
                Console.SetCursorPosition(gird.Position.x,gird.Position.y);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write("◎");
            }
            else
            {
                player.Draw(map);
                computer.Draw(map);
            }
        }
        #endregion

        #region 玩家电脑开始投骰子
        static void EraseMessage(int l)
        {
            Console.SetCursorPosition(2, l-2);
            Console.Write("                                   ");
            Console.SetCursorPosition(2, l-3);
            Console.Write("                                   ");
            Console.SetCursorPosition(2, l-4);
            Console.Write("                                   ");
            Console.SetCursorPosition(2, l-5);
            Console.Write("                                   ");
        }
        static bool PlayerMove(int l ,ref Player p1, ref Player p2,Map map)
        {
            EraseMessage(l);
            Console.ForegroundColor = p1.type == E_playertype.Player? ConsoleColor.Cyan:ConsoleColor.Red;

            if (p1.ispause)
            {
                Console.SetCursorPosition(2,l-5);
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write("{0}踩到暂停，本回合无法行动",p1.type == E_playertype.Player ? "你":"电脑");
                p1.ispause = false;
                return false;
            }

            Random r = new Random();
            int movestep = r.Next(1, 7);
            p1.index += movestep;
            if (p1.index >= map.grids.Length-1)
            {
                p1.index = map.grids.Length-1;
                Console.SetCursorPosition(2, l-5);
                if (p1.type == E_playertype.Player)
                {
                    Console.Write("恭喜你到达了终点");
                }
                else
                {
                    Console.Write("很遗憾电脑先到达了终点");
                }
                Console.SetCursorPosition(2, l - 4);
                Console.Write("请按任意键结束游戏");
                return true;
            }
            else
            {
                Grid grid = map.grids[p1.index];
                switch (grid.grid)
                {
                    case E_Grid.normal:
                        Console.SetCursorPosition(2, l-5);
                        Console.Write("{0}掷出了{1}", p1.type == E_playertype.Player ? "你" : "电脑",movestep);
                        Console.SetCursorPosition(2, l-4);
                        Console.Write("请按任意键，让{0}开始掷", p1.type == E_playertype.Player ? "电脑" : "你");
                        break;
                    case E_Grid.boom:
                        p1.index -= 5;
                        if(p1.index <= 0)
                        {
                            p1.index = 0;
                        }
                        Console.SetCursorPosition(2, l-5);
                        Console.Write("{0}掷出了{1}", p1.type == E_playertype.Player ? "你" : "电脑", movestep);
                        Console.SetCursorPosition(2, l-4);
                        Console.Write("{0}踩到了炸弹，向后退五格", p1.type == E_playertype.Player ? "你" : "电脑");
                        Console.SetCursorPosition(2, l-3);
                        Console.Write("请按任意键，让{0}开始掷", p1.type == E_playertype.Player ? "电脑" : "你");
                        break;
                    case E_Grid.pause:
                        p1.ispause = true;
                        Console.SetCursorPosition(2, l-5);
                        Console.Write("{0}掷出了{1}", p1.type == E_playertype.Player ? "你" : "电脑", movestep);
                        Console.SetCursorPosition(2, l-4);
                        Console.Write("{0}踩到了暂定，下回合不能投掷", p1.type == E_playertype.Player ? "你" : "电脑");
                        Console.SetCursorPosition(2, l-3);
                        Console.Write("请按任意键，让{0}开始掷", p1.type == E_playertype.Player ? "电脑" : "你");
                        break;
                    case E_Grid.tunnel:
                        Console.SetCursorPosition(2, l-5);
                        Console.Write("{0}掷出了{1}", p1.type == E_playertype.Player ? "你" : "电脑", movestep);
                        Console.SetCursorPosition(2, l - 4);
                        Console.Write("{0}踩到了时空隧道", p1.type == E_playertype.Player ? "你" : "电脑");

                        //随机
                        int randomNum = r.Next(1, 91);
                        //触发 倒退
                        if (randomNum <= 30)
                        {
                            p1.index -= 5;
                            if (p1.index < 0)
                            {
                                p1.index = 0;
                            }
                            Console.SetCursorPosition(2, l - 3);
                            Console.Write("触发倒退5格");
                        }
                        //触发 暂停
                        else if (randomNum <= 60)
                        {
                            p1.ispause = true;
                            Console.SetCursorPosition(2, l - 3);
                            Console.Write("触发暂停一回合");
                        }
                        //触发换位置
                        else
                        {
                            int temp = p1.index;
                            p1.index = p2.index;
                            p2.index = temp;
                            Console.SetCursorPosition(2, l - 3);
                            Console.Write("惊喜，惊喜，双方交换位置");
                        }

                        Console.SetCursorPosition(2, l - 2);
                        Console.Write("请按任意键，让{0}开始扔色子", p1.type == E_playertype.Player ? "电脑" : "你");
                        break;

                }
            }
            return false;
        }
        #endregion

        static void Main(string[] args)
        {
            int window_w = 60, window_l = 50;
            ConsoleInit(window_w, window_l);

            E_SceneType nowSceneType = E_SceneType.End;
            while (true)
            {
                switch (nowSceneType)
                {
                    case E_SceneType.Start:  //开始场景逻辑
                        Console.Clear();
                        GameScene_1(window_w,ref nowSceneType);
                        break;
                    case E_SceneType.Game:   //游戏场景逻辑
                        Console.Clear();
                        GameScene_2(window_w, window_l);
                        Map map = new Map(12, 5, 200);
                        map.Draw();
                        Player player = new Player(E_playertype.Player ,0);
                        Player computer = new Player(E_playertype.Computer ,0);
                        DrawPlayer(player, computer, map);
                        bool wheatherstopwhile = true;
                        while(wheatherstopwhile)
                        {

                            if (player.index == map.grids.Length-1 || computer.index == map.grids.Length-1)
                            {
                                wheatherstopwhile = false;
                                Console.ReadKey(true);
                                nowSceneType = E_SceneType.End;
                            }
                            Console.ReadKey(true);
                            PlayerMove(window_l ,ref player,ref computer, map);
                            map.Draw();
                            DrawPlayer(player, computer, map);

                            Console.ReadKey(true);
                            PlayerMove(window_l, ref computer, ref player, map);
                            map.Draw();
                            DrawPlayer(player, computer, map);
                        }
                        break;
                    case E_SceneType.End:    //结束场景逻辑
                        Console.Clear();
                        GameScene_1(window_w, ref nowSceneType);
                        break;
                    default:
                        break;
                }
            }
        }

        }
    }
