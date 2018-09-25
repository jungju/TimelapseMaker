using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace TimelapseMaker
{
    public class TimelapseProviderConfig
    {
        public static TimelapseProviderConfig Currnet = Init();

        public string destinationdir = "";
        public int intervalsec = 380;
        public int fps = 30;
        public string rawdir = "rawdir";

        public string orderdir = "rawdir_order";
        public string timelapseapp = "osnap";
        public string export = "export";

        private static TimelapseProviderConfig Init()
        {
            TimelapseProviderConfig cur = new TimelapseProviderConfig();
            //cur.ConfigFileLoad();

            return cur;
        }

        private void ConfigFileLoad()
        {
            string[] lines = System.IO.File.ReadAllLines(@"D:\미정리_영상\Timelapse\osnap\timelapse.conf");

            foreach (string line in lines)
            {
                string[] nameval = line.Split(new char[] { '=' });
                if (nameval.Length <= 1) continue;
                string val = nameval[1].Trim();
                switch (nameval[0].Trim())
                {
                    case "destinationdir":
                        destinationdir = val;
                        break;
                    case "intervalsec":
                        intervalsec = int.Parse(val);
                        break;
                    case "fps":
                        fps = int.Parse(val);
                        break;
                    case "rawdir":
                        rawdir = val;
                        break;
                    case "orderdir":
                        orderdir = val;
                        break;
                    case "timelapseapp":
                        timelapseapp = val;
                        break;
                    case "export":
                        export = val;
                        break;
                }
            }
        }
    }
}
