using Library.Models.Births;
using Library.Models.FamilyMembers;
using Library.Models.Reservations;
using Library.Models.Rooms;
using Library.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using Library.Models.Clinicians;
using MongoDB.Bson;

namespace Library.Display
{
    public class Display
    {

        private readonly IBirthRepository birthRepo;
        private readonly IClinicianRepository clinicianRepo;
        private readonly IRoomRepository roomRepo;

        public Display(IBirthRepository BirthRepo, IClinicianRepository ClinicianRepo, IRoomRepository RoomRepo)
        {
            birthRepo = BirthRepo;
            clinicianRepo = ClinicianRepo;
            roomRepo = RoomRepo;
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {

                Console.WriteLine("Loading.");
                Console.Beep(660, 100);
                Thread.Sleep(150);
                Console.SetCursorPosition(0, 0);
                ClearCurrentConsoleLine();

                Console.WriteLine("Loading..");
                Console.Beep(660, 100);
                Thread.Sleep(300);
                Console.SetCursorPosition(0, 0);
                ClearCurrentConsoleLine();

                Console.WriteLine("Loading...");
                Console.Beep(660, 100);
                Thread.Sleep(300);
                Console.SetCursorPosition(0, 0);
                ClearCurrentConsoleLine();

                Console.WriteLine("Loading..");
                Console.Beep(510, 100);
                Thread.Sleep(100);
                Console.SetCursorPosition(0, 0);
                ClearCurrentConsoleLine();

                Console.WriteLine("Loading.");
                Console.Beep(660, 100);
                Thread.Sleep(300);
                Console.SetCursorPosition(0, 0);
                ClearCurrentConsoleLine();

                Console.WriteLine("Loading..");
                Console.Beep(770, 100);
                Thread.Sleep(550);
                Console.SetCursorPosition(0, 0);
                ClearCurrentConsoleLine();

                Console.WriteLine("Loading...");
                Console.Beep(380, 100);
                Thread.Sleep(575);
                Console.SetCursorPosition(0, 0);
                ClearCurrentConsoleLine();
            }

            Console.WriteLine("Hello, please type the corresponding letter to choose one of the following options:");
            Console.WriteLine("A: Show planned births for the coming three days");
            Console.WriteLine("B: Show the current ongoing births with information about the birth, parents, clinicians associated and the birth room.");
            Console.WriteLine("C: Specific information about a specific planned birth");
        }

        public static void Reset()
        {
            Console.WriteLine("\nOnce you finish reading, please press the escape key to return to the main menu.");

            ConsoleKey line = Console.ReadKey().Key;
            while (line != ConsoleKey.Escape)
            {
                line = Console.ReadKey().Key;
            }
            Console.Clear();
            Console.Write("A");
            Console.WriteLine("Hello, please type the corresponding letter to choose one of the following options:");
            Console.WriteLine("A: Show planned births for the coming three days");
            Console.WriteLine("B: Show clinicians, birth rooms, maternity rooms and rest rooms available at the clinic for the next five days");
            Console.WriteLine("C: Show the current ongoing births with information about the birth, parents, clinicians associated and the birth room.");
            Console.WriteLine("D: Show the maternity rooms and the rest rooms in use with the parent(s) and child(ren) using the room.");
            Console.WriteLine("E: Specific information about a specific planned birth");
            Console.WriteLine("F: Creation Section");
            Console.WriteLine("H: Removal Section");

        }

        public static void ForceReset(string errorMessage)
        {
            Console.Clear();
            Console.WriteLine(errorMessage);
            Thread.Sleep(1000);
            Console.Clear();
            Console.WriteLine("Hello, please type the corresponding letter to choose one of the following options:");
            Console.WriteLine("A: Show planned births for the coming three days");
            Console.WriteLine("B: Show clinicians, birth rooms, maternity rooms and rest rooms available at the clinic for the next five days");
            Console.WriteLine("C: Show the current ongoing births with information about the birth, parents, clinicians associated and the birth room.");
            Console.WriteLine("D: Show the maternity rooms and the rest rooms in use with the parent(s) and child(ren) using the room.");
            Console.WriteLine("E: Specific information about a specific planned birth");
            Console.WriteLine("F: Creation Section");
            Console.WriteLine("H: Removal Section");

        }
        public static void ClearCurrentConsoleLine()
        {
            int currentLineCursor = Console.CursorTop;
            Console.SetCursorPosition(0, Console.CursorTop);
            Console.Write(new string(' ', Console.WindowWidth));
            Console.SetCursorPosition(0, currentLineCursor);
        }

