using Dicom;
using Dicom.Imaging.Render;
using Dicom.Log;
using Dicom.Network;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using TRANSDICOM.Common;
using TRANSDICOM.Model;

namespace TRANSDICOM.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        Assembly assembly = Assembly.GetExecutingAssembly();
        private string[] args;
        public Setting __setting;
        public Setting setting { get { return __setting; } set { __setting = value; RaisePropertyChanged("setting"); } }

        #region property
        string _PatientID = "";  //= string.Empty;
        public string PatientID { get { return _PatientID; } set { _PatientID = value; RaisePropertyChanged("PatientID"); } }
        string _LocalIP = "";  //= string.Empty;
        public string LocalIP { get { return _LocalIP; } set { _LocalIP = value; RaisePropertyChanged("LocalIP"); } }

        string _ErrMessege = string.Empty;
        public string ErrMessege { get { return _ErrMessege; } set { _ErrMessege = value; RaisePropertyChanged("ErrMessege"); } }
        string _ErrMessegeColor = "#0000FF";
        public string ErrMessegeColor { get { return _ErrMessegeColor; } set { _ErrMessegeColor = value; RaisePropertyChanged("ErrMessegeColor"); } }
        public CurrentCount _currentCount;
        public CurrentCount currentCount { get { return _currentCount; } set { _currentCount = value; RaisePropertyChanged("currentCount"); } }
        
        Dictionary<string, string> _VLstStudy;
        public Dictionary<string, string> VLstStudy { get { return _VLstStudy; } set { _VLstStudy = value; RaisePropertyChanged("VLstStudy"); } }
        IList _selStudy;
        public IList selStudy { get { return _selStudy; } set { _selStudy = value; selStudyChanged(_selStudy); } }

        Dictionary<string, string> _VLstSeries;
        public Dictionary<string, string> VLstSeries { get { return _VLstSeries; } set { _VLstSeries = value; RaisePropertyChanged("VLstSeries"); } }
        IList _selSeries;
        public IList selSeries { get { return _selSeries; } set { _selSeries = value; selSeriesChanged(_selSeries); } }

        List<dicomImagesInfo> _PreviewImages;
        public List<dicomImagesInfo> PreviewImages { get { return _PreviewImages; } set { _PreviewImages = value; RaisePropertyChanged("PreviewImages"); } }

        bool _ButtonIsEnabled = true;  //= string.Empty;
        public bool ButtonIsEnabled { get { return _ButtonIsEnabled; } set { _ButtonIsEnabled = value; RaisePropertyChanged("ButtonIsEnabled"); } }
        #endregion property
        #region Command
        /// <summary>
        /// 実行ボタンのコマンド・変数
        /// </summary>
        private DelegateCommand _ExesCommand;
        /// <summary>
        /// 実行ボタンのコマンド・プロパティ
        /// </summary>
        public DelegateCommand ExecCommand
        {
            get
            {
                if (_ExesCommand == null)
                {
                    _ExesCommand = new DelegateCommand(ExecCommandExecute);
                }

                return _ExesCommand;
            }
        }
        
        /// <summary>
        /// 実行ボタンのコマンド・変数
        /// </summary>
        private DelegateCommand _FindStudiesExecCommand;
        /// <summary>
        /// 実行ボタンのコマンド・プロパティ
        /// </summary>
        public DelegateCommand FindStudiesExecCommand
        {
            get
            {
                if (_FindStudiesExecCommand == null)
                {
                    _FindStudiesExecCommand = new DelegateCommand(FindStudiesExecCommandExecute);
                }

                return _FindStudiesExecCommand;
            }
        }
        /// <summary>
        /// 実行ボタンのコマンド・変数
        /// </summary>
        private DelegateCommand _FindSeriesExecCommand;
        /// <summary>
        /// 実行ボタンのコマンド・プロパティ
        /// </summary>
        public DelegateCommand FindSeriesExecCommand
        {
            get
            {
                if (_FindSeriesExecCommand == null)
                {
                    _FindSeriesExecCommand = new DelegateCommand(FindSeriesExecCommandExecute);
                }

                return _FindSeriesExecCommand;
            }
        }
        /// <summary>
        /// 実行ボタンのコマンド・変数
        /// </summary>
        private DelegateCommand _StudiesExecCommand;
        /// <summary>
        /// 実行ボタンのコマンド・プロパティ
        /// </summary>
        public DelegateCommand StudiesExecCommand
        {
            get
            {
                if (_StudiesExecCommand == null)
                {
                    _StudiesExecCommand = new DelegateCommand(StudiesExecCommandExecute);
                }

                return _StudiesExecCommand;
            }
        }
        /// <summary>
        /// 実行ボタンのコマンド・変数
        /// </summary>
        private DelegateCommand _SeriesExecCommand;
        /// <summary>
        /// 実行ボタンのコマンド・プロパティ
        /// </summary>
        public DelegateCommand SeriesExecCommand
        {
            get
            {
                if (_SeriesExecCommand == null)
                {
                    _SeriesExecCommand = new DelegateCommand(SeriesExecCommandExecute);
                }

                return _SeriesExecCommand;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        private DelegateCommand _PreviewImagesExecCommand;
        /// <summary>
        /// 
        /// </summary>
        public DelegateCommand PreviewImagesExecCommand
        {
            get
            {
                if (_PreviewImagesExecCommand == null)
                {
                    _PreviewImagesExecCommand = new DelegateCommand(PreviewImagesExecCommandExecute);
                }

                return _PreviewImagesExecCommand;
            }
        }
        private DelegateCommand _ShowEditSettingExecCommand;
        public DelegateCommand ShowEditSettingExecCommand
        {
            get
            {
                if (_ShowEditSettingExecCommand == null)
                {
                    _ShowEditSettingExecCommand = new DelegateCommand(ShowEditSettingExecCommandExecute);
                }

                return _ShowEditSettingExecCommand;
            }
        }

        #endregion Command

        public MainViewModel(string[] _args, Setting _setting)
        {
            setSetting(_args, _setting);



            currentCount = new CurrentCount();

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
        }

        private void setSetting(string[] _args, Setting _setting)
        {
            setting = _setting;
            if (_args.Length >= 1)
            {
                PatientID = _args[0];
            }
        }

        public void OnLoaded()
        {
        }
        private void selStudyChanged(IList invalse)
        {
            foreach (KeyValuePair<string, string> item in invalse)
            {
                //setSeriesModality(item.Key);
            }
        }
        private void selSeriesChanged(IList invalse)
        {
            foreach (KeyValuePair<string, string> item in invalse)
            {
                //setSeriesModality(item.Key);
            }
        }

        private async void FindStudiesExecCommandExecute()
        {
            try
            {
                ButtonIsEnabled = false;

                ClearCounter();

                if (string.IsNullOrEmpty(PatientID))
                {
                    ShowMessege("Enter a PatientID",true);
                    return;
                }

                ShowMessege("Connecting",false);

                DicomClientModel model = new DicomClientModel(setting);

                List<dicomImagesInfo> previewImages = new List<dicomImagesInfo>();
                PreviewImages = previewImages;
                model.ClearCash();

                var lstStudies = await Task.Run<List<studiesInfo>>(() => model.GetStudies(PatientID));
                currentCount.StudiesCount = lstStudies.Count();
                if (lstStudies.Count == 0)
                {
                    ShowMessege("It was no Study!!", true);
                    return;
                }
                ShowMessege("Some Studies are found", false);

                var newStudy = new Dictionary<string, string>();


                foreach (var study in lstStudies)
                {
                    currentCount.StudiesCurrent++;
                    if (newStudy.ContainsKey(study.StudyInstanceUID) == false)
                        newStudy.Add(study.StudyInstanceUID, "(" + study.PatientName + "  " + study.StudyDate + ")   " + study.StudyInstanceUID);
                }
                VLstStudy = newStudy;



                ShowMessege("Our tasks are done!", false);
                ButtonIsEnabled = true;

            }
            catch (Exception ex)
            {
                ShowMessege(ex.Message,true);
            }
            finally
            {
                ButtonIsEnabled = true;
            }
        }

        private void ClearCounter()
        {
            currentCount.StudiesCount = 0;
            currentCount.StudiesCurrent = 0;
            currentCount.SeriesCount = 0;
            currentCount.SeriesCurrent = 0;
            currentCount.CMoveCount = 0;
            currentCount.CMoveCurrent = 0;
            currentCount.CStoreCount = 0;
            currentCount.CStoreCurrent = 0;
        }

        private async void FindSeriesExecCommandExecute()
        {
            try
            {
                ButtonIsEnabled = false;

                ClearCounter();

                if (string.IsNullOrEmpty(PatientID))
                {
                    ShowMessege("Enter a PatientID", true);
                    return;
                }


                ShowMessege("Connecting", false);

                if (selStudy is null || selStudy.Count == 0)
                {
                    ShowMessege("Select a StudyInstanceUID", true);
                    return;
                }

                DicomClientModel model = new DicomClientModel(setting);
                currentCount.StudiesCount = selStudy.Count;

                List<dicomImagesInfo> previewImages = new List<dicomImagesInfo>();
                PreviewImages = previewImages;
                model.ClearCash();

                var newSeries = new Dictionary<string, string>();

                foreach (KeyValuePair<string, string> study in selStudy)
                {
                    currentCount.StudiesCurrent++;
                    var lstSeries = await Task.Run<List<seriesInfo>>(() => model.GetSeries(study.Key));
                    currentCount.SeriesCount = lstSeries.Count();
                    currentCount.SeriesCurrent = 0;
                    if (lstSeries.Count == 0)
                    {
                        ShowMessege("It was no Series!!", true);
                    }

                    foreach (var series in lstSeries)
                    {
                        ShowMessege("Some Studies are found", false);
                        currentCount.SeriesCurrent++;
                        
                        if (newSeries.ContainsKey(study.Key + ":" + series.SeriesInstanceUID) == false)
                            newSeries.Add(study.Key+ ":" + series.SeriesInstanceUID, "(" + series.Modality + ")   " + series.SeriesInstanceUID);
                    }
                }
                VLstSeries = newSeries;

                ShowMessege("Our tasks are done!", false);

            }
            catch (Exception ex)
            {
                ShowMessege(ex.Message, true);
            }
            finally
            {
                ButtonIsEnabled = true;
            }
        }

        

        private async void ExecCommandExecute()
        {
            IDicomServer? dServer = null;
            try
            {
                ButtonIsEnabled = false;

                ClearCounter();

                if (string.IsNullOrEmpty(PatientID))
                {
                    ShowMessege("Enter a PatientID", true);
                    return;
                }

                ShowMessege("Connecting", false);

                DicomServerModel s = new DicomServerModel(setting);
                dServer = s.Start();

                DicomClientModel model = new DicomClientModel(setting);
                model.PropertyChanged += (object? sender, System.ComponentModel.PropertyChangedEventArgs e) =>
                {
                    if (sender is null)
                    {
                        return;
                    }
                    var v = (DicomClientModel)sender;
                    switch (e.PropertyName)
                    {
                        case "CMoveCount":
                                currentCount.CMoveCount = v.CMoveCount;
                            break;
                        case "CMoveCurrent":
                                currentCount.CMoveCurrent = v.CMoveCurrent;
                            break;
                        case "CStoreCount":
                                currentCount.CStoreCount = v.CStoreCount;
                            break;
                        case "CStoreCurrent":
                                currentCount.CStoreCurrent = v.CStoreCurrent;
                            break;
                    }
                };

                List<dicomImagesInfo> previewImages = new List<dicomImagesInfo>();
                PreviewImages = previewImages;
                model.ClearCash();

                var lstStudies = await Task.Run<List<studiesInfo>>(() => model.GetStudies(PatientID));
                currentCount.StudiesCount = lstStudies.Count();
                if (lstStudies.Count == 0)
                {
                    ShowMessege("It was no Study!!", true);
                    return;
                }
                ShowMessege("Our tasks are running...", false);

                foreach (var study in lstStudies)
                {
                    currentCount.StudiesCurrent++;
                    var lstSeries = await Task.Run<List<seriesInfo>>(() => model.GetSeries(study.StudyInstanceUID));
                    currentCount.SeriesCount = lstSeries.Count();
                    currentCount.SeriesCurrent = 0;
                    if (lstSeries.Count == 0)
                    {
                        ShowMessege("It was no Series!!", true);
                    }
                    foreach (var series in lstSeries)
                    {
                        currentCount.SeriesCurrent++;
                        var rc = await Task.Run<bool>(() => model.ExecCMove(series));
                    }
                }

                await Task.Run<bool>(() => model.ExecCStoreCL());



                ShowMessege("Our tasks are done!", false);

            }
            catch (Exception ex)
            {
                ShowMessege(ex.Message, true);
            }
            finally 
            {
                ButtonIsEnabled = true;
                if (dServer is not null)
                {
                    dServer.Stop();
                    dServer.Dispose();
                }
            }


        }
        private async void StudiesExecCommandExecute()
        {
            IDicomServer? dServer = null;
            try
            {
                ButtonIsEnabled = false;

                ClearCounter();

                if (string.IsNullOrEmpty(PatientID))
                {
                    ShowMessege("Enter a PatientID", true);
                    return;
                }

                ShowMessege("Connecting", false);

                DicomServerModel s = new DicomServerModel(setting);
                dServer = s.Start();

                DicomClientModel model = new DicomClientModel(setting);
                model.PropertyChanged += (object? sender, System.ComponentModel.PropertyChangedEventArgs e) =>
                {
                    if (sender is null)
                    {
                        return;
                    }
                    var v = (DicomClientModel)sender;
                    switch (e.PropertyName)
                    {
                        case "CMoveCount":
                            currentCount.CMoveCount = v.CMoveCount;
                            break;
                        case "CMoveCurrent":
                            currentCount.CMoveCurrent = v.CMoveCurrent;
                            break;
                        case "CStoreCount":
                            currentCount.CStoreCount = v.CStoreCount;
                            break;
                        case "CStoreCurrent":
                            currentCount.CStoreCurrent = v.CStoreCurrent;
                            break;
                    }
                };

                List<dicomImagesInfo> previewImages = new List<dicomImagesInfo>();
                PreviewImages = previewImages;
                model.ClearCash();

                if (selStudy is null || selStudy.Count == 0)
                {
                    ShowMessege("Select a StudyInstanceUID!!", true);
                    return;
                }

                currentCount.StudiesCount = selStudy.Count;

                ShowMessege("Our tasks are running...", false);

                foreach (KeyValuePair<string, string> study in selStudy)
                {
                    currentCount.StudiesCurrent++;
                    var lstSeries = await Task.Run<List<seriesInfo>>(() => model.GetSeries(study.Key));
                    currentCount.SeriesCount = lstSeries.Count();
                    currentCount.SeriesCurrent = 0;
                    if (lstSeries.Count == 0)
                    {
                        ShowMessege("It was no Series!!", true);
                    }
                    foreach (var series in lstSeries)
                    {
                        currentCount.SeriesCurrent++;
                        var rc = await Task.Run<bool>(() => model.ExecCMove(series));
                    }
                }

                await Task.Run<bool>(() => model.ExecCStoreCL());



                ShowMessege("Our tasks are done!", false);

            }
            catch (Exception ex)
            {
                ShowMessege(ex.Message, true);
            }
            finally
            {
                ButtonIsEnabled = true;
                if (dServer is not null)
                {
                    dServer.Stop();
                    dServer.Dispose();
                }
            }


        }
        private async void SeriesExecCommandExecute()
        {
            IDicomServer? dServer = null;
            try
            {
                ButtonIsEnabled = false;

                DicomClientModel model = new DicomClientModel(setting);
                model.PropertyChanged += (object? sender, System.ComponentModel.PropertyChangedEventArgs e) =>
                {
                    if (sender is null)
                    {
                        return;
                    }
                    var v = (DicomClientModel)sender;
                    switch (e.PropertyName)
                    {
                        case "CMoveCount":
                            currentCount.CMoveCount = v.CMoveCount;
                            break;
                        case "CMoveCurrent":
                            currentCount.CMoveCurrent = v.CMoveCurrent;
                            break;
                        case "CStoreCount":
                            currentCount.CStoreCount = v.CStoreCount;
                            break;
                        case "CStoreCurrent":
                            currentCount.CStoreCurrent = v.CStoreCurrent;
                            break;
                    }
                };

                if (PreviewImages == null || PreviewImages.Count == 0)
                { 
                    ClearCounter();

                    if (string.IsNullOrEmpty(PatientID))
                    {
                        ShowMessege("Enter a PatientID", true);
                        return;
                    }


                    if (selSeries is null || selSeries.Count == 0)
                    {
                        ShowMessege("Select a SeriesInstanceUID", true);
                        return;
                    }

                    ShowMessege("Connecting", false);

                    DicomServerModel s = new DicomServerModel(setting);
                    dServer = s.Start();

                    List<dicomImagesInfo> previewImages = new List<dicomImagesInfo>();
                    PreviewImages = previewImages;
                    model.ClearCash();

                    ShowMessege("Our tasks are running...", false);
                    currentCount.SeriesCount = selSeries.Count;
                    foreach (KeyValuePair<string, string> series in selSeries)
                    {
                        currentCount.SeriesCurrent++;
                        string[] v = series.Key.Split(':');
                        var seriesinfo = new seriesInfo(v[0], v[1], "");
                        var rc = await Task.Run<bool>(() => model.ExecCMove(seriesinfo));
                    }

                }

                ShowMessege("Our tasks are running...", false);
                await Task.Run<bool>(() => model.ExecCStoreCL());

                ShowMessege("Our tasks are done!", false);

            }
            catch (Exception ex)
            {
                ShowMessege(ex.Message, true);
            }
            finally
            {
                ButtonIsEnabled = true;
                if (dServer is not null)
                {
                    dServer.Stop();
                    dServer.Dispose();
                }
            }


        }


        
        private async void PreviewImagesExecCommandExecute()
        {
            IDicomServer? dServer = null;
            try
            {
                ButtonIsEnabled = false;

                ClearCounter();

                if (string.IsNullOrEmpty(PatientID))
                {
                    ShowMessege("Enter a PatientID", true);
                    return;
                }


                if (selSeries is null || selSeries.Count == 0)
                {
                    ShowMessege("Select a SeriesInstanceUID", true);
                    return;
                }

                ShowMessege("Connecting", false);

                DicomServerModel s = new DicomServerModel(setting);
                dServer = s.Start();

                DicomClientModel model = new DicomClientModel(setting);
                model.PropertyChanged += (object? sender, System.ComponentModel.PropertyChangedEventArgs e) =>
                {
                    if (sender is null)
                    {
                        return;
                    }
                    var v = (DicomClientModel)sender;
                    switch (e.PropertyName)
                    {
                        case "CMoveCount":
                            currentCount.CMoveCount = v.CMoveCount;
                            break;
                        case "CMoveCurrent":
                            currentCount.CMoveCurrent = v.CMoveCurrent;
                            break;
                        case "CStoreCount":
                            currentCount.CStoreCount = v.CStoreCount;
                            break;
                        case "CStoreCurrent":
                            currentCount.CStoreCurrent = v.CStoreCurrent;
                            break;
                    }
                };

                List<dicomImagesInfo> previewImages = new List<dicomImagesInfo>();
                PreviewImages = previewImages;
                model.ClearCash();

                ShowMessege("Our tasks are running...", false);
                currentCount.SeriesCount = selSeries.Count;
                foreach (KeyValuePair<string, string> series in selSeries)
                {
                    currentCount.SeriesCurrent++;
                    string[] v = series.Key.Split(':');
                    var seriesinfo = new seriesInfo(v[0], v[1], "");
                    var rc = await Task.Run<bool>(() => model.ExecCMove(seriesinfo));
                }

                string cashPah = model.GetCashPath();
                previewImages = new List<dicomImagesInfo>();
                model.SetPreviewImages(cashPah, ref previewImages);
                PreviewImages = previewImages;

                ShowMessege("Our tasks are done!", false);

            }
            catch (Exception ex)
            {
                ShowMessege(ex.Message, true);
            }
            finally
            {
                ButtonIsEnabled = true;
                if (dServer is not null)
                {
                    dServer.Stop();
                    dServer.Dispose();
                }
            }
        }
        public string ShowCashPath()
        {
            DicomClientModel model = new DicomClientModel(setting);
            return model.GetCashPath();
        }
        private void ShowEditSettingExecCommandExecute()
        {
            WindowNavService windowNav = new WindowNavService();
            IWindowService windowService = windowNav;
            windowService.CreateWindow(setting);
            setting.GetSetting();
        }
        private void ShowMessege(string errmessege, bool err)
        {
            ErrMessege = errmessege;
            if (err == true)
            {
                ErrMessegeColor = "#FF0000";
            }
            else
            {
                ErrMessegeColor = "#0000FF";
            }
        }

    }
}
