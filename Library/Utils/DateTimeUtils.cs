using Library.Models.Births;
using Library.Models.Clinicians;
using Library.Models.Reservations;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Utils
{
    public class DateTimeUtils
    {
        public static List<DateTime> FindAvailableRoomTimeSlots(List<Reservation> reservations, TimeSpan TimeRequiredForSlot)
        {
            List<DateTime> foundSlots = new();


            //start at 1 as we will compare i-1 in first iteration.
            for (int i = 0; i <= reservations.Count; i++)
            {
                if (reservations.Count == 0)
                {
                    foundSlots.Add(DateTime.Now);
                    return foundSlots;
                }
                else if (i == 0)
                {
                    if ((DateTime.Now - reservations[i].StartTime) > TimeRequiredForSlot)
                    {
                        foundSlots.Add(DateTime.Now);
                    }
                }
                else if (i == reservations.Count)
                {
                    if ((reservations[i - 1].EndTime - DateTime.Now.AddDays(5)) > TimeRequiredForSlot)
                    {
                        foundSlots.Add(reservations[i - 1].EndTime);
                    }
                }
                else if ((reservations[i - 1].EndTime - reservations[i].StartTime) > TimeRequiredForSlot)
                {
                    foundSlots.Add(reservations[i - 1].EndTime);
                }
            }
            return foundSlots;
        }

        public static List<DateTime> FindAvailableClinicianTimeSlots(Clinician clinician, DateTime Start, DateTime End, TimeSpan RequiredDelta, int AllowedOccurences, List<Birth> births)
        {
            List<DateTime> foundSlots = new();
            for (int i = 0; i < births.Count; i++)
            {
                var currentTime = i == 0 ? Start : births[i].BirthDate;
                int CurrentOccurrences = 0;

                for (int j = i + 1; j < births.Count; j++)
                {
                    if (CurrentOccurrences > AllowedOccurences) break;

                    var nextTime = j == births.Count - 1 ? End : births[j].BirthDate;

                    if (nextTime - currentTime > RequiredDelta)
                    {
                        foundSlots.Add(currentTime);
                        break;
                    }
                    else
                    {
                        CurrentOccurrences++;
                    }
                }
            }
            return foundSlots;
        }
    }
}
