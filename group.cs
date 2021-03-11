//!Group是一组学生的抽象
namespace Course
{
    public partial class Group
    {
        private int groupId;
        private int groupSize;
        private int[] moduleIds;

        public Group(int groupId, int groupSize, int[] moduleIds)
        {
            this.groupId = groupId;
            this.groupSize = groupSize;
            this.moduleIds = moduleIds;
        }

        public int Size
        {
            get { return this.groupSize; }
        }

        public int Id
        {
            get { return this.groupId; }
        }

        public int[] Modules
        {
            get { return this.moduleIds; }
        }

    }
}