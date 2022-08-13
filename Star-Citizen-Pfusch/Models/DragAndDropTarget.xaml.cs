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

                if (ContentFrame.Content.GetType() == typeof(DragAndDropFrame))
                {
                    if (item.type == getSubType(type) && item.Size.Equals(((DragAndDropFrame)ContentFrame.Content).Size))
                    {
                        ((DragAndDropFrame)ContentFrame.Content).ContentFrame.Content = new DragAndDropItem(item.QtGradeText, item.Size, item.QtNameText, item.type);
                    }
                }
                else
                {
                    if (item.type == type && item.Size.Equals(Size))
                    {
                        ContentFrame.Content = new DragAndDropItem(item.QtGradeText, item.Size, item.QtNameText, item.type);
                    }
                }

                e.Effects = DragDropEffects.Move;
            }
            if (e.Data.GetDataPresent("Frame"))
            {
                DragAndDropItem item = e.Data.GetData("Frame") as DragAndDropItem;

                if (item.type == type && item.Size.Equals(Size))
                {
                    ContentFrame.Content = new DragAndDropFrame(item.Size, item.QtNameText, item.type);
                    item = null;

                    e.Effects = DragDropEffects.Move;
                }
            }

            e.Handled = true;
        }
        private ModuleTypeEnum getSubType(ModuleTypeEnum type)
        {
            switch (type)
            {
                case ModuleTypeEnum.Missile_Rack:
                    return ModuleTypeEnum.Missile;
                default:
                    return ModuleTypeEnum.Unknown;
            }
        }
        public string Text { get; set; }
        public int Size { get; set; }
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
