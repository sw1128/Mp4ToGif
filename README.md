# Mp4ToGif
初学者自制的简易Mp4转Gif工具
## 效果演示
![](https://zwhy-1310134253.cos.ap-beijing.myqcloud.com/mp4togif_1.gif)
## 起因
写博客需要用到动图，当录制完视频之后，在网上寻找mp4转gif的在线网站，有很多转换出来的效果不好，还有一些需要开通会员或者付费才能下载。有人推荐格式工厂，但是目前需求没有那么多，也不想下载它100MB庞大的客户端
## 经过
通过度娘了解到ffmpeg这款音视频处理神器，但是它是运行在cmd控制台中的，需要输入命令才能使用，多少有点不方便，还容易忘记命令格式
## 准备工作
### 系统环境变量
将下载好的ffmpeg文件中的bin文件夹添加到系统环境变量Path中

![](https://zwhy-1310134253.cos.ap-beijing.myqcloud.com/mp4togif_2.png)

在终端中输入`ffmpeg -version`，像这样就是配置好了

![](https://zwhy-1310134253.cos.ap-beijing.myqcloud.com/mp4togif_3.png)
### IDE及编程语言
IDE：`Visual Studio 2022`
编程语言：`C#`
### 需要用到的命名空间
```C#
using System;
using System.Windows.Forms;
using System.Diagnostics;
using WMPLib;
```
其中WMPLib命名空间需要安装COM组件中的Windows Media Player
![](https://zwhy-1310134253.cos.ap-beijing.myqcloud.com/mp4togif_4.png)
### 设计软件的布局
![](https://zwhy-1310134253.cos.ap-beijing.myqcloud.com/mp4togif_5.png)
## 实现功能
### ffmpeg命令的使用方法
`ffmpeg -ss 00:00 -t 3.5 -i xxx.mp4 -r 24 xxx.gif`
意思：从第0秒开始截取3.5秒的内容转为gif（每秒24帧）
### 初始化变量
```C#
string command = "";
string filePath = "";
string startTime = "";
string cutTime = "";
string frames = "";
string fileName = "";
string outputPath = "";
```
### 获取视频的总时长（秒）
```C#
public Double Duration(String file)
{
    WindowsMediaPlayer wmp = new WindowsMediaPlayerClass();
    IWMPMedia mediainfo = wmp.newMedia(file);
    return mediainfo.duration;
}
```
### 将秒转换为mm:ss写法
```C#
private string sec_to_hms(double duration)
{
    TimeSpan ts = new TimeSpan(0, 0, Convert.ToInt32(duration));
    string str = "";
    if (ts.Hours > 0)
    {
        str = "不支持一小时以上的视频";
    }
    if (ts.Hours == 0 && ts.Minutes > 0)
    {
        str = String.Format("{0:00}", ts.Minutes) + ":" + String.Format("{0:00}", ts.Seconds);
    }
    if (ts.Hours == 0 && ts.Minutes == 0)
    {
        str = "00:" + String.Format("{0:00}", ts.Seconds);
    }
    return str;
}
```
### "选择文件"按钮的点击事件
```C#
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
```
### “选择”按钮的点击事件
```C#
private void button3_Click(object sender, EventArgs e)
{
    FolderBrowserDialog folder = new FolderBrowserDialog();
    if (folder.ShowDialog() == DialogResult.OK)
    {
        fileName = folder.SelectedPath.Replace("\\", "/");
        outputPath = fileName + "/" + outfile.Text + ".gif";
        label10.Text = outputPath;
    }
}
```
### 将输入框中的各种参数组合成控制台命令并返回
```C#
private string getCommand()
{
    startTime = start.Text;
    cutTime = cut.Text;
    filePath = infile.Text;
    frames = frame.Text;
    command = "ffmpeg -ss " + startTime + " -t " + cutTime + " -i " + filePath + " -r " + frames + " " + outputPath;
    return command;
}
```
### “开始转换”按钮的点击事件
```C#
private void button2_Click(object sender, EventArgs e)
{
    if (infile.Text == "未选择")
    {
        MessageBox.Show("请选择一个文件","提示");
        return;
    }
    if (label10.Text == "")
    {
        MessageBox.Show("请选择导出路径", "提示");
        return;
    }
    else
    {
        command = getCommand();
        Process p = new Process();
        p.StartInfo.FileName = "cmd.exe";
        p.StartInfo.UseShellExecute = false; 
        p.StartInfo.RedirectStandardInput = true;
        p.StartInfo.RedirectStandardOutput = true;
        p.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
        p.StartInfo.CreateNoWindow = true;
        p.Start();
        p.StandardInput.WriteLine(command);
        System.Threading.Thread.Sleep(1000);
        p.StandardInput.WriteLine("exit");
    }
}
```
## 注意事项
如果文件名中含有空格，则无法正常转换
## 一个简单小巧的自用工具完成！
![](https://zwhy-1310134253.cos.ap-beijing.myqcloud.com/mp4togif_6.png)
