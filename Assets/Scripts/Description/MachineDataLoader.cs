using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MachineDataLoader : MonoBehaviour
{
    public Text machineInfoText;
    public int selectedMachineId;
    private List<MachineData> machineDataList;

    void Start()
    {
        LoadMachineData();
        DisplayMachineInfo(selectedMachineId);
    }

    void LoadMachineData()
    {
        // Carregar o arquivo JSON da pasta Resources
        TextAsset jsonTextFile = Resources.Load<TextAsset>("machines");
        if (jsonTextFile != null)
        {
            MachineDataList dataList = JsonUtility.FromJson<MachineDataList>(jsonTextFile.text);
            machineDataList = dataList.machines;
        }
        else
        {
            Debug.LogError("Arquivo JSON não encontrado!");
        }
    }

    void DisplayMachineInfo(int machineId)
    {
        MachineData selectedMachine = machineDataList?.Find(machine => machine.id == machineId);

        if (selectedMachine != null)
        {
            machineInfoText.text = $"Máquina: {selectedMachine.machineName}\n" +
                                   $"Corrente Máxima: {selectedMachine.maxCurrent} A\n" +
                                   $"Tensão Máxima: {selectedMachine.maxVoltage} V\n" +
                                   $"Corrente Mínima: {selectedMachine.minCurrent} A\n" +
                                   $"Tensão Mínima: {selectedMachine.minVoltage} V";

            // Exibir gráficos de corrente e tensão
            var voltageGraph = FindObjectOfType<VoltageGraph>();
            var currentGraph = FindObjectOfType<CurrentGraph>();

            if (voltageGraph != null)
            {
                voltageGraph.DisplayVoltageGraph(selectedMachine.voltageData, selectedMachine.maxVoltage, selectedMachine.machineName);
            }

            if (currentGraph != null)
            {
                currentGraph.DisplayCurrentGraph(selectedMachine.currentData, selectedMachine.maxCurrent, selectedMachine.machineName);
            }
        }
        else
        {
            machineInfoText.text = "Máquina não encontrada!";
        }
    }
}
