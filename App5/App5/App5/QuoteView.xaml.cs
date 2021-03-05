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

    public partial class QuoteView : ContentView
    {
        ListEngine m_ListEngine;
        MainPage m_ParentMainPage;
        public bool bSpeakFlag = false;
        public bool bDeleteFlag = false;
        public bool bDeleteItemFlag = false;
        public bool bReOrderFlag = false;
        public string m_CurrentTopicType;
        public Topic m_CurrentTopic;
        public Item m_EditItem;
        public Topic m_EditTopic;
        public QuoteView()
        {
            InitializeComponent();
            m_CurrentTopicType = TopicType.Quotes;
            //BackgroundImageSource = ImageSource.FromResource("App5.Images.bkg1.png");
            hbartop.Source = ImageSource.FromResource("App5.Images.hbar.png");
            m_ListEngine = new ListEngine();
            //dgItemsLists.QueryRowHeight += DataGrid_QueryRowHeight;

        }
        //private void DataGrid_QueryRowHeight(object sender, Syncfusion.SfDataGrid.XForms.QueryRowHeightEventArgs e)
        //{
        //    if (e.RowIndex > 0)
        //    {
        //        e.Height = SfDataGridHelpers.GetRowHeight(dgItemsLists, e.RowIndex);
        //        e.Handled = true;
        //    }
        //}
        public void SetParentMainPage(MainPage mParent)
        {
            m_ParentMainPage = mParent;
        }
        public void PageAppearing()
        {
            m_ListEngine.GetTopicList(m_CurrentTopicType);
            dgTopicsLists.ItemsSource = m_ListEngine.topiccollection;
            dgTopicsLists.SelectionChanged += DataGrid_SelectionChanged;
            dgTopicsLists.CurrentCellActivating += DataGrid_CurrentCellActivating;
            dgTopicsLists.CurrentCellBeginEdit += TopicDataGrid_CurrentCellBeginEdit;
            dgTopicsLists.CurrentCellEndEdit += TopicDataGrid_CurrentCellEndEdit;
            dgTopicsLists.AllowEditing = true;

            dgItemsLists.ItemsSource = m_ListEngine.itemscollection;
            dgItemsLists.SelectionChanged += ItemDataGrid_SelectionChanged;
            dgItemsLists.CurrentCellActivating += ItemDataGrid_CurrentCellActivating;
            dgItemsLists.CurrentCellBeginEdit += ItemDataGrid_CurrentCellBeginEdit;
            dgItemsLists.CurrentCellEndEdit += ItemDataGrid_CurrentCellEndEdit;
            dgItemsLists.AllowEditing = true;

        }
        private async void DataGrid_SelectionChanged(object sender, GridSelectionChangedEventArgs e)
        {
            try
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
            catch (Exception exception)
            {
                //LogMsg.Log(exception.Message);
                await App.Current.MainPage.DisplayAlert("DataGrid_SelectionChanged", exception.Message, "ok");
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
            try
            {
                if (e.AddedItems.Count == 0)
                return;
                // Gets the selected item 
                var selectedItem = (Item)e.AddedItems[0];
                txtEssay.Text = selectedItem.ItemText;
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
            catch (Exception exception)
            {
                //LogMsg.Log(exception.Message);
                await App.Current.MainPage.DisplayAlert("DataGrid_SelectionChanged", exception.Message, "ok");
            }
        }
        void ItemDataGrid_CurrentCellActivating(object sender, CurrentCellActivatingEventArgs e)
        {
            var selectedItem = e.CurrentRowColumnIndex;
            if (selectedItem.ColumnIndex == 1)
                bDeleteItemFlag = true;
            else
                bDeleteItemFlag = false;
        }
        private void TopicDataGrid_CurrentCellBeginEdit(object sender, GridCurrentCellBeginEditEventArgs e)
        {
            m_EditTopic = (Topic)dgTopicsLists.SelectedItem;
        }
        private async void TopicDataGrid_CurrentCellEndEdit(object sender, GridCurrentCellEndEditEventArgs e)
        {
            try
            {
                // await App.Current.MainPage.DisplayAlert("Inside", "ItemDataGrid_CurrentCellEndEdit", "ok");
                var recordIndex = dgTopicsLists.ResolveToRecordIndex(e.RowColumnIndex.RowIndex);
                var columnIndex = dgTopicsLists.ResolveToGridVisibleColumnIndex(e.RowColumnIndex.ColumnIndex);
                var mappingName = dgTopicsLists.Columns[columnIndex].MappingName;
                //await App.Current.MainPage.DisplayAlert(mappingName, "mappingName", "ok");
                Topic selItem = (Topic)dgTopicsLists.GetRecordAtRowIndex(recordIndex + 1);

                if (mappingName == "TopicName" && selItem != null)
                {
                    //Topic selItem = (Topic)m_EditTopic;
                    //if (selItem != null)
                    {
                        selItem.TopicName = (string)e.NewValue;
                        //await App.Current.MainPage.DisplayAlert("TopicName", selItem.TopicName, "ok");
                        m_ListEngine.UpdateTopic(selItem);
                    }
                }

            }
            catch (Exception exception)
            {
                //LogMsg.Log(exception.Message);
                await App.Current.MainPage.DisplayAlert("TopicDataGrid_CurrentCellEndEdit", exception.Message, "ok");
            }
        }

        private void ItemDataGrid_CurrentCellBeginEdit(object sender, GridCurrentCellBeginEditEventArgs e)
        {
            m_EditItem = (Item)dgItemsLists.SelectedItem;
        }
        private async void ItemDataGrid_CurrentCellEndEdit(object sender, GridCurrentCellEndEditEventArgs e)
        {
            try
            {
                // await App.Current.MainPage.DisplayAlert("Inside", "ItemDataGrid_CurrentCellEndEdit", "ok");
                var recordIndex = dgItemsLists.ResolveToRecordIndex(e.RowColumnIndex.RowIndex);
                var columnIndex = dgItemsLists.ResolveToGridVisibleColumnIndex(e.RowColumnIndex.ColumnIndex);
                var mappingName = dgItemsLists.Columns[columnIndex].MappingName;
                //await App.Current.MainPage.DisplayAlert(mappingName, "mappingName", "ok");
                Item selItem = (Item)dgItemsLists.GetRecordAtRowIndex(recordIndex + 1);
                if (mappingName == "ItemText" && selItem != null)
                {
                    selItem.ItemText = (string)e.NewValue;
                    //Item selItem = (Item)m_EditItem;
                    //if (selItem != null)
                    {
                        //await App.Current.MainPage.DisplayAlert("ItemText", selItem.ItemText, "ok");
                        m_ListEngine.UpdateItem(selItem);
                    }
                }

            }
            catch (Exception exception)
            {
                //LogMsg.Log(exception.Message);
                await App.Current.MainPage.DisplayAlert("ItemDataGrid_CurrentCellEndEdit", exception.Message, "ok");
            }
        }

        //protected  override void OnAppearing()
        //{
        //}
        private void BtnAdd_Clicked(object sender, EventArgs e)
        {
            if (m_CurrentTopic == null)
            {
                string strTopic = "Quotes_"+DateTime.Now.ToString("yyyyMMddhhmmssfff");
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

            Item newItem = new Item(m_CurrentTopic, txtEssay.Text);
            m_ListEngine.SaveItem(newItem);
            txtEssay.Text = "";

            //bSpeakFlag = false;
            //btnSpeak.Text = "Speak";
            //btnSpeak.TextColor = Color.FromHex("#FFFFFF");
            //btnSpeak.BackgroundColor = Color.FromHex("#407DEC");
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
                m_ParentMainPage.StopSpeechToText();
                bSpeakFlag = false;
                BtnAdd_Clicked(sender, e);
                btnSpeak.Text = "Speak";
                btnSpeak.TextColor = Color.FromHex("#FFFFFF");
                btnSpeak.BackgroundColor = Color.FromHex("#407DEC");
            }
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
        public void AddTopicItemSpeechtoText(string strItem)
        {
            if (bSpeakFlag)
                txtEssay.Text = strItem;


            //// if topic not set , create a unique one
            //if (m_CurrentTopic == null)
            //{
            //    string strTopic = DateTime.Now.ToString("Quotes_yyyyMMddhhmmssfff");
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
