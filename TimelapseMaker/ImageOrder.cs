using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;

namespace TimelapseMaker
{
    public class ImageOrder
    {
        public void Run(string root)
        {
            DirectoryInfo rawdir = new DirectoryInfo(root +"\\"+ TimelapseProviderConfig.Currnet.rawdir);

            if (rawdir.Exists == false) throw new Exception("초기폴더의 집합폴더가 없습니다.");
            foreach (DirectoryInfo projectdir in rawdir.GetDirectories())
            {
                //이미 작업한 것은 패스
                if (projectdir.Name.Substring(projectdir.Name.Length - 3, 3) == "_ok") continue;

                //초기폴더이름 규칙이 다르면 패스
                Regex rx = new Regex("^.*_Project_p[0-9]");
                if (rx.IsMatch(projectdir.Name) == false) continue;

                //초기폴더 아래 아무것도 없으면
                if (0 >= projectdir.GetDirectories().Length) continue;

                // 프로젝트 이름 설정 후 폴더 생성
                string creationtime = projectdir.GetFiles("*.jpg", SearchOption.AllDirectories)[0].CreationTime.ToString("yyyy-MM-dd");
                string projectName = Regex.Match(projectdir.Name, @"Project_p[0-9]*", RegexOptions.None).ToString();
                DirectoryInfo orderdir = new DirectoryInfo(root + "\\" + TimelapseProviderConfig.Currnet.orderdir + "\\" + creationtime + "_" + projectName);
                if (orderdir.Exists == false) orderdir.Create();

                int file_index = 1;
                foreach (DirectoryInfo unitdir in projectdir.GetDirectories())
                {
                    //가끔 (null)도 있음. 그놈은 패스
                    rx = new Regex("^.*_Snapshot_p[0-9]");
                    if (rx.IsMatch(unitdir.Name) == false) continue;

                    //파일 이름 만들기
                    //string unitName = Regex.Match(unitdir.Name, @"Snapshot_p[0-9]*", RegexOptions.None).ToString();
                    //unitName = unitName.Substring(10, unitName.Length - 10).PadLeft(20, '0');

                    //파일 복사
                    FileInfo fi = new FileInfo(unitdir.FullName + "\\original.jpg");
                    //파일없으면 무시
                    if (fi.Exists == false) continue;
                    //예) 2013-03-22_Project_p5_1.jpg
                    string copypath = orderdir.FullName + "\\" + creationtime + "_" + projectName + "_" + file_index.ToString().PadLeft(5, '0') + ".jpg";
                    fi.CopyTo(copypath,true);
                    
                    file_index++;
                }

                projectdir.MoveTo(projectdir.FullName + "_ok");
                System.Console.Out.WriteLine(projectName + " 정렬 완료");
            }
        }
    }
}
