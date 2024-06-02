#if UNITY_EDITOR
using ACC_API;
using TFG_Videojocs.ACC_Utilities;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using Button = UnityEngine.UIElements.Button;
using Image = UnityEngine.UIElements.Image;

[InitializeOnLoad]
internal class ACC_InitializationWindow : EditorWindow
{
    private const string FirstTimeKey = "ACC_InitializationWindow_FirstTime";

    static ACC_InitializationWindow()
    {
        EditorApplication.update += ShowWindowOnFirstTime;
    }

    private static void ShowWindowOnFirstTime()
    {
        if (!EditorPrefs.HasKey(FirstTimeKey))
        {
            ShowWindow();
            EditorPrefs.SetBool(FirstTimeKey, true);
        }
        EditorApplication.update -= ShowWindowOnFirstTime;
    }
    
    [MenuItem("Tools/ACC/Initialization Window")]
    public static void ShowWindow()
    {
        var window = GetWindow<ACC_InitializationWindow>("Initialization Window");
        window.minSize = new Vector2(500, 600);
        window.maxSize = new Vector2(500, 600);
    }

    private void CreateGUI()
    {
        var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/TFG_Videojocs/ACC_Core/ACC_InitializationWindow.uss");

        var title = new Label("Accessibility Pro Toolkit");
        title.AddToClassList("title");
        
        var texture = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/TFG_Videojocs/ACC_Utilities/ACC_Logo.png");
        
        var image = new Image() {image = texture};
        image.AddToClassList("image");
        
        var subtitle0 = new Label("Welcome to the ACC_Accessibility Manager Setup!");
        subtitle0.AddToClassList("subtitle");
        var subtitleText = new TextElement() {text = "To begin using the asset and take advantage of all the accessibility features, follow these steps:"};
        subtitleText.style.marginBottom = 12;

        var instructionsContainer = new ScrollView(ScrollViewMode.Vertical);
        instructionsContainer.AddToClassList("instructions-container");
        
        var subtitle1 = new Label("0. Required Libraries");
        subtitle1.AddToClassList("subtitle");

        var step0Container = new VisualElement();
        step0Container.AddToClassList("list-container");
        var step0Label = new Label("URP: ");
        step0Label.AddToClassList("list-label");
        step0Label.style.width = 100;
        var step0 = new TextElement() {text = "The asset requires the Universal Render Pipeline (URP) to work correctly. If you are not using URP, you will need to switch to it."};
        step0Container.Add(step0Label);
        step0Container.Add(step0);
        
        var step0_1Container = new VisualElement();
        step0_1Container.AddToClassList("list-container");
        var step0_1Label = new Label("Input System: ");
        step0_1Label.AddToClassList("list-label");
        step0_1Label.style.width = 100;
        var step0_1 = new TextElement() {text = "The asset requires the Input System package to work correctly. If you are not using the Input System, you will need to install it."};
        step0_1Container.Add(step0_1Label);
        step0_1Container.Add(step0_1);
        
        var subtitle2 = new Label("1. Creating the ACC_Accessibility Manager Object");
        subtitle2.AddToClassList("subtitle");
        
        var step1Container = new VisualElement();
        step1Container.AddToClassList("list-container");
        var step1Label = new Label("Step 1: ");
        step1Label.AddToClassList("list-label");
        step1Label.style.width = 50;
        var step1 = new TextElement() {text = "First, you need to create an object in the scene that contains the ACC_Accessibility Manager. This is essential to utilize all the accessibility tools provided by the asset."};
        step1Container.Add(step1Label);
        step1Container.Add(step1);
        
        var step2Container = new VisualElement();
        step2Container.AddToClassList("list-container");
        var step2Label = new Label("Step 2: ");
        step2Label.AddToClassList("list-label");
        step2Label.style.width = 50;
        var step2 = new TextElement() {text = "To create this object, you can use the button at the bottom of this window or the Refresh Accessibility Manager button in the Accessibility Window. When refreshing Accessibility Manager, shaders will be disabled and you will have to enable them again in high-contrast settings. Also, the Input Action Asset will be removed from the inspector and you will have to re-assign it."};
        step2Container.Add(step2Label);
        step2Container.Add(step2);
        step2Container.style.marginBottom = 12;
        
        var subtitle3 = new Label("2. Configuring and Using the Accessibility Window");
        subtitle3.AddToClassList("subtitle");
        
        var extraText = new TextElement
        {
            text =
                "<b>Accessibility Window: </b> The Accessibility Window is a powerful tool that allows you to configure and customize the accessibility features of your game. You can open it by going to Tools > ACC > Accessibility Window.",
            enableRichText = true
        };
        extraText.style.marginBottom = 6;
        
        var step3Container = new VisualElement();
        step3Container.AddToClassList("list-container");
        var step3Label = new Label("Audio Management: ");
        step3Label.AddToClassList("list-label");
        var step3 = new TextElement() {text = "Allows you to add, control and adjust the volumes and audio sources in the scene, including the ability to add 3D audio sources."};
        step3Container.Add(step3Label);
        step3Container.Add(step3);
        
        var step4Container = new VisualElement();
        step4Container.AddToClassList("list-container");
        var step4Label = new Label("High Contrast: ");
        step4Label.AddToClassList("list-label");
        var step4 = new TextElement() {text = "Enables you to change the color scheme of your game to high contrast mode, making it easier for players with visual impairments to see the content."};
        step4Container.Add(step4Label);
        step4Container.Add(step4);
        
        var step5Container = new VisualElement();
        step5Container.AddToClassList("list-container");
        var step5Label = new Label("Subtitles: ");
        step5Label.AddToClassList("list-label");
        var step5 = new TextElement() {text = "Allows you to add and customize subtitles for your game, including the ability to change the font, size, and color."};
        step5Container.Add(step5Label);
        step5Container.Add(step5);
        
        var step6Container = new VisualElement();
        step6Container.AddToClassList("list-container");
        var step6Label = new Label("Visual Notifications: ");
        step6Label.AddToClassList("list-label");
        var step6 = new TextElement() {text = "Enables you to add visual notifications to your game, such as screen flashes, color changes, and text pop-ups, to alert players of important events."};
        step6Container.Add(step6Label);
        step6Container.Add(step6);
        
        var step7Container = new VisualElement();
        step7Container.AddToClassList("list-container");
        var step7Label = new Label("Remap Controls: ");
        step7Label.AddToClassList("list-label");
        var step7 = new TextElement() {text = "Allows you to remap the controls of your game, including the ability to change the key bindings, button mappings, and input methods."};
        step7Container.Add(step7Label);
        step7Container.Add(step7);
        
        var subtitle4 = new Label("3. Customization through the Inspector");
        subtitle4.AddToClassList("subtitle");
        var extraText2 = new TextElement
        {
            text =
                "From the inspector of the <b>ACC_Accessibility Manager</b>, you can:",
            enableRichText = true
        };
        extraText2.style.marginBottom = 6;
        
        var step8Container = new VisualElement();
        step8Container.AddToClassList("list-container");
        var step8Label = new Label("1. ");
        step8Label.AddToClassList("list-label");
        step8Label.style.width = 20;
        var step8 = new TextElement() {text = "Activate or deactivate each accessibility feature."};
        step8Container.Add(step8Label);
        step8Container.Add(step8);
        
        var step9Container = new VisualElement();
        step9Container.AddToClassList("list-container");
        var step9Label = new Label("2. ");
        step9Label.AddToClassList("list-label");
        step9Label.style.width = 20;
        var step9 = new TextElement() {text = "Display the options menu for each feature, allowing for fine-tuned control and configuration."};
        step9Container.Add(step9Label);
        step9Container.Add(step9);
        
        var step10Container = new VisualElement();
        step10Container.AddToClassList("list-container");
        var step10Label = new Label("3. ");
        step10Label.AddToClassList("list-label");
        step10Label.style.width = 20;
        var step10 = new TextElement()
        {
            text = "<b>Remap Controls:</b> In addition to the standard options, you can choose the input action asset and change the menu displayed in a selector. This is because there will be a different menu for each control scheme (e.g., keyboard & mouse, gamepad).",
            enableRichText = true
        };
        step10Container.Add(step10Label);
        step10Container.Add(step10);
        
        var step11Container = new VisualElement();
        step11Container.AddToClassList("list-container");
        var step11Label = new Label("4. ");
        step11Label.AddToClassList("list-label");
        step11Label.style.width = 20;
        var step11 = new TextElement()
        {
            text = "Delete the keys created during testing, allowing for easy resetting and reconfiguration.",
            enableRichText = true
        };
        step11Container.Add(step11Label);
        step11Container.Add(step11);
        
        var subtitle5 = new Label("4. Persistence of Settings");
        subtitle5.AddToClassList("subtitle");
        var extraText3 = new TextElement
        {
            text =
                "The plugin works with <b>PlayerPrefs</b>, so the values entered for each feature will be saved. This functionality is available both in the editor and in the build. However, in the editor, developers can delete the keys created during testing from the inspector, allowing for easy resetting and reconfiguration.",
            enableRichText = true
        };
        extraText3.style.marginBottom = 12;
        
        var subtitle6 = new Label("5. Settings Templates for Each Feature");
        subtitle6.AddToClassList("subtitle");
        var extraText4 = new TextElement
        {
            text =
                "Each feature comes with its own configuration window template. These templates allows the player to adjust settings specific to each feature, such as font size, text color, background color, audio source options, and more. Also, This enables you to fine-tune the settings according to your needs and preferences.",
            enableRichText = true
        };
        extraText4.style.marginBottom = 12;
        
        var subtitle7 = new Label("Notes");
        subtitle7.AddToClassList("subtitle");
        var extraText5 = new TextElement
        {
            text =
                "When importing the package into the project, the necessary tags and layers are also included. Ensure these are not removed. Additionally, a pre-configured render pipeline asset is imported and automatically set in the project settings. However, you can create your own URP Asset as long as the renderer within the URP Asset is configured to use the Forward rendering path. If this is not set up correctly, the high contrast mode will not function properly.",
            enableRichText = true
        };
        extraText5.style.marginBottom = 12;
        
        var createButton = new Button() { text = "Create" };
        createButton.AddToClassList("button");
        createButton.clicked += () =>
        {
            CreateAccessibilityManager();
            Close();
        };
        
        rootVisualElement.styleSheets.Add(styleSheet);
        rootVisualElement.Add(title);
        rootVisualElement.Add(image);
        
        instructionsContainer.Add(subtitle0);
        instructionsContainer.Add(subtitleText);
        
        instructionsContainer.Add(subtitle1);
        instructionsContainer.Add(step0Container);
        instructionsContainer.Add(step0_1Container);
        
        instructionsContainer.Add(subtitle2);
        instructionsContainer.Add(step1Container);
        instructionsContainer.Add(step2Container);
        
        instructionsContainer.Add(subtitle3);
        instructionsContainer.Add(extraText);
        instructionsContainer.Add(step3Container);
        instructionsContainer.Add(step4Container);
        instructionsContainer.Add(step5Container);
        instructionsContainer.Add(step6Container);
        instructionsContainer.Add(step7Container);
        
        instructionsContainer.Add(subtitle4);
        instructionsContainer.Add(extraText2);
        instructionsContainer.Add(step8Container);
        instructionsContainer.Add(step9Container);
        instructionsContainer.Add(step10Container);
        instructionsContainer.Add(step11Container);
        
        instructionsContainer.Add(subtitle5);
        instructionsContainer.Add(extraText3);
        
        instructionsContainer.Add(subtitle6);
        instructionsContainer.Add(extraText4);
        
        instructionsContainer.Add(subtitle7);
        instructionsContainer.Add(extraText5);
        
        rootVisualElement.Add(instructionsContainer);
        rootVisualElement.Add(createButton);
        rootVisualElement.AddToClassList("root");
    }
    
