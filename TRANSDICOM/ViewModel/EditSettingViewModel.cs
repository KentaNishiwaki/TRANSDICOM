using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using TRANSDICOM.Common;
using TRANSDICOM.Model;

namespace TRANSDICOM.ViewModel
{
    public class EditSettingViewModel : ViewModelBase
    {
        public Setting __setting;
        public Setting setting { get { return __setting; } set { __setting = value; RaisePropertyChanged("setting"); } }
        bool noEditChange = false;
        #region property
        string _LocalIP = "";  //= string.Empty;
        public string LocalIP { get { return _LocalIP; } set { _LocalIP = value; RaisePropertyChanged("LocalIP"); } }
    
        string _ErrMessege = string.Empty;
        public string ErrMessege { get { return _ErrMessege; } set { _ErrMessege = value; RaisePropertyChanged("ErrMessege"); } }

        string _ErrMessegeColor = "#0000FF";
        public string ErrMessegeColor { get { return _ErrMessegeColor; } set { _ErrMessegeColor = value; RaisePropertyChanged("ErrMessegeColor"); } }

        string _FromName = "";
        public string FromName { get { return _FromName; } set { _FromName = value; RaisePropertyChanged("FromName"); EditChange(); } }

        string _FromServerIP = "";
        public string FromServerIP { get { return _FromServerIP; } set { _FromServerIP = value; RaisePropertyChanged("FromServerIP"); EditChange(); } }
        int _FromServerPort = 0;
        public int FromServerPort { get { return _FromServerPort; } set { _FromServerPort = value; RaisePropertyChanged("FromServerPort"); EditChange(); } }
        string _FromCallingAETitle = "";
        public string FromCallingAETitle { get { return _FromCallingAETitle; } set { _FromCallingAETitle = value; RaisePropertyChanged("FromCallingAETitle"); EditChange(); } }
        string _FromCalledAETitle = "";
        public string FromCalledAETitle { get { return _FromCalledAETitle; } set { _FromCalledAETitle = value; RaisePropertyChanged("FromCalledAETitle"); EditChange(); } }
        
        string _DestinationName = "";
        public string DestinationName { get { return _DestinationName; } set { _DestinationName = value; RaisePropertyChanged("DestinationName"); EditChange(); } }

        int _DestinationPort = 104;
        public int DestinationPort { get { return _DestinationPort; } set { _DestinationPort = value; RaisePropertyChanged("DestinationPort"); EditChange(); } }

        string _DestinationAE = "";
        public string DestinationAE { get { return _DestinationAE; } set { _DestinationAE = value; RaisePropertyChanged("DestinationAE"); EditChange(); } }

        string _ToName = "";
        public string ToName { get { return _ToName; } set { _ToName = value; RaisePropertyChanged("ToName"); EditChange(); } }
        string _ToServerIP = "";
        public string ToServerIP { get { return _ToServerIP; } set { _ToServerIP = value; RaisePropertyChanged("ToServerIP"); EditChange(); } }
        int _ToServerPort = 0;
        public int ToServerPort { get { return _ToServerPort; } set { _ToServerPort = value; RaisePropertyChanged("ToServerPort"); EditChange(); } }
        string _ToCallingAETitle = "";
        public string ToCallingAETitle { get { return _ToCallingAETitle; } set { _ToCallingAETitle = value; RaisePropertyChanged("ToCallingAETitle"); EditChange(); } }
        string _ToCalledAETitle = "";
        public string ToCalledAETitle { get { return _ToCalledAETitle; } set { _ToCalledAETitle = value; RaisePropertyChanged("ToCalledAETitle"); EditChange(); } }
        bool _ButtonIsEnabled = true;  //= string.Empty;
        public bool ButtonIsEnabled { get { return _ButtonIsEnabled; } set { _ButtonIsEnabled = value; RaisePropertyChanged("ButtonIsEnabled"); } }

        Dictionary<string,string> _LstEchoMessege = new Dictionary<string, string>();  //= string.Empty;
        public Dictionary<string, string> LstEchoMessege { get { return _LstEchoMessege; } set { _LstEchoMessege = value; RaisePropertyChanged("LstEchoMessege"); } }

        #endregion property

        #region Command
        private DelegateCommand _SaveCommand;
        public DelegateCommand SaveCommand
        {
            get
            {
                if (_SaveCommand == null)
                {
                    _SaveCommand = new DelegateCommand(SaveCommandExecute);
                }

                return _SaveCommand;
            }
        }


        private DelegateCommand _CancelCommand;
        public DelegateCommand CancelCommand
        {
            get
            {
                if (_CancelCommand == null)
                {
                    _CancelCommand = new DelegateCommand(CancelCommandExecute);
                }

                return _CancelCommand;
            }
        }

        private DelegateCommand _FromAddCommand;
        public DelegateCommand FromAddCommand
        {
            get
            {
                if (_FromAddCommand == null)
                {
                    _FromAddCommand = new DelegateCommand(FromAddCommandExecute);
                }

                return _FromAddCommand;
            }
        }


