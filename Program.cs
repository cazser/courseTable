using System;

namespace courseTableGA
{
    class Program
    {
        static void Main(string[] args)
        {
            Room room = new Room(1, "理工一101", 120);
            Console.WriteLine("教室{0}在{1}能容纳{2}学生", room.Id, room.Address, room.Capacity);
        }
    }
}
