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
using Nit.Phonebook.Logics;
using Nit.Phonebook.Models.Data;
using Nit.Phonebook.Models;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Data;
using System.Threading;
using System.Data.Entity;
using System.Runtime.CompilerServices;
using System.Data.SqlClient;

namespace Nit.Phonebook
{
    /// <summary>
    /// Interaction logic for UcWindowRowEditor.xaml
    /// </summary>
    public partial class UcWindowRowEditor : UserControl
    {
        public event EventHandler<CustomDialogResult> UcClosing;

        protected virtual void OnClosing(object sender, CustomDialogResult e)
        {
            UcClosing?.Invoke(this, e);
        }

        public UcWindowRowEditor()
        {
            InitializeComponent();
        }
        public CustomDialogResult CustomDialogResult { get; set; } = CustomDialogResult.OPEN;

        public string CategoryId { get; set; }
        public string RowId { get; set; } = "InsertMode";//default

        ObservableCollection<LocalEmployee> localEmployees = new ObservableCollection<LocalEmployee>();
        ObservableCollection<LocalPhone> localPhones = new ObservableCollection<LocalPhone>();

        private async void DataGridPhone_PreviewKeyDownAsync(object sender, KeyEventArgs e)
        {
            DataGrid dataGrid = sender as DataGrid;
            if (dataGrid != null)
            {
                DataGridRow dataGridRow = (DataGridRow)(dataGrid.ItemContainerGenerator.ContainerFromIndex(dataGrid.SelectedIndex));

                if (e.Key == Key.Delete && !dataGridRow.IsEditing)
                {

                    var result = MessageBox.Show(
     "آیا می خواهید سطر (های) انتخاب شده حذف شوند؟",
                        "حذف",
                        MessageBoxButton.YesNo,
                        MessageBoxImage.Question,
                        MessageBoxResult.No);
                    if (result == MessageBoxResult.Yes)
                    {
                        Dispatcher.Invoke(() =>
                        {

                            toolBarTrayProgress.Visibility = Visibility.Visible;
                            dataGridPhone.IsEnabled = false;
                            dataGridEmployee.IsEnabled = false;
                            btnInsertEmployeeToRow.IsEnabled = false;
                            btnInsertPhoneToRow.IsEnabled = false;
                        });

                        try
                        {
                            List<string> ids = new List<string>();
                            var selectedItmes = dataGrid.SelectedItems;
                            foreach (var s in selectedItmes)
                            {
                                LocalPhone gridRow = (LocalPhone)s;
                                ids.Add(gridRow.Id);
                            }

                            bool conn = chkRemoveUnused.IsChecked.HasValue == true && chkRemoveUnused.IsChecked.Value == true;
                            bool succ = false;

                            await Task.Run(() =>
                            {
                                try
                                {

                                    PhonebookContext db = new PhonebookContext(ConnectionStringBuilder.ConnectionString);
                                    var row = db.Rows.SingleOrDefault(r => r.Id.ToString() == RowId);
                                    if (row != null)
                                    {
                                        using (var dbContextTransaction = db.Database.BeginTransaction())
                                        {
                                            try
                                            {
                                                var selectedPhones = db.PhoneNumbers.Where(ph => ids.Any(id2 => id2 == ph.Id.ToString())).ToList();

                                                if (conn)
                                                {
                                                    selectedPhones.ForEach(r =>
                                                    {
                                                        if (r.Rows.Count <= 1)
                                                        {
                                                            db.PhoneNumbers.Remove(r);
                                                        }
                                                        else
                                                        {
                                                            row.PhoneNumbers.Remove(r);
                                                        }
                                                    });
                                                }
                                                else
                                                {
                                                    selectedPhones.ForEach(r => r.Rows.Remove(row));
                                                }


                                                int x = db.SaveChanges();
                                                if (x > 0)
                                                {

                                                }
                                                else
                                                {

                                                }

                                                dbContextTransaction.Commit();
                                                succ = true;

                                            }
                                            catch
                                            {
                                                dbContextTransaction.Rollback();
                                            }
                                        }
                                        if (!succ) throw new Exception();
                                    }
                                    else
                                    {
                                        throw new Exception();
                                    }
                                }
                                catch (Exception ex)
                                {
                                    Dispatcher.Invoke(() =>
                                    {
                                        e.Handled = true;
                                    });
                                }


                            });

                            if (succ)
                            {
                                try
                                {
                                    for (int i = selectedItmes.Count - 1; i >= 0; i--)
                                    {
                                        localPhones.Remove((LocalPhone)selectedItmes[i]);
                                    }
                                }
                                catch { }
                            }
                        }
                        catch
                        {
                            Dispatcher.Invoke(() =>
                            {
                                e.Handled = true;
                            });
                        }
                    }
                    else
                    {
                        e.Handled = true;
                    }
                    Dispatcher.Invoke(() =>
                    {
                        toolBarTrayProgress.Visibility = Visibility.Collapsed;
                        dataGridPhone.IsEnabled = true;
                        dataGridEmployee.IsEnabled = true;
                        btnInsertEmployeeToRow.IsEnabled = true;
                        btnInsertPhoneToRow.IsEnabled = true;
                    });

                }
                else
                {
                    e.Handled = true;
                }
            }
        }

