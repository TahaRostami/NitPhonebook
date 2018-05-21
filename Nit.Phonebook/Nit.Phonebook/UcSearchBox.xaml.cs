using Nit.Phonebook.Logics;
using Nit.Phonebook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
using static Nit.Phonebook.Logics.SearchBoxSearchProvider;

namespace Nit.Phonebook
{
    /// <summary>
    /// Interaction logic for UcSearchBox.xaml
    /// </summary>
    public partial class UcSearchBox : UserControl
    {
        public UcSearchBox()
        {
            InitializeComponent();
            txtSearch.ItemTemplateSelector = new CustomTemplateSelector();
            txtSearch.Provider = new SearchBoxSearchProvider();
            (txtSearch.Provider as SearchBoxSearchProvider).SearchType = TypeOfSeacrh.EMPLOYEE;// searchEmp.IsChecked.HasValue ? searchEmp.IsChecked.Value == true ? TypeOfSeacrh.EMPLOYEE : TypeOfSeacrh.PHONE : TypeOfSeacrh.PHONE;
        }

        private async void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            progressBar.Visibility = Visibility.Visible;

            await Task.Run(() =>
            {
                string filter = "";
                TypeOfSeacrh typeOfSeacrh = TypeOfSeacrh.EMPLOYEE;
                Dispatcher.Invoke(() =>
                {
                    typeOfSeacrh = (txtSearch.Provider as SearchBoxSearchProvider).SearchType;
                    filter = txtSearch.Text;
                    lstSearchResults.Items.Clear();
                });
                using (PhonebookContext db = new PhonebookContext(ConnectionStringBuilder.ConnectionString))
                {

                    List<string> result = new List<string>();
                    if (typeOfSeacrh == TypeOfSeacrh.EMPLOYEE)
                    {
                        var lst = db.Employees.Where(p => p.Name.Contains(filter));
                        foreach (var emp in lst)
                        {
                            var rows = emp.Rows;
                            foreach (var row in rows)
                            {
                                string catName = row.Category.Title;

                                var phoneNumbers = row.PhoneNumbers;

                                string str = catName + Environment.NewLine + Environment.NewLine;
                                foreach (var ph in phoneNumbers)
                                {
                                    //result.Add($"{catName}\t{ph.Number}");
                                    str += ph.Number + "," + Environment.NewLine + Environment.NewLine;
                                }
                                str += "- - - - - - - - - - - -";
                                result.Add(str);
                            }
                        }
                    }
                    else if (typeOfSeacrh == TypeOfSeacrh.PHONE)
                    {
                        var lst = db.PhoneNumbers.Where(p => p.Number.Contains(filter));
                        foreach (var ph in lst)
                        {
                            var rows = ph.Rows;
                            foreach (var row in rows)
                            {
                                string catName = row.Category.Title;

                                var empName = row.Employees;

                                string str = catName + Environment.NewLine + Environment.NewLine;
                                foreach (var emp in empName)
                                {
                                    //result.Add($"{catName}\t{ph.Number}");
                                    str += emp.Name + "," + Environment.NewLine + Environment.NewLine;
                                }
                                str += "- - - - - - - - - - - -";
                                result.Add(str);
                            }
                        }
                    }
                    result.Distinct().OrderBy(r => r).ToList().ForEach(r =>
                    {
                        Dispatcher.Invoke(() =>
                        {
                            lstSearchResults.Items.Add(r);
                        });

                    });

                };
            });

            progressBar.Visibility = Visibility.Collapsed;
        }

        private void searchEmp_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                if (sender == seacrhPh)
                {
                    (txtSearch.Provider as SearchBoxSearchProvider).SearchType = TypeOfSeacrh.PHONE;
                }
                else if (sender == searchEmp)
                {
                    (txtSearch.Provider as SearchBoxSearchProvider).SearchType = TypeOfSeacrh.EMPLOYEE;
                }
            }
            catch { }
        }

        //delete item
        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            string filter = "";
            bool Emp = searchEmp.IsChecked.HasValue && searchEmp.IsChecked.Value == true ? true : false;
            if (txtSearch.Text != null)
            {
                filter = txtSearch.Text;
            }

            var result = MessageBox.Show(
                filter,
                "حذف",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question,
                MessageBoxResult.No);
            if (result == MessageBoxResult.Yes)
            {
                bool succ = false;
                try
                {
                    await Task.Run(() =>
                    {
                        using (PhonebookContext db = new PhonebookContext(ConnectionStringBuilder.ConnectionString))
                        {
                            using (var dbContextTransaction = db.Database.BeginTransaction())
                            {
                                try
                                {
                                    if (Emp)
                                    {
                                        var employees = db.Employees.Where(r => r.Name.CompareTo(filter) == 0).ToList();
                                        employees.ForEach(emp =>
                                        {
                                            var rows = emp.Rows.ToList();
                                            rows.ForEach(r =>
                                            {
                                                r.Employees.Remove(emp);
                                            });

                                            db.Employees.Remove(emp);
                                        });

                                    }
                                    else
                                    {
                                        var phones = db.PhoneNumbers.Where(r => r.Number.CompareTo(filter) == 0).ToList();
                                        phones.ForEach(ph =>
                                        {
                                            var rows = ph.Rows.ToList();
                                            rows.ForEach(r =>
                                            {
                                                r.PhoneNumbers.Remove(ph);
                                            });
                                            db.PhoneNumbers.Remove(ph);
                                        });


                                    }

                                    int x = db.SaveChanges();
                                    if (x > 0)
                                    {
                                        succ = true;
                                    }
                                    dbContextTransaction.Commit();
                                }
                                catch
                                {
                                    dbContextTransaction.Rollback();
                                }
                            }
                        }
                    });
                }
                catch
                {

                }
                if (succ)
                {
                    MessageBox.Show("حذف با موفقیت انجام شد");
                }
                else
                {
                    MessageBox.Show("عدم موفقیت");
                }
            }

        }

    }
}
