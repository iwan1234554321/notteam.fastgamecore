using UnityEngine;

namespace Notteam.FastGameCore
{
    public abstract class GameData : ScriptableObject
    {
    }
    
    public abstract class GameData<T> : GameData where T : struct
    {
        private T _changedData;
        
        public abstract T Data { get; }

        public bool ChangedData()
        {
            if (Equals(_changedData, Data) == false)
            {
                _changedData = Data;
                
                return true;
            }
            
            return false;
        }
    }
}
