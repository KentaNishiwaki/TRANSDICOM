using Dicom.Log;
using Dicom.Network;
using Dicom;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TRANSDICOM.Common
{
    public delegate DicomCStoreResponse OnCStoreRequestCallback(DicomCStoreRequest request);
    class CStoreSCP : DicomService, IDicomServiceProvider, IDicomCStoreProvider, IDicomCEchoProvider
    {
        private static DicomTransferSyntax[] AcceptedTransferSyntaxes = new DicomTransferSyntax[] {
        DicomTransferSyntax.ExplicitVRLittleEndian,
        DicomTransferSyntax.ExplicitVRBigEndian,
        DicomTransferSyntax.ImplicitVRLittleEndian
        };
        public CStoreSCP(Stream stream, Logger log):base(stream, Encoding.UTF8, log)
        {
        }
        public static OnCStoreRequestCallback OnCStoreRequestCallBack;
        public DicomCStoreResponse OnCStoreRequest(DicomCStoreRequest request)
        {
            // to do yourself
            // Implement a custom storage scheme
            if (OnCStoreRequestCallBack != null)
            {
                return OnCStoreRequestCallBack(request);
            }
            return new DicomCStoreResponse(request, DicomStatus.NoSuchActionType);
        }
    }
}
