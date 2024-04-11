using System;
using System.Collections.Generic;
using TFG_Videojocs.ACC_Utilities;
using UnityEditor;
using UnityEditor.Compilation;
using UnityEngine;
using UnityEngine.UIElements;
using Object = UnityEngine.Object;

namespace TFG_Videojocs
{
    public abstract class ACC_BaseFloatingWindow<TController,TWindow, TData>: EditorWindow where TController : ACC_FloatingWindowController<TWindow, TData>, new() where TWindow : EditorWindow where TData : ACC_AbstractData, new()
    {
        protected TController controller;
        protected ACC_UIElementFactory uiElementFactory => controller.uiElementFactory;

        protected void OnEnable()
        {
            controller = new TController();
            controller.Initialize(this as TWindow);
            CompilationPipeline.compilationStarted += controller.SerializeDataForCompilation;
        }

        private void OnDisable()
        {
            CompilationPipeline.compilationStarted -= controller.SerializeDataForCompilation;
        }

        protected void CreateGUI()
        {
            rootVisualElement.name = "root-visual-element";
            ColorUtility.TryParseHtmlString("#4f4f4f", out var backgroundColor);
            rootVisualElement.style.backgroundColor = new StyleColor(backgroundColor);
        }

        private void OnGUI()
        {
            //Debug.Log(controller.currentData.name);
        }

        public void CloneWindowAttributes<T>(T sourceWindow) where T : ACC_BaseFloatingWindow<TController, TWindow, TData>
        {
            foreach (var item in sourceWindow.controller.uiElementFactory.nameCounters)
            {
                controller.uiElementFactory.nameCounters[item.Key] = item.Value;
            }
            controller.CloneControllerAttributes(sourceWindow.controller);
        }
    }
}