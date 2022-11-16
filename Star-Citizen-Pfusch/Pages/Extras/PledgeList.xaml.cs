using Star_Citizen_Pfusch.Functions;
using Star_Citizen_Pfusch.Models.UserControls;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media.Imaging;

namespace Star_Citizen_Pfusch.Pages.Extras
{
    /// <summary>
    /// Interaction logic for PledgeList.xaml
    /// </summary>
    public partial class PledgeList : Page
    {
        public PledgeList()
        {
            InitializeComponent();
            init();
        }

        private void init()
        {
            var list = LocalDataManager.GetPledgeItems();

            foreach (var item in list)
            {
                PledgeDisplayItem pledgeItem = new PledgeDisplayItem()
                {
                    ImageURI = new BitmapImage(new Uri(item.ImagePath)),
                    Height = 120,
                    HorizontalAlignment = HorizontalAlignment.Stretch,
                    PledgeName = item.name,
                    PledgeCreated = item.createdAt,
                    PledgeContains = item.contains
                };

                MasterListBox.Items.Add(pledgeItem);
            }
        }
    }
}
