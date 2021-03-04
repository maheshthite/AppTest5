using App5.db;
using Syncfusion.SfDataGrid.XForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace App5
{
    //Custom style class

    public partial class ListView: ContentView
    {
        ListEngine m_ListEngine;
        MainPage m_ParentMainPage;
        public bool bSpeakFlag = false;
        public bool bDeleteFlag = false;
        public bool bDeleteItemFlag = false;
        public bool bReOrderFlag = false;
        public string m_CurrentTopicType;
        public Topic m_CurrentTopic;
        public ListView()
        {
            InitializeComponent();
            m_CurrentTopicType = TopicType.List;
            //BackgroundImageSource = ImageSource.FromResource("App5.Images.bkg1.png");
            hbartop.Source = ImageSource.FromResource("App5.Images.hbar.png");
            m_ListEngine = new ListEngine();
            m_ListEngine.DoSomeDataAccess();
            m_ListEngine.GetTopicList(m_CurrentTopicType);
            dgTopicsLists.ItemsSource = m_ListEngine.topiccollection;
            dgTopicsLists.SelectionChanged += DataGrid_SelectionChanged;
            dgTopicsLists.CurrentCellActivating += DataGrid_CurrentCellActivating;

            dgItemsLists.ItemsSource = m_ListEngine.itemscollection;
            dgItemsLists.SelectionChanged += ItemDataGrid_SelectionChanged;
            dgItemsLists.CurrentCellActivating += ItemDataGrid_CurrentCellActivating;
        }
        public void SetParentMainPage(MainPage mParent)
        {
            m_ParentMainPage = mParent;
        }

        private async void DataGrid_SelectionChanged(object sender, GridSelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count == 0)
                return;
            // Gets the selected item 
            var selectedItem = (Topic)e.AddedItems[0];
            if (bDeleteFlag)
            {
                var ans = await App.Current.MainPage.DisplayAlert(selectedItem.TopicName, "Would you like Delete", "Yes", "No");
                if (ans == true)
                {
                    m_ListEngine.DeleteTopic(selectedItem);
                }
                bDeleteFlag = false;
            }
            else
            {
                m_CurrentTopic = selectedItem;
                m_ListEngine.GetItemsList(m_CurrentTopic);

            }
        }
        void DataGrid_CurrentCellActivating(object sender, CurrentCellActivatingEventArgs e)
        {
            var selectedItem = e.CurrentRowColumnIndex;
            if (selectedItem.ColumnIndex == 1)
                bDeleteFlag = true;
            else
                bDeleteFlag = false;
        }
        private async void ItemDataGrid_SelectionChanged(object sender, GridSelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count == 0)
                return;
            // Gets the selected item 
            var selectedItem = (Item)e.AddedItems[0];
            if (bDeleteItemFlag)
            {
                var ans = await App.Current.MainPage.DisplayAlert(selectedItem.ItemText, "Would you like Delete", "Yes", "No");
                if (ans == true)
                {
                    m_ListEngine.DeleteItem(selectedItem);
                }
            }
            bDeleteItemFlag = false;
        }
        void ItemDataGrid_CurrentCellActivating(object sender, CurrentCellActivatingEventArgs e)
        {
            var selectedItem = e.CurrentRowColumnIndex;
            if (selectedItem.ColumnIndex == 2)
                bDeleteItemFlag = true;
            else
                bDeleteItemFlag = false;
        }
        //protected  override void OnAppearing()
        //{
        //}
        private  void BtnAdd_Clicked(object sender, EventArgs e)
        {
            if (m_CurrentTopic == null)
            {
                string strTopic = DateTime.Now.ToString("List_yyyyMMddhhmmssfff");
                Topic newtopic = new Topic(m_CurrentTopicType, strTopic);
                m_ListEngine.SaveTopic(newtopic);
                var rowindex = dgTopicsLists.ResolveToRowIndex(newtopic);
                //Make the row in to available on the view. 
                dgTopicsLists.ScrollToRowIndex(rowindex);
                //to set the found row as current row 
                dgTopicsLists.View.MoveCurrentTo(newtopic);
                dgTopicsLists.SelectedIndex = rowindex;
                m_CurrentTopic = newtopic;
            }
            Item newItem= new Item(m_CurrentTopic, txtItem.Text);
            m_ListEngine.SaveItem(newItem);
            txtItem.Text = "";
        }

        private  void BtnAddTopic_Clicked(object sender, EventArgs e)
        {
            Topic newtopic = new Topic(m_CurrentTopicType, txtTopic.Text);
            m_ListEngine.SaveTopic(newtopic);
            txtTopic.Text = "";
            var rowindex = dgTopicsLists.ResolveToRowIndex(newtopic);
            //Make the row in to available on the view. 
            dgTopicsLists.ScrollToRowIndex(rowindex);
            //to set the found row as current row 
            dgTopicsLists.View.MoveCurrentTo(newtopic);
            dgTopicsLists.SelectedIndex = rowindex;
        }
        private void BtnSpeak_Clicked(object sender, EventArgs e)
        {
            if (!bSpeakFlag)
            {
                btnSpeak.Text = "Stop";
                btnSpeak.TextColor = Color.Red;
                btnSpeak.BackgroundColor = Color.White;
                m_ParentMainPage.Start_Clicked(sender, e);
                bSpeakFlag = true;
            }
            else
            {
                bSpeakFlag = false;
                BtnAdd_Clicked(sender, e);
                btnSpeak.Text = "Speak";
                btnSpeak.TextColor = Color.FromHex("#FFFFFF");
                btnSpeak.BackgroundColor = Color.FromHex("#407DEC");
            }
        }
            
        public void AddTopicItemSpeechtoText(string strItem)
        {
            if(bSpeakFlag)
                txtItem.Text = strItem;


            //// if topic not set , create a unique one
            //if (m_CurrentTopic == null)
            //{
            //    string strTopic = DateTime.Now.ToString("List_yyyyMMddhhmmssfff");
            //    Topic newtopic = new Topic(m_CurrentTopicType, strTopic);
            //    m_ListEngine.SaveTopic(newtopic);
            //    var rowindex = dgTopicsLists.ResolveToRowIndex(newtopic);
            //    //Make the row in to available on the view. 
            //    dgTopicsLists.ScrollToRowIndex(rowindex);
            //    //to set the found row as current row 
            //    dgTopicsLists.View.MoveCurrentTo(newtopic);
            //    dgTopicsLists.SelectedIndex = rowindex;
            //    m_CurrentTopic = newtopic;
            //}
            //Item newItem = new Item(m_CurrentTopic, strItem);
            //m_ListEngine.SaveItem(newItem);

        }
        public  void RefreshData()
        {
        }
    }
}
