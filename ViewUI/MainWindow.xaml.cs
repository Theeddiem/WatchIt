using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows;
using System.Windows.Controls;
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

            if(m_ViewModelGlue.CurrentSettings.LoadMovies())
            {
                m_ViewModelGlue.MoviesFound = m_ViewModelGlue.CurrentSettings.MoviesFound;
                m_ViewModelGlue.LoadImageForEachMovie();
            }


            if (m_ViewModelGlue.CurrentSettings.LoadFiles())
            {
                m_ViewModelGlue.StoredFilesInPc = m_ViewModelGlue.CurrentSettings.StoredFilesInPc;
            }

            getDataListBox.ItemsSource = m_ViewModelGlue.StoredFilesInPc;
            MoviesListBox.ItemsSource = m_ViewModelGlue.MoviesFound;

            m_ViewModelGlue.sortType("By Rating");
            SortTypeComboBox.SelectedIndex = 1;

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

        private void MoviesListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            if (MoviesListBox.SelectedItem != null)
            {
                CoverImage.Source = (MoviesListBox.SelectedItem as Movie).CoverImage;
            }

            FixedListBox.Visibility = Visibility.Hidden;
        }

 
        private void ImdbBtn_Click(object sender, RoutedEventArgs e)
        {
            if (MoviesListBox.SelectedItem != null)
            {
                m_ViewModelGlue.OpenImdbSite(MoviesListBox.SelectedItem as Video);
            }
        }

        private void PlayBtn_Click(object sender, RoutedEventArgs e)
        {
            if (MoviesListBox.SelectedItem != null)
            {
                m_ViewModelGlue.PlayFile(MoviesListBox.SelectedItem as Video);
            }
        }

        private void OpenFolderBtn_Click(object sender, RoutedEventArgs e)
        {
            if (MoviesListBox.SelectedItem != null)
            {
                m_ViewModelGlue.OpenFolder(MoviesListBox.SelectedItem as Video);
            }
        }

        private void EditMovieBtn_Click(object sender, RoutedEventArgs e)
        {
            if (MoviesListBox.SelectedItem != null)
            {
                FixedListBox.Items.Clear();
                FixedListBox.Visibility = Visibility.Visible;
                foreach (var item in m_ViewModelGlue.MoviesResults(MoviesListBox.SelectedItem as Movie))
                {
                    FixedListBox.Items.Add(item);
                }
            }

        }

        private void FixedListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (FixedListBox.SelectedItem != null)
            {
                CoverImage.Source = (FixedListBox.SelectedItem as Movie).CoverImage;

                m_ViewModelGlue.changeRefMovies(MoviesListBox.SelectedItem as Movie, FixedListBox.SelectedItem as Movie);
            }

        }


        private void Window_Closed(object sender, EventArgs e)
        {
            m_ViewModelGlue.CurrentSettings.Save();
        }
    }
}

