using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PolyLayout : MultiviewLayout
{
    protected Dictionary<View, float> focusedViewA = new Dictionary<View, float>();
    protected float margin;
    protected bool constraintWidth = false;

    public float Margin { get => margin; set => margin = value; }


    public void UpdateLayout()
    {
        UpdateA();
        UpdateGeometry();
    }

    /// <summary>
    /// Get total a of all focused views
    /// </summary>
    /// <returns></returns>
    public float GetTotalFocusedA()
    {
        float total = 0;
        foreach(View v in GetFocusedViews())
        {
            total += focusedViewA[v];
        }

        return total;
    }

    /// <summary>
    /// Get total a in level
    /// </summary>
    /// <param name="level"></param>
    /// <returns></returns>
    public float GetTotalAInLevel(int level)
    {
        float total = 0;
        foreach(View v in GetViewsInLevel(level))
        {
            total += v.A;
        }
        return total;
    }


    /// <summary>
    /// Remove all focus views
    /// </summary>
    public void RemoveAllFocus()
    {
        foreach(View v in GetFocusedViews())
        {
            RemoveFocus(v);
        }
    }

    /// <summary>
    /// Set focused view
    /// </summary>
    /// <param name="v"></param>
    /// <param name="a"></param>
    public void SetFocus(View v, float a)
    {
        if (focusedViewA.ContainsKey(v))
        {
            focusedViewA[v] = a;
        }
        else
        {
            focusedViewA.Add(v, a);
        }
        v.A = a;

        float remain = 1 - a;
        
        //Update other a in Focused view if total A > 1
        if (IsFocusViewAMoreThan1() && constraintWidth)
        {
            foreach (View view in GetFocusedViews())
            {
                if (view != v)
                {
                    RemoveFocus(view);
                }
            }
        }      
    }

    /// <summary>
    /// Is focuse View total A > 1 before UpdateA
    /// </summary>
    /// <returns></returns>
    public bool IsFocusViewAMoreThan1()
    {
        float a = 0;
        foreach(View v in GetFocusedViews())
        {
            a += focusedViewA[v];
        }

        return (a > 1f);
    }

    /// <summary>
    /// Get all focused views
    /// </summary>
    /// <returns></returns>
    public List<View> GetFocusedViews()
    {
        return focusedViewA.Keys.ToList(); 
    }

    /// <summary>
    /// If view focused
    /// </summary>
    /// <param name="v"></param>
    /// <returns></returns>
    public bool IsViewFocused(View v)
    {
        return focusedViewA.ContainsKey(v);
    }

    /// <summary>
    /// Remove focus
    /// </summary>
    /// <param name="v"></param>
    /// <param name="a"></param>
    public void RemoveFocus(View v)
    {
        if (focusedViewA.ContainsKey(v)) focusedViewA.Remove(v);
    }

    /// <summary>
    /// Update scale
    /// </summary>
    protected void UpdateA()
    {
        float total = 1f;
        //adjust a for focused view
        for (int i = 0; i < GetNumberOfL(); i++)
        {
            float max = 0;
            foreach (View v in GetViewsInLevel(i))
            {
                if (focusedViewA.ContainsKey(v))
                {
                    v.A = focusedViewA[v];
                    max = (max < v.A)? v.A : max;
                }
            }
            total -= max;
        }
        total = (total < 0) ? 0 : total;

        //adjust a for non focused view
        for (int i = 0; i < GetNumberOfL(); i++)
        {
            foreach (View v in GetViewsInLevel(i))
            {
                if (!focusedViewA.ContainsKey(v))
                {
                    int n = GetNumberOfL();
                    float a = total / GetNumberOfL();
                    v.A = a;
                }
            }
        }
    }

    /// <summary>
    /// Update position and dimension
    /// </summary>
    protected void UpdateGeometry()
    {
        for (int i = 0; i < GetNumberOfL(); i++)
        {
            foreach (View v in GetViewsInLevel(i))
            {
                float y = GetYPosition(v);
                float x = GetXPosition(v);
                v.Position = new Vector3(x, y, 0);
                v.Height = GetViewHeight(v) - margin;
                v.Width = GetViewWidth(v) - margin;
                //Debug.Log("Postiion v " + v.Level + " = " + v.Position);
            }
        }
    }

    /// <summary>
    /// Get the highest A in level 
    /// </summary>
    /// <param name="level"></param>
    /// <returns></returns>
    protected float GetMaxAInLevel(int level)
    {
        float max = 0; 
        foreach(View v in GetViewsInLevel(level))
        {
            if (IsViewFocused(v))
            {
                max = (max < focusedViewA[v]) ? focusedViewA[v] : max;
            }
            else
            {
                max = (max < v.A)? v.A : max;
            }
        }
        return max;
    }


    /// <summary>
    /// Return the minimun value of the non focused view, technically all a should be the same
    /// </summary>
    /// <returns></returns>
    protected float GetMinAOfNonFocusedViews()
    {
        float min = 0;
        foreach(View v in Views)
        {
            if (!IsViewFocused(v))
            {
                min = (min > v.A) ? v.A : min;
            }
        }

        return min;
    }   

    /// <summary>
    /// Get total A before level 
    /// </summary>
    /// <returns></returns>
    protected float GetTotalABeforeLevel(int level)
    {
        float total = 0;
        for (int i = 0; i < level; i++)
        {
            total += GetMaxAInLevel(i);
        }
        return total;
    }

    /// <summary>
    /// Get total A from left to right 
    /// </summary>
    /// <param name="v"></param>
    /// <returns></returns>
    protected float GetTotalABeforeViewInSameLevel(View v)
    {
        float total = 0;
        foreach(View cv in GetViewsInLevel(v.Level))
        {
            if(cv != v)
            {
                total += cv.A;
            }
            else
            {
                break;
            }
        }

        return total;
    }

    /// <summary>
    /// Check whether level has focused  view
    /// </summary>
    /// <param name="level"></param>
    /// <returns></returns>
    protected bool IsLevelHasFocus(int level)
    {
        bool r = false;
        foreach(View v in GetViewsInLevel(level))
        {
            if (IsViewFocused(v))
            {
                r = true; break;
            }
        }
        return r;
    }

    /// <summary>
    /// The y position of Vi is calculated based on the number of level in Vi.level and A
    /// </summary>
    /// <returns></returns>
    protected float GetYPosition(View v)
    {
        float ay = GetTotalABeforeLevel(v.Level);
        float h = H * ay;

        if (IsLevelHasFocus(v.Level))
        {
            return (H * GetMaxAInLevel(v.Level) * 0.5f) + h;
        }
        else
        {
            return (H * v.A * 0.5f) + h;
        }            
    }

    //The x position of Vi is calculated based on the number of views in Vi.level and w 
    protected float GetXPosition(View v)
    {
        //get width
        float w = GetViewWidth(v);
        //first, get the center point
        float center = W / 2;
        //get left point, by substracting center with half of total w of all views in current level
        float totalWLevel = GetTotalAInLevel(v.Level) * W;
        float left = center - (totalWLevel / 2);
        float before = GetTotalABeforeViewInSameLevel(v) * W;
        float x = left + before + (W * v.A * 0.5f);

        return x;
    }


    /// <summary>
    /// Get height of Vi based on ai
    /// </summary>
    /// <returns></returns>
    protected float GetViewHeight(View v)
    {
        return H * v.A;
    }

    /// <summary>
    /// Get width of Vi based on bi
    /// </summary>
    /// <returns></returns>
    protected float GetViewWidth(View v)
    {
        return W * v.A;
    }
}
