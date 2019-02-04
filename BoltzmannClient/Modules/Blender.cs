using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;


namespace BoltzmannClient
{
    class Blender
    {
        private ClientInfo ClientInfo;
        public string OutputPath = "";

        public Blender(ClientInfo clientInfo)
        {
            ClientInfo = clientInfo;
        }

        public void RunBlenderTask(RenderSetting renderSetting)
        {
            string command = BuildBatchString(renderSetting);

            Console.WriteLine("Running Blender Task");
            Console.WriteLine("Command: blender " + command);
            Process blender = new Process();
            blender.StartInfo = new ProcessStartInfo()
            {
                FileName = Program.blenderPath + "blender.exe",
                Arguments = command,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                CreateNoWindow = false
            };
            blender.Start();
            while (!blender.StandardOutput.EndOfStream)
            {
                string line = blender.StandardOutput.ReadLine();
                WriteProgress(line);
                GetOutputPath(line);
                if (line.Contains("Finished"))
                {
                    Console.WriteLine("Done");
                }
            }
            
            Console.ReadLine();
        }

        private void GetOutputPath(string line)
        {
            if (!line.Contains("Saved: "))
                return;
            OutputPath = line.Split('\'')[1];
        }

        private string BuildBatchString(RenderSetting renderSetting)
        {
            string str = "-b " + renderSetting.FileName;
            str += " -o " + renderSetting.OutputPath;
            str += " --python-expr " + '"' + "import bpy;";
            if (ClientInfo.useGPU)
            {
                str += "bpy.context.scene.cycles.device = 'GPU';";
                str += "bpy.context.scene.render.tile_x = 256;";
                str += "bpy.context.scene.render.tile_y = 128;";
            }
            else
            {
                str += "bpy.context.scene.cycles.device = 'CPU';";
                str += "bpy.context.scene.render.tile_x = 64;";
                str += "bpy.context.scene.render.tile_y = 64;";
            }
            str += "bpy.context.scene.cycles.seed = " + renderSetting.Seed + ';';
            str += "bpy.context.scene.cycles.samples = " + renderSetting.Sample + '"';
            str += " -f 1";
            return str;
        }

        public static void WriteProgress(string line)
        {
            string[] array = line.Split('|');
            int i = 0;
            while (i < array.Length && !array[i].Contains("Path Tracing Tile")) { i++; }
            if (i == array.Length)
                return;
            string str = array[i].Split(',')[0];
            int value = Convert.ToInt32(str.Split(' ').Last().Split('/')[0]);
            int max = Convert.ToInt32(str.Split(' ').Last().Split('/')[1]);
            int percent = (int)(Math.Round((double)value / (double)max * 100));
            Console.Write("\rProgress: " + percent + "%");
            if (percent == 100)
                Console.Write("\n\r");
        }

        public static string GetBlenderPath(string[] args)
        {
            string path = "";
            string[] value = System.Environment.GetEnvironmentVariable("PATH").ToLower().Split(';');
            if (value.Length == 0)
                value = System.Environment.GetEnvironmentVariable("Path").ToLower().Split(';');
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
    }
}
