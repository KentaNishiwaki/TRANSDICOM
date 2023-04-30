using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TRANSDICOM.Common
{
    public class studiesInfo
    {
        public string StudyInstanceUID { set; get; } = "";
        public string StudyDate { set; get; } = "";
        public string PatientName { set; get; } = "";
        public studiesInfo(string studyInstanceUID, string studyDate, string patientName)
        {
            StudyInstanceUID = studyInstanceUID;
            StudyDate = studyDate;
            PatientName = patientName;
        }
    }
}
