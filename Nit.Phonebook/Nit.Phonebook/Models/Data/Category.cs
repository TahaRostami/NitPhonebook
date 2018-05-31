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

    [Table("Category")]
    public partial class Category : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Category()
        {
            Rows = new ObservableCollection<Row>();
        }

        private int id;
        public int Id
        {
            get => id;
            set
            {
                id = value;
                OnPropertyChanged();
            }
        }

        private int parentId;

        public int ParentId
        {
            get => parentId;

            set {
                parentId = value;
                OnPropertyChanged();
            }
        }

        private string title { get; set; }

        public string Title { get => title;
            set
            {
                title = value;
                OnPropertyChanged();
            }
        }


        private ObservableCollection<Row> rows;

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ObservableCollection<Row> Rows {
            get => rows;
            set
            {
                rows = value;
                OnPropertyChanged();
            }
        }

    }
}
