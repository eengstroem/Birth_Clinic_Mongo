using Bogus;
using Library.Models.FamilyMembers;

namespace Library.Factory.FamilyMembers
{
    public class FamilyMemberFactory
    {
        public enum FamilyMemberType
        {
            FATHER,
            MOTHER,
            CHILD,
            RELATIVE
        }
        public static FamilyMember CreateFakeFamilyMember(FamilyMemberType type)
        {

            var faker = new Faker("en");
            switch (type)
            {
                case FamilyMemberType.FATHER:
                    var father = new Father
                    {
                        FirstName = faker.Name.FirstName(Bogus.DataSets.Name.Gender.Male),
                        LastName = faker.Name.LastName()
                    };
                    return father;
                case FamilyMemberType.MOTHER:
                    var mother = new Mother
                    {
                        FirstName = faker.Name.FirstName(Bogus.DataSets.Name.Gender.Female),
                        LastName = faker.Name.LastName()
                    };
                    return mother;
                case FamilyMemberType.CHILD:
                    var child = new Child
                    {
                        FirstName = faker.Name.FirstName(),
                        LastName = faker.Name.LastName()
                    };
                    return child;
                case FamilyMemberType.RELATIVE:
                    var relative = new Relative
                    {
                        FirstName = faker.Name.FirstName(),
                        LastName = faker.Name.LastName()
                    };
                    return relative;
            }
            return null;
        }
    }
}
