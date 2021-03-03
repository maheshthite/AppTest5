using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using SQLite;

namespace App5.db
{
    public static class DBConstants
    {
        public const string DatabaseFilename = "mylistsV2.db3";

        public const SQLite.SQLiteOpenFlags Flags =
            // open the database in read/write mode
            SQLite.SQLiteOpenFlags.ReadWrite |
            // create the database if it doesn't exist
            SQLite.SQLiteOpenFlags.Create |
            // enable multi-threaded database access
            SQLite.SQLiteOpenFlags.SharedCache;

        public static string DatabasePath
        {
            get
            {
                //var basePath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                //return Path.Combine(basePath, DatabaseFilename);
                //string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal); // Documents folder
                //return Path.Combine(documentsPath, "..", "Library"); // Library folder instead
                var basePath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                return Path.Combine(basePath, DatabaseFilename);
            }
        }
    }

    class ListEngine : INotifyPropertyChanged
    {
        public ObservableCollection<Topic> topiccollection { get; set; }
        public ObservableCollection<Item> itemscollection { get; set; }

        public ListEngine()
        {
            topiccollection = new ObservableCollection<Topic>();
            itemscollection = new ObservableCollection<Item>();
        }
        public int Init()
        {
            var db = new SQLiteConnection(DBConstants.DatabasePath);
            return 1;
        }
        //public static bool TableExists(String tableName, SQLiteConnection connection)
        //{
        //    using (SQLiteCommand cmd = new SQLiteCommand())
        //    {
        //        cmd.CommandType = CommandType.Text;
        //        cmd.Connection = connection;
        //        cmd.CommandText = "SELECT * FROM sqlite_master WHERE type = 'table' AND name = @name";
        //        cmd.Parameters.AddWithValue("@name", tableName);

        //        using (SqliteDataReader sqlDataReader = cmd.ExecuteReader())
        //        {
        //            if (sqlDataReader.Read())
        //                return true;
        //            else
        //                return false;
        //        }
        //    }
        //}
        public void DoSomeDataAccess()
        {
            var db = new SQLiteConnection(DBConstants.DatabasePath);
            //db.DropTable<Topic>();
            var info = db.GetTableInfo("Topic");
            if (!info.Any())
            {

                //var list = db.Query<Topic>("select * from Topic");

                var tableEmpty = db.Table<Topic>();
                //   var cnt = tableEmpty.Count();
                //   if (tableEmpty != null && tableEmpty.Count() > 0)
                //       return;
                //db.DropTable<Topic>();
                db.CreateTable<Topic>();
                db.CreateTable<Item>();
                if (db.Table<Topic>().Count() == 0)
                {
                    // only insert the data if it doesn't already exist
                    var newTopic = new Topic(TopicType.List, "Shopping");
                    db.Insert(newTopic);
                    var newTopic1 = new Topic(TopicType.List, "ToDo");
                    db.Insert(newTopic1);
                    var newTopic2 = new Topic(TopicType.List, "FavMovies");
                    db.Insert(newTopic2);
                    var newTopic3 = new Topic(TopicType.List, "FavBooks");
                    db.Insert(newTopic3);

                    var newTopic4 = new Topic(TopicType.Quotes, "This Week");
                    db.Insert(newTopic4);
                    var newTopic5 = new Topic(TopicType.Quotes, "2021");
                    db.Insert(newTopic5);
                    var newTopic6 = new Topic(TopicType.Thoughts, "01/15/2021");
                    db.Insert(newTopic6);
                    var newTopic7 = new Topic(TopicType.Thoughts, "02/20/2021");
                    db.Insert(newTopic7);
                }
                Console.WriteLine("Reading data");
                var table = db.Table<Topic>();
                foreach (var s in table)
                {
                    Console.WriteLine(s.TopicType + " " + s.TopicName);
                }
            }
        }
        public int GetTopicList(string sTopicType)
        {
            topiccollection.Clear();
            itemscollection.Clear();
            var db = new SQLiteConnection(DBConstants.DatabasePath);
            Console.WriteLine("Reading data");
            //var table = db.Table<Topic>();
            var table = db.Query<Topic>("SELECT * FROM Topic WHERE TopicType = ?", sTopicType);
            foreach (var s in table)
            {
                Console.WriteLine(s.TopicType + " " + s.TopicName);
                topiccollection.Add(s);
            }
            return 1;
        }
        public Topic GetTopic(string TopicType, string sTopicName)
        {
            return topiccollection.Where(i => i.TopicName == sTopicName).FirstOrDefault();
        }

        public int SaveTopic(Topic newTopic)
        {
            var db = new SQLiteConnection(DBConstants.DatabasePath);
            var tableEmpty = db.Table<Topic>();
            db.Insert(newTopic);
            topiccollection.Add(newTopic);
            return 1;
        }
        public int UpdateTopic(Topic newTopic)
        {
            var db = new SQLiteConnection(DBConstants.DatabasePath);
            var tableEmpty = db.Table<Topic>();
            db.Update(newTopic);
            //topiccollection.Add(newTopic);
            return 1;
        }
        public int DeleteTopic(Topic selectedTopic)
        {
            var db = new SQLiteConnection(DBConstants.DatabasePath);
            db.Table<Item>().Delete(x => x.TopicId == selectedTopic.TopicId);
            itemscollection.Clear();
            var tableEmpty = db.Table<Topic>();
            db.Delete(selectedTopic);
            topiccollection.Remove(topiccollection.Where(i => i.TopicId == selectedTopic.TopicId).Single());
            return 1;
        }
        public int GetItemsList(Topic selTopic)
        {
            itemscollection.Clear();
            var db = new SQLiteConnection(DBConstants.DatabasePath);
            Console.WriteLine("Reading data");
            //var table = db.Table<Topic>();
            var table = db.Query<Item>("SELECT * FROM Item WHERE TopicId = ?", selTopic.TopicId);
            foreach (var s in table)
            {
                //  Console.WriteLine(s.TopicType + " " + s.TopicName);
                itemscollection.Add(s);
            }
            return 1;
        }
        public int SaveItem(Item newItem)
        {
            var db = new SQLiteConnection(DBConstants.DatabasePath);
            var tableEmpty = db.Table<Item>();
            db.Insert(newItem);
            itemscollection.Add(newItem);
            return 1;
        }
        public int UpdateItem(Item newItem)
        {
            var db = new SQLiteConnection(DBConstants.DatabasePath);
            var tableEmpty = db.Table<Item>();
            db.Update(newItem);
            //itemscollection.Add(newItem);
            return 1;
        }
        public int DeleteItem(Item selectedItem)
        {
            var db = new SQLiteConnection(DBConstants.DatabasePath);
            var tableEmpty = db.Table<Item>();
            db.Delete(selectedItem);
            itemscollection.Remove(itemscollection.Where(i => i.ItemId == selectedItem.ItemId).Single());
            return 1;
        }
        public int SaveThoughtItem(Item newItem)
        {
            var db = new SQLiteConnection(DBConstants.DatabasePath);
            db.Table<Item>().Delete(x => x.TopicId == newItem.TopicId);
            itemscollection.Clear();

            var tableEmpty = db.Table<Item>();
            db.Insert(newItem);
            itemscollection.Add(newItem);
            return 1;
        }

        #region INotifyPropertyChanged Members

        protected virtual void OnPropertyChanged(string propertyName)
        {
            OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
        }

        protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (null != this.PropertyChanged)
            {
                PropertyChanged(this, e);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion
    }
}
