﻿using Syncfusion.SfDataGrid.XForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using App5.db;

namespace App5
{
    public class Dark : DataGridStyle
    {
        public Dark()
        {
        }

        public override Color GetHeaderBackgroundColor()
        {
            return Color.FromRgb(80, 80, 80);
        }

        public override Color GetHeaderForegroundColor()
        {
            return Color.FromRgb(255, 255, 255);
        }

        public override Color GetRecordBackgroundColor()
        {
            return Color.FromRgb(42, 159, 214);//return Color.FromRgb(120, 120, 120);
        }

        public override Color GetRecordForegroundColor()
        {
            return Color.FromRgb(255, 255, 255);
        }

        public override Color GetSelectionBackgroundColor()
        {
            return Color.FromRgb(120, 120, 120);
        }

        public override Color GetSelectionForegroundColor()
        {
            return Color.FromRgb(255, 255, 255);
        }

        //public override Color GetCaptionSummaryRowBackgroundColor()
        //{
        //    return Color.FromRgb(02, 02, 02);
        //}

        //public override Color GetCaptionSummaryRowForeGroundColor()
        //{
        //    return Color.FromRgb(255, 255, 255);
        //}

        public override Color GetBorderColor()
        {
            return Color.FromRgb(150, 150, 150);//return Color.FromRgb(81, 83, 82);
        }

        public override Color GetLoadMoreViewBackgroundColor()
        {
            return Color.FromRgb(242, 242, 242);
        }

        public override Color GetLoadMoreViewForegroundColor()
        {
            return Color.FromRgb(34, 31, 31);
        }

        //public override Color GetAlternatingRowBackgroundColor()
        //{
        //    return Color.Yellow;
        //}
    }
    public class HistTheme : DataGridStyle
    {
        public HistTheme()
        {
        }

        public override Color GetHeaderBackgroundColor()
        {
            return Color.FromRgb(80, 80, 80);
        }

        public override Color GetHeaderForegroundColor()
        {
            return Color.FromRgb(255, 255, 255);
        }

        public override Color GetRecordBackgroundColor()
        {
            return Color.FromRgb(120, 120, 120);
        }

        public override Color GetRecordForegroundColor()
        {
            return Color.FromRgb(255, 255, 255);
        }

        public override Color GetSelectionBackgroundColor()
        {
            return Color.FromRgb(120, 120, 120);
        }

        public override Color GetSelectionForegroundColor()
        {
            return Color.FromRgb(255, 255, 255);
        }

        //public override Color GetCaptionSummaryRowBackgroundColor()
        //{
        //    return Color.FromRgb(02, 02, 02);
        //}

        //public override Color GetCaptionSummaryRowForeGroundColor()
        //{
        //    return Color.FromRgb(255, 255, 255);
        //}

        public override Color GetBorderColor()
        {
            return Color.FromRgb(81, 83, 82);
        }

        public override Color GetLoadMoreViewBackgroundColor()
        {
            return Color.FromRgb(242, 242, 242);
        }

        public override Color GetLoadMoreViewForegroundColor()
        {
            return Color.FromRgb(34, 31, 31);
        }

        //public override Color GetAlternatingRowBackgroundColor()
        //{
        //    return Color.Yellow;
        //}
    }
    public partial class MainPage : ContentPage
    {
        public bool bDeleteFlag = false;
        public bool bReOrderFlag = false;
        public string m_CurrentTopicType;
        public MainPage()
        {
            InitializeComponent();
            m_CurrentTopicType = TopicType.List;
            BackgroundImageSource = ImageSource.FromResource("App1.Images.bkg1.png");
            //hbartop.Source = ImageSource.FromResource("App1.Images.hbar.png");
            //hbarbottom.Source = ImageSource.FromResource("App1.Images.hbar.png");

            prev.Source = ImageSource.FromResource("App1.Images.prev2.png");
            next.Source = ImageSource.FromResource("App1.Images.next2.png");

            yogaW.Source = ImageSource.FromResource("App1.Images.yogaW.png");
            studyW.Source = ImageSource.FromResource("App1.Images.studyW.png");
            runningW.Source = ImageSource.FromResource("App1.Images.runningW.png");
            musicW.Source = ImageSource.FromResource("App1.Images.musicW.png");
            celebrateW.Source = ImageSource.FromResource("App1.Images.celebrateW.png");
        }
        private void Start_Clicked(object sender, EventArgs e)
        {

        }
        private void BtnPrevTopictype_Clicked(object sender, EventArgs e)
        {
            m_CurrentTopicType = TopicType.GetPrev(m_CurrentTopicType);
            lblTopicType.Text = m_CurrentTopicType;
            //m_ListEngine.GetTopicList(m_CurrentTopicType);
            if (m_CurrentTopicType == TopicType.List)
            {
                MyLists.IsVisible = true;
                MyQuotes.IsVisible = false;
                MyThoughts.IsVisible = false;

            }
            else if (m_CurrentTopicType == TopicType.Quotes)
            {
                MyLists.IsVisible = false;
                (MyQuotes as QuoteView)?.PageAppearing();
                MyQuotes.IsVisible = true;
                MyThoughts.IsVisible = false;
            }
            else if (m_CurrentTopicType == TopicType.Thoughts)
            {
                MyLists.IsVisible = false;
                MyQuotes.IsVisible = false;
                (MyThoughts as ThoughtsView)?.PageAppearing();
                MyThoughts.IsVisible = true;
            }
            else
            {
                MyLists.IsVisible = true;
                MyQuotes.IsVisible = false;
                MyThoughts.IsVisible = false;
            }
        }
        private void BtnNextTopictype_Clicked(object sender, EventArgs e)
        {
            m_CurrentTopicType = TopicType.GetNext(m_CurrentTopicType);
            lblTopicType.Text = m_CurrentTopicType;
            //m_ListEngine.GetTopicList(m_CurrentTopicType);
            if (m_CurrentTopicType == TopicType.List)
            {
                MyLists.IsVisible = true;
                MyQuotes.IsVisible = false;
                MyThoughts.IsVisible = false;
            }
            else if (m_CurrentTopicType == TopicType.Quotes)
            {
                MyLists.IsVisible = false;
                (MyQuotes as QuoteView)?.PageAppearing();
                MyQuotes.IsVisible = true;
                MyThoughts.IsVisible = false;
            }
            else if (m_CurrentTopicType == TopicType.Thoughts)
            {
                MyLists.IsVisible = false;
                MyQuotes.IsVisible = false;
                (MyThoughts as ThoughtsView)?.PageAppearing();
                MyThoughts.IsVisible = true;
            }
            else
            {
                MyLists.IsVisible = true;
                MyQuotes.IsVisible = false;
                MyThoughts.IsVisible = false;
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            // (MyThoughts as ThoughtsView)?.PageAppearing();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            //       (MyThoughts as ThoughtsView?.PageDisappearing();
        }

        public void RefreshData()
        {
        }
    }
}