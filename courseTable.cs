using System;
using System.Collections.Generic;
using Course;
using Algorithm;
//!主要的类课程表(CourseTable)类

public partial class CourseTable
{
    private Dictionary<int, Room> rooms;
    private Dictionary<int, Teacher> teachers;
    private Dictionary<int, Group> groups;
    private Dictionary<int, Module> modules;
    private Dictionary<int, TimeSlot> timeslots;


    private Class[] classes;

    private int numClasses = 0;

    public CourseTable()
    {
        this.rooms = new Dictionary<int, Room>();
        this.teachers = new Dictionary<int, Teacher>();
        this.modules = new Dictionary<int, Module>();
        this.groups = new Dictionary<int, Group>();
        this.timeslots = new Dictionary<int, TimeSlot>();
    }

    //?拷贝构造函数
    public CourseTable(CourseTable cloneable)
    {
        this.rooms = cloneable.Rooms;
        this.teachers = cloneable.Teachers;
        this.modules = cloneable.Modules;
        this.groups = cloneable.Groups;
        this.timeslots = cloneable.Timeslots;
    }

    public Dictionary<int, Group> Groups
    {
        get { return this.groups; }
    }

    public Dictionary<int, TimeSlot> Timeslots
    {
        get { return this.timeslots; }
    }

    public Dictionary<int, Module> Modules
    {
        get { return this.modules; }
    }

    public Dictionary<int, Teacher> Teachers
    {
        get { return this.teachers; }
    }

    public Dictionary<int, Room> Rooms
    {
        get { return this.rooms; }
    }

    public void AddRoom(int roomId, string roomName, int capacity)
    {
        this.rooms.Add(roomId, new Room(roomId, roomName, capacity));
    }

    public void AddTeacher(int teacherId, string teacherName)
    {
        this.teachers.Add(teacherId, new Teacher(teacherId, teacherName));
    }


    public void AddModule(int moduleId, string moduleCode, string module, int[] teacherIds)
    {
        this.modules.Add(moduleId, new Module(moduleId, moduleCode, module, teacherIds));
    }


    public void AddGroup(int groupId, int groupSize, int[] moduleIds)
    {
        this.groups.Add(groupId, new Group(groupId, groupSize, moduleIds));
        this.numClasses = 0;
    }


    public void AddTimeSlot(int timeslotId, string timeslot)
    {
        this.timeslots.Add(timeslotId, new TimeSlot(timeslotId, timeslot));
    }

    /**
     * Create classes using individual's chromosome
     * 
     * One of the two important methods in this class; given a chromosome, unpack it
     * and turn it into an array of Class (with a capital C) objects. These Class
     * objects will later be evaluated by the calcClashes method, which will loop
     * through the Classes and calculate the number of conflicting timeslots, rooms,
     * professors, etc.
     * 
     * While this method is important, it's not really difficult or confusing. Just
     * loop through the chromosome and create Class objects and store them.
     * 
     * @param individual
     *///&根据染色体得到课程表,最重要的方法之一
    public void CreateClasses(Individual individual)
    {
        // Init classes
        Class[] classes = new Class[this.GetNumClasses()];

        // Get individual's chromosome
        int[] chromosome = individual.Chromosome;
        int chromosomePos = 0;
        int classIndex = 0;

        foreach (Group group in this.GetGroupsAsArray())
        {
            int[] moduleIds = group.Modules;
            foreach (int moduleId in moduleIds)
            {
                classes[classIndex] = new Class(classIndex, group.Id, moduleId);

                // Add timeslot
                classes[classIndex].TimeslotId = chromosome[chromosomePos];
                chromosomePos++;

                // Add room
                classes[classIndex].RoomId = chromosome[chromosomePos];
                chromosomePos++;

                // Add professor
                classes[classIndex].TeacherId = chromosome[chromosomePos];
                chromosomePos++;

                classIndex++;
            }
        }

        this.classes = classes;
    }


    public Room GetRoom(int roomId)
    {
        if (!this.rooms.ContainsKey(roomId))
        {
            Console.WriteLine("Rooms doesn't contain key " + roomId);
        }
        return (Room)this.rooms[roomId];
    }

