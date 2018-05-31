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
using Nit.Phonebook.Logics;
using MaterialDesignThemes.Wpf;

namespace Nit.Phonebook
{
    /// <summary>
    /// Interaction logic for PageMainTableInfoEditor.xaml
    /// </summary>
    public partial class PageMainTableInfoEditor : Page
    {

        string lastSelectedCategoryId = string.Empty;

        UcEmployeeEditor ucEmployeeEditor = null;
        UcPhoneNumberEditor ucPhoneEditor = null;
        UcSearchBox ucSearchBox = null;


        public PageMainTableInfoEditor()
        {
            InitializeComponent();
            ucCat.TreeCategoryOnItemSelected += UcCategoryBar_TreeCategoryOnItemSelectedAsync;
            ucRowEditor.RequestEditorPageToShow += UcRowEditor_RequestEditorPageToShow;

            GridBackgroundPage.Visibility = Visibility.Visible;
            ucLogin.ConnectionCreated += UcLogin_ConnectionCreated;
            ucLogin.ClosingWindow += UcLogin_ClosingWindow;
            ucThemeSettings.ClosingWindow += UcThemeSettings_ClosingWindow;

            try
            {
                new PaletteHelper().ReplacePrimaryColor(Properties.Settings.Default.Primary);
                new PaletteHelper().SetLightDark(Properties.Settings.Default.IsDark);
                new PaletteHelper().ReplaceAccentColor(Properties.Settings.Default.Accent);
            }
            catch
            {

            }

            try
            {
                ucThemeSettings.txtPreNumber.Text = Properties.Settings.Default.PreNumber;
            }
            catch
            {

            }

            try
            {
                ucWindowRowEditor.chkRemoveUnused.IsChecked = Properties.Settings.Default.ChkRemoveFromOriginTable;
                ucWindowRowEditor.chkAutoSuggestionMode.IsChecked = Properties.Settings.Default.ChkAutoSeggestion;
            }
            catch
            {

            }

            try
            {
                if (Properties.Settings.Default.ServerName == "" || Properties.Settings.Default.Login == "")
                {
                    ucLogin.txtServerName.Text = Environment.MachineName;
                    ucLogin.txtLogin.Text = "NitGuest";
                    GridBackgroundPage.Visibility = Visibility.Visible;
                    ucLogin.Visibility = Visibility.Visible;
                }
                else
                {
                    UcLogin_ConnectionCreated(null, null);
                }

            }
            catch { }


            btnSearchBoxLeftMenu_Click(this, null);


        }

        private void UcThemeSettings_ClosingWindow(object sender, EventArgs e)
        {
            GridBackgroundPage.Visibility = Visibility.Collapsed;
            ucThemeSettings.Visibility = Visibility.Collapsed;
        }

        private void UcLogin_ClosingWindow(object sender, EventArgs e)
        {
            GridBackgroundPage.Visibility = Visibility.Collapsed;
            ucLogin.Visibility = Visibility.Collapsed;
        }

        private async void UcLogin_ConnectionCreated(object sender, EventArgs e)
        {
            await Task.Run(async () =>
            {
                await Dispatcher.InvokeAsync(() =>
                {
                    this.RunAfterCreated();
                    GridBackgroundPage.Visibility = Visibility.Collapsed;
                    ucLogin.Visibility = Visibility.Collapsed;
                });

            });
        }


        private void UcRowEditor_RequestEditorPageToShow(object sender, Tuple<string, string> e)
        {
            try
            {
                GridBackgroundPage.Visibility = Visibility.Visible;
                ucWindowRowEditor.Visibility = Visibility.Visible;

                ucWindowRowEditor.RunAfterConstructorToBind(e.Item1, e.Item2);

            }
            catch
            {

            }
        }

        public void RunAfterCreated()
        {
            ucCat.RunAfterCreated();
        }

        private async void UcCategoryBar_TreeCategoryOnItemSelectedAsync(object sender, string e)
        {
            lastSelectedCategoryId = e;

            await Task.Run(async () =>
            {
                await Dispatcher.InvokeAsync(() =>
                {
                    ucRowEditor.CategoryId = e;
                });
            });
        }

        private async void btnEmployeeLeftMenu_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                await Task.Run(() =>
                {
                    Dispatcher.Invoke(() =>
                    {
                        if (ucEmployeeEditor == null)
                        {
                            ucEmployeeEditor = new UcEmployeeEditor() { Visibility = Visibility.Collapsed };
                            ucEmployeeEditor.Width = 350;
                            ucEmployeeEditor.Margin = new Thickness(2, 0, 2, 0);
                            DockPanel.SetDock(ucEmployeeEditor, Dock.Left);
                            dockPanel1.Children.Insert(dockPanel1.Children.IndexOf(wrapPanel1) + 1, ucEmployeeEditor);
                        }
                    });
                });

