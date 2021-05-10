using Library.Models.Births;
using System.ComponentModel.DataAnnotations;

namespace Library.Models.FamilyMembers
{
    public class Mother : FamilyMember
    {
        [Required]
        public Birth AssociatedBirth { get; set; }
    }
}
