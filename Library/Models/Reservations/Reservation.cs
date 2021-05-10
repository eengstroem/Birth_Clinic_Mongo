using Library.Models.Births;
using Library.Models.Rooms;
using System;
using System.ComponentModel.DataAnnotations;

namespace Library.Models.Reservations
{
    public class Reservation
    {
        [Key]
        public int ReservationId { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime StartTime { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime EndTime { get;  set; }

        [Required]
        public Room ReservedRoom { get;  set; }

        [Required]
        public Birth AssociatedBirth { get;  set; }

        public int? BirthId { get; set; }

        [Required]
        public bool IsEndedEarly { get; set; } = false;

    }
}
