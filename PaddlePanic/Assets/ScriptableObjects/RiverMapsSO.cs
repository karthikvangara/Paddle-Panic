using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="RiverMapsSO", menuName ="RiverMapsSO")]
public class RiverMapsSO : ScriptableObject
{
    public List<GameObject> easyRiverMapPrefabs;
    public List<GameObject> mediumRiverMapPrefabs;
    public List<GameObject> hardRiverMapPrefabs;
}
