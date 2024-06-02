using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary1
{
    public class PersonCollection : IEnumerable<Person>
    {
        private List<Person> people = new List<Person>();

        public void Add(Person person)
        {
            people.Add(person);
        }

        public bool Remove(Person person)
        {
            return people.Remove(person);
        }

        public IEnumerator<Person> GetEnumerator()
        {
            return people.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void LoadFromCsv(FileInfo csvFile)
        {

            using (TextFieldParser parser = new TextFieldParser(csvFile.FullName))
            {
                parser.TextFieldType = FieldType.Delimited;
                parser.SetDelimiters(",");

                while (!parser.EndOfData)
                {
                    string[] fields = parser.ReadFields();

                    List<string> departments = fields[5].Split(',').Select(d => d.Trim()).ToList();

                    // Construct Person object using fields array
                    Person person = new Person(
                        fields[0],
                        fields[1], 
                        fields[2], 
                        fields[3], 
                        fields[4],
                        departments // Departments
                    );

                    Add(person);
                }
            }
        }
        public List<string> departments1()
        {
            var departments = people
     .Where(p => p.Departments != null) // Filter out persons with null Departments
     .SelectMany(p => p.Departments)
     .Distinct()
     .OrderBy(d => d)
     .ToList();
            return departments;


        }

            public DepartmentReport[] GenerateDepartmentReports()
        {
            var departments = people
     .Where(p => p.Departments != null) // Filter out persons with null Departments
     .SelectMany(p => p.Departments)
     .Distinct()
     .OrderBy(d => d)
     .ToList();

            var reports = new List<DepartmentReport>();

            foreach (var department in departments)
            {
                var employees = people.Where(p => p.Departments.Contains(department) && p.Position != "doktorand").ToList();
                var phdStudents = people.Where(p => p.Departments.Contains(department) && p.Position == "doktorand").ToList();

                var head = employees.FirstOrDefault(p => p.Position == "vedúci");
                var deputy = employees.FirstOrDefault(p => p.Position == "zástupca vedúceho");
                var secretary = employees.FirstOrDefault(p => p.Position == "sekretárka");

                var numberOfProfessors = employees.Count(p => p.DisplayName.Contains("prof."));
                var numberOfAssociateProfessors = employees.Count(p => p.DisplayName.Contains("doc."));

                var report = new DepartmentReport(
                    department,
                    head,
                    deputy,
                    secretary,
                    numberOfProfessors,
                    numberOfAssociateProfessors,
                    employees,
                    phdStudents
                );

                reports.Add(report);
            }

            return reports.ToArray();
        }
    }
}
