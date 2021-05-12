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
using Library.Services;
using MongoDB.Bson;

namespace Library.Display
{
    public class Display
    {

        private readonly IBirthService BirthService;

        public Display(IBirthService birthService)
        {
            BirthService = birthService;
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
            Console.WriteLine("B: Show the current ongoing births with information about the birth, parents, clinicians associated and the birth room.");
            Console.WriteLine("C: Specific information about a specific planned birth");

        }

        public static void ForceReset(string errorMessage)
        {
            Console.Clear();
            Console.WriteLine(errorMessage);
            Thread.Sleep(1000);
            Console.Clear();
            Console.WriteLine("Hello, please type the corresponding letter to choose one of the following options:");
            Console.WriteLine("A: Show planned births for the coming three days");
            Console.WriteLine("B: Show the current ongoing births with information about the birth, parents, clinicians associated and the birth room.");
            Console.WriteLine("C: Specific information about a specific planned birth");

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
            var line = "";
            var Choice = -1;
            while (Choice == -1)
            {
                try
                {
                    line = Console.ReadLine();
                    Choice = int.Parse(line);
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

            //get all births in the next 3 days
            var BirthList = BirthService.GetAllWithinTimespan(DateTime.Now, DateTime.Now.AddDays(3));


            //select a birth to display further data of
            Console.WriteLine("Please enter a number between 1 and " + BirthList.Count() + ", to view the specific birth's details.");
            var Choice = ReadAndParseInt32FromDisplay();
            Console.Clear();

            //get the relevant birth
            var B = BirthList.ElementAt(Choice - 1);

            DisplayBirthInfo(B);
        }

        private static void DisplayBirthInfo(Birth birth)
        {
            //presentation logic 
            foreach (var c in birth.ChildrenToBeBorn)
            {
                Console.WriteLine("Name: " + c.FirstName + " " + c.LastName);
            }

            Console.WriteLine("Date of birth: " + birth.BirthDate.ToLongDateString());
            Console.WriteLine("Mother: " + birth.Mother.FirstName + " " + birth.Mother.LastName);

            if (birth.Father != null)
            {
                Console.WriteLine("Father: " + birth.Father.FirstName + " " + birth.Father.LastName);
            }

            if (birth.Relatives.Any())
            {
                Console.WriteLine("Relatives:");
                foreach (var r in birth.Relatives)
                {
                    Console.WriteLine("Name: " + r.FirstName + " " + r.LastName);
                }
            }


            Console.WriteLine("Clinicians:");

            foreach (var c in birth.AssociatedClinicians)
            {
                Console.WriteLine(c.Role + ": " + c.FirstName + " " + c.LastName);
            }


            Console.WriteLine("This birth has the following reservations:");

            foreach (var r in birth.Reservations)
            {
                var room = r.Room;
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

            var births = BirthService.GetAllBirthsUsingABirthRoomAtTime(FilterDate);

            if (!births.Any())
            {
                Console.WriteLine("There are currently no ongoing births.");
                return;
            }


            foreach (var B in births)
            {
                //Find the birthroom that is in use
                var birthroom = B.Reservations.Select(r => r.Room)
                    .First(room => room.RoomType == RoomType.BIRTH);

                Console.WriteLine("In Birthroom " + birthroom.Id + ".");
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
                    foreach (var c in B.AssociatedClinicians)
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
            var births = BirthService.GetAll();

            foreach (var b in births)
            {
                Console.WriteLine("Journal for " + b.Mother.FirstName + "'s Planned birth - " + b.Id);
            }

            Console.WriteLine("Please enter a number according to the Journal you wish to read.");
            var Choice = ReadAndParseInt32FromDisplay();

            //Select particular birth
            var B = births.ElementAt(Choice - 1);

            Console.Clear();

            //presentation logic 

            DisplayBirthInfo(B);
        }
    }
}