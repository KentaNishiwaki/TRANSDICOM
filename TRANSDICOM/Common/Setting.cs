using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO.IsolatedStorage;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using BitMiracle.LibJpeg;
using System.Reflection;

namespace TRANSDICOM.Common
{
    public class Setting : ViewModelBase
    {
        public event EventHandler SettingEvent;

        string _FromServerIP = "";
        public string FromServerIP { get { return _FromServerIP; } set { _FromServerIP = value; RaisePropertyChanged("FromServerIP"); } }
        int _FromServerPort = 0;
        public int FromServerPort { get { return _FromServerPort; } set { _FromServerPort = value; RaisePropertyChanged("FromServerPort"); } }
        string _FromCallingAETitle = "";
        public string FromCallingAETitle { get { return _FromCallingAETitle; } set { _FromCallingAETitle = value; RaisePropertyChanged("FromCallingAETitle"); } }
        string _FromCalledAETitle = "";
        public string FromCalledAETitle { get { return _FromCalledAETitle; } set { _FromCalledAETitle = value; RaisePropertyChanged("FromCalledAETitle"); } }

        int _DestinationPort = 104;
        public int DestinationPort { get { return _DestinationPort; } set { _DestinationPort = value; RaisePropertyChanged("DestinationPort"); } }

        string _DestinationAE = "";
        public string DestinationAE { get { return _DestinationAE; } set { _DestinationAE = value; RaisePropertyChanged("DestinationAE"); } }


        string _ToServerIP = "";
        public string ToServerIP { get { return _ToServerIP; } set { _ToServerIP = value; RaisePropertyChanged("ToServerIP"); } }
        int _ToServerPort = 0;
        public int ToServerPort { get { return _ToServerPort; } set { _ToServerPort = value; RaisePropertyChanged("ToServerPort"); } }
        string _ToCallingAETitle = "";
        public string ToCallingAETitle { get { return _ToCallingAETitle; } set { _ToCallingAETitle = value; RaisePropertyChanged("ToCallingAETitle"); } }
        string _ToCalledAETitle = "";
        public string ToCalledAETitle { get { return _ToCalledAETitle; } set { _ToCalledAETitle = value; RaisePropertyChanged("ToCalledAETitle"); } }


        public void GetSetting()
        {
            try
            {
                var setting = LoadFileSetting();

                FromServerIP = setting.FromServerIP;
                FromServerPort = setting.FromServerPort;
                FromCallingAETitle = setting.FromCallingAETitle;
                FromCalledAETitle = setting.FromCalledAETitle;
                DestinationAE = setting.DestinationAE;
                DestinationPort = setting.DestinationPort;
                ToServerIP = setting.ToServerIP;
                ToServerPort = setting.ToServerPort;
                ToCallingAETitle = setting.ToCallingAETitle;
                ToCalledAETitle = setting.ToCalledAETitle;

            }
            catch (Exception)
            {
            }
        }

        public void Save()
        {
            try
            {
                SaveFileSetting();
                OnSettingSaved(EventArgs.Empty);
            }
            catch (Exception)
            {
                throw;
            }
        }

        protected virtual void OnSettingSaved(EventArgs e)
        {
            SettingEvent?.Invoke(this, e);
        }

        public Setting LoadFileSetting()
        {
            try
            {
                // デシリアライズする
                var xmlSerializer = new XmlSerializer(typeof(Setting));
                var xmlSettings = new System.Xml.XmlReaderSettings()
                {
                    CheckCharacters = false,
                };
                var xmlFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),"TRANSDICOM", "setting.xml");
                using (var streamReader = new StreamReader(xmlFile, Encoding.UTF8))
                using (var xmlReader
                        = System.Xml.XmlReader.Create(streamReader, xmlSettings))
                {
                    return (Setting)xmlSerializer.Deserialize(xmlReader);
                }
                return new Setting();
            }
            catch (Exception)
            {
                var rc = new Setting();
                rc.FromServerIP = "localhost";
                rc.FromServerPort = 5678;
                rc.FromCallingAETitle = "CONQUESTSRV1";
                rc.FromCalledAETitle = "TRANSDICOM_SCU";
                rc.DestinationAE = "TRANSDICOM";
                rc.DestinationPort = 105;
                rc.ToServerIP = "localhost";
                rc.ToServerPort = 104;
                rc.ToCallingAETitle = "RTNS1_SCP";
                rc.ToCalledAETitle = "TRANSDICOM_SCU";


                return rc;
            }
        }

        public void SaveFileSetting()
        {
            try
            {
                var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "TRANSDICOM");
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);

                // シリアライズする
                var xmlSerializer1 = new XmlSerializer(typeof(Setting));
                var xmlFile = Path.Combine(path, "setting.xml");
                using (var streamWriter = new StreamWriter(xmlFile, false, Encoding.UTF8))
                {
                    xmlSerializer1.Serialize(streamWriter, this);
                    streamWriter.Flush();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }


    }
}
