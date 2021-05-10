using Library.Context;
using Library.Models.Births;
using Library.Models.FamilyMembers;
using Library.Models.Reservations;
using Library.Models.Rooms;
using Library.Models.FamilyMembers;
using Library.Utils;
using Library.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;

namespace Library.Display
{
    public class Display
    {

        public Display()
        {
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
            Console.WriteLine("B: Show clinicians, birth rooms, maternity rooms and rest rooms available at the clinic for the next five days");
            Console.WriteLine("C: Show the current ongoing births with information about the birth, parents, clinicians associated and the birth room.");
            Console.WriteLine("D: Show the maternity rooms and the rest rooms in use with the parent(s) and child(ren) using the room.");
            Console.WriteLine("E: Specific information about a specific planned birth");
            Console.WriteLine("F: Creation Section");
            Console.WriteLine("H: Removal Section");
        }

        public void Reset()
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

        public void ForceReset(string errorMessage)
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
        public char ReadSingleCharFromDisplay()
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
        public void MarioFunny()
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
        public void Case1(BirthClinicDbContext context)
        {
            Console.Clear();
            DateTime FilterDate = DateTime.Now.AddDays(3);
            List<Birth> BirthList = context.Births.Where(c => c.BirthDate < FilterDate).ToList();
            
            
            Console.WriteLine("Please enter a number between 1 and " + BirthList.Count + ", to view the specific birth's details.");
            int Choice = ReadAndParseInt32FromDisplay();
            Console.Clear();
            Birth B = BirthList.ElementAt(Choice - 1);
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
            foreach (var c in B.AssociatedClinicians)
            {
                Console.WriteLine(c.Role + ": " + c.FirstName + " " + c.LastName);
            }

            List<Reservation> ReservationList = context.Reservations.Where(r => r.AssociatedBirth == B).ToList();

            Console.WriteLine("This birth has the following reservations:");
            foreach (var r in ReservationList)
            {
                string room = r.ReservedRoom.RoomType.ToString();
                room = room.Substring(0, 1).ToUpper() + room[1..].ToLower();
                Console.WriteLine("Room #" + r.ReservedRoom.RoomId + " - " + room + " room.");
                Console.WriteLine("Between: " + r.StartTime.ToLongDateString() + " " + r.StartTime.ToShortTimeString() + " and " + r.EndTime.ToLongDateString() + " " + r.EndTime.ToShortTimeString());
            }
        }
        public void Case2(BirthClinicDbContext context)
        {
            Console.Clear();
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                Console.SetBufferSize(500, 1000);
            }
            ReservationRepository.PrintAvailableRoomsInNext5Days(context);
            ReservationRepository.PrintAvailableCliniciansInNext5Days(context);

        }

