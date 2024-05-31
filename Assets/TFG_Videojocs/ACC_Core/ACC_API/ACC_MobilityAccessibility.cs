using TFG_Videojocs.ACC_RemapControls;
using TFG_Videojocs.ACC_Utilities;
using UnityEngine;
using UnityEngine.InputSystem;


namespace ACC_API
{
    /// <summary>
    /// Enum representing different mobility accessibility features.
    /// </summary>
    public enum MobilityFeatures
    {
        /// <summary>
        /// Remapping of controls for accessibility.
        /// </summary>
        RemapControls
    }

    public class ACC_MobilityAccessibility
    {
        private ACC_RemapControlsManager accRemapControlsManager;
        internal ACC_MobilityAccessibility()
        {
            accRemapControlsManager = ACC_PrefabHelper.InstantiatePrefabAsChild(
                    "RemapControls", 
                    ACC_AccessibilityManager.Instance.accCanvas, 
                    ACC_AccessibilityManager.Instance.remapControlsAsset.name)
                .GetComponent<ACC_RemapControlsManager>();
        }

        internal void InitializeState(MobilityFeatures feature, bool state)
        {
            switch (feature)
            {
                case MobilityFeatures.RemapControls:
                    accRemapControlsManager.InitializeRemapControls(state);
                    break;
            }
        }
        
        /// <summary>
        /// Sets the state of a specified mobility feature to either enabled or disabled.
        /// </summary>
        /// <param name="feature">The mobility feature to modify. Use MobilityFeatures enum to specify the feature.</param>
        /// <param name="state">A boolean value indicating whether the feature should be enabled (true) or disabled (false).</param>
        public void SetFeatureState(MobilityFeatures feature, bool state)
        {
            switch (feature)
            {
                case MobilityFeatures.RemapControls:
                    accRemapControlsManager.SetRemapControls(state);
                    break;
            }
        }
        
        /// <summary>
        /// Determines whether the specified mobility feature is enabled or disabled based on the user's saved preferences.
        /// </summary>
        /// <param name="feature">The mobility feature to check (e.g., remap controls).</param>
        /// <returns>True if the feature is enabled, otherwise false.</returns>
        public bool GetFeatureState(MobilityFeatures feature)
        {
            switch (feature)
            {
                case MobilityFeatures.RemapControls:
                    if (PlayerPrefs.HasKey(ACC_AccessibilitySettingsKeys.RemapControlsEnabled))
                    {
                        return PlayerPrefs.GetInt(ACC_AccessibilitySettingsKeys.RemapControlsEnabled) == 1;
                    }
                    return ACC_AccessibilityManager.Instance.remapControlsEnabled;
                default:
                    return false;
            }
        }
        
        /// <summary>
        /// Resets the state of the specified mobility feature to its default settings.
        /// </summary>
        /// <param name="feature">The mobility feature to reset (e.g., remap controls).</param>
        public void ResetFeatureState(MobilityFeatures feature)
        {
            switch (feature)
            {
                case MobilityFeatures.RemapControls:
                    accRemapControlsManager.ResetRemapControlsState();
                    break;
            }
        }

        /// <summary>
        /// Loads and applies the user's accessibility preferences related to mobility features.
        /// </summary>
        public void LoadUserPreferences()
        {
            LoadUserPreferencesRemapControls();
        }
        
        /// <summary>
        /// Displays the rebind menu for a specific device.
        /// </summary>
        /// <param name="device">The identifier for the device whose controls are to be rebound.</param>
        public void EnableRemapControlsMenu(string device)
        {
            ACC_AccessibilityManager.Instance.AudioAccessibility.ShowSubtitles(false);
            ACC_AccessibilityManager.Instance.AudioAccessibility.ShowVisualNotification(false);
            accRemapControlsManager.EnableRemapControlsMenu(device);
        }
        
        /// <summary>
        /// Hides the remap controls menu.
        /// </summary>
        public void DisableRemapControlsMenu()
        {
            ACC_AccessibilityManager.Instance.AudioAccessibility.ShowSubtitles(true);
            ACC_AccessibilityManager.Instance.AudioAccessibility.ShowVisualNotification(true);
            accRemapControlsManager.DisableRebindMenu();
        }
        
        /// <summary>
        /// Changes the current control scheme to the specified one.
        /// </summary>
        /// <param name="controlScheme">The name of the control scheme to switch to.</param>
        public void ChangeControlScheme(string controlScheme)
        {
            accRemapControlsManager.ChangeControlScheme(controlScheme);
        }
        
        /// <summary>
        /// Updates a specific binding for a given action by replacing it with a new binding.
        /// </summary>
        /// <param name="actionName">The name of the action to modify.</param>
        /// <param name="bindingIndex">The index of the binding to replace within the action.</param>
        /// <param name="newBinding">The new input binding to assign.</param>
        public void ChangeBinding(string actionName, int bindingIndex, InputBinding newBinding)
        {
            accRemapControlsManager.ChangeBinding(actionName, bindingIndex, newBinding);
        }
        
        /// <summary>
        /// Resets a specific binding for a given action to its default state.
        /// </summary>
        /// <param name="actionName">The name of the action whose binding needs to be reset.</param>
        /// <param name="bindingIndex">The index of the binding to reset within the action.</param>
        public void ResetBinding(string actionName, int bindingIndex)
        {
            accRemapControlsManager.ResetBinding(actionName, bindingIndex);
        }
        
        /// <summary>
        /// Resets all bindings to their default settings using the AccRebindControlsManager.
        /// </summary>
        public void ResetAllBindings()
        {
            accRemapControlsManager.ResetAllBindings();
        }
        
        /// <summary>
        /// Loads the user's preferences for remapped controls.
        /// </summary>
        public void LoadUserPreferencesRemapControls()
        {
            accRemapControlsManager.LoadRemapControlsSettings();
        }
        
        /// <summary>
        /// Resets all bindings associated with a specific control scheme to their default settings.
        /// </summary>
        /// <param name="controlScheme">The name of the control scheme to reset.</param>
        public void ResetControlSchemeBindings(string controlScheme)
        {
            accRemapControlsManager.ResetControlSchemeBindings(controlScheme);
        }
    }
}