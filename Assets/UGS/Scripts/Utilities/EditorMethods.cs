using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Reflection;
using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace EditorExtensions
{

    public static class EditorMethods
    {

        public static Rect RatioPositionX(this Rect position, float start, float width, ref float addWidth)
        {
            addWidth += width;
            return new Rect(position.x + position.XPosRatio(start), position.y, position.XPosRatio(start + width) - position.XPosRatio(start), position.height);
        }

        public static Rect RatioPositionXLayout(this Rect position, float space, float width, ref List<float> linesProgression, int lineIndex)
        {
            float initialAddWidth = linesProgression[lineIndex];
            linesProgression[lineIndex] += space + width;

            return new Rect(position.x + position.XPosRatio(initialAddWidth + space), position.y, position.XPosRatio(initialAddWidth + width) - position.XPosRatio(initialAddWidth), position.height);
        }

        public static Rect RatioPositionXLayout(this Rect position, float space, float width, ref float widthProgression)
        {
            float initialAddWidth = widthProgression;
            widthProgression += space + width;

            return new Rect(position.x + position.XPosRatio(initialAddWidth + space), position.y, position.XPosRatio(initialAddWidth + width) - position.XPosRatio(initialAddWidth), position.height);
        }

        public static Rect RatioPositionXLayout(this Rect position, float space, float width, float addWidth)
        {
            float initialAddWidth = addWidth;
            addWidth += space + width;
            return new Rect(position.x + position.XPosRatio(initialAddWidth + space), position.y, position.XPosRatio(initialAddWidth + width) - position.XPosRatio(initialAddWidth), position.height);
        }

        public static Rect RatioPositionX(this Rect position, float start, float width)
        {
            return new Rect(position.x + position.XPosRatio(start), position.y, position.XPosRatio(start + width) - position.XPosRatio(start), position.height);
        }

        public static Rect RatioPositionY(this Rect position, float start, float height, LayoutPosition layout = LayoutPosition.Center)
        {
            float layoutStart = position.y + position.YPosRatio(start);

            if (layout == LayoutPosition.Center) layoutStart = position.y + (position.YPosRatio(start + height) - position.YPosRatio(start)) / 2;

            return new Rect(position.x, layoutStart, position.width, position.YPosRatio(start + height) - position.YPosRatio(start));
        }

        public static Rect RatioPositionXY(this Rect position, float startX, float startY, float width, float height)
        {
            return new Rect(position.RatioPositionX(startX, width).x, position.RatioPositionY(startY, height).y, position.RatioPositionX(startX, width).width, position.RatioPositionY(startY, height).height);
        }

        public static float XPosRatio(this Rect position, float start)
        {
            return Mathf.Lerp(0, position.width, (float)(start / 100f));
        }

        public static float YPosRatio(this Rect position, float start)
        {
            return Mathf.Lerp(0, position.height, (float)(start / 100f));
        }

        public static Rect ChangeX(this Rect position, float value)
        {
            position.x = value;
            return position;
        }
        public static Rect ChangeY(this Rect position, float value)
        {
            position.y = value;
            return position;
        }

        public static Rect ChangeWidth(this Rect position, float value)
        {
            position.width = value;
            return position;
        }

        public static Rect ChangeHeight(this Rect position, float value)
        {
            position.height = value;
            return position;
        }

        public static Rect ClampX(this Rect position, float value)
        {
            return position.ChangeX(Mathf.Clamp(position.x, 0, value));
        }

        public static Rect ClampY(this Rect position, float value)
        {
            return position.ChangeY(Mathf.Clamp(position.y, 0, value));
        }
        public static Rect ClampWidth(this Rect position, float value)
        {
            return position.ChangeWidth(Mathf.Clamp(position.width, 0, value));
        }

        public static Rect ClampHeight(this Rect position, float value)
        {
            return position.ChangeHeight(Mathf.Clamp(position.height, 0, value));
        }

        public static Rect SkipLine(this Rect rect, int lineIndex, float height)
        {
            return rect.ClampHeight(height).ChangeY((rect.position.y + lineIndex * height));
        }


        public static object GetValue(SerializedProperty prop)
        {
            string path = prop.propertyPath.Replace(".Array.data[", "[");
            object obj = prop.serializedObject.targetObject;
            string[] elements = path.Split('.');

            foreach (string element in elements.Take(elements.Length))
            {
                if (element.Contains("["))
                {
                    string elementName = element.Substring(0, element.IndexOf("["));
                    int index = Convert.ToInt32(element.Substring(element.IndexOf("[")).Replace("[", "").Replace("]", ""));
                    obj = GetValue(obj, elementName, index);
                }
                else
                {
                    obj = GetValue(obj, element);
                }
            }

            return obj;
        }

        public static object GetParent(SerializedProperty prop)
        {
            var path = prop.propertyPath.Replace(".Array.data[", "[");
            object obj = prop.serializedObject.targetObject;
            var elements = path.Split('.');
            foreach (var element in elements.Take(elements.Length - 1))
            {
                if (element.Contains("["))
                {
                    var elementName = element.Substring(0, element.IndexOf("["));
                    var index = Convert.ToInt32(element.Substring(element.IndexOf("[")).Replace("[", "").Replace("]", ""));
                    obj = GetValue(obj, elementName, index);
                }
                else
                {
                    obj = GetValue(obj, element);
                }
            }
            return obj;
        }

        public static object GetValue(object source, string name)
        {
            if (source == null)
                return null;
            var type = source.GetType();
            var f = type.GetField(name, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
            if (f == null)
            {
                var p = type.GetProperty(name, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
                if (p == null)
                    return null;
                return p.GetValue(source, null);
            }
            return f.GetValue(source);
        }

        public static object GetValue(object source, string name, int index)
        {
            var enumerable = GetValue(source, name) as IEnumerable;
            var enm = enumerable.GetEnumerator();
            while (index-- >= 0)
                enm.MoveNext();
            return enm.Current;
        }

    }


}
public enum LayoutPosition {Top, Center, Bottom}
