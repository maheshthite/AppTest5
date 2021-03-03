using SQLite;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Text;
using Xamarin.Forms;

namespace App5.db
{
    public static class TopicType
    {
        public const string List = "List";
        public const string Quotes = "Quotes";
        public const string Thoughts = "Thoughts";
        //public const string Memes = "Memes";
        public static string GetNext(string curr)
        {
            if (curr == List)
                return Quotes;
            if (curr == Quotes)
                return Thoughts;
            else
                return List;
        }
        public static string GetPrev(string curr)
        {
            if (curr == List)
                return Thoughts;
            if (curr == Thoughts)
                return Quotes;
            else
                return List;
        }
    }

    public class Topic : INotifyPropertyChanged
    {
        string _TopicId;
        [PrimaryKey]
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
        string _TopicType;
        [DataMember]
        public string TopicType
        {
            get { return _TopicType; }
            set
            {
                if (value != _TopicType)
                {
                    _TopicType = value;
                    OnPropertyChanged("TopicType");
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
        public Topic()
        {

        }
        public Topic(string strtype,string strname)
        {
            TopicId = GetUniqueId();
            TopicType = strtype;
            TopicName = strname;
            UpdateTime = DateTime.Now;
        }
        public Topic(Topic ord)
        {
            TopicId = ord.TopicId;
            TopicType = ord.TopicType;
            TopicName = ord.TopicName;
            UpdateTime = ord.UpdateTime;
        }
        public Topic Clone()
        {
            Topic neword = new Topic(this);
            neword.TopicId = GetUniqueId();
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
