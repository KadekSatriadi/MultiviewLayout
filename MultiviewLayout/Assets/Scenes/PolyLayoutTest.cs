using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MultiViewLayout;

public class PolyLayoutTest : MonoBehaviour
{
    public float margin = 0f;
    public float h, w;
    public GameObject v;
    public float a = 1f;
    public GameObject label;

    public TestTextCommand textCommand;
    View v0, v1, v2, v3;
    PolyLayout poly = new PolyLayout();
    List<GameObject> transforms = new List<GameObject>();

    private void Start()
    {
        poly.Height = h;
        poly.Width = w;

        v0 = new View();
        v0.Level = 0;
        v0.A = 0.2f;

        poly.Register(v0);
        poly.UpdateLayout();

        foreach(View v in poly.Views())
        {
            GameObject t = GameObject.CreatePrimitive(PrimitiveType.Cube);
            t.name = "V" + v.Level;
            t.transform.position = v.Position;
            t.transform.localScale = new Vector3(v.Width, v.Height, 1);
            t.name = "V" + v.Level + "_" + transforms.Count;
            transforms.Add(t);
            GameObject text = Instantiate(label);
            text.transform.position = t.transform.position - transform.forward;
            text.GetComponentInChildren<TextMesh>().text = t.name;
            text.transform.SetParent(t.transform);
        }

         TextCommand("add 1");
         TextCommand("add 1");
         TextCommand("add 1");
         TextCommand("add 1");
         TextCommand("add 1");
         TextCommand("add 1");
         TextCommand("add 1");
         TextCommand("add 2");
         TextCommand("add 2");
         TextCommand("add 2");
         TextCommand("add 2");
         TextCommand("add 2");
         TextCommand("add 2");
         TextCommand("add 3");
         TextCommand("add 3");
         TextCommand("add 3");
         TextCommand("add 3");
         TextCommand("add 4");
         TextCommand("add 5");
         TextCommand("add 5");
         TextCommand("add 5");
        //TextCommand("focus V1_1 0.6");
        //TextCommand("focus V2_8 0.6");
        //TextCommand("focus V0_0 0.9");
        //TextCommand("focus V0_0 0.25");
        //TextCommand("focus V1_1 0.15");
    }

    public void EnterCommand()
    {
        InputField field = FindObjectOfType<InputField>();
        TextCommand(field.text);
        field.text = "";
    }
    public void TextCommand(string txt)
    {
        string[] args = txt.Split(' ');
        switch (args[0])
        {
            case "add":
                int level = int.Parse(args[1]);
                View v = new View();
                v.Level = level;
                poly.Register(v);
                GameObject t = GameObject.CreatePrimitive(PrimitiveType.Cube);
                t.name = args[2];
                transforms.Add(t);
                GameObject text = Instantiate(label);
                text.transform.position = t.transform.position - transform.forward;
                text.GetComponentInChildren<TextMesh>().text = t.name;
                text.transform.SetParent(t.transform);
                break;
            case "remove":
                View view = GetView(args[1]);
                if (view != null)
                {
                    poly.Remove(view);
                    transforms.Remove(GameObject.Find(args[1]));
                    Destroy(GameObject.Find(args[1]));
                }
                break;
            case "focus":
                View view2 = GetView(args[1]);
                if (view2 != null)
                {
                    float a = float.Parse(args[2]);
                    poly.SetFocus(view2, a);
                }
                break;
            case "outfocus":
                View view3 = GetView(args[1]);
                if (view3 != null)
                {
                    poly.RemoveFocus(view3);
                }
                break;
            case "outfocusall":
                poly.RemoveAllFocus();
                v = null;
                break;
        }
    }


    private View GetView(string name)
    {
        GameObject g = GameObject.Find(name);
        if (g != null)
        {
            View view = poly.Views()[transforms.IndexOf(g)];
            return view;
        }
        else
        {
            return null;
        }
    }

    private void Update()
    {
        poly.Margin = margin;
        poly.Height = h;
        poly.Width = w;

        if(v != null)  TextCommand("focus " + v.name +" " + a);


        poly.UpdateLayout();
        foreach (View v in poly.Views())
        {
            GameObject t = transforms[poly.Views().IndexOf(v)];
            t.transform.position = v.Position;
            t.transform.localScale = new Vector3(v.Width, v.Height, 1);
        }
    }
}
