using FileHelpers;
using System.ComponentModel;

namespace Lastpass_HaveIBeenPwned_Checker
{
    [IgnoreFirst(1)]
    [IgnoreEmptyLines()]
    [DelimitedRecord(",")]
    public class Site: INotifyPropertyChanged
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
        [FieldOrder(7)]
        [FieldHidden]
        public bool Matched { get { return _Matched; } set { this._Matched = value; this.onPropertyChanged("Matched"); } }
        [FieldHidden]
        public int Count { get; set; }
        [FieldHidden]
        private bool _Matched;

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
        }

        public override string ToString()
        {
            return this.Name + " (" + this.Username + ")";
        }

        public event PropertyChangedEventHandler PropertyChanged;
        
        private void onPropertyChanged(string PropName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(PropName));
            }
        }
    }
}
