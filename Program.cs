﻿using System;
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
            DateTime start = DateTime.Now;
            // Get a Timetable object with all the available information.
            CourseTable timetable = InitializeTimetable();

            // Initialize GA
            GeneticAlgorithm ga = new GeneticAlgorithm(100, 0.01, 0.9, 2, 5);

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
            timetable.AddRoom(1, "A1", 15);
            timetable.AddRoom(2, "B1", 30);
            timetable.AddRoom(4, "D1", 20);
            timetable.AddRoom(5, "F1", 25);

            // Set up timeslots
            timetable.AddTimeSlot(1, "Mon 9:00 - 11:00");
            timetable.AddTimeSlot(2, "Mon 11:00 - 13:00");
            timetable.AddTimeSlot(3, "Mon 13:00 - 15:00");
            timetable.AddTimeSlot(4, "Tue 9:00 - 11:00");
            timetable.AddTimeSlot(5, "Tue 11:00 - 13:00");
            timetable.AddTimeSlot(6, "Tue 13:00 - 15:00");
            timetable.AddTimeSlot(7, "Wed 9:00 - 11:00");
            timetable.AddTimeSlot(8, "Wed 11:00 - 13:00");
            timetable.AddTimeSlot(9, "Wed 13:00 - 15:00");
            timetable.AddTimeSlot(10, "Thu 9:00 - 11:00");
            timetable.AddTimeSlot(11, "Thu 11:00 - 13:00");
            timetable.AddTimeSlot(12, "Thu 13:00 - 15:00");
            timetable.AddTimeSlot(13, "Fri 9:00 - 11:00");
            timetable.AddTimeSlot(14, "Fri 11:00 - 13:00");
            timetable.AddTimeSlot(15, "Fri 13:00 - 15:00");

            // Set up professors
            timetable.AddTeacher(1, "Dr P Smith");
            timetable.AddTeacher(2, "Mrs E Mitchell");
            timetable.AddTeacher(3, "Dr R Williams");
            timetable.AddTeacher(4, "Mr A Thompson");

            // Set up modules and define the professors that teach them
            timetable.AddModule(1, "cs1", "Computer Science", new int[] { 1, 2 });
            timetable.AddModule(2, "en1", "大学英语", new int[] { 1, 3 });
            timetable.AddModule(3, "ma1", "高等数学", new int[] { 1, 2 });
            timetable.AddModule(4, "ph1", "大学物理", new int[] { 3, 4 });
            timetable.AddModule(5, "hi1", "History", new int[] { 4 });
            timetable.AddModule(6, "dr1", "Drama", new int[] { 1, 4 });

            // Set up student groups and the modules they take.
            timetable.AddGroup(1, 10, new int[] { 1, 3, 4 });
            timetable.AddGroup(2, 30, new int[] { 2, 3, 5, 6 });
            timetable.AddGroup(3, 18, new int[] { 3, 4, 5 });
            timetable.AddGroup(4, 25, new int[] { 1, 4 });
            timetable.AddGroup(5, 20, new int[] { 2, 3, 5 });
            timetable.AddGroup(6, 22, new int[] { 1, 4, 5 });
            timetable.AddGroup(7, 16, new int[] { 1, 3 });
            timetable.AddGroup(8, 18, new int[] { 2, 6 });
            timetable.AddGroup(9, 24, new int[] { 1, 6 });
            timetable.AddGroup(10, 25, new int[] { 3, 4 });
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


