namespace Course
{
    public partial class Room
    {
        private int roomId;
        private string roomAddress;
        private int capacity;

        public Room(int Id, string address, int _capa)
        {
            this.roomId = Id;
            this.roomAddress = address;
            this.capacity = _capa;
        }

        public int Id
        {
            get { return this.roomId; }
        }

        public string Address
        {
            get { return this.roomAddress; }
        }

        public int Capacity
        {
            get { return this.capacity; }
        }
    }
}