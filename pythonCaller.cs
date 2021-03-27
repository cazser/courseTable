using System;
using System.Diagnostics;
using System.Runtime;
public partial class PythonCaller
{
    //调用python核心代码
    public static void RunPythonScript(string sArgName, string args = "", params string[] teps)
    {
        Process p = new Process();
        string path = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + sArgName;// 获得python文件的绝对路径（将文件放在c#的debug文件夹中可以这样操作）
                                                                                                 //Console.WriteLine(path);
                                                                                                 //path = @"C:\Users\user\Desktop\test\" + sArgName;//(因为我没放debug下，所以直接写的绝对路径,替换掉上面的路径了)

        p.StartInfo.FileName = @"python.exe";//没有配环境变量的话，可以像我这样写python.exe的绝对路径。如果配了，直接写"python.exe"即可
        string sArguments = path;
        foreach (string sigstr in teps)
        {
            sArguments += " " + sigstr;//传递参数
        }

        sArguments += " " + args;

        p.StartInfo.Arguments = sArguments;

        p.StartInfo.UseShellExecute = false;

        p.StartInfo.RedirectStandardOutput = true;

        p.StartInfo.RedirectStandardInput = true;

        p.StartInfo.RedirectStandardError = true;

        p.StartInfo.CreateNoWindow = true;

        p.Start();
        string output = p.StandardOutput.ReadToEnd();
        p.WaitForExit();//关键，等待外部程序退出后才能往下执行}
        Console.Write(output);//输出
        p.Close();

    }
    //输出打印的信息
    static void p_OutputDataReceived(object sender, DataReceivedEventArgs e)
    {
        if (!string.IsNullOrEmpty(e.Data))
        {
            AppendText(e.Data + Environment.NewLine);
        }
    }
    public delegate void AppendTextCallback(string text);
    public static void AppendText(string text)
    {
        Console.WriteLine(text);     //此处在控制台输出.py文件print的结果

    }
}