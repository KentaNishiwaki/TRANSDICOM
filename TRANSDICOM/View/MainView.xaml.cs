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
    /// MainView.xaml の相互作用ロジック
    /// </summary>
    public partial class MainView : Window
    {
        MainViewModel viewModel;
        string[] args;
        Setting setting = new Setting();
        public MainView(string[] _args)
        {
            InitializeComponent();
            args = _args;
            setting.GetSetting();
            viewModel = new MainViewModel(args, setting);
            this.DataContext = viewModel;
            setEvents();

        }

        private void setEvents()
        {
            this.Loaded += (s, e) =>
            {
                viewModel.OnLoaded();
            };
            this.btnCashPath.Click += (s, e) =>
            {
                string msgtext = viewModel.ShowCashPath();
                if (MessageBox.Show("You can copy the path by OK. \r\n" + msgtext, "Cash Path. (OK to copy),", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                { Clipboard.SetText(msgtext); }
            };
        }
    }
}