    private static void CreateAccessibilityManager()
    {
        var accessibilityManager = GameObject.Find("ACC_AccessibilityManager");
        if (accessibilityManager) DestroyImmediate(accessibilityManager);
        accessibilityManager = new GameObject("ACC_AccessibilityManager");
        var accessibilityManagerComponent = accessibilityManager.AddComponent<ACC_AccessibilityManager>();
        
        ACC_PrefabHelper.CreatePrefab("Subtitles");
        ACC_PrefabHelper.CreatePrefab("VisualNotification");
        ACC_PrefabHelper.CreatePrefab("HighContrast");
        Material highContrastColorMaterial = AssetDatabase.LoadAssetAtPath<Material>("Assets/TFG_Videojocs/ACC_HighContrast/High-Contrast-Color.mat");
        Material highContrastOutlineMaterial = AssetDatabase.LoadAssetAtPath<Material>("Assets/TFG_Videojocs/ACC_HighContrast/High-Contrast_Outline.mat");
        accessibilityManagerComponent.highContrastColorMaterial = highContrastColorMaterial;
        accessibilityManagerComponent.highContrastOutlineMaterial = highContrastOutlineMaterial;
        
        var loadedData = ACC_JSONHelper.LoadJson<ACC_AudioManagerData>("ACC_AudioManager/ACC_AudioManager");
        if (loadedData == null)
        {
            var path = "/ACC_AudioManager/";
            ACC_AudioManagerData data = new ACC_AudioManagerData();
            ACC_JSONHelper.CreateJson(data, path);
        }
        ACC_PrefabHelper.CreatePrefab("Audio", "ACC_AudioManager");
    }
}
#endif
