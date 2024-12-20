namespace LibraryApp.Entity
{
    public abstract class PersonDto
    {

        public string FirstName { get; set; }

        public string SurName { get; set; }

        public string Patronomic { get; set; }

        public string Sex { get; set; }

        public DateTime DateOfBirthday { get; set; }

    }
}
