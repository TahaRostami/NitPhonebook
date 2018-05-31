using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Nit.Phonebook.Logics
{
    public class CustomTemplateSelector : System.Windows.Controls.DataTemplateSelector
    {
        public System.Windows.DataTemplate FileTemplate { get; set; }
        public System.Windows.DataTemplate DirectoryTemplate { get; set; }

        public override System.Windows.DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            return base.SelectTemplate(item, container);
        }
    }
}
