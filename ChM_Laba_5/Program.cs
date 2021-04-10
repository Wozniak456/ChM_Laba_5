using System;
using ChM5_Library;
namespace ChM_Laba_5
{
    class Program
    {
        static void Main(string[] args)
        {
            LaGrange l = new LaGrange();
            l.NodesShow();
            Console.WriteLine();
            l.LaGrangeEquation();
            Console.WriteLine("\nIнтерполяцiя кубiчними сплайнами");
            l.Show();
        }
    }
}