    public Room GetRandomRoom()
    {
        var rand = new Random();
        Room[] roomsArray = new Room[this.rooms.Count];
        int c = 0;
        foreach (var item in this.rooms.Values)
        {
            roomsArray[c] = item;
            c++;
        }
        Room room = (Room)roomsArray[rand.Next(roomsArray.Length)];
        return room;
    }

    /**
     * Get professor from professorId
     * 
     * @param professorId
     * @return professor
     */
    public Teacher GetTeacher(int teacherId)
    {
        return this.teachers[teacherId];
    }

    /**
     * Get module from moduleId
     * 
     * @param moduleId
     * @return module
     */
    public Module GetModule(int moduleId)
    {
        return (Module)this.modules[moduleId];
    }

    /**
     * Get moduleIds of student group
     * 
     * @param groupId
     * @return moduleId array
     */
    public int[] GetGroupModules(int groupId)
    {
        Group group = (Group)this.groups[groupId];
        return group.Modules;
    }

    /**
     * Get group from groupId
     * 
     * @param groupId
     * @return group
     */
    public Group GetGroup(int groupId)
    {
        return (Group)this.groups[groupId];
    }

    /**
     * Get all student groups
     * 
     * @return array of groups
     */
    public Group[] GetGroupsAsArray()
    {
        Group[] groupArray = new Group[this.groups.Count];
        int c = 0;
        foreach (var item in this.groups.Values)
        {
            groupArray[c] = item;
            c++;
        }
        return groupArray;
    }


    public TimeSlot GetTimeslot(int timeslotId)
    {
        return (TimeSlot)this.timeslots[timeslotId];
    }


    public TimeSlot GetRandomTimeslot()
    {
        Random rand = new Random();
        TimeSlot[] timeslotArray = new TimeSlot[this.timeslots.Count];
        int c = 0;
        foreach (var item in this.timeslots.Values)
        {
            timeslotArray[c] = item;
            c++;
        }
        TimeSlot timeslot = timeslotArray[rand.Next(timeslotArray.Length)];
        return timeslot;
    }

    /**
     * Get classes
     * 
     * @return classes
     */
    public Class[] GetClasses()
    {
        return this.classes;
    }

    /**
     * Get number of classes that need scheduling
     * 
     * @return numClasses
     */
    public int GetNumClasses()
    {
        if (this.numClasses > 0)
        {
            return this.numClasses;
        }

        int numClasses = 0;
        Group[] groups = new Group[this.groups.Count];
        int c = 0;
        foreach (var item in this.groups.Values)
        {
            groups[c] = item;
            c++;
        }
        foreach (Group group in groups)
        {
            numClasses += group.Modules.Length;
        }
        this.numClasses = numClasses;

        return this.numClasses;
    }

    /**
     * Calculate the number of clashes between Classes generated by a chromosome.
     * 
     * The most important method in this class; look at a candidate timetable and
     * figure out how many constraints are violated.
     * 
     * Running this method requires that createClasses has been run first (in order
     * to populate this.classes). The return value of this method is simply the
     * number of constraint violations (conflicting professors, timeslots, or
     * rooms), and that return value is used by the GeneticAlgorithm.calcFitness
     * method.
     * 
     * There's nothing too difficult here either -- loop through this.classes, and
     * check constraints against the rest of the this.classes.
     * 
     * The two inner `for` loops can be combined here as an optimization, but kept
     * separate for clarity. For small values of this.classes.length it doesn't make
     * a difference, but for larger values it certainly does.
     * 
     * @return numClashes
     *///?检测冲突
    public int CalcClashes()
    {
        int clashes = 0;

        foreach (Class classA in this.classes)
        {
            // Check room capacity
            int roomCapacity = this.GetRoom(classA.RoomId).Capacity;
            int groupSize = this.GetGroup(classA.GroupId).Size;

            if (roomCapacity < groupSize)
            {
                clashes++;
            }

            // Check if room is taken
            foreach (Class classB in this.classes)
            {
                if (classA.RoomId == classB.RoomId && classA.TimeslotId == classB.TimeslotId
                        && classA.ClassId != classB.ClassId)
                {
                    clashes++;
                    break;
                }
            }

            // Check if professor is available
            foreach (Class classB in this.classes)
            {
                if (classA.TeacherId == classB.TeacherId
                        && classA.TimeslotId == classB.TimeslotId
                        && classA.ClassId != classB.ClassId)
                {
                    clashes++;
                    break;
                }
            }
        }

        return clashes;
    }

}