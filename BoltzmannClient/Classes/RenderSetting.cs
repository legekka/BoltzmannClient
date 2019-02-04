using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoltzmannClient
{
    class RenderSetting
    {
        public string FileName;
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
