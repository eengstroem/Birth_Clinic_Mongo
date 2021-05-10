using Library.Context;
using Library.Models.Clinicians;
using Library.Models.Rooms;
using Library.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Repository
{
    public class ReservationRepository
    {
        public static void PrintAvailableRoomsInNext5Days(BirthClinicDbContext context)
        {
            DateTime FilterDate = DateTime.Now.AddDays(5);

            /*rInner.CurrentReservations.OrderBy(p => p.StartTime)*/

            var filterTime = DateTime.Now.AddDays(5);

            //Makes a tuple of room ID and a list of the room's reservations in the next 5 days. Also converts to list as no more processing can be done server-side.
            var MaternityRooms = context.Rooms.Where(r => r.RoomType == RoomType.MATERNITY).OrderBy(r => r.RoomId);
            var MaternityReservations = MaternityRooms.Select(r => r.CurrentReservations.Where(r => r.EndTime < filterTime).ToList()).ToList();
            var MaternityTuples = MaternityRooms.Select(r => r.RoomId).ToList().Zip(MaternityReservations).ToList();

            var BirthRooms = context.Rooms.Where(r => r.RoomType == RoomType.BIRTH).OrderBy(r => r.RoomId);
            var BirthReservations = BirthRooms.Select(r => r.CurrentReservations.Where(r => r.EndTime < filterTime)).ToList();
            var BirthTuples = BirthRooms.Select(r => r.RoomId).ToList().Zip(BirthReservations).ToList();

            var RestRooms = context.Rooms.Where(r => r.RoomType == RoomType.REST).OrderBy(r => r.RoomId);
            var RestReservations = RestRooms.Select(r => r.CurrentReservations.Where(r => r.EndTime < filterTime)).ToList();
            var RestTuples = RestRooms.Select(r => r.RoomId).ToList().Zip(RestReservations).ToList();



            var MaternityRoomDict = new Dictionary<int, List<DateTime>>();
            var BirthRoomDict = new Dictionary<int, List<DateTime>>();
            var RestRoomDict = new Dictionary<int, List<DateTime>>();

            RestTuples.ForEach(tuple =>
            {
                var slots = DateTimeUtils.FindAvailableRoomTimeSlots(tuple.Second.ToList(), TimeSpan.FromHours(4));
                if (slots.Count > 0)
                {
                    Console.WriteLine("Room " + tuple.First + " (Rest) is available from the following times");
                    Console.WriteLine(StringUtils.DateTimeListToCommaSeparatedString(slots) + "\n");
                }
            });
            BirthTuples.ForEach(tuple =>
            {
                var slots = DateTimeUtils.FindAvailableRoomTimeSlots(tuple.Second.ToList(), TimeSpan.FromHours(12));
                if (slots.Count > 0)
                {
                    Console.WriteLine("Room " + tuple.First + " (Birth) is available from the following times");
                    Console.WriteLine(StringUtils.DateTimeListToCommaSeparatedString(slots) + "\n");
                }
            });
            MaternityTuples.ForEach(tuple =>
            {
                var slots = DateTimeUtils.FindAvailableRoomTimeSlots(tuple.Second.ToList(), TimeSpan.FromDays(5));
                if (slots.Count > 0)
                {
                    Console.WriteLine("Room " + tuple.First + " (Maternity) is available from the following times");
                    Console.WriteLine(StringUtils.DateTimeListToCommaSeparatedString(slots) + "\n");
                }
            });

        }
        public static void PrintAvailableCliniciansInNext5Days(BirthClinicDbContext context)
        {
            DateTime FilterDate = DateTime.Now.AddDays(5);

            /*rInner.CurrentReservations.OrderBy(p => p.StartTime)*/

            var filterTime = DateTime.Now.AddDays(5);

            //Makes a tuple of room ID and a list of the room's reservations in the next 5 days. Also converts to list as no more processing can be done server-side.
            var Doctors = context.Clinicians.Where(r => r.Role == ClinicianType.DOCTOR).ToList();
            var DoctorsBirths = Doctors.Select(d => d.AssignedBirths.OrderBy(b => b.BirthDate).ToList()).ToList();
            var DoctorsWithBirths = Doctors.Zip(DoctorsBirths).ToList();

            var Assistants = context.Clinicians.Where(r => r.Role == ClinicianType.HEALTH_ASSISTANT).ToList();
            var AssistantsBirths = Assistants.Select(d => d.AssignedBirths.OrderBy(b => b.BirthDate).ToList()).ToList();
            var AssistantsWithBirths = Assistants.Zip(AssistantsBirths).ToList();

            var Nurses = context.Clinicians.Where(r => r.Role == ClinicianType.NURSE).ToList();
            var NursesBirths = Nurses.Select(d => d.AssignedBirths.OrderBy(b => b.BirthDate).ToList()).ToList();
            var NursesWithBirths = Nurses.Zip(NursesBirths).ToList();

            var Midwives = context.Clinicians.Where(r => r.Role == ClinicianType.MIDWIFE).ToList();
            var MidwivesBirths = Midwives.Select(d => d.AssignedBirths.OrderBy(b => b.BirthDate).ToList()).ToList();
            var MidwivesWithBirths = Midwives.Zip(MidwivesBirths).ToList();


            DoctorsWithBirths.ForEach(tuple =>
            {
                var slots = DateTimeUtils.FindAvailableClinicianTimeSlots(tuple.First, DateTime.Now, DateTime.Now.AddDays(5), TimeSpan.FromHours(12),4, tuple.Second.ToList());
                if (slots.Count > 0)
                {
                    Console.WriteLine("Doctor " + tuple.First.FirstName + " " + tuple.First.LastName + " is available from the following times");
                    Console.WriteLine(StringUtils.DateTimeListToCommaSeparatedString(slots) + "\n");
                }
            });

            AssistantsWithBirths.ForEach(tuple =>
            {
                var slots = DateTimeUtils.FindAvailableClinicianTimeSlots(tuple.First, DateTime.Now, DateTime.Now.AddDays(5), TimeSpan.FromHours(4),2, tuple.Second.ToList());
                if (slots.Count > 0)
                {
                    Console.WriteLine("Assistant " + tuple.First.FirstName + " " + tuple.First.LastName + " is available from the following times");
                    Console.WriteLine(StringUtils.DateTimeListToCommaSeparatedString(slots) + "\n");
                }
            });

            NursesWithBirths.ForEach(tuple =>
            {
                var slots = DateTimeUtils.FindAvailableClinicianTimeSlots(tuple.First, DateTime.Now, DateTime.Now.AddDays(5), TimeSpan.FromHours(136),9, tuple.Second.ToList());
                if (slots.Count > 0)
                {
                    Console.WriteLine("Nurse " + tuple.First.FirstName + " " + tuple.First.LastName + " is available from the following times");
                    Console.WriteLine(StringUtils.DateTimeListToCommaSeparatedString(slots) + "\n");
                }
            });

            MidwivesWithBirths.ForEach(tuple =>
            {
                var slots = DateTimeUtils.FindAvailableClinicianTimeSlots(tuple.First, DateTime.Now, DateTime.Now.AddDays(5), TimeSpan.FromHours(132),8, tuple.Second.ToList());
                if (slots.Count > 0)
                {
                    Console.WriteLine("Midwife " + tuple.First.FirstName + " " + tuple.First.LastName + " is available from the following times");
                    Console.WriteLine(StringUtils.DateTimeListToCommaSeparatedString(slots) + "\n");
                }
            });
        }

        }
}
