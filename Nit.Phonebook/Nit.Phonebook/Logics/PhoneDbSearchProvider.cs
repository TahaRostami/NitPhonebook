using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfControls.Editors;
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

namespace Nit.Phonebook.Logics
{
    class PhoneDbSearchProvider : ISuggestionProvider
    {
        public bool IsEnabeled { get; set; }
        public System.Collections.IEnumerable GetSuggestions(string filter)
        {
            try
            {
                if (!IsEnabeled) throw new Exception();
                PhonebookContext db = new PhonebookContext(ConnectionStringBuilder.ConnectionString);
                return db.PhoneNumbers.Where(r => r.Number.Contains(filter)).Select(s => s.Number).ToList();
            }
            catch
            {
                return null;
            }
        }
    }
}
