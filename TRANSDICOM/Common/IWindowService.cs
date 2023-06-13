using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TRANSDICOM.Common
{
    public interface IWindowService
    {
        void CreateWindow(Setting _setting);
        void CloseWindow();
    }
}
