using System;

namespace AdventOfCode
{
    class Program
    {
        static void Main(string[] args)
        {
            var day = 3;
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
                default:
                    throw new NotImplementedException();
            }

            Console.WriteLine("Solution A : " + dayClass.SolveA());
            Console.WriteLine("Solution B : " + dayClass.SolveB());
        }
    }
}
