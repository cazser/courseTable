public partial class TimeSlot
{
    private int timeslotId;
    private string timeslot;

    public TimeSlot(int timeslotId, string timeslot)
    {
        this.timeslotId = timeslotId;
        this.timeslot = timeslot;
    }


    public int Id
    {
        get
        {
            return this.timeslotId;
        }
    }


    public string TimeSegment
    //&考虑到Timeslot与类名或者变量的名字太相似，所以叫TimeSegment
    {
        get
        {
            return this.timeslot;
        }
    }
}