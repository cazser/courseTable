using System.Collections.Generic;
using Course;

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


    public void addModule(int moduleId, String moduleCode, String module, int professorIds[])
    {
        this.modules.put(moduleId, new Module(moduleId, moduleCode, module, professorIds));
    }

    /**
     * Add new group
     * 
     * @param groupId
     * @param groupSize
     * @param moduleIds
     */
    public void addGroup(int groupId, int groupSize, int moduleIds[])
    {
        this.groups.put(groupId, new Group(groupId, groupSize, moduleIds));
        this.numClasses = 0;
    }

    /**
     * Add new timeslot
     * 
     * @param timeslotId
     * @param timeslot
     */
    public void addTimeslot(int timeslotId, String timeslot)
    {
        this.timeslots.put(timeslotId, new Timeslot(timeslotId, timeslot));
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
     */
    public void createClasses(Individual individual)
    {
        // Init classes
        Class classes[] = new Class[this.getNumClasses()];

        // Get individual's chromosome
        int chromosome[] = individual.getChromosome();
        int chromosomePos = 0;
        int classIndex = 0;

        for (Group group : this.getGroupsAsArray())
        {
            int moduleIds[] = group.getModuleIds();
            for (int moduleId : moduleIds)
            {
                classes[classIndex] = new Class(classIndex, group.getGroupId(), moduleId);

                // Add timeslot
                classes[classIndex].addTimeslot(chromosome[chromosomePos]);
                chromosomePos++;

                // Add room
                classes[classIndex].setRoomId(chromosome[chromosomePos]);
                chromosomePos++;

                // Add professor
                classes[classIndex].addProfessor(chromosome[chromosomePos]);
                chromosomePos++;

                classIndex++;
            }
        }

        this.classes = classes;
    }

    /**
     * Get room from roomId
     * 
     * @param roomId
     * @return room
     */
    public Room getRoom(int roomId)
    {
        if (!this.rooms.containsKey(roomId))
        {
            System.out.println("Rooms doesn't contain key " + roomId);
        }
        return (Room)this.rooms.get(roomId);
    }
    /**
     * Get random room
     * 
     * @return room
     */
    public Room getRandomRoom()
    {
        Object[] roomsArray = this.rooms.values().toArray();
        Room room = (Room)roomsArray[(int)(roomsArray.length * Math.random())];
        return room;
    }

    /**
     * Get professor from professorId
     * 
     * @param professorId
     * @return professor
     */
    public Professor getProfessor(int professorId)
    {
        return (Professor)this.professors.get(professorId);
    }

    /**
     * Get module from moduleId
     * 
     * @param moduleId
     * @return module
     */
    public Module getModule(int moduleId)
    {
        return (Module)this.modules.get(moduleId);
    }

    /**
     * Get moduleIds of student group
     * 
     * @param groupId
     * @return moduleId array
     */
    public int[] getGroupModules(int groupId)
    {
        Group group = (Group)this.groups.get(groupId);
        return group.getModuleIds();
    }

    /**
     * Get group from groupId
     * 
     * @param groupId
     * @return group
     */
    public Group getGroup(int groupId)
    {
        return (Group)this.groups.get(groupId);
    }

    /**
     * Get all student groups
     * 
     * @return array of groups
     */
    public Group[] getGroupsAsArray()
    {
        return (Group[])this.groups.values().toArray(new Group[this.groups.size()]);
    }

    /**
     * Get timeslot by timeslotId
     * 
     * @param timeslotId
     * @return timeslot
     */
    public Timeslot getTimeslot(int timeslotId)
    {
        return (Timeslot)this.timeslots.get(timeslotId);
    }

    /**
     * Get random timeslotId
     * 
     * @return timeslot
     */
    public Timeslot getRandomTimeslot()
    {
        Object[] timeslotArray = this.timeslots.values().toArray();
        Timeslot timeslot = (Timeslot)timeslotArray[(int)(timeslotArray.length * Math.random())];
        return timeslot;
    }

    /**
     * Get classes
     * 
     * @return classes
     */
    public Class[] getClasses()
    {
        return this.classes;
    }

    /**
     * Get number of classes that need scheduling
     * 
     * @return numClasses
     */
    public int getNumClasses()
    {
        if (this.numClasses > 0)
        {
            return this.numClasses;
        }

        int numClasses = 0;
        Group groups[] = (Group[])this.groups.values().toArray(new Group[this.groups.size()]);
        for (Group group : groups)
        {
            numClasses += group.getModuleIds().length;
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
     */
    public int calcClashes()
    {
        int clashes = 0;

        for (Class classA : this.classes)
        {
            // Check room capacity
            int roomCapacity = this.getRoom(classA.getRoomId()).getRoomCapacity();
            int groupSize = this.getGroup(classA.getGroupId()).getGroupSize();

            if (roomCapacity < groupSize)
            {
                clashes++;
            }

            // Check if room is taken
            for (Class classB : this.classes)
            {
                if (classA.getRoomId() == classB.getRoomId() && classA.getTimeslotId() == classB.getTimeslotId()
                        && classA.getClassId() != classB.getClassId())
                {
                    clashes++;
                    break;
                }
            }

            // Check if professor is available
            for (Class classB : this.classes)
            {
                if (classA.getProfessorId() == classB.getProfessorId()
                        && classA.getTimeslotId() == classB.getTimeslotId()
                        && classA.getClassId() != classB.getClassId())
                {
                    clashes++;
                    break;
                }
            }
        }

        return clashes;
    }

}