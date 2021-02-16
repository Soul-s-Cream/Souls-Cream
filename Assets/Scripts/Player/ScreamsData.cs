using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ScreamData
{
    public ScreamType scream;
    public Role unlockableBy;
    public AK.Wwise.Event sound;
    public Sprite iconUI;
}

[CreateAssetMenu]
public class ScreamsData : ScriptableObject
{
    public List<ScreamData> data;
}
