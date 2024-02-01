using CharLS;
using Dicom;
using Dicom.Imaging;
using Dicom.Network;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
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
            useTls = false;
        }

        int _CMoveCount = 0;
        public int CMoveCount { get { return _CMoveCount; } set { _CMoveCount = value; RaisePropertyChanged("CMoveCount"); } }

        int _CMoveCurrent = 0;
        public int CMoveCurrent { get { return _CMoveCurrent; } set { _CMoveCurrent = value; RaisePropertyChanged("CMoveCurrent"); } }

        int _CStoreCount = 0;
        public int CStoreCount { get { return _CStoreCount; } set { _CStoreCount = value; RaisePropertyChanged("CStoreCount"); } }

        int _CStoreCurrent = 0;
        public int CStoreCurrent { get { return _CStoreCurrent; } set { _CStoreCurrent = value; RaisePropertyChanged("CStoreCurrent"); } }


        bool useTls = false;

        public List<studiesInfo> GetStudies(string PatientID)
        {
            try
            {
                List<echomessege> status = new List<echomessege>();
                int index = 1;

                var studies = new List<studiesInfo>();
                var client = new Dicom.Network.Client.DicomClient(setting.FromList[setting.FromSelectIndex].FromServerIP, setting.FromList[setting.FromSelectIndex].FromServerPort
                    , useTls, setting.FromList[setting.FromSelectIndex].FromCallingAETitle, setting.FromList[setting.FromSelectIndex].FromCalledAETitle
                    , setting.AssociationRequestTimeoutInMs, setting.AssociationReleaseTimeoutInMs, setting.AssociationLingerTimeoutInMs);

                var cfind = DicomCFindRequest.CreateStudyQuery(PatientID);
                cfind.Dataset.Remove(DicomTag.IssuerOfPatientID);
                cfind.Dataset.Remove(DicomTag.StudyDescription);
                cfind.Dataset.Remove(DicomTag.NumberOfStudyRelatedSeries);
                cfind.Dataset.Remove(DicomTag.NumberOfStudyRelatedInstances);
                bool resComplete = false;

                string strErr = string.Empty;
                cfind.OnResponseReceived = (DicomCFindRequest rq, DicomCFindResponse rp) =>
                {
                    status.Add(new echomessege(index, rp.Status.ToString() + ":Comment:" + rp.Status.ErrorComment + "Description:" + rp.Status.Description));
                    index++;
                    if (rp.Status.State == DicomState.Pending)
                    {
                        if (rp.Dataset != null)
                        {
                            string studydate = string.Empty;
                            if (rp.Dataset.Contains(DicomTag.StudyDate))
                            {
                                studydate = rp.Dataset.GetValueOrDefault(DicomTag.StudyDate, 0, "");
                            }
                            if (rp.Dataset.Contains(DicomTag.ModalitiesInStudy))
                            {
                                studydate = studydate + " : " + rp.Dataset.GetValueOrDefault(DicomTag.ModalitiesInStudy, 0, "");
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
                        //strErr = "Comment:" + rp.Status.ErrorComment + "\n"
                        //        + "Description:" + rp.Status.Description;
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
                    status.Add(new echomessege(index, e.NewState.ToString()));
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

                if (string.IsNullOrEmpty(strErr) == false)
                {
                    foreach (var s in status)
                    {
                        strErr = strErr + s.Messege + "\r\n";
                    }
                    throw new Exception(strErr);
                }
                return studies;
            } catch (Exception ex) { throw ex; }
        }
        public List<seriesInfo> GetSeries(string StudyInstanceUID)
        {
            var series = new List<seriesInfo>();
            var client = new Dicom.Network.Client.DicomClient(setting.FromList[setting.FromSelectIndex].FromServerIP, setting.FromList[setting.FromSelectIndex].FromServerPort
                , useTls, setting.FromList[setting.FromSelectIndex].FromCallingAETitle, setting.FromList[setting.FromSelectIndex].FromCalledAETitle
                , setting.AssociationRequestTimeoutInMs, setting.AssociationReleaseTimeoutInMs, setting.AssociationLingerTimeoutInMs);
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
            var client = new Dicom.Network.Client.DicomClient(setting.FromList[setting.FromSelectIndex].FromServerIP, setting.FromList[setting.FromSelectIndex].FromServerPort
                , useTls, setting.FromList[setting.FromSelectIndex].FromCallingAETitle, setting.FromList[setting.FromSelectIndex].FromCalledAETitle
                , setting.AssociationRequestTimeoutInMs, setting.AssociationReleaseTimeoutInMs, setting.AssociationLingerTimeoutInMs);
            var cmove = new DicomCMoveRequest(setting.DestinationList[setting.DestinationSelectIndex].DestinationAE, series.StudyInstanceUID, series.SeriesInstanceUID);
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


                CStoreCount = (new DirectoryInfo(path)).GetFiles("*.dcm", System.IO.SearchOption.AllDirectories).Length;
                foreach (var f in (new DirectoryInfo(path)).GetFiles("*.dcm", System.IO.SearchOption.AllDirectories))
                {
                    CStoreCurrent = CStoreCurrent + 1;
                    var client = new Dicom.Network.Client.DicomClient(setting.ToList[setting.ToSelectIndex].ToServerIP, setting.ToList[setting.ToSelectIndex].ToServerPort
                                    , useTls, setting.ToList[setting.ToSelectIndex].ToCalledAETitle, setting.ToList[setting.ToSelectIndex].ToCallingAETitle
                                    , setting.AssociationRequestTimeoutInMs, setting.AssociationReleaseTimeoutInMs, setting.AssociationLingerTimeoutInMs);
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
        public string GetCashPath() 
        {
            try
            {
                var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "TRANSDICOM");
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);
                path = Path.Combine(path, "tmp");
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);

                return path;
            }
            catch (Exception ex)
            {
                string s = ex.Message;
                return "";
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

        internal bool SetPreviewImages(string cashPah, ref List<dicomImagesInfo> images)
        {
            try
            {

                foreach (var f in (new DirectoryInfo(cashPah)).GetFiles("*.dcm", System.IO.SearchOption.AllDirectories))
                {
                    if (f.Exists)
                    {
                        try
                        { 
                            var file = DicomFile.Open(f.FullName, FileReadOption.ReadAll);
                            var dicom = new Dicom.Imaging.DicomImage(file.Dataset);
                            if (dicom.NumberOfFrames != 1)
                            {
                                throw new Exception();
                            }
                            else
                            {
                                var bytes = (dicom.RenderImage() as RawImage).AsBytes();
                                
                                var bmp = SixLabors.ImageSharp.Image.LoadPixelData<SixLabors.ImageSharp.PixelFormats.Bgra32>(SixLabors.ImageSharp.Configuration.Default, bytes, dicom.Width, dicom.Height);
                                string InstanceNumber = "";
                                if (file.Dataset.GetValue<string>(DicomTag.InstanceNumber, 0).IndexOf('\0') >= 0)
                                {
                                    InstanceNumber = file.Dataset.GetValue<string>(DicomTag.InstanceNumber, 0)[0] + "";
                                }
                                else
                                {
                                    InstanceNumber = file.Dataset.GetValue<string>(DicomTag.InstanceNumber, 0);
                                }
                                int intInstanceNumber =0;
                                int.TryParse(InstanceNumber ,out intInstanceNumber);

                                using (var stream = new MemoryStream())
                                {
                                    var en = new SixLabors.ImageSharp.Formats.Bmp.BmpEncoder();
                                    bmp.Save(stream, en);
                                    images.Add(new dicomImagesInfo(intInstanceNumber, InstanceNumber, stream.GetBuffer()));
                                }
                                //string filepath = "";
                                //filepath = Path.Combine(cashPah, InstanceNumber + ".bmp");
                                //using (var stream = new FileStream(filepath, FileMode.OpenOrCreate))
                                //{
                                //    var en = new SixLabors.ImageSharp.Formats.Bmp.BmpEncoder();
                                //    bmp.Save(stream, en);
                                //}
                                //using (var stream = new FileStream(filepath, FileMode.Open))
                                //{
                                //    byte[] bs = new byte[stream.Length];
                                //    stream.Read(bs, 0, bs.Length);
                                //    images.Add(new dicomImagesInfo(intInstanceNumber, InstanceNumber, bs));
                                //}

                            }
                        } catch (Exception ex) 
                        {
                            try
                            {
                                var fileNonImage = DicomFile.Open(f.FullName, FileReadOption.ReadAll);
                                string InstanceNumber = "";
                                int intInstanceNumber = -1;
                                int.TryParse(InstanceNumber, out intInstanceNumber);
                                //var dicomNonImage = new Dicom.Imaging.DicomImage(fileNonImage.Dataset);
                                CreateImageModel cm = new CreateImageModel();
                                images.Add(new dicomImagesInfo(intInstanceNumber, InstanceNumber, cm.GetBlankImage(fileNonImage.Dataset.GetValue<string>(DicomTag.Modality, 0))));
                                
                            }
                            catch { }

                        }
                    }
                }
                images = images.OrderBy(f => f.ID).ToList();

                return true;
            }
            catch (Exception ex)
            {
                string s = ex.Message;
                return false;
            }
        }

        internal List<echomessege> Echo(string fromServerIP, int fromServerPort, string fromCalledAETitle, string fromCallingAETitle)
        {
            List<echomessege> status = new List<echomessege>();
            int index = 1;
            try
            {
                var client = new Dicom.Network.Client.DicomClient(fromServerIP, fromServerPort
                    , useTls, fromCalledAETitle, fromCallingAETitle
                    , setting.AssociationRequestTimeoutInMs, setting.AssociationReleaseTimeoutInMs, setting.AssociationLingerTimeoutInMs);
                var echo = new DicomCEchoRequest();
                bool echoComplete = false;
                echo.OnResponseReceived = (DicomCEchoRequest rq, DicomCEchoResponse rp) =>
                {
                    status.Add(new echomessege(index, rp.Status.ToString()));
                    index++;
                };
                client.AddRequestAsync(echo);
                client.StateChanged += (s, e) => {
                    status.Add(new echomessege(index, e.NewState.ToString()));
                    index++;
                    if (e.NewState.ToString() == "COMPLETED")
                    {
                        echoComplete = true;
                    }
                };
                client.SendAsync().ConfigureAwait(false);

                while (echoComplete != true)
                {
                    // Log("waiting...waiting....");
                }
            }
            catch (Exception ex) { status.Add(new echomessege(index,ex.Message)); }
            return status;
        }
    }
}
