using UnityEngine;
using System.Collections;

[System.Serializable]
public class MissionEvent
{
    public string eventID;
    public string eventType;
    public float conditionTime;
    public string conditionLocation;
    public float x;
    public float y;
    public float z;
    public float xRotation;
    public float yRotation;
    public float zRotation;
    public string data1;
    public string data2;
    public string data3;
    public string data4;
    public string data5;
    public string data6;
    public string data7;
    public string data8;
    public string data9;
    public string data10;
    public string data11;
    public string data12;
    public string data13;
    public string data14;
    public string data15;
    public string nextEvent1;
    public string nextEvent2;
    public string nextEvent3;
    public string nextEvent4;
    public float nodePosX;
    public float nodePosY;
}

[System.Serializable]
public class Mission
{
    public MissionEvent[] missionEventData;
}
