using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName ="AchievementData", menuName ="AchievementData")]
public class AchievementData : ScriptableObject
{
    public List<Achievement> achievement=new List<Achievement>();
}
[Serializable]
public class Achievement
{
    public AchievementAction achievementAction;
    public List<AchievementInfo> achievementInfo = new List<AchievementInfo>();
}

public enum AchievementAction { 
    Batting ,
    Bowling, 
    Fielding, 
    Progression_Milestones , 
    Skill_Based , 
    Exploration_Discovery,
    Multiplayer_Social,
    SpecialEvents_Seasonal,
    Prestige_Mastery,
    Fun_QuirkyBadges
}
