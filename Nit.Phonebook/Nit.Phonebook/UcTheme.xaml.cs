using MaterialDesignColors;
using MaterialDesignThemes.Wpf;
using Nit.Phonebook.Logics;
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

namespace Nit.Phonebook
{
    /// <summary>
    /// Interaction logic for UcTheme.xaml
    /// </summary>
    public partial class UcTheme : UserControl
    {
        public event EventHandler ClosingWindow;

        protected virtual void OnClosingWindow(object sender, EventArgs e)
        {
            ClosingWindow?.Invoke(sender, e);
        }

        public IEnumerable<Swatch> Swatches { get; set; }

        public UcTheme()
        {
            InitializeComponent();
            this.Swatches = new SwatchesProvider().Swatches;
        }

        string primaryColor = "";
        string accentColor = "";

        private void ToggleButton_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                if (chkDarkLight.IsChecked == true)
                {
                    //Dark
                    new PaletteHelper().SetLightDark(true);
                }
                else
                {
                    //Light
                    new PaletteHelper().SetLightDark(false);
                }
            }
            catch { }
        }

        private void PrimaryClick(object sender, RoutedEventArgs e)
        {
            primaryColor = "BlueGrey";
            try
            {
                Button btn = (Button)sender;
                if (btn == btnPrimaryAmber)
                {
                    primaryColor = "Amber";
                }
                else if (btn == btnPrimaryBlueGrey)
                {
                    primaryColor = "BlueGrey";
                }
                else if (btn == btnPrimaryBrown)
                {
                    primaryColor = "Brown";
                }
                else if (btn == btnPrimaryDeepOrange)
                {
                    primaryColor = "DeepOrange";
                }
                else if (btn == btnPrimaryDeepPurpule)
                {
                    primaryColor = "DeepPurple";
                }
                else if (btn == btnPrimaryGrey)
                {
                    primaryColor = "Grey";
                }
                else if (btn == btnPrimaryGrren)
                {
                    primaryColor = "Green";
                }
                else if (btn == btnPrimaryIndigo)
                {
                    primaryColor = "Indigo";
                }
                else if (btn == btnPrimaryRed)
                {
                    primaryColor = "Red";
                }

                new PaletteHelper().ReplacePrimaryColor(primaryColor);
            }
            catch
            {

            }
        }

        private void AccentClick(object sender, RoutedEventArgs e)
        {
            accentColor = "Lime";
            try
            {
                Button btn = (Button)sender;
                if (btn == btnAccentAmber)
                {
                    accentColor = "Amber";
                }
                else if (btn == btnAccentDeepOrange)
                {
                    accentColor = "DeepOrange";
                }
                else if (btn == btnAccentDeepPurpule)
                {
                    accentColor = "DeepPurple";
                }
                else if (btn == btnAccentGrren)
                {
                    accentColor = "Green";
                }
                else if (btn == btnAccentIndigo)
                {
                    accentColor = "Indigo";
                }
                else if (btn == btnAccentRed)
                {
                    accentColor = "Red";
                }

                new PaletteHelper().ReplaceAccentColor(accentColor);
            }
            catch
            {

            }
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            OnClosingWindow(this, null);
        }

        private void btnSetDefault_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                primaryColor = "BlueGrey";
                accentColor = "Lime";
                new PaletteHelper().ReplacePrimaryColor(primaryColor);
                new PaletteHelper().SetLightDark(false);
                new PaletteHelper().ReplaceAccentColor(accentColor);
            }
            catch
            {

            }

        }

        private async void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (primaryColor != "")
                    Properties.Settings.Default.Primary = primaryColor;
                if (accentColor != "")
                    Properties.Settings.Default.Accent = accentColor;

                Properties.Settings.Default.IsDark = chkDarkLight.IsChecked == true ? true : false;

                Properties.Settings.Default.PreNumber = Helper.PreNumber;

                await Task.Run(() =>
                {
                    Properties.Settings.Default.Save();
                });


                OnClosingWindow(this, null);
            }
            catch
            {
                MessageBox.Show("خطا");
            }
        }

        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Properties.Settings.Default.Reset();
                Properties.Settings.Default.Save();                
                MessageBox.Show("عملیات با موفقیت انجام شد");
                txtPreNumber.Text = Properties.Settings.Default.PreNumber;
            }
            catch
            {
                MessageBox.Show("خطا");
            }
        }

        private void txtPreNumber_TextChanged(object sender, TextChangedEventArgs e)
        {
            Helper.PreNumber = txtPreNumber.Text;
        }
    }
}
