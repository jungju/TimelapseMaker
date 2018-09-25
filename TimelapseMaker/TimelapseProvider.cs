using AForge.Video.FFMPEG;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Text;
using System.IO;

namespace TimelapseMaker
{
    public class TimelapseProvider
    {
        public void AllCreateVideo(string root, int frame)
        {
            DirectoryInfo orderdir = new DirectoryInfo(root + "\\" + TimelapseProviderConfig.Currnet.orderdir);

            foreach (DirectoryInfo prodir in orderdir.GetDirectories())
            {
                if (prodir.Name == "0_del" || prodir.Name.Substring(prodir.Name.Length - 3, 3) == "_ok") continue;

                CreateVideo(root, prodir.FullName, frame);
                prodir.MoveTo(prodir.FullName + "_ok");
                System.Console.Out.WriteLine(prodir.Name + " 영상 완료");
            }
            
        }

        public void CreateVideo(string root, string imagesPath, int frame)
        {
            //이미지 로드
            DirectoryInfo dirInfo = new DirectoryInfo(imagesPath);
            if (dirInfo.Exists == false)
            {
                throw new Exception("folder not exists");
            }

            string firstimage = dirInfo.GetFiles("*.*", SearchOption.TopDirectoryOnly)[0].FullName;
            Bitmap image = new Bitmap(firstimage);
            int width = image.Width;
            int height = image.Height;
            image.Dispose();
            
            // create instance of video writer
            VideoFileWriter writer = new VideoFileWriter();
            // create new video file
            writer.Open(root + "\\" + TimelapseProviderConfig.Currnet.destinationdir + "\\" + dirInfo.Name + ".avi", width, height, frame, VideoCodec.Raw, 2000000);
            foreach (FileInfo fileInfo in dirInfo.GetFiles("*.*", SearchOption.TopDirectoryOnly))
            {
                image = new Bitmap(fileInfo.FullName);
                writer.WriteVideoFrame(image);
                image.Dispose();
            }
            writer.Close();
        }
    }
}
