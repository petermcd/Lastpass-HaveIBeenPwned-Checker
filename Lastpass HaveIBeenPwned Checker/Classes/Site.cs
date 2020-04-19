using FileHelpers;
using System;
using System.ComponentModel;

namespace Lastpass_HaveIBeenPwned_Checker
{
    [IgnoreFirst(1)]
    [IgnoreEmptyLines]
    [DelimitedRecord(",")]
    public class Site : INotifyPropertyChanged
    {
        [FieldOrder(0)]
        public string Url { get; set; }
        [FieldOrder(1)]
        public string Username { get; set; }
        [FieldOrder(2)]
        [FieldOptional]
        public string Password { get; set; }
        [FieldOrder(3)]
        [FieldOptional]
        [FieldQuoted(QuoteMode.OptionalForRead, MultilineMode.AllowForRead)]
        public string Extra { get; set; }
        [FieldOrder(4)]
        [FieldOptional]
        public string Name { get; set; }
        [FieldOrder(5)]
        [FieldOptional]
        public string Grouping { get; set; }
        [FieldOrder(6)]
        [FieldOptional]
        public string Fav { get; set; }
        [FieldHidden]
        public bool Matched { get { return this._Matched; } set { this._Matched = value; this.OnPropertyChanged("Matched"); } }
        [FieldHidden]
        public int Count { get; set; }
        [FieldHidden]
        public string Sha1Password
        {
            get
            {
                if (this._Sha1Password == string.Empty)
                {
                    this._Sha1Password = Helpers.ConvertToSha1(this.Password);
                    OnPropertyChanged("Sha1Password");
                }
                return this._Sha1Password;
            }
            set
            {
                throw new InvalidOperationException();
            }
        }
        [FieldHidden]
        public string Sha1PasswordShortened
        {
            get
            {
                if (this._Sha1PasswordShortened == string.Empty)
                {
                    this._Sha1PasswordShortened = this.Sha1Password.Substring(0, Math.Min(this.Password.Length, 5));
                    OnPropertyChanged("Sha1PasswordShortened");
                }
                return this._Sha1PasswordShortened;
            }
            set
            {
                throw new InvalidOperationException();
            }
        }
        [FieldHidden]
        public string[] Responses
        {
            get => this._Responses;
            set
            {
                this._Responses = value;
                this.OnPropertyChanged("Responses");
            }
        }
        [FieldHidden]
        public bool Processed
        {
            get => this._Processed;
            set
            {
                this._Processed = value;
                this.OnPropertyChanged("Processed");
            }
        }
        [FieldHidden]
        private bool _Matched;
        [FieldHidden]
        private string[] _Responses;
        [FieldHidden]
        private bool _Processed;
        [FieldHidden]
        private string _Sha1Password = "";
        [FieldHidden]
        private string _Sha1PasswordShortened = "";

        public Site()
        {
            this.Url = string.Empty;
            this.Username = string.Empty;
            this.Password = string.Empty;
            this.Extra = string.Empty;
            this.Name = string.Empty;
            this.Grouping = string.Empty;
            this.Fav = string.Empty;
            this.Matched = false;
            this.Count = 0;
            this._Processed = false;
            this.Responses = new string[] { "Not Checked" };
        }

        public override string ToString()
        {
            return this.Name + " (" + this.Username + ")";
        }

        public bool HasPassword()
        {
            if(this.Password == string.Empty)
            {
                return false;
            }
            return true;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string PropName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(PropName));
            }
        }
    }
}
