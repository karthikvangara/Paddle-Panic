using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="RiverMapsSO", menuName ="RiverMapsSO")]
public class RiverMapsSO : ScriptableObject
{
    public List<RiverMap> riverMapPrefabsFromEasyToHard=new List<RiverMap>();
}

[System.Serializable]
public class RiverMap
{
    public GameObject Map;
    public RiverMapDifficulty mapDifficulty;
    public GameObject SceneInstance;
}

public enum RiverMapDifficulty
{
    Easy,
    Medium,
    Hard
}
