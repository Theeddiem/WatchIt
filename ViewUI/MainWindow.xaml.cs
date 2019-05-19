using System;
using System.Threading;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using ViewModel;

namespace MainProgramUi
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 


    public partial class MainWindow : Window
    {
        ViewModelGlue m_ViewModelGlue = new ViewModelGlue();
        Movie m_CurrentSelectedMovie;
        
        public MainWindow()
        {
            InitializeComponent();

            getDataListBox.ItemsSource = m_ViewModelGlue.StoredFilesInPc;
            MoviesListBox.ItemsSource = m_ViewModelGlue.MoviesFound;
            FixedListBox.ItemsSource = m_ViewModelGlue.movieChangePageManager.reSearchMoviesFound;
        }

        private void GetMoviesFromPc_Click(object sender, RoutedEventArgs e)
        {
            m_ViewModelGlue.GetMoviesFromPc();
        }

        private void SortTypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBoxItem select = (ComboBoxItem)SortTypeComboBox.SelectedValue;
            m_ViewModelGlue.SortMovies(select.Content.ToString());
        }

        private void MoviesListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            m_CurrentSelectedMovie = MoviesListBox.SelectedItem as Movie;
            CoverImage.Source = (m_CurrentSelectedMovie).CoverImage;
            showHideControls(Visibility.Hidden);
        }

        private void ImdbBtn_Click(object sender, RoutedEventArgs e)
        {
            if(!m_ViewModelGlue.OpenImdbSite(m_CurrentSelectedMovie))
            {
                active2secondLabelEffect();
            }
        }

        private void PlayBtn_Click(object sender, RoutedEventArgs e)
        {
            if(!m_ViewModelGlue.PlayFile(m_CurrentSelectedMovie))
            {
                active2secondLabelEffect();
            }
        }

        private void OpenFolderBtn_Click(object sender, RoutedEventArgs e)
        {
            if(!m_ViewModelGlue.OpenFolder(m_CurrentSelectedMovie))
            {
                active2secondLabelEffect();
            }
        }

        private void EditMovieBtn_Click(object sender, RoutedEventArgs e)
        {
            if (FixedListBox.Visibility == Visibility.Visible) // for second click on the button to close fixedlist
            {
                showHideControls(Visibility.Hidden); 
            }

            else if (m_CurrentSelectedMovie != null)
            {
                showHideControls(Visibility.Visible);

                m_ViewModelGlue.movieChangePageManager.MoiveReSearchResults(m_CurrentSelectedMovie, m_ViewModelGlue.movieChangePageManager.PageNumber);
                PageLabel.Content = m_ViewModelGlue.movieChangePageManager.PageNumber;
            }

            else if(m_CurrentSelectedMovie == null)
            {
                active2secondLabelEffect();
            }

        }

        private void NextPageButton_Click(object sender, RoutedEventArgs e)
        {
            if (m_CurrentSelectedMovie != null)
            {
                m_ViewModelGlue.movieChangePageManager.MoiveReSearchResults(m_CurrentSelectedMovie, ++m_ViewModelGlue.movieChangePageManager.PageNumber);
                PageLabel.Content = m_ViewModelGlue.movieChangePageManager.PageNumber;
            }
        }

        private void PreviousPageButton_Click(object sender, RoutedEventArgs e)
        {
            if (m_CurrentSelectedMovie != null)
            {
                m_ViewModelGlue.movieChangePageManager.MoiveReSearchResults(m_CurrentSelectedMovie, --m_ViewModelGlue.movieChangePageManager.PageNumber);
                PageLabel.Content = m_ViewModelGlue.movieChangePageManager.PageNumber;
            }
        }


        private void FixedListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (FixedListBox.SelectedItem != null)
            {
                CoverImage.Source = (FixedListBox.SelectedItem as Movie).CoverImage;
            }

        }

        private void Window_Closed(object sender, EventArgs e)
        {
            m_ViewModelGlue.CurrentSettings.Save();
        }

        private void AcceptButton_Click(object sender, RoutedEventArgs e)
        {
            if (FixedListBox.SelectedItem != null)
            {
                m_ViewModelGlue.ChangeMovies(MoviesListBox.SelectedItem as Movie, FixedListBox.SelectedItem as Movie);
            }
        }
    
        private void CoverImage_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {

            if (m_CurrentSelectedMovie != null)
            {
                OverViewLabel.Content = m_CurrentSelectedMovie.Overview;
            }
        }

        private void showHideControls(Visibility i_Choise)
        {
            FixedListBox.Visibility = i_Choise;
            AcceptButton.Visibility = i_Choise;
            NextPageButton.Visibility = i_Choise;
            PreviousPageButton.Visibility = i_Choise;
        }

        private void active2secondLabelEffect()
        {
            new Thread(WarrningLabel.startTimedLabel).Start();
        }

        //private void SearchBtn_Click(object sender, RoutedEventArgs e)
        //{

        //}
    }
}

