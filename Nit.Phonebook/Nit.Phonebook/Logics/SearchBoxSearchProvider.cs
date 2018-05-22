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
using System;
using System.Linq;

namespace Nit.Phonebook.Logics
{
    public class SearchBoxSearchProvider : WpfControls.Editors.ISuggestionProvider
    {
        public bool IsEnabeled { get; set; } = true;
        public enum TypeOfSeacrh
        {
            EMPLOYEE,
            PHONE,
        }
        public TypeOfSeacrh SearchType { get; set; } = TypeOfSeacrh.EMPLOYEE;

        public System.Collections.IEnumerable GetSuggestions(string filter)
        {
            try
            {
                if (!IsEnabeled) throw new Exception();

                PhonebookContext db = new PhonebookContext(ConnectionStringBuilder.ConnectionString);
                if (SearchType == TypeOfSeacrh.EMPLOYEE)
                {
                    return db.Employees.Where(r => r.Name.Contains(filter)).Select(s => s.Name).ToList();
                }
                else if (SearchType == TypeOfSeacrh.PHONE)
                {
                    return db.PhoneNumbers.Where(r => r.Number.Contains(filter)).Select(s => s.Number).ToList();
                }
                else
                    return null;
            }
            catch
            {
                return null;
            }
        }
    }
}
