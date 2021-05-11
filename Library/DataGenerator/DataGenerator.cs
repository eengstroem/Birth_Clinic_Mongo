using Library.Factory.Births;
using Library.Factory.Clinicians;
using Library.Factory.FamilyMembers;
using Library.Factory.Rooms;
using Library.Models.Births;
using Library.Models.Clinicians;
using Library.Models.FamilyMembers;
using Library.Models.Reservations;
using Library.Models.Rooms;
using Library.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Library.Factory.FamilyMembers.FamilyMemberFactory;

namespace Library.DataGenerator
{
    public class DataGenerator
    {
        private readonly IBirthRepository birthRepo;
        private readonly IClinicianRepository clinicianRepo;
        private readonly IRoomRepository roomRepo;

        private static readonly int HowManyBirthsToGenerate = 30;

        public DataGenerator(IBirthRepository BirthRepo, IClinicianRepository ClinicianRepo, IRoomRepository RoomRepo)
        {
            birthRepo = BirthRepo;
            clinicianRepo = ClinicianRepo;
            roomRepo = RoomRepo;
        }
            public async void GenerateStaticData()
        {
            //create midwives
            for (int i = 0; i < 10; i++)
            {
                await clinicianRepo.Create(ClinicianFactory.CreateFakeClinician(ClinicianType.MIDWIFE));
            }

            //create nurses
            for (int i = 0; i < 20; i++)
            {
                await clinicianRepo.Create(ClinicianFactory.CreateFakeClinician(ClinicianType.NURSE));
            }

            //create assistants
            for (int i = 0; i < 20; i++)
            {
                await clinicianRepo.Create(ClinicianFactory.CreateFakeClinician(ClinicianType.HEALTH_ASSISTANT));
            }

            //create Doctors
            for (int i = 0; i < 5; i++)
            {
                await clinicianRepo.Create(ClinicianFactory.CreateFakeClinician(ClinicianType.DOCTOR));
            }

            //create Secretaries
            for (int i = 0; i < 4; i++)
            {
                await clinicianRepo.Create(ClinicianFactory.CreateFakeClinician(ClinicianType.SECRETARY));
            }

            //create Maternity Rooms
            for (int i = 0; i < 22; i++)
            {
                await roomRepo.Create(RoomFactory.CreateRoom(RoomType.MATERNITY));
            }

            //create Rest Rooms
            for (int i = 0; i < 5; i++)
            {
                await roomRepo.Create(RoomFactory.CreateRoom(RoomType.REST));
            }

            //create Birth Rooms
            for (int i = 0; i < 15; i++)
            {
                await roomRepo.Create(RoomFactory.CreateRoom(RoomType.BIRTH));
            }

        }

        public async void GenerateData()
        {
            //Adding 136 Births since there are 5000 births per year (13.6 per day), and we want to simulate 10 days of fake data.
            for (int i = 0; i < HowManyBirthsToGenerate; i++)
            {
                List<Clinician> Clinicians;
                var B = BirthFactory.CreateFakeBirth();
                if (!CreateReservations(roomRepo, B, out List<Reservation> reservations))
                {
                    Console.WriteLine("We are out of rooms");
                    continue;
                }
                Clinicians = await AddClinicians(clinicianRepo, B);
                if (Clinicians == null)
                {

                    continue;
                }

                B.Reservations.AddRange(reservations);

                foreach(Clinician c in Clinicians)
                {
                    B.AssociatedClinicians.Add(c._id);
                }
                B.Mother = AddMother();
                Random rand = new();
                if (rand.Next(1, 10) > 1)
                {
                    B.Father = AddFather();
                }
                B.Relatives = AddRelatives();

                B.ChildrenToBeBorn = AddChildrenToBorn();

                B.IsEnded = false;

                await birthRepo.Create(B);
            }
        }

