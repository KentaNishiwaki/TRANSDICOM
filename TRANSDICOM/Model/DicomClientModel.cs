using CharLS;
using Dicom;
using Dicom.Network;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TRANSDICOM.Common;

namespace TRANSDICOM.Model
{
    public class DicomClientModel : ViewModelBase
    {
        Setting setting;
        public DicomClientModel(Setting _setting) 
        {
            setting = _setting;
        }

        int _CMoveCount = 0;
        public int CMoveCount { get { return _CMoveCount; } set { _CMoveCount = value; RaisePropertyChanged("CMoveCount"); } }

        int _CMoveCurrent = 0;
        public int CMoveCurrent { get { return _CMoveCurrent; } set { _CMoveCurrent = value; RaisePropertyChanged("CMoveCurrent"); } }

        int _CStoreCount = 0;
        public int CStoreCount { get { return _CStoreCount; } set { _CStoreCount = value; RaisePropertyChanged("CStoreCount"); } }

        int _CStoreCurrent = 0;
        public int CStoreCurrent { get { return _CStoreCurrent; } set { _CStoreCurrent = value; RaisePropertyChanged("CStoreCurrent"); } }

        public List<studiesInfo> GetStudies(string PatientID)
        {
            try
            { 
                var studies = new List<studiesInfo>();
                var client = new Dicom.Network.Client.DicomClient(setting.FromServerIP, setting.FromServerPort
                    , false, setting.FromCallingAETitle, setting.FromCalledAETitle);
                var cfind = DicomCFindRequest.CreateStudyQuery(PatientID);
                bool resComplete = false;
            
                cfind.OnResponseReceived = (DicomCFindRequest rq, DicomCFindResponse rp) =>
                {
                    if (rp.Status.State == DicomState.Pending)
                    {
                        if (rp.Dataset != null)
                        {
                            string studydate = string.Empty;
                            if (rp.Dataset.Contains(DicomTag.StudyDate))
                            {
                                studydate = rp.Dataset.GetValueOrDefault(DicomTag.StudyDate, 0, "");
                            }
                            string patientName = string.Empty;
                            if (rp.Dataset.Contains(DicomTag.PatientName))
                            {
                                patientName = rp.Dataset.GetValueOrDefault(DicomTag.PatientName, 0, "");
                            }
                            studies.Add(new studiesInfo(
                                rp.Dataset.GetValue<string>(DicomTag.StudyInstanceUID, 0)
                                , studydate
                                , patientName));
                            resComplete = false;
                        }
                    }
                    else if (rp.Status.State == DicomState.Success)
                    {
                        resComplete = true;
                    }
                    else if (rp.Status.State == DicomState.Failure)
                    {
                        resComplete = true;
                    }
                    else if (rp.Status.State == DicomState.Warning)
                    {
                        resComplete = false;
                    }
                    else if (rp.Status.State == DicomState.Cancel)
                    {
                        resComplete = true;
                    }

                };
                client.AddRequestAsync(cfind);
                client.StateChanged += (s, e) => {
                    if (e.NewState.ToString() == "COMPLETED")
                    {
                        resComplete = true;
                    }
                    else 
                    {
                    }
                };
                client.SendAsync();
                while (resComplete != true)
                {
                    // Log("waiting...waiting....");
                }
                return studies;
            } catch (Exception ex) { throw ex; }
        }
        public List<seriesInfo> GetSeries(string StudyInstanceUID)
        {
            var series = new List<seriesInfo>();
            var client = new Dicom.Network.Client.DicomClient(setting.FromServerIP, setting.FromServerPort
                , false, setting.FromCallingAETitle, setting.FromCalledAETitle);
            var cfind = DicomCFindRequest.CreateSeriesQuery(StudyInstanceUID);
            bool resComplete = false;
            cfind.OnResponseReceived = (DicomCFindRequest rq, DicomCFindResponse rp) =>
            {
                if (rp.Status.State == DicomState.Pending)
                {
                    if (rp.Dataset != null)
                    {
                        string modality = rp.Dataset.GetValue<string>(DicomTag.Modality, 0).ToString();
                        series.Add(new seriesInfo(rp.Dataset.GetValue<string>(DicomTag.StudyInstanceUID, 0).ToString()
                            , rp.Dataset.GetValue<string>(DicomTag.SeriesInstanceUID, 0).ToString()
                            , rp.Dataset.GetValue<string>(DicomTag.Modality, 0).ToString()));
                    }
                }
                else if (rp.Status.State == DicomState.Success)
                {
                    resComplete = true;
                }
                else if (rp.Status.State == DicomState.Failure)
                {
                    resComplete = true;
                }
                else if (rp.Status.State == DicomState.Warning)
                {
                    resComplete = false;
                }
                else if (rp.Status.State == DicomState.Cancel)
                {
                    resComplete = true;
                }
            };

            client.AddRequestAsync(cfind);
            client.StateChanged += (s, e) => {
                if (e.NewState.ToString() == "COMPLETED")
                {
                    resComplete = true;
                }
                else
                {
                }
            };
            client.SendAsync();
            while (resComplete != true)
            {
                // Log("waiting...waiting....");
            }
            return series;
        }

