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
using BL;
using BE;

namespace UI
{
    /// <summary>
    /// Interaction logic for NannyWPF.xaml
    /// </summary>
    public partial class NannyWPF : UserControl
    {
        BE.Nanny nanny;
        //BL.IBL bl = BL.BL_Factory.GetBLInstance();
        //BL.IBL bl;
        public NannyWPF()
        {
            InitializeComponent();
        }

        private void Categories_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Tab_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void nannyid_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            //try
            //{
            //    bl.AddNanny(nanny);
            //    nanny = new Nanny();
            //    Close();
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message);
            //}
        }

        private void U_Sub_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