        //TODO switch to single instead of where
        public static async Task<Room> FindAvailableRooms(IRoomRepository RoomRepo, DateTime StartTime, DateTime EndTime, RoomType Type)
        {
            try
            {
                return await RoomRepo.GetUnreservedRoomOfType(StartTime, EndTime, Type);
            }
            catch
            {
                return null;
            }
        }

        public async static Task<IEnumerable<Clinician>> FindAvailableClinicians(IClinicianRepository ClinicianRepo, Birth Birth, ClinicianType Role)
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

            return await ClinicianRepo.FindAvailableClinicians(Role, Birth, RequiredDelta, AllowedOccurences);


        }

        public static bool CreateReservations(IRoomRepository RoomRepo, Birth Birth, out List<Reservation> reservations)
        {
            var MaternityStartTime = Birth.BirthDate.AddHours(-132);
            var MaternityEndTime = Birth.BirthDate.AddHours(-12);

            var RestStartTime = Birth.BirthDate;
            var RestEndTime = Birth.BirthDate.AddHours(4);

            var BirthStartTime = Birth.BirthDate.AddHours(-12);
            var BirthEndTime = Birth.BirthDate;

            var AvailableMaternityRoom = FindAvailableRooms(RoomRepo, MaternityStartTime, MaternityEndTime, RoomType.MATERNITY);
            var AvailableBirthRoom = FindAvailableRooms(RoomRepo, MaternityStartTime, MaternityEndTime, RoomType.BIRTH);
            var AvailableRestRoom = FindAvailableRooms(RoomRepo, MaternityStartTime, MaternityEndTime, RoomType.REST);

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

        public async static Task<List<Clinician>> AddClinicians(IClinicianRepository ClinicianRepo, Birth Birth)
        {
            List<Clinician> Clinicians;
            Random Rand = new();
            IEnumerable<Clinician> FoundClinicians;
            Clinicians = new();

            // Finds available Doctor and inserts one random available Doctor into output List.
            FoundClinicians = await FindAvailableClinicians(ClinicianRepo, Birth, ClinicianType.DOCTOR);
            if (!FoundClinicians.Any())
            {
                Console.WriteLine("We are out of Doctors");
                return null;
            }
            Clinicians.Add(FoundClinicians.ElementAt(Rand.Next(0, FoundClinicians.Count())));


            // Finds available Midwife and inserts one random available Midwife into output List.
            FoundClinicians = await FindAvailableClinicians(ClinicianRepo, Birth, ClinicianType.MIDWIFE);
            if (!FoundClinicians.Any())
            {
                Console.WriteLine("We are out of Midwives");
                return null;
            }
            Clinicians.Add(FoundClinicians.ElementAt(Rand.Next(0, FoundClinicians.Count())));


            // Finds available Nurse and inserts two random available Nurse into output List.
            FoundClinicians = await FindAvailableClinicians(ClinicianRepo, Birth, ClinicianType.NURSE);

            if (FoundClinicians.Count() < 2)
            {
                Console.WriteLine("We are out of Nurses");
                return null;
            }

            Clinicians.Add(FoundClinicians.ElementAt(0));
            Clinicians.Add(FoundClinicians.ElementAt(1));


            // Finds available Assistant and inserts two random available Assistant into output List.
            FoundClinicians = await FindAvailableClinicians(ClinicianRepo, Birth, ClinicianType.HEALTH_ASSISTANT);
            if (!FoundClinicians.Any())
            {
                Console.WriteLine("We are out of Health Assistants");
                return null;
            }
            Clinicians.Add(FoundClinicians.ElementAt(Rand.Next(0, FoundClinicians.Count())));


            // Finds available Secretary and inserts two random available Secretary into output List.
            FoundClinicians = await FindAvailableClinicians(ClinicianRepo, Birth, ClinicianType.SECRETARY);
            if (!FoundClinicians.Any())
            {
                Console.WriteLine("We are out of Secretaries");
                return null;
            }
            Clinicians.Add(FoundClinicians.ElementAt(Rand.Next(0, FoundClinicians.Count())));

            return Clinicians;
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
        
    }
}
