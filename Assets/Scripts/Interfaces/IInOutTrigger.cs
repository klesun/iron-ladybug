using System;
using Util;

namespace Interfaces
{
    public interface IInOutTrigger
    {
        void OnIn (D.Cb cb);
        void OnOut (D.Cb cb);
    }
}

