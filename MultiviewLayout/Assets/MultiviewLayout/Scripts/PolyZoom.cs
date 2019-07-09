using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public class PolyZoom : PolyLayout
{
    protected Dictionary<ViewZoom, List<ViewFinder>> parentViewfinders = new Dictionary<ViewZoom, List<ViewFinder>>();

    public void Register(ViewZoom v)
    {
        base.Register(v);

        if(v.ViewFinder != null)
        {
            ViewFinder vf = v.ViewFinder;
            if (parentViewfinders.ContainsKey(vf.Parent))
            {
                parentViewfinders[vf.Parent].Add(vf);
            }
            else
            {
                parentViewfinders.Add(vf.Parent, new List<ViewFinder>() { vf });
            }
        }
        
    }

    /// <summary>
    /// Get all viewfinders attached to v
    /// </summary>
    /// <param name="v"></param>
    /// <returns></returns>
    public List<ViewFinder> GetViewFinders(ViewZoom v)
    {
        return parentViewfinders[v];
    }


    /// <summary>
    /// Check whether or not there is overlap in children of v
    /// </summary>
    /// <param name="v"></param>
    /// <returns></returns>
    public bool IsChildrenOverlap(ViewZoom v)
    {
        //if there is no children,
        if (!parentViewfinders.ContainsKey(v)) return false;

        List<ViewFinder> viewFinders = parentViewfinders[v];
        //if there is one child
        if (viewFinders.Count == 1) return false;

        bool ret = false;
        //sort viewfinders
        List<ViewFinder> sortedViewfinder = GetSortedLeftRightViewfinders(v);

        //get children 
        List<View> children = GetChildren(v);

        ///Loop viewfinder
        /// Viewfinder: 0 1 2 3
        /// Child     : 1 0 2 3
        /// There is overlap if the index of the viewfinder is not the same as index of child
        foreach(ViewFinder vf in sortedViewfinder)
        {
            int idx = sortedViewfinder.IndexOf(vf);
            int childIdx = children.IndexOf(vf.Child);
            if(idx != childIdx)
            {
                ret = true;
                break;
            }
        }

        return ret;
    }


    public void SolveChildrenOverlap(ViewZoom v)
    {
        //sort viewfinders
        List<ViewFinder> sortedViewfinder = GetSortedLeftRightViewfinders(v);

       

        ///Loop viewfinder
        /// Viewfinder: 0 1 2 
        /// Child     : 2 1 0
        /// There is overlap if the index of the viewfinder is not the same as index of child
        /// Swap
        /// Viewfinder: 0 1 2 
        /// Child     : 0 1 2
        /// Overlap, swap
        /// Viewfinder: 0 1 2 3
        /// Child     : 0 1 2 3
        foreach (ViewFinder vf in sortedViewfinder)
        {
            //get children 
            List<View> children = GetChildren(v);

            int idx = sortedViewfinder.IndexOf(vf);
            int childIdx = children.IndexOf(vf.Child);
            if (idx != childIdx)
            {
                
                //get swap anchor
                View swapChild = children[idx];
                //conflict child
                View child = vf.Child;
                //swap children
                SwapView(swapChild, child);

                Debug.Log("Overlap " + idx + " and " + childIdx);
            }
        }
    }

    // UNUSED
    ///// <summary>
    ///// Get children of v sorted from left to right
    ///// </summary>
    ///// <param name="v"></param>
    ///// <returns></returns>
    //protected List<View> GetSortedLeftRightChildren(View v)
    //{  
    //    //get all children
    //    List<View> children = GetChildren(v);
    //    //calculate and map children position form left to right
    //    Dictionary<float, ViewZoom> childrenDistance = new Dictionary<float, ViewZoom>();
    //    foreach (ViewZoom child in children)
    //    {
    //        //Very far left point from the parent
    //        Vector3 leftPoint = v.Position - new Vector3(100f, v.Position.y, v.Position.z);
    //        //Vector from left to parent center
    //        Vector3 leftToParentVector = v.Position - leftPoint;
    //        //Vector from left to vf
    //        Vector3 leftToVfVector = child.Position - leftPoint;
    //        //Project
    //        Vector3 projection = Vector3.Project(leftToVfVector, leftToParentVector);
    //        //add distance
    //        childrenDistance.Add(projection.magnitude, child);
    //    }
    //    //then short the distance
    //    float[] distances = childrenDistance.Keys.ToArray();
    //    distances = InsertionSort.Sort(distances.ToArray());
    //    //List of sorted children
    //    List<View> sortedChildren = new List<View>();
    //    for (int i = 0; i < distances.Length; i++)
    //    {
    //        sortedChildren.Add(childrenDistance[distances[i]]);
    //    }

    //    return sortedChildren;
    //}

    /// <summary>
    /// Get list of sorted viewfinders on v from left to right
    /// </summary>
    /// <param name="v"></param>
    /// <returns></returns>
    protected List<ViewFinder> GetSortedLeftRightViewfinders(ViewZoom v)
    {
        //get all viewfinders
        List<ViewFinder> children = parentViewfinders[v];
        //first, calculate and map viewfinders position from left to right
        Dictionary<float, ViewFinder> viewFinderDistance = new Dictionary<float, ViewFinder>();
        foreach (ViewFinder vf in children)
        {
            //Very far left point from the parent
            Vector3 leftPoint = v.Position - new Vector3(100f, v.Position.y, v.Position.z);
            //Vector from left to parent center
            Vector3 leftToParentVector = v.Position - leftPoint;
            //Vector from left to vf
            Vector3 leftToVfVector = vf.Position - leftPoint;
            //Project
            Vector3 projection = Vector3.Project(leftToVfVector, leftToParentVector);
            //add distance
            viewFinderDistance.Add(projection.magnitude, vf);
        }
        //then short the distance
        float[] distances = viewFinderDistance.Keys.ToArray();
        distances = InsertionSort.Sort(distances.ToArray());
        //List of sorted viewfinders
        List<ViewFinder> sortedViewfinders = new List<ViewFinder>();
        for(int i = 0; i < distances.Length; i++)
        {
            sortedViewfinders.Add(viewFinderDistance[distances[i]]);
        }

        return sortedViewfinders;
    }


   



}
