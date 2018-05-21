using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
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
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Nit.Phonebook
{
    public partial class MainWindowApplication : Window
    {

        PageMainTableInfoEditor pageMain = null;

        public MainWindowApplication()
        {
            InitializeComponent();
            
            try
            {
                pageMain = new PageMainTableInfoEditor();
                frame.Content = pageMain;
            }
            catch
            {

            }
        }



        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            App.Current.Shutdown();
            Process.GetCurrentProcess().Kill();                     
        }
    }
}