                if (ucEmployeeEditor.Visibility == Visibility.Collapsed)
                {
                    ucEmployeeEditor.Visibility = Visibility.Visible;
                    if (ucPhoneEditor != null)
                        ucPhoneEditor.Visibility = Visibility.Collapsed;
                    if (ucSearchBox != null)
                        ucSearchBox.Visibility = Visibility.Collapsed;
                }
                else
                {
                    ucEmployeeEditor.Visibility = Visibility.Collapsed;
                }
            }
            catch
            {

            }

        }

        private async void btnPhoneLeftMenu_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                await Task.Run(() =>
                {
                    Dispatcher.Invoke(() =>
                    {
                        if (ucPhoneEditor == null)
                        {
                            ucPhoneEditor = new UcPhoneNumberEditor() { Visibility = Visibility.Collapsed };
                            ucPhoneEditor.Width = 350;
                            ucPhoneEditor.Margin = new Thickness(2, 0, 2, 0);
                            DockPanel.SetDock(ucPhoneEditor, Dock.Left);
                            dockPanel1.Children.Insert(dockPanel1.Children.IndexOf(wrapPanel1) + 1, ucPhoneEditor);
                        }
                    });
                });

                if (ucPhoneEditor.Visibility == Visibility.Collapsed)
                {
                    if (ucEmployeeEditor != null)
                        ucEmployeeEditor.Visibility = Visibility.Collapsed;
                    if (ucSearchBox != null)
                        ucSearchBox.Visibility = Visibility.Collapsed;

                    ucPhoneEditor.Visibility = Visibility.Visible;

                }
                else
                {
                    ucPhoneEditor.Visibility = Visibility.Collapsed;
                }
            }
            catch
            {

            }
        }

        private async void btnReportLeftMenu_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(lastSelectedCategoryId))
            {
                bool en = btnReportLeftMenu.IsEnabled;
                try
                {
                    btnReportLeftMenu.IsEnabled = false;
                    await Task.Run(() =>
                    {
                        ReportGenerator report = new ReportGenerator(int.Parse(lastSelectedCategoryId));
                    });

                }
                catch
                {

                }
                finally
                {
                    btnReportLeftMenu.IsEnabled = en;
                }
            }
        }

        private async void btnSearchBoxLeftMenu_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                await Task.Run(() =>
                {
                    Dispatcher.Invoke(() =>
                    {
                        if (ucSearchBox == null)
                        {
                            ucSearchBox = new UcSearchBox() { Visibility = Visibility.Collapsed };
                            ucSearchBox.Width = 350;
                            ucSearchBox.Margin = new Thickness(2, 0, 2, 0);
                            DockPanel.SetDock(ucSearchBox, Dock.Left);
                            dockPanel1.Children.Insert(dockPanel1.Children.IndexOf(wrapPanel1) + 1, ucSearchBox);
                        }
                    });
                });

                if (ucSearchBox.Visibility == Visibility.Collapsed)
                {
                    if (ucEmployeeEditor != null)
                        ucEmployeeEditor.Visibility = Visibility.Collapsed;
                    if (ucPhoneEditor != null)
                        ucPhoneEditor.Visibility = Visibility.Collapsed;

                    ucSearchBox.Visibility = Visibility.Visible;

                }
                else
                {
                    ucSearchBox.Visibility = Visibility.Collapsed;
                }
            }
            catch
            {

            }
        }


        private void ucWindowRowEditor_UcClosing(object sender, CustomDialogResult e)
        {
            try
            {
                if (e == CustomDialogResult.OK)
                {
                    ucWindowRowEditor.Visibility = Visibility.Collapsed;
                    GridBackgroundPage.Visibility = Visibility.Collapsed;

                    ucRowEditor.ReFillDataGrid();
                }
                else
                {
                    ucWindowRowEditor.Visibility = Visibility.Collapsed;
                    GridBackgroundPage.Visibility = Visibility.Collapsed;
                }
            }
            catch
            {
                ucWindowRowEditor.Visibility = Visibility.Collapsed;
                GridBackgroundPage.Visibility = Visibility.Collapsed;
            }
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            GridBackgroundPage.Visibility = Visibility.Visible;
            ucLogin.Visibility = Visibility.Visible;
        }

        private void btnTheme_Click(object sender, RoutedEventArgs e)
        {
            GridBackgroundPage.Visibility = Visibility.Visible;
            ucThemeSettings.Visibility = Visibility.Visible;
        }

    }
}
