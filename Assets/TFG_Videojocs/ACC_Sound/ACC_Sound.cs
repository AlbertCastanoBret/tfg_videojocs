using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Serialization;

[System.Serializable]
public class ACC_Sound
{
    [ACC_ReadOnly] public string name;
    public AudioClip audioClip;
    //public bool needsVisualNotification;
}
