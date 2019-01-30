using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace BoltzmannClient
{

    class MicroBenchmark
    {
        public int IntScore;
        public int FloatScore;
        public int DoubleScore;

        private const int n = 200;
        public double[] resultArray = new double[n];
        public int counter;


        public MicroBenchmark()
        {

            for (int i = 0; i < n; i++)
            {
                new Thread(() => { benchmark(i, "int"); }).Start();
            }
            Thread.Sleep(3000);
            CalcScore("int");


            for (int i = 0; i < n; i++)
            {
                new Thread(() => { benchmark(i, "float"); }).Start();
            }
            Thread.Sleep(3000);
            CalcScore("float");

            for (int i = 0; i < n; i++)
            {
                new Thread(() => { benchmark(i, "double"); }).Start();
            }
            Thread.Sleep(3000);
            CalcScore("double");
            Console.ReadLine();

        }


        private async void benchmark(int index, string method)
        {
            DateTime start;
            DateTime end;

            start = DateTime.Now;
            switch (method)
            {
                case "int":
                        IntMethod();
                    break;
                case "float":
                        FloatMethod();
                    break;
                case "double":
                        DoubleMethod();
                    break;
            }
            end = DateTime.Now;
            if (index<n)
            { 
                resultArray[index] = end.Subtract(start).TotalMilliseconds;
            }
        }

        private void CalcScore(string method)
        {
            double avg = 0;
            for (int i = 0; i < n; i++)
                avg += resultArray[i];
            avg /= n;
            Console.WriteLine(method + " method: " + avg + " ms avg");
        }

        private void IntMethod()
        {
            Random rnd = new Random();
            for (int i=0; i<1000000; i++)
            {
                int a = rnd.Next() + rnd.Next();
            }
        }
        
        private void FloatMethod()
        {
            Random rnd = new Random();
            for (int i = 0; i < 1000000; i++)
            {
                float a = (float)rnd.NextDouble() + (float)rnd.NextDouble();
            }
        }

        private void DoubleMethod()
        {
            Random rnd = new Random();
            for (int i = 0; i < 1000000; i++)
            {
                double a = rnd.NextDouble() + rnd.NextDouble();
            }
        }
    }
}
