using Dicom.Log;
using Dicom.Network;
using Dicom;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.ComponentModel;

namespace TRANSDICOM.Common
{
    public delegate DicomCStoreResponse OnCStoreRequestCallback(DicomCStoreRequest request);

    class DicomCStoreProvider : DicomService, IDicomServiceProvider, IDicomCStoreProvider, IDicomCEchoProvider
    {
        #region Transfer Syntaxes
        private static DicomTransferSyntax[] AcceptedTransferSyntaxes = new DicomTransferSyntax[] {
                DicomTransferSyntax.ExplicitVRLittleEndian,
                DicomTransferSyntax.ExplicitVRBigEndian,
                DicomTransferSyntax.ImplicitVRLittleEndian
            };

        private static DicomTransferSyntax[] AcceptedImageTransferSyntaxes = new DicomTransferSyntax[] {
				// Lossless
				DicomTransferSyntax.JPEGLSLossless,
                DicomTransferSyntax.JPEG2000Lossless,
                DicomTransferSyntax.JPEGProcess14SV1,
                DicomTransferSyntax.JPEGProcess14,
                DicomTransferSyntax.RLELossless,
			
				// Lossy
				DicomTransferSyntax.JPEGLSNearLossless,
                DicomTransferSyntax.JPEG2000Lossy,
                DicomTransferSyntax.JPEGProcess1,
                DicomTransferSyntax.JPEGProcess2_4,

				// Uncompressed
				DicomTransferSyntax.ExplicitVRLittleEndian,
                DicomTransferSyntax.ExplicitVRBigEndian,
                DicomTransferSyntax.ImplicitVRLittleEndian
            };
        #endregion
        public DicomCStoreProvider(INetworkStream stream, Encoding fallbackEncoding, Logger log)
            : base(stream, fallbackEncoding, log)
        {
        }
        public static OnCStoreRequestCallback? OnCStoreRequestCallBack;
        public DicomCStoreResponse OnCStoreRequest(DicomCStoreRequest request)
        {

            if (OnCStoreRequestCallBack != null)
            {
                return OnCStoreRequestCallBack(request);
            }
            return new DicomCStoreResponse(request, DicomStatus.NoSuchActionType);

        }
        public void OnCStoreRequestException(string tempFileName, Exception e)
        {
            throw new NotImplementedException();
        }
        public Task OnCStoreRequestExceptionAsync(string tempFileName, Exception e)
        {
            // let library handle logging and error response
            return Task.CompletedTask;
        }



        public void OnReceiveAbort(DicomAbortSource source, DicomAbortReason reason)
        {
        }

        public void OnConnectionClosed(Exception exception)
        {
        }

        public void OnReceiveAssociationReleaseRequest()
        {
            this.SendAssociationReleaseResponseAsync();
        }

        public void OnReceiveAssociationRequest()
        {
            var association = new DicomAssociation();
            this.SendAssociationAcceptAsync(association);
        }
        public void OnReceiveAssociationRequest(DicomAssociation association)
        {

            // if (association.CalledAE != "STORESCP")
            //{
            //  SendAssociationReject(DicomRejectResult.Permanent, DicomRejectSource.ServiceUser, DicomRejectReason.CalledAENotRecognized);
            //  return;
            //}

            foreach (var pc in association.PresentationContexts)
            {
                if (pc.AbstractSyntax == DicomUID.Verification)
                    pc.AcceptTransferSyntaxes(AcceptedTransferSyntaxes);
                else if (pc.AbstractSyntax.StorageCategory != DicomStorageCategory.None)
                    pc.AcceptTransferSyntaxes(AcceptedImageTransferSyntaxes);
            }

            this.SendAssociationAcceptAsync(association);

        }
        public Task OnReceiveAssociationRequestAsync(DicomAssociation association)
        {
            //if (association.CalledAE != "STORESCP")
            //{
            //    return SendAssociationRejectAsync(
            //        DicomRejectResult.Permanent,
            //        DicomRejectSource.ServiceUser,
            //        DicomRejectReason.CalledAENotRecognized);
            //}

            foreach (var pc in association.PresentationContexts)
            {
                if (pc.AbstractSyntax == DicomUID.Verification)
                {
                    pc.AcceptTransferSyntaxes(AcceptedTransferSyntaxes);
                }
                else if (pc.AbstractSyntax.StorageCategory != DicomStorageCategory.None)
                {
                    pc.AcceptTransferSyntaxes(AcceptedImageTransferSyntaxes);
                }
            }

            return SendAssociationAcceptAsync(association);
        }

        public Task OnReceiveAssociationReleaseRequestAsync()
        {
            return this.SendAssociationReleaseResponseAsync();
        }

        public DicomCEchoResponse OnCEchoRequest(DicomCEchoRequest request)
        {
            return new DicomCEchoResponse(request, DicomStatus.Success);
        }
        public Task<DicomCEchoResponse> OnCEchoRequestAsync(DicomCEchoRequest request)
        {
            return Task.FromResult(new DicomCEchoResponse(request, DicomStatus.Success));
        }

    }
}