        private async void DataGridPhone_RowEditEndingAsync(object sender, DataGridRowEditEndingEventArgs e)
        {
            Dispatcher.Invoke(() =>
            {
                toolBarTrayProgress.Visibility = Visibility.Visible;
                dataGridPhone.IsEnabled = false;
                dataGridEmployee.IsEnabled = false;
                btnInsertEmployeeToRow.IsEnabled = false;
                btnInsertPhoneToRow.IsEnabled = false;
            });

            LocalPhone oldItem = null;
            if (this.dataGridPhone.SelectedItem != null)
            {
                oldItem = new LocalPhone();

                oldItem.Id = (e.Row.Item as LocalPhone).Id;
                oldItem.PhoneNumber = (e.Row.Item as LocalPhone).PhoneNumber;
                oldItem.IsInternal = (e.Row.Item as LocalPhone).IsInternal;

                (sender as DataGrid).RowEditEnding -= DataGridPhone_RowEditEndingAsync;
                (sender as DataGrid).CommitEdit();
                (sender as DataGrid).Items.Refresh();
                (sender as DataGrid).RowEditEnding += DataGridPhone_RowEditEndingAsync;
            }
            else
            {
                return;
            }
            var newItem = e.Row.Item as LocalPhone;

            string number = newItem.PhoneNumber;
            bool isInternal = newItem.IsInternal;
            string id = newItem.Id;

            await Task.Run(async () =>
            {

                try
                {
                    PhonebookContext db = new PhonebookContext(ConnectionStringBuilder.ConnectionString);
                    bool condition = false;

                    Dispatcher.Invoke(() =>
                    {
                        condition = (newItem.PhoneNumber != oldItem.PhoneNumber || newItem.IsInternal != oldItem.IsInternal) && newItem.Id == oldItem.Id && !string.IsNullOrWhiteSpace(newItem.Id);
                    });

                    if (condition)
                    {


                        var ph = db.PhoneNumbers.SingleOrDefault(r => r.Id.ToString() == id);
                        if (ph != null)
                        {
                            //db.PhoneNumbers.Attach(ph);

                            //ph.Number = newItem.PhoneNumber;
                            //ph.IsInternal = newItem.IsInternal;
                            int x = 0;
                            if (ph.Number == number && ph.IsInternal != isInternal)
                            {
                                ph.Number = number;
                                ph.IsInternal = isInternal;
                                x = db.SaveChanges();
                            }
                            else if (ph.Number != number)
                            {
                                x = db.Database.ExecuteSqlCommand(System.Data.Entity.TransactionalBehavior.EnsureTransaction,
                                         SqlCammandHelper.CMD_UPDATE_INTO_PHONENUMBER_UNDER_CONSTRAINTS,
                                         new SqlParameter("@Id", id), new SqlParameter("@Number", number), new SqlParameter("IsInternal", isInternal)
                                          );
                            }
                            //db.Entry(ph).State = EntityState.Modified;

                            //int x = db.SaveChanges();
                            if (x > 0)
                            {

                            }
                            else
                            {
                                throw new Exception();
                            }
                        }
                        else
                        {
                            throw new Exception();
                        }
                    }

                }
                catch (Exception ex)
                {
                    await Dispatcher.InvokeAsync(() =>
                    {
                        MessageBox.Show("خطا");

                        if (string.IsNullOrWhiteSpace(oldItem.Id))
                        {
                            newItem.PhoneNumber = null;
                            newItem.IsInternal = false;
                            localPhones.Remove(newItem);
                        }
                        else
                        {
                            newItem.PhoneNumber = oldItem.PhoneNumber;
                            newItem.IsInternal = oldItem.IsInternal;

                        }
                       (sender as DataGrid).Items.Refresh();
                    });

                }

            });

            Dispatcher.Invoke(() =>
            {
                toolBarTrayProgress.Visibility = Visibility.Collapsed;
                dataGridPhone.IsEnabled = true;
                dataGridEmployee.IsEnabled = true;
                btnInsertEmployeeToRow.IsEnabled = true;
                btnInsertPhoneToRow.IsEnabled = true;
            });
        }


