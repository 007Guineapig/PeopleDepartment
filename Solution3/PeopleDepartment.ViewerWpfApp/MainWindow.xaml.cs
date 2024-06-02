using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ClassLibrary1;
using Microsoft.Win32;

namespace PeopleDepartment.ViewerWpfApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        PersonCollection p;
        string selectedItem;
        DepartmentReport[] pomocna;
        public MainWindow()
        {
            InitializeComponent();
            p = new PersonCollection();
            

        }
        private void PopulateComboBox()
        {
            var departments = p.departments1();
            combobox1.Items.Clear(); // Clear any existing items

            foreach (var department in departments)
            {
                combobox1.Items.Add(department);
            }

            // Optionally, select the first item
            if (combobox1.Items.Count > 0)
            {
                combobox1.SelectedIndex = 0;
            }
        }



        private void Nacitanie(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "CSV Files (*.csv)|*.csv|All Files (*.*)|*.*",
                FilterIndex = 1,
                InitialDirectory = @"C:\",
                RestoreDirectory = true
            };

            bool? result = openFileDialog.ShowDialog();
            if (result == true)
            {
                string filePath = openFileDialog.FileName;
                try
                {
                    p.LoadFromCsv(new FileInfo(filePath)); // Pass FileInfo to LoadFromCsv
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error loading CSV file: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            pomocna = p.GenerateDepartmentReports();
            PopulateComboBox();
        }

        private void combobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            
                selectedItem = combobox1.SelectedItem.ToString();
                naplnZamestnancov();
            

        }

        private void naplnZamestnancov()
        {
            listViewEmployees.Items.Clear();
           
            foreach (var l in pomocna)
            {

                if (l.Department == selectedItem)
                {
                    var gl = l.Employees;
                    foreach (var aa in gl)
                    {
                        listViewEmployees.Items.Add(aa);

                    }
                    t1.Text = l.Head.DisplayName;
                    t2.Text = l.Deputy.DisplayName;
                    t3.Text = l.Secretary.DisplayName;
                    
                    emp.Text = "Počet: "+ l.NumberOfEmployees.ToString();
                    phd.Text = "Počet: " + l.NumberOfPhDStudents.ToString();

                }
              
                
                
            }
            listViewEmployees1.Items.Clear();
            foreach (var l in pomocna)
            {

                if (l.Department == selectedItem)
                {
                    var gl = l.PhDStudents;
                    foreach (var aa in gl)
                    {
                        listViewEmployees1.Items.Add(aa);

                    }
                    

                }
                

                
            }
            


        }
    }
}
