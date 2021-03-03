﻿using App5.db;
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
        public bool bDeleteFlag = false;
        public bool bReOrderFlag = false;
        public string m_CurrentTopicType;
        public Topic m_CurrentTopic;
        public bool bflagtextchanged = false;
        public ThoughtsView()
        {
            InitializeComponent();
            m_CurrentTopicType = TopicType.Thoughts;
            //BackgroundImageSource = ImageSource.FromResource("App5.Images.bkg1.png");
            hbartop.Source = ImageSource.FromResource("App5.Images.hbar.png");
            m_ListEngine = new ListEngine();


            //dgItemsLists.ItemsSource = m_ListEngine.itemscollection;

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

        public void PageAppearing()
        {
            m_ListEngine.GetTopicList(m_CurrentTopicType);
            dgTopicsLists.ItemsSource = m_ListEngine.topiccollection;
            dgTopicsLists.SelectionChanged += DataGrid_SelectionChanged;
            dgTopicsLists.CurrentCellActivating += DataGrid_CurrentCellActivating;
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
        public void EditorTextChanged(object sender, TextChangedEventArgs e)
        {
            bflagtextchanged = true;
        }
        public  void RefreshData()
        {
        }
    }
}