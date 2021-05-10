using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Library.Models.FamilyMembers
{

    public abstract class FamilyMember
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int FamilyMemberId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

    }
}
