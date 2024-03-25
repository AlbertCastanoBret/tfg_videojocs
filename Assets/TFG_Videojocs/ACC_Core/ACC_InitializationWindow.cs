using System;
using System.Collections;
using System.Collections.Generic;
using TFG_Videojocs;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class ACC_InitializationWindow : EditorWindow
{
    [MenuItem("Tools/ACC/Initialization Window")]
    public static void ShowWindow()
    {
        var window = GetWindow<ACC_InitializationWindow>("Initialization Window");
        window.minSize = new Vector2(400, 600);
        window.maxSize = new Vector2(400, 600);
    }

    private void CreateGUI()
    {
        var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/TFG_Videojocs/ACC_Core/ACC_InitializationWindow.uss");

        var title = new Label("Accessibility Plugin");
        title.AddToClassList("title");
        
        var texture = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/TFG_Videojocs/ACC_Utilities/aiLogo.png");
        var image = new Image() {image = texture};
        image.AddToClassList("image");

        var text = new TextElement(){text = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum."};
        text.AddToClassList("text");
        
        var createButton = new Button() { text = "Create" };
        createButton.AddToClassList("button");
        createButton.clicked += () =>
        {
            if (!GameObject.Find("ACC_AccessibilityManager"))
            {
                GameObject accessibilityManager = new GameObject("ACC_AccessibilityManager");
                accessibilityManager.AddComponent<ACC_AccessibilityManager>();
            }
            
            if (!GameObject.Find("ACC_AudioManager"))
            {
                GameObject audioManager = new GameObject("ACC_AudioManager");

                GameObject musicSource = new GameObject("ACC_MusicSource");
                musicSource.AddComponent<AudioSource>();
                musicSource.transform.SetParent(audioManager.transform);
                
                GameObject sfxSource = new GameObject("ACC_SFXSource");
                sfxSource.AddComponent<AudioSource>();
                sfxSource.transform.SetParent(audioManager.transform);
                
                audioManager.AddComponent<ACC_AudioManager>();
            }
        };
        
        rootVisualElement.styleSheets.Add(styleSheet);
        rootVisualElement.Add(title);
        rootVisualElement.Add(image);
        rootVisualElement.Add(text);
        rootVisualElement.Add(createButton);
        rootVisualElement.AddToClassList("root");
    }
}