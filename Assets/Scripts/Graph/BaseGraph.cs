using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class BaseGraph : MonoBehaviour
{
    public RectTransform graphContainer;
    public Sprite dotSprite;
    public RectTransform labelTemplateY;

    protected GameObject CreateDot(Vector2 anchoredPosition, bool overMax, Color color)
    {
        GameObject gameObject = new GameObject("dot", typeof(Image));
        gameObject.transform.SetParent(graphContainer, false);
        Image image = gameObject.GetComponent<Image>();
        image.sprite = dotSprite;
        image.color = color;

        RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = anchoredPosition;
        rectTransform.sizeDelta = new Vector2(11, 11); // Ajuste o tamanho do ponto conforme necessário
        rectTransform.anchorMin = new Vector2(0, 0);
        rectTransform.anchorMax = new Vector2(0, 0);

        return gameObject;
    }

    protected void CreateBezierCurve(Vector2 pointA, Vector2 pointB, Vector2 controlPointA, Vector2 controlPointB, Color color)
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

    protected Vector2 CalculateBezierPoint(float t, Vector2 p0, Vector2 p1, Vector2 p2, Vector2 p3)
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

    protected void ShowGraph(List<float> valueList, float yMax, float xOffset, Color color, string title)
    {
        float graphHeight = graphContainer.sizeDelta.y;
        float xSize = 50f;

        GameObject lastDot = null;
        Vector2 lastDotPosition = Vector2.zero;

        // Adiciona o título do gráfico
        CreateGraphTitle(title, xOffset);

        for (int i = 0; i < valueList.Count; i++)
        {
            float xPosition = xOffset + xSize + i * xSize;
            float yPosition = (valueList[i] / yMax) * graphHeight; // Ajuste de altura proporcional
            Vector2 dotPosition = new Vector2(xPosition, yPosition);
            bool overMax = valueList[i] > yMax;

            GameObject dot = CreateDot(dotPosition, overMax, color);

            if (lastDot != null)
            {
                Vector2 nextDotPosition = dotPosition;

                // Calcula os pontos de controle para a curva
                Vector2 controlPointA = lastDotPosition + new Vector2(xSize / 2, 0);
                Vector2 controlPointB = nextDotPosition - new Vector2(xSize / 2, 0);

                Color currentLineColor = valueList[i] > yMax ? color : color;
                CreateBezierCurve(lastDotPosition, nextDotPosition, controlPointA, controlPointB, currentLineColor);
            }

            lastDot = dot;
            lastDotPosition = dotPosition;
        }
    }

    protected void CreateLine(Vector2 pointA, Vector2 pointB, Color color)
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

    protected void CreateYAxisLabels(float yMax, float yInterval, float xOffset)
    {
        float graphHeight = graphContainer.sizeDelta.y;
        int labelCount = (int)(yMax / yInterval);

        for (int i = 0; i <= labelCount; i++)
        {
            RectTransform labelY = Instantiate(labelTemplateY);
            labelY.SetParent(graphContainer, false);
            float normalizedValue = i * yInterval / yMax; // Normalização para a altura do gráfico
            labelY.anchoredPosition = new Vector2(-10f + xOffset, normalizedValue * graphHeight); // Posição alinhada
            labelY.GetComponent<Text>().text = (i * yInterval).ToString();
            labelY.gameObject.SetActive(true);
        }
    }

    protected void CreateGraphTitle(string title, float xOffset)
    {
        GameObject titleObj = new GameObject("graphTitle", typeof(Text));
        titleObj.transform.SetParent(graphContainer, false);
        Text titleText = titleObj.GetComponent<Text>();
        titleText.text = title;
        titleText.alignment = TextAnchor.MiddleCenter;
        titleText.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        titleText.color = Color.black;
        titleText.fontSize = 14;

        RectTransform rectTransform = titleObj.GetComponent<RectTransform>();
        rectTransform.anchorMin = new Vector2(0, 1);
        rectTransform.anchorMax = new Vector2(0, 1);
        rectTransform.anchoredPosition = new Vector2(xOffset + graphContainer.sizeDelta.x / 2, -20f); // Ajustar conforme necessário
        rectTransform.sizeDelta = new Vector2(200, 30);
    }
}
