using Dicom;
using Dicom.Network;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TRANSDICOM.Common;

namespace TRANSDICOM.Model
{
    public class DicomServerModel
    {
        Setting setting;
        public DicomServerModel(Setting _setting) 
        {
            setting = _setting;
        }

        public IDicomServer Start()
        {
            var dServer = DicomServer.Create<DicomCStoreProvider>(setting.DestinationPort);
            DicomCStoreProvider.OnCStoreRequestCallBack = (request) =>
            {
                var studyUid = request.Dataset.GetValue<string>(DicomTag.StudyInstanceUID, 0);
                var seriesUid = request.Dataset.GetValue<string>(DicomTag.SeriesInstanceUID, 0);
                var instUid = request.SOPInstanceUID.UID;

                var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "TRANSDICOM");
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);
                path = Path.Combine(path, "tmp");
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);


                path = System.IO.Path.Combine(path, studyUid);
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);

                path = System.IO.Path.Combine(path, seriesUid);
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);

                path = System.IO.Path.Combine(path, instUid) + ".dcm";

                request.File.Save(path);

                return new DicomCStoreResponse(request, DicomStatus.Success);
            };

            return dServer;

        }
    }
}
