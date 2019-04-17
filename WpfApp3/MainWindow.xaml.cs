﻿using Logic;
using OMDbApiNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using TMDbLib.Client;
using TMDbLib.Objects.General;
using TMDbLib.Objects.Search;


namespace MainProgramUi
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private TMDbClient client;
        private List<Logic.Video> m_MoviesFound = new List<Logic.Video>();

        public MainWindow()
        {
            client = new TMDbClient("a959178bb3475c959db8941953d19bad");
            InitializeComponent();
        }

        private void UpdateMoviesAndSeriesListBox()
        {
                       
            try
            {
                for (int i = 0; i < getDataListBox.Items.Count; i++)
                {
                    SearchContainer<SearchMovie> results = client.SearchMovieAsync(getDataListBox.Items[i].ToString()).Result;

                    if (results.TotalResults > 0 )
                    {
                        SearchMovie result = results.Results[0];

                        Logic.Video title = null; ;

                        if (result.MediaType == MediaType.Movie)
                        {
                            TMDbLib.Objects.Movies.Movie movie = client.GetMovieAsync(result.Id).Result;

                            string genres = "";
                            foreach (var item in movie.Genres)
                            {
                                genres += (item.Name + ", ");
                            }

                            DateTime ReleaseYear = (DateTime)movie.ReleaseDate;

                            title = new Movie(movie.Title, genres, movie.VoteAverage, ReleaseYear.Year);

                            if (!m_MoviesFound.Any(n => n.Title == title.Title))
                            {
                                m_MoviesFound.Add(title);
                                MoviesListBox.Items.Add(title);
                            }


                        }
                    }

                    else
                    {
                        getDataListBox.Items[i] += "Not found try changeing the file name";

                    }
                }
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
     
        private void GetMoviesFromPc_Click(object sender, RoutedEventArgs e)
        {
            bool toUpdate = false;

            try
            {        
                foreach (var item in Utilities.GetMoviesFromPc())
                {
                    if (!getDataListBox.Items.Contains(item))
                    {
                        getDataListBox.Items.Add(item);
                        toUpdate = true;
                    }
                }
            }

            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }

            if (toUpdate)
            {
                UpdateMoviesAndSeriesListBox();
            }

        }

        private void SortTypeComboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {

          ComboBoxItem s = (ComboBoxItem)SortTypeComboBox.SelectedValue;

            if((string)s.Content == "By Year")
            {
                m_MoviesFound = m_MoviesFound.OrderByDescending(w => w.ReleasedYear).ToList();
            }

            if ((string)s.Content == "By Rating")
            {
                m_MoviesFound = m_MoviesFound.OrderByDescending(w => w.Rating).ToList();
            }

            if ((string)s.Content == "By Genre")
            {
                m_MoviesFound = m_MoviesFound.OrderBy(w => w.Genre).ToList();
            }

            MoviesListBox.Items.Clear();

            foreach (var item in m_MoviesFound)
            {
                MoviesListBox.Items.Add(item);
            }

        }

        private void ClearListBtn_Click(object sender, RoutedEventArgs e)
        {
            getDataListBox.Items.Clear();
        }
    }
}