        public void Case3(BirthClinicDbContext context)
        {
            Console.Clear();
            DateTime FilterDate = DateTime.Now;
            List<Reservation> ReservationList = context.Reservations.Where(r =>
            r.ReservedRoom.RoomType == RoomType.BIRTH
            &&
            r.StartTime <= FilterDate
            &&
            r.EndTime > FilterDate).ToList();
            if (!ReservationList.Any())
            {
                Console.WriteLine("There are currently no ongoing births.");
                return;
            }
            foreach (var r in ReservationList)
            {
                var B = r.AssociatedBirth;
                Console.WriteLine("In Birthroom " + r.ReservedRoom.RoomId + ".");
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
                    foreach (Relative rel in B.Relatives)
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
        public void Case4(BirthClinicDbContext context)
        {
            Console.Clear();
            DateTime FilterDate = DateTime.Now;
            List<Reservation> ReservationList = context.Reservations.Where(r =>
            (r.ReservedRoom.RoomType == RoomType.MATERNITY || r.ReservedRoom.RoomType == RoomType.REST)
            &&
            r.StartTime <= FilterDate
            &&
            r.EndTime > FilterDate).ToList();
            if (!ReservationList.Any())
            {
                Console.WriteLine("There are currently no occupied resting or maternity rooms in use.");
                return;
            }
            foreach (var r in ReservationList)
            {
                var B = r.AssociatedBirth;
                string room = r.ReservedRoom.RoomType.ToString();
                room = $"{room.Substring(0, 1).ToUpper()}{room[1..].ToLower()}";

                Console.WriteLine("In " + room + " room " + r.ReservedRoom.RoomId + ".");
                Console.WriteLine("Mother: " + B.Mother.FirstName + " " + B.Mother.LastName);
                if (B.Father != null)
                {
                    Console.WriteLine("Father: " + B.Father.FirstName + " " + B.Father.LastName);
                }
                foreach (var c in B.ChildrenToBeBorn)
                {
                    Console.WriteLine("Child: " + c.FirstName + " " + c.LastName);
                }

                Console.WriteLine();

            }
        }
        public void Case5(BirthClinicDbContext context)
        {
            Console.Clear();
            List<Birth> BirthList = context.Births.ToList();
            int i = 0;
            foreach (var b in BirthList)
            {
                i++;
                Console.WriteLine("Journal for " + b.Mother.FirstName + "'s Planned birth - #" + i);
            }

            Console.WriteLine("Please enter a number according to the Journal you wish to read.");
            int Choice = ReadAndParseInt32FromDisplay();
            Birth B = BirthList.ElementAt(Choice - 1);
            Console.Clear();

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
            foreach (var c in B.AssociatedClinicians)
            {
                Console.WriteLine(c.Role + ": " + c.FirstName + " " + c.LastName);
            }

            List<Reservation> ReservationList = context.Reservations.Where(r => r.AssociatedBirth == B).ToList();

            Console.WriteLine("This birth has the following reservations:");
            foreach (var r in ReservationList)
            {
                string room = r.ReservedRoom.RoomType.ToString();
                room = room.Substring(0, 1).ToUpper() + room[1..].ToLower();
                Console.WriteLine("Room #" + r.ReservedRoom.RoomId + " - " + room + " room.");
                Console.WriteLine("Between: " + r.StartTime.ToLongDateString() + " " + r.StartTime.ToShortTimeString() + " and " + r.EndTime.ToLongDateString() + " " + r.EndTime.ToShortTimeString());
            }
        }

        public void EndBirth(BirthClinicDbContext context)
        {
            Console.Clear();
            List<Birth> BirthList = context.Births.Where(b =>b.IsEnded == false).ToList();
            int i = 0;
            foreach (var b in BirthList)
            {
                i++;
                Console.WriteLine("Journal for " + b.Mother.FirstName + "'s Planned birth - #" + i);
            }
            Console.WriteLine("Please choose one of the following active births to end.");
            int Choice = ReadAndParseInt32FromDisplay();
            Birth B = BirthList.ElementAt(Choice - 1);
            B.IsEnded = true;
            context.SaveChanges();
            ForceReset("Birth has been ended.");
        }

        public void RemoveReservation(BirthClinicDbContext context)
        {
            Console.Clear();
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                Console.SetBufferSize(500, 1000);
            }
            List<Reservation> ReservationList = context.Reservations.OrderBy(r => r.ReservedRoom.RoomId).ToList();
            int i = 0;
            foreach (var r in ReservationList)
            {
                i++; 
                string room = r.ReservedRoom.RoomType.ToString();
                room = $"{room.Substring(0, 1).ToUpper()}{room[1..].ToLower()}";

                Console.WriteLine("Reservation for " +room + " room - Reservation nr. " + i);
                Console.WriteLine("From " + r.StartTime.ToLongDateString() +" "+ r.StartTime.ToLongTimeString() + " Until " +r.EndTime.ToLongDateString() + " " + r.EndTime.ToLongTimeString());
                Console.WriteLine();
            }
            Console.WriteLine("Please choose one of the following active Reservations to removed.");
            int Choice = ReadAndParseInt32FromDisplay();
            Reservation R = ReservationList.ElementAt(Choice - 1);
            context.Remove(R);
            context.SaveChanges();
            ForceReset("Reservation has been removed.");
        }
    }
}