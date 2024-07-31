using UnityEngine;
using System.Collections.Generic;

public class CurrentGraph : BaseGraph
{
    public Color dotColor = Color.green;
    public Color dotColorOverMax = Color.red; // Cor para pontos acima da corrente máxima
    public Color lineColor = Color.green;
    public Color lineColorOverMax = Color.red; // Cor para linhas acima da corrente máxima

    public void DisplayCurrentGraph(List<float> currentData, float maxCurrent, string machineName)
    {
        // Limpar gráficos anteriores, se houver
        foreach (Transform child in graphContainer)
        {
            Destroy(child.gameObject);
        }

        // Exibir gráfico de corrente
        ShowGraph(currentData, maxCurrent, 0, dotColor, $"Corrente - {machineName}");
        CreateYAxisLabels(maxCurrent, 10, 0);
    }
}
