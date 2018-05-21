namespace Nit.Phonebook.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using System.Collections.ObjectModel;
    using Nit.Phonebook.Models.Data;
    using System.ComponentModel;

    public partial class PhonebookContext : DbContext
    {
        public PhonebookContext(string connStr)
          : base(connStr)
        {
        }



        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<ChangeTracingkInformation> ChangeTracingkInformations { get; set; }
        public virtual DbSet<Employee> Employees { get; set; }
        public virtual DbSet<PhoneNumber> PhoneNumbers { get; set; }
        public virtual DbSet<Row> Rows { get; set; }
        public virtual DbSet<sysdiagram> sysdiagrams { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employee>()
                .HasMany(e => e.Rows)
                .WithMany(e => e.Employees)
                .Map(m => m.ToTable("RelationshipRowEmployee").MapLeftKey("EmployeeId").MapRightKey("RowId"));

            modelBuilder.Entity<PhoneNumber>()
                .HasMany(e => e.Rows)
                .WithMany(e => e.PhoneNumbers)
                .Map(m => m.ToTable("RealtionshipRowPhoneNumber").MapLeftKey("PhoneNumber").MapRightKey("RowId"));
        }
    }

    public class ConnectionStringBuilder
    {
        public static string ServerName { get; private set; } = Environment.MachineName;
        public static string DatabaseName { get; private set; } = "Phonebook";
        public static string UserName { get; private set; } ="NitGuest";
        public static string Password { get; private set; } = "";
        public static int Timeout { get; set; } = 15;
        public static void SetConnectionString(string serverName, string userName, string passWord, int timeout = 15)
        {
            ServerName = serverName;
            UserName = userName;
            Password = passWord;
            Timeout = timeout > 0 ? timeout : 15;
            ConnectionString= $"Data Source={ServerName};Initial Catalog={DatabaseName};Integrated Security=False;User ID={UserName};Password={Password};MultipleActiveResultSets=True;Connection Timeout={Timeout.ToString()};App=EntityFramework";
        }
        public static string ConnectionString { get; private set; } = $"Data Source={ServerName};Initial Catalog={DatabaseName};Integrated Security=False;User ID={UserName};Password={Password};MultipleActiveResultSets=True;Connection Timeout={Timeout.ToString()};App=EntityFramework";
    }
}
