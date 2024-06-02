using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary1
{
    public class DepartmentReport
    {
        public string Department { get; set; }
        public Person? Head { get; set; }
        public Person? Deputy { get; set; }
        public Person? Secretary { get; set; }
        public int NumberOfProfessors { get; set; }
        public int NumberOfAssociateProfessors { get; set; }
        public int NumberOfEmployees => Employees.Count();
        public int NumberOfPhDStudents => PhDStudents.Count();
        public IEnumerable<Person> Employees { get; set; }
        public IEnumerable<Person> PhDStudents { get; set; }

        public DepartmentReport(
            string department,
            Person? head,
            Person? deputy,
            Person? secretary,
            int numberOfProfessors,
            int numberOfAssociateProfessors,
            IEnumerable<Person> employees,
            IEnumerable<Person> phdStudents)
        {
            Department = department;
            Head = head;
            Deputy = deputy;
            Secretary = secretary;
            NumberOfProfessors = numberOfProfessors;
            NumberOfAssociateProfessors = numberOfAssociateProfessors;
            Employees = employees.OrderBy(p => p.LastName).ThenBy(p => p.FirstName).ToList();
            PhDStudents = phdStudents.OrderBy(p => p.LastName).ThenBy(p => p.FirstName).ToList();
        }
    }
}
