using System.Windows;
using System.Windows.Forms;
using System;
using System.Drawing;
using FileHelpers;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Lastpass_HaveIBeenPwned_Checker
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ObservableCollection<Site> sites;
        public MainWindow()
        {
            this.sites = new ObservableCollection<Site>();
            InitializeComponent();
            QueueView.ItemsSource = this.sites;
            this.DataContext = this;
            this.PopulateDetailView();
        }

        private void PopulateDetailView()
        {
            int SelectedItem = QueueView.SelectedIndex;
            if (SelectedItem == -1)
            {
                return;
            }
            SiteLabel.Content = this.sites[SelectedItem].Name;
            URLLabel.Content = this.sites[SelectedItem].Url;
            UsernameLabel.Content = this.sites[SelectedItem].Username;
            PasswordLabel.Content = this.sites[SelectedItem].Password;
            var a = QueueView.Items[SelectedItem];
            if (this.sites[SelectedItem].Matched)
            {
                MatchedLabel.Content = "Yes";
                MatchCountLabel.Content = this.sites[SelectedItem].Count.ToString();
            } else
            {
                MatchedLabel.Content = "No";
                MatchCountLabel.Content = "0";
            }
        }

        private void ImportClick(object sender, RoutedEventArgs e)
        {
            string filePath = string.Empty;
            using(OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "CSV files (*.csv)|*.csv";
                if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    this.sites.Clear();
                    FileHelperEngine<Site> engine = new FileHelperEngine<Site>();
                    Site[] result = engine.ReadFile(openFileDialog.FileName);
                    foreach (Site res in result)
                    {
                        this.sites.Add(res);
                    }
                }
            }
        }

        private void RunClick(object sender, RoutedEventArgs e)
        {
            Checker ch = new Checker();
            for (int i = 0; i < this.sites.Count; i++)
            {
                ch.CheckSite(this.sites[i]);
                if (i == this.QueueView.SelectedIndex)
                {
                    this.PopulateDetailView();
                }
            }
        }

        private void ExitClick(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }

        private void QueueViewSelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            this.PopulateDetailView();
        }
    }
}
