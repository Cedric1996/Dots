using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class SimpleConsole : MonoBehaviour
{
    static SimpleConsole ins;

    List<string> lines;
    Text text;

    public static void Create()
    {
        if (ins != null)
            return;

        var go = new GameObject("SimpleConsole");
        var canvas = go.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        go.AddComponent<SimpleConsole>();
    }

    public static void Destroy()
    {
        if (ins == null)
            return;

        Destroy(ins.gameObject);
    }

    public static void WriteLine(string line)
    {
        if (ins == null)
            return;

        ins.lines.Add(line);
        if (ins.lines.Count > 10)
            ins.lines.RemoveAt(1);
        ins.text.text = ins.lines.Aggregate((i, j) => i + "\n" + j);
    }

    public static void SetColor(Color color)
    {
        if (ins == null)
            return;

        ins.text.color = color;
    }

    // Start is called before the first frame update
    void Start()
    {
        ins = this;
        lines = new List<string>();

        var textGo = new GameObject("Text");
        textGo.transform.parent = this.transform;
        text = textGo.AddComponent<Text>();
        text.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        text.rectTransform.pivot = new Vector2(0, 1);
        text.rectTransform.anchorMin = new Vector2(0, 1);
        text.rectTransform.anchorMax = new Vector2(0, 1);
        text.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 320);
        text.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 240);
        text.rectTransform.anchoredPosition = new Vector2(0, 0);

        DontDestroyOnLoad(transform.root);

        WriteLine("F1 to toggle the console");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            text.gameObject.SetActive(!text.gameObject.activeSelf);
        }
    }

}
