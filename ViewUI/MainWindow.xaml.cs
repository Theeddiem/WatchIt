using System;
using System.Timers;
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
            FixedListBox.ItemsSource = m_ViewModelGlue.reSearchMoviesFound;

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

            showHideControls(Visibility.Hidden);
        }

       private void labelTimer()
       {

               this.Dispatcher.Invoke(() =>
            {
                WarrningLabel.Visibility = Visibility.Visible;
                Timer timer = new Timer(3000);
                timer.Elapsed += dispatcherTimer_Tick;
                timer.Start();
               
            });

            //private void dispatcherTimer_Tick(object sender, EventArgs e)
            //{
            //    // code goes here
            //}
            //this.Dispatcher.Invoke(() =>
            //{
            //    WarrningLabel.Visibility = Visibility.Visible;
            //    System.Threading.Thread.Sleep(5000);
            //    WarrningLabel.Visibility = Visibility.Hidden;
            //});
            //WarrningLabel.Visibility = Visibility.Visible;
            //System.Threading.Thread.Sleep(5000);
            ////Timer timer = new Timer();
            ////timer.Interval = (200);
            ////timer.Start();
            //WarrningLabel.Visibility = Visibility.Hidden;

        }

        private void dispatcherTimer_Tick(Object obj, ElapsedEventArgs e)
        {
            this.Dispatcher.Invoke(() =>
            {
                WarrningLabel.Visibility = Visibility.Hidden;
            });
        }

        private bool ask() // do it in a a thred 
        {
            bool movieSelected = true;
            WarrningLabel.Visibility = Visibility.Hidden;
            if (MoviesListBox.SelectedItem == null)
            {
                new System.Threading.Thread(labelTimer).Start();
                movieSelected = false;

            }

            return movieSelected;
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
            if (ask())
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
            if(FixedListBox.Visibility == Visibility.Visible)
            {
                showHideControls(Visibility.Hidden);
            }

            else if (MoviesListBox.SelectedItem != null)
            {
                showHideControls(Visibility.Visible);
                m_ViewModelGlue.PageNumber = 0;
                m_ViewModelGlue.MoviesResults(MoviesListBox.SelectedItem as Movie, m_ViewModelGlue.PageNumber);
                PageLabel.Content = m_ViewModelGlue.PageNumber;
            }

        }

        private void showHideControls(Visibility i_Choise)
        {
            FixedListBox.Visibility = i_Choise;
            AcceptButton.Visibility = i_Choise;
            NextPageButton.Visibility = i_Choise;
            PreviousPageButton.Visibility = i_Choise;
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
                m_ViewModelGlue.changeRefMovies(MoviesListBox.SelectedItem as Movie, FixedListBox.SelectedItem as Movie);
            }
        }

        private void NextPageButton_Click(object sender, RoutedEventArgs e)
        {
            if (MoviesListBox.SelectedItem != null)
            {
                m_ViewModelGlue.MoviesResults(MoviesListBox.SelectedItem as Movie, ++m_ViewModelGlue.PageNumber);
                PageLabel.Content = m_ViewModelGlue.PageNumber;
            }
        }

        private void PreviousPageButton_Click(object sender, RoutedEventArgs e)
        {
            if (MoviesListBox.SelectedItem != null)
            {
                m_ViewModelGlue.MoviesResults(MoviesListBox.SelectedItem as Movie, --m_ViewModelGlue.PageNumber);
                PageLabel.Content = m_ViewModelGlue.PageNumber;
            }
        }
    }
}

