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

        private int _FromSelectIndex = 0;
        public int FromSelectIndex { get { return _FromSelectIndex; } set { _FromSelectIndex = value; RaisePropertyChanged("FromSelectIndex"); } }

        private List<PacsFrom> _FromList = new List<PacsFrom>();
        public List<PacsFrom> FromList { get { return _FromList; } set { _FromList = value; RaisePropertyChanged("FromList"); } }

        public class PacsFrom : ViewModelBase
        {
            string _FromName = "";
            public string FromName { get { return _FromName; } set { _FromName = value; RaisePropertyChanged("FromName"); } }
            string _FromServerIP = "";
            public string FromServerIP { get { return _FromServerIP; } set { _FromServerIP = value; RaisePropertyChanged("FromServerIP"); } }
            int _FromServerPort = 0;
            public int FromServerPort { get { return _FromServerPort; } set { _FromServerPort = value; RaisePropertyChanged("FromServerPort"); } }
            string _FromCallingAETitle = "";
            public string FromCallingAETitle { get { return _FromCallingAETitle; } set { _FromCallingAETitle = value; RaisePropertyChanged("FromCallingAETitle"); } }
            string _FromCalledAETitle = "";
            public string FromCalledAETitle { get { return _FromCalledAETitle; } set { _FromCalledAETitle = value; RaisePropertyChanged("FromCalledAETitle"); } }
            public string FromDescription { get { return getFromDescription(); } }
            public string FromImage { get { return getFromImage(); } }
            public PacsFrom() 
            {
            }
            public PacsFrom(string fromname, string fromserverip,int fromserverport, string fromcallingaetitle,string fromcalledaetitle)
            {
                FromName = fromname;
                FromServerIP = fromserverip;
                FromServerPort = fromserverport;
                FromCallingAETitle = fromcallingaetitle;
                FromCalledAETitle = fromcalledaetitle;
            }
            private string getFromDescription()
            {
                return "Name: " + FromName
                    + "\r\nIP: " + FromServerIP
                    + "\r\nPort: " + FromServerPort.ToString()
                    + "\r\nSCP AE: " + FromCallingAETitle
                    + "\r\nSCU AE: " + FromCalledAETitle;
            }
            private string getFromImage()
            {
                string dir = "/img/";

                string pacsinfo = FromName + FromServerIP + FromServerPort.ToString() + FromCallingAETitle + FromCalledAETitle;

                int sum = 0;
                for (int i = 0; i < pacsinfo.Length; i++)
                {
                    char c = pacsinfo[i];
                    sum += (int)c;
                }
                ;

                switch (sum.ToString().Substring(sum.ToString().Length - 1))
                {
                    case "0":
                        return dir + "icons8_basketball.ico";
                    case "1":
                        return dir + "icons8_soccer_ball.ico";
                    case "2":
                        return dir + "icons8_tennis.ico";
                    case "3":
                        return dir + "icons8_rugby_football.ico";
                    case "4":
                        return dir + "icons8_volleyball.ico";
                    case "5":
                        return dir + "icons8_baseball_ball.ico";
                    case "6":
                        return dir + "icons8_shuttlecock.ico";
                    case "7":
                        return dir + "icons8_billiard_ball.ico";
                    case "8":
                        return dir + "icons8_pokeball.ico";
                    case "9":
                        return dir + "icons8_Disco_Ball.ico";
                    default:
                        return dir + "icons8_pokeball.ico"; ;
                }
            }

        }

        private int _DestinationSelectIndex = 0;
        public int DestinationSelectIndex { get { return _DestinationSelectIndex; } set { _DestinationSelectIndex = value; RaisePropertyChanged("DestinationSelectIndex"); } }
        private List<PacsDestination> _DestinationList = new List<PacsDestination>();
        public List<PacsDestination> DestinationList { get { return _DestinationList; } set { _DestinationList = value; RaisePropertyChanged("DestinationList"); } }
        public class PacsDestination : ViewModelBase
        {
            string _DestinationName = "";
            public string DestinationName { get { return _DestinationName; } set { _DestinationName = value; RaisePropertyChanged("DestinationName"); } }

            int _DestinationPort = 104;
            public int DestinationPort { get { return _DestinationPort; } set { _DestinationPort = value; RaisePropertyChanged("DestinationPort"); } }

            string _DestinationAE = "";
            public string DestinationAE { get { return _DestinationAE; } set { _DestinationAE = value; RaisePropertyChanged("DestinationAE"); } }
            public string DestinationDescription { get { return getDestinationDescription(); } }
            public string DestinationImage { get { return getDestinationImage(); } }

            public PacsDestination()
            {
            }
            public PacsDestination(string destinationname, int destinationport, string destinationae)
            {
                DestinationName = destinationname;
                DestinationPort = destinationport;
                DestinationAE = destinationae;
            }
            private string getDestinationDescription()
            {
                return "Name: " + DestinationName
                    + "\r\nPort: " + DestinationPort.ToString()
                    + "\r\nDestination AE: " + DestinationAE;
            }
            private string getDestinationImage()
            {
                string dir = "/img/";

                string pacsinfo = DestinationName + DestinationPort.ToString() + DestinationAE;

                int sum = 0;
                for (int i = 0; i < pacsinfo.Length; i++)
                {
                    char c = pacsinfo[i];
                    sum += (int)c;
                }
                ;

                switch (sum.ToString().Substring(sum.ToString().Length - 1))
                {
                    case "0":
                        return dir + "icons8_lotus.ico";
                    case "1":
                        return dir + "icons8_hops.ico";
                    case "2":
                        return dir + "icons8_broccoli.ico";
                    case "3":
                        return dir + "icons8_sunflower.ico";
                    case "4":
                        return dir + "icons8_three_leaf_clover.ico";
                    case "5":
                        return dir + "icons8_cactus.ico";
                    case "6":
                        return dir + "icons8_flower.ico";
                    case "7":
                        return dir + "icons8_blossom.ico";
                    case "8":
                        return dir + "icons8_tulip.ico";
                    case "9":
                        return dir + "icons8_rose.ico";
                    default:
                        return dir + "icons8_lotus.ico"; ;
                }
            }

        }

        private int _ToSelectIndex = 0;
        public int ToSelectIndex { get { return _ToSelectIndex; } set { _ToSelectIndex = value; RaisePropertyChanged("ToSelectIndex"); } }

        private List<PacsTo> _ToList = new List<PacsTo>();
        public List<PacsTo> ToList { get { return _ToList; } set { _ToList = value; RaisePropertyChanged("ToList"); } }
        public class PacsTo : ViewModelBase
        {
            string _ToName = "";
            public string ToName { get { return _ToName; } set { _ToName = value; RaisePropertyChanged("ToName"); } }
            string _ToServerIP = "";
            public string ToServerIP { get { return _ToServerIP; } set { _ToServerIP = value; RaisePropertyChanged("ToServerIP"); } }
            int _ToServerPort = 0;
            public int ToServerPort { get { return _ToServerPort; } set { _ToServerPort = value; RaisePropertyChanged("ToServerPort"); } }
            string _ToCallingAETitle = "";
            public string ToCallingAETitle { get { return _ToCallingAETitle; } set { _ToCallingAETitle = value; RaisePropertyChanged("ToCallingAETitle"); } }
            string _ToCalledAETitle = "";
            public string ToCalledAETitle { get { return _ToCalledAETitle; } set { _ToCalledAETitle = value; RaisePropertyChanged("ToCalledAETitle"); } }
            public string ToDescription { get { return getToDescription(); } }
            public string ToImage { get { return getToImage(); } }
            public PacsTo()
            {
            }
            public PacsTo(string toname, string toserverip, int toserverport, string tocallingaetitle, string tocalledaetitle)
            {
                ToName = toname;
                ToServerIP = toserverip;
                ToServerPort = toserverport;
                ToCallingAETitle = tocallingaetitle;
                ToCalledAETitle = tocalledaetitle;
            }
            private string getToDescription()
            {
                return "Name: " + ToName
                    + "\r\nIP: " + ToServerIP
                    + "\r\nPort: " + ToServerPort.ToString()
                    + "\r\nSCP AE: " + ToCallingAETitle
                    + "\r\nSCU AE: " + ToCalledAETitle;
            }
            private string getToImage()
            {
                string dir = "/img/";

                string pacsinfo = ToName + ToServerIP + ToServerPort.ToString() + ToCallingAETitle + ToCalledAETitle;

                int sum = 0;
                for (int i = 0; i < pacsinfo.Length; i++)
                {
                    char c = pacsinfo[i];
                    sum += (int)c;
                }
                ;

                switch (sum.ToString().Substring(sum.ToString().Length - 1))
                {
                    case "0":
                        return dir + "icons8_tiger_face.ico";
                    case "1":
                        return dir + "icons8_panda.ico";
                    case "2":
                        return dir + "icons8_pig_face.ico";
                    case "3":
                        return dir + "icons8_zebra.ico";
                    case "4":
                        return dir + "icons8_cat.ico";
                    case "5":
                        return dir + "icons8_dog.ico";
                    case "6":
                        return dir + "icons8_duck.ico";
                    case "7":
                        return dir + "icons8_rabbit_face.ico";
                    case "8":
                        return dir + "icons8_crocodile.ico";
                    case "9":
                        return dir + "icons8_bear.ico";
                    default:
                        return dir + "icons8_tiger_face.ico"; ;
                }
            }

        }


        private int _AssociationRequestTimeoutInMs = 5000;
        private int _AssociationReleaseTimeoutInMs = 10000;
        private int _AssociationLingerTimeoutInMs = 50;
        public int AssociationRequestTimeoutInMs { get { return _AssociationRequestTimeoutInMs; } set { _AssociationRequestTimeoutInMs = value; RaisePropertyChanged("AssociationRequestTimeoutInMs"); } }
        public int AssociationReleaseTimeoutInMs { get { return _AssociationReleaseTimeoutInMs; } set { _AssociationReleaseTimeoutInMs = value; RaisePropertyChanged("AssociationReleaseTimeoutInMs"); } }
        public int AssociationLingerTimeoutInMs { get { return _AssociationLingerTimeoutInMs; } set { _AssociationLingerTimeoutInMs = value; RaisePropertyChanged("AssociationLingerTimeoutInMs"); } }



        public void GetSetting()
        {
            try
            {
                var setting = LoadFileSetting();

                FromList = setting.FromList;
                DestinationList = setting.DestinationList;
                ToList = setting.ToList;
                FromSelectIndex = setting.FromSelectIndex;
                DestinationSelectIndex = setting.DestinationSelectIndex;
                ToSelectIndex = setting.ToSelectIndex;

                AssociationRequestTimeoutInMs = setting.AssociationRequestTimeoutInMs;
                AssociationReleaseTimeoutInMs = setting.AssociationReleaseTimeoutInMs;
                AssociationLingerTimeoutInMs = setting.AssociationLingerTimeoutInMs;

                //FromServerIP = setting.FromServerIP;
                //FromServerPort = setting.FromServerPort;
                //FromCallingAETitle = setting.FromCallingAETitle;
                //FromCalledAETitle = setting.FromCalledAETitle;
                //DestinationAE = setting.DestinationAE;
                //DestinationPort = setting.DestinationPort;
                //ToServerIP = setting.ToServerIP;
                //ToServerPort = setting.ToServerPort;
                //ToCallingAETitle = setting.ToCallingAETitle;
                //ToCalledAETitle = setting.ToCalledAETitle;

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
                rc.FromList = new List<PacsFrom>();
                rc.DestinationList = new List<PacsDestination>();
                rc.ToList = new List<PacsTo>();
                rc.FromList.Add(new PacsFrom("From Sample","localhost", 5678, "CONQUESTSRV1", "TRANSDICOM_SCU"));
                rc.DestinationList.Add(new PacsDestination("Destination Sample", 105, "TRANSDICOM"));
                rc.ToList.Add(new PacsTo("To Sample", "localhost", 104, "RTNS1_SCP", "TRANSDICOM_SCU"));
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

        public delegate void CloseEditWindowDelegate();
        public event CloseEditWindowDelegate CloseEditWindow = null;
        public void ExcCloseEditWindow()
        {
            if (CloseEditWindow != null)
            {
                CloseEditWindow();
            }
        }


    }
}
