using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MultiViewLayout;

public class PolyZoomTest : MonoBehaviour
{
    public float H;
    public float W;
    public float Margin;
    public GameObject label;
    public TestTextCommand textCommand;

    PolyZoom poly;
    List<GameObject> transforms = new List<GameObject>();
    ViewZoom v0, v1, v2, v3, v4;
    ViewFinder vF1, vF2, vF3, vF4;
    // Start is called before the first frame update
    void Start()
    {
        poly = new PolyZoom();
        poly.Height = H;
        poly.Width = W;
        poly.Margin = Margin;

       // ChildrenOverlapTest();

        CrossBranchOverlapTest();

    }

    void CrossBranchOverlapTest()
    {
        v0 = new ViewZoom(0);
        v1 = new ViewZoom(1);
        v2 = new ViewZoom(1);
        v3 = new ViewZoom(2);
        v4 = new ViewZoom(2);

        //is overlap return false
        //vF1 = new ViewFinder(v0, v1, 0.15f, 0.15f, new Vector3(0.5f, 0.25f,0));
        //vF2 = new ViewFinder(v0, v2, 0.15f, 0.15f, new Vector3(0.75f, 0.25f,0));
        //vF3 = new ViewFinder(v0, v3, 0.15f, 0.15f, new Vector3(1f, 0.25f,0));

        //is overlap return true
        vF1 = new ViewFinder(v0, v1, 0.15f, 0.15f, new Vector3(1f, 0.25f, 0));
        vF2 = new ViewFinder(v0, v2, 0.15f, 0.15f, new Vector3(0.75f, 0.25f, 0));
        vF3 = new ViewFinder(v1, v3, 0.15f, 0.15f, new Vector3(0.6666666f, 0.5f, 0));
        vF4 = new ViewFinder(v2, v4, 0.15f, 0.15f, new Vector3(1.55f, 0.5f, 0));

        v1.ViewFinder = vF1;
        v2.ViewFinder = vF2;
        v3.ViewFinder = vF3;
        v4.ViewFinder = vF4;

        poly.Register(v0);
        poly.Register(v1);
        poly.Register(v2);
        poly.Register(v3);
        poly.Register(v4);
        poly.UpdateLayout();


        GameObject v0g = new GameObject();
        v0g.name = "V" + v0.Level;
        v0g.transform.position = v0.Position;
        v0g.transform.localScale = new Vector3(v0.Width, v0.Height, 1);

        GameObject v1g = GameObject.CreatePrimitive(PrimitiveType.Cube);
        v1g.name = "V1" + v1.Level;
        v1g.transform.position = v1.Position;
        v1g.transform.localScale = new Vector3(v1.Width, v1.Height, 1);

        GameObject v2g = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        v2g.name = "V2" + v2.Level;
        v2g.transform.position = v2.Position;
        v2g.transform.localScale = new Vector3(v2.Width, v2.Height, 1);

        GameObject v3g = GameObject.CreatePrimitive(PrimitiveType.Cube);
        v3g.name = "V3" + v3.Level;
        v3g.transform.position = v3.Position;
        v3g.transform.localScale = new Vector3(v3.Width, v3.Height, 1);


        GameObject v4g = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        v4g.name = "V4" + v4.Level;
        v4g.transform.position = v4.Position;
        v4g.transform.localScale = new Vector3(v4.Width, v4.Height, 1);

        GameObject vf1g = GameObject.CreatePrimitive(PrimitiveType.Cube);
        vf1g.name = "VF" + vF1.Parent.Level;
        vf1g.transform.position = vF1.Position;
        vf1g.transform.localScale = new Vector3(vF1.Width, vF1.Height, 1);

        GameObject vf2g = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        vf2g.name = "VF" + vF2.Parent.Level;
        vf2g.transform.position = vF2.Position;
        vf2g.transform.localScale = new Vector3(vF2.Width, vF2.Height, 1);

        GameObject vf3g = GameObject.CreatePrimitive(PrimitiveType.Cube);
        vf3g.name = "VF" + vF3.Parent.Level;
        vf3g.transform.position = vF3.Position;
        vf3g.transform.localScale = new Vector3(vF3.Width, vF3.Height, 1);

        GameObject vf4g = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        vf4g.name = "VF" + vF4.Parent.Level;
        vf4g.transform.position = vF4.Position;
        vf4g.transform.localScale = new Vector3(vF4.Width, vF4.Height, 1);


        //There is overlap
        Debug.Log(poly.IsChildrenOverlap(v0));

        bool isSolve = true;
        //Solve overlap
        if (poly.IsChildrenOverlap(v0) && isSolve)
        {
            poly.SolveChildrenOverlap(v0);
        }
        if (poly.IsChildrenOverlap(v1) && isSolve)
        {
            poly.SolveChildrenOverlap(v1);
        }
        poly.UpdateLayout();


            v0g.transform.position = v0.Position;
            v0g.transform.localScale = new Vector3(v0.Width, v0.Height, 1);

            v1g.transform.position = v1.Position;
            v1g.transform.localScale = new Vector3(v1.Width, v1.Height, 1);

            v2g.transform.position = v2.Position;
            v2g.transform.localScale = new Vector3(v2.Width, v2.Height, 1);

            v3g.transform.position = v3.Position;
            v3g.transform.localScale = new Vector3(v3.Width, v3.Height, 1);

            vf4g.transform.position = vF4.Position;
            vf4g.transform.localScale = new Vector3(vF4.Width, vF4.Height, 1);
 

    }

