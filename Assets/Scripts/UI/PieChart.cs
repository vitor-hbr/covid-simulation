using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PieChart : MonoBehaviour
{
    private Image[] imagesPieChart;
    public int[] values;
    public Color[] colors;
    public GameObject valuesObject;

    public void UpdateChart(int[] newValues, GameObject legendObject0, GameObject legendObject1, bool final, int legend)
    {
        imagesPieChart = new Image[valuesObject.transform.childCount];
        for (int i = 0; i < valuesObject.transform.childCount; i++)
        {
            GameObject pieValue = valuesObject.transform.GetChild(i).gameObject;
            pieValue.SetActive(true);
            imagesPieChart[i] = pieValue.GetComponent<Image>();
            imagesPieChart[i].color = colors[imagesPieChart.Length - 1 - i];
        }
        float totalValues = 0;
        for (int i = 0; i < imagesPieChart.Length; i++)
        {
            totalValues += FindPercentage(newValues, i);
            if(legend == 0) {
            legendObject0.transform.GetChild(i).GetComponent<TextMeshProUGUI>().text += (((100 * FindPercentage(newValues, i))).ToString("N2"));
            if (!final)
                legendObject0.transform.GetChild(i).GetComponent<TextMeshProUGUI>().text += "-";
            } else {
                legendObject1.transform.GetChild(i).GetComponent<TextMeshProUGUI>().text += (((100 * FindPercentage(newValues, i))).ToString("N2"));
            if (!final)
                legendObject1.transform.GetChild(i).GetComponent<TextMeshProUGUI>().text += "-";
            }
            imagesPieChart[imagesPieChart.Length - 1 - i].fillAmount = totalValues;
        }
    }

    private float FindPercentage(int[] newValues, int index)
    {
        float total = 0;
        for (int i = 0; i < newValues.Length; i++)
        {
            total += newValues[i];
        }

        return newValues[index] / total;
    }
}
