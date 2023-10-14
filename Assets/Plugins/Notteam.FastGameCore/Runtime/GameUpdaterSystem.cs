using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace Notteam.FastGameCore
{
    [DefaultExecutionOrder(1)]
    public abstract class GameUpdaterSystem : MonoBehaviour
    {
        [Serializable]
        public class FilteredByTypeComponents
        {
            public Type FilterType;

            public int componentsCount;
        }

        private bool _onAwake;
        private bool _addedAllComponentsInUpdate;
        
        private List<GameUpdaterComponent> _readyComponents = new();
        private List<GameUpdaterComponent> _updateComponents = new();

        private List<FilteredByTypeComponents> _filteredComponents = new();

        public bool AddedAllComponentsInUpdate => _addedAllComponentsInUpdate;
        
        internal void OnUpdateFixedInternal()
        {
            if (_addedAllComponentsInUpdate)
            {
                OnUpdateFixedSystem();
            
                foreach (var component in _updateComponents)
                    component.OnUpdateFixedInternal();
            }
        }
        
        internal void OnUpdateInternal()
        {
            if (!_addedAllComponentsInUpdate)
            {
                foreach (var readyComponent in _readyComponents)
                {
                    if (!_filteredComponents.Exists(e => e.FilterType == readyComponent.GetType()))
                    {
                        _filteredComponents.Add(new FilteredByTypeComponents { FilterType = readyComponent.GetType(), });
                    }
                }

                foreach (var filteredComponent in _filteredComponents)
                {
                    foreach (var readyComponent in _readyComponents)
                    {
                        if (filteredComponent.FilterType == readyComponent.GetType())
                        {
                            if (!_updateComponents.Contains(readyComponent))
                            {
                                _updateComponents.Add(readyComponent);
                                
                                filteredComponent.componentsCount++;
                            }
                        }
                    }
                }
                
                SortComponents(ref _updateComponents);
                
                if (!_onAwake)
                {
                    OnAwakeSystem();

                    Debug.Log($"System : {name} : Awake");
                    
                    _onAwake = true;
                }
            
                foreach (var updateComponent in _updateComponents)
                {
                    updateComponent.OnAwakeInternal();
                    updateComponent.OnEnableInternal();
                }
                    
                _addedAllComponentsInUpdate = true;
            }
            else
            {
                OnUpdateSystem();
                
                foreach (var component in _updateComponents)
                    component.OnUpdateInternal();
            }
        }

        internal void OnUpdateLateInternal()
        {
            if (_addedAllComponentsInUpdate)
            {
                OnUpdateLateSystem();
            
                foreach (var component in _updateComponents)
                    component.OnUpdateLateInternal();
            }
        }
        
        protected virtual void OnAwakeSystem() { }
        protected virtual void OnUpdateFixedSystem() { }
        protected virtual void OnUpdateSystem() { }
        protected virtual void OnUpdateLateSystem() { }
        
        private void SortComponents(ref List<GameUpdaterComponent> components)
        {
            foreach (var componentA in components)
            {
                var updateBefore = componentA.GetType().GetCustomAttribute<UpdateBeforeAttribute>();
                var updateAfter = componentA.GetType().GetCustomAttribute<UpdateAfterAttribute>();
                
                if (updateBefore != null)
                {
                    foreach (var componentB in components)
                    {
                        if (updateBefore.ComponentType == componentB.GetType())
                            componentA.UpdateOrder = componentB.UpdateOrder - 1;
                    }
                }

                if (updateAfter != null)
                {
                    foreach (var componentB in components)
                    {
                        if (updateAfter.ComponentType == componentB.GetType())
                            componentA.UpdateOrder = componentB.UpdateOrder + 1;
                    }
                }
            }
            
            components.Sort();
        }
        
        internal void AddToReadyList(GameUpdaterComponent component)
        {
            _addedAllComponentsInUpdate = false;
            
            if (!_readyComponents.Contains(component))
            {
                _readyComponents.Add(component);

                SortComponents(ref _readyComponents);
            }
        }
        internal void RemoveFromReadyList(GameUpdaterComponent component)
        {
            if (_readyComponents.Contains(component))
            {
                _readyComponents.Remove(component);
            }
            
            if (_updateComponents.Contains(component))
            {
                _updateComponents.Remove(component);
            }

            if (_filteredComponents.Exists(e => e.FilterType == component.GetType()))
            {
                var filteredComponent = _filteredComponents.Find(e => e.FilterType == component.GetType());
                var filteredComponentIndex = _filteredComponents.IndexOf(filteredComponent);
                
                if (filteredComponent.componentsCount > 1)
                    filteredComponent.componentsCount--;
                else
                    _filteredComponents.RemoveAt(filteredComponentIndex);
            }
            
            component.OnDisableInternal();
        }
    }
}
