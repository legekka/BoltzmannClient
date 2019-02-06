using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace BoltzmannClient
{
    class JobHandler
    {
        public static Job Job;

        public static void NewJob(Job job)
        {
            Job = job;
            Job.RenderSetting = job.RenderSetting;
            Job.JobID = job.JobID;
            Job.hasResources = job.hasResources;
        }

        public static void ExecuteJob()
        {
            if (Job.hasResources)
            {
                Console.WriteLine("Executing job.");
                Blender.RunBlenderTask(Job.RenderSetting);
            }
        }

        public static void SendResult(RenderSetting renderSetting, string outputpath)
        {
            Result result = new Result(Job.JobID, renderSetting);

            string message = "result|";
            message += JsonConvert.SerializeObject(result);
            wsClient.Send(message);

            message = "packet|";
            List<FilePacket> filePackets = FilePacker.Generate(outputpath, 32768);
            foreach (var packet in filePackets)
                wsClient.Send(message + JsonConvert.SerializeObject(packet));
        }
    }
}