        private DelegateCommand _FromDelCommand;
        public DelegateCommand FromDelCommand
        {
            get
            {
                if (_FromDelCommand == null)
                {
                    _FromDelCommand = new DelegateCommand(FromDelCommandExecute);
                }

                return _FromDelCommand;
            }
        }

        private DelegateCommand _DestinationAddCommand;
        public DelegateCommand DestinationAddCommand
        {
            get
            {
                if (_DestinationAddCommand == null)
                {
                    _DestinationAddCommand = new DelegateCommand(DestinationAddCommandExecute);
                }

                return _DestinationAddCommand;
            }
        }

        private DelegateCommand _DestinationDelCommand;
        public DelegateCommand DestinationDelCommand
        {
            get
            {
                if (_DestinationDelCommand == null)
                {
                    _DestinationDelCommand = new DelegateCommand(DestinationDelCommandExecute);
                }

                return _DestinationDelCommand;
            }
        }
        private DelegateCommand _ToAddCommand;
        public DelegateCommand ToAddCommand
        {
            get
            {
                if (_ToAddCommand == null)
                {
                    _ToAddCommand = new DelegateCommand(ToAddCommandExecute);
                }

                return _ToAddCommand;
            }
        }
        private DelegateCommand _ToDelCommand;
        public DelegateCommand ToDelCommand
        {
            get
            {
                if (_ToDelCommand == null)
                {
                    _ToDelCommand = new DelegateCommand(ToDelCommandExecute);
                }

                return _ToDelCommand;
            }
        }

        private DelegateCommand _FromEchoCommand;
        public DelegateCommand FromEchoCommand
        {
            get
            {
                if (_FromEchoCommand == null)
                {
                    _FromEchoCommand = new DelegateCommand(FromEchoCommandExecute);
                }

                return _FromEchoCommand;
            }
        }

        private DelegateCommand _ToEchoCommand;
        public DelegateCommand ToEchoCommand
        {
            get
            {
                if (_ToEchoCommand == null)
                {
                    _ToEchoCommand = new DelegateCommand(ToEchoCommandExecute);
                }

                return _ToEchoCommand;
            }
        }



        #endregion Command

        public EditSettingViewModel(Setting _setting)
        {
            setting = _setting;
            string hostname = Dns.GetHostName();
            IPAddress[] adrList = Dns.GetHostAddresses(hostname);
            StringBuilder strIP = new StringBuilder("@");
            foreach (IPAddress address in adrList)
            {
                if (address.IsIPv6UniqueLocal == false && address.IsIPv6Teredo == false && address.IsIPv6LinkLocal == false)
                {
                    strIP.Append(" / " + address.ToString());
                }
            }
            LocalIP = strIP.ToString().Replace("@ / ", "");

            SetEditValues("FromSelectIndex");
            SetEditValues("DestinationSelectIndex");
            SetEditValues("ToSelectIndex");

            setting.PropertyChanged += Setting_PropertyChanged;

            ButtonIsEnabled = false;
        }

        private void Setting_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            SetEditValues(e.PropertyName + "");
        }

        private void SetEditValues(string e)
        {
            noEditChange = true;
            ButtonIsEnabled = false;
            switch (e)
            {
                case "AssociationRequestTimeoutInMs":
                case "AssociationReleaseTimeoutInMs":
                case "AssociationLingerTimeoutInMs":
                    noEditChange = false;
                    EditChange();
                    break;
                case "FromSelectIndex":
                    if (setting.FromSelectIndex >= 0)
                    {
                        FromName = setting.FromList[setting.FromSelectIndex].FromName;
                        FromServerIP = setting.FromList[setting.FromSelectIndex].FromServerIP;
                        FromServerPort = setting.FromList[setting.FromSelectIndex].FromServerPort;
                        FromCallingAETitle = setting.FromList[setting.FromSelectIndex].FromCallingAETitle;
                        FromCalledAETitle = setting.FromList[setting.FromSelectIndex].FromCalledAETitle;
                    }
                    break;
                case "DestinationSelectIndex":
                    if (setting.DestinationSelectIndex >= 0)
                    {
                        DestinationName = setting.DestinationList[setting.DestinationSelectIndex].DestinationName;
                        DestinationPort = setting.DestinationList[setting.DestinationSelectIndex].DestinationPort;
                        DestinationAE = setting.DestinationList[setting.DestinationSelectIndex].DestinationAE;
                    }
                    break;
                case "ToSelectIndex":
                    if (setting.ToSelectIndex >= 0)
                    {
                        ToName = setting.ToList[setting.ToSelectIndex].ToName;
                        ToServerIP = setting.ToList[setting.ToSelectIndex].ToServerIP;
                        ToServerPort = setting.ToList[setting.ToSelectIndex].ToServerPort;
                        ToCallingAETitle = setting.ToList[setting.ToSelectIndex].ToCallingAETitle;
                        ToCalledAETitle = setting.ToList[setting.ToSelectIndex].ToCalledAETitle;
                    }
                    break;
            }
            noEditChange = false;
        }

