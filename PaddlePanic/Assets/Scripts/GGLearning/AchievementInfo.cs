using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class AchievementInfo
{
    public string achievementName;
    public AchievementTypes achievementTypes;
    public int achievementId;
    public int playerCurrScore;
    public List<TargetTrophy> targetTrophy=new List<TargetTrophy>();
}

public enum AchievementTypes { Runs, Fours, Sixes, HalfCentury, Century, LastBallFinish, WallOfTheGully}
