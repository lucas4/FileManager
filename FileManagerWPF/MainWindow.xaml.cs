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
            MenuItem root = new MenuItem() { Title = "Menu" };
            MenuItem childItem1 = new MenuItem() { Title = "Child item #1" };
            childItem1.Items.Add(new MenuItem() { Title = "Child item #1.1" });
            childItem1.Items.Add(new MenuItem() { Title = "Child item #1.2" });
            root.Items.Add(childItem1);
            root.Items.Add(new MenuItem() { Title = "Child item #2" });
            trvMenu.Items.Add(root);


            lb.Items.Add("aaa");
            lb.Items.Add("bbb");
            lb.Items.Add("ddd");
            lb.Items.Add("ddd");
            lb.Items.Add("aaa");
            lb.Items.Add("bbb");
            lb.Items.Add("ddd");
            lb.Items.Add("ddd");



            //ListViewBack.ItemsPanel = (ItemsPanelTemplate)ListViewBack.FindResource("IconGridPanelTemplate");
            //ListViewBack.ItemTemplate = (DataTemplate)ListViewBack.FindResource("IconGridDataTemplate");





            InitializeListView(ListView1);
            ObservableCollection<string> List1 = new ObservableCollection<string>();
            List1.Add("John Smith");
            List1.Add("Michael Roth");
            List1.Add("Anna Johnson");
            List1.Add("James White");
            List1.Add("Elisabeth Young");
            List1.Add("John Smith2");
            List1.Add("Michael Roth2");
            List1.Add("Anna Johnson2");
            List1.Add("James White2");
            List1.Add("Elisabeth Young2");
            ListView1.ItemsSource = List1;
        }
        public class MenuItem
        {
            public MenuItem()
            {
                this.Items = new ObservableCollection<MenuItem>();
            }

            public string Title { get; set; }

            public ObservableCollection<MenuItem> Items { get; set; }
        }

        #region listview drag & drop
        void InitializeListView(ListView listView)

        {

            listView.AllowDrop = true;

            listView.PreviewMouseDown += new MouseButtonEventHandler(listView_PreviewMouseDown);
            listView.PreviewMouseUp += new MouseButtonEventHandler(listView_PreviewMouseUp);
            listView.PreviewMouseMove += new MouseEventHandler(listView_PreviewMouseMove);
            listView.LostMouseCapture += new MouseEventHandler(listView_LostMouseCapture);
            listView.DragEnter += new DragEventHandler(listView_DragEnter);
            listView.DragOver += new DragEventHandler(listView_DragOver);
            listView.Drop += new DragEventHandler(listView_Drop);
        }
        
        #region Data operations
        
        void SaveData(IList source, IDataObject target)
        {
            string Buffer = "";
            foreach (object Item in source)
                Buffer += (Buffer == "" ? "" : "\r\n") + "" + Item;
            target.SetData(typeof(string), Buffer);
        }
        
        bool CanLoadData(IDataObject source)
        {
            return source.GetDataPresent(typeof(string));
        }

        void LoadData(IDataObject source, ListView listView)
        {
            IList target = listView.ItemsSource as IList;
            if (target == null)
                return;
            string Buffer = (string)source.GetData(typeof(string));
            string[] Separators = new string[] { "\r\n" };
            string[] Items = Buffer.Split(Separators, StringSplitOptions.RemoveEmptyEntries);

            foreach (string Item in Items)
                if (!target.Contains(Item))
                    target.Add(Item);
        }
        
        #endregion

        #region Drop operations

        void MakeDropEffect(DragEventArgs e)
        {
            if (!CanLoadData(e.Data))
                e.Effects = DragDropEffects.None;
            else if ((e.KeyStates & DragDropKeyStates.AltKey) == DragDropKeyStates.AltKey)
                e.Effects = DragDropEffects.None;
            else
            {
                if ((e.KeyStates & DragDropKeyStates.ControlKey) == DragDropKeyStates.ControlKey)
                {
                    if ((e.AllowedEffects & DragDropEffects.Copy) == DragDropEffects.Copy)
                    {
                        e.Effects = DragDropEffects.Copy;
                        return;
                    }
                }
                if ((e.KeyStates & DragDropKeyStates.ShiftKey) == DragDropKeyStates.ShiftKey)
                {
                    if ((e.AllowedEffects & DragDropEffects.Move) == DragDropEffects.Move)
                    {
                        e.Effects = DragDropEffects.Move;
                        return;
                    }
                }

                if ((e.AllowedEffects & DragDropEffects.Move) == DragDropEffects.Move)

                {

                    e.Effects = DragDropEffects.Move;

                    return;

                }

                if ((e.AllowedEffects & DragDropEffects.Copy) == DragDropEffects.Copy)

                {

                    e.Effects = DragDropEffects.Copy;

                    return;

                }

                e.Effects = DragDropEffects.None;

            }

        }



        void listView_DragEnter(object sender, DragEventArgs e)

        {

            e.Handled = true;

            MakeDropEffect(e);

        }



        void listView_DragOver(object sender, DragEventArgs e)

        {

            listView_DragEnter(sender, e);

        }



        void listView_Drop(object sender, DragEventArgs e)

        {

            ListView listView = (ListView)sender;

            e.Handled = true;

            MakeDropEffect(e);

            if (e.Effects == DragDropEffects.Copy || e.Effects == DragDropEffects.Move)

                LoadData(e.Data, listView);

        }



        #endregion
        
        #region Drag start operation



        void StartDrag(ListView listView)

        {

            IList Selection = listView.SelectedItems;

            if (Selection.Count == 0)

                return;



            DataObject Buffer = new DataObject();

            SaveData(Selection, Buffer);



            DragDropEffects Result = DragDrop.DoDragDrop(listView, Buffer,

            DragDropEffects.Copy | DragDropEffects.Move);

            if (Result == DragDropEffects.Move)

            {

                IList Source = (IList)listView.ItemsSource;

                object[] DeletedItems = new object[Selection.Count];

                Selection.CopyTo(DeletedItems, 0);

                foreach (object Item in DeletedItems)

                    if (Source.Contains(Item))

                        Source.Remove(Item);

            }

        }



        #endregion
        
        #region Mouse events handling for both multiple selection and drag start



        void listView_LostMouseCapture(object sender, MouseEventArgs e)

        {

            Log("LostMouseCapture()");

            ListView listView = (ListView)sender;

            listView.Tag = null;

        }



        void listView_PreviewMouseMove(object sender, MouseEventArgs e)

        {

            ListView listView = (ListView)sender;

            if (!listView.IsMouseCaptured)

                return;



            e.Handled = true;



            Point P = e.GetPosition(listView);

            Log("" + PP.X + "/" + PP.Y + " - " + P.X + "/" + P.Y);



            int Limit = 4;

            if (P.X - Limit > PP.X ||

            P.X + Limit < PP.X ||

            P.Y - Limit > PP.Y ||

            P.Y + Limit < PP.Y)

            {

                listView.ReleaseMouseCapture();

                StartDrag(listView);

            }

        }



        Point PP; // Mouse position for last PreviewMouseDown event



        void listView_PreviewMouseDown(object sender, MouseButtonEventArgs e)

        {

            Log("PreviewMouseDown()");

            ListView listView = (ListView)sender;

            listView.Tag = null;



            PP = e.GetPosition(listView);



            ListViewItem Item = (ListViewItem)VisualTree.GetParent(

            e.OriginalSource, typeof(ListViewItem));

            if (Item == null)

                return;



            if (Item.IsSelected && listView.CaptureMouse())

            {

                Log("PreviewMouseDown() - Selected item mouse down.");

                e.Handled = true;

                listView.Tag = Item;

            }

        }



        void listView_PreviewMouseUp(object sender, MouseButtonEventArgs e)

        {

            Log("PreviewMouseUp()");

            ListView listView = (ListView)sender;



            ListViewItem Item = (ListViewItem)listView.Tag;

            listView.Tag = null;

            if (Item == null)

                return;



            if (!listView.IsMouseCaptured)

                return;



            e.Handled = true;

            listView.ReleaseMouseCapture();



            Log("PreviewMouseUp(): Item = " + Item);

            if (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl))

                Item.IsSelected = !Item.IsSelected;

            else

            {

                listView.SelectedItems.Clear();

                Item.IsSelected = true;

            }



            if (!Item.IsKeyboardFocused)

                Item.Focus();

        }



        #endregion
        
        void Log(object message)
        {
            System.Diagnostics.Debug.WriteLine(message);
        }
        #endregion
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
