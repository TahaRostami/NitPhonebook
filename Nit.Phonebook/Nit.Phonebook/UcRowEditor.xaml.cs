using System;
using System.Collections.Generic;
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
using Nit.Phonebook.Models;
using System.Collections.ObjectModel;
using Nit.Phonebook.Logics;
using System.Threading;
using System.ComponentModel;
using System.Globalization;

namespace Nit.Phonebook
{
    /// <summary>
    /// Interaction logic for UcRowEditor.xaml
    /// </summary>
    public partial class UcRowEditor : UserControl
    {
        public UcRowEditor()
        {
            InitializeComponent();
            Thread.CurrentThread.CurrentCulture = new CultureInfo("fa-IR");


            dataGrid1.AutoGeneratingColumn += dataGrid1_AutoGeneratingColumn;
            dataGrid1.PreviewKeyDown += DataGrid1_PreviewKeyDownAsync;
        }
        private void dataGrid1_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            var desc = e.PropertyDescriptor as PropertyDescriptor;
            var att = desc.Attributes[typeof(ColumnNameAttribute)] as ColumnNameAttribute;
            if (att != null)
            {
                e.Column.Header = att.Name;
                e.Column.IsReadOnly = true;

            }
        }
        private async void DataGrid1_PreviewKeyDownAsync(object sender, KeyEventArgs e)
        {

            DataGrid dataGrid = sender as DataGrid;
            if (dataGrid != null)
            {
                DataGridRow dataGridRow = (DataGridRow)(dataGrid.ItemContainerGenerator.ContainerFromIndex(dataGrid.SelectedIndex));

                if (e.Key == Key.Delete && !dataGridRow.IsEditing)
                {
                    // User is attempting to delete the row
                    var result = MessageBox.Show(
                        "آیا می خواهید سطر (های) انتخاب شده حذف شوند؟",
                        "حذف",
                        MessageBoxButton.YesNo,
                        MessageBoxImage.Question,
                        MessageBoxResult.No);
                    if (result == MessageBoxResult.Yes)
                    {
                        try
                        {
                            List<string> ids = new List<string>();
                            var selectedItmes = dataGrid.SelectedItems;
                            foreach (var s in selectedItmes)
                            {
                                GridRow gridRow = (GridRow)s;
                                ids.Add(gridRow.Id);
                            }

                            await Task.Run(() =>
                            {
                                try
                                {
                                    bool succ = false;
                                    using (PhonebookContext db = new PhonebookContext(ConnectionStringBuilder.ConnectionString))
                                    {
                                        using (var dbContextTransaction = db.Database.BeginTransaction())
                                        {

                                            try
                                            {
                                                var rows = db.Rows.Where(row => ids.Any(id2 => id2 == row.Id.ToString())).ToList();
                                                //var items = db.Rows.RemoveRange(db.Rows.Where(row => ids.Any(id2 => id2 == row.Id.ToString())));
                                                rows.ForEach(r =>
                                                {
                                                    var employees = r.Employees.ToList();
                                                    var phones = r.PhoneNumbers.ToList();
                                                    employees.ForEach(emp =>
                                                    {
                                                        r.Employees.Remove(emp);
                                                    });
                                                    phones.ForEach(ph =>
                                                    {
                                                        r.PhoneNumbers.Remove(ph);
                                                    });

                                                    db.Rows.Remove(r);

                                                });


                                                int x = db.SaveChanges();

                                                if (x > 0)
                                                {
                                                    succ = true;
                                                    dbContextTransaction.Commit();
                                                }

                                                else throw new Exception();

                                            }
                                            catch
                                            {
                                                dbContextTransaction.Rollback();
                                            }
                                        }
                                    }
                                    if (succ == false)
                                        throw new Exception();
                                }
                                catch (Exception ex)
                                {
                                    Dispatcher.Invoke(() =>
                                    {
                                        e.Handled = true;
                                    });

                                }
                            });


                        }
                        catch (Exception ex)
                        {
                            e.Handled = true;
                        }

                    }
                    else
                    {
                        e.Handled = true;
                    }

                }
            }
        }

        private string categoryId;

        public string CategoryId
        {
            get { return categoryId; }
            set
            {
                bool condition = categoryId == value;

                if (value != "1")
                    categoryId = value;

                if (!condition && value != "1")
                {
                    ReFillDataGrid();
                }

            }
        }


        internal async virtual void ReFillDataGrid()
        {
            Dispatcher.Invoke(() =>
            {
                progressBar.Visibility = Visibility.Visible;
                btnInsert.IsEnabled = false;
                btnUpdate.IsEnabled = false;

            });

            await Task.Run(() =>
            {
                try
                {
                    //Thread.Sleep(1000);
                    PhonebookContext db = new PhonebookContext(ConnectionStringBuilder.ConnectionString);
                    var x = db.Categories.SingleOrDefault(r => r.Id.ToString() == categoryId);
                    if (x != null)
                    {
                        //dataGrid1.ItemsSource = x.Rows.ToList().Select(r => new { Employee = r.Employees.ToList(), Internal = r.PhoneNumbers.Where(t => t.IsInternal).ToList(), External = r.PhoneNumbers.Where(t => !t.IsInternal).ToList() }).ToList();
                    }

                    List<GridRow> Rows = new List<GridRow>();
                    x.Rows.ToList().ForEach(r =>
                    {
                        GridRow newRow = new GridRow();
                        newRow.Id = r.Id.ToString();

                        foreach (var emp in r.Employees)
                        {
                            newRow.Employees += emp.Name + Environment.NewLine + Environment.NewLine;
                        }

                        foreach (var phone in r.PhoneNumbers)
                        {
                            if (phone.IsInternal)
                            {


                                newRow.InternalNumbers += phone.Number.AddHalfSpace() + Environment.NewLine + Environment.NewLine;
                            }
                            else
                            {
                                newRow.ExternalNumbers += phone.Number.AddHalfSpace() + Environment.NewLine + Environment.NewLine;
                            }
                        }
                        Rows.Add(newRow);
                    });
                    Dispatcher.Invoke(() =>
                    {
                        dataGrid1.ItemsSource = Rows;
                    });

                }
                catch { }
            });

            Dispatcher.Invoke(() =>
            {
                progressBar.Visibility = Visibility.Collapsed;
                btnInsert.IsEnabled = true;
                btnUpdate.IsEnabled = true;
            });

        }

        public class GridRow
        {
            [ColumnName("شناسه")]
            public string Id { get; set; }
            [ColumnName("داخلی")]
            public string InternalNumbers { get; set; } = "";
            [ColumnName("مستقیم")]
            public string ExternalNumbers { get; set; } = "";
            [ColumnName("اسامی")]
            public string Employees { get; set; } = "";
        }



        private void btnExecute_ClickAsync(object sender, RoutedEventArgs e)
        {
            btnExecute.IsEnabled = false;
            ReFillDataGrid();
            btnExecute.IsEnabled = true;
        }


        public event EventHandler<Tuple<string, string>> RequestEditorPageToShow;//catId , Insertmode | updatemode

        protected virtual void OnRequestEditorPageToShow(object sender, Tuple<string, string> e)
        {
            RequestEditorPageToShow?.Invoke(sender, e);
        }

        private void btnInsert_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                OnRequestEditorPageToShow(this, Tuple.Create(categoryId, "InsertMode"));
            }
            catch { }
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                OnRequestEditorPageToShow(this, Tuple.Create(categoryId, (dataGrid1.SelectedItem as GridRow).Id));

            }
            catch { }
        }


        private void dataGrid1_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            btnUpdate_Click(null, null);
        }

    }
}
