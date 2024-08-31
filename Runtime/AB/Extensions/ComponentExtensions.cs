using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XFABManager
{

    internal static class ComponentExtensions 
    {
        internal static bool IsDestroy(this Component component)
        { 
            return !component;
        }
    }
}

