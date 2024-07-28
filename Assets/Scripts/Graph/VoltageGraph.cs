using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class VoltageGraph : MonoBehaviour
{
    public RectTransform graphContainer;
    public Sprite dotSprite;
    public Color dotColor = Color.white;
    public Color dotColorOverMax = Color.red; // Cor para pontos acima da tensão máxima
    public Color lineColor = Color.white;
    public Color lineColorOverMax = Color.red; // Cor para linhas acima da tensão máxima
    public RectTransform labelTemplateY;
    public float maxVoltage = 35f; // Defina a tensão máxima aqui

    private void Start()
    {
        List<float> valueList = new List<float> { 5, 10, 15, 20, 25, 30, 35, 40 };
        ShowGraph(valueList);
        CreateYAxisLabels(50, 5); // Exemplo de escala de 0 a 50 com passos de 5
    }

    private GameObject CreateDot(Vector2 anchoredPosition, bool overMax)
    {
        GameObject gameObject = new GameObject("dot", typeof(Image));
        gameObject.transform.SetParent(graphContainer, false);
        Image image = gameObject.GetComponent<Image>();
        image.sprite = dotSprite;
        image.color = overMax ? dotColorOverMax : dotColor; // Ajuste a cor do ponto conforme necessário

        RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = anchoredPosition;
        rectTransform.sizeDelta = new Vector2(11, 11); // Ajuste o tamanho do ponto conforme necessário
        rectTransform.anchorMin = new Vector2(0, 0);
        rectTransform.anchorMax = new Vector2(0, 0);

        return gameObject;
    }

    private void CreateLine(Vector2 pointA, Vector2 pointB, Color color)
    {
        GameObject lineObj = new GameObject("line", typeof(Image));
        lineObj.transform.SetParent(graphContainer, false);
        Image image = lineObj.GetComponent<Image>();
        image.color = color;

        RectTransform rectTransform = lineObj.GetComponent<RectTransform>();
        Vector2 direction = (pointB - pointA).normalized;
        float distance = Vector2.Distance(pointA, pointB);

        rectTransform.anchorMin = new Vector2(0, 0);
        rectTransform.anchorMax = new Vector2(0, 0);
        rectTransform.sizeDelta = new Vector2(distance, 3f); // Ajuste a largura da linha conforme necessário
        rectTransform.anchoredPosition = pointA + direction * distance * 0.5f;
        rectTransform.localRotation = Quaternion.Euler(0, 0, Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg);
    }

    private void ShowGraph(List<float> valueList)
    {
        float graphHeight = graphContainer.sizeDelta.y;
        float yMaximum = 50f;
        float xSize = 50f;

        GameObject lastDot = null;
        for (int i = 0; i < valueList.Count; i++)
        {
            float xPosition = xSize + i * xSize;
            float yPosition = (valueList[i] / yMaximum) * graphHeight;
            Vector2 dotPosition = new Vector2(xPosition, yPosition);
            bool overMax = valueList[i] > maxVoltage;

            GameObject dot = CreateDot(dotPosition, overMax);

            if (lastDot != null)
            {
                Vector2 lastDotPosition = lastDot.GetComponent<RectTransform>().anchoredPosition;
                Color currentLineColor = valueList[i] > maxVoltage ? lineColorOverMax : lineColor;
                CreateLine(lastDotPosition, dotPosition, currentLineColor);
            }

            lastDot = dot;
        }
    }

    private void CreateYAxisLabels(float yMax, float yInterval)
    {
        float graphHeight = graphContainer.sizeDelta.y;
        int labelCount = (int)(yMax / yInterval);

        for (int i = 0; i <= labelCount; i++)
        {
            RectTransform labelY = Instantiate(labelTemplateY);
            labelY.SetParent(graphContainer, false);
            float normalizedValue = i * yInterval / yMax;
            labelY.anchoredPosition = new Vector2(-10f, normalizedValue * graphHeight);
            labelY.GetComponent<Text>().text = (i * yInterval).ToString();
            labelY.gameObject.SetActive(true);
        }
    }
}
