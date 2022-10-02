using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Star_Citizen_Pfusch.Models.UserControls
{
    /// <summary>
    /// Interaction logic for ArrowFrame.xaml
    /// </summary>
    public partial class ArrowFrame : UserControl
    {
        public ImageSource Source { get; set; }
        public ModuleItem[] ModuleItems { get; set; }
        public ArrowFrame()
        {
            InitializeComponent();
            this.DataContext = this;
        }
    }
}
