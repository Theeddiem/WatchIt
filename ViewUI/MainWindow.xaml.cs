﻿using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using ViewModel;

namespace MainProgramUi
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ViewModelGlue m_ViewModelGlue = new ViewModelGlue();

        public MainWindow()
        {
            InitializeComponent();
            getDataListBox.ItemsSource = m_ViewModelGlue.StoredFilesInPc;
            MoviesListBox.ItemsSource = m_ViewModelGlue.MoviesFound;
        }   

        private void GetMoviesFromPc_Click(object sender, RoutedEventArgs e)
        {
            m_ViewModelGlue.GetMoviesFromPc();
        }

        private void SortTypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
          ComboBoxItem select = (ComboBoxItem)SortTypeComboBox.SelectedValue;

          m_ViewModelGlue.sortType(select.Content.ToString());
        }

        private void ClearListBtn_Click(object sender, RoutedEventArgs e)
        {
            getDataListBox.Items.Clear();
        }

        private void MoviesListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            if (MoviesListBox.SelectedItem != null)
            {
                string url = (MoviesListBox.SelectedItem as Movie).ImagePathUrl;
                var bi = new BitmapImage();
                bi.BeginInit();
                bi.UriSource = new Uri(url);
                bi.EndInit();
                CoverImage.Source = bi;
            }
        }

        private void ImdbBtn_Click(object sender, RoutedEventArgs e)
        {
            imdbButton();
        }

        private void imdbButton()
        {
            if (MoviesListBox.SelectedItem != null)
            {
                string imdbSite = string.Format("https://www.imdb.com/title/{0}", (MoviesListBox.SelectedItem as Movie).ImdbId);
                System.Diagnostics.Process.Start(imdbSite);
            }
        }

        private void PlayBtn_Click(object sender, RoutedEventArgs e)
        {
            playButton();
        }

        private void playButton()
        {
            if (MoviesListBox.SelectedItem != null)
            {
                System.Diagnostics.Process.Start((MoviesListBox.SelectedItem as Movie).FilePath);
            }
        }
    }
}
