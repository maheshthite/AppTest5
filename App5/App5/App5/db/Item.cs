using SQLite;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Text;
using Xamarin.Forms;
namespace App5.db
{
    public class Item : INotifyPropertyChanged
    {
        string _TopicId;
        [DataMember]
        public string TopicId
        {
            get { return _TopicId; }
            set
            {
                if (value != _TopicId)
                {
                    _TopicId = value;
                    OnPropertyChanged("TopicId");
                }
            }
        }
        string _TopicName;
        [DataMember]
        public string TopicName
        {
            get { return _TopicName; }
            set
            {
                if (value != _TopicName)
                {
                    _TopicName = value;
                    OnPropertyChanged("TopicName");
                }
            }
        }
        string _ItemId;
        [PrimaryKey]
        [DataMember]
        public string ItemId
        {
            get { return _ItemId; }
            set
            {
                if (value != _ItemId)
                {
                    _ItemId = value;
                    OnPropertyChanged("ItemId");
                }
            }
        }
        string _ItemText;
        [DataMember]
        public string ItemText
        {
            get { return _ItemText; }
            set
            {
                if (value != _ItemText)
                {
                    _ItemText = value;
                    OnPropertyChanged("_ItemText");
                }
            }
        }
        bool _Status = false;
        [DataMember]
        public bool Status
        {
            get { return _Status; }
            set
            {
                if (value != _Status)
                {
                    _Status = value;
                    OnPropertyChanged("Status");
                }
            }
        }
        DateTime _updatetime;
        [DataMember]
        public DateTime UpdateTime
        {
            get { return _updatetime; }
            set
            {
                if (value != _updatetime)
                {
                    _updatetime = value;
                    OnPropertyChanged("UpdateTime");
                }
            }
        }

        ImageSource _trashimg = ImageSource.FromResource("App5.Images.TrashCan.png");
        public ImageSource TrashImg
        {
            get { return _trashimg; }
        }
        ImageSource _reorderimg = ImageSource.FromResource("App5.Images.next2.png");
        public ImageSource ReOrderImg
        {
            get { return _reorderimg; }
        }

        public Item()
        {

        }
        public Item(Topic sTopic,string sItemText)
        {
            TopicId = sTopic.TopicId;
            TopicName = sTopic.TopicName;
            ItemId = GetUniqueId();
            ItemText = sItemText;
            Status = false;
            UpdateTime = DateTime.Now;
        }
        public Item(Item ord)
        {
            TopicId = ord.TopicId;
            TopicName = ord.TopicName;
            ItemId = ord.ItemId;
            ItemText = ord.ItemText;
            Status = ord.Status;
            UpdateTime = ord.UpdateTime;
        }
        public Item Clone()
        {
            Item neword = new Item(this);
            neword.ItemId = GetUniqueId();
            return neword;
        }
        public static string GetUniqueId()
        {
            return DateTime.Now.ToString("A1_yyyyMMddhhmmssfff");
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
