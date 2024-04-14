using System;
using System.Collections.Generic;
using System.Linq;
using TFG_Videojocs.ACC_Utilities;
using TFG_Videojocs.ACC_VisualNotification;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

namespace TFG_Videojocs.ACC_RemapControls
{
    public class ACC_ControlSchemesConfigurationEditorWindow:ACC_BaseFloatingWindow<ACC_ControlSchemesConfigurationEditorWindowController, ACC_ControlSchemesConfigurationEditorWindow, ACC_ControlSchemeData>
    {
        public InputActionAsset inputActionAsset { get; private set; }
        private bool isReadyToCreateGUI;
        
        private ScrollView controlSchemesScrollView;

        public static void ShowWindow(InputActionAsset inputActionAsset)
        {
            ACC_ControlSchemesConfigurationEditorWindow window =
                GetWindow<ACC_ControlSchemesConfigurationEditorWindow>();
            window.titleContent = new GUIContent("Control Schemes Configuration");
            window.minSize = new Vector2(600, 450);
            window.maxSize = new Vector2(600, 450);
            window.inputActionAsset = inputActionAsset;
            window.controller.isEditing = true;
            window.isReadyToCreateGUI = true;
            window.CreateGUI();
            
            window.controller.isEditing = true;
            window.controller.LoadOnlyEditableWindow(inputActionAsset.name);
            
            //window.ShowModal();
        }

        private new void CreateGUI()
        {
            if (!isReadyToCreateGUI) return;
            base.CreateGUI();
            var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/TFG_Videojocs/ACC_RemapControls/ACC_ControlSchemesConfigurationEditorWindow.uss");
            rootVisualElement.styleSheets.Add(styleSheet);

            var controlSchemesLabel = uiElementFactory.CreateLabel("title", "Control Schemes");
            rootVisualElement.Add(controlSchemesLabel);
            
            CreateDropdownField();
            controlSchemesScrollView = new ScrollView();
            rootVisualElement.Add(controlSchemesScrollView);
            CreateTable();
            
            var createButton = new Button() { text = "Save" };
            createButton.AddToClassList("create-button");
            createButton.clicked += () => controller.HandleSave(this);
            
            rootVisualElement.Add(createButton);
        }

        private void CreateDropdownField()
        {
            var devices = inputActionAsset.controlSchemes
                .Select(scheme => 
                {
                    if (!scheme.deviceRequirements.Any())
                    {
                        return "All devices";
                    }

                    return scheme.deviceRequirements
                        .Select(requirement => requirement.controlPath.Replace("<", "").Replace(">", ""))
                        .Distinct()
                        .OrderBy(device => device)
                        .Aggregate((current, next) => current + ", " + next);
                })
                .Where(device => device != null)
                .Distinct()
                .ToList();
            
            devices.Insert(0, "All devices");
            var devicesDropdown = (DropdownField) uiElementFactory.CreateDropdownField("option-input", "Devices: ", 
                devices, "option-input-label"); 

            var controlSchemes = inputActionAsset.controlSchemes.Select(scheme => scheme.name).ToList();
            controlSchemes.ForEach(key => controller.onScreenControlSchemeToggleValues[key] = false);
            
            devicesDropdown.RegisterValueChangedCallback(evt =>
            {
                if (evt.newValue == "All devices")
                {
                    controlSchemes = inputActionAsset.controlSchemes.Select(scheme => scheme.name).ToList(); 
                    controller.onScreenControlSchemeToggleValues = new Dictionary<string, bool>();
                    controller.onScreenBindingToggleValues = new Dictionary<ACC_BindingData, bool>();
                    controlSchemes.ForEach(key => controller.onScreenControlSchemeToggleValues[key] = controller.currentData.controlSchemesList.Items.Find(item => item.key == key).value);
                }
                else
                {
                    controlSchemes = inputActionAsset.controlSchemes
                        .Where(scheme => String.Join(", ", scheme.deviceRequirements
                            .Select(requirement => requirement.controlPath.Replace("<", "").Replace(">", ""))
                            .Distinct()
                            .OrderBy(device => device)) == evt.newValue)
                        .Select(scheme => scheme.name)
                        .ToList();
                    controller.onScreenControlSchemeToggleValues = new Dictionary<string, bool>();
                    controller.onScreenBindingToggleValues = new Dictionary<ACC_BindingData, bool>();
                    controlSchemes.ForEach(key => controller.onScreenControlSchemeToggleValues[key] = controller.currentData.controlSchemesList.Items.Find(item => item.key == key).value);
                }
                CreateTable();
            });
            
            for (int i = 0; i < controlSchemes.Count(); i++)
            {
                controller.currentData.controlSchemesList.AddOrUpdate(controlSchemes[i], false);
            }
            
            controller.onScreenBindingToggleValues = new Dictionary<ACC_BindingData, bool>();
            foreach (var controlScheme in controlSchemes)
            {
                foreach (var actionMap in inputActionAsset.actionMaps)
                {
                    foreach (var action in actionMap.actions)
                    {
                        foreach (var binding in action.bindings)
                        {
                            string[] groups = binding.groups.Split(';');
                            foreach (string group in groups)
                            {
                                if (group == controlScheme)
                                {
                                    ACC_BindingData accBindingData = new ACC_BindingData(binding.id.ToString(), controlScheme);
                                    controller.onScreenBindingToggleValues[accBindingData] = false;
                                    controller.currentData.bindingsList.AddOrUpdate(accBindingData, false);
                                }
                            }
                        }
                    }
                }
            }
            rootVisualElement.Add(devicesDropdown);
        }
        
