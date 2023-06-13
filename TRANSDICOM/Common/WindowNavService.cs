using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TRANSDICOM.View;
using TRANSDICOM.ViewModel;

namespace TRANSDICOM.Common
{
    class WindowNavService : IWindowService
    {
        EditSettingView editSettingView;
        public void CreateWindow(Setting _setting)
        {
            editSettingView = new EditSettingView(_setting)
            {
                //DataContext = new EditSettingViewModel(_setting)
            };
            _setting.CloseEditWindow -= _setting_CloseEditWindow;
            _setting.CloseEditWindow += _setting_CloseEditWindow;
            editSettingView.ShowDialog();
        }

        private void _setting_CloseEditWindow()
        {
            CloseWindow();
        }

        public void CloseWindow() 
        {
            editSettingView.Close();
            
        }
    }
}
