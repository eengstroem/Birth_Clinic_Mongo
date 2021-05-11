using Library.Context;
using Library.Factory.Births;
using Library.Factory.Clinicians;
using Library.Factory.FamilyMembers;
using Library.Factory.Rooms;
using Library.Models.Births;
using Library.Models.Clinicians;
using Library.Models.FamilyMembers;
using Library.Models.Reservations;
using Library.Models.Rooms;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using static Library.Factory.FamilyMembers.FamilyMemberFactory;

namespace Library.DataGenerator
{
    public class DataGenerator
    {

        private static readonly int HowManyBirthsToGenerate = 30;
        public static void GenerateStaticData(BirthClinicDbContext Context)
        {
            //create midwives
            for (int i = 0; i < 10; i++)
            {
                Context.Clinicians.Add(ClinicianFactory.CreateFakeClinician(ClinicianType.MIDWIFE));
            }

            //create nurses
            for (int i = 0; i < 20; i++)
            {
                Context.Clinicians.Add(ClinicianFactory.CreateFakeClinician(ClinicianType.NURSE));
            }

            //create assistants
            for (int i = 0; i < 20; i++)
            {
                Context.Clinicians.Add(ClinicianFactory.CreateFakeClinician(ClinicianType.HEALTH_ASSISTANT));
            }

            //create Doctors
            for (int i = 0; i < 5; i++)
            {
                Context.Clinicians.Add(ClinicianFactory.CreateFakeClinician(ClinicianType.DOCTOR));
            }

            //create Secretaries
            for (int i = 0; i < 4; i++)
            {
                Context.Clinicians.Add(ClinicianFactory.CreateFakeClinician(ClinicianType.SECRETARY));
            }

            //create Maternity Rooms
            for (int i = 0; i < 22; i++)
            {
                Context.Rooms.Add(RoomFactory.CreateRoom(RoomType.MATERNITY));
            }

            //create Rest Rooms
            for (int i = 0; i < 5; i++)
            {
                Context.Rooms.Add(RoomFactory.CreateRoom(RoomType.REST));
            }

            //create Birth Rooms
            for (int i = 0; i < 15; i++)
            {
                Context.Rooms.Add(RoomFactory.CreateRoom(RoomType.BIRTH));
            }

            //Save before adding births. 
            Context.SaveChanges();
        }

        public static void GenerateData(BirthClinicDbContext Context)
        {
            //Adding 136 Births since there are 5000 births per year (13.6 per day), and we want to simulate 10 days of fake data.
            for (int i = 0; i < HowManyBirthsToGenerate; i++)
            {
                var B = BirthFactory.CreateFakeBirth();
                if (!CreateReservations(Context, B, out List<Reservation> reservations))
                {
                    Console.WriteLine("We are out of rooms");
                    continue;
                }
                if (!AddClinicians(Context, B, out List<Clinician> Clinicians))
                {

                    continue;
                }

                Context.AddRange(reservations);

                B.AssociatedClinicians = Clinicians;
                B.Mother = AddMother();
                Random rand = new();
                if (rand.Next(1, 10) > 1)
                {
                    B.Father = AddFather();
                }
                B.Relatives = AddRelatives();

                B.ChildrenToBeBorn = AddChildrenToBorn();

                B.IsEnded = false;

                Context.Births.Add(B);
                Context.SaveChanges();
            }
        }

        //TODO switch to single instead of where
        public static Room FindAvailableRooms(DbSet<Room> Rooms, DateTime StartTime, DateTime EndTime, RoomType Type)
        {
            try
            {
                return Rooms.First(room =>

                        //search for conflicts
                        room.RoomType == Type && !room.CurrentReservations.Any(res =>
                        (StartTime >= res.StartTime && StartTime <= res.EndTime)
                        ||
                        (EndTime >= res.StartTime && EndTime <= res.EndTime)
                        )//Only returns true if there are no conflicts
                     );
            }
            catch
            {
                return null;
            }
        }

