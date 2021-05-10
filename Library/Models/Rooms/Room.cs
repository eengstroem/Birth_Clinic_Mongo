using Library.Models.Reservations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Library.Models.Rooms
{
    public enum RoomType
    {
        MATERNITY,
        BIRTH,
        REST
    }

    public class Room
    {
        // This can also be used as the actual room number.
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RoomId { get; set; }

        [Required]
        public RoomType RoomType { get; set; }

        //optional
        public ICollection<Reservation> CurrentReservations { get; set; }

    }
}
