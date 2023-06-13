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
using System.Windows.Shapes;
using TRANSDICOM.Common;
using TRANSDICOM.ViewModel;

namespace TRANSDICOM.View
{
    /// <summary>
    /// EditSettingView.xaml の相互作用ロジック
    /// </summary>
    public partial class EditSettingView : Window
    {
        EditSettingViewModel viewModel;

        public EditSettingView(Setting _setting)
        {
            InitializeComponent();
            viewModel = new EditSettingViewModel(_setting);
            this.DataContext = viewModel;
            setEvents();
        }
        private void setEvents()
        {
            this.Loaded += (s, e) =>
            {
                //viewModel.OnLoaded();
            };
        }

    }
}
