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

namespace Nit.Phonebook
{
    /// <summary>
    /// Interaction logic for UcCategoryBar.xaml
    /// </summary>
    public partial class UcCategoryBar : UserControl
    {

        public event EventHandler<string> TreeCategoryOnItemSelected;//categoryId

        protected virtual void OnTreeCategoryOnItemSelected(object sender, Category e)
        {
            TreeCategoryOnItemSelected?.Invoke(sender, e.Id.ToString());
        }

        public DbRealTimeManager managerInsertOrDelete;

        public UcCategoryBar()
        {
            InitializeComponent();
        }

        public void RunAfterCreated()
        {
            try
            {
                Task.Run(() =>
                {
                    try
                    {
                        ReFillTreeCategoryVisual();

                        PhonebookContext db = new PhonebookContext(ConnectionStringBuilder.ConnectionString);

                        var li = db.ChangeTracingkInformations.Single(r => r.TableName == nameof(Category) && r.Action == "Insert");//last inserted
                        var ld = db.ChangeTracingkInformations.Single(r => r.TableName == nameof(Category) && r.Action == "Delete");//last deleted
                        var lu = db.ChangeTracingkInformations.Single(r => r.TableName == nameof(Category) && r.Action == "Update");//last updated

                        bool chkSync1 = false;

                        Dispatcher.InvokeAsync(() =>
                        {
                            chkSync1 = chkSync.IsChecked.HasValue ? !chkSync.IsChecked.Value : true;
                        });


                        managerInsertOrDelete = new DbRealTimeManager()
                        {
                            ActionInsert = true,
                            ActionDelete = true,
                            ActionUpdate = true,
                            TabelName = nameof(Category),
                            Interval = 100,
                            CurrentStop = chkSync1,
                        };
                        managerInsertOrDelete.Inserted += ManagerInsertOrDeleteOrUpdated_InsertedOrDeleted;
                        managerInsertOrDelete.Deleted += ManagerInsertOrDeleteOrUpdated_InsertedOrDeleted;
                        managerInsertOrDelete.Updated += ManagerInsertOrDelete_Updated; ;



                        managerInsertOrDelete.LastTimeInsert = li.LastTime;


                        managerInsertOrDelete.LastTimeDelete = ld.LastTime;


                        managerInsertOrDelete.LastTimeUpdate = lu.LastTime;

                        Thread thread = new Thread(managerInsertOrDelete.Run) { IsBackground = true };
                        thread.Start(managerInsertOrDelete);
                    }
                    catch
                    {

                    }
                });
            }
            catch { }
        }

        private void IterateTreeCategory()
        {
            ItemCollection itemCollection = treeCategory.Items;
            foreach (TreeItem node in itemCollection)
            {

                IterateTreeCategoryHelper(node);
            }
        }

        private void IterateTreeCategoryHelper(TreeItem treeItem)
        {
            var collection = treeItem.Items;
            foreach (TreeItem node in collection)
            {

                IterateTreeCategoryHelper(node);
            }
        }


        private TreeItem FindInTreeCategory(string id)
        {
            ItemCollection itemCollection = treeCategory.Items;
            foreach (TreeItem node in itemCollection)
            {
                if (node.Category.Id.ToString() == id)
                {
                    return node;
                }

                TreeItem item = FindInTreeCategoryHelper(node, id);
                if (item != null)
                {
                    return item;
                }

            }

            return null;
        }

        private TreeItem FindInTreeCategoryHelper(TreeItem treeItem, string id)
        {
            var collection = treeItem.Items;
            foreach (TreeItem node in collection)
            {
                if (node.Category.Id.ToString() == id)
                {
                    return node;
                }

                TreeItem item = FindInTreeCategoryHelper(node, id);
                if (item != null)
                {
                    return item;
                }
            }
            return null;
        }


        private bool RemoveRecursively(string id)
        {
            bool succ = false;
            using (PhonebookContext db = new PhonebookContext(ConnectionStringBuilder.ConnectionString))
            {
                using (var dbContextTransaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        RemoveRecursivelyHelper(db, db.Categories.ToList(), db.Categories.Single(c => c.Id.ToString() == id));
                        int x = db.SaveChanges();
                        if (x > 0)
                        {
                            succ = true;
                            dbContextTransaction.Commit();
                        }
                        else
                            throw new Exception();

                    }
                    catch
                    {
                        dbContextTransaction.Rollback();
                    }
                }
            }
            return succ;
        }
        private void RemoveRecursivelyHelper(PhonebookContext db, List<Category> Categories, Category item)
        {
            var children = Categories.Where(c => c.ParentId == item.Id).ToList();

            if (children.Count > 0)
            {
                foreach (var child in children)
                {
                    RemoveRecursivelyHelper(db, Categories, child);
                }
            }

            var rows = db.Rows.Where(r => r.CategoryId == item.Id).ToList();
            rows.ForEach(r =>
            {
                var employees = r.Employees.ToList();
                var phones = r.PhoneNumbers.ToList();

                employees.ForEach(emp =>
                {
                    emp.Rows.Remove(r);
                });
                phones.ForEach(ph =>
                {
                    ph.Rows.Remove(r);
                });
                db.Rows.Remove(r);
            });

            db.Categories.Remove(item);

        }


