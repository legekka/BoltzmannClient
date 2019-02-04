using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoltzmannClient
{
    class ClientInfo
    {
        public int CpuScore;
        public int GpuScore;
        public bool useGPU = false;

        public ClientInfo()
        {
            MicroBenchmark benchmark = new MicroBenchmark();
            CpuScore = benchmark.cpuScore;
            GpuScore = benchmark.gpuScore;
            if (GpuScore > CpuScore)
                useGPU = true;
        }
    }
}
