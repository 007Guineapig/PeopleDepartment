namespace ClassLibrary1
{
    public class Person
    {

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string DisplayName { get; set; }
        public string? Position { get; set; }
        public string Email { get; set; }
        public List<string> Departments { get; set; }

        public Person(string firstName, string lastName, string displayName, string? position, string email, List<string> departments)
        {
            FirstName = firstName;
            LastName = lastName;
            DisplayName = displayName;
            Position = position;
            Email = email;
            Departments = departments;
        }

       

        public string TitleBefore
        {
            get
            {
                var parts = DisplayName.Split(' ');
                if (parts.Length > 2)
                {
                    var titlesBefore = string.Join(" ", parts.Take(parts.Length - 2));

                    if (titlesBefore.Contains(FirstName))
                        titlesBefore = titlesBefore.Replace(FirstName, "");
                    
                    return titlesBefore.Contains('.') ? titlesBefore : null;
                }
                return null;
            }
        }

        public string TitleAfter
        {
            get
            {
                var parts = DisplayName.Split(new[] { ',' }, 2);
                if (parts.Length > 1)
                {
                    return parts[1].Trim();
                }
                return null;
            }
        }
        public string gr()
        {

            return DisplayName;
        }

        public string ToFormattedString()
        {
            return $"{DisplayName,-40} {Email}";
        }
    }
}