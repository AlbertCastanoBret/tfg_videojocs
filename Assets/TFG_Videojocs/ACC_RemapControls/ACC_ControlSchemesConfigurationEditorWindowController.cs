using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace TFG_Videojocs.ACC_RemapControls
{
    public class ACC_ControlSchemesConfigurationEditorWindowController: ACC_FloatingWindowController<ACC_ControlSchemesConfigurationEditorWindow, ACC_ControlSchemeData>
    {
        public Dictionary<string, bool> onScreenControlSchemeToggleValues = new();
        public Dictionary<ACC_BindingData, bool> onScreenBindingToggleValues = new();

        public override void ConfigureJson()
        {
            currentData.inputActionAsset = window.inputActionAsset;
            base.ConfigureJson();
            window.accRebindControlsManager.LoadRebindControlsManager(oldName);
        }

        protected override void RestoreFieldValues()
        {
            foreach (var controlScheme in currentData.controlSchemesList.Items)
            {
                onScreenControlSchemeToggleValues[controlScheme.key] = controlScheme.value;
            }

            foreach (var binding in currentData.bindingsList.Items)
            {
                onScreenBindingToggleValues[binding.key] = binding.value;
            }
            
            window.CreateTable();
        }
    }
}