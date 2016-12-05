using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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
using FileManagerEngine;
using System.IO;
using System.Management;
using System.ComponentModel;
using System.Globalization;

namespace FileManagerWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 
    public partial class MainWindow : Window
    {
        FileManager FileManager;
        public MainWindow()
        {
            InitializeComponent();
            FileManager = new FileManager();
            ListViewDirectory.ItemTemplate = (DataTemplate)ListViewDirectory.FindResource("ListViewSmallView");
            FileManager.Error += ShowError;
            ShowTreeDisks();

            SetDirectory(@"C:\");
        }

        private void SetDirectory(string path)
        {
            try
            {
                if (FileManager.CheckReadPermissions(path))
                {
                    FileManager.SetCurrentDirectory(path);
                    ShowDirectory();
                    SetAddresBoxText();
                    EnableButton();
                }
                else
                {
                    ShowError(this, new ErrorEvent() { Value = string.Format("Odmowa dostępu do {0}.", path) });
                }
            } catch { }
        }

        private void ShowError(object sender, ErrorEvent e)
        {
            MessageBox.Show(e.Value, "Błąd", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);
            AddressTextBox.Focus();
        }

        private void ShowTreeDisks()
        {
            ComputerMenuItem comp = new ComputerMenuItem();
            //trvMenu.Items.Add(comp);
            Dictionary<string, DriveInfo> disks = DriveManager.GetDrives();
            foreach (var item in disks)
            {
                ShowTreeItems(item.Key);
            }
        }

        private void ShowTreeItems(string path, ObservableCollection<MenuItem> items = null)
        {
            MenuItem root = new MenuItem(path);
            ObservableCollection<DirectoryInfo> dirlist = FileManager.GetDirectories(root.Path);
            if (items != null)
                foreach (var item in dirlist)
                {
                    MenuItem dir = new MenuItem(item.FullName);
                    items.Add(dir);
                }
            else
            {
                foreach (var item in dirlist)
                {
                    MenuItem dir = new MenuItem(item.FullName);
                    root.Items.Add(dir);
                }
                trvMenu.Items.Add(root);
            }
        }
        
        private void ShowDirectory()
        {
            ShowDirectory(FileManager.GetCurrentDirectory().FullName);
        }

        private void ShowDirectory(string path)
        {
            DirectoryInfo dir = new DirectoryInfo(path);
            if (!dir.Exists)
                dir = FileManager.GetCurrentDirectory();

            ObservableCollection<FileManagerObject> list = new ObservableCollection<FileManagerObject>();
            foreach (var item in FileManager.GetFilesAndDirectories())
            {
                if (item is DirectoryInfo)
                    list.Add(new DirectoryObject(item as DirectoryInfo));
                else if (item is FileInfo)
                    list.Add(new FileObject(item as FileInfo));
            }

            ListViewDirectory.ItemsSource = list;
        }

        private void TreeViewItem_Expanded(object sender, RoutedEventArgs e)
        {
            MenuItem item = (e.OriginalSource as TreeViewItem).DataContext as MenuItem;
            if (item != null && (item.HasItems && item.Items.Count == 0))
            {
                ShowTreeItems(item.Path, item.Items);
            }
        }

        private void TreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            MenuItem item = e.NewValue as MenuItem;
            if (item != null)
            {
                SetDirectory(item.Path);
            }
        }
        
        #region address box
        
        private void SetAddresBoxText()
        {
            AddressTextBox.Text = FileManager.GetCurrentDirectory().FullName;
        }

        private void SetAddresBoxText(string text)
        {
            AddressTextBox.Text = text;
        }

        #endregion
        
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ListViewDirectory.ItemTemplate = (DataTemplate)ListViewDirectory.FindResource("ListViewSmallView");
            ListViewDirectory.ItemsPanel = (ItemsPanelTemplate)ListViewDirectory.FindResource("ListViewIconItemsPanel");
            ListViewDirectory.View = null;
        }
        private void Button_Click2(object sender, RoutedEventArgs e)
        {
            ListViewDirectory.ItemTemplate = (DataTemplate)ListViewDirectory.FindResource("ListViewBigView");
            ListViewDirectory.ItemsPanel = (ItemsPanelTemplate)ListViewDirectory.FindResource("ListViewIconItemsPanel");
            ListViewDirectory.View = null;
        }
        private void Button_Click3(object sender, RoutedEventArgs e)
        {
            ListViewDirectory.ItemTemplate = (DataTemplate)ListViewDirectory.FindResource("ListViewDetailsView");
            ListViewDirectory.ItemsPanel = (ItemsPanelTemplate)ListViewDirectory.FindResource("ListViewListItemsPanel");
            ListViewDirectory.View = (ViewBase)ListViewDirectory.FindResource("ListViewDetailsGridView");
        }
        private void Button_Click4(object sender, RoutedEventArgs e)
        {
            ListViewDirectory.ItemTemplate = (DataTemplate)ListViewDirectory.FindResource("ListViewBigView");
            ListViewDirectory.ItemsPanel = (ItemsPanelTemplate)ListViewDirectory.FindResource("ListViewIconItemsPanel");
            ListViewDirectory.View = null;
        }

        private void AddressTextBox_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Escape:
                    SetAddresBoxText(FileManager.GetCurrentDirectory().FullName);
                    ListViewDirectory.Focus();
                    break;
                case Key.Enter:
                    SetDirectory(AddressTextBox.Text);
                    ListViewDirectory.Focus();
                    break;
                default:
                    break;
            }
        }

        private void AddressTextBox_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            SetAddresBoxText(FileManager.GetCurrentDirectory().FullName);
        }

        private void ButtonRefresh_Click(object sender, RoutedEventArgs e)
        {
            SetDirectory(FileManager.GetCurrentDirectory().FullName);
        }

        private void ListViewDirectory_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var item = ListViewDirectory.SelectedItem;
            if (item != null)
            {
                if (item is DirectoryObject)
                    SetDirectory((item as DirectoryObject).File.FullName);
                else if (item is FileObject)
                {

                }
            }
        }

        private void EnableButton()
        {
            ButtonBack.IsEnabled = FileManager.CanDirectoryGoBack() ? true : false;
            ButtonForward.IsEnabled = FileManager.CanDirectoryGoForward() ? true : false;
            ButtonUp.IsEnabled = FileManager.CanDirectoryGoUp() ? true : false;
        }

        private void ButtonBack_Click(object sender, RoutedEventArgs e)
        {
            if (FileManager.CanDirectoryGoBack())
            {
                FileManager.DirectoryGoBack();
                SetDirectory(FileManager.GetCurrentDirectory().FullName);
            }
        }

        private void ButtonForward_Click(object sender, RoutedEventArgs e)
        {
            if (FileManager.CanDirectoryGoForward())
            {
                FileManager.DirectoryGoForward();
                SetDirectory(FileManager.GetCurrentDirectory().FullName);
            }
        }

        private void ButtonUp_Click(object sender, RoutedEventArgs e)
        {
            if (FileManager.CanDirectoryGoUp())
            {
                FileManager.DirectoryGoUp();
                SetDirectory(FileManager.GetCurrentDirectory().FullName);
            }
        }

        private void ButtonCreateFile_Click(object sender, RoutedEventArgs e)
        {
            AddEditWindow window = new AddEditWindow(0, FileManager.GetCurrentDirectory().FullName);
            window.Closed += (sender2, e2) =>
            {
                ButtonRefresh_Click(this, e);
            };
            window.ShowDialog();
        }

        private void ButtonCreateDirectory_Click(object sender, RoutedEventArgs e)
        {
            AddEditWindow window = new AddEditWindow(2, FileManager.GetCurrentDirectory().FullName);
            window.Closed += (sender2, e2) =>
            {
                ButtonRefresh_Click(this, e);
            };
            window.ShowDialog();
        }

        private void ButtonRename_Click(object sender, RoutedEventArgs e)
        {
            var item = ListViewDirectory.SelectedItem;
            if (item != null)
            {

                AddEditWindow window = null;
                if (item is FileObject)
                    window = new AddEditWindow(1, (item as FileObject).File.FullName);
                else
                    window = new AddEditWindow(3, (item as DirectoryObject).File.FullName);
                window.Closed += (sender2, e2) =>
                {
                    ButtonRefresh_Click(this, e);
                };
                window.ShowDialog();
            }
            
        }

        private void ButtonDelete_Click(object sender, RoutedEventArgs e)
        {
            var item = ListViewDirectory.SelectedItem;
            if (item != null)
            {
                if (item is FileObject)
                    FileManager.Delete((item as FileObject).File);
                else
                    FileManager.Delete((item as DirectoryObject).File);
                ButtonRefresh_Click(this, e);
            }
        }
    }

    public class TreeViewMarginConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var level = 0;
            if (value is DependencyObject)
            {
                var parent = VisualTreeHelper.GetParent(value as DependencyObject);
                while (!(parent is TreeView) && (parent != null))
                {
                    if (parent is TreeViewItem)
                        level++;
                    parent = VisualTreeHelper.GetParent(parent);
                }
            }
            return new Thickness(level * 15, 0, 0, 0); ;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return new Thickness(0, 0, 0, 0);
        }
    }

    public static class VisualTree

    {

        public static object GetParent(object child, Type parentType)

        {

            DependencyObject Item = child as DependencyObject;

            while (Item != null)

            {

                if (Item.GetType() == parentType)

                    return Item;

                Item = VisualTreeHelper.GetParent(Item);

            }

            return null;

        }
    }
    
}
