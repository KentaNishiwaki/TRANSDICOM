using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace TRANSDICOM.Common
{
    public class dicomImagesInfo
    {
        public int ID { set; get; } = 0;
        public string Title { set; get; } = "";
        public byte[] Image { set; get; }
        public dicomImagesInfo(int id, string title, byte[] image)
        {
            ID = id;
            Title = title;
            Image = image;
        }
    }
}
