using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CodeMonkey.Utils;

public class Window_Graph : MonoBehaviour
{
    [SerializeField] private Sprite circleSprite;
    private RectTransform graphContainer;

    List<GameObject> graphList = new List<GameObject>();
    public List<int> valueList = new List<int>();
    public int measure = 150;
    public int y = 1000;

    public void setData(int[] infectRecord)
    {
        int max = 0;

        graphList.Clear();
        valueList.Clear();
        //foreach(int i in infectRecord)
        for(int i=0; i<=infectRecord.Length; i+= measure)
        {
            valueList.Add(infectRecord[i]);
            Debug.Log("infectRecord " + i + "  " + infectRecord[i]);
            if (max < infectRecord[i])
                max = infectRecord[i];
        }
        y = max * 2;
        
    }

    private void Awake()
    {
        graphContainer = transform.Find("graphContainer").GetComponent<RectTransform>();

        //List<int> valueList = new List<int>() { 5, 170, 5, 6, 500, 5, 7, 8, 45, 17, 18, 19, 33 };
        //ShowGraph(valueList);
        //해당 임의의 값이 아닌 반환값을 넣어주면 됨.
    }

    private GameObject CreateCircle(Vector2 anchoredPosition)
    {
        GameObject gameObject = new GameObject("circle", typeof(Image));
        gameObject.transform.SetParent(graphContainer, false);
        gameObject.GetComponent<Image>().sprite = circleSprite;
        RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = anchoredPosition;
        rectTransform.sizeDelta = new Vector2(4, 4); //점의 크기
        rectTransform.anchorMin = new Vector2(0, 0);
        rectTransform.anchorMax = new Vector2(0, 0);
        return gameObject;
    }

    public void ShowGraph(List<int> valueList)
    {
        float graphHeight = graphContainer.sizeDelta.y;
        float yMaximum = y; //Y축 범위 (값이 이 범위안에 있어야 그래프가 정상적으로 출력됨)
        float xSize = 30f; //X축 범위

        GameObject lastCircleGameObject = null;
        for (int i = 0; i < valueList.Count; i++)
        {
            float xPosition = xSize + i * xSize;
            float yPosition = (valueList[i] / yMaximum) * graphHeight;
            GameObject circleGameObject = CreateCircle(new Vector2(xPosition, yPosition));
            if (lastCircleGameObject != null)
            {
                CreateDotConnection(lastCircleGameObject.GetComponent<RectTransform>().anchoredPosition, circleGameObject.GetComponent<RectTransform>().anchoredPosition);
            }
            lastCircleGameObject = circleGameObject;
        }
    }

    private void CreateDotConnection(Vector2 dotPositionA, Vector2 dotPositionB)
    {
        
        GameObject gameObject = new GameObject("dotConnection", typeof(Image));
        graphList.Add(gameObject);
        gameObject.transform.SetParent(graphContainer, false);
        gameObject.GetComponent<Image>().color = new Color(1, 1, 1, .5f);
        RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
        Vector2 dir = (dotPositionB - dotPositionA).normalized;
        float distance = Vector2.Distance(dotPositionA, dotPositionB);
        rectTransform.anchorMin = new Vector2(0, 0);
        rectTransform.anchorMax = new Vector2(0, 0);
        rectTransform.sizeDelta = new Vector2(distance, 1.5f); //점들을 이어주는 선의 너비
        rectTransform.anchoredPosition = dotPositionA + dir * distance * .5f; //점과 선의 거리(?)
        rectTransform.localEulerAngles = new Vector3(0, 0, UtilsClass.GetAngleFromVectorFloat(dir));
    }

}