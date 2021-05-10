using Library.Models.Rooms;

namespace Library.Factory.Rooms
{
    class RoomFactory
    {

        public static Room CreateRoom(RoomType RoomType)
        {
            Room r = new();
            r.RoomType = RoomType;

            return r;
        }
    }
}
