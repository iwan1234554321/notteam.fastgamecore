using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Notteam.FastGameCore
{
    public class GameSceneBridge : MonoBehaviour
    {
        internal Object FindObjectOnScene(Type type, bool includeInactive)
        {
            return FindObjectOfType(type, includeInactive);
        }
        
        internal Object[] FindObjectsOnScene(Type type, bool includeInactive)
        {
            return FindObjectsOfType(type, includeInactive);
        }
        
        internal Object FindObjectOnScene(Type type)
        {
            return FindObjectOfType(type, false);
        }
        
        internal Object[] FindObjectsOnScene(Type type)
        {
            return FindObjectsOfType(type, false);
        }
        
        internal T[] FindObjectsOnScene<T>(bool includeInactive) where T : MonoBehaviour
        {
            return FindObjectsOfType<T>(includeInactive);
        }
        
        internal T[] FindObjectsOnScene<T>() where T : MonoBehaviour
        {
            return FindObjectsOnScene<T>(false);
        }
        
        internal T FindObjectOnScene<T>(bool includeInactive) where T : MonoBehaviour
        {
            return FindObjectOfType<T>(includeInactive);
        }
        
        internal T FindObjectOnScene<T>() where T : MonoBehaviour
        {
            return FindObjectOfType<T>(false);
        }
    }
}
