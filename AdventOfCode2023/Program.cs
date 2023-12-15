using System;

namespace AdventOfCode
{
    class Program
    {
        static void Main(string[] args)
        {
            var day = 15;
            IDay dayClass;
            switch (day){
                case 1:
                    dayClass = new Day1();
                    break;
                case 2:
                    dayClass = new Day2();
                    break;
                case 3:
                    dayClass = new Day3();
                    break;
                case 4:
                    dayClass = new Day4();
                    break;
                case 5:
                    dayClass = new Day5();
                    break;
                case 6:
                    dayClass = new Day6();
                    break;
                case 7:
                    dayClass = new Day7();
                    break;
                case 8:
                    dayClass = new Day8();
                    break;
                case 9:
                    dayClass = new Day9();
                    break;
                case 10:
                    dayClass = new Day10();
                    break;
                case 11:
                    dayClass = new Day11();
                    break;
                case 12:
                    dayClass = new Day12();
                    break;
                // case 13:
                //     dayClass = new Day13();
                //     break;
                case 14:
                    dayClass = new Day14();
                    break;
                case 15:
                    dayClass = new Day15();
                    break;
                // case 16:
                //     dayClass = new Day16();
                //     break;
                // case 17:
                //     dayClass = new Day17();
                //     break;
                // case 18:
                //     dayClass = new Day18();
                //     break;
                // case 19:
                //     dayClass = new Day19();
                //     break;
                // case 20:
                //     dayClass = new Day20();
                //     break;
                // case 21:
                //     dayClass = new Day21();
                //     break;
                // case 23:
                //     dayClass = new Day23();
                //     break;
                // case 24:
                //     dayClass = new Day24();
                //     break;
                default:
                    throw new NotImplementedException();
            }

            Console.WriteLine("Solution A : " + dayClass.SolveA());
            Console.WriteLine("Solution B : " + dayClass.SolveB());
        }
    }
}