        public void CreateTable()
        {
            controlSchemesScrollView.Clear();
            var controlSchemesContainer = new VisualElement();
            controlSchemesContainer.AddToClassList("control-schemes-container");
            
            var controlSchemeTitle = uiElementFactory.CreateVisualElement("control-scheme-title");
            var controlSchemeNameTitle = uiElementFactory.CreateLabel("control-scheme-name-title", "Name");
            var controlSchemeRebindTitle = uiElementFactory.CreateLabel("control-scheme-rebind-title", "Rebind");
            
            controlSchemeTitle.Add(controlSchemeNameTitle);
            controlSchemeTitle.Add(controlSchemeRebindTitle);
            
            controlSchemesContainer.Add(controlSchemeTitle);
            
            for (int i=0; i<controller.onScreenControlSchemeToggleValues.Count; i++)
            {
                var controlScheme = uiElementFactory.CreateVisualElement("control-scheme");
                
                var controlSchemeFirstRow = uiElementFactory.CreateVisualElement("control-scheme-first-row");
                if (i == controller.onScreenControlSchemeToggleValues.Count - 1) controlSchemeFirstRow.AddToClassList("control-scheme-last");

                var arrowButton = uiElementFactory.CreateButton("\u25b6", "control-scheme-arrow");
                arrowButton.clicked += () => ToggleControlSchemeDisplay(arrowButton, controlScheme);
                
                var controlSchemeLabel = uiElementFactory.CreateLabel("control-scheme-label", controller.onScreenControlSchemeToggleValues.Keys.ElementAt(i));
                var controlSchemeToggleContainer = uiElementFactory.CreateVisualElement("control-scheme-toggle-container");
                
                var controlSchemeToggle = (Toggle) uiElementFactory.CreateToggle("control-scheme-toggle", "", controller.onScreenControlSchemeToggleValues.Values.ElementAt(i),
                    onValueChanged: value =>
                    {
                        controller.onScreenControlSchemeToggleValues[controlSchemeLabel.text] = value;
                        controller.currentData.controlSchemesList.AddOrUpdate(controlSchemeLabel.text, value);
                        for (int k = 1; k < controlScheme.childCount; k++)
                        {
                            for (int l = 1; l < controlScheme[k].childCount; l++)
                            {
                                for (int m = 1; m < controlScheme[k][l].childCount; m++)
                                {
                                    controlScheme[k][l][m].Query<Toggle>().First().SetEnabled(value);
                                }
                            }
                        }
                    });
                
                controlSchemeToggleContainer.Add(controlSchemeToggle);
                controlSchemeFirstRow.Add(arrowButton);
                controlSchemeFirstRow.Add(controlSchemeLabel);
                controlSchemeFirstRow.Add(controlSchemeToggleContainer);
                controlScheme.Add(controlSchemeFirstRow);
                controlSchemesContainer.Add(controlScheme);
                CreateActionMaps(controlScheme);
            }

            controlSchemesScrollView.Add(controlSchemesContainer);
        }

        private void CreateActionMaps(VisualElement parent)
        {
            var actionMaps = inputActionAsset.actionMaps;
            foreach (var actionMap in actionMaps)
            {
                var mainContainer = uiElementFactory.CreateVisualElement("control-scheme-action-map-container");
                mainContainer.style.display = DisplayStyle.None;
                    
                var firstRow = uiElementFactory.CreateVisualElement("control-scheme-action-map-first-row");
                var arrowButton = uiElementFactory.CreateButton("\u25b6", "control-scheme-arrow");
                arrowButton.clicked += () => ToggleControlSchemeDisplay(arrowButton, mainContainer);
                    
                var actionMapLabel = uiElementFactory.CreateLabel("control-scheme-action-map-label", actionMap.name);
                    
                firstRow.Add(arrowButton);
                firstRow.Add(actionMapLabel);
                mainContainer.Add(firstRow);
                parent.Add(mainContainer);
                CreateActions(mainContainer);
            }
        }
        
