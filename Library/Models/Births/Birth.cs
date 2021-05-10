using Library.Models.Clinicians;
using Library.Models.FamilyMembers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Library.Models.Births
{
    public class Birth
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int BirthId { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime BirthDate { get; set; }

        public ICollection<Clinician> AssociatedClinicians { get; set; }

        [Required]
        public ICollection<Child> ChildrenToBeBorn { get; set; }

        [Required]
        public Mother Mother { get; set; }
        public int MotherForeignKey { get; set; }

        [Required]
        public bool IsEnded { get; set; }

        //optional
        public Father Father { get; set; }
        public int? FatherForeignKey { get; set; }

        //optional
        public ICollection<Relative> Relatives { get; set; }


    }
}
