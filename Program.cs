using System;
using System.IO;
//?using System.Text.Json;
using Course;
using Algorithm;
namespace courseTableGA
{
    class Program
    {
        static void Main(string[] args)
        {

            string sArguments = @"main.py";//这里是python的文件名字
            PythonCaller.RunPythonScript(sArguments, "-u", new string[] { "one" });
            //pythonCaller.Button_Click();

            DateTime start = DateTime.Now;
            // Get a Timetable object with all the available information.
            CourseTable timetable = InitializeTimetable();

            // Initialize GA
            GeneticAlgorithm ga = new GeneticAlgorithm(200, 0.01, 0.9, 2, 5);

            // Initialize population
            Population population = ga.InitPopulation(timetable);

            // Evaluate population
            ga.EvalPopulation(population, timetable);

            // Keep track of current generation
            int generation = 1;

            // Start evolution loop
            while (ga.IsTerminationConditionMet(generation, 1000) == false
                    && ga.IsTerminationConditionMet(population) == false)
            {
                // Print fitness
                // System.out.println("G" + generation + " Best fitness: " +
                // population.getFittest(0).getFitness());

                // Apply crossover
                population = ga.CrossoverPopulation(population);

                // Apply mutation
                population = ga.MutatePopulation(population, timetable);

                // Evaluate population
                ga.EvalPopulation(population, timetable);

                // Increment the current generation
                generation++;
            }

            // Print fitness
            timetable.CreateClasses(population.GetFittest(0));

            // Print classes
            Console.WriteLine();
            Class[] classes = timetable.GetClasses();
            string path = @"output.json";
            if (File.Exists(path))
            {
                File.Delete(path);
            }

            using (FileStream fs = File.Create(path))
            {
                //?Utf8JsonWriter writer = new Utf8JsonWriter(fs);
                using (StreamWriter sw = new StreamWriter(fs))
                {
                    bool firstItem = true;
                    sw.WriteLine("[");
                    int classIndex = 1;

                    foreach (Class bestClass in classes)
                    {
                        if (firstItem)
                        {
                            firstItem = false;
                        }
                        else
                        {
                            sw.Write(",");
                        }
                        sw.WriteLine("{");

                        sw.Write("\"Class\" " + ":" + classIndex);
                        sw.Write(",");

                        sw.Write("\"Module\" : " + "\"" + timetable.GetModule(bestClass.ModuleId).ModuleName + "\"");
                        sw.Write(",");

                        sw.Write("\"Group\" : " + "\"" + timetable.GetGroup(bestClass.GroupId).Id + "\"");
                        sw.Write(",");

                        sw.Write("\"Room\" :" + "\"" + timetable.GetRoom(bestClass.RoomId).Id + "\"");
                        sw.Write(",");

                        sw.Write("\"Teacher\" : " + "\"" + timetable.GetTeacher(bestClass.TeacherId).Name + "\"");
                        sw.Write(",");

                        sw.WriteLine("\"Time\": " + "\"" + timetable.GetTimeslot(bestClass.TimeslotId).TimeSegment + "\"");


                        classIndex++;
                        sw.WriteLine("}");


                    }
                    sw.WriteLine("]");
                }
            }

            DateTime end = DateTime.Now;
            Console.WriteLine(DateDiff(start, end));

        }

