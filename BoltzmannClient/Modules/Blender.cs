using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;


namespace BoltzmannClient
{
    class Blender
    {

        public Blender()
        {
        }

        public static void RunBlenderTask(RenderSetting renderSetting)
        {
            string command = BuildBatchString(renderSetting);

            
            Console.WriteLine("Command: blender " + command);
            Process blender = new Process();
            blender.StartInfo = new ProcessStartInfo()
            {
                FileName = Program.blenderPath,
                Arguments = command,
                UseShellExecute = true,
                //RedirectStandardOutput = true,
                CreateNoWindow = false
            };
            
            Console.WriteLine("Starting Blender");
            blender.Start();
            /*while (!blender.StandardOutput.EndOfStream)
            {
                string line = blender.StandardOutput.ReadLine();
                WriteProgress(line);
                GetOutputPath(line);
                if (line.Contains("Finished"))
                {
                    Console.WriteLine("Done");
                }
            } 
            */
            blender.WaitForExit();
            if (File.Exists(Directory.GetCurrentDirectory() + @"\Blender\" + renderSetting.OutputPath + "0001.png"))
            {
                Console.WriteLine(renderSetting.OutputPath + "0001.png finished.");
                string OutputPath = Directory.GetCurrentDirectory() + @"\Blender\" + renderSetting.OutputPath + "0001.png";
                JobHandler.SendResult(renderSetting, OutputPath);
                Console.WriteLine(OutputPath);
            }
            else
            {
                Console.WriteLine("FATAL ERROR DETECTED!!! THE SYSTEM WILL DESTROY ITSELF NOW!");
            }

        }

        private static string BuildBatchString(RenderSetting renderSetting)
        {
            string str = @"-b ./Blender/" + renderSetting.FileName;
            str += " -o " + '"' + Directory.GetCurrentDirectory() + @"\Blender\" + renderSetting.OutputPath + '"';
            str += " --python-expr " + '"' + "import bpy;";
            if (Program.clientInfo.useGPU)
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

            if (path.First() != '"' && path.Contains(' '))
                path = '"' + path + '"';

            if (path.Last() == '"')
                if (path[path.Length - 2] != '\\')
                    path = path.Insert(path.Length - 1, "\\blender.exe");
                else
                    if (path[path.Length - 1] != '\\')
                    path += @"\blender.exe";
            return path;
        }
    }
}
