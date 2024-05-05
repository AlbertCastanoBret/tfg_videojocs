using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TFG_Videojocs.ACC_HighContrast;
using TFG_Videojocs.ACC_RemapControls;
using TFG_Videojocs.ACC_Sound;
using TFG_Videojocs.ACC_Sound.ACC_Example;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Samples.RebindUI;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace TFG_Videojocs.ACC_Utilities
{
    public class ACC_PrefabHelper
    {
        public static void CreatePrefab(string feature, string jsonFile = "")
        {
            if(Application.isPlaying) return;
            GameObject gameObject = CreateGameObject(feature, jsonFile);
            gameObject.transform.SetParent(GameObject.Find("ACC_Canvas").transform, false);
            var folder = "ACC_" + feature + "/";
            var name = "ACC_" + feature + "Manager.prefab";
            
            Directory.CreateDirectory("Assets/Resources/ACC_Prefabs/" + folder);
            
            var prefabPath = "Assets/Resources/ACC_Prefabs/" + folder + name;
            PrefabUtility.SaveAsPrefabAsset(gameObject, prefabPath);
            GameObject.DestroyImmediate(gameObject);
            AssetDatabase.Refresh();
        }
        public static GameObject InstantiatePrefabAsChild(string feature, GameObject parent, string jsonFile="")
        {
            var folder = "ACC_" + feature + "/";
            var name = "ACC_" + feature + "Manager";
            var prefabPath = "ACC_Prefabs/" + folder + name;
            
            GameObject prefab = Resources.Load<GameObject>(prefabPath);
            if (prefab == null)
            {
                Debug.Log("Prefab couldn't be loaded in " + prefabPath);
                return null;
            }

            return GameObject.Instantiate(prefab, parent.transform);
        }
        private static GameObject CreateGameObject(string feature, string jsonFile = "")
        {
            string name = "ACC_" + feature + "Manager";
            GameObject gameObject = new GameObject(name);
            
            switch (feature)
            {
                case "Subtitles":
                    CreateSubtitleManager(gameObject);
                    break;
                case "VisualNotification":
                    CreateVisualNotificationManager(gameObject);
                    break;
                case "HighContrast":
                    gameObject.AddComponent<ACC_HighContrastManager>();
                    break;
                case "RemapControls":
                    CreateRemapControlsManager(gameObject, jsonFile);
                    break;
                case "Audio":
                    CreateAudioManager(gameObject, jsonFile);
                    break;
            }
            return gameObject;
        }
        public static bool FileNameAlreadyExists(string feature, string name)
        {
            var folder = "ACC_" + feature + "/";
            string filePath = "Assets/Resources/ACC_Prefabs/" + folder + name + ".prefab";
            return File.Exists(filePath);
        }
        private static void CreateSubtitleManager(GameObject subtitleManager)
        {
            RectTransform subtitleManagerTextRectTransform = subtitleManager.AddComponent<RectTransform>();
            subtitleManagerTextRectTransform.anchorMin = new Vector2(0.1f, 0);
            subtitleManagerTextRectTransform.anchorMax = new Vector2(0.9f, 0);
            subtitleManagerTextRectTransform.pivot = new Vector2(0.5f, 0);
            subtitleManagerTextRectTransform.anchoredPosition = new Vector2(0, 50);
            subtitleManagerTextRectTransform.sizeDelta = new Vector2(0, 40);
            
            var subtitleBackground = new GameObject("ACC_SubtitleBackground");
            subtitleBackground.transform.SetParent(subtitleManager.transform, false);
            var backgroundColorImage = subtitleBackground.AddComponent<UnityEngine.UI.Image>();
            backgroundColorImage.color = new Color(0, 0, 0, 0);
            
            RectTransform backgroundTextRectTransform = subtitleBackground.GetComponent<RectTransform>();
            backgroundTextRectTransform.anchorMin = new Vector2(0, 0.5f);
            backgroundTextRectTransform.anchorMax = new Vector2(1, 0.5f);
            backgroundTextRectTransform.pivot = new Vector2(0.5f, 0.5f);
            backgroundTextRectTransform.anchoredPosition = new Vector2(0, 0);
            backgroundTextRectTransform.sizeDelta = new Vector2(0, 40);

            var subtitleText = new GameObject("ACC_SubtitleText");
            subtitleText.transform.SetParent(subtitleManager.transform, false);

            TextMeshProUGUI subtitleTextMeshProUGUI = subtitleText.AddComponent<TextMeshProUGUI>();
            subtitleTextMeshProUGUI.font = Resources.Load<TMP_FontAsset>("Fonts & Materials/LiberationSans SDF");
            subtitleTextMeshProUGUI.alignment = TextAlignmentOptions.MidlineLeft;
            subtitleTextMeshProUGUI.enableWordWrapping = true;
            subtitleTextMeshProUGUI.color = new Color(1f, 0f, 0f, 1);

            RectTransform subtitleTextRectTransform = subtitleText.GetComponent<RectTransform>();
            subtitleTextRectTransform.anchorMin = new Vector2(0, 0.5f);
            subtitleTextRectTransform.anchorMax = new Vector2(1, 0.5f);
            subtitleTextRectTransform.pivot = new Vector2(0.5f, 0.5f);
            subtitleTextRectTransform.anchoredPosition = new Vector2(0, 0);
            subtitleTextRectTransform.sizeDelta = new Vector2(0, 40);
            
            subtitleManager.AddComponent<ACC_SubtitlesManager>();
        }
        private static void CreateVisualNotificationManager(GameObject visualNotificationManager)
        {
            RectTransform visualNotificationManagerTextRectTransform = visualNotificationManager.AddComponent<RectTransform>();
            visualNotificationManagerTextRectTransform.anchorMin = new Vector2(0.1f, 1);
            visualNotificationManagerTextRectTransform.anchorMax = new Vector2(0.5f, 1);
            visualNotificationManagerTextRectTransform.pivot = new Vector2(0.5f, 0.5f);
            visualNotificationManagerTextRectTransform.anchoredPosition = new Vector2(0, -100);
            visualNotificationManagerTextRectTransform.sizeDelta = new Vector2(0, 100);
            
            var visualNotificationBackground = new GameObject("ACC_VisualNotificationBackground");
            visualNotificationBackground.transform.SetParent(visualNotificationManager.transform, false);
            var backgroundColorImage = visualNotificationBackground.AddComponent<UnityEngine.UI.Image>();
            backgroundColorImage.color = new Color(1, 1, 1, 0);
            
            RectTransform backgroundTextRectTransform = visualNotificationBackground.GetComponent<RectTransform>();
            backgroundTextRectTransform.anchorMin = new Vector2(0, 0.5f);
            backgroundTextRectTransform.anchorMax = new Vector2(1, 0.5f);
            backgroundTextRectTransform.pivot = new Vector2(0.5f, 0.5f);
            backgroundTextRectTransform.anchoredPosition = new Vector2(0, 0);
            backgroundTextRectTransform.sizeDelta = new Vector2(0, 100);
    
            var visualNotificationText = new GameObject("ACC_VisualNotificationText");
            visualNotificationText.transform.SetParent(visualNotificationManager.transform, false);
            
            TextMeshProUGUI visualNotificationTextMeshProUGUI = visualNotificationText.AddComponent<TextMeshProUGUI>();
            visualNotificationTextMeshProUGUI.font = Resources.Load<TMP_FontAsset>("Fonts & Materials/LiberationSans SDF");
            visualNotificationTextMeshProUGUI.alignment = TextAlignmentOptions.MidlineLeft;
            visualNotificationTextMeshProUGUI.enableWordWrapping = true;
            visualNotificationTextMeshProUGUI.color = new Color(1f, 0f, 0f, 0);
            
            RectTransform visualNotificationTextRectTransform = visualNotificationText.GetComponent<RectTransform>();
            visualNotificationTextRectTransform.anchorMin = new Vector2(0, 0.5f);
            visualNotificationTextRectTransform.anchorMax = new Vector2(1, 0.5f);
            visualNotificationTextRectTransform.pivot = new Vector2(0.5f, 0.5f);
            visualNotificationTextRectTransform.anchoredPosition = new Vector2(0, 0);
            visualNotificationTextRectTransform.sizeDelta = new Vector2(0, 100);
    
            visualNotificationManager.AddComponent<ACC_VisualNotificationManager>();
        }
        private static void CreateRemapControlsManager(GameObject remapControlsManager, string jsonFile)
        {
            RectTransform remapControlsManagerRectTransform = remapControlsManager.AddComponent<RectTransform>();
            remapControlsManagerRectTransform.anchorMin = new Vector2(0, 0);
            remapControlsManagerRectTransform.anchorMax = new Vector2(1, 1);
            remapControlsManagerRectTransform.pivot = new Vector2(0.5f, 0.5f);
            remapControlsManagerRectTransform.anchoredPosition = Vector3.zero;
            remapControlsManagerRectTransform.sizeDelta = new Vector2(0, 0);
    
            var remapControlsManagerComponent = remapControlsManager.AddComponent<ACC_RemapControlsManager>();
            remapControlsManagerComponent.defaultMenu = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/TFG_Videojocs/ACC_RemapControls/ACC_DeviceManager.prefab");
            remapControlsManagerComponent.rebindUIPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/TFG_Videojocs/ACC_RemapControls/Rebinding UI/RebindUIPrefab.prefab");
            remapControlsManagerComponent.rebindOverlay = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/TFG_Videojocs/ACC_RemapControls/WaitForInputPanel.prefab");
            
            var waitForInputPanel = GameObject.Instantiate(remapControlsManagerComponent.rebindOverlay, remapControlsManager.transform, true);
            waitForInputPanel.transform.localScale = new Vector3(1, 1, 1);
            waitForInputPanel.transform.localPosition = new Vector3(0, 0, 0);
            
            try
            {
                var loadedData = ACC_JSONHelper.LoadJson<ACC_ControlSchemeData>("ACC_ControlSchemesConfiguration/" + jsonFile);
                remapControlsManagerComponent.loadedData = loadedData;
                SetControlSchemes(loadedData, remapControlsManager);
            }
            #pragma warning disable 0168
            catch (Exception e)
            {
            }
            #pragma warning restore 0168
        }
        private static void SetControlSchemes(ACC_ControlSchemeData loadedData, GameObject remapControlsManager)
        {
            var defaultMenu = remapControlsManager.GetComponent<ACC_RemapControlsManager>().defaultMenu;
            var devices = loadedData.inputActionAsset.controlSchemes
                .Select(scheme => 
                {
                    return scheme.deviceRequirements
                        .Select(requirement => requirement.controlPath.Replace("<", "").Replace(">", ""))
                        .Distinct()
                        .OrderBy(device => device)
                        .Aggregate((current, next) => current + ", " + next);
                })
                .Where(device => device != null)
                .Distinct()
                .ToList();
            
            bool first = true;

            foreach (var device in devices)
            {
                var deviceManager = GameObject.Instantiate(defaultMenu, remapControlsManager.transform, true);
                deviceManager.name = device;
                
                RectTransform rectTransform = deviceManager.GetComponent<RectTransform>();
                rectTransform.localScale = new Vector3(1, 1, 1);
                rectTransform.localPosition = new Vector3(0, 0, 0);
                rectTransform.sizeDelta = new Vector2(0, 0);
                
                deviceManager.transform.Find("Title").GetComponent<TextMeshProUGUI>().text =
                    "CUSTOMIZE CONTROLS - " + device.ToUpper();
                
                deviceManager.SetActive(first);
                first = false;
                
                var controlSchemes = loadedData.inputActionAsset.controlSchemes
                    .Where(scheme => String.Join(", ", scheme.deviceRequirements
                        .Select(requirement => requirement.controlPath.Replace("<", "").Replace(">", ""))
                        .Distinct()
                        .OrderBy(d => d)) == device)
                    .Select(scheme => scheme.name)
                    .ToList();
                
                deviceManager.transform.Find("ControlSchemeSelector").Find("CurrentControlSchemeParent").Find("CurrentControlScheme").GetComponent<TextMeshProUGUI>().text =
                    controlSchemes[0];
                
                SetBindings(loadedData, deviceManager, controlSchemes, remapControlsManager);
            }
        }
        private static void SetBindings(ACC_ControlSchemeData loadedData, GameObject deviceManager,
            List<string> controlSchemes, GameObject remapControlsManager)
        {
            var rebindUIPrefab = remapControlsManager.GetComponent<ACC_RemapControlsManager>().rebindUIPrefab;
            var rebindOverlay = remapControlsManager.GetComponent<ACC_RemapControlsManager>().rebindOverlay;
            var rebindPrompt = remapControlsManager.transform.GetChild(0).Find("WaitForInputText").GetComponent<TextMeshProUGUI>();
            
            Object[] subAssets = AssetDatabase.LoadAllAssetsAtPath(AssetDatabase.GetAssetPath(loadedData.inputActionAsset));
            List<InputActionReference> inputActionReferences = new List<InputActionReference>();
            foreach (Object obj in subAssets) {
                if (obj is InputActionReference inputActionReference && (inputActionReference.hideFlags & HideFlags.HideInHierarchy) == 0) {
                    inputActionReferences.Add(inputActionReference);
                }
            }
            
            bool first = true;
            foreach (var controlScheme in controlSchemes)
            {
                var rebindingsScroll = deviceManager.transform.Find("RebindsScroll");

                var controlSchemeParent = new GameObject
                {
                    name = controlScheme,
                    tag = "ACC_ScrollList"
                };
                var rectTransform = controlSchemeParent.AddComponent<RectTransform>();

                rectTransform.SetParent(rebindingsScroll);
                rectTransform.localScale = new Vector3(1, 1, 1);
                rectTransform.anchorMin = new Vector2(0.5f, 1);
                rectTransform.anchorMax = new Vector2(0.5f, 1);
                rectTransform.pivot = new Vector2(0.5f, 1);
                rectTransform.anchoredPosition = new Vector2(0, 0);

                if (first)
                {
                    rebindingsScroll.GetComponent<ScrollRect>().content = rectTransform;
                    first = false;
                }
                else controlSchemeParent.SetActive(false);

                var verticalLayoutGroup = controlSchemeParent.AddComponent<VerticalLayoutGroup>();
                verticalLayoutGroup.spacing = 0;

                var contentSizeFitter = controlSchemeParent.AddComponent<ContentSizeFitter>();
                contentSizeFitter.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
                contentSizeFitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;

                foreach (var binding in loadedData.bindingsList.Items)
                {
                    if (controlScheme == binding.key.controlScheme)
                    {
                        var uiPrefab = Object.Instantiate(rebindUIPrefab, controlSchemeParent.transform, true);
                        uiPrefab.name = binding.key.id;
                        uiPrefab.transform.localScale = new Vector3(1, 1, 1);

                        var rebindUI = uiPrefab.GetComponent<RebindActionUI>();
                        
                        InputAction action = loadedData.inputActionAsset.FindAction(binding.key.actionId);
                        InputActionReference actionReference = inputActionReferences
                            .Find(reference => reference.action.id == action.id);
                        actionReference.Set(action);
                        rebindUI.actionReference = actionReference;
                        rebindUI.bindingId = binding.key.id;
                        rebindUI.rebindOverlay = rebindOverlay;
                        rebindUI.rebindPrompt = rebindPrompt;
                        if (!binding.value || loadedData.controlSchemesList.Items
                                .Find(scheme => scheme.key == controlScheme).value == false)
                        {
                            var button = uiPrefab.transform.Find("TriggerRebindButton");
                            button.GetComponent<Image>().color = new Color(1, 1, 1, 0.4f);
                            button.GetComponent<Button>().enabled = false;

                            var resetButton = uiPrefab.transform.Find("ResetToDefaultButton");
                            resetButton.GetComponent<Image>().color = new Color(1, 1, 1, 0.4f);
                            resetButton.GetComponent<Button>().enabled = false;
                        }
                    }
                }
            }
        }
        private static void CreateAudioManager(GameObject audioManager, string jsonFile)
        {
            RectTransform audioManagerManagerRectTransform = audioManager.AddComponent<RectTransform>();
            audioManagerManagerRectTransform.anchorMin = new Vector2(0, 0);
            audioManagerManagerRectTransform.anchorMax = new Vector2(1, 1);
            audioManagerManagerRectTransform.pivot = new Vector2(0.5f, 0.5f);
            audioManagerManagerRectTransform.anchoredPosition = Vector3.zero;
            audioManagerManagerRectTransform.sizeDelta = new Vector2(0, 0);
            
            GameObject audioSettings = GameObject.Instantiate(AssetDatabase.LoadAssetAtPath<GameObject>("Assets/TFG_Videojocs/ACC_Sound/ACC_AudioSettings.prefab"), audioManager.transform, true);
            
            var loadedData = ACC_JSONHelper.LoadJson<ACC_AudioManagerData>("ACC_AudioManager/" + jsonFile);
            if(loadedData == null) return;
            
            var audioManagerComponent = audioManager.AddComponent<ACC_AudioManager>();
            audioManagerComponent.audioSources = loadedData.audioSources;
            
            GameObject audioSources = new GameObject("ACC_AudioSources");
            audioSources.transform.SetParent(audioManager.transform);
            
            for (int i = 0; i < loadedData.audioSources.Items.Count; i++)
            {
                if (loadedData.audioSources.Items[i].value.is3D)
                {
                    //Create3DAudioSource(loadedData.audioSources.Items[i].value);
                    continue;
                }
                CreateAudioSource(audioSources, audioSettings, loadedData.audioSources.Items[i].value);
            }

            for (int i = 0; i < loadedData.audioClips.Items.Count; i++)
            {
                if (loadedData.audioSources.Items[i].value.is3D) { continue; }
                var audioClips = new ACC_SerializableDictiornary<int, AudioClip>();
                for (int j = 0; j < loadedData.audioClips.Items[i].value.Items.Count; j++)
                {
                    var audioClipPath = AssetDatabase.GUIDToAssetPath(loadedData.audioClips.Items[i].value.Items[j].value);
                    var audioClip = AssetDatabase.LoadAssetAtPath<AudioClip>(audioClipPath);
                    audioClips.AddOrUpdate(j, audioClip);
                }
                audioManagerComponent.audioClips.AddOrUpdate(i, audioClips);
            }
        }
        private static void CreateAudioSource(GameObject audioSources, GameObject audioSettings, ACC_AudioSourceData audioSourceData)
        {
            GameObject audioSource = new GameObject {tag = "ACC_AudioSource", name = audioSourceData.name};
            audioSource.transform.SetParent(audioSources.transform);
                
            var audioSourceComponent = audioSource.AddComponent<AudioSource>();
            audioSourceComponent.volume = audioSourceData.volume;
            
            var audioSourcesList = audioSettings.transform.Find("AudioSourcesScroll").transform.Find("AudioSourcesList");
            var audioSourceSlider = GameObject.Instantiate(AssetDatabase.LoadAssetAtPath<GameObject>("Assets/TFG_Videojocs/ACC_Sound/ACC_AudioSourceVolumeSlider.prefab"), audioSourcesList.transform, true);
            
            audioSourceSlider.name = audioSourceData.name;
            audioSourceSlider.transform.Find("ACC_AudioSourceName").GetComponent<TextMeshProUGUI>().text = audioSourceData.name;
            
            var audioSourceSliderComponent = audioSourceSlider.transform.Find("ACC_AudioSourceVolumeSlider").GetComponent<Slider>();
            audioSourceSliderComponent.value = audioSourceData.volume;
            
            audioSourceSliderComponent.onValueChanged.AddListener((value) =>
            {
                audioSourceComponent.volume = value;
            });
        }

        public static void Create3DAudioSource(ACC_AudioSourceData audioSourceData)
        {
            var objectGUID = audioSourceData.sourceObjectGUID;
            var sourceObject = AssetDatabase.LoadAssetAtPath<GameObject>(AssetDatabase.GUIDToAssetPath(objectGUID));
            if (audioSourceData.prefabGUID != "") AssetDatabase.DeleteAsset(AssetDatabase.GUIDToAssetPath(audioSourceData.prefabGUID));

            GameObject parentObject;
            AudioSource audioSourceComponent;
            var gameObjectName = sourceObject != null ? sourceObject.name : "EmptyObject";
            
            if (sourceObject != null)
            {
                parentObject =  GameObject.Instantiate(sourceObject);
                
                GameObject audioSource = new GameObject {tag = "ACC_AudioSource", name = "ACC_AudioSource_" + gameObjectName + "_3D"};
                audioSource.transform.SetParent(parentObject.transform);
            
                audioSourceComponent = audioSource.AddComponent<AudioSource>();
            }
            else
            {
                audioSourceData.sourceObjectGUID = "-1";
                parentObject = new GameObject {tag = "ACC_AudioSource", name = "ACC_AudioSource_" + gameObjectName + "_3D"};

                audioSourceComponent = parentObject.AddComponent<AudioSource>();
            }
            
            audioSourceComponent.volume = audioSourceData.volume;
            audioSourceComponent.spatialBlend = 1;
            
            var folder = "ACC_Audio/ACC_3DAudioSources/";
            var name = "ACC_3DAS-" + audioSourceData.name + "_GO-" + gameObjectName + ".prefab"; 
            
            Directory.CreateDirectory("Assets/Resources/ACC_Prefabs/" + folder);
            var prefabPath = "Assets/Resources/ACC_Prefabs/" + folder + name;
            
            PrefabUtility.SaveAsPrefabAsset(parentObject, prefabPath);
            GameObject.DestroyImmediate(parentObject);
            AssetDatabase.Refresh();
            
            var guid = AssetDatabase.AssetPathToGUID(prefabPath);
            audioSourceData.prefabGUID = guid;
        }
        
        public static void Delete3DAudioSource(ACC_AudioSourceData audioSourceData)
        {
            if (string.IsNullOrEmpty(audioSourceData.prefabGUID)) return;
            var prefabPath = AssetDatabase.GUIDToAssetPath(audioSourceData.prefabGUID);
            AssetDatabase.DeleteAsset(prefabPath);
            AssetDatabase.Refresh();
            audioSourceData.prefabGUID = "";
            
        }
    }
}