using System.ComponentModel;
using UnityEngine;


namespace XFABManager
{
    internal static class GameObjectExtensions
    {

        internal static bool IsDestroy(this GameObject gameObject)
        {
            return !gameObject;
        }

    }

}