using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Star_Citizen_Pfusch.Pages.Integration;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using System.Net.Http;

namespace Star_Citizen_Pfusch.Models.UserControls
{
    /// <summary>
    /// Interaction logic for FleetyardOrgaContainer.xaml
    /// </summary>
    public partial class OrgaContainer : UserControl
    {
        private Frame ContentFrame;
        private OrgaMenu OrgaMenu;
        public OrgaContainer(FleetYardOrgaItem item, Frame ContentFrame)
        {
            this.ContentFrame = ContentFrame;

            InitializeComponent();
            init(item);

            this.MouseLeftButtonUp += FleetyardOrgaContainer_MouseLeftButtonUp;
            this.MouseEnter += FleetyardOrgaContainer_MouseEnter;
            this.MouseLeave += FleetyardOrgaContainer_MouseLeave;

        }
        private async void init(FleetYardOrgaItem item)
        {
            HttpClient client = new HttpClient();

            MemoryStream stream = new MemoryStream(await client.GetByteArrayAsync(item.logo));
            ImageSource source = BitmapFrame.Create(stream, BitmapCreateOptions.None, BitmapCacheOption.OnLoad);
            OrgaImage.Source = source;
            OrgaName.Text = item.name;
        }

        private void FleetyardOrgaContainer_MouseLeave(object sender, MouseEventArgs e)
        {
            this.Cursor = Cursors.Arrow;
        }

        private void FleetyardOrgaContainer_MouseEnter(object sender, MouseEventArgs e)
        {
            this.Cursor = Cursors.Hand;
        }

        private void FleetyardOrgaContainer_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
           if (OrgaMenu == null) OrgaMenu = new OrgaMenu(OrgaImage.Source);
            ContentFrame.Navigate(OrgaMenu);
        }
        private void Border_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            ((Border)sender).CornerRadius = new CornerRadius((this.ActualHeight + this.ActualWidth) / 2 / 20);
        }
    }
}
