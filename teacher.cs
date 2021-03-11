namespace Course
{
    public partial class Teacher
    {
        private int teacherId;
        private string teacherName;

        public Teacher(int id, string name)
        {
            this.teacherId = id;
            this.teacherName = name;
        }

        public int Id
        {
            get { return this.teacherId; }
        }

        public string Name
        {
            get { return this.teacherName; }
        }
    }
}