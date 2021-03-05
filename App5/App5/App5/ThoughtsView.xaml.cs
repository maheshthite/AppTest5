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

    public partial class ThoughtsView : ContentView
    {
        ListEngine m_ListEngine;
        MainPage m_ParentMainPage;
        public bool bSpeakFlag = false;
        public bool bDeleteFlag = false;
        public bool bReOrderFlag = false;
        public string m_CurrentTopicType;
        public Topic m_CurrentTopic;
        public bool bflagtextchanged = false;
        public Topic m_EditTopic;
        public ThoughtsView()
        {
            InitializeComponent();
            m_CurrentTopicType = TopicType.Thoughts;
            //BackgroundImageSource = ImageSource.FromResource("App5.Images.bkg1.png");
            hbartop.Source = ImageSource.FromResource("App5.Images.hbar.png");
            m_ListEngine = new ListEngine();


            //dgItemsLists.ItemsSource = m_ListEngine.itemscollection;

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

                if (mappingName == "TopicName" && m_EditTopic != null)
                {
                    Topic selItem = (Topic)m_EditTopic;
                    if (selItem != null)
                    {
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

        public void SetParentMainPage(MainPage mParent)
        {
            m_ParentMainPage = mParent;
        }
        private async void DataGrid_SelectionChanged(object sender, GridSelectionChangedEventArgs e)
        {

            if(bflagtextchanged)
            {
                if (m_CurrentTopic.TopicName == null|| m_CurrentTopic.TopicName == "")
                {
                    var ans = await App.Current.MainPage.DisplayAlert(m_CurrentTopic.TopicName, "If you like Save edited text please name the topic ", "Yes", "No");
                    if (ans == true)
                    {
                        return;
                    }
                }
                else
                {
                    if(m_CurrentTopic.TopicName == txtTopic.Text)  // update exising
                    {
                        Item newItem = new Item(m_CurrentTopic, txtEssay.Text);
                        m_ListEngine.SaveThoughtItem(newItem);
                    }
                    else // create new
                    {
                        Topic newtopic = new Topic(m_CurrentTopicType, txtTopic.Text);
                        m_ListEngine.SaveTopic(newtopic);
                        Item newItem = new Item(newtopic, txtEssay.Text);
                        m_ListEngine.SaveThoughtItem(newItem);
                    }

                }
                bflagtextchanged = false;
            }
            if (e.AddedItems.Count == 0)
                return;
            // Gets the selected item 
            var selectedItem = (Topic)e.AddedItems[0];
            if (bDeleteFlag)
            {
                var ans = await App.Current.MainPage.DisplayAlert(selectedItem.TopicName, "Would you like Delete", "Yes", "No");
                if (ans == true)
                {
                    txtEssay.Text = "";
                    m_ListEngine.DeleteTopic(selectedItem);
                    bDeleteFlag = false;
                }
            }
            else
            {
                m_CurrentTopic = selectedItem;
                m_ListEngine.GetItemsList(m_CurrentTopic);
                var item = m_ListEngine.itemscollection.FirstOrDefault();
                if (item != null)
                {
                    txtTopic.Text = m_CurrentTopic.TopicName;
                    txtEssay.Text = item.ItemText;
                }
                else
                {
                    txtTopic.Text = m_CurrentTopic.TopicName;
                    txtEssay.Text = "";
                }
                bflagtextchanged = false;
            }
        }
        void DataGrid_CurrentCellActivating(object sender, CurrentCellActivatingEventArgs e)
        {
            var selectedItem = e.CurrentRowColumnIndex;
            if (selectedItem.ColumnIndex == 1)
                bDeleteFlag = true;
        }

        public async void PageDisappearing()
        {
            if (bflagtextchanged)
            {
                if (m_CurrentTopic.TopicName == null || m_CurrentTopic.TopicName == "")
                {
                    var ans = await App.Current.MainPage.DisplayAlert(m_CurrentTopic.TopicName, "If you like Save edited text please name the topic ", "Yes", "No");
                    if (ans == true)
                    {
                        return;
                    }
                }
                else
                {
                    if (m_CurrentTopic.TopicName == txtTopic.Text)  // update exising
                    {
                        Item newItem = new Item(m_CurrentTopic, txtEssay.Text);
                        m_ListEngine.SaveThoughtItem(newItem);
                    }
                    else // create new
                    {
                        Topic newtopic = new Topic(m_CurrentTopicType, txtTopic.Text);
                        m_ListEngine.SaveTopic(newtopic);
                        Item newItem = new Item(newtopic, txtEssay.Text);
                        m_ListEngine.SaveThoughtItem(newItem);
                    }

                }
                bflagtextchanged = false;
            }

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
                txtEssay.Text = txtEssay.Text + "\r" + txtSpeachToText.Text;
                btnSpeak.Text = "Speak";
                btnSpeak.TextColor = Color.FromHex("#FFFFFF");
                btnSpeak.BackgroundColor = Color.FromHex("#407DEC");
                txtSpeachToText.Text = "";
            }
        }
        private async void BtnClearText_Clicked(object sender, EventArgs e)
        {
            if (bflagtextchanged)
            {
                var ans = await App.Current.MainPage.DisplayAlert("Clear Text", "Would you like erase unsaved text", "Yes", "No");
                if (ans == false)
                    return;
            }
            txtTopic.Text = "";
            txtEssay.Text = "";
            bflagtextchanged = false;
        }
        private  void BtnAddTopic_Clicked(object sender, EventArgs e)
        {
            if (m_CurrentTopic == null)
            {
                txtTopic.Text = "Thoughts_" + DateTime.Now.ToString("yyyyMMddhhmmssfff");
            }

            Topic newtopic = m_ListEngine.GetTopic(m_CurrentTopicType, txtTopic.Text);
            if (newtopic == null)
            {
                newtopic = new Topic(m_CurrentTopicType, txtTopic.Text);
                m_ListEngine.SaveTopic(newtopic);
            }
            m_CurrentTopic = newtopic;
            Item newItem = new Item(m_CurrentTopic, txtEssay.Text);
            m_ListEngine.SaveThoughtItem(newItem);
            var rowindex = dgTopicsLists.ResolveToRowIndex(newtopic);
            //Make the row in to available on the view. 
            dgTopicsLists.ScrollToRowIndex(rowindex);
            //to set the found row as current row 
            dgTopicsLists.View.MoveCurrentTo(newtopic);
            dgTopicsLists.SelectedIndex = rowindex;
            bflagtextchanged = false;
        }
        public void AddTopicItemSpeechtoText(string strItem)
        {
            if (bSpeakFlag)
                txtSpeachToText.Text = strItem;

            //// if topic not set , create a unique one
            //if (m_CurrentTopic == null)
            //{
            //    string strTopic = DateTime.Now.ToString("Thoughts_yyyyMMddhhmmssfff");
            //    Topic newtopic = new Topic(m_CurrentTopicType, strTopic);
            //    m_ListEngine.SaveTopic(newtopic);
            //    Item newItem = new Item(m_CurrentTopic, txtEssay.Text);
            //    m_ListEngine.SaveThoughtItem(newItem);
            //    var rowindex = dgTopicsLists.ResolveToRowIndex(newtopic);
            //    //Make the row in to available on the view. 
            //    dgTopicsLists.ScrollToRowIndex(rowindex);
            //    //to set the found row as current row 
            //    dgTopicsLists.View.MoveCurrentTo(newtopic);
            //    dgTopicsLists.SelectedIndex = rowindex;
            //    m_CurrentTopic = newtopic;
            //}
            //else
            //{
            //    Item newItem = new Item(m_CurrentTopic, txtEssay.Text);
            //    m_ListEngine.SaveThoughtItem(newItem);
            //}

        }
        public void EditorTextChanged(object sender, TextChangedEventArgs e)
        {
            bflagtextchanged = true;
        }
        public  void RefreshData()
        {
        }
    }
}
