using System.Collections.Generic;
using TFG_Videojocs.ACC_HighContrast;
using TFG_Videojocs.ACC_Utilities;
using UnityEngine;

namespace TFG_Videojocs
{
    public enum VisibilityFeatures
    {
        HighContrast,
    }
    
    public class ACC_VisualAccessibility
    {
        private ACC_HighContrastManager accHighContrastManager;
        
        public ACC_VisualAccessibility()
        {
            accHighContrastManager = ACC_PrefabHelper.InstantiatePrefabAsChild("HighContrast", ACC_AccessibilityManager.Instance.accCanvas).GetComponent<ACC_HighContrastManager>();
        }
        
        /// <summary>
        /// Sets the state of a specified visibility feature to either enabled or disabled.
        /// </summary>
        /// <param name="feature">The visibility feature to modify. Use VisibilityFeatures enum to specify the feature.</param>
        /// <param name="state">A boolean value indicating whether the feature should be enabled (true) or disabled (false).</param>
        public void SetFeatureState(VisibilityFeatures feature, bool state)
        {
            switch (feature)
            {
                case VisibilityFeatures.HighContrast:
                    accHighContrastManager.SetHighContrastMode(state);
                    break;
            }
        }
        
        /// <summary>
        /// Retrieves the enabled state of a specified visibility feature.
        /// </summary>
        /// <param name="feature">The visibility feature to check, e.g., high contrast.</param>
        /// <returns>True if the specified feature is enabled, false otherwise.</returns>
        public bool GetFeatureState(VisibilityFeatures feature)
        {
            switch (feature)
            {
                case VisibilityFeatures.HighContrast:
                    return PlayerPrefs.HasKey(ACC_AccessibilitySettingsKeys.HighContrastEnabled) && PlayerPrefs.GetInt(ACC_AccessibilitySettingsKeys.HighContrastEnabled) == 1;
                default:
                    return false;
            }
        }

        /// <summary>
        /// Changes the high contrast configuration based on the provided JSON configuration file.
        /// </summary>
        /// <param name="configuration">The name of the configuration file within the 'ACC_HighContrast' directory.</param>
        public void ChangeHighContrastConfiguration(string configuration)
        {
            accHighContrastManager.ChangeHighContrastConfiguration(configuration);
        }
        
        /// <summary>
        /// Retrieves a list of available high contrast configurations.
        /// </summary>
        /// <returns>A list of strings representing the names of available high contrast configurations.</returns>
        public List<string> GetHighContrastConfigurations()
        {
            return accHighContrastManager.GetHighContrastConfigurations();
        }
        
        public void ResetHighContrastConfiguration()
        {
            accHighContrastManager.ResetHighContrastConfiguration();
        }
    }
}