using System.Runtime.InteropServices;

namespace 面向流程控制台回合制小游戏实践
{
    internal class Program
    {
        static void Main(string[] args)
        {
            #region 窗口初始化设置
            Console.Clear();
            int window_sizex = 50;
            int window_sizey = 40;
            Console.SetWindowSize(window_sizex, window_sizey);
            Console.SetBufferSize(window_sizex, window_sizey);
            Console.CursorVisible =false;
            #endregion

            #region 游戏场景
            int gamescene = 0;
            #endregion

            #region 战斗场景

            #region 人物初始化
            int hero_positionx = 4, hero_positiony = 4;
            int hero_atk_max = 12, hero_atk_min = 9;
            int hero_hp = 100;
            Random hero_atk = new Random();
            #endregion

            #region BOSS初始化
            int boss_hp = 100;
            int boss_positionx = 20, boss_positiony = 20;
            int boss_atk_max = 13, boss_atk_min = 8;
            Random boss_atk = new Random();
            #endregion

            #region 公主初始化
            int princess_positionx = 20, princess_positiony = 5;
            string princess_icon = "○";
            #endregion

            bool fight_act = true;

            #endregion



            while (true)
            {
                switch (gamescene)
                {
                    case 0:   //开始场景
                        Console.Clear();
                        int mouseicon = 0;
                        bool weatherQuitWhile = true;  //开始场景的循环控制器
                        Console.SetCursorPosition(window_sizex/2-7, 7);
                        Console.ForegroundColor = ConsoleColor.Magenta;
                        Console.Write("打魔王，救公主");
                        while (weatherQuitWhile)
                        {
                            Console.SetCursorPosition(window_sizex/2-4, 26);
                            Console.ForegroundColor = mouseicon == 0 ? ConsoleColor.Red : ConsoleColor.White;
                            Console.Write("开始游戏");
                            Console.SetCursorPosition(window_sizex/2-4, 30);
                            Console.ForegroundColor = mouseicon == 1 ? ConsoleColor.Red : ConsoleColor.White;
                            Console.Write("结束游戏");
                            char player_action = Console.ReadKey(true).KeyChar;
                            switch (player_action)
                            {
                                case 'W':
                                case 'w':
                                    mouseicon--;
                                    if (mouseicon < 0)
                                    {
                                        mouseicon = 0;
                                    }
                                    break;
                                case 's':
                                case 'S':
                                    mouseicon++;
                                    if (mouseicon > 1)
                                    {
                                        mouseicon = 1;
                                    }
                                    break;
                                case 'j':
                                case 'J':
                                    if (mouseicon == 0)
                                    {
                                        gamescene = 1;
                                        weatherQuitWhile =false;
                                    }
                                    else if (mouseicon == 1)
                                    {
                                        Environment.Exit(0);
                                    }
                                    break;
                            }
                        }
                        break;
                    ////战斗场景
                    case 1:
                        Console.Clear();
                        #region 绘制边界墙
                        for (int i = 0; i < window_sizex/2; i++)
                        {
                            Console.SetCursorPosition(2*i, 0);
                            Console.ForegroundColor = ConsoleColor.White;
                            Console.Write("■");
                            Console.SetCursorPosition(2*i, 30);
                            Console.Write("■");
                            Console.SetCursorPosition(2*i, 39);
                            Console.Write("■");
                        }
                        for (int i = 0; i < window_sizey; i++)
                        {
                            Console.SetCursorPosition(0, i);
                            Console.ForegroundColor = ConsoleColor.White;
                            Console.Write("■");
                            Console.SetCursorPosition(window_sizex -2, i);
                            Console.Write("■");
                        }
                        #endregion

                        #region 绘制战斗人物
                        //初始化位置
                        Console.SetCursorPosition(hero_positionx, hero_positiony);
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.Write("□");
                        #endregion

                        #region 绘制BOSS
                        if (boss_hp > 0)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.SetCursorPosition(boss_positionx, boss_positiony);
                            Console.Write("●");
                        }
                        #endregion

                        #region 公主绘制
                        if (boss_hp <= 0)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.SetCursorPosition(princess_positionx, princess_positiony);
                            Console.Write(princess_icon);
                            Console.SetCursorPosition(2, 31);
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.Write("战斗胜利！请去营救公主！");
                        }
                        #endregion

                        bool whether_move = true;

                        #region 操作逻辑
                        while (whether_move)
                        {
                            char hero_movement = Console.ReadKey(true).KeyChar;
                            switch (hero_movement)
                            {
                                case 'w':
                                case 'W':
                                    Console.SetCursorPosition(hero_positionx, hero_positiony);
                                    Console.Write("  ");
                                    hero_positiony -= 1;
                                    Console.ForegroundColor = ConsoleColor.Green;
                                    if (hero_positiony <= 1)
                                    {
                                        hero_positiony = 1;
                                        Console.SetCursorPosition(hero_positionx, hero_positiony);
                                        Console.Write("□");
                                    }
                                    else if (hero_positiony == boss_positiony && hero_positionx ==boss_positionx)
                                    {
                                        hero_positiony++;
                                        Console.SetCursorPosition(hero_positionx, hero_positiony);
                                        Console.Write("□");
                                    }
                                    else if (hero_positiony == princess_positiony && hero_positionx ==princess_positionx)
                                    {
                                        hero_positiony++;
                                        Console.SetCursorPosition(hero_positionx, hero_positiony);
                                        Console.Write("□");
                                    }
                                    else
                                    {
                                        Console.SetCursorPosition(hero_positionx, hero_positiony);
                                        Console.Write("□");
                                    }
                                    break;
                                case 's':
                                case 'S':
                                    Console.SetCursorPosition(hero_positionx,hero_positiony);
                                    Console.Write("  ");
                                    hero_positiony += 1;
                                    Console.ForegroundColor = ConsoleColor.Green;
                                    if (hero_positiony >= 30)
                                    {
                                        hero_positiony = 29;
                                        Console.SetCursorPosition(hero_positionx, hero_positiony);
                                        Console.Write("□");
                                    }
                                    else if (hero_positiony == boss_positiony && hero_positionx ==boss_positionx)
                                    {
                                        hero_positiony--;
                                        Console.SetCursorPosition(hero_positionx, hero_positiony);
                                        Console.Write("□");
                                    }
                                    else if (hero_positiony == princess_positiony && hero_positionx ==princess_positionx)
                                    {
                                        hero_positiony--;
                                        Console.SetCursorPosition(hero_positionx, hero_positiony);
                                        Console.Write("□");
                                    }
                                    else
                                    {
                                        Console.SetCursorPosition(hero_positionx, hero_positiony);
                                        Console.Write("□");
                                    }
                                    break;
                                case 'a':
                                case 'A':
                                    Console.SetCursorPosition(hero_positionx, hero_positiony);
                                    Console.Write("  ");
                                    hero_positionx -= 2;
                                    Console.ForegroundColor = ConsoleColor.Green;
                                    if (hero_positionx <= 2)
                                    {
                                        hero_positionx = 2;
                                        Console.SetCursorPosition(hero_positionx, hero_positiony);
                                        Console.Write("□");
                                    }
                                    else if (hero_positiony == boss_positiony && hero_positionx ==boss_positionx)
                                    {
                                        hero_positionx += 2;
                                        Console.SetCursorPosition(hero_positionx, hero_positiony);
                                        Console.Write("□");
                                    }
                                    else if (hero_positiony == princess_positiony && hero_positionx ==princess_positionx)
                                    {
                                        hero_positionx += 2;
                                        Console.SetCursorPosition(hero_positionx, hero_positiony);
                                        Console.Write("□");
                                    }
                                    else
                                    {
                                        Console.SetCursorPosition(hero_positionx, hero_positiony);
                                        Console.Write("□");
                                    }
                                    break;
                                case 'D':
                                case 'd':
                                    Console.SetCursorPosition(hero_positionx, hero_positiony);
                                    Console.Write("  ");
                                    hero_positionx += 2;
                                    Console.ForegroundColor = ConsoleColor.Green;
                                    if (hero_positionx >= 46)
                                    {
                                        hero_positionx = 46;
                                        Console.SetCursorPosition(hero_positionx, hero_positiony);
                                        Console.Write("□");
                                    }
                                    else if (hero_positiony == boss_positiony && hero_positionx ==boss_positionx)
                                    {
                                        hero_positionx -= 2;
                                        Console.SetCursorPosition(hero_positionx, hero_positiony);
                                        Console.Write("□");
                                    }
                                    else if (hero_positiony == princess_positiony && hero_positionx ==princess_positionx)
                                    {
                                        hero_positionx -= 2;
                                        Console.SetCursorPosition(hero_positionx, hero_positiony);
                                        Console.Write("□");
                                    }
                                    else
                                    {
                                        Console.SetCursorPosition(hero_positionx,hero_positiony);
                                        Console.Write("□");
                                    }
                                    break;
                                case 'j':
                                case 'J':
                                    if ((hero_positiony == boss_positiony-1 && hero_positionx == boss_positionx) ||
                                        (hero_positiony == boss_positiony+1 && hero_positionx == boss_positionx) ||
                                        (hero_positiony == boss_positiony && hero_positionx == boss_positionx-2) ||
                                        (hero_positiony == boss_positiony && hero_positionx == boss_positionx+2))
                                    {
                                        whether_move = false;
                                        Console.SetCursorPosition(2, 31);
                                        Console.ForegroundColor = ConsoleColor.White;
                                        Console.Write("开始和Boss战斗了，按J键继续");
                                    }
                                    else if ((hero_positiony == princess_positiony-1 && hero_positionx == princess_positionx) ||
                                        (hero_positiony == princess_positiony+1 && hero_positionx == princess_positionx) ||
                                        (hero_positiony == princess_positiony && hero_positionx == princess_positionx-2) ||
                                        (hero_positiony == princess_positiony && hero_positionx == princess_positionx+2))
                                    {
                                        gamescene = 2;
                                        whether_move = false;
                                    }
                                    break;
                            }
                        }
                        while (fight_act)
                        {

                            char hero_fight = Console.ReadKey(true).KeyChar;
                            if ((hero_fight == 'j' || hero_fight == 'J') && boss_hp > 0 && hero_hp> 0)
                            {
                                int boss_hp_down = hero_atk.Next(hero_atk_min, hero_atk_max);
                                boss_hp -= boss_hp_down;
                                int hero_hp_down = boss_atk.Next(boss_atk_min, boss_atk_max);
                                hero_hp -= hero_hp_down;
                                Console.SetCursorPosition(2, 32);
                                Console.ForegroundColor = ConsoleColor.White;
                                Console.Write("                                          ");
                                Console.SetCursorPosition(2, 32);
                                Console.ForegroundColor = ConsoleColor.White;
                                Console.Write("你的攻击对boss造成{0}点伤害，boss还剩{1}点hp", boss_hp_down, boss_hp);
                                if (boss_hp <= 0)
                                {
                                    fight_act = false;
                                    whether_move = true;
                                }
                                Console.SetCursorPosition(2, 33);
                                Console.ForegroundColor = ConsoleColor.White;
                                Console.Write("                                          ");
                                Console.SetCursorPosition(2, 33);
                                Console.ForegroundColor = ConsoleColor.White;
                                Console.Write("boss对你造成{0}点伤害，你还剩{1}点hp", hero_hp_down, hero_hp);
                                if (hero_hp <= 0)
                                {
                                    Console.SetCursorPosition(2, 33);
                                    Console.ForegroundColor = ConsoleColor.White;
                                    Console.Write("                                          ");
                                    Console.SetCursorPosition(2, 33);
                                    Console.ForegroundColor = ConsoleColor.Yellow;
                                    Console.Write("战斗失败！");
                                    gamescene = 2;
                                    fight_act = false;
                                }
                            }
                        }
                        #endregion
                        break;
                    case 2: //结束场景
                        Console.Clear();
                        if (boss_hp <= 0)
                        {
                            Console.SetCursorPosition(window_sizex/2-7, 7);
                            Console.ForegroundColor = ConsoleColor.Magenta;
                            Console.Write("恭喜通关！！！");
                        }
                        else
                        {
                            Console.SetCursorPosition(window_sizex/2-7, 7);
                            Console.ForegroundColor = ConsoleColor.Magenta;
                            Console.Write("游戏失败！！！");
                        }
                        int mouseicon_end = 0;
                        bool weatherQuitWhile_end = true;  //开始场景的循环控制器
                        while (weatherQuitWhile_end)
                        {
                            Console.SetCursorPosition(window_sizex/2-6, 26);
                            Console.ForegroundColor = mouseicon_end == 0 ? ConsoleColor.Red : ConsoleColor.White;
                            Console.Write("返回开始界面");
                            Console.SetCursorPosition(window_sizex/2-4, 30);
                            Console.ForegroundColor = mouseicon_end == 1 ? ConsoleColor.Red : ConsoleColor.White;
                            Console.Write("结束游戏");
                            char player_action = Console.ReadKey(true).KeyChar;
                            switch (player_action)
                            {
                                case 'W':
                                case 'w':
                                    mouseicon_end--;
                                    if (mouseicon_end < 0)
                                    {
                                        mouseicon_end = 0;
                                    }
                                    break;
                                case 's':
                                case 'S':
                                    mouseicon_end++;
                                    if (mouseicon_end > 1)
                                    {
                                        mouseicon_end = 1;
                                    }
                                    break;
                                case 'j':
                                case 'J':
                                    if (mouseicon_end == 0)
                                    {
                                        gamescene = 0;
                                        weatherQuitWhile_end =false;
                                        #region 战斗场景

                                        #region 人物初始化
                                        hero_positionx = 4;
                                        hero_positiony = 4;
                                        hero_hp = 100;
                                        #endregion

                                        #region BOSS初始化
                                        boss_hp = 100;
                                        #endregion

                                        fight_act = true;

                                        #endregion
                                    }
                                    else if (mouseicon_end == 1)
                                    {
                                        Environment.Exit(0);
                                    }
                                    break;
                            }
                        }
                        break;
                }
            }
        }
    }
}
