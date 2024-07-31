using UnityEngine;
using System.Collections.Generic;

public class VoltageGraph : BaseGraph
{
    public Color dotColor = Color.white;
    public Color dotColorOverMax = Color.red; // Cor para pontos acima da tensão máxima
    public Color lineColor = Color.white;
    public Color lineColorOverMax = Color.red; // Cor para linhas acima da tensão máxima

    public void DisplayVoltageGraph(List<float> voltageData, float maxVoltage, string machineName)
    {
        // Limpar gráficos anteriores, se houver
        foreach (Transform child in graphContainer)
        {
            Destroy(child.gameObject);
        }

        // Exibir gráfico de tensão
        ShowGraph(voltageData, maxVoltage, 0, dotColor, $"Tensão - {machineName}");
        CreateYAxisLabels(maxVoltage, 20, 0);
    }
}
