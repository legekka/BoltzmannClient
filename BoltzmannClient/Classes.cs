using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

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

        [JsonConstructor]
        public ClientInfo(int cpuscore, int gpuscore, bool usegpu)
        {
            CpuScore = cpuscore;
            GpuScore = gpuscore;
            useGPU = usegpu;
        }
    }

    class FilePacket
    {
        public int PacketID;
        public string FileName;
        public byte[] Data;
        public bool LastPacket;

        public FilePacket(int packetId, string fileName, byte[] data)
        {
            PacketID = packetId;
            FileName = fileName;
            Data = data;
            LastPacket = false;
        }
        [JsonConstructor]
        public FilePacket(int packetId, string fileName, byte[] data, bool lastPacket)
        {
            PacketID = packetId;
            FileName = fileName;
            Data = data;
            LastPacket = lastPacket;
        }
    }

    class Job
    {
        public string JobID;
        public RenderSetting RenderSetting;
        public bool hasResources;

        [JsonConstructor]
        public Job(string jobId, RenderSetting renderSetting)
        {
            JobID = jobId;
            RenderSetting = renderSetting;
            hasResources = false;
        }
    }

    class Result
    {
        public string JobID;
        public RenderSetting RenderSetting;

        [JsonConstructor]
        public Result(string jobid, RenderSetting rendersetting)
        {
            JobID = jobid;
            RenderSetting = rendersetting;
        }
    }

    class RenderSetting
    {
        public string FileName;
        //TODO: change to OutputName
        public string OutputPath;
        public int Sample;
        public int Seed;
        public string CustomCommands;


        public RenderSetting(string filename, string outputpath, int sample, int seed)
        {
            FileName = filename;
            OutputPath = outputpath;
            Sample = sample;
            Seed = seed;
        }

        [JsonConstructor]
        public RenderSetting(string filename, string outputpath, int sample, int seed, string customcommands)
        {
            FileName = filename;
            OutputPath = outputpath;
            Sample = sample;
            Seed = seed;
            CustomCommands = customcommands;
        }
    }
}
