using System;

namespace AdventOfCode
{
    class Program
    {
        static void Main(string[] args)
        {
            var day = 2;
            IDay dayClass = new Day1();
            switch (day){
                case 1:
                    dayClass = new Day1();
                    break;
                case 2:
                    dayClass = new Day2();
                    break;
            }

            Console.WriteLine("Solution A : " + dayClass.SolveA());
            Console.WriteLine("Solution B : " + dayClass.SolveB());
        }
    }
}
