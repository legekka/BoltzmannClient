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
        public Job Job;

        public void NewJob(Job job)
        {
            Job = job;
        }

        public void ExecuteJob()
        {
            if (Job.hasResources)
            {
                Console.WriteLine("Executing job.");
                Program.Blender.RunBlenderTask(Job.RenderSetting);
                Console.WriteLine(Program.Blender.OutputPath);
            }
        }

        public void SendResult(RenderSetting renderSetting, string outputpath)
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
