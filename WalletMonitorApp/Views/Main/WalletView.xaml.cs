using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
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
using WalletMonitorApp.Helpers;
using WalletMonitorApp.Models;
using WalletMonitorApp.ViewModels;

namespace WalletMonitorApp.Views.Main
{
    /// <summary>
    /// Interaction logic for CalibrateView.xaml
    /// </summary>
    public partial class WalletView : UserControl
    {
        public WalletView()
        {
            InitializeComponent();
        }

        private void dataDhanged(object sender, RoutedEventArgs e)
        {

        }

        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            Process.Start(e.Uri.ToString());
        }

        private GridViewColumnHeader listViewSortCol = null;
        private SortAdorner listViewSortAdorner = null;
        private void columnSort(object sender, RoutedEventArgs e)
        {
            var vm = (MainViewModel)DataContext;
            GridViewColumnHeader column = sender as GridViewColumnHeader;
            string sortBy = column.Tag.ToString();
            if (listViewSortCol != null)
            {
                AdornerLayer.GetAdornerLayer(listViewSortCol).Remove(listViewSortAdorner);
                lstWallets.Items.SortDescriptions.Clear();
            }

            ListSortDirection newDir = ListSortDirection.Descending;
            if (listViewSortCol == column && listViewSortAdorner.Direction == newDir)
                newDir = ListSortDirection.Ascending;

            listViewSortCol = column;
            listViewSortAdorner = new SortAdorner(listViewSortCol, newDir);
            AdornerLayer.GetAdornerLayer(listViewSortCol).Add(listViewSortAdorner);
            List<Wallet> list = null;
            if (newDir == ListSortDirection.Ascending)
            {
                list = vm.WalletList.OrderBy(i => ReflectionHelper.GetPropertyValue(i, sortBy)).ToList<Wallet>();
            }
            else
            {
                list = vm.WalletList.OrderByDescending(i => ReflectionHelper.GetPropertyValue(i, sortBy)).ToList<Wallet>();
            }
            vm.SortBy = sortBy;
            vm.SortOrder = newDir;
            vm.WalletList = new ObservableCollection<Wallet>(list);
        }
    }
}
