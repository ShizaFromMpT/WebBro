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
        ObservableCollection<string> favorites = new ObservableCollection<string>();

        public static List<string> list_themeNord = new List<string>
        {
            "GridBackgroundNord",    //0
            "GridTopNord",           //1
            "TextBoxStyleNord",      //2
            "ButonStyleNord",        //3
            "TabControlStyleNord",   //4
            "GridListNord",          //5
            "LabelStyleNord",        //6
            "ButonStyle2Nord",       //7
            "TextBlockStyleNord",    //8
            "CheckboxStyleNord",     //9
            "RadioButtonStyleNord"      //10
        };


        public static List<string> list_themeDark = new List<string>
        {
            "GridBackgroundDark",    //0
            "GridTopDark",           //1
            "TextBoxStyleDark",      //2
            "ButonStyleDark",        //3
            "TabControlStyleDark",   //4
            "GridListDark",          //5
            "LabelStyleDark",        //6
            "ButonStyle2Dark",       //7
            "TextBlockStyleDark",    //8
            "CheckboxStyleDark",     //9
            "RadioButtonStyleDark"      //10
        };


        public static List<string> list_themeLight = new List<string>
        {
            "GridBackgroundLight",    //0
            "GridTopLight",           //1
            "TextBoxStyleLight",      //2
            "ButonStyleLight",        //3
            "TabControlStyleLight",   //4
            "GridListLight",          //5
            "LabelStyleLight",        //6
            "ButonStyle2Light",       //7
            "TextBlockStyleLight",    //8
            "CheckboxStyleLight",     //9
            "RadioButtonStyleLight"      //10
        };


        public MainWindow()
        {
            InitializeComponent();
            GridWithLists.Width = 0;
            GridWithSettings.Width = 0;



            if (!File.Exists(DataBase.FILE_HISTORY))
                File.Create(DataBase.FILE_HISTORY);
            if (!File.Exists(DataBase.FILE_FAVORITES))
                File.Create(DataBase.FILE_FAVORITES);

            DataBase.fillList(history, DataBase.FILE_HISTORY);
            DataBase.fillList(favorites, DataBase.FILE_FAVORITES);



            this.WindowState = WindowState.Maximized;
            var tab1 = new TabVM();

            tab1.Content = new ChromiumWebBrowser("https://github.com");
            tab1.Header = tab1.Content.Title;


            tab1.Content.TitleChanged += ChromiumWebBrowser_TitleChanged;


            Tabs.Add(tab1);
            AddNewPlusButton();

            MyTabControl.ItemsSource = Tabs;
            MyTabControl.SelectionChanged += MyTabControl_SelectionChanged;
            ListHistory.ItemsSource = history;
            ListFavorites.ItemsSource = favorites;
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

        private void Button_Click_shdo(object sender, RoutedEventArgs e)
        {
            DataBase.saveList(history, DataBase.FILE_HISTORY);
            DataBase.saveList(favorites, DataBase.FILE_FAVORITES);
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
          //  MessageBox.Show(tab.Content.Title);
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
            if (e.Key == Key.Return)
            {
                if (MyTabControl.SelectedItem != null)
                {
                    (MyTabControl.SelectedItem as TabVM).Content.Address = SearchTextBox.Text;
                    (MyTabControl.SelectedItem as TabVM).Header = (MyTabControl.SelectedItem as TabVM).Content.Title;
                }
                history.Add(SearchTextBox.Text);
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
            animation.From = GridWithLists.ActualWidth;
            if (GridWithLists.ActualWidth == 0)
                animation.To = 500;
            else
                animation.To = 0;
            animation.AccelerationRatio = 1;
            animation.Duration = TimeSpan.FromSeconds(0.1);
            GridWithLists.BeginAnimation(StackPanel.WidthProperty, animation);


            DoubleAnimation animation2 = new DoubleAnimation();
            animation2.From = GridWithSettings.ActualWidth;
            if (GridWithSettings.ActualWidth != 0)
                animation2.To = 0;
            animation2.AccelerationRatio = 1;
            animation2.Duration = TimeSpan.FromSeconds(0.1);
            GridWithSettings.BeginAnimation(StackPanel.WidthProperty, animation2);
        }



        private void Button_Click_Settings(object sender, RoutedEventArgs e)
        {

            DoubleAnimation animation = new DoubleAnimation();
            animation.From = GridWithSettings.ActualWidth;
            if (GridWithSettings.ActualWidth == 0)
                animation.To = 300;
            else
                animation.To = 0;
            animation.AccelerationRatio = 1;
            animation.Duration = TimeSpan.FromSeconds(0.1);
            GridWithSettings.BeginAnimation(StackPanel.WidthProperty, animation);


            DoubleAnimation animation2 = new DoubleAnimation();
            animation2.From = GridWithLists.ActualWidth;
            if (GridWithLists.ActualWidth != 0)
                animation2.To = 0;
            animation2.AccelerationRatio = 1;
            animation2.Duration = TimeSpan.FromSeconds(0.1);
            GridWithLists.BeginAnimation(StackPanel.WidthProperty, animation2);
        }


        private void Button_Click_delete_item_history(object sender, RoutedEventArgs e)
        {
            if (ListHistory.SelectedItem != null)
                history.Remove(ListHistory.SelectedItem as string);
        }

        private void List_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ListView listView = (ListView)sender;
            if (listView.SelectedItem != null)
            {
                TabIndex++;
                TabVM tw = new TabVM();
                tw.Content = new ChromiumWebBrowser(listView.SelectedItem as string);
                tw.Header = tw.Content.Title;
                Tabs.Insert(Tabs.Count - 1, tw);
            }

        }

        private void Button_Click_Add_Favorites(object sender, RoutedEventArgs e)
        {
            try
            {
                if (MyTabControl.SelectedItem != null)
                {
                    TabVM tab = MyTabControl.SelectedItem as TabVM;
                    ChromiumWebBrowser w = tab.Content;
                    if (!favorites.Any(str2 => str2 == w.Address.ToString()))
                        favorites.Add(w.Address.ToString());
                }
            }
            catch (Exception e2) { }
          
        }

        private void Button_Click_delete_item_history_All(object sender, RoutedEventArgs e)
        {
            history.Clear();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            favorites.Clear();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            if (ListFavorites.SelectedItem != null)
                favorites.Remove(ListFavorites.SelectedItem as string);
        }

        private void ChromiumWebBrowser_TitleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (MyTabControl.SelectedIndex != -1)
                Tabs[MyTabControl.SelectedIndex].Header = (sender as ChromiumWebBrowser).Title;
        }

        private void ButtonDownload_Click(object sender, RoutedEventArgs e)
        {
           
        }

        private void Button_Click_Apply_Theme(object sender, RoutedEventArgs e)
        {
            if (RadioButLightTheme.IsChecked == true)
            {
                 setStile(list_themeLight);
            }
            else if (RadioButDarkTheme.IsChecked == true)
            {
                setStile(list_themeDark);
            }
            else if (RadioButNordTheme.IsChecked == true)
            {
                setStile(list_themeNord);

            }

        }

       

        public void setStile(List<string> styles)
        {
            GridBackGround.Style = (Style)FindResource(styles[0]);
            GridTop.Style = (Style)FindResource(styles[1]);
            SearchTextBox.Style = (Style)FindResource(styles[2]);

            

            ButtonBack.Style = (Style)FindResource(styles[3]);
            ButtonForvard.Style = (Style)FindResource(styles[3]);
            ButtonReload.Style = (Style)FindResource(styles[3]);
            ButtonHome.Style = (Style)FindResource(styles[3]);
            ButtonDownload.Style = (Style)FindResource(styles[3]);
            ButtonFavorite.Style = (Style)FindResource(styles[3]);
            ButtonLists.Style = (Style)FindResource(styles[3]);
            ButtonSettings.Style = (Style)FindResource(styles[3]);

            MyTabControl.Style = (Style)FindResource(styles[4]);

            GridWithLists.Style = (Style)FindResource(styles[5]);

            LabelHistory.Style = (Style)FindResource(styles[6]);
            Button_deleteAll_History.Style = (Style)FindResource(styles[7]);
            Button_delete_History.Style = (Style)FindResource(styles[7]);

            LabelFavorites.Style = (Style)FindResource(styles[6]);
            Button_delete_Favorites.Style = (Style)FindResource(styles[7]);
            Button_deleteAll_Favorites.Style = (Style)FindResource(styles[7]);

            GridWithSettings.Style = (Style)FindResource(styles[5]);
            Label_Settings.Style = (Style)FindResource(styles[6]);

            Label_IncognitoMode.Style = (Style)FindResource(styles[6]);
            CheckBoxIncognito.Style = (Style)FindResource(styles[9]);
            LabelThemes.Style = (Style)FindResource(styles[6]);
            Button_Apply_Theme.Style = (Style)FindResource(styles[7]);


            RadioButLightTheme.Style = (Style)FindResource(styles[10]);
            RadioButDarkTheme.Style = (Style)FindResource(styles[10]);
            RadioButNordTheme.Style = (Style)FindResource(styles[10]);

        }




    }
}