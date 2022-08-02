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
    public partial class DragAndDropItem : UserControl
    {
        public DragAndDropItem()
        {
            InitializeComponent();
            this.DataContext = this;
        }

        public DragAndDropItem(string grade, string size, string name,ModuleTypeEnum type)
        {
            QtGradeText = grade;
            QtNameText = name;
            QtSizeText = size;
            this.type = type;

            InitializeComponent();
            ModuleImage.Source = new BitmapImage(new Uri(@"/Graphics/" + type + ".png", UriKind.Relative));
            this.DataContext = this;
        }
        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DataObject data = new DataObject();
                data.SetData("Object", this);

                DragDrop.DoDragDrop(this, data, DragDropEffects.Move);
            }
        }

        public string _id { get; set; }
        public string QtGradeText { get; set; }
        public string QtSizeText { get; set; }
        public string QtNameText { get; set; }
        public object moduleItem { get; set; }
        public ModuleTypeEnum type { get; set; }

        private void Border_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            BackgroundBorder.CornerRadius = new CornerRadius((this.ActualHeight + this.ActualWidth) / 2 / 20);
            ModuleImage.Source = new BitmapImage(new Uri(@"/Graphics/" + type + ".png", UriKind.Relative));
        }
    }
}
