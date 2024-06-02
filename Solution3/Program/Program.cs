using ClassLibrary1;

namespace Program
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var arguments = ParseArguments(args);
                if (!arguments.ContainsKey("input") || !arguments.ContainsKey("template"))
                {
                    Console.Error.WriteLine("Required arguments: --input <path_to_csv> --template <path_to_template>");
                    return;
                }

                var inputFilePath = arguments["input"];
                var templateFilePath = arguments["template"];
                var outputDirectory = arguments.ContainsKey("output") ? arguments["output"] : null;

                if (!File.Exists(inputFilePath))
                {
                    Console.Error.WriteLine($"Input file does not exist: {inputFilePath}");
                    return;
                }

                if (!File.Exists(templateFilePath))
                {
                    Console.Error.WriteLine($"Template file does not exist: {templateFilePath}");
                    return;
                }

                var personCollection = new PersonCollection();
                personCollection.LoadFromCsv(new FileInfo(inputFilePath));

                var template = File.ReadAllText(templateFilePath);
                var reports = personCollection.GenerateDepartmentReports();

                if (outputDirectory != null)
                {
                    if (!Directory.Exists(outputDirectory))
                    {
                        Directory.CreateDirectory(outputDirectory);
                    }

                    foreach (var report in reports)
                    {
                        var reportContent = GenerateReportContent(template, report);
                        var reportFilePath = Path.Combine(outputDirectory, $"{report.Department}.txt");
                        File.WriteAllText(reportFilePath, reportContent);
                    }
                }
                else
                {
                    foreach (var report in reports)
                    {
                        var reportContent = GenerateReportContent(template, report);
                        Console.WriteLine(reportContent);
                        Console.WriteLine(new string('-', 80)); // Separator between reports
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"An error occurred: {ex.Message}");
            }
        }

        static Dictionary<string, string> ParseArguments(string[] args)
        {
            var arguments = new Dictionary<string, string>();
            for (int i = 0; i < args.Length - 1; i += 2)
            {
                if (args[i].StartsWith("--"))
                {
                    arguments[args[i].Substring(2)] = args[i + 1];
                }
            }
            return arguments;
        }

        static string GenerateReportContent(string template, DepartmentReport report)
        {
            var reportContent = template
                .Replace("[[Department]]", report.Department)
                .Replace("[[Head]]", report.Head != null ? $"{report.Head.DisplayName} ({report.Head.TitleBefore} {report.Head.TitleAfter})" : "N/A")
                .Replace("[[Deputy]]", report.Deputy != null ? $"{report.Deputy.DisplayName} ({report.Deputy.TitleBefore} {report.Deputy.TitleAfter})" : "N/A")
                .Replace("[[Secretary]]", report.Secretary != null ? $"{report.Secretary.DisplayName} ({report.Secretary.TitleBefore} {report.Secretary.TitleAfter})" : "N/A")
                .Replace("[[NumberOfEmployees]]", report.NumberOfEmployees.ToString())
                .Replace("[[NumberOfProfessors]]", report.NumberOfProfessors.ToString())
                .Replace("[[NumberOfAssociateProfessors]]", report.NumberOfAssociateProfessors.ToString())
                .Replace("[[NumberOfPhDStudents]]", report.NumberOfPhDStudents.ToString())
                .Replace("[[Employees]]", string.Join(Environment.NewLine, report.Employees.Select(e => $"{e.ToFormattedString()} ({e.TitleBefore} {e.TitleAfter})")))
                .Replace("[[PhDStudents]]", string.Join(Environment.NewLine, report.PhDStudents.Select(e => $"{e.ToFormattedString()} ({e.TitleBefore} {e.TitleAfter})")));

            return reportContent;
        }
    }
}