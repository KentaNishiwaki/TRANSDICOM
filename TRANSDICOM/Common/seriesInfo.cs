using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TRANSDICOM.Common
{
    public class seriesInfo
    {
        public string StudyInstanceUID { set; get; } = "";
        public string SeriesInstanceUID { set; get; } = "";
        public string Modality { set; get; } = "";
        public seriesInfo(string studyInstanceUID, string seriesInstanceUID, string modality)
        {
            StudyInstanceUID = studyInstanceUID;
            SeriesInstanceUID = seriesInstanceUID;
            Modality = modality;
        }
    }
}
