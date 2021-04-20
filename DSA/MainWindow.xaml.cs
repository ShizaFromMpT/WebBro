using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using CefSharp;

namespace DSA
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        new int TabIndex = 1;
        ObservableCollection<TabVM> Tabs = new ObservableCollection<TabVM>();

        public MainWindow()
        {
            InitializeComponent();

            var tab1 = new TabVM()
            {
                Header = $"Tab {TabIndex}",
                //Content = new ContentVM("First tab", 1)
                Content = new CefSharp.Wpf.ChromiumWebBrowser("https://mpt.ru")
            };
            Tabs.Add(tab1);
            AddNewPlusButton();

            MyTabControl.ItemsSource = Tabs;
            MyTabControl.SelectionChanged += MyTabControl_SelectionChanged;

        }

        private void Button_Click_SV(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void Button_Click_FullScreen(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Maximized;
        }
        private void Grid_Mouse(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void MyTabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.Source is TabControl)
            {
                var pos = MyTabControl.SelectedIndex;
                if (pos != 0 && pos == Tabs.Count - 1) //last tab
                {
                    var tab = Tabs.Last();
                    ConvertPlusToNewTab(tab);
                    AddNewPlusButton();
                }
            }
        }


        void ConvertPlusToNewTab(TabVM tab)
        {
            //Do things to make it a new tab.
            TabIndex++;
            tab.Header = $"Tab {TabIndex}";
            tab.IsPlaceholder = false;
            //tab.Content = new ContentVM("Tab content", TabIndex);
            tab.Content = new CefSharp.Wpf.ChromiumWebBrowser("https://duckduckgo.com");
        }

        void AddNewPlusButton()
        {
            var plusTab = new TabVM()
            {
                Header = "+",
                IsPlaceholder = true
            };
            Tabs.Add(plusTab);
        }

        private void OnTabCloseClick(object sender, RoutedEventArgs e)
        {
            var tab = (sender as Button).DataContext as TabVM;
            if (Tabs.Count > 2)
            {
                var index = Tabs.IndexOf(tab);
                if (index == Tabs.Count - 2)//last tab before [+]
                {
                    MyTabControl.SelectedIndex--;
                }
                Tabs.RemoveAt(index);
            }
        }

    }
}