        private async void DataGridEmployee_PreviewKeyDownAsync(object sender, KeyEventArgs e)
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
                        Dispatcher.Invoke(() =>
                        {
                            //toolBarTray.IsEnabled = false;
                            toolBarTrayProgress.Visibility = Visibility.Visible;
                            dataGridPhone.IsEnabled = false;
                            dataGridEmployee.IsEnabled = false;
                            btnInsertEmployeeToRow.IsEnabled = false;
                            btnInsertPhoneToRow.IsEnabled = false;
                        });
                        try
                        {
                            List<string> ids = new List<string>();
                            var selectedItmes = dataGrid.SelectedItems;
                            foreach (var s in selectedItmes)
                            {
                                LocalEmployee gridRow = (LocalEmployee)s;
                                ids.Add(gridRow.Id);
                            }

                            bool conn = chkRemoveUnused.IsChecked.HasValue == true && chkRemoveUnused.IsChecked.Value == true;
                            bool succ = false;

                            await Task.Run(() =>
                            {

                                try
                                {

                                    PhonebookContext db = new PhonebookContext(ConnectionStringBuilder.ConnectionString);

                                    var row = db.Rows.SingleOrDefault(r => r.Id.ToString() == RowId);
                                    if (row != null)
                                    {
                                        using (var dbContextTransaction = db.Database.BeginTransaction())
                                        {
                                            try
                                            {
                                                var selectedEmployees = db.Employees.Where(emp => ids.Any(id2 => id2 == emp.Id.ToString())).ToList();

                                                if (conn)
                                                {
                                                    selectedEmployees.ForEach(r =>
                                                    {
                                                        if (r.Rows.Count <= 1)
                                                        {
                                                            db.Employees.Remove(r);
                                                        }
                                                        else
                                                        {
                                                            row.Employees.Remove(r);
                                                        }
                                                    });
                                                }
                                                else
                                                {
                                                    selectedEmployees.ForEach(r => r.Rows.Remove(row));
                                                }


                                                int x = db.SaveChanges();
                                                if (x > 0)
                                                {

                                                }
                                                else
                                                {

                                                }

                                                dbContextTransaction.Commit();
                                                succ = true;

                                            }
                                            catch
                                            {
                                                dbContextTransaction.Rollback();
                                            }
                                        }
                                        if (!succ) throw new Exception();
                                    }
                                    else
                                    {
                                        throw new Exception();
                                    }
                                }
                                catch (Exception ex)
                                {
                                    Dispatcher.Invoke(() =>
                                    {
                                        e.Handled = true;
                                    });

                                }

                            });


                            if (succ)
                            {
                                try
                                {
                                    for (int i = selectedItmes.Count - 1; i >= 0; i--)
                                    {
                                        localEmployees.Remove((LocalEmployee)selectedItmes[i]);
                                    }
                                }
                                catch { }
                            }

                        }
                        catch (Exception ex)
                        {
                            Dispatcher.Invoke(() =>
                            {
                                e.Handled = true;
                            });

                        }
                    }
                    else
                    {
                        e.Handled = true;
                    }