        public static IEnumerable<Clinician> FindAvailableClinicians(DbSet<Clinician> clinicians, Birth Birth, ClinicianType Role)
        {
            int RequiredDelta = 0;
            int AllowedOccurences = 0;

            switch (Role)
            {
                case ClinicianType.DOCTOR:
                    RequiredDelta = 12;
                    AllowedOccurences = 4;
                    break;
                case ClinicianType.HEALTH_ASSISTANT:
                    RequiredDelta = 4;
                    AllowedOccurences = 2;

                    break;
                case ClinicianType.MIDWIFE:
                    RequiredDelta = 120;
                    AllowedOccurences = 8;

                    break;
                case ClinicianType.NURSE:
                    RequiredDelta = 136;
                    AllowedOccurences = 9;

                    break;
                case ClinicianType.SECRETARY:
                    // Secretary only has to check in the birth, so she is freed up immediately, but still associated.
                    AllowedOccurences = 50000;

                    break;
            }

            return clinicians.Where(clinician =>
                    //search for conflicts
                    clinician.Role == Role &&
                    clinician.AssignedBirths.Where(b =>
                     EF.Functions.DateDiffMinute(b.BirthDate, Birth.BirthDate) >= RequiredDelta * 60).Count() <= AllowedOccurences
                 );


        }

        public static bool CreateReservations(BirthClinicDbContext Context, Birth Birth, out List<Reservation> reservations)
        {
            var MaternityStartTime = Birth.BirthDate.AddHours(-132);
            var MaternityEndTime = Birth.BirthDate.AddHours(-12);

            var RestStartTime = Birth.BirthDate;
            var RestEndTime = Birth.BirthDate.AddHours(4);

            var BirthStartTime = Birth.BirthDate.AddHours(-12);
            var BirthEndTime = Birth.BirthDate;

            var AvailableMaternityRoom = FindAvailableRooms(Context.Rooms, MaternityStartTime, MaternityEndTime, RoomType.MATERNITY);
            var AvailableBirthRoom = FindAvailableRooms(Context.Rooms, MaternityStartTime, MaternityEndTime, RoomType.BIRTH);
            var AvailableRestRoom = FindAvailableRooms(Context.Rooms, MaternityStartTime, MaternityEndTime, RoomType.REST);

            //Not possible to create a birth at the given time. Find another  hospital.
            if (AvailableBirthRoom == null || AvailableMaternityRoom == null || AvailableRestRoom == null)
            {
                reservations = null;
                return false;
            }
            else //There are available rooms of all 3 categories! Nice!
            {
                //create reservations
                var MaternityRes = new Reservation
                {
                    StartTime = MaternityStartTime,
                    EndTime = MaternityEndTime,
                    ReservedRoomId = AvailableMaternityRoom.Id
                };

                var BirthRes = new Reservation
                {
                    StartTime = BirthStartTime,
                    EndTime = BirthEndTime,
                    ReservedRoomId = AvailableBirthRoom.Id
                };

                var RestRes = new Reservation
                {
                    StartTime = RestStartTime,
                    EndTime = RestEndTime,
                    ReservedRoomId = AvailableRestRoom.Id
                };

                reservations = new List<Reservation> { MaternityRes, BirthRes, RestRes };
                return true;
            }
        }

        public static bool AddClinicians(BirthClinicDbContext Context, Birth Birth, out List<Clinician> Clinicians)
        {

            Random Rand = new();
            IEnumerable<Clinician> FoundClinicians;
            Clinicians = new();

            // Finds available Doctor and inserts one random available Doctor into output List.
            FoundClinicians = FindAvailableClinicians(Context.Clinicians, Birth, ClinicianType.DOCTOR);
            if (!FoundClinicians.Any())
            {
                Console.WriteLine("We are out of Doctors");
                return false;
            }
            Clinicians.Add(FoundClinicians.ElementAt(Rand.Next(0, FoundClinicians.Count())));


            // Finds available Midwife and inserts one random available Midwife into output List.
            FoundClinicians = FindAvailableClinicians(Context.Clinicians, Birth, ClinicianType.MIDWIFE);
            if (!FoundClinicians.Any())
            {
                Console.WriteLine("We are out of Midwives");
                return false;
            }
            Clinicians.Add(FoundClinicians.ElementAt(Rand.Next(0, FoundClinicians.Count())));


            // Finds available Nurse and inserts two random available Nurse into output List.
            FoundClinicians = FindAvailableClinicians(Context.Clinicians, Birth, ClinicianType.NURSE);

            if (FoundClinicians.Count() < 2)
            {
                Console.WriteLine("We are out of Nurses");
                return false;
            }

            Clinicians.Add(FoundClinicians.ElementAt(0));
            Clinicians.Add(FoundClinicians.ElementAt(1));


            // Finds available Assistant and inserts two random available Assistant into output List.
            FoundClinicians = FindAvailableClinicians(Context.Clinicians, Birth, ClinicianType.HEALTH_ASSISTANT);
            if (!FoundClinicians.Any())
            {
                Console.WriteLine("We are out of Health Assistants");
                return false;
            }
            Clinicians.Add(FoundClinicians.ElementAt(Rand.Next(0, FoundClinicians.Count())));


            // Finds available Secretary and inserts two random available Secretary into output List.
            FoundClinicians = FindAvailableClinicians(Context.Clinicians, Birth, ClinicianType.SECRETARY);
            if (!FoundClinicians.Any())
            {
                Console.WriteLine("We are out of Secretary");
                return false;
            }
            Clinicians.Add(FoundClinicians.ElementAt(Rand.Next(0, FoundClinicians.Count())));

            return true;
        }

