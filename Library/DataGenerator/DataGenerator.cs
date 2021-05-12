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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Library.Factory.FamilyMembers.FamilyMemberFactory;

namespace Library.DataGenerator
{
    public class DataGenerator
    {
        private readonly IBirthRepository _birthRepo;
        private readonly IClinicianRepository _clinicianRepo;
        private readonly IRoomRepository _roomRepo;

        private static readonly int HowManyBirthsToGenerate = 30;

        public DataGenerator(IBirthRepository birthRepo, IClinicianRepository clinicianRepo, IRoomRepository roomRepo)
        {
            _birthRepo = birthRepo;
            _clinicianRepo = clinicianRepo;
            _roomRepo = roomRepo;
        }
        public async Task
GenerateStaticData()
        {
            //create midwives
            for (int i = 0; i < 10; i++)
            {
                await _clinicianRepo.Create(ClinicianFactory.CreateFakeClinician(ClinicianType.MIDWIFE));
            }

            //create nurses
            for (int i = 0; i < 20; i++)
            {
                await _clinicianRepo.Create(ClinicianFactory.CreateFakeClinician(ClinicianType.NURSE));
            }

            //create assistants
            for (int i = 0; i < 20; i++)
            {
                await _clinicianRepo.Create(ClinicianFactory.CreateFakeClinician(ClinicianType.HEALTH_ASSISTANT));
            }

            //create Doctors
            for (int i = 0; i < 5; i++)
            {
                await _clinicianRepo.Create(ClinicianFactory.CreateFakeClinician(ClinicianType.DOCTOR));
            }

            //create Secretaries
            for (int i = 0; i < 4; i++)
            {
                await _clinicianRepo.Create(ClinicianFactory.CreateFakeClinician(ClinicianType.SECRETARY));
            }

            //create Maternity Rooms
            for (int i = 0; i < 22; i++)
            {
                await _roomRepo.Create(RoomFactory.CreateRoom(RoomType.MATERNITY));
            }

            //create Rest Rooms
            for (int i = 0; i < 5; i++)
            {
                await _roomRepo.Create(RoomFactory.CreateRoom(RoomType.REST));
            }

            //create Birth Rooms
            for (int i = 0; i < 15; i++)
            {
                await _roomRepo.Create(RoomFactory.CreateRoom(RoomType.BIRTH));
            }

        }

        public async Task GenerateData()
        {

            for (var i = 0; i < HowManyBirthsToGenerate; i++)
            {
                var B = BirthFactory.CreateFakeBirth();
                if (!CreateReservations(_roomRepo, B, out var reservations))
                {
                    Console.WriteLine("We are out of rooms");
                    continue;
                }
                var Clinicians = AddClinicians(_clinicianRepo);
                if (Clinicians == null)
                {

                    continue;
                }

                B.Reservations = reservations;
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

                await _birthRepo.Create(B);
            }
        }

        //TODO switch to single instead of where
        public static Room FindAvailableRooms(IRoomRepository roomRepo, RoomType type)
        {
            var rooms = roomRepo.GetAll().Where(r => r.RoomType == type).ToList();
            var random = new Random();
            int index = random.Next(rooms.Count);
            return rooms[index];
        }

        public static List<Clinician> FindAvailableClinicians(IClinicianRepository clinicianRepo, ClinicianType role)
        {
            return clinicianRepo.GetAll().Where(c => c.Role == role).ToList();

        }

        public static bool CreateReservations(IRoomRepository RoomRepo, Birth Birth, out List<Reservation> reservations)
        {
            var MaternityStartTime = Birth.BirthDate.AddHours(-132);
            var MaternityEndTime = Birth.BirthDate.AddHours(-12);

            var RestStartTime = Birth.BirthDate;
            var RestEndTime = Birth.BirthDate.AddHours(4);

            var BirthStartTime = Birth.BirthDate.AddHours(-12);
            var BirthEndTime = Birth.BirthDate;

            var AvailableMaternityRoom = FindAvailableRooms(RoomRepo, RoomType.MATERNITY);
            var AvailableBirthRoom = FindAvailableRooms(RoomRepo, RoomType.BIRTH);
            var AvailableRestRoom = FindAvailableRooms(RoomRepo, RoomType.REST);

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
                    Room = AvailableMaternityRoom
                };

                var BirthRes = new Reservation
                {
                    StartTime = BirthStartTime,
                    EndTime = BirthEndTime,
                    Room = AvailableBirthRoom
                };

                var RestRes = new Reservation
                {
                    StartTime = RestStartTime,
                    EndTime = RestEndTime,
                    Room = AvailableRestRoom
                };

                reservations = new List<Reservation> { MaternityRes, BirthRes, RestRes };
                return true;
            }
        }

        public static List<Clinician> AddClinicians(IClinicianRepository clinicianRepo)
        {
            List<Clinician> Clinicians = new();
            Random Rand = new();

            // Finds available Doctor and inserts one random available Doctor into output List.
            var Doctors = FindAvailableClinicians(clinicianRepo, ClinicianType.DOCTOR);
            if (!Doctors.Any())
            {

                Console.WriteLine("We are out of Doctors");
                return null;
            }
            Clinicians.Add(Doctors.ElementAt(Rand.Next(0, Doctors.Count)));


            // Finds available Midwife and inserts one random available Midwife into output List.
            var Midwives = FindAvailableClinicians(clinicianRepo, ClinicianType.MIDWIFE);
            if (!Midwives.Any())
            {
                Console.WriteLine("We are out of Midwives");
                return null;
            }
            Clinicians.Add(Midwives.ElementAt(Rand.Next(0, Midwives.Count)));


            // Finds available Nurse and inserts two random available Nurse into output List.
            var Nurses = FindAvailableClinicians(clinicianRepo, ClinicianType.NURSE);

            if (Nurses.Count < 2)
            {
                Console.WriteLine("We are out of Nurses");
                return null;
            }

            //TODO måske find noget der gør så det ikke kun er de første 2 nurses
            Clinicians.Add(Nurses.ElementAt(0));
            Clinicians.Add(Nurses.ElementAt(1));


            // Finds available Assistant and inserts two random available Assistant into output List.
            var Assistants = FindAvailableClinicians(clinicianRepo, ClinicianType.HEALTH_ASSISTANT);
            if (!Assistants.Any())
            {
                Console.WriteLine("We are out of Health Assistants");
                return null;
            }
            Clinicians.Add((Assistants.ElementAt(Rand.Next(0, Assistants.Count))));


            // Finds available Secretary and inserts two random available Secretary into output List.
            var Secretaries = FindAvailableClinicians(clinicianRepo, ClinicianType.SECRETARY);
            if (!Secretaries.Any())
            {
                Console.WriteLine("We are out of Secretaries");
                return null;
            }
            Clinicians.Add(Secretaries.ElementAt(Rand.Next(0, Secretaries.Count)));

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
