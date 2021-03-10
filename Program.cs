using System;

namespace courseTableGA
{
    class Program
    {
        static void Main(string[] args)
        {
            Teacher teacher = new Teacher(1, "王富贵");
            Console.WriteLine("{0}号老师叫{1}", teacher.Id, teacher.Name);
        }
    }
}
