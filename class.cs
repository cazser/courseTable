public partial class Class
{
    private int classId;
    private int groupId;
    private int moduleId;
    private int teacherId;
    private int timeslotId;
    private int roomId;
    //?构造函数
    public Class(int classId, int groupId, int moduleId)
    {
        this.classId = classId;
        this.moduleId = moduleId;
        this.groupId = groupId;
    }

    public int ClassId { get { return this.classId; } }

    public int GroupId { get { return this.groupId; } }

    public int ModuleId { get { return this.moduleId; } }
    public int TeacherId
    {
        get { return this.teacherId; }
        set { this.teacherId = value; }
    }

    public int TimeslotId
    {
        get { return this.timeslotId; }
        set { this.timeslotId = value; }
    }

    public int RoomId
    {
        get { return this.roomId; }
        set { this.roomId = value; }
    }


}