        private void CreateActions(VisualElement parent)
        {
            var actionMaps = inputActionAsset.actionMaps;
            var actionMap = actionMaps.FirstOrDefault(am => am.name == parent.Query<Label>().First().text);
            if (actionMap != null)
            {
                var actionMapActions = actionMap.actions;
                foreach (var action in actionMapActions)
                {
                    var mainContainer = uiElementFactory.CreateVisualElement("control-scheme-action-container");
                    mainContainer.style.display = DisplayStyle.None;
                    
                    var firstRow = uiElementFactory.CreateVisualElement("control-scheme-action-first-row");
                    
                    var arrowButton = uiElementFactory.CreateButton("\u25b6", "control-scheme-arrow");
                    arrowButton.clicked += () => ToggleControlSchemeDisplay(arrowButton, mainContainer);
                    
                    var actionLabel = uiElementFactory.CreateLabel("control-scheme-action-label", action.name);
                    
                    firstRow.Add(arrowButton);
                    firstRow.Add(actionLabel);
                    mainContainer.Add(firstRow);
                    parent.Add(mainContainer);
                    CreateBindings(mainContainer);
                }
            }
        }

        private void CreateBindings(VisualElement parent)
        {
            var actionMaps = inputActionAsset.actionMaps;
            var controlScheme = parent.parent.parent.Query<Label>().First().text;
            var actionMap = actionMaps.FirstOrDefault(am => am.name == parent.parent.Query<Label>().First().text);
            if (actionMap != null)
            {
                var actionMapActions = actionMap.actions;
                var action = actionMapActions.FirstOrDefault(a => a.name == parent.Query<Label>().First().text);
                if (action != null)
                {
                    var actionBindings = action.bindings;
                    foreach (var binding in actionBindings)
                    {
                        string[] groups = binding.groups.Split(';');
                        foreach (string group in groups)
                        {
                            if (group == controlScheme)
                            {
                                var presentablePath = MakePathPresentable(binding.path);
                                var mainContainer = uiElementFactory.CreateVisualElement("control-scheme-binding-container");
                                mainContainer.style.display = DisplayStyle.None;
                    
                                var firstRow = uiElementFactory.CreateVisualElement("control-scheme-binding-first-row");
                                
                                var bindingLabel = uiElementFactory.CreateLabel("control-scheme-binding-label", presentablePath);
                                
                                var bindingToggleContainer = uiElementFactory.CreateVisualElement("binding-toggle-container");
                                
                                ACC_BindingData accBindingData = controller.currentData.bindingsList.Items.Find(item => item.key.id == binding.id.ToString() && item.key.controlScheme == controlScheme).key;
                                if (accBindingData != null)
                                {
                                    controller.onScreenBindingToggleValues[accBindingData] = controller.currentData.bindingsList.Items.Find(item => item.key == accBindingData).value;
                                    var bindingToggle = uiElementFactory.CreateToggle("binding-toggle", "",
                                        controller.currentData.bindingsList.Items.Find(item => item.key == accBindingData).value,
                                        onValueChanged: value =>
                                        {
                                            controller.currentData.bindingsList.AddOrUpdate(accBindingData, value);
                                        });
                                    bindingToggle.SetEnabled(controller.currentData.controlSchemesList.Items.Find(item => item.key == controlScheme).value);
                                    bindingToggleContainer.Add(bindingToggle);
                                }

                                firstRow.Add(bindingLabel);
                                firstRow.Add(bindingToggleContainer);
                                mainContainer.Add(firstRow);
                                parent.Add(mainContainer);
                            }
                        }
                    }
                }
            }
        }
        
        private void ToggleControlSchemeDisplay(Button arrowButton, VisualElement controlScheme)
        {
            if (arrowButton.text == "\u25b6")
            {
                arrowButton.text = "\u25bc";
                for (int j = 1; j < controlScheme.childCount; j++)
                {
                    controlScheme[j].style.display = DisplayStyle.Flex;
                }
            }
            else
            {
                arrowButton.text = "\u25b6";
                for (int j = 1; j < controlScheme.childCount; j++)
                {
                    controlScheme[j].style.display = DisplayStyle.None;
                }
            }
        }
        
