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

    private void CreateBezierCurve(Vector2 pointA, Vector2 pointB, Vector2 controlPointA, Vector2 controlPointB, Color color)
    {
        int segmentCount = 20; // Número de segmentos para a curva

        Vector2 previousPoint = pointA;
        for (int i = 1; i <= segmentCount; i++)
        {
            float t = i / (float)segmentCount;
            Vector2 currentPoint = CalculateBezierPoint(t, pointA, controlPointA, controlPointB, pointB);

            CreateLine(previousPoint, currentPoint, color);

            previousPoint = currentPoint;
        }
    }

    private Vector2 CalculateBezierPoint(float t, Vector2 p0, Vector2 p1, Vector2 p2, Vector2 p3)
    {
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;
        float uuu = uu * u;
        float ttt = tt * t;

        Vector2 p = uuu * p0; // primeiro ponto
        p += 3 * uu * t * p1; // primeiro ponto de controle
        p += 3 * u * tt * p2; // segundo ponto de controle
        p += ttt * p3; // último ponto

        return p;
    }

    private void ShowGraph(List<float> valueList)
    {
        float graphHeight = graphContainer.sizeDelta.y;
        float yMaximum = 50f;
        float xSize = 50f;

        GameObject lastDot = null;
        Vector2 lastDotPosition = Vector2.zero;

        for (int i = 0; i < valueList.Count; i++)
        {
            float xPosition = xSize + i * xSize;
            float yPosition = (valueList[i] / yMaximum) * graphHeight;
            Vector2 dotPosition = new Vector2(xPosition, yPosition);
            bool overMax = valueList[i] > maxVoltage;

            GameObject dot = CreateDot(dotPosition, overMax);

            if (lastDot != null)
            {
                Vector2 nextDotPosition = dotPosition;

                // Calcula os pontos de controle para a curva
                Vector2 controlPointA = lastDotPosition + new Vector2(xSize / 2, 0);
                Vector2 controlPointB = nextDotPosition - new Vector2(xSize / 2, 0);

                Color currentLineColor = valueList[i] > maxVoltage ? lineColorOverMax : lineColor;
                CreateBezierCurve(lastDotPosition, nextDotPosition, controlPointA, controlPointB, currentLineColor);
            }

            lastDot = dot;
            lastDotPosition = dotPosition;
        }
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
