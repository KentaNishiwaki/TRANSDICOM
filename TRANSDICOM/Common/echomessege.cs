using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TRANSDICOM.Common
{
    public class echomessege
    {
        public int Index =0;
        public string Messege { set; get; } = "";
        public echomessege(int index,string messege)
        {
            Index = index;
            Messege = messege;

        }
    }
}
