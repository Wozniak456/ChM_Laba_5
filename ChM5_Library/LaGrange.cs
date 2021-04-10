using System;
using System.Text;
namespace ChM5_Library
{
    public class LaGrange
    {
        private int _size;
        private double[] array_x, array_y, a, b, d, h; 
        private double[,] A;
        private StringBuilder numerator; // чисельник
        private StringBuilder denumerator; // знаменник
        public LaGrange()
        {// 7 - the num of variant
            int k = 7 - 1;
            _size = 5;
            array_x = new double[_size];
            array_y = new double[_size];
            array_x[0] = -5 + k;    array_y[0] = YCalculate(array_x[0]);
            array_x[1] = -3 + k;    array_y[1] = YCalculate(array_x[1]);
            array_x[2] = -1 + k;    array_y[2] = YCalculate(array_x[2]);
            array_x[3] =  1 + k;    array_y[3] = YCalculate(array_x[3]);
            array_x[4] =  3 + k;    array_y[4] = YCalculate(array_x[3]);
        }
        private double YCalculate(double x)
        {
            double alfa = 1;
            double y = Math.Sin((alfa / 2) * x) + Math.Pow(x * alfa, (double)1 / 3);
            return y;
        }
        public void NodesShow()
        {
            Console.WriteLine("Таблиця з заданими точками\n");
            for (int i = 0; i < _size; i++)
            {
                Console.Write($"{array_x[i]:f2} | ");
            }
            Console.WriteLine("\n--------------------------------");
            for (int i = 0; i < _size; i++)
            {
                Console.Write($"{array_y[i]:f2} | ");
            }
            Console.WriteLine();
        }
        public void l (int k)
        {
            numerator.Append($"{array_y[k]:f2}");
            for (int i = 0; i < _size; i++)
            {
                if (i != k)
                {
                    numerator.Append($"(x-{array_x[i]:f0})");
                    denumerator.Append($"({array_x[k]}-{array_x[i]})");
                }
            }
            numerator.Append("    ");
            denumerator.Append("        ");
        }
        public void LaGrangeEquation()
        {
            numerator = new StringBuilder(); denumerator = new StringBuilder();
            for (int k = 0; k < _size; k++)
            {
                l(k);
            }
            Console.WriteLine($"\t{numerator}");
            Console.WriteLine("L(x) = ---------------------  +  ----------------------  +   ----------------------  +   ----------------------  +    ----------------------  ");
            Console.WriteLine($"\t{denumerator}  \n");
        }
        private void Spline(double x0)
        {
            a = new double[_size - 1]; b = new double[_size - 1];
            d = new double[_size - 1]; h = new double[_size - 1];
            A = new double[_size - 1, _size]; 
            for (int i = 0; i < _size - 1; i++)
            {
                a[i] = array_y[i];              //а_і = у_і
                h[i] = array_x[i + 1] - array_x[i]; //масив проміжків між х-значеннями
            }
            A[0, 0] = 1;
            A[_size - 2, _size - 2] = 1;
            // заповнення трьохдіагональної матриці
            for (int i = 1; i < _size - 2; i++)
            {
                A[i, i - 1] = h[i - 1];
                A[i, i] = 2 * (h[i - 1] + h[i]);
                A[i, i + 1] = h[i];
            }
            //знаходження масиву коефіцієнта с
            double[] c = Progon();
            //знаходження коефіцієнта b i d
            for (int i = 0; i < _size - 1; i++)
            {
                if (i != _size - 2)
                {
                    d[i] = (c[i + 1] - c[i]) / (3 * h[i]);
                    b[i] = ((array_y[i + 1] - array_y[i]) / h[i]) - h[i] * (c[i + 1] + 2 * c[i]) / 3;
                }
                else
                {
                    d[i] = (-1) * (c[i] / (3 * h[i]));
                    b[i] = ((array_y[i] - array_y[i - 1]) / h[i]) - ((2 * h[i] * c[i]) / 3);
                }
            }
            d[_size - 2] = -c[_size - 2] / (3 * h[_size - 2]);
            b[_size - 2] = ((array_y[_size - 1] - array_y[_size - 2]) / h[_size - 2]) - 2 * h[_size - 2] * c[_size - 2] / 3;
            int m = 0;
            for (int i = 1; i < _size; i++)
            {
                if (x0 >= array_x[i - 1] && x0 <= array_x[i])
                {
                    m = i - 1;
                }
            }
            Console.WriteLine($"Коефiцiєнти для {array_y[m]:f5}\na = {a[m]:f5}\nb = {b[m]:f5}\nc = {c[m]:f5}\nd = {d[m]:f5}\n");
            Console.WriteLine($"F({array_x[m]:f0})={a[m]:f2}+{b[m]:f2}(x-{array_x[m]:f2})+{c[m]/2:f2}(x-{array_x[m]:f2})^2+{d[m]/6:f2}(x-{array_x[m]:f2})^3\t for x є [{array_x[m]} - {array_x[m + 1]}]\n");
        }
        private double[] Progon()
        {
            double[] с = new double[_size]; // масив коефіцієнтів с
            int n1 = _size - 1;
            double y = A[0, 0]; 
            double[] a = new double[_size]; // відома матриця
            double[] B = new double[_size]; //стовпець вільних членів    
            a[0] = -A[0, 1] / y;                        
            B[0] = b[0] / y;      
            //прямий хід методу, заповнення масивів а і В
            for (int i = 1; i < n1; i++)
            {
                y = A[i, i] + A[i, i - 1] * a[i - 1];
                a[i] = -A[i, i + 1] / y;
                B[i] = (b[i] - A[i, i - 1] * B[i - 1]) / y;
            }
            //обернений хід методу, знаходження стовпця с
            с[n1] = B[n1];
            //c[n1] = (b[n1]-A[n1,n1-1]*B[n1-1])/(A[n1,n1]+A[n1,n1-1]*a[n1-1]);
            for (int i = n1 - 1; i >= 0; i--)
            {
                с[i] = a[i] * с[i + 1] + B[i];
            }
            return с;
        }
        public void Show()
        {
            for (int i = 0; i < _size; i++)
                Spline(array_x[i]);
        }
    }
}
