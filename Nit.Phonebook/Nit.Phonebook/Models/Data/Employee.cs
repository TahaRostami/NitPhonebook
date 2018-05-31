namespace Nit.Phonebook.Models
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using Nit.Phonebook.Models.Data;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;

    [Table("Employee")]
    public partial class Employee : INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Employee()
        {
            Rows = new ObservableCollection<Row>();
        }

        private int id;
        public int Id
        {
            get => id; set
            {
                id = value;
                OnPropertyChanged();
            }
        }

        private string name;
        [Required]
        public string Name
        {
            get => name; set
            {
                name = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<Row> rows;

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ObservableCollection<Row> Rows
        {
            get => rows;
            set
            {
                rows = value;
                OnPropertyChanged();
            }
        }

    }
}
