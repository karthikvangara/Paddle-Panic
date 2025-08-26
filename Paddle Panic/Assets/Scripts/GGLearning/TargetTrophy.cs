using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class TargetTrophy
{
    public int targetScore;
    public Trophy trophy;
}

public enum Trophy { Bronze, Silver, Gold, Platinum, Diamond, Lengendary, SecretBadge}
