using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using MaterialDesignThemes.Wpf;
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
using System.Drawing;
using MaterialDesignColors;
using System.Data.SqlClient;
using Nit.Phonebook.Logics;

namespace Nit.Phonebook
{
    /// <summary>
    /// Interaction logic for UcStartup.xaml
    /// </summary>
    public partial class UcStartup : UserControl
    {


        public event EventHandler ConnectionCreated;

        public event EventHandler ClosingWindow;

        protected virtual void OnClosingWindow(object sender, EventArgs e)
        {
            ClosingWindow?.Invoke(sender, e);
        }


        CancellationTokenSource tokenSource = null;

        protected virtual void OnConnectionCreated(object sender, EventArgs e)
        {
            ConnectionCreated?.Invoke(sender, e);
        }
        public UcStartup()
        {
            InitializeComponent();

            try
            {
                if (Properties.Settings.Default.ChkRememberMe)
                {
                    chkRememberPass.IsChecked = true;                    
                    ConnectionStringBuilder.SetConnectionString(Properties.Settings.Default.ServerName, Properties.Settings.Default.Login, Security.ToInsecureString(Security.DecryptString(Properties.Settings.Default.Pass)));                    
                }
            }
            catch
            {

            }

            txtServerName.Text = ConnectionStringBuilder.ServerName;
            txtLogin.Text = ConnectionStringBuilder.UserName;
            txtPassword.Password = ConnectionStringBuilder.Password;

        }
        bool succ = false;

        private async void btnConnect_Click(object sender, RoutedEventArgs e)
        {
            succ = false;
            btnConnect.IsEnabled = false;
            btnClose.IsEnabled = false;


            tokenSource = new CancellationTokenSource();
            var token = tokenSource.Token;

            string serverName = txtServerName.Text;
            string login = txtLogin.Text;
            string password = txtPassword.Password;

            bool isChecked = chkRememberPass.IsChecked == true;

            try
            {
                ConnectionStringBuilder.SetConnectionString(txtServerName.Text, txtLogin.Text, txtPassword.Password);

                await Task.Run(new Action(() =>
                {
                    if (token.IsCancellationRequested)
                        token.ThrowIfCancellationRequested();

                    PhonebookContext db = new PhonebookContext(ConnectionStringBuilder.ConnectionString);

                    // Create database. 
                    // If failed to connect to the database server, it will throw an exception.
                    db.Database.CreateIfNotExists();

                    if (token.IsCancellationRequested)
                        token.ThrowIfCancellationRequested();

                    succ = true;


                    OnConnectionCreated(this, null);

                }), token);
            }
            catch
            {

            }

            try
            {
                tokenSource.Dispose();
            }
            catch { }

            btnConnect.IsEnabled = true;
            btnCancel.IsEnabled = true;
            btnClose.IsEnabled = true;

            if (succ == false)
                MessageBox.Show("خطا");
            else
            {
                try
                {
                    if (isChecked)
                    {
                        string pass = password;
                        Properties.Settings.Default.ServerName = serverName;
                        Properties.Settings.Default.Login = login;
                        await Task.Run(() => {

                            Properties.Settings.Default.Pass = Security.EncryptString(Security.ToSecureString(pass));
                            Properties.Settings.Default.Save();
                        });
                    }
                }
                catch
                {

                }
            }

        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            btnCancel.IsEnabled = false;
            btnClose.IsEnabled = false;

            try
            {
                if (tokenSource != null)
                    tokenSource.Cancel();
                else
                    btnCancel.IsEnabled = true;
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

            btnClose.IsEnabled = true;

        }

        private void Image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start("https://github.com/tataiee1375/NitPhonebook");
            }
            catch { }
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            OnClosingWindow(this, null);
        }

        private async void chkRememberPass_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                Properties.Settings.Default.ChkRememberMe = true;
                await Task.Run(() => {
                    Properties.Settings.Default.Save();
                });                
            }
            catch
            {

            }
        }

        private async void chkRememberPass_Unchecked(object sender, RoutedEventArgs e)
        {
            try
            {
                Properties.Settings.Default.ChkRememberMe = false;
                await Task.Run(() => {
                    Properties.Settings.Default.Save();
                });
            }
            catch
            {

            }
        }
    }
}
