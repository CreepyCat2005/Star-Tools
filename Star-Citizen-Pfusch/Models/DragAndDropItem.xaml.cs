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
    public partial class DragAndDropItem : UserControl
    {
        public DragAndDropItem()
        {
            InitializeComponent();
            this.DataContext = this;
        }

        public DragAndDropItem(string grade, int size, string name,string Type)
        {
            QtGradeText = grade;
            QtNameText = name;
            QtSizeText = "Size: " + size;
            Size = size;
            this.Type = Type;

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
                if (!Type.Equals("MissileRack"))
                {
                    data.SetData("Object", this);
                }
                else
                {
                    data.SetData("Frame", this);
                }

                DragDrop.DoDragDrop(this, data, DragDropEffects.Move);
            }
        }

        public string _id { get; set; }
        public int Size { get; set; }
        public string QtGradeText { get; set; }
        public string QtSizeText { get; set; }
        public string QtNameText { get; set; }
        public ModuleItem moduleItem { get; set; }
        public string Type { get; set; }

        private void Border_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            BackgroundBorder.CornerRadius = new CornerRadius((this.ActualHeight + this.ActualWidth) / 2 / 20);
            ModuleImage.Source = new BitmapImage(new Uri(@"/Graphics/" + Type + ".png", UriKind.Relative));
        }
    }
}