        public static int ReadAndParseInt32FromDisplay()
        {
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
            return Choice;
        }
        public static char ReadSingleCharFromDisplay()
        {
            char line = ' ';
            while (line == ' ')
            {
                try
                {
                    line = Console.ReadLine().ToString().ToUpper()[0];
                }
                catch (IndexOutOfRangeException)
                {
                    Console.WriteLine("Unacceptable input!\nTry again:");
                    line = ' ';

                }
            }
            return line;
        }
        public static void MarioFunny()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                Console.Beep(660, 100);
                Thread.Sleep(150);
                Console.Beep(660, 100);
                Thread.Sleep(300);
                Console.Beep(660, 100);
                Thread.Sleep(300);
                Console.Beep(510, 100);
                Thread.Sleep(100);
                Console.Beep(660, 100);
                Thread.Sleep(300);
                Console.Beep(770, 100);
                Thread.Sleep(550);
                Console.Beep(380, 100);
                Thread.Sleep(575);
            }
            else
            {
                Console.WriteLine("Lol, not on windows cringe!");
                ForceReset("");
            }
        }

        public void Case1()
        {
            Console.Clear();

            //get all births 
            var BirthList = birthRepo.GetAllWithinTimespan(DateTime.Now, DateTime.Now.AddDays(3)).Result;
            
            
            //select a birth to get further data on
            Console.WriteLine("Please enter a number between 1 and " + BirthList.Count() + ", to view the specific birth's details.");
            var Choice = ReadAndParseInt32FromDisplay();
            Console.Clear();

            //get the relevant birth
            var B = BirthList.ElementAt(Choice - 1);

            //get the associated clinicians from the db
            var clinicians = clinicianRepo.GetAllMatching(B.AssociatedClinicians).Result;
            
            //Get the Rooms
            var roomIds = B.Reservations.Select(r => r.ReservedRoomId).ToList();
            var rooms = roomRepo.GetDictionaryOfAllMatching(roomIds).Result;


            //presentation logic 


            foreach (var c in B.ChildrenToBeBorn)
            {
                Console.WriteLine("Name: " + c.FirstName + " " + c.LastName);
            }

            Console.WriteLine("Date of birth: " + B.BirthDate.ToLongDateString());
            Console.WriteLine("Mother: " + B.Mother.FirstName + " " + B.Mother.LastName);

            if (B.Father != null)
            {
                Console.WriteLine("Father: " + B.Father.FirstName + " " + B.Father.LastName);
            }

            if (B.Relatives.Any())
            {
                Console.WriteLine("Relatives:");
                foreach (var r in B.Relatives)
                {
                    Console.WriteLine("Name: " + r.FirstName + " " + r.LastName);
                }
            }


            Console.WriteLine("Clinicians:");

            foreach (var c in clinicians)
            {
                Console.WriteLine(c.Role + ": " + c.FirstName + " " + c.LastName);
            }

            
            Console.WriteLine("This birth has the following reservations:");

            foreach (var r in B.Reservations)
            {
                var room = rooms[r.ReservedRoomId];
                var roomTypeCaps = room.RoomType.ToString();
                var roomType = roomTypeCaps.Substring(0, 1).ToUpper() + roomTypeCaps[1..].ToLower();
                Console.WriteLine("Room #" + room.Id + " - " + roomType + " room.");
                Console.WriteLine("Between: " + r.StartTime.ToLongDateString() + " " + r.StartTime.ToShortTimeString() + " and " + r.EndTime.ToLongDateString() + " " + r.EndTime.ToShortTimeString());
            }
        }

        public void Case3()
        {
            Console.Clear();
            DateTime FilterDate = DateTime.Now;

            /*List<Reservation> ReservationList = context.Reservations.Where(r =>
            r.ReservedRoom.RoomType == RoomType.BIRTH
            &&
            r.StartTime <= FilterDate
            &&
            r.EndTime > FilterDate).ToList();*/

            var BirthsWithRoomNumber = birthRepo.GetAllBirthsWithCliniciansUsingBirthRoomAtTime(FilterDate).Result;


            if (!BirthsWithRoomNumber.Any())
            {
                Console.WriteLine("There are currently no ongoing births.");
                return;
            }
            foreach (var set in BirthsWithRoomNumber)
            {
                var B = set.Item1;
                Console.WriteLine("In Birthroom " + set.Item2 + ".");
                foreach (var c in B.ChildrenToBeBorn)
                {
                    Console.WriteLine("Name: " + c.FirstName + " " + c.LastName);
                }
                Console.WriteLine("Date of birth: " + B.BirthDate.ToLongDateString());
                Console.WriteLine("Mother: " + B.Mother.FirstName + " " + B.Mother.LastName);
                if (B.Father != null)
                {
                    Console.WriteLine("Father: " + B.Father.FirstName + " " + B.Father.LastName);
                }
                if (B.Relatives.Any())
                {
                    Console.WriteLine("Relatives:");
                    foreach (var rel in B.Relatives)
                    {
                        Console.WriteLine("Name: " + rel.FirstName + " " + rel.LastName);
                    }
                    Console.WriteLine("Clinicians:");
                    foreach (var c in set.Item3)
                    {
                        Console.WriteLine(c.Role + ": " + c.FirstName + " " + c.LastName);
                    }
                }
                Console.WriteLine();

            }
        }
        public void Case5()
        {
            Console.Clear();
            var births = birthRepo.GetAll().Result;

            foreach (var b in births)
            {
                Console.WriteLine("Journal for " + b.Mother.FirstName + "'s Planned birth - " + b.Id);
            }

            Console.WriteLine("Please enter a number according to the Journal you wish to read.");
            var Choice = ReadAndParseInt32FromDisplay();
            
            //Select particular birth
            var B = births.ElementAt(Choice - 1);
           
            //get clinicians
            var Clinicians = clinicianRepo.GetAllMatching(B.AssociatedClinicians).Result;

            //Get the Rooms
            var roomIds = B.Reservations.Select(r => r.ReservedRoomId).ToList();
            var rooms = roomRepo.GetDictionaryOfAllMatching(roomIds).Result;

            Console.Clear();

            //presentation logic 

            foreach (var c in B.ChildrenToBeBorn)
            {
                Console.WriteLine("Name: " + c.FirstName + " " + c.LastName);
            }
            Console.WriteLine("Date of birth: " + B.BirthDate.ToLongDateString());
            Console.WriteLine("Mother: " + B.Mother.FirstName + " " + B.Mother.LastName);
            if (B.Father != null)
            {
                Console.WriteLine("Father: " + B.Father.FirstName + " " + B.Father.LastName);
            }
            if (B.Relatives.Any())
            {
                Console.WriteLine("Relatives:");
                foreach (var r in B.Relatives)
                {
                    Console.WriteLine("Name: " + r.FirstName + " " + r.LastName);
                }
            }
            Console.WriteLine("Clinicians:");
            foreach (var c in Clinicians)
            {
                Console.WriteLine(c.Role + ": " + c.FirstName + " " + c.LastName);
            }

            Console.WriteLine("This birth has the following reservations:");
            foreach (var r in B.Reservations)
            {
                var room = rooms[r.ReservedRoomId];
                var roomTypeCaps = room.RoomType.ToString();
                var roomType = roomTypeCaps.Substring(0, 1).ToUpper() + roomTypeCaps[1..].ToLower();
                Console.WriteLine("Room #" + room.Id + " - " + roomType + " room.");
                Console.WriteLine("Between: " + r.StartTime.ToLongDateString() + " " + r.StartTime.ToShortTimeString() + " and " + r.EndTime.ToLongDateString() + " " + r.EndTime.ToShortTimeString());
            }
        }
    }
}