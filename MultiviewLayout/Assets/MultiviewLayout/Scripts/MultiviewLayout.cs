﻿using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace MultiViewLayout
{
    public class View
    {
        protected float height;
        protected float width;
        protected int level;
        protected View parent;
        protected Quaternion rotation;
        protected Vector3 position;
        protected float a;

        public float Height { get => height; set => height = value; }
        public float Width { get => width; set => width = value; }
        public int Level { get => level; set => level = value; }
        public View Parent { get => parent; set => parent = value; }
        public Quaternion Rotation { get => rotation; set => rotation = value; }
        public Vector3 Position { get => position; set => position = value; }
        public float A { get => a; set => a = value; }

        public View(int lvl)
        {
            Level = lvl;
        }

        public View() { }
    }

    public class ViewZoom : View
    {
        protected ViewFinder vf;

        public ViewFinder ViewFinder { get => vf; set => vf = value; }

        public ViewZoom(int lvl) : base(lvl)
        {
            Level = lvl;
        }

        public ViewZoom() : base() { }
    }

    public class ViewFinder
    {
        protected Vector3 position;
        protected Quaternion rotation;
        protected float height;
        protected float width;
        protected ViewZoom parent;
        protected ViewZoom child;

        public Vector3 Position { get => position; set => position = value; }
        public Quaternion Rotation { get => rotation; set => rotation = value; }
        public float Height { get => height; set => height = value; }
        public float Width { get => width; set => width = value; }
        public ViewZoom Parent { get => parent; set => parent = value; }
        public ViewZoom Child { get => child; set => child = value; }

        public ViewFinder(ViewZoom _p, ViewZoom _c)
        {
            Parent = _p;
            Child = _c;
        }

        public ViewFinder(ViewZoom _p, ViewZoom _c, float h, float w)
        {
            Parent = _p;
            Child = _c;
            Height = h;
            Width = w;
        }

        public ViewFinder(ViewZoom _p, ViewZoom _c, float h, float w, Vector3 _pos)
        {
            Parent = _p;
            Child = _c;
            Height = h;
            Width = w;
            Position = _pos;
        }
    }

    public static class InsertionSort
    {
        /// <summary>
        /// https://www.tutorialspoint.com/insertion-sort-in-chash
        /// </summary>
        /// <returns></returns>
        public static int[] Sort(int[] arr)
        {
            int n = arr.Length, i, j, val, flag;
            for (i = 1; i < n; i++)
            {
                val = arr[i];
                flag = 0;
                for (j = i - 1; j >= 0 && flag != 1;)
                {
                    if (val < arr[j])
                    {
                        arr[j + 1] = arr[j];
                        j--;
                        arr[j + 1] = val;
                    }
                    else flag = 1;
                }
            }

            return arr;
        }

        public static float[] Sort(float[] arr)
        {
            int n = arr.Length, i, j, flag;
            float val;
            for (i = 1; i < n; i++)
            {
                val = arr[i];
                flag = 0;
                for (j = i - 1; j >= 0 && flag != 1;)
                {
                    if (val < arr[j])
                    {
                        arr[j + 1] = arr[j];
                        j--;
                        arr[j + 1] = val;
                    }
                    else flag = 1;
                }
            }

            return arr;
        }
    }

    public abstract class MultiviewLayout
    {
        //contain all views in level
        protected Dictionary<int, List<View>> VsLevels = new Dictionary<int, List<View>>();
        protected float H;
        protected float W;

        public float Height { get => H; set => H = value; }
        public float Width { get => W; set => W = value; }

        public abstract void UpdateLayout();

        public List<View> Views()
        {
            List<View> views = new List<View>();
            foreach(int i in VsLevels.Keys.ToList())
            {
                views.AddRange(VsLevels[i]);
            }

            return views;
        }

        /// <summary>
        /// Remove view from views and view levels
        /// </summary>
        /// <param name="v"></param>
        public virtual void Remove(View v)
        {
            int level = v.Level;
            int childLevel = v.Level + 1;
            List<View> children = GetChildren(v);
           
            //upgrade children
            if(children != null && children.Count > 0)
            {
                foreach (View c in children)
                {
                    VsLevels[childLevel].Remove(c);
                    c.Level = level;
                    VsLevels[level].Add(c);
                }
            }
            
            //remove parent
            if (VsLevels.ContainsKey(v.Level))
            {
                VsLevels[v.Level].Remove(v);
                if (VsLevels[v.Level].Count == 0) VsLevels.Remove(v.Level);
            }
        }

        /// <summary>
        /// Register view to List and Dictionary
        /// </summary>
        /// <param name="v"></param>
        public virtual void Register(View v)
        {
            if (VsLevels.ContainsKey(v.Level))
            {
                VsLevels[v.Level].Add(v);
            }
            else
            {
                VsLevels.Add(v.Level, new List<View>() { v });
            }
        }

        /// <summary>
        /// Get all views in level
        /// </summary>
        /// <param name="level"></param>
        /// <returns></returns>
        public List<View> GetViewsInLevel(int level)
        {
            if (VsLevels.ContainsKey(level))
            {
                return VsLevels[level];

            }
            else
            {
                return new List<View>();
            }
        }

        /// <summary>
        /// Get the number of views in level
        /// </summary>
        /// <param name="level"></param>
        /// <returns></returns>
        public int GetNViewsInLevel(int level)
        {
            return GetViewsInLevel(level).Count;
        }

        /// <summary>
        /// Get the order of the view in current view level, starts from 0
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public int GetViewIndexInLevel(View v)
        {
            return GetViewsInLevel(v.Level).IndexOf(v);
        }

        /// <summary>
        /// Get count of View
        /// </summary>
        /// <returns></returns>
        public int GetN()
        {
            return Views().Count;
        }

        /// <summary>
        /// Get the number of level, (level start from 0, to this will be GetMaxL + 1), use GetMaxLevel to get maximum level
        /// </summary>
        /// <returns></returns>
        public int GetNumberOfL()
        {
            return GetMaxL() + 1;
        }

        /// <summary>
        /// Get max level
        /// </summary>
        /// <returns></returns>
        public int GetMaxL()
        {
            int max = 0;
            foreach (View v in Views())
            {
                if (v.Level > max)
                {
                    max = v.Level;
                }
            }

            return max;
        }

        /// <summary>
        /// Swap v1 and v2 position in Vs and VsLevels
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        protected virtual void SwapView(View v1, View v2)
        {
            //Update views
            int v1Idx = Views().IndexOf(v1);
            int v2Idx = Views().IndexOf(v2);
            Views()[v1Idx] = v2;
            Views()[v2Idx] = v1;

            //update level views
            v1Idx = VsLevels[v1.Level].IndexOf(v1);
            v2Idx = VsLevels[v2.Level].IndexOf(v2);
            VsLevels[v1.Level][v1Idx] = v2;
            VsLevels[v2.Level][v2Idx] = v1;
        }


        /// <summary>
        /// Get all children of v
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        protected List<View> GetChildren(View v)
        {
            List<View> children = new List<View>();
            List<View> views = GetViewsInLevel(v.Level + 1);

            foreach (View child in views)
            {
                children.Add(child);
            }

            return children;
        }
    }

}
