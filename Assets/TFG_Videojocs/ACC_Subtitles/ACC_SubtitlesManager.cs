using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ACC_SubtitlesManager : MonoBehaviour
{
    private TextMeshProUGUI subtitleText;
    private Image backgroundColor;
    
    private bool canPlaySubtitle;
    private int currentIndex;
    private float startTime;
    private float nextSubtitleTime;
    private ACC_SubtitleData loadedData;

    private void Awake()
    {
        subtitleText = GameObject.Find("ACC_SubtitleManager/ACC_SubtitleText").GetComponent<TextMeshProUGUI>();
        backgroundColor = GameObject.Find("ACC_SubtitleManager/ACC_Background").GetComponent<Image>();
    }

    void Update()
    {
        if (canPlaySubtitle)
        {
            float currentTime = Time.time;
            if (currentTime >= nextSubtitleTime)
            {
                if (currentIndex < loadedData.subtitleText.Count)
                {
                    subtitleText.text = loadedData.subtitleText[currentIndex].value;
                    startTime = currentTime;
                    nextSubtitleTime = startTime + loadedData.timeText[currentIndex].value;
                    GetComponent<RectTransform>().sizeDelta =
                        new Vector2(0, subtitleText.preferredHeight);
                    subtitleText.GetComponent<RectTransform>().sizeDelta = 
                        new Vector2(0, subtitleText.preferredHeight);
                    backgroundColor.GetComponent<RectTransform>().sizeDelta =
                        new Vector2(0, subtitleText.preferredHeight);
                }
                else if (currentIndex >= loadedData.subtitleText.Count)
                {
                    currentIndex = -1;
                    canPlaySubtitle = false;
                    subtitleText.text = "";
                    backgroundColor.color = new Color(0, 0, 0, 0);
                }
                currentIndex++;
            }
        }
    }

    public void LoadSubtitles(string jsonFile)
    {
        string json = File.ReadAllText("Assets/TFG_Videojocs/ACC_JSON/ACC_JSONSubtitle/" + jsonFile + ".json");
        loadedData = JsonUtility.FromJson<ACC_SubtitleData>(json);
    }

    public void PlaySubtitle()
    {
        canPlaySubtitle = true;
        subtitleText.text = "";
        currentIndex = 0;
        
        subtitleText.color = new Color(loadedData.fontColor.r, loadedData.fontColor.g,
            loadedData.fontColor.b, loadedData.fontColor.a);
        backgroundColor.color = new Color(loadedData.backgroundColor.r, loadedData.backgroundColor.g,
            loadedData.backgroundColor.b, loadedData.backgroundColor.a);
        subtitleText.fontSize = loadedData.fontSize;
    }

    /*public void EndSubtitle()
    {
        canPlaySubtitle = false;
        subtitleText.text = "";
        currentIndex = 0;
    }*/
}
