using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MachineData
{
    public int id;
    public string machineName;
    public string maxCurrent;
    public string maxVoltage;
    public string minCurrent;
    public string minVoltage;
}

[System.Serializable]
public class MachineDataList
{
    public List<MachineData> machines;
}
