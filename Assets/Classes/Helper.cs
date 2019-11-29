using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Helper
{
    public static float SetTextAndExpandVertically(GameObject go, string text, float width)
    {
        TextMeshProUGUI tmpro = go.GetComponent<TextMeshProUGUI>();
        RectTransform rect = go.GetComponent<RectTransform>();
        tmpro.SetText(text);
        float lineHeight = tmpro.GetPreferredValues().y;
        float height = (tmpro.GetPreferredValues(text,width,lineHeight).y / lineHeight);
        int h = (int)Mathf.Floor(height);
        if (height - h > 0.1f) h += 1;
        rect.sizeDelta = new Vector2(rect.sizeDelta.x,h*lineHeight);
        return rect.sizeDelta.y;
    }

    public static float SetTextAndExpandHorizontally(GameObject go, string text)
    {
        TextMeshProUGUI tmpro = go.GetComponent<TextMeshProUGUI>();
        RectTransform rect = go.GetComponent<RectTransform>();
        tmpro.SetText(text);
        float lineWidth = tmpro.GetPreferredValues().x;
        rect.sizeDelta = new Vector2(lineWidth,rect.sizeDelta.y);
        return rect.sizeDelta.y;
    }

    public static string GetInitials(string text)
    {
        string[] words = text.Split(' ');
        string res = "";
        foreach (string word in words) res += word[0];
        return res;
    }

    public static List<T> readJsonArray<T>(string json)
    {
        json = json.Substring(1, json.Length-1);
        int curlCounter = 0;
        List<T> items = new List<T>();
        string current = "";
        foreach (char c in json) {
            if (c == ',' && curlCounter == 0) continue;
            current += c;
            if (c == '}') {
                curlCounter -= 1;
                if (curlCounter == 0) {
                    items.Add(JsonUtility.FromJson<T>(current));
                    current = "";
                }
            }
            else if (c == '{') curlCounter += 1;
        }
        return items;
    }

    public static string GetPossesivePronoun(string name)
    {
        return name.EndsWith("s") ? name + "'" : name + "'s";
    }
}