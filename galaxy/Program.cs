using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace Galaxy
{
    class Program
    {
        static int delay = 1500;
        static int delayspd = 25;
        const int FieldX = 15;
        const int FieldY = 25;
        static int X = 14;
        static int Y = 12;
        static bool EndGame = false;
        static bool Fire = false;
        static bool hit = false;
        static int hitcnt = 0;
        static List<int>[] _object = new List<int>[2];


        static void Main(string[] args)
        {
            ConsoleKeyInfo _key;
            Console.CursorVisible = false;
            Thread Play = new Thread(Game);
            Play.Start();

            while (true)
            {
                

                _key = Console.ReadKey(true);
                
                if (_key.Key == ConsoleKey.RightArrow)
                {
                    if (Y < FieldY - 1) Y++;
                }

                if (_key.Key == ConsoleKey.LeftArrow)
                {
                    if (Y != 0) Y--;
                }

                if (_key.Key == ConsoleKey.Spacebar)
                {
                    Fire = true;
                }

                if (_key.Key == ConsoleKey.Escape)
                {
                    Console.SetCursorPosition(0, FieldX + 2);
                    Environment.Exit(0);
                }

                if (_key.Key == ConsoleKey.Enter)
                {
                    if (EndGame)
                    {
                        Play.Abort();
                        EndGame = false;
                        Fire = false;
                        delay = 1500;
                        hitcnt = 0;
                        Console.SetCursorPosition(0, FieldX + 2);
                        Console.WriteLine("         ");
                        Console.SetCursorPosition(0, 0);
                        Play = new Thread(Game);
                        Play.Start();
                    }
                }
              //  Thread.Sleep(delay);
            }

        }
        static void Game()
        {
            List<int>[] canon = new List<int>[2];
            for (int k = 0; k < 2; k++)
            {
                canon[k] = new List<int>();
            }

            List<int>[] _fire = new List<int>[2];
            for (int k = 0; k < 2; k++)
            {
                _fire[k] = new List<int>();
            }

            canon[0].Add(X);
            canon[1].Add(Y);

            Thread ObjMove = new Thread(ObjectMove);
            ObjMove.Start();

            Print(canon, _fire, _object, X, Y, FieldX, FieldY);
            Console.SetCursorPosition(0, 0);

            while (true)
            {
                if (EndGame)
                {
                    ObjMove.Abort();
                    Console.SetCursorPosition(0, FieldX + 2);
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Game Over");
                    Console.ResetColor();
                    Console.SetCursorPosition(0, 0);
                    break;
                }

                if (Fire)
                {
                    _fire[0].Add(X);
                    _fire[1].Add(Y);
                    Fire = false;
                    Console.Beep(37,20);
                }

                if (_fire[0].Count != 0)
                {
                    for (int i = 0; i < _fire[0].Count; i++)
                    {
                        hit = false;
                        for (int k = 0; k < _object[0].Count; k++)
                        {
                            if (_fire[0][i] == _object[0][k] && _fire[1][i] == _object[1][k])
                            {
                                Console.Beep(500, 20);
                                _fire[0].RemoveAt(i);
                                _fire[1].RemoveAt(i);
                                _object[0].RemoveAt(k);
                                _object[1].RemoveAt(k);
                                hit = true;
                                hitcnt++;
                                if (hitcnt % 5 == 0) delay -= delayspd;
                                break;
                            }
                        }
                        if (hit) break;
                        if (_fire[0][i] == 0)
                        {
                            _fire[0].RemoveAt(i);
                            _fire[1].RemoveAt(i);
                        }
                        else _fire[0][i]--;
                    }

                }

                CanonPos(canon, X, Y);
                Print(canon, _fire, _object, X, Y, FieldX, FieldY);
                Console.SetCursorPosition(0, 0);
            }
        }

        static void ObjectMove()
        {
            Random rand = new Random();
            for (int k = 0; k < 2; k++)
            {
                _object[k] = new List<int>();
            }

            while (true)
            {
                Thread.Sleep(delay);
                _object[0].Add(0);
                _object[1].Add(rand.Next(0, FieldY));
                for (int i = 0; i < _object[0].Count; i++)
                {
                    _object[0][i]++;
                    if (_object[0][i] == FieldX)
                    {

                        EndGame = true;
                    }
                }

            }
        }

        static void CanonPos(List<int>[] canon, int x, int y)
        {
            canon[0].Add(x);
            canon[1].Add(y);
            canon[0].RemoveAt(0);
            canon[1].RemoveAt(0);
        }

        static void Print(List<int>[] canon, List<int>[] _fire, List<int>[] _object, int x, int y, int FieldX, int FieldY)
        {
            int flag = 0;
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("┌");
            for (int i = 0; i < FieldY; i++) Console.Write("─");
            Console.Write("┐");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Сбито: {0}", hitcnt);
            Console.ForegroundColor = ConsoleColor.Yellow;
            for (int i = 0; i < FieldX; i++)
            {
                Console.Write("│");
                for (int j = 0; j < FieldY; j++)
                {


                    for (int k = 0; k < _fire[0].Count; k++)
                    {
                        if (i == _fire[0][k] && j == _fire[1][k] && flag == 0)
                        {
                            Console.ForegroundColor = ConsoleColor.Cyan;
                            Console.Write("│");
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            flag++;
                        }
                    }

                    for (int k = 0; k < _object[0].Count; k++)
                    {
                        if (i == _object[0][k] && j == _object[1][k] && flag == 0)
                        {
                            Console.ForegroundColor = ConsoleColor.Blue;
                            Console.Write("▓");
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            flag++;
                        }
                    }

                    for (int k = 0; k < canon[0].Count; k++)
                    {
                        if (i == canon[0][k] && j == canon[1][k] && flag == 0)
                        {
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.Write("▄");
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            
                            flag++;
                        }
                    }

                    if (flag > 0) flag = 0;
                    else Console.Write(" ");
                }
                Console.WriteLine("│");
            }
            Console.Write("└");
            for (int i = 0; i < FieldY; i++) Console.Write("─");
            Console.WriteLine("┘");
            Console.ResetColor();
            //if (_fire[0].Count != 0)
            //{
            //    Console.WriteLine(_fire[0][0]);
            //    Console.WriteLine(_fire[1][0]);
            //}

            //if (_object[0].Count != 0)
            //{
            //    Console.WriteLine(_object[0][0]);
            //    Console.WriteLine(_object[1][0]);
            //}


        }
    }
}
