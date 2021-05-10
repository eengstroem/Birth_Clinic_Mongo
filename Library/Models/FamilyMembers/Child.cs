using Library.Models.Births;
using System.ComponentModel.DataAnnotations;

namespace Library.Models.FamilyMembers
{
    public class Child : FamilyMember
    {

        [Required]
        public Birth AssociatedBirth { get; set; }
    }
}