        private void FromAddCommandExecute()
        {
            setting.FromList.Add(new Setting.PacsFrom());
            setting.FromSelectIndex = setting.FromList.Count - 1;
        }
        private void FromDelCommandExecute()
        {
            var newList = new List<Setting.PacsFrom>();
            int index = 0;
            foreach (var item in setting.FromList)
            {
                if (index != setting.FromSelectIndex)
                {
                    newList.Add(item);
                }
                index++;
            }
            setting.FromSelectIndex = 0;
            setting.FromList = newList;
            EditChange();
        }
        private void DestinationAddCommandExecute()
        {
            setting.DestinationList.Add(new Setting.PacsDestination());
            setting.DestinationSelectIndex = setting.DestinationList.Count - 1;
        }
        private void DestinationDelCommandExecute()
        {
            var newList = new List<Setting.PacsDestination>();
            int index = 0;
            foreach (var item in setting.DestinationList)
            {
                if (index != setting.DestinationSelectIndex)
                {
                    newList.Add(item);
                }
                index++;
            }
            setting.DestinationSelectIndex = 0;
            setting.DestinationList = newList;
            EditChange();

        }
        private void ToAddCommandExecute()
        {
            setting.ToList.Add(new Setting.PacsTo());
            setting.ToSelectIndex = setting.ToList.Count - 1;
        }
        private void ToDelCommandExecute()
        {
            var newList = new List<Setting.PacsTo>();
            int index = 0;
            foreach (var item in setting.ToList)
            {
                if (index != setting.ToSelectIndex)
                {
                    newList.Add(item);
                }
                index++;
            }
            setting.ToSelectIndex = 0;
            setting.ToList = newList;
            EditChange();
        }

        private void SaveCommandExecute()
        {
            setting.FromList[setting.FromSelectIndex].FromName = FromName;
            setting.FromList[setting.FromSelectIndex].FromServerIP = FromServerIP;
            setting.FromList[setting.FromSelectIndex].FromServerPort = FromServerPort;
            setting.FromList[setting.FromSelectIndex].FromCallingAETitle = FromCallingAETitle;
            setting.FromList[setting.FromSelectIndex].FromCalledAETitle = FromCalledAETitle;
            setting.DestinationList[setting.DestinationSelectIndex].DestinationName = DestinationName;
            setting.DestinationList[setting.DestinationSelectIndex].DestinationPort = DestinationPort;
            setting.DestinationList[setting.DestinationSelectIndex].DestinationAE = DestinationAE;
            setting.ToList[setting.ToSelectIndex].ToName = ToName;
            setting.ToList[setting.ToSelectIndex].ToServerIP = ToServerIP;
            setting.ToList[setting.ToSelectIndex].ToServerPort = ToServerPort;
            setting.ToList[setting.ToSelectIndex].ToCallingAETitle = ToCallingAETitle;
            setting.ToList[setting.ToSelectIndex].ToCalledAETitle = ToCalledAETitle;
            setting.Save();
            ButtonIsEnabled = false;
        }
        private void CancelCommandExecute()
        {
            setting.PropertyChanged -= Setting_PropertyChanged;
            setting.ExcCloseEditWindow();
        }
        private void EditChange()
        {
            if (noEditChange == false)
            {
                ButtonIsEnabled = true;
            }
            
        }

        private  void FromEchoCommandExecute()
        {
            ExcEcho(FromServerIP, FromServerPort, FromCalledAETitle, FromCallingAETitle);
        }
        private void ToEchoCommandExecute()
        {
            ExcEcho(ToServerIP, ToServerPort, ToCalledAETitle, ToCallingAETitle);
        }
        private async void ExcEcho(string ServerIP, int ServerPort ,string CalledAETitle, string CallingAETitle) 
        {
            try
            {
                ShowMessege("Connecting", false);

                DicomClientModel model = new DicomClientModel(setting);

                LstEchoMessege = new Dictionary<string, string>();
                Dictionary<string, string> newEcho = new Dictionary<string, string>();

                newEcho.Add("0", "0" + ": " + ServerIP + "(" + ServerPort.ToString() + ")");

                var lstEcho = await Task.Run<List<echomessege>>(() => model.Echo(
                            ServerIP, ServerPort, CalledAETitle, CallingAETitle));
                foreach (var echo in lstEcho)
                {
                    newEcho.Add(echo.Index.ToString(), echo.Index.ToString() + ": " + echo.Messege);
                }
                LstEchoMessege = newEcho;

                ShowMessege("Our tasks are done!", false);

            }
            catch (Exception ex)
            {
                ShowMessege(ex.Message, true);
            }
            finally
            {
            }

        }

        private void ShowMessege(string errmessege, bool err)
        {
            ErrMessege = errmessege;
            if (err == true)
            {
                ErrMessegeColor = "#FF0000";
            }else
            {
                ErrMessegeColor = "#0000FF";
            }
        }
    }
}
