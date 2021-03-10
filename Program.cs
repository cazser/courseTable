using System;

namespace courseTableGA
{
    class Program
    {
        static void Main(string[] args)
        {
            TimeSlot timeSlot = new TimeSlot(1, "8:00am~9:30am");
            Console.WriteLine("时间段{0}在{1}", timeSlot.Id, timeSlot.TimeSegment);
        }
    }
}
