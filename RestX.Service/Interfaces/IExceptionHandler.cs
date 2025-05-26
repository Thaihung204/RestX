using System;
using System.Collections.Generic;
using System.Text;

namespace Tprofile.BLL.Interfaces
{
    public interface IExceptionHandler
    {
        void RaiseException(Exception ex, string customMessage = "");
    }
}
