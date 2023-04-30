using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TRANSDICOM.Common
{
    public class CurrentCount : ViewModelBase
    {
        int _StudiesCount = 0;
        public int StudiesCount { get { return _StudiesCount; } set { _StudiesCount = value; RaisePropertyChanged("StudiesCount"); } }

        int _StudiesCurrent = 0;
        public int StudiesCurrent { get { return _StudiesCurrent; } set { _StudiesCurrent = value; RaisePropertyChanged("StudiesCurrent"); } }

        int _SeriesCount = 0;
        public int SeriesCount { get { return _SeriesCount; } set { _SeriesCount = value; RaisePropertyChanged("SeriesCount"); } }

        int _SeriesCurrent = 0;
        public int SeriesCurrent { get { return _SeriesCurrent; } set { _SeriesCurrent = value; RaisePropertyChanged("SeriesCurrent"); } }

        int _CMoveCount = 0;
        public int CMoveCount { get { return _CMoveCount; } set { _CMoveCount = value; RaisePropertyChanged("CMoveCount"); } }

        int _CMoveCurrent = 0;
        public int CMoveCurrent { get { return _CMoveCurrent; } set { _CMoveCurrent = value; RaisePropertyChanged("CMoveCurrent"); } }

        int _CStoreCount = 0;
        public int CStoreCount { get { return _CStoreCount; } set { _CStoreCount = value; RaisePropertyChanged("CStoreCount"); } }

        int _CStoreCurrent = 0;
        public int CStoreCurrent { get { return _CStoreCurrent; } set { _CStoreCurrent = value; RaisePropertyChanged("CStoreCurrent"); } }
    }

}