        private async Task<TreeItem> ReFillTreeCategoryVisual()
        {
            TreeItem root = new TreeItem();
            Dispatcher.Invoke(() =>
            {
                progressBar.Visibility = Visibility.Visible;
            });
            await Task.Run(() =>
            {
                PhonebookContext db = new PhonebookContext(ConnectionStringBuilder.ConnectionString);
                root.Category = db.Categories.Single(c => c.Id == 1);
                FillTree(db.Categories.ToList(), root);
                Dispatcher.Invoke(() =>
                {

                    treeCategory.Items.Clear();
                    treeCategory.Items.Add(root);
                });
            });
            Dispatcher.Invoke(() =>
            {
                progressBar.Visibility = Visibility.Collapsed;
            });
            return root;
        }

        private void ManagerInsertOrDelete_Updated(object sender, string e)
        {
            try
            {
                DbRealTimeManager mng = (DbRealTimeManager)sender;
                Dispatcher.Invoke(() =>
                {
                    ReFillTreeCategoryVisual();
                });
            }
            catch { }
        }

        private void ManagerInsertOrDeleteOrUpdated_InsertedOrDeleted(object sender, string e)
        {
            try
            {
                DbRealTimeManager mng = (DbRealTimeManager)sender;

                if (e == "Insert")
                {
                    Dispatcher.Invoke(() =>
                    {
                        ReFillTreeCategoryVisual();


                    });
                }
                else if (e == "Delete")
                {
                    Dispatcher.Invoke(() =>
                    {
                        ReFillTreeCategoryVisual();


                    });
                }
                else
                    throw new Exception();

            }
            catch { }
        }

        private void FillTree(List<Category> Categories, TreeItem Item)
        {
            var children = Categories.Where(c => c.ParentId == Item.Category.Id);
            foreach (var child in children)
            {
                TreeItem menuItem = new TreeItem();
                menuItem.Category = child;
                Item.Items.Add(menuItem);
                FillTree(Categories, menuItem);
            }
        }

        public class TreeItem
        {
            public TreeItem()
            {
                this.Items = new ObservableCollection<TreeItem>();
            }

            public Category Category { get; set; }

            public ObservableCollection<TreeItem> Items { get; set; }
        }

        private async void treeCategory_SelectedAsync(object sender, RoutedEventArgs e)
        {
            await Task.Run(() =>
            {
                Dispatcher.Invoke(() =>
                {
                    try
                    {
                        TreeViewItem item = e.OriginalSource as TreeViewItem;

                        TreeItem selected = (TreeItem)item.DataContext;

                        txtTitle.Text = selected.Category.Title;
                        txtId.Text = selected.Category.Id.ToString();
                        txtParentId.Text = selected.Category.ParentId.ToString();

                        if (chkAlterMode.IsChecked.HasValue && chkAlterMode.IsChecked.Value == true)
                        {
                            popupAlter.IsPopupOpen = true;
                            txtTitle.Focus();
                            txtTitle.SelectAll();
                        }

                        if (chkAlterMode.IsChecked.HasValue == false || (chkAlterMode.IsChecked.HasValue && chkAlterMode.IsChecked.Value == false))
                        {
                            OnTreeCategoryOnItemSelected(this, selected.Category);
                        }
                    }
                    catch { }
                });
            });

        }

        private async void btnInsert_ClickAsync(object sender, RoutedEventArgs e)
        {
            treeCategory.IsEnabled = false;
            popupAlter.IsEnabled = false;
            try
            {
                bool saveSate = managerInsertOrDelete.CurrentStop;

                managerInsertOrDelete.CurrentStop = true;

                await Task.Run(async () =>
                {
                    Category category = null;
                    Dispatcher.Invoke(() =>
                    {
                        category = new Category()
                        {
                            Title = txtTitle.Text,
                            ParentId = int.Parse(txtId.Text),
                        };
                    });

                    PhonebookContext db = new PhonebookContext(ConnectionStringBuilder.ConnectionString);
                    category = db.Categories.Add(category);
                    int x = db.SaveChanges();

                    if (x > 0)
                    {
                        var lastItemInserted = db.ChangeTracingkInformations.Single(r => r.Action == "Insert" && r.TableName == nameof(Category));

                        await Dispatcher.InvokeAsync(() =>
                        {
                            TreeItem item = FindInTreeCategory(txtId.Text);
                            TreeItem newItem = new TreeItem()
                            {
                                Category = category,
                            };
                            item.Items.Add(newItem);
                        });

                        managerInsertOrDelete.LastTimeInsert = lastItemInserted.LastTime;
                        managerInsertOrDelete.LastIdInserted = lastItemInserted.LastId;

                        Dispatcher.Invoke(() =>
                        {
                            txtBlockMsg.Text = "";
                            popupAlter.IsPopupOpen = false;
                        });
                    }
                    else
                        throw new Exception("خطا");
                });

                managerInsertOrDelete.CurrentStop = saveSate;
            }
            catch
            {
                Dispatcher.Invoke(() =>
                {
                    txtBlockMsg.Text = "خطا";
                });

            }
            popupAlter.IsEnabled = true;
            treeCategory.IsEnabled = true;
        }