        public static Mother AddMother()
        {
            return (Mother)FamilyMemberFactory.CreateFakeFamilyMember(FamilyMemberType.MOTHER);
        }

        public static Father AddFather()
        {
            return (Father)CreateFakeFamilyMember(FamilyMemberType.FATHER);
        }

        public static List<Relative> AddRelatives()
        {
            Random rand = new();
            List<Relative> relatives = new();
            for (int i = rand.Next(1, 5); i > 0; i--)
            {
                relatives.Add((Relative)CreateFakeFamilyMember(FamilyMemberType.RELATIVE));
            }
            return relatives;
        }

        public static List<Child> AddChildrenToBorn()
        {
            Random rand = new();
            double weight = rand.NextDouble();

            List<Child> Children = new();

            Children.Add((Child)CreateFakeFamilyMember(FamilyMemberType.CHILD));
            if (weight > 0.75)
            {
                Children.Add((Child)CreateFakeFamilyMember(FamilyMemberType.CHILD));
                if (weight > 0.85)
                {
                    Children.Add((Child)CreateFakeFamilyMember(FamilyMemberType.CHILD));
                    if (weight > 0.95)
                    {
                        Children.Add((Child)CreateFakeFamilyMember(FamilyMemberType.CHILD));
                    }
                }
            }
            return Children;
        }
        public static bool CreateBirth(BirthClinicDbContext Context)
        {
            var B = BirthFactory.CreateFakeBirth();
            if (!CreateReservations(Context, B, out List<Reservation> reservations))
            {
                Console.WriteLine("We are out of rooms");
                return false;
            }
            if (!AddClinicians(Context, B, out List<Clinician> Clinicians))
            {
                return false;
            }

            Context.AddRange(reservations);

            B.AssociatedClinicians = Clinicians;
            B.Mother = AddMother();
            Random rand = new();
            if (rand.Next(1, 10) > 1)
            {
                B.Father = AddFather();
            }
            B.Relatives = AddRelatives();

            B.ChildrenToBeBorn = AddChildrenToBorn();
            B.IsEnded = false;
            Context.Births.Add(B);
            Context.SaveChanges();
            return true;
        }
        public static bool CreateReservation(BirthClinicDbContext Context, RoomType Type)
        {

            List<Birth> BirthList = Context.Births.ToList();

            Console.WriteLine("Please choose a Birth to add this reservation to.");
            Console.WriteLine("Pick between " + 1 + " and " + BirthList.Count + ".");

            string line = "";
            int Choice = -1;
            while (Choice == -1)
            {
                try
                {
                    line = Console.ReadLine();
                    Choice = Int32.Parse(line);
                }
                catch (FormatException)
                {
                    Console.WriteLine("{0} is not a valid integer!\nTry again:", line);
                    Choice = -1;

                }
            }
            var B = BirthList.ElementAt(Choice - 1);
            var StartTime = B.BirthDate;
            var EndTime = B.BirthDate;
            switch (Type)
            {
                case RoomType.BIRTH:
                    StartTime = B.BirthDate.AddHours(-12);
                    EndTime = B.BirthDate;
                    break;
                case RoomType.MATERNITY:
                    StartTime = B.BirthDate.AddHours(-132);
                    EndTime = B.BirthDate.AddHours(-12);
                    break;
                case RoomType.REST:
                    StartTime = B.BirthDate;
                    EndTime = B.BirthDate.AddHours(4);
                    break;
            }

            var AvailableRoom = FindAvailableRooms(Context.Rooms, StartTime, EndTime, Type);

            //Not possible to create a birth at the given time. Find another  hospital.
            if (AvailableRoom == null)
            {
                return false;
            }
            else //There are available rooms of all 3 categories! Nice!
            {
                //create reservations
                var reservation = new Reservation
                {
                    StartTime = StartTime,
                    EndTime = EndTime,
                    ReservedRoom = AvailableRoom,
                    AssociatedBirth = B
                };

                Context.Reservations.Add(reservation);
                Context.SaveChanges();
                return true;
            }

        }
    }
}
