using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using WMPLib;

namespace MovieToGif
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        string command = "";
        string filePath = "";
        string startTime = "";
        string cutTime = "";
        string frames = "";
        string fileName = "";
        string outputPath = "";

        public double Duration(string file)
        {
            WindowsMediaPlayer wmp = new WindowsMediaPlayerClass();
            IWMPMedia mediainfo = wmp.newMedia(file);
            return mediainfo.duration;
        }
        private string sec_to_hms(double duration)
        {
            TimeSpan ts = new TimeSpan(0, 0, Convert.ToInt32(duration));
            string str = "";
            if (ts.Hours > 0)
            {
                str = "不支持一小时以上的视频";
            }
            else if (ts.Hours == 0 && ts.Minutes > 0)
            {
                str = $'{String.Format("{0:00}", ts.Minutes)}:{String.Format("{0:00}", ts.Seconds)}';
                // 使用字符串内插语法 $"{变量名}"
            }
            else if (ts.Hours == 0 && ts.Minutes == 0)
            {
                str = $'00:{String.Format("{0:00}", ts.Seconds)}';
            }
            return str;
        }

        private void button1_Click(object sender, EventArgs e)
        {

            OpenFileDialog file = new OpenFileDialog();
            file.Title = "选择一个mp4文件";
            file.Filter = "视频文件|*.mp4";
            if (file.ShowDialog() == DialogResult.OK)
            {
                filePath = file.FileName.Replace("\\","/");
                infile.Text = filePath;
                double totalTime = Duration(filePath);
                cut.Text = totalTime.ToString();
                string date = sec_to_hms(totalTime);
                label12.Text = date;
            }
        }

        private string getCommand()
        {
            startTime = start.Text;
            cutTime = cut.Text;
            filePath = infile.Text;
            frames = frame.Text;
            command = $"ffmpeg -ss {startTime} -t {cutTime} -i {filePath} -r {frames} {outputPath}";
            return command;
        }
        private void button2_Click(object sender, EventArgs e)
        {
            if (infile.Text == "未选择")
            {
                MessageBox.Show("请选择一个文件","提示");
            }
            else if (label10.Text == "")
            {
                MessageBox.Show("请选择导出路径", "提示");
            }
            else
            {
                command = getCommand();
                Process p = new Process();
                p.StartInfo.FileName = "cmd.exe";
                p.StartInfo.UseShellExecute = false;    //是否使用操作系统shell启动
                p.StartInfo.RedirectStandardInput = true;//接受来自调用程序的输入信息
                p.StartInfo.RedirectStandardOutput = true;//由调用程序获取输出信息
                p.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                p.StartInfo.CreateNoWindow = true;
                p.Start();
                p.StandardInput.WriteLine(command);
                System.Threading.Thread.Sleep(1000);
                p.StandardInput.WriteLine("exit");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folder = new FolderBrowserDialog();
            if (folder.ShowDialog() == DialogResult.OK)
            {
                fileName = folder.SelectedPath.Replace("\\", "/");
                outputPath = $"{fileName}/{outfile.Text}.gif";
                label10.Text = outputPath;
            }
        }
    }
}
