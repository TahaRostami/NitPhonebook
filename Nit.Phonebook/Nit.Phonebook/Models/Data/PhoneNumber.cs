namespace Nit.Phonebook.Models
{
    using Nit.Phonebook.Models.Data;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Runtime.CompilerServices;

    [Table("PhoneNumber")]
    public partial class PhoneNumber : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public PhoneNumber()
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



        private string number;
        [Required]
        public string Number
        {
            get => number; set
            {
                number = value;
                OnPropertyChanged();
            }
        }

        private bool isInternal;

        public bool IsInternal {
            get => isInternal;
            set
            {
                isInternal = value;
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
