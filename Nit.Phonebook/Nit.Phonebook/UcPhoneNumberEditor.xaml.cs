using Nit.Phonebook.Logics;
using Nit.Phonebook.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.SqlClient;
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

namespace Nit.Phonebook
{
    /// <summary>
    /// Interaction logic for UcPhoneNumberEditor.xaml
    /// </summary>
    public partial class UcPhoneNumberEditor : UserControl
    {
        readonly ObservableCollection<GridRow> observableCollection = new ObservableCollection<GridRow>();
        public UcPhoneNumberEditor()
        {
            InitializeComponent();
            dataGrid1.AutoGeneratingColumn += dataGrid1_AutoGeneratingColumn;

            dataGrid1.RowEditEnding += DataGrid1_RowEditEndingAsync;

            dataGrid1.PreviewKeyDown += DataGrid1_PreviewKeyDownAsync;
      
        }

        private async void DataGrid1_RowEditEndingAsync(object sender, DataGridRowEditEndingEventArgs e)
        {
            progressBar.Visibility = Visibility.Visible;
            dataGrid1.IsReadOnly = true;

            GridRow oldItem = null;
            if (this.dataGrid1.SelectedItem != null)
            {
                oldItem = new GridRow();

                oldItem.Id = (e.Row.Item as GridRow).Id;
                oldItem.Number = (e.Row.Item as GridRow).Number;
                oldItem.IsInternal = (e.Row.Item as GridRow).IsInternal;

                (sender as DataGrid).RowEditEnding -= DataGrid1_RowEditEndingAsync;
                (sender as DataGrid).CommitEdit();
                (sender as DataGrid).Items.Refresh();
                (sender as DataGrid).RowEditEnding += DataGrid1_RowEditEndingAsync;
            }
            else
            {
                return;
            }



            try
            {
                var newItem = e.Row.Item as GridRow;


                bool conn1 = string.IsNullOrWhiteSpace(newItem.Id) && newItem.Id == oldItem.Id;
                bool conn2 = ((newItem.Number != oldItem.Number) || (newItem.IsInternal != oldItem.IsInternal)) && newItem.Id == oldItem.Id && !string.IsNullOrWhiteSpace(newItem.Id);
                bool connNested1 = string.IsNullOrEmpty(newItem.Number) && !string.IsNullOrEmpty(oldItem.Number);

                string id = newItem.Id == null ? "" : newItem.Id.ToString();
                string number = newItem.Number;
                bool isInternal = newItem.IsInternal;

                await Task.Run(async () =>
                {
                    try
                    {
                        PhonebookContext db = new PhonebookContext(ConnectionStringBuilder.ConnectionString);
                        if (conn1)//insert
                        {
                            if (connNested1)
                            {
                                return;
                            }
                            PhoneNumber phn = new PhoneNumber()
                            {
                                Number = number,
                                IsInternal = isInternal,
                            };


                            int x = db.Database.ExecuteSqlCommand(System.Data.Entity.TransactionalBehavior.EnsureTransaction,
                                                     SqlCammandHelper.CMD_INSERT_INTO_PHONENUMBER_UNDER_CONSTRAINTS
                                                     , new SqlParameter("@Number", number), new SqlParameter("IsInternal", isInternal)
                            );


                            if (x > 0)
                            {
                                await Dispatcher.InvokeAsync(() =>
                                {
                                    ReFillDataGrid();
                                });
                            }
                            else
                            {
                                throw new Exception();
                            }
                        }
                        //update
                        else if (conn2)
                        {
                            var phn = db.PhoneNumbers.SingleOrDefault(r => r.Id.ToString() == id);
                            if (phn != null)
                            {


                                int x = 0;
                                if (phn.Number == number && phn.IsInternal != isInternal)
                                {
                                    phn.Number = number;
                                    phn.IsInternal = isInternal;
                                    x = db.SaveChanges();
                                }
                                else if (phn.Number != number)
                                {
                                    x = db.Database.ExecuteSqlCommand(System.Data.Entity.TransactionalBehavior.EnsureTransaction,
                                             SqlCammandHelper.CMD_UPDATE_INTO_PHONENUMBER_UNDER_CONSTRAINTS,
                                             new SqlParameter("@Id", id), new SqlParameter("@Number", number), new SqlParameter("IsInternal", isInternal)
                                              );
                                }


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
                    catch
                    {
                        await Dispatcher.InvokeAsync(() =>
                        {

                            if (string.IsNullOrWhiteSpace(oldItem.Id))
                            {
                                newItem.Number = null;
                                newItem.IsInternal = false;
                                observableCollection.Remove(newItem);
                            }
                            else
                            {
                                newItem.Number = oldItem.Number;
                                newItem.IsInternal = oldItem.IsInternal;
                            }
                           (sender as DataGrid).Items.Refresh();
                        });

                    }
                });
            }
            catch
            {

            }



            progressBar.Visibility = Visibility.Collapsed;
            dataGrid1.IsReadOnly = false;

        }

        private async void DataGrid1_PreviewKeyDownAsync(object sender, KeyEventArgs e)
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
                                    PhonebookContext db = new PhonebookContext(ConnectionStringBuilder.ConnectionString);
                                    var items = db.PhoneNumbers.RemoveRange(db.PhoneNumbers.Where(emp => ids.Any(id2 => id2 == emp.Id.ToString())));
                                    int x = db.SaveChanges();
                                    if (x > 0)
                                    {
                                        Dispatcher.Invoke(() =>
                                        {
                                            ReFillDataGrid();
                                        });
                                    }
                                    else
                                        throw new Exception();
                                }
                                catch
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

        private async void ReFillDataGrid()
        {
            progressBar.Visibility = Visibility.Visible;
            try
            {
                await Task.Run(() =>
                {

                    bool mustFilter = false;

                    Dispatcher.Invoke(() =>
                    {
                        if (chkQueryUnusedVar.IsChecked.HasValue && chkQueryUnusedVar.IsChecked.Value)
                        {
                            mustFilter = true;
                        }
                    });

                    PhonebookContext db = new PhonebookContext(ConnectionStringBuilder.ConnectionString);

                    Dispatcher.Invoke(() =>
                    {
                        observableCollection.Clear();
                    });

                    if (mustFilter)
                    {
                        db.PhoneNumbers.Where(r => r.Rows.Count < 1).ToList().ForEach((r) =>
                            {
                                Dispatcher.Invoke(() =>
                                {
                                    observableCollection.Add(new GridRow()
                                    {
                                        Id = r.Id.ToString(),
                                        Number = r.Number,
                                        IsInternal = r.IsInternal
                                    });
                                });

                            });
                    }
                    else
                    {
                        db.PhoneNumbers.ToList().ForEach((r) =>
                        {
                            Dispatcher.Invoke(() =>
                            {
                                observableCollection.Add(new GridRow()
                                {
                                    Id = r.Id.ToString(),
                                    Number = r.Number,
                                    IsInternal = r.IsInternal
                                });
                            });

                        });
                    }

                });
                dataGrid1.ItemsSource = observableCollection;
            }
            catch
            {

            }
            progressBar.Visibility = Visibility.Collapsed;
        }

        private void dataGrid1_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
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

        class GridRow
        {
            [ColumnName("شناسه")]
            public string Id { get; set; }

            [ColumnName("شماره تلفن")]
            public string Number { get; set; }

            [ColumnName("داخلی")]
            public bool IsInternal { get; set; }
        }

        private async void btnExecute_ClickAsync(object sender, RoutedEventArgs e)
        {
            await Task.Run(() =>
            {
                try
                {
                    Dispatcher.Invoke(() =>
                    {
                        btnExecute.IsEnabled = false;
                    });

                    Dispatcher.Invoke(() =>
                    {
                        ReFillDataGrid();
                    });


                }
                catch (Exception ex)
                {

                }
                finally
                {
                    Dispatcher.Invoke(() =>
                    {
                        btnExecute.IsEnabled = true;
                    });
                }

            });


        }


        private async void chkQueryUnusedVar_Checked(object sender, RoutedEventArgs e)
        {
            await Task.Run(() =>
            {
                try
                {
                    Dispatcher.Invoke(() =>
                    {
                        btnExecute.IsEnabled = false;
                    });

                    Dispatcher.Invoke(() =>
                    {
                        ReFillDataGrid();
                    });


                }
                catch (Exception ex)
                {

                }
                finally
                {
                    Dispatcher.Invoke(() =>
                    {
                        btnExecute.IsEnabled = true;
                    });
                }

            });
        }

        private async void chkQueryUnusedVar_Unchecked(object sender, RoutedEventArgs e)
        {
            await Task.Run(() =>
            {
                try
                {
                    Dispatcher.Invoke(() =>
                    {
                        btnExecute.IsEnabled = false;
                    });

                    Dispatcher.Invoke(() =>
                    {
                        ReFillDataGrid();
                    });


                }
                catch (Exception ex)
                {

                }
                finally
                {
                    Dispatcher.Invoke(() =>
                    {
                        btnExecute.IsEnabled = true;
                    });
                }

            });
        }

    }
}
