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

            //string sArguments = @"main.py";//这里是python的文件名字
            //PythonCaller.RunPythonScript(sArguments, "-u", new string[] { "one" });
            //pythonCaller.Button_Click();

            DateTime start = DateTime.Now;
            // Get a Timetable object with all the available information.
            CourseTable timetable = InitializeTimetable();

            // Initialize GA
            GeneticAlgorithm ga = new GeneticAlgorithm(800, 0.01, 0.9, 2, 5);
            //?100-0.25 0.47 1.41
            //?200-1.21 1.31 2.54 
            //?400-2.36 3.37 6.6
            //?800-7.36 10.53 15.01
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
            timetable.AddTeacher(15, "李世明");
            timetable.AddTeacher(16, "杜军");
            timetable.AddTeacher(17, "郑岩");
            timetable.AddTeacher(18, "刘仁辉");
            timetable.AddTeacher(19, "周英");
            timetable.AddTeacher(20, "倪蕴涛");
            timetable.AddTeacher(21, "穆全起");
            timetable.AddTeacher(22, "姚艳雪");
            timetable.AddTeacher(23, "朱海龙");
            timetable.AddTeacher(24, "尹启天");
            timetable.AddTeacher(25, "周英");
            timetable.AddTeacher(26, "付宝君");
            timetable.AddTeacher(27, "王金江");
            timetable.AddTeacher(28, "付伟");
            timetable.AddTeacher(29, "李志聪");
            timetable.AddTeacher(30, "刘明宇");
            timetable.AddTeacher(31, "李英梅");
            timetable.AddTeacher(32, "林琳");
            timetable.AddTeacher(33, "刘靖宇");
            timetable.AddTeacher(34, "黄玉研");
            timetable.AddTeacher(35, "石晔琼");
            timetable.AddTeacher(36, "孙鹏飞");
            timetable.AddTeacher(37, "季伟东");
            timetable.AddTeacher(38, "李晶");
            timetable.AddTeacher(39, "丁云鸿");
            timetable.AddTeacher(40, "伦立军");
            timetable.AddTeacher(41, "马宁");
            timetable.AddTeacher(42, "边奕心");
            timetable.AddTeacher(43, "于瑞彬");
            timetable.AddTeacher(44, "黄玉妍");
            timetable.AddTeacher(45, "李玉霞");
            timetable.AddTeacher(46, "朱晓");
            timetable.AddTeacher(47, "张军");
            timetable.AddTeacher(48, "于丹");
            timetable.AddTeacher(49, "张广玲");
            timetable.AddTeacher(50, "王洪侠");
            timetable.AddTeacher(51, "王秀珍");
            timetable.AddTeacher(52, "魏洪伟");
            timetable.AddTeacher(53, "刘月兰");
            timetable.AddTeacher(54, "李红宇");
            timetable.AddTeacher(55, "刘玉喜");
            timetable.AddTeacher(56, "赵松");
            timetable.AddTeacher(57, "常晓娟");
            timetable.AddTeacher(58, "刑凯");
            timetable.AddTeacher(59, "马瑞华");
            timetable.AddTeacher(60, "于延");
            timetable.AddTeacher(61, "范雪琴");
            timetable.AddTeacher(62, "伊波");
            timetable.AddTeacher(63, "魏红伟");




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

            timetable.AddModule(25, "计算机网络", "计算机网络", new int[] { 15 });
            timetable.AddModule(26, "物联网通信技术", "物联网通信技术", new int[] { 16 });
            timetable.AddModule(27, "数据处理与智能决策", "数据处理与智能决策", new int[] { 17 });
            timetable.AddModule(28, "物联网通信课程设计", "物联网通信课程设计", new int[] { 16 });
            timetable.AddModule(29, "Linux程序设计", "Linux程序设计", new int[] { 18 });
            timetable.AddModule(30, "网络攻防", "网络攻防", new int[] { 15 });

            timetable.AddModule(31, "数据结构", "数据结构", new int[] { 19 });
            timetable.AddModule(32, "数据库原理与应用", "数据库原理与应用", new int[] { 18 });
            timetable.AddModule(33, "概率与数理统计", "概率与数理统计", new int[] { 8 });
            timetable.AddModule(34, "数字电子技术", "数字电子技术", new int[] { 20, 21 });
            timetable.AddModule(35, "Linux操作系统---Linux操作系统应用(教务系统里的名称)",
                "Linux操作系统---Linux操作系统应用(教务系统里的名称)", new int[] { 22 });
            timetable.AddModule(36, "物联网专业英语", "物联网专业英语", new int[] { 21 });
            timetable.AddModule(37, "Web程序设计", "Web程序设计", new int[] { 22 });
            timetable.AddModule(38, "Python语言", "Python语言", new int[] { 23 });
            timetable.AddModule(39, "移动终端软件开发", "移动终端软件开发", new int[] { 24 });

            timetable.AddModule(40, "物联网工程导论", "物联网工程导论", new int[] { 26 });
            timetable.AddModule(41, "高级语言程序设计", "高级语言程序设计", new int[] { 25 });

            timetable.AddModule(42, "线性代数", "线性代数", new int[] { 27 });
            timetable.AddModule(43, "计算与信息素养", "计算与信息素养", new int[] { 28 });
            timetable.AddModule(44, "高级语言程序设计", "高级语言程序设计", new int[] { 29 });

            timetable.AddModule(45, "*数据结构", "*数据结构", new int[] { 30 });
            timetable.AddModule(46, "*数据库原理", "*数据库原理", new int[] { 31, 32 });
            timetable.AddModule(47, "概率与数理统计", "概率与数理统计", new int[] { 33 });
            timetable.AddModule(48, "数据结构课程设计", "数据结构课程设计", new int[] { 30, 34, 35, 32 });
            timetable.AddModule(49, "第二外语（日语）", "第二外语（日语）", new int[] { 36 });
            timetable.AddModule(50, "Web程序设计", "Web程序设计", new int[] { 35 });
            timetable.AddModule(51, "数字电路", "数字电路", new int[] { 37 });

            timetable.AddModule(52, "计算机组成原理", "计算机组成原理", new int[] { 38 });
            timetable.AddModule(53, "*软件需求分析", "*软件需求分析", new int[] { 39 });
            timetable.AddModule(54, "*操作系统", "*操作系统", new int[] { 40 });
            timetable.AddModule(55, "UML统一建模", "UML统一建模", new int[] { 41 });
            timetable.AddModule(56, "软件测试与质量保证", "软件测试与质量保证", new int[] { 42 });
            timetable.AddModule(57, "软件框架开发技术", "软件框架开发技术", new int[] { 43 });
            timetable.AddModule(58, "移动端应用开发基础", "移动端应用开发基础", new int[] { 29 });
            timetable.AddModule(59, "个人级软件开发", "个人级软件开发", new int[] { 28, 37, 41, 32 });

            timetable.AddModule(60, "专业综合", "专业综合", new int[] { 44 });
            timetable.AddModule(61, "毕业实习实训", "毕业实习实训", new int[] { 28, 37, 32, 42, 35 });

            timetable.AddModule(62, "编译原理", "编译原理", new int[] { 45, 46 });
            timetable.AddModule(63, "操作系统", "操作系统", new int[] { 47, 48 });
            timetable.AddModule(64, "单片机原理与应用", "单片机原理与应用", new int[] { 49, 50 });
            timetable.AddModule(65, "计算机网络原理", "计算机网络原理", new int[] { 51 });
            timetable.AddModule(66, "专业英语", "专业英语", new int[] { 52 });
            timetable.AddModule(67, "数据库应用与设计", "数据库应用与设计", new int[] { 49, 53 });
            timetable.AddModule(68, "SSH框架开发技术", "SSH框架开发技术", new int[] { 54 });
            timetable.AddModule(69, "UML统一建模", "UML统一建模", new int[] { 53 });
            timetable.AddModule(70, "ARM体系结构", "ARM体系结构", new int[] { 49 });
            timetable.AddModule(71, "C++程序设计", "C++程序设计", new int[] { 55 });
            timetable.AddModule(72, "编译原理课程设计", "编译原理课程设计", new int[] { 45, 46, 56 });

            timetable.AddModule(73, "数据结构", "数据结构", new int[] { 63, 57 });
            timetable.AddModule(74, "数字逻辑", "数字逻辑", new int[] { 58, 50 });
            timetable.AddModule(75, "计算方法", "计算方法", new int[] { 59 });
            timetable.AddModule(76, "计算机赋值教学", "计算机辅助教学", new int[] { 55, 56 });
            timetable.AddModule(77, "Web程序设计", "Web程序设计", new int[] { 57, 60 });
            timetable.AddModule(78, "数据结构课程设计", "数据结构课程设计", new int[] { 63, 57 });

            timetable.AddModule(79, "高级语言程序设计", "高级语言程序设计", new int[] { 61, 54 });
            timetable.AddModule(80, "计算机导论", "计算机导论", new int[] { 49 });

            timetable.AddModule(81, "高级语言程序设计", "高级语言程序设计", new int[] { 60 });
            timetable.AddModule(82, "计算机系统基础", "计算机系统基础", new int[] { 62 });

            timetable.AddModule(83, "高级语言程序设计", "高级语言程序设计", new int[] { 60 });
            timetable.AddModule(84, "计算机系统基础", "计算机系统基础", new int[] { 62 });










            // Set up student groups and the modules they take.
            timetable.AddGroup(1, 60, new int[] { 1, 2, 3, 4, 5, 6, 7, 8 });
            timetable.AddGroup(2, 60, new int[] { 9, 10, 11, 12, 13,
                                                  14, 15, 16, 17, 18, 19});
            timetable.AddGroup(3, 60, new int[] { 20, 21, 22, 23, 24 });
            timetable.AddGroup(4, 60, new int[] { 25, 26, 27, 28, 29, 30 });
            timetable.AddGroup(5, 60, new int[] { 31, 32, 33, 34, 35, 36, 37, 38, 39 });
            timetable.AddGroup(6, 60, new int[] { 40, 41 });

            timetable.AddGroup(7, 60, new int[] { 42, 43, 44 });
            timetable.AddGroup(8, 60, new int[] { 45, 46, 47, 48, 49, 50, 51 });
            timetable.AddGroup(9, 60, new int[] { 52, 53, 54, 55, 56, 57, 58, 59 });
            timetable.AddGroup(10, 60, new int[] { 60, 61 });

            timetable.AddGroup(11, 60, new int[] { 62, 63, 64, 65, 66, 67, 68, 69, 70, 71, 72 });
            timetable.AddGroup(12, 60, new int[] { 73, 74, 75, 76, 78 });
            timetable.AddGroup(13, 60, new int[] { 79, 80 });
            timetable.AddGroup(14, 60, new int[] { 81, 82 });
            timetable.AddGroup(15, 60, new int[] { 83, 84 });




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


