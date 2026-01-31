using UnityEngine;

[CreateAssetMenu(fileName = "NewMission", menuName = "Office/MissionData")]
public class MissionData : ScriptableObject
{
    public enum MissionType { Mail, Sorting, Correction, DataEntry }
    public MissionType type;
    public bool isGlitchy;
    [TextArea(3, 10)]
    public string customText;
    public int dayNumber;
}