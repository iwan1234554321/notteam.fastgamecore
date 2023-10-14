using System;
using UnityEngine;

namespace Notteam.FastGameCore
{
    [ExecuteAlways]
    [DefaultExecutionOrder(2)]
    public abstract class GameUpdaterComponent : MonoBehaviour, IComparable<GameUpdaterComponent>
    {
        internal GameUpdaterComponent() { }

        private bool _onAwake;
        private bool _onEnabled;
        private bool _onInitializedEditor;
        internal int UpdateOrder { get; set; }
        internal abstract Type GameUpdateSystemType { get; }

        internal void OnAwakeInternal()
        {
            if (!_onAwake)
            {
                OnAwakeInherited();
                
                Debug.Log($"Component : {name} : {GetType()} : Awake");
                
                _onAwake = true;
            }
        }
        internal void OnEnableInternal()
        {
            if (!_onEnabled)
            {
                OnEnabledInherited();

                Debug.Log($"Component : {name} : {GetType()} : Enabled");
                
                _onEnabled = true;
            }
        }
        internal void OnDisableInternal()
        {
            if (_onEnabled)
            {
                OnDisabledInherited();
                
                Debug.Log($"Component : {name} : {GetType()} : Disabled");

                _onEnabled = false;
            }
        }
        internal void OnUpdateFixedInternal()
        {
            OnUpdateFixedInherited();
        }
        internal void OnUpdateInternal()
        {
            OnUpdateInherited();
        }
        internal void OnUpdateLateInternal()
        {
            OnUpdateLateInherited();
        }

        private protected virtual void OnAwakeInherited() { }
        private protected virtual void OnEnabledInherited() { }
        private protected virtual void OnDisabledInherited() { }
        private protected virtual void OnUpdateFixedInherited() { }
        private protected virtual void OnUpdateInherited() { }
        private protected virtual void OnUpdateLateInherited() { }
        
        protected virtual void OnInitializedEditor() { }
        protected virtual void OnUpdatedEditor() { }
        
        private void OnEnable()
        {
            _onInitializedEditor = false;
            
            if (Application.isPlaying)
                GameUpdater.Instance.AddToUpdateSystem(this);
        }
        private void OnDisable()
        {
            _onInitializedEditor = false;
            
            if (Application.isPlaying)
                GameUpdater.Instance.RemoveFromUpdateSystem(this);
        }

        private void Update()
        {
            if (!Application.isPlaying)
            {
                if (!_onInitializedEditor)
                {
                    OnInitializedEditor();
                    
                    Debug.Log($"InitializedEditor : {name} ");
                    
                    _onInitializedEditor = true;
                }
                
                OnUpdatedEditor();
            }
        }

        public int CompareTo(GameUpdaterComponent other) => UpdateOrder.CompareTo(other.UpdateOrder);
    }
    
    public abstract class GameUpdaterComponent<T> : GameUpdaterComponent where T : GameUpdaterSystem
    {
        internal override Type GameUpdateSystemType => typeof(T);

        protected T System { get; private set; }

        private protected override void OnAwakeInherited()
        {
            System = GameUpdater.Instance.GetUpdateSystem<T>();
            
            OnAwakeComponent();
        }
        private protected override void OnEnabledInherited()
        {
            OnEnabledComponent();
        }
        private protected override void OnDisabledInherited()
        {
            OnDisabledComponent();
        }
        private protected override void OnUpdateFixedInherited()
        {
            OnUpdateFixedComponent();
        }
        private protected override void OnUpdateInherited()
        {
            OnUpdateComponent();
        }
        private protected override void OnUpdateLateInherited()
        {
            OnUpdateLateComponent();
        }

        protected virtual void OnAwakeComponent() { }
        protected virtual void OnEnabledComponent() { }
        protected virtual void OnDisabledComponent() { }
        protected virtual void OnUpdateFixedComponent() { }
        protected virtual void OnUpdateComponent() { }
        protected virtual void OnUpdateLateComponent() { }
    }
}