        public bool ExecCMove(seriesInfo series)
        {
            var client = new Dicom.Network.Client.DicomClient(setting.FromServerIP, setting.FromServerPort
                , false, setting.FromCallingAETitle, setting.FromCalledAETitle);
            var cmove = new DicomCMoveRequest(setting.DestinationAE, series.StudyInstanceUID, series.SeriesInstanceUID);
            bool moveComplete = false;
            cmove.OnResponseReceived += (DicomCMoveRequest request, DicomCMoveResponse response) =>
            {
                if (response.Status.State == DicomState.Pending)
                {
                    moveComplete = false;
                    CMoveCurrent = response.Completed;
                    if (response.Completed == 1)
                    {
                        CMoveCount = response.Remaining + 1;
                    }
                    
                }
                else if (response.Status.State == DicomState.Success)
                {
                    moveComplete = true;
                }
                else if (response.Status.State == DicomState.Failure)
                {
                    moveComplete = true;
                }
                else if (response.Status.State == DicomState.Warning)
                {
                    moveComplete = false;
                }
                else if (response.Status.State == DicomState.Cancel)
                {
                    moveComplete = true;
                }
            };
            client.AddRequestAsync(cmove);
            client.StateChanged += (s, e) => {
                if (e.NewState.ToString() == "COMPLETED")
                {
                    moveComplete = true;
                }
                else
                {
                }
            };
            client.SendAsync().ConfigureAwait(false);

            while (moveComplete != true)
            {
                // Log("waiting...waiting....");
            }
            return moveComplete;

        }

        public bool ExecCStoreCL()
        {
            try
            {
                var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "TRANSDICOM");
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);
                path = Path.Combine(path, "tmp");
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);


                CStoreCount = (new DirectoryInfo(path)).GetFiles("*.*", System.IO.SearchOption.AllDirectories).Length;
                foreach (var f in (new DirectoryInfo(path)).GetFiles("*.*", System.IO.SearchOption.AllDirectories))
                {
                    CStoreCurrent = CStoreCurrent + 1;
                    var client = new Dicom.Network.Client.DicomClient(setting.ToServerIP, setting.ToServerPort
                                    , false, setting.ToCalledAETitle, setting.ToCallingAETitle);
                    var cstore = new DicomCStoreRequest(f.FullName);
                    bool reqComplete = false;

                    client.AddRequestAsync(cstore);
                        client.StateChanged += (s, e) => {
                            if (e.NewState.ToString() == "COMPLETED")
                            {
                                reqComplete = true;
                            }
                        };
                    client.SendAsync().ConfigureAwait(false);

                    while (reqComplete != true)
                    {
                        // Log("waiting...waiting....");
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                string s = ex.Message;
                return false;
            }

        }
        public bool ClearCash()
        {
            try
            {
                var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "TRANSDICOM");
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);
                path = Path.Combine(path, "tmp");
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);

                if (!Directory.Exists(path))
                {
                    return false;
                }

                CashDelete(path);

                return true;
            }
            catch (Exception ex)
            {
                string s = ex.Message;
                return false;
            }

        }
        public void CashDelete(string targetDirectoryPath)
        {
            try
            {
                if (!Directory.Exists(targetDirectoryPath))
                {
                    return;
                }

                //ディレクトリ以外の全ファイルを削除
                string[] filePaths = Directory.GetFiles(targetDirectoryPath);
                foreach (string filePath in filePaths)
                {
                    File.SetAttributes(filePath, FileAttributes.Normal);
                    File.Delete(filePath);
                }

                //ディレクトリの中のディレクトリも再帰的に削除
                string[] directoryPaths = Directory.GetDirectories(targetDirectoryPath);
                foreach (string directoryPath in directoryPaths)
                {
                    CashDelete(directoryPath);
                }

                //中が空になったらディレクトリ自身も削除
                Directory.Delete(targetDirectoryPath, false);
            }
            catch (Exception ex)
            {
                string s = ex.Message;
            }

        }
    }
}