        private string MakePathPresentable(string path)
        {
            var parts = path.Split('/');
            var device = parts[0].Replace("<", "").Replace(">", "");
            var controlParts = parts[1].Split(new[] { '_' }, StringSplitOptions.RemoveEmptyEntries);
            
            for (int i = 0; i < controlParts.Length; i++)
            {
                controlParts[i] = char.ToUpper(controlParts[0][0]) + controlParts[0].Substring(1);
            }
            
            var control = string.Join(" ", controlParts);
            return $"{control} [{device}]";
        }
        
        private void LoadJson()
        {
            string path = "/ACC_RemapControls/" + inputActionAsset.name;
            ACC_ControlSchemeData accControlSchemeData = ACC_JSONHelper.LoadJson<ACC_ControlSchemeData>(path);
            if(accControlSchemeData != null)
            {
                /*for (int i=0; i<inputActionAsset.controlSchemes.Count; i++)
                {
                    if (accControlSchemeData.controlSchemesList.Count > i)
                    {
                        if (accControlSchemeData.controlSchemesList[i].key != inputActionAsset.controlSchemes[i].name &&
                            inputActionAsset.controlSchemes.Skip(i).Any(controlsScheme =>
                                controlsScheme.name == accControlSchemeData.controlSchemesList[i].key))
                        {
                            accControlSchemeData.controlSchemesList.Remove(accControlSchemeData.controlSchemesList[i]);
                            accControlSchemeData.controlSchemesList.Add(accControlSchemeData.controlSchemesList[i]);
                            while (accControlSchemeData.controlSchemesList[i].key != inputActionAsset.controlSchemes[i].name)
                            {
                                accControlSchemeData.controlSchemesList.Insert(i, new ACC_KeyValuePairData<string, bool>(inputActionAsset.controlSchemes[i].name, false));
                                i++;
                            
                                if (accControlSchemeData.controlSchemesList.Count-1 == i)
                                {
                                    break;
                                }
                            
                                if (accControlSchemeData.controlSchemesList[i].key ==
                                    inputActionAsset.controlSchemes[i].name)
                                {
                                    break;
                                }
                            }
                        }
                        else if (accControlSchemeData.controlSchemesList[i].key != inputActionAsset.controlSchemes[i].name &&
                                 inputActionAsset.controlSchemes.Take(i).Any(controlsScheme => controlsScheme.name == accControlSchemeData.controlSchemesList[i].key))
                        {
                            accControlSchemeData.controlSchemesList.Remove(accControlSchemeData.controlSchemesList[i]);
                            accControlSchemeData.controlSchemesList.Add(accControlSchemeData.controlSchemesList[i]);
                            while (accControlSchemeData.controlSchemesList[i].key != inputActionAsset.controlSchemes[i].name)
                            {
                                accControlSchemeData.controlSchemesList.Insert(i, new ACC_KeyValuePairData<string, bool>(inputActionAsset.controlSchemes[i].name, false));
                                i--;
                            
                                if (accControlSchemeData.controlSchemesList.Count-1 == i)
                                {
                                    break;
                                }
                            
                                if (accControlSchemeData.controlSchemesList[i].key ==
                                    inputActionAsset.controlSchemes[i].name)
                                {
                                    break;
                                }
                            }
                        }
                        else
                        {
                            accControlSchemeData.controlSchemesList[i].key = inputActionAsset.controlSchemes[i].name;
                        }
                    }
                    else
                    {
                        accControlSchemeData.controlSchemesList.Add(new ACC_KeyValuePairData<string, bool>(inputActionAsset.controlSchemes[i].name, false));
                    }
                }
                
                ACC_JSONHelper.CreateJson(accControlSchemeData, "/ACC_JSONRemapControls/");*/
                
                controller.onScreenControlSchemeToggleValues = new Dictionary<string, bool>();
                /*foreach (var scheme in accControlSchemeData.controlSchemesList)
                {
                    if (controlSchemeToggleValues.ContainsKey(scheme.key))
                    {
                        controlSchemeToggleValues[scheme.key] = scheme.value;
                        onScreenControlSchemeToggleValues[scheme.key] = scheme.value;
                    }
                }*/
                
                //bindingsToggleValues = new Dictionary<ACC_BindingData, bool>();
                //currentBindingsToggleValues = new Dictionary<ACC_BindingData, bool>();
                /*foreach (var binding in accControlSchemeData.bindingsList)
                {
                    bindingsToggleValues[binding.key] = binding.value;
                    currentBindingsToggleValues[binding.key] = binding.value;
                }*/
                
                //lastSaveControlSchemeToggleValues = new Dictionary<string, bool>(controlSchemeToggleValues);
                //lastSaveBindingsToggleValues = new Dictionary<ACC_BindingData, bool>(bindingsToggleValues);
                CreateTable();
            }
        }
    }
}