using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using CefSharp;
using CefSharp.Wpf;

namespace DSA
{
    public partial class MainWindow : Window
    {
        new int TabIndex = 1;
        ObservableCollection<TabVM> Tabs = new ObservableCollection<TabVM>();
        ObservableCollection<string> history = new ObservableCollection<string>();

        public MainWindow()
        {
            InitializeComponent();
            StackPanelWithListHistory.Width = 0;
            if (!File.Exists(DataBase.FILE_HISTORY)) 
                File.Create(DataBase.FILE_HISTORY);

            DataBase.fillList(history, DataBase.FILE_HISTORY);
            this.WindowState = WindowState.Maximized;
            var tab1 = new TabVM()
            {
                Header = $"Tab {TabIndex}",
                Content = new ChromiumWebBrowser("http://github.com")
            };
            Tabs.Add(tab1);
            AddNewPlusButton();

            MyTabControl.ItemsSource = Tabs;
            MyTabControl.SelectionChanged += MyTabControl_SelectionChanged;
            ListHistory.ItemsSource = history;
        }


       

     
        private void Button_Click_SV(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void Button_Click_FullScreen(object sender, RoutedEventArgs e)
        {
            if(this.WindowState == WindowState.Maximized)
            {
                this.WindowState = WindowState.Normal;
            }else
                this.WindowState = WindowState.Maximized;
        }
        private void Grid_Mouse(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                this.WindowState = WindowState.Normal;
                DragMove();
                
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DataBase.saveList(history, DataBase.FILE_HISTORY);
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
            //tab.Header = $"Tab {TabIndex}";
            tab.IsPlaceholder = false;
            tab.Content = new ChromiumWebBrowser("https://duckduckgo.com");
            MessageBox.Show(tab.Content.Title);
            tab.Header = tab.Content.Title;
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
                if (index == Tabs.Count - 2)
                {
                    MyTabControl.SelectedIndex--;
                }
                Tabs.RemoveAt(index);
            }
        }

     

        private void SearchTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (MyTabControl.SelectedItem != null)
            {
                (MyTabControl.SelectedItem as TabVM).Content.Address = SearchTextBox.Text;
                (MyTabControl.SelectedItem as TabVM).Header = (MyTabControl.SelectedItem as TabVM).Content.Title;
            }
        }

        private void Button_Click_Back(object sender, RoutedEventArgs e)
        {
            if (MyTabControl.SelectedItem != null)
            {
                TabVM tab = MyTabControl.SelectedItem as TabVM;
                ChromiumWebBrowser w = tab.Content;
                if (w.CanGoBack)
                    w.Back();
            }
        }

        private void Button_Click_Next(object sender, RoutedEventArgs e)
        {
            if (MyTabControl.SelectedItem != null)
            {
                TabVM tab = MyTabControl.SelectedItem as TabVM;
                ChromiumWebBrowser w = tab.Content;
                if (w.CanGoForward)
                    w.Forward();
            }
        }

        private void Button_Click_Update(object sender, RoutedEventArgs e)
        {
            if (MyTabControl.SelectedItem != null)
            {
                TabVM tab = MyTabControl.SelectedItem as TabVM;
                ChromiumWebBrowser w = tab.Content;
                w.Address = w.Address;
          
            }
        }

        private void Button_Click_GoHome(object sender, RoutedEventArgs e)
        {
            if (MyTabControl.SelectedItem != null)
            {
                TabVM tab = MyTabControl.SelectedItem as TabVM;
                ChromiumWebBrowser w = tab.Content;
                w.Address = "https://duckduckgo.com";
            }
        }


        private void Button_Click_History(object sender, RoutedEventArgs e)
        {
            DoubleAnimation animation = new DoubleAnimation();
            animation.From = StackPanelWithListHistory.ActualWidth;
            if (StackPanelWithListHistory.ActualWidth == 0)
                animation.To = 500;
            else
                animation.To = 0;
            animation.AccelerationRatio = 1;
            animation.Duration = TimeSpan.FromSeconds(0.1);
           // asd.Background = (SolidColorBrush) Application.Current.Resources["ColorBlack"];
            StackPanelWithListHistory.BeginAnimation(StackPanel.WidthProperty, animation);
        }

        private void Button_Click_delete_item_history(object sender, RoutedEventArgs e)
        {
            if (ListHistory.SelectedItem != null)
                history.Remove(ListHistory.SelectedItem as string);
        }

        private void ListHistory_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (ListHistory.SelectedItem != null)
            {
                TabIndex++;
                var tab1 = new TabVM()
                {
                    Header = $"Tab {TabIndex}",
                    Content = new ChromiumWebBrowser(ListHistory.SelectedItem as string)
                };
            
                Tabs.Insert(Tabs.Count - 1, tab1);
            }
                
        }
    }
}