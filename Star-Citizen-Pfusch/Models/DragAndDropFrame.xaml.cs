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
    /// Interaction logic for DragAndDropItem.xaml
    /// </summary>
    public partial class DragAndDropFrame : UserControl
    {
        public DragAndDropFrame()
        {
            InitializeComponent();
            this.DataContext = this;
        }

        public DragAndDropFrame(int size, string name,ModuleTypeEnum type)
        {
            this.ModuleName = name;
            this.Type = type;
            this.Size = size;

            InitializeComponent();
            ModuleImage.Source = new BitmapImage(new Uri(@"/Graphics/" + Type + ".png", UriKind.Relative));
            this.DataContext = this;
        }
        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DataObject data = new DataObject();
                data.SetData("Frame", this);

                DragDrop.DoDragDrop(this, data, DragDropEffects.Move);
            }
        }

        public string _id { get; set; }
        public int Size { get; set; }
        public string ModuleName { get; set; }
        public ModuleTypeEnum Type { get; set; }

        private void Border_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            BackgroundBorder.CornerRadius = new CornerRadius((this.ActualHeight + this.ActualWidth) / 2 / 20);
            DropField.CornerRadius = new CornerRadius((this.ActualHeight + this.ActualWidth) / 2 / 20);
            ModuleImage.Source = new BitmapImage(new Uri("/Graphics/" + Type + ".png", UriKind.Relative));
        }
    }
}
