using System;
using UnityEngine;

namespace AssemblyCSharp
{
    public abstract class IQuest: MonoBehaviour
    {
        public abstract bool CheckIsCompleted();
    }
}

