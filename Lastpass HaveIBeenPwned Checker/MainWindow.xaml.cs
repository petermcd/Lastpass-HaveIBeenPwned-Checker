using System.Windows;
using System.Windows.Forms;
using System;
using System.Drawing;
using FileHelpers;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using System.Linq;

namespace Lastpass_HaveIBeenPwned_Checker
{
    public partial class MainWindow : Window
    {
        public ObservableCollection<Site> sites;
        Thread mainThread;
        public MainWindow()
        {
            this.mainThread = Thread.CurrentThread;
            this.sites = new ObservableCollection<Site>();
            InitializeComponent();
            QueueView.ItemsSource = this.sites;
            this.DataContext = this;
            this.PopulateDetailView();
            this.HideProgress();
        }
        #region Progress Bar
        private void ShowProgress()
        {
            progress.Value = 0;
            progress.Visibility = Visibility.Visible;
            ProgressLabel.Visibility = Visibility.Visible;
        }
        private void IncreaseProgress()
        {
            progress.Value++;
            if (progress.Value == this.sites.Count)
            {
                ProgressLabel.Content = "Complete";
                return;
            }
            ProgressLabel.Content = progress.Value.ToString() + "/" + this.sites.Count.ToString();
        }
        private void HideProgress()
        {
            progress.Value = 0;
            progress.Visibility = Visibility.Hidden;
            ProgressLabel.Visibility = Visibility.Hidden;
        }
        #endregion
        #region Click Actions
        private void ImportClick(object sender, RoutedEventArgs e)
        {
            string filePath = string.Empty;
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "CSV files (*.csv)|*.csv";
                if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    this.sites.Clear();
                    this.HideProgress();
                    FileHelperEngine<Site> engine = new FileHelperEngine<Site>();
                    Site[] result = engine.ReadFile(openFileDialog.FileName);
                    foreach (Site res in result)
                    {
                        this.sites.Add(res);
                    }
                    progress.Maximum = this.sites.Count;
                    if (this.sites.Count > 0)
                    {
                        QueueView.SelectedIndex = 0;
                    }
                }
            }
        }
        private void RunClick(object sender, RoutedEventArgs e)
        {
            this.ShowProgress();
            for (int i = 0; i < this.sites.Count; i++)
            {
                ThreadPool.QueueUserWorkItem(CheckSite, this.sites[i]);
            }
        }
        private void CheckSite(object site)
        {
            Site iSite = site as Site;
            Checker ch = new Checker();
            ch.CheckSite(iSite);
            this.Dispatcher.Invoke(() =>
            {
                this.IncreaseProgress();
                this.PopulateDetailView();
            });
        }
        private void ExitClick(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }
        private void About_Click(object sender, RoutedEventArgs e)
        {
            About about_screen = new About();
            about_screen.Show();
        }
        #endregion
        #region Details View
        private void QueueViewSelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
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
            }
            else
            {
                MatchedLabel.Content = "No";
                MatchCountLabel.Content = "0";
            }
            string ApiUrl = "Not Checked";
            string HashedPassword = "Not Checked";
            string Response = "Not Checked";
            if (this.sites[SelectedItem].Processed)
            {
                ApiUrl = "No Password";
                HashedPassword = "No Password";
                Response = "No Password";
                if (this.sites[SelectedItem].HasPassword())
                {
                    ApiUrl = Checker.API_URL + this.sites[SelectedItem].Sha1PasswordShortened;
                    HashedPassword = this.sites[SelectedItem].Sha1Password;
                    Response = string.Join("\r\n", this.sites[SelectedItem].Responses);
                }
            }
            ApiUrlLabel.Content = ApiUrl;
            HashedPasswordLabel.Content = HashedPassword;
            ResponseLabel.Text = Response;
        }
        #endregion
    }
}