        private static CourseTable InitializeTimetable()
        {
            // Create timetable
            CourseTable timetable = new CourseTable();

            // Set up rooms
            timetable.AddRoom(1, "理工一523", 120);
            timetable.AddRoom(2, "理工一521", 120);
            timetable.AddRoom(3, "理工一519", 150);
            timetable.AddRoom(4, "理工一411", 150);
            timetable.AddRoom(5, "理工一117", 150);
            timetable.AddRoom(6, "理工一429", 150);
            timetable.AddRoom(7, "理工一431", 150);
            timetable.AddRoom(8, "理工一119", 150);
            timetable.AddRoom(9, "理工一121", 150);
            timetable.AddRoom(10, "理工一131", 150);
            timetable.AddRoom(11, "崇师楼C101", 120);
            timetable.AddRoom(12, "崇师楼C102", 120);
            timetable.AddRoom(13, "崇师楼C103", 120);
            timetable.AddRoom(14, "崇师楼C104", 120);
            timetable.AddRoom(15, "崇师楼C306", 120);
            timetable.AddRoom(16, "崇师楼D304", 180);
            timetable.AddRoom(17, "体育楼209", 120);
            timetable.AddRoom(18, "体育楼211", 120);
            timetable.AddRoom(19, "体育楼305", 120);
            timetable.AddRoom(20, "体育楼307", 120);
            timetable.AddRoom(21, "体育楼309", 120);

            // Set up timeslots
            timetable.AddTimeSlot(1, "Mon 8:00 - 9:30");
            timetable.AddTimeSlot(2, "Mon 10:00 - 11:30");
            timetable.AddTimeSlot(4, "Mon 15:00 - 16:30");
            timetable.AddTimeSlot(3, "Mon 13:00 - 14:30");

            timetable.AddTimeSlot(5, "Tue 8:00 - 9:30");
            timetable.AddTimeSlot(6, "Tue 10:00 - 11:30");
            timetable.AddTimeSlot(7, "Tue 13:00 - 14:30");
            timetable.AddTimeSlot(8, "Tue 15:00 - 16:30");

            timetable.AddTimeSlot(9, "Wed 8:00 - 9:30");
            timetable.AddTimeSlot(10, "Wed 10:00 - 11:30");

            timetable.AddTimeSlot(11, "Thu 8:00 - 9:30");
            timetable.AddTimeSlot(12, "Thu 10:00 - 11:30");
            timetable.AddTimeSlot(13, "Thu 13:00 - 14:30");
            timetable.AddTimeSlot(14, "Thu 15:00 - 16:30");

            timetable.AddTimeSlot(15, "Fri 8:00 - 9:30");
            timetable.AddTimeSlot(16, "Fri 10:00 - 11:30");
            timetable.AddTimeSlot(17, "Fri 13:00 - 14:30");
            timetable.AddTimeSlot(18, "Fri 15:00 - 16:30");

            // Set up professors
            timetable.AddTeacher(1, "韩贤东");
            timetable.AddTeacher(2, "张明宇");
            timetable.AddTeacher(3, "崔艳玲");
            timetable.AddTeacher(4, "段喜梅");
            timetable.AddTeacher(5, "邸佳奇");
            timetable.AddTeacher(6, "赵丽");
            timetable.AddTeacher(7, "贺裕");
            timetable.AddTeacher(8, "邵晶波");
            timetable.AddTeacher(9, "丁金凤");
            timetable.AddTeacher(10, "于梦");
            timetable.AddTeacher(11, "赵微");
            timetable.AddTeacher(12, "外聘");
            timetable.AddTeacher(13, "周国辉");
            timetable.AddTeacher(14, "李持磊");



            // Set up modules and define the professors that teach them
            timetable.AddModule(1, "计网", "计算机网络", new int[] { 1 });
            timetable.AddModule(2, "游戏引擎", "游戏引擎原理与实现", new int[] { 2 });
            timetable.AddModule(3, "图形学", "计算机图形学", new int[] { 3 });
            timetable.AddModule(4, "游戏职业与素质拓展", "游戏职业与素质拓展", new int[] { 3 });
            timetable.AddModule(5, "游戏英语", "游戏英语", new int[] { 4 });
            timetable.AddModule(6, "Unity3D", "Unity3D", new int[] { 1 });

            timetable.AddModule(7, "游戏场景设计", "游戏场景设计", new int[] { 5 });
            timetable.AddModule(8, "ANDROID平台程序设计", "ANDROID平台程序设计", new int[] { 6 });
            timetable.AddModule(9, "数据结构", "数据结构", new int[] { 4 });
            timetable.AddModule(10, "三维动画制作技术", "三维动画制作技术", new int[] { 5 });
            timetable.AddModule(11, "游戏策划课程设计", "游戏策划课程设计", new int[] { 5 });
            timetable.AddModule(12, "游戏策划课程设计", "游戏策划课程设计", new int[] { 2 });
            timetable.AddModule(13, "UI课程设计", "UI课程设计", new int[] { 7 });
            timetable.AddModule(14, "UI课程设计", "UI课程设计", new int[] { 3 });
            timetable.AddModule(15, "概率与数理统计", "概率与数理统计", new int[] { 8 });
            timetable.AddModule(16, "二维动画制作技术", "二维动画制作技术", new int[] { 9 });

            timetable.AddModule(17, "游戏编程制作", "游戏编程制作", new int[] { 2 });
            timetable.AddModule(18, "操作系统API实践", "操作系统API实践", new int[] { 1 });
            timetable.AddModule(19, "影视特效制作", "影视特效制作", new int[] { 10 });
            timetable.AddModule(20, "高级语言程序设计", "高级语言程序设计", new int[] { 11 });
            timetable.AddModule(21, "游戏原画设计基础1", "游戏原画设计基础1", new int[] { 11 });
            timetable.AddModule(22, "线性代数", "线性代数", new int[] { 12 });
            timetable.AddModule(23, "游戏概论", "游戏概论", new int[] { 13 });
            timetable.AddModule(24, "高等数学1", "高等数学1", new int[] { 14 });










            // Set up student groups and the modules they take.
            timetable.AddGroup(1, 60, new int[] { 1, 2, 3, 4, 5, 6, 7, 8 });
            timetable.AddGroup(2, 60, new int[] { 9, 10, 11, 12, 13,
                                                  14, 15, 16, 17, 18, 19});
            timetable.AddGroup(3, 60, new int[] { 20, 21, 22, 23, 24 });
            return timetable;
        }

        private static string DateDiff(DateTime DateTime1, DateTime DateTime2)
        {
            string dateDiff = null;
            TimeSpan ts1 = new TimeSpan(DateTime1.Ticks);
            TimeSpan ts2 = new
            TimeSpan(DateTime2.Ticks);
            TimeSpan ts = ts1.Subtract(ts2).Duration();
            dateDiff = ts.Days.ToString() + "天" + ts.Hours.ToString() + "小时" + ts.Minutes.ToString() + "分钟" + ts.Seconds.ToString() + "秒";
            return dateDiff;
        }
    }
}