                    Dispatcher.Invoke(() =>
                    {
                        //toolBarTray.IsEnabled = false;
                        toolBarTrayProgress.Visibility = Visibility.Collapsed;
                        dataGridPhone.IsEnabled = true;
                        dataGridEmployee.IsEnabled = true;
                        btnInsertEmployeeToRow.IsEnabled = true;
                        btnInsertPhoneToRow.IsEnabled = true;
                    });

                }
            }
        }

        private async void DataGridEmployee_RowEditEndingAsync(object sender, DataGridRowEditEndingEventArgs e)
        {
            Dispatcher.Invoke(() =>
            {
                toolBarTrayProgress.Visibility = Visibility.Visible;
                dataGridPhone.IsEnabled = false;
                dataGridEmployee.IsEnabled = false;
                btnInsertEmployeeToRow.IsEnabled = false;
                btnInsertPhoneToRow.IsEnabled = false;
            });


            LocalEmployee oldItem = null;
            if (this.dataGridEmployee.SelectedItem != null)
            {
                oldItem = new LocalEmployee();

                oldItem.Id = (e.Row.Item as LocalEmployee).Id;
                oldItem.Name = (e.Row.Item as LocalEmployee).Name;

                (sender as DataGrid).RowEditEnding -= DataGridEmployee_RowEditEndingAsync;
                (sender as DataGrid).CommitEdit();
                (sender as DataGrid).Items.Refresh();
                (sender as DataGrid).RowEditEnding += DataGridEmployee_RowEditEndingAsync;
            }
            else
            {
                return;
            }

            var newItem = e.Row.Item as LocalEmployee;

            string name = newItem.Name;
            string id = newItem.Id;

            await Task.Run(async () =>
            {

                try
                {
                    PhonebookContext db = new PhonebookContext(ConnectionStringBuilder.ConnectionString);


                    bool condition = false;
                    Dispatcher.Invoke(() =>
                    {
                        condition = newItem.Name != oldItem.Name && newItem.Id == oldItem.Id && !string.IsNullOrWhiteSpace(newItem.Id);
                    });
                    if (condition)
                    {
                        var emp = db.Employees.SingleOrDefault(r => r.Id.ToString() == id);
                        if (emp != null)
                        {

                            int x = db.Database.ExecuteSqlCommand(System.Data.Entity.TransactionalBehavior.EnsureTransaction,
                                    SqlCammandHelper.CMD_UPDATE_INTO_EMPLOYEE_UNDER_CONSTRAINTS
                                    , new SqlParameter("@Id", emp.Id), new SqlParameter("@Name", name)
                                   );

                            if (x > 0)
                            {

                            }
                            else
                            {
                                throw new Exception();
                            }
                        }
                        else
                        {
                            throw new Exception();
                        }
                    }

                }
                catch (Exception ex)
                {

                    await Dispatcher.InvokeAsync(() =>
                    {
                        MessageBox.Show("خطا");

                        if (string.IsNullOrWhiteSpace(oldItem.Id))
                        {
                            newItem.Name = null;
                            localEmployees.Remove(newItem);
                        }
                        else
                        {
                            newItem.Name = oldItem.Name;
                        }
                           (sender as DataGrid).Items.Refresh();
                    });

                }

            });

            Dispatcher.Invoke(() =>
            {

                toolBarTrayProgress.Visibility = Visibility.Collapsed;
                dataGridPhone.IsEnabled = true;
                dataGridEmployee.IsEnabled = true;
                btnInsertEmployeeToRow.IsEnabled = true;
                btnInsertPhoneToRow.IsEnabled = true;
            });
        }



        CancellationTokenSource tokenSource = null;

        bool firstInit = true;



        public async void RunAfterConstructorToBind(string catId, string rowId = "InsertMode")
        {

            btnClose.IsEnabled = false;
            btnOk.IsEnabled = false;
            btnCancelAndClose.Visibility = Visibility.Visible;

            bool firstCon = firstInit;
            if (firstInit)
            {
                firstInit = false;
            }

            txtAutoEmployee.Text = "";
            txtAutoPhone.Text = "";

            localEmployees.Clear();
            localPhones.Clear();


            RowId = rowId;
            CategoryId = catId;


            if (firstCon)
            {
                dataGridEmployee.AutoGeneratingColumn += dataGrid_AutoGeneratingColumn;

                dataGridEmployee.ItemsSource = localEmployees;

                if (RowId != "InsertMode")
                    dataGridEmployee.RowEditEnding += DataGridEmployee_RowEditEndingAsync;
                if (RowId != "InsertMode")
                    dataGridEmployee.PreviewKeyDown += DataGridEmployee_PreviewKeyDownAsync;

                dataGridPhone.AutoGeneratingColumn += dataGrid_AutoGeneratingColumn;

                dataGridPhone.ItemsSource = localPhones;

                if (RowId != "InsertMode")
                    dataGridPhone.RowEditEnding += DataGridPhone_RowEditEndingAsync;
                if (RowId != "InsertMode")
                    dataGridPhone.PreviewKeyDown += DataGridPhone_PreviewKeyDownAsync;

                //firstInit = false;
            }

            tokenSource = new CancellationTokenSource();
            var token = tokenSource.Token;



            await Task.Run(() =>
            {

                try
                {
                    //Thread.Sleep(4000);


                    if (RowId.CompareTo("InsertMode") == 0)
                    {

                    }
                    else//UpdateMode
                    {

                        PhonebookContext db = new PhonebookContext(ConnectionStringBuilder.ConnectionString);

                        if (token.IsCancellationRequested)
                            token.ThrowIfCancellationRequested();

                        var row = db.Rows.SingleOrDefault(r => r.Id.ToString() == RowId);

                        if (token.IsCancellationRequested)
                            token.ThrowIfCancellationRequested();

                        row.Employees.ToList().ForEach(emp =>
                        {
                            LocalEmployee lemp = new LocalEmployee()
                            {
                                Id = emp.Id.ToString(),
                                Name = emp.Name
                            };
                            Dispatcher.Invoke(() =>
                            {
                                localEmployees.Add(lemp);
                            });

                        });

                        if (token.IsCancellationRequested)
                            token.ThrowIfCancellationRequested();

                        row.PhoneNumbers.ToList().ForEach(ph =>
                        {

                            LocalPhone lph = new LocalPhone()
                            {

                                Id = ph.Id.ToString(),
                                PhoneNumber = ph.Number,
                                IsInternal = ph.IsInternal,
                            };
                            Dispatcher.Invoke(() =>
                            {
                                localPhones.Add(lph);
                            });

                        });

                        if (token.IsCancellationRequested)
                            token.ThrowIfCancellationRequested();

                    }

                }
                catch
                {

                }

            }, token);

            bool isCancellationRequested = token.IsCancellationRequested;

            try
            {
                tokenSource.Dispose();
            }
            catch { }

            try
            {
                if (firstCon)
                {
                    txtAutoEmployee.ItemTemplateSelector = new CustomTemplateSelector();
                    txtAutoEmployee.Provider = new EmployeeDbSearchProvider();

                    txtAutoPhone.ItemTemplateSelector = new CustomTemplateSelector();
                    txtAutoPhone.Provider = new PhoneDbSearchProvider();

                    (txtAutoEmployee.Provider as EmployeeDbSearchProvider).IsEnabeled = chkAutoSuggestionMode.IsChecked.HasValue ? chkAutoSuggestionMode.IsChecked.Value : true;
                    (txtAutoPhone.Provider as PhoneDbSearchProvider).IsEnabeled = chkAutoSuggestionMode.IsChecked.HasValue ? chkAutoSuggestionMode.IsChecked.Value : true;
                }

            }
            catch
            {

            }

            btnCancelAndClose.Visibility = Visibility.Collapsed;
            btnClose.IsEnabled = true;
            btnOk.IsEnabled = true;

            if (isCancellationRequested)
            {
                btnClose_Click(null, null);
            }

        }


        class LocalEmployee
        {
            [ColumnName("شناسه")]
            public string Id { get; set; }
            [ColumnName("نام")]
            public string Name { get; set; }
        }

        class LocalPhone
        {

            [ColumnName("شناسه")]
            public string Id { get; set; }
            [ColumnName("شماره تلفن")]
            public string PhoneNumber { get; set; }
            [ColumnName("داخلی")]
            public bool IsInternal { get; set; }
        }


        private void dataGrid_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            var desc = e.PropertyDescriptor as PropertyDescriptor;
            var att = desc.Attributes[typeof(ColumnNameAttribute)] as ColumnNameAttribute;
            if (att != null)
            {
                e.Column.Header = att.Name;
                if (att.Name == "شناسه")
                    e.Column.IsReadOnly = true;
            }
        }


        private async void btnInsertEmployeeToRow_Click(object sender, RoutedEventArgs e)
        {
            if (RowId != "InsertMode")
            {
                string txtAutoEmployeeText = txtAutoEmployee.Text;

                Dispatcher.Invoke(() =>
                {

                    toolBarTrayProgress.Visibility = Visibility.Visible;
                    dataGridPhone.IsEnabled = false;
                    dataGridEmployee.IsEnabled = false;
                    btnInsertEmployeeToRow.IsEnabled = false;
                    btnInsertPhoneToRow.IsEnabled = false;
                });

                await Task.Run(() =>
                {
                    try
                    {
                        bool succ = false;

                        Employee emp = new Employee()
                        {
                            Name = txtAutoEmployeeText,
                        };

                        PhonebookContext db = new PhonebookContext(ConnectionStringBuilder.ConnectionString);

                        var row = db.Rows.SingleOrDefault(r => r.Id.ToString() == RowId);

                        if (row != null)
                        {
                            var employee = db.Employees.FirstOrDefault(r => r.Name == emp.Name);

                            if (employee == null)
                            {
                                Employee addedEmp = null;
                                using (var dbContextTransaction = db.Database.BeginTransaction())
                                {
                                    try
                                    {
                                        addedEmp = db.Employees.Add(emp);
                                        row.Employees.Add(addedEmp);
                                        db.SaveChanges();
                                        dbContextTransaction.Commit();
                                        succ = true;
                                    }
                                    catch
                                    {
                                        dbContextTransaction.Rollback();
                                    }
                                }

                                if (addedEmp != null && succ)
                                {
                                    Dispatcher.Invoke(() =>
                                    {
                                        localEmployees.Add(new LocalEmployee
                                        {
                                            Id = addedEmp.Id.ToString(),
                                            Name = addedEmp.Name,
                                        });
                                    });
                                    //(sender as DataGrid).Items.Refresh();
                                }
                            }
                            else
                            {
                                row.Employees.Add(employee);
                                int x = db.SaveChanges();
                                if (x > 0)
                                {
                                    localEmployees.Add(new LocalEmployee
                                    {
                                        Id = employee.Id.ToString(),
                                        Name = employee.Name,
                                    });
                                }
                                else
                                {
                                    throw new Exception();
                                }

                            }
                        }
                        else
                        {
                            throw new Exception();
                        }
                    }
                    catch { }
                });


                Dispatcher.Invoke(() =>
                {

                    toolBarTrayProgress.Visibility = Visibility.Collapsed;
                    dataGridPhone.IsEnabled = true;
                    dataGridEmployee.IsEnabled = true;
                    btnInsertEmployeeToRow.IsEnabled = true;
                    btnInsertPhoneToRow.IsEnabled = true;
                });

            }
            else
            {
                LocalEmployee emp = new LocalEmployee
                {
                    Name = txtAutoEmployee.Text,
                };
                localEmployees.Add(emp);
            }
        }

        private async void btnInsertPhoneToRow_Click(object sender, RoutedEventArgs e)
        {

            if (RowId != "InsertMode")
            {

                string txtAutoPhoneText = txtAutoPhone.Text;
                bool isInternal = chkIsInternalPhoneNumber.IsChecked.HasValue ? chkIsInternalPhoneNumber.IsChecked.Value : false;
                Dispatcher.Invoke(() =>
                {

                    toolBarTrayProgress.Visibility = Visibility.Visible;
                    dataGridPhone.IsEnabled = false;
                    dataGridEmployee.IsEnabled = false;
                    btnInsertEmployeeToRow.IsEnabled = false;
                    btnInsertPhoneToRow.IsEnabled = false;
                });

                await Task.Run(() =>
                {
                    try
                    {
                        bool succ = false;
                        PhoneNumber ph = new PhoneNumber()
                        {
                            Number = txtAutoPhoneText,
                            IsInternal = isInternal,
                        };

                        PhonebookContext db = new PhonebookContext(ConnectionStringBuilder.ConnectionString);

                        var row = db.Rows.SingleOrDefault(r => r.Id.ToString() == RowId);

                        if (row != null)
                        {
                            var phone = db.PhoneNumbers.FirstOrDefault(r => r.Number == ph.Number && r.IsInternal == ph.IsInternal);

                            if (phone == null)
                            {
                                PhoneNumber addedPh = null;
                                using (var dbContextTransaction = db.Database.BeginTransaction())
                                {
                                    try
                                    {
                                        addedPh = db.PhoneNumbers.Add(ph);
                                        row.PhoneNumbers.Add(addedPh);
                                        db.SaveChanges();
                                        dbContextTransaction.Commit();
                                        succ = true;
                                    }
                                    catch
                                    {
                                        dbContextTransaction.Rollback();
                                    }
                                }

                                if (addedPh != null && succ)
                                {
                                    Dispatcher.Invoke(() =>
                                    {
                                        localPhones.Add(new LocalPhone
                                        {
                                            Id = addedPh.Id.ToString(),
                                            PhoneNumber = addedPh.Number,
                                            IsInternal = addedPh.IsInternal
                                        });
                                    });


                                }


                            }
                            else
                            {
                                row.PhoneNumbers.Add(phone);
                                int x = db.SaveChanges();
                                if (x > 0)
                                {
                                    localPhones.Add(new LocalPhone
                                    {
                                        Id = phone.Id.ToString(),
                                        PhoneNumber = phone.Number,
                                        IsInternal = phone.IsInternal,
                                    });
                                }
                                else
                                {
                                    throw new Exception();
                                }

                            }
                        }
                        else
                        {
                            throw new Exception();
                        }
                    }
                    catch { }
                });

                Dispatcher.Invoke(() =>
                {

                    toolBarTrayProgress.Visibility = Visibility.Collapsed;
                    dataGridPhone.IsEnabled = true;
                    dataGridEmployee.IsEnabled = true;
                    btnInsertEmployeeToRow.IsEnabled = true;
                    btnInsertPhoneToRow.IsEnabled = true;
                });
            }
            else
            {
                LocalPhone phone = new LocalPhone
                {
                    PhoneNumber = txtAutoPhone.Text,
                    IsInternal = chkIsInternalPhoneNumber.IsChecked.HasValue ? chkIsInternalPhoneNumber.IsChecked.Value : false,
                };
                localPhones.Add(phone);
            }

        }


        private async void chkAutoSuggestionMode_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                (txtAutoEmployee.Provider as EmployeeDbSearchProvider).IsEnabeled = true;
                (txtAutoPhone.Provider as PhoneDbSearchProvider).IsEnabeled = true;
                await Task.Run(() =>
                {
                    try
                    {
                        Properties.Settings.Default.ChkAutoSeggestion = true;
                        Properties.Settings.Default.Save();
                    }
                    catch
                    {

                    }
                });
            }
            catch { }

        }

        private async void chkAutoSuggestionMode_Unchecked(object sender, RoutedEventArgs e)
        {
            try
            {
                (txtAutoEmployee.Provider as EmployeeDbSearchProvider).IsEnabeled = false;
                (txtAutoPhone.Provider as PhoneDbSearchProvider).IsEnabeled = false;
                await Task.Run(() =>
                {
                    try
                    {
                        Properties.Settings.Default.ChkAutoSeggestion = false;
                        Properties.Settings.Default.Save();
                    }
                    catch
                    {

                    }
                });
            }
            catch
            {

            }
        }

        private async void btnOk_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Dispatcher.Invoke(() =>
                {
                    //toolBarTray.IsEnabled = false;
                    toolBarTrayProgress.Visibility = Visibility.Visible;
                    dataGridPhone.IsEnabled = false;
                    dataGridEmployee.IsEnabled = false;
                    btnInsertEmployeeToRow.IsEnabled = false;
                    btnInsertPhoneToRow.IsEnabled = false;
                });

                await Task.Run(() =>
                {
                    try
                    {
                        if (RowId != "InsertMode")
                        {

                        }
                        else//InsertMode
                        {
                            using (PhonebookContext db = new PhonebookContext(ConnectionStringBuilder.ConnectionString))
                            {
                                using (var dbContextTransaction = db.Database.BeginTransaction())
                                {
                                    try
                                    {

                                        var selectedEmployees = new List<Employee>();
                                        var selectedPhones = new List<PhoneNumber>();

                                        List<string> empNames = new List<string>();

                                        Dispatcher.Invoke(() =>
                                        {
                                            localEmployees.ToList().ForEach(em => empNames.Add(em.Name));
                                        });


                                        empNames.Distinct().ToList().ForEach(name =>
                                        {
                                            Employee emp = emp = db.Employees.FirstOrDefault(e1 => e1.Name == name);


                                            if (emp == null)
                                            {
                                                selectedEmployees.Add(db.Employees.Add(new Employee
                                                {
                                                    Name = name,
                                                }));

                                            }
                                            else
                                            {
                                                selectedEmployees.Add(emp);
                                            }
                                        });


                                        List<string> phNumbers = new List<string>();
                                        List<bool> phIsInternals = new List<bool>();

                                        Dispatcher.Invoke(() =>
                                        {
                                            localPhones.ToList().ForEach(ph =>
                                            {
                                                phNumbers.Add(ph.PhoneNumber);
                                                phIsInternals.Add(ph.IsInternal);
                                            });
                                        });

                                        phNumbers = phNumbers.Distinct().ToList();
                                        phIsInternals = phIsInternals.Distinct().ToList();

                                        for (int i = 0; i < phNumbers.Count; i++)
                                        {
                                            string num = phNumbers[i];
                                            bool isIn = phIsInternals[i];
                                            PhoneNumber pho = db.PhoneNumbers.FirstOrDefault(e1 => e1.Number == num);
                                            if (pho == null)
                                            {
                                                selectedPhones.Add(db.PhoneNumbers.Add(new PhoneNumber
                                                {
                                                    Number = num,
                                                    IsInternal = isIn,
                                                }));
                                            }
                                            else
                                            {

                                                selectedPhones.Add(pho);

                                            }
                                        }




                                        var cat = db.Categories.SingleOrDefault(r => r.Id.ToString() == CategoryId);

                                        var newRow = db.Rows.Add(new Row
                                        {
                                            CategoryId = cat.Id,
                                            Category = cat,
                                        });

                                        selectedEmployees.ForEach(em => newRow.Employees.Add(em));
                                        selectedPhones.ForEach(ph => newRow.PhoneNumbers.Add(ph));


                                        db.SaveChanges();

                                        dbContextTransaction.Commit();
                                    }
                                    catch
                                    {
                                        dbContextTransaction.Rollback();
                                    }
                                }
                            }
                        }
                    }
                    catch { }
                });

                Dispatcher.Invoke(() =>
                {

                    toolBarTrayProgress.Visibility = Visibility.Collapsed;
                    dataGridPhone.IsEnabled = true;
                    dataGridEmployee.IsEnabled = true;
                    btnInsertEmployeeToRow.IsEnabled = true;
                    btnInsertPhoneToRow.IsEnabled = true;
                });

            }
            catch { }
            finally
            {
                OnClosing(this, CustomDialogResult.OK);
                CustomDialogResult = CustomDialogResult.OK;

            }
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (RowId != "InsertMode")
                {

                }
                else//InsertMode
                {

                }
            }
            catch { }
            finally
            {
                OnClosing(this, CustomDialogResult.CLOSE);
                CustomDialogResult = CustomDialogResult.CLOSE;

            }
        }

        private async void chkRemoveUnused_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                await Task.Run(() =>
                {
                    try
                    {
                        Properties.Settings.Default.ChkRemoveFromOriginTable = true;
                        Properties.Settings.Default.Save();
                    }
                    catch
                    {

                    }
                });
            }
            catch { }
        }

        private async void chkRemoveUnused_Unchecked(object sender, RoutedEventArgs e)
        {
            try
            {
                await Task.Run(() =>
                {
                    try
                    {
                        Properties.Settings.Default.ChkRemoveFromOriginTable = false;
                        Properties.Settings.Default.Save();
                    }
                    catch
                    {

                    }
                });
            }
            catch { }
        }

        private void btnCancelAndClose_Click(object sender, RoutedEventArgs e)
        {
            btnCancelAndClose.IsEnabled = false;
            try
            {
                if (tokenSource != null)
                    tokenSource.Cancel();
            }
            catch
            {

            }
            finally
            {
                try
                {
                    tokenSource.Dispose();
                }
                catch { }
            }
            btnCancelAndClose.IsEnabled = true;

        }
    }
}