        private async void btnUpdate_ClickAsync(object sender, RoutedEventArgs e)
        {
            treeCategory.IsEnabled = false;
            popupAlter.IsEnabled = false;

            bool saveSate = managerInsertOrDelete.CurrentStop;
            managerInsertOrDelete.CurrentStop = true;


            try
            {
                string txtIdText = txtId.Text;
                string txtTitleText = txtTitle.Text;

                //دفترچه تلفن را نباید بتوان تغییر داد
                if (txtIdText == "1")
                    throw new Exception();

                await Task.Run(async () =>
                {
                    try
                    {
                        PhonebookContext db = new PhonebookContext(ConnectionStringBuilder.ConnectionString);
                        var cat = db.Categories.SingleOrDefault(r => r.Id.ToString() == txtIdText);
                        if (cat != null)
                        {
                            cat.Title = txtTitleText;
                            int x = db.SaveChanges();

                            var lastItemUpdated = db.ChangeTracingkInformations.Single(r => r.Action == "Update" && r.TableName == nameof(Category));

                            await Dispatcher.InvokeAsync(() =>
                            {
                                TreeItem item = FindInTreeCategory(txtId.Text);
                                item.Category.Title = cat.Title;
                                txtBlockMsg.Text = "";
                                popupAlter.IsPopupOpen = false;
                            });

                            managerInsertOrDelete.LastTimeUpdate = lastItemUpdated.LastTime;
                        }
                        else
                            throw new Exception();
                    }
                    catch
                    {
                        Dispatcher.Invoke(() =>
                        {
                            txtBlockMsg.Text = "خطا";
                        });
                    }
                    finally
                    {

                    }
                });
            }
            catch
            {

            }
            finally
            {
                managerInsertOrDelete.CurrentStop = saveSate;
            }



            popupAlter.IsEnabled = true;
            treeCategory.IsEnabled = true;
        }

        private async void btnDelete_ClickAsync(object sender, RoutedEventArgs e)
        {

            var result = MessageBox.Show(
                       "آیا می خواهید دسته بندی (های) انتخاب شده حذف شوند؟",
                       "حذف",
                       MessageBoxButton.YesNo,
                       MessageBoxImage.Question,
                       MessageBoxResult.No);
            if (result == MessageBoxResult.Yes)
            {
                treeCategory.IsEnabled = false;
                popupAlter.IsEnabled = false;

                bool saveSate = managerInsertOrDelete.CurrentStop;

                managerInsertOrDelete.CurrentStop = true;
                string txtIdText = txtId.Text;


                try
                {

                    //دفترچه تلفن را نباید بتوان حذف کرد
                    if (txtIdText == "1")
                        throw new Exception();

                    await Task.Run(async () =>
                    {
                        try
                        {
                            bool suc = RemoveRecursively(txtIdText);
                            if (suc)
                            {
                                using (PhonebookContext db = new PhonebookContext(ConnectionStringBuilder.ConnectionString))
                                {
                                    var lastItemDeleted = db.ChangeTracingkInformations.Single(r => r.Action == "Delete" && r.TableName == nameof(Category));

                                    await Dispatcher.InvokeAsync(() =>
                                       {
                                           TreeItem item = FindInTreeCategory(txtParentId.Text);
                                           item.Items.Remove(item.Items.Single(r => r.Category.Id.ToString() == txtId.Text));
                                           txtBlockMsg.Text = "";
                                           popupAlter.IsPopupOpen = false;
                                       });

                                    managerInsertOrDelete.LastTimeDelete = lastItemDeleted.LastTime;
                                    managerInsertOrDelete.LastIdDeleted = lastItemDeleted.LastId;
                                }
                            }
                            else
                            {
                                throw new Exception();
                            }
                        }
                        catch
                        {
                            Dispatcher.Invoke(() =>
                            {
                                txtBlockMsg.Text = "خطا";
                            });
                        }

                    });
                }
                catch
                {

                }
                finally
                {
                    managerInsertOrDelete.CurrentStop = saveSate;
                }

                popupAlter.IsEnabled = true;
                treeCategory.IsEnabled = true;
            }

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
                        ReFillTreeCategoryVisual();
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

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (!popupAlter.IsPopupOpen)
                popupAlter.IsPopupOpen = true;
        }

        private void chkSync_Checked(object sender, RoutedEventArgs e)
        {
            managerInsertOrDelete.CurrentStop = false;
        }

        private void chkSync_Unchecked(object sender, RoutedEventArgs e)
        {
            managerInsertOrDelete.CurrentStop = true;
        }

        private void treeCategory_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete && treeCategory.SelectedItem != null)
            {
                btnDelete_ClickAsync(null, null);
            }
        }
    }
}
