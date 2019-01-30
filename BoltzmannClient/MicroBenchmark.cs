using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Diagnostics;

namespace BoltzmannClient
{

    class MicroBenchmark
    {

        public int cpuScore = 0;
        public int gpuScore = 0;

        private string blenderPath;


        public MicroBenchmark(string[] _args)
        {
            blenderPath = GetBlenderPath(_args);

            RunCpuBenchmark();
            RunGpuBenchmark();
            
            Console.WriteLine("cpuScore: " + cpuScore);
            Console.WriteLine("gpuScore: " + gpuScore);
        }

        private string GetBlenderPath(string[] args)
        {
            string path = "";
            string[] value = System.Environment.GetEnvironmentVariable("PATH").ToLower().Split(';');

            int i = 0;
            while (i < value.Length && !value[i].Contains("blender")) { i++; }

            if (i == value.Length)
            {
                Console.WriteLine("Blender was not found in %PATH%");
                if (args.Length < 1)
                {
                    Console.WriteLine("You must specify the Blender folder's path!");
                    Console.ReadLine();
                    Environment.Exit(200);
                }
                else
                {
                    path = args[0];

                }
            }
            else
            {
                path = value[i];
            }

            if (path[path.Length - 1] != '\\')
                path += @"\";
            return path;
        }

        public int CalculateScore(int milliseconds)
        {
            return Convert.ToInt32((1 / (double)milliseconds) * 10000000);
        }

        private void RunCpuBenchmark()
        {
            Process bench_cpu = new Process();
            bench_cpu.StartInfo = new ProcessStartInfo()
            {
                FileName = blenderPath + "blender.exe",
                Arguments = @"-b ./Benchmark/bench_cpu.blend -f 1",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                CreateNoWindow = false
            };

            int milliseconds = 0;

            Console.WriteLine("Starting CPU benchmark...");
            bench_cpu.Start();
            while (!bench_cpu.StandardOutput.EndOfStream)
            {
                string line = bench_cpu.StandardOutput.ReadLine();
                if (line.Contains("Finished"))
                {
                    DateTime date = DateTime.MinValue;
                    milliseconds = Convert.ToInt32(line.Split('|')[1].Trim().Split(':')[1]) * 60 * 1000;
                    string str = line.Split('|')[1].Trim().Split(':')[2];
                    milliseconds += Convert.ToInt32(str.Split('.')[0]) * 1000;
                    milliseconds += Convert.ToInt32(str.Split('.')[1]) * 10;
                    cpuScore = CalculateScore(milliseconds);
                }
            }
            if (cpuScore == 0)
                Console.WriteLine("CPU benchmark failed.");
            else
                Console.WriteLine("CPU benchmark finished...");
        }

        private void RunGpuBenchmark()
        {
            Process bench_gpu = new Process();
            bench_gpu.StartInfo = new ProcessStartInfo()
            {
                FileName = blenderPath + "blender.exe",
                Arguments = @"-b ./Benchmark/bench_gpu.blend -f 1",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                CreateNoWindow = true
            };

            int milliseconds = 0;

            Console.WriteLine("Starting GPU benchmark...");
            bench_gpu.Start();
            while (!bench_gpu.StandardOutput.EndOfStream)
            {
                string line = bench_gpu.StandardOutput.ReadLine();
                if (line.Contains("Finished"))
                {
                    DateTime date = DateTime.MinValue;
                    milliseconds = Convert.ToInt32(line.Split('|')[1].Trim().Split(':')[1]) * 60 * 1000;
                    string str = line.Split('|')[1].Trim().Split(':')[2];
                    milliseconds += Convert.ToInt32(str.Split('.')[0]) * 1000;
                    milliseconds += Convert.ToInt32(str.Split('.')[1]) * 10;
                    gpuScore = CalculateScore(milliseconds);
                }
            }
            if (gpuScore == 0)
                Console.WriteLine("GPU benchmark failed.");
            else
                Console.WriteLine("CPU benchmark finished...");
        }
    }
}
