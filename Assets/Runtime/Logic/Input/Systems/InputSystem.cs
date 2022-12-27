using System;
using Notteam.FastGameCore;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputSystem : GameUpdaterSystem
{
    [SerializeField] private InputActionAsset inputAsset;

    protected override void OnAwakeSystem()
    {
        inputAsset.Enable();
    }
    
    public void BindToInputAction(string nameAction, Action<float> valueAction)
    {
        var actionFound = null as InputAction;
        
        foreach (var map in inputAsset.actionMaps)
        {
            foreach (var action in map.actions)
            {
                if (action.name == nameAction)
                    actionFound = action;
            }
        }

        if (actionFound != null)
        {
            actionFound.started   += (x) => { valueAction.Invoke(x.ReadValue<float>()); };
            actionFound.performed += (x) => { valueAction.Invoke(x.ReadValue<float>()); };
            actionFound.canceled  += (x) => { valueAction.Invoke(x.ReadValue<float>()); };
        }
        else
            Debug.LogWarning($"Missing input action : {nameAction}");
    }
}
