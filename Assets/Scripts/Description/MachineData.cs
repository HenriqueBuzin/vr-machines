using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MachineData
{
    public int id;
    public string machineName;
    public float maxCurrent;
    public float maxVoltage;
    public float minCurrent;
    public float minVoltage;
    public List<float> currentData;
    public List<float> voltageData;
}

[System.Serializable]
public class MachineDataList
{
    public List<MachineData> machines;
}