    void ChildrenOverlapTest()
    {
        v0 = new ViewZoom(0);
        v1 = new ViewZoom(1);
        v2 = new ViewZoom(1);
        v3 = new ViewZoom(1);
        v4 = new ViewZoom(1);

        //is overlap return false
        //vF1 = new ViewFinder(v0, v1, 0.15f, 0.15f, new Vector3(0.5f, 0.25f,0));
        //vF2 = new ViewFinder(v0, v2, 0.15f, 0.15f, new Vector3(0.75f, 0.25f,0));
        //vF3 = new ViewFinder(v0, v3, 0.15f, 0.15f, new Vector3(1f, 0.25f,0));

        //is overlap return true
        vF1 = new ViewFinder(v0, v1, 0.15f, 0.15f, new Vector3(1f, 0.25f, 0));
        vF2 = new ViewFinder(v0, v2, 0.15f, 0.15f, new Vector3(0.75f, 0.25f, 0));
        vF3 = new ViewFinder(v0, v3, 0.15f, 0.15f, new Vector3(1.5f, 0.25f, 0));
        vF4 = new ViewFinder(v0, v4, 0.15f, 0.15f, new Vector3(1.55f, 0.25f, 0));

        v1.ViewFinder = vF1;
        v2.ViewFinder = vF2;
        v3.ViewFinder = vF3;
        v4.ViewFinder = vF4;

        poly.Register(v0);
        poly.Register(v1);
        poly.Register(v2);
        poly.Register(v3);
        poly.Register(v4);
        poly.UpdateLayout();

        GameObject v0g = new GameObject();
        v0g.name = "V" + v0.Level;
        v0g.transform.position = v0.Position;
        v0g.transform.localScale = new Vector3(v0.Width, v0.Height, 1);

        GameObject v1g = GameObject.CreatePrimitive(PrimitiveType.Cube);
        v1g.name = "V" + v1.Level;
        v1g.transform.position = v1.Position;
        v1g.transform.localScale = new Vector3(v1.Width, v1.Height, 1);

        GameObject v2g = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        v2g.name = "V" + v2.Level;
        v2g.transform.position = v2.Position;
        v2g.transform.localScale = new Vector3(v2.Width, v2.Height, 1);

        GameObject v3g = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        v3g.name = "V" + v3.Level;
        v3g.transform.position = v3.Position;
        v3g.transform.localScale = new Vector3(v3.Width, v3.Height, 1);


        GameObject v4g = GameObject.CreatePrimitive(PrimitiveType.Capsule);
        v4g.name = "V" + v4.Level;
        v4g.transform.position = v4.Position;
        v4g.transform.localScale = new Vector3(v4.Width, v4.Height, 1);

        GameObject vf1g = GameObject.CreatePrimitive(PrimitiveType.Cube);
        vf1g.name = "V" + vF1.Parent.Level;
        vf1g.transform.position = vF1.Position;
        vf1g.transform.localScale = new Vector3(vF1.Width, vF1.Height, 1);

        GameObject vf2g = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        vf2g.name = "V" + vF2.Parent.Level;
        vf2g.transform.position = vF2.Position;
        vf2g.transform.localScale = new Vector3(vF2.Width, vF2.Height, 1);

        GameObject vf3g = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        vf3g.name = "V" + vF3.Parent.Level;
        vf3g.transform.position = vF3.Position;
        vf3g.transform.localScale = new Vector3(vF3.Width, vF3.Height, 1);

        GameObject vf4g = GameObject.CreatePrimitive(PrimitiveType.Capsule);
        vf4g.name = "V" + vF4.Parent.Level;
        vf4g.transform.position = vF4.Position;
        vf4g.transform.localScale = new Vector3(vF4.Width, vF4.Height, 1);

        //There is overlap
        Debug.Log(poly.IsChildrenOverlap(v0));

        bool isSolve = false;
        //Solve overlap
        if (poly.IsChildrenOverlap(v0) && isSolve)
        {
            poly.SolveChildrenOverlap(v0);
            poly.UpdateLayout();


            v0g.transform.position = v0.Position;
            v0g.transform.localScale = new Vector3(v0.Width, v0.Height, 1);

            v1g.transform.position = v1.Position;
            v1g.transform.localScale = new Vector3(v1.Width, v1.Height, 1);

            v2g.transform.position = v2.Position;
            v2g.transform.localScale = new Vector3(v2.Width, v2.Height, 1);

            v3g.transform.position = v3.Position;
            v3g.transform.localScale = new Vector3(v3.Width, v3.Height, 1);

            vf4g.transform.position = vF4.Position;
            vf4g.transform.localScale = new Vector3(vF4.Width, vF4.Height, 1);
        }

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
    // Update is called once per frame
    void Update()
    {
        //poly.Height = H;
        //poly.Width = W;
        //poly.Margin = Margin;

        //poly.UpdateLayout();


        //foreach (View v in poly.Views)
        //{
        //    GameObject t = transforms[poly.Views.IndexOf(v)];
        //    t.transform.position = v.Position;
        //    t.transform.localScale = new Vector3(v.Width, v.Height, 1);
                 
        //}

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
}
