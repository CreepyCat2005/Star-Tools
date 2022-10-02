using System;
using System.Collections.Generic;
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

namespace Star_Citizen_Pfusch.Models
{
    /// <summary>
    /// Interaction logic for DragAndDropTarget.xaml
    /// </summary>
    public partial class DragAndDropTarget : UserControl
    {
        public DragAndDropTarget()
        {
            InitializeComponent();
            this.DataContext = this;

            Loaded += init;
        }
        private void init(object sender, RoutedEventArgs e)
        {
            ModuleImage.Source = new BitmapImage(new Uri("/Graphics/" + GetBetterType(type, SubType) + ".png", UriKind.Relative));
            TextBox.Text = GetBetterType(type,SubType);
        }
        private string GetBetterType(string type, string subType)
        {
            if (type.Equals("Turret") && subType.Equals("GunTurret"))
            {
                return "Mounting";
            }
            else
            {
                return type;
            }
        }

        protected override void OnDrop(DragEventArgs e)
        {
            base.OnDrop(e);
            if (e.Data.GetDataPresent("Object"))
            {
                DragAndDropItem item = e.Data.GetData("Object") as DragAndDropItem;

                if (ContentFrame.Content != null && ContentFrame.Content.GetType() == typeof(DragAndDropFrame))
                {
                    if (item.Type == type && item.Size.Equals(((DragAndDropFrame)ContentFrame.Content).Size))
                    {
                        ((DragAndDropFrame)ContentFrame.Content).ContentFrame.Content = new DragAndDropItem("Anzahl: " + ((DragAndDropFrame)ContentFrame.Content).Size.ToString(), item.Size, item.QtNameText, item.Type);
                    }
                }
                else
                {
                    if (item.Type == type && item.Size.Equals(Size))
                    {
                        ContentFrame.Content = new DragAndDropItem(item.QtGradeText, item.Size, item.QtNameText, item.Type);
                    }
                }

                e.Effects = DragDropEffects.Move;
            }
            if (e.Data.GetDataPresent("Frame"))
            {
                DragAndDropItem item = e.Data.GetData("Frame") as DragAndDropItem;

                if (item.Type == type && item.Size.Equals(Size))
                {
                    ContentFrame.Content = new DragAndDropFrame(item.Size, item.QtNameText, item.Type);
                    item = null;

                    e.Effects = DragDropEffects.Move;
                }
            }

            e.Handled = true;
        }
        public int Size { get; set; }
        public string type { get; set; }
        public string SubType { get; set; }

        private void Border_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            DropField.CornerRadius = new CornerRadius((this.ActualHeight + this.ActualWidth) / 2 / 20);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ContentFrame.Content = null;
        }
    }
}
