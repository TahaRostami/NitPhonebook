using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nit.Phonebook.Models;
using System.Threading;

namespace Nit.Phonebook.Logics
{
    public class DbRealTimeManager
    {

        public event EventHandler<string> Updated;

        protected virtual void OnUpdated(object sender, string e)
        {
            Updated?.Invoke(sender, e);
        }

        public event EventHandler<string> Inserted;

        protected virtual void OnInserted(object sender, string e)
        {
            Inserted?.Invoke(sender, e);
        }

        public event EventHandler<string> Deleted;

        protected virtual void OnDeleted(object sender, string e)
        {
            Deleted?.Invoke(sender, e);
        }

        public string TabelName { get; set; }

        public bool ActionInsert { get; set; } = false;
        public string LastTimeInsert { get; set; } = "*";
        public string LastIdInserted { get; set; } = "*";

        public bool ActionUpdate { get; set; } = false;
        public string LastTimeUpdate { get; set; } = "*";


        public bool ActionDelete { get; set; } = false;
        public string LastTimeDelete { get; set; } = "*";
        public string LastIdDeleted { get; set; } = "*";

        public int Interval { get; set; } = 100;//>0


        public bool CurrentStop { get; set; } = false;

        private bool finished;

        public DbRealTimeManager(bool automaticUpdate = true)
        {
            finished = automaticUpdate ? false : true;
        }


        public void Run(object obj)
        {
            DbRealTimeManager manager = obj as DbRealTimeManager;
            while (true)
            {
                try
                {
                    Thread.Sleep(Interval);

                    if (finished) break;

                    if (CurrentStop) continue;

                    string lastDtInsert = "";
                    string lastDtUpdate = "";
                    string lastDtDelete = "";

                    string lastIdInsert = "";
                    string lastIdDelete = "";

                    if (ActionInsert)
                    {
                        var tmp = new PhonebookContext(ConnectionStringBuilder.ConnectionString).ChangeTracingkInformations.Single(r => r.TableName == manager.TabelName && r.Action == "Insert");
                        lastDtInsert = tmp.LastTime;
                        lastIdInsert = tmp.LastId;

                        if (lastDtInsert != manager.LastTimeInsert)
                        {
                            manager.LastTimeInsert = lastDtInsert;
                            manager.LastIdInserted = lastIdInsert;
                            OnInserted(manager, "Insert");
                        }
                    }
                    if (ActionUpdate)
                    {
                        lastDtUpdate = new PhonebookContext(ConnectionStringBuilder.ConnectionString).ChangeTracingkInformations.Single(r => r.TableName == manager.TabelName && r.Action == "Update").LastTime;
                        if (lastDtUpdate != manager.LastTimeUpdate)
                        {
                            manager.LastTimeUpdate = lastDtUpdate;
                            OnUpdated(manager, "Update");
                        }
                    }
                    if (ActionDelete)
                    {
                        var tmp = new PhonebookContext(ConnectionStringBuilder.ConnectionString).ChangeTracingkInformations.Single(r => r.TableName == manager.TabelName && r.Action == "Delete");
                        lastDtDelete = tmp.LastTime;
                        lastIdDelete = tmp.LastId;

                        if (lastDtDelete != manager.LastTimeDelete)
                        {
                            manager.LastTimeDelete = lastDtDelete;
                            manager.LastIdDeleted = lastIdDelete;
                            OnDeleted(manager, "Delete");
                        }
                    }

                }
                catch
                {

                }
            }
        }
        public void Finish()
        {
            finished = true;
        }

    }
}
