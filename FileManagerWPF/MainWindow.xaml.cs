using System;
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

namespace FileManagerWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 

    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            MenuItem root = new MenuItem(@"C:\");
            MenuItem childItem1 = new MenuItem(@"C:\matlab");
            childItem1.Items.Add(new MenuItem(@"C:\matlab"));
            childItem1.Items.Add(new MenuItem(@"C:\matlab"));
            root.Items.Add(childItem1);
            root.Items.Add(new MenuItem(@"C:\matlab"));
            trvMenu.Items.Add(root);
            

            lb.Items.Add("aaa");
            lb.Items.Add("bbb");
            lb.Items.Add("ddd");
            lb.Items.Add("ddd");
            lb.Items.Add("aaa");
            lb.Items.Add("bbb");
            lb.Items.Add("ddd");
            lb.Items.Add("ddd");

        }
        public class MenuItem
        {
            public ObservableCollection<MenuItem> Items { get; set; }
            public FileManagerObject File { get; set; }

            public MenuItem(string Path)
            {
                this.Items = new ObservableCollection<MenuItem>();
                this.File = new FileObject(Path);
            }
        }

    }
}
