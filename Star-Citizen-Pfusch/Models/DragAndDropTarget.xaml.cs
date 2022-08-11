using Star_Citizen_Pfusch.Models.Enums;
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
        }

        protected override void OnDrop(DragEventArgs e)
        {
            base.OnDrop(e);
            if (e.Data.GetDataPresent("Object"))
            {
                DragAndDropItem item = e.Data.GetData("Object") as DragAndDropItem;

                if (item.type == type && item.QtSizeText.Equals(Size))
                {
                    ContentFrame.Content = new DragAndDropItem(item.QtGradeText, item.QtSizeText, item.QtNameText, item.type);
                    item = null;

                    e.Effects = DragDropEffects.Move;
                }
            }

            e.Handled = true;
        }
        public string Text { get; set; }
        public string Size { get; set; }
        public ModuleTypeEnum type { get; set; }

        private void Border_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            DropField.CornerRadius = new CornerRadius((this.ActualHeight + this.ActualWidth) / 2 / 20);
            ModuleImage.Source = new BitmapImage(new Uri("/Graphics/" + type + ".png", UriKind.Relative));
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ContentFrame.Content = null;
        }
    }
}
