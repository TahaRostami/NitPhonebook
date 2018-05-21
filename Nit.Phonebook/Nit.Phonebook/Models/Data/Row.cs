

namespace Nit.Phonebook.Models.Data
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Runtime.CompilerServices;

    [Table("Row")]
    public partial class Row : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Row()
        {
            PhoneNumbers = new ObservableCollection<PhoneNumber>();
            Employees = new ObservableCollection<Employee>();
        }



        private int id;

        public int Id {
            get => id;
            set
            {
                id = value;
                OnPropertyChanged();
            }
        }

        private int categoryId;

        public int CategoryId
        {
            get => categoryId;
            set
            {
                categoryId = value;
                OnPropertyChanged();
            }
        }

        private Category category;

        public virtual Category Category {
            get => category;
            set
            {
                category = value;
                OnPropertyChanged();
            }
        }



        private ObservableCollection<Employee> employees;

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ObservableCollection<Employee> Employees
        {
            get => employees;
            set
            {
                employees = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<PhoneNumber> phoneNumbers;

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ObservableCollection<PhoneNumber> PhoneNumbers
        {
            get => phoneNumbers;
            set
            {
                phoneNumbers = value;
                OnPropertyChanged();
            }
        }

    }
}
