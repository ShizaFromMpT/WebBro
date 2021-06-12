using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace DSA
{
    class TabVM : INotifyPropertyChanged, ICloneable
    {
        string _Header;
        public string Header
        {
            get => _Header;
            set
            {
                _Header = value;
                OnPropertyChanged();
            }
        }




        bool _IsPlaceholder = false;
        public bool IsPlaceholder
        {
            get => _IsPlaceholder;
            set
            {
                _IsPlaceholder = value;
                OnPropertyChanged();
            }
        }

       public CefSharp.Wpf.ChromiumWebBrowser Content { get; set; }
       /* CefSharp.Wpf.ChromiumWebBrowser _Content = null;
        public CefSharp.Wpf.ChromiumWebBrowser Content
        {
            get => _Content;
            set
            {
                _Content = value;
                OnPropertyChanged();
            }
        }*/

        public event PropertyChangedEventHandler PropertyChanged;
        void OnPropertyChanged([CallerMemberName] string property = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
