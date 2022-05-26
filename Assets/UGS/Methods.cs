using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using System;
using Debug = UnityEngine.Debug;
using System.Diagnostics;
public static class Methods
{

    public static Vector3 ZeroZMousePos()
    {
        return Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0));
    }

    public static Vector3 ZDistanceMousePos(float distanceFromCamera)
    {
        return Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -Camera.main.transform.position.z + distanceFromCamera));
    }

    public static Vector3 ChangeX(this Vector3 vec, float xValue)
    {
        return new Vector3(xValue, vec.y, vec.z);
    }
    public static Vector3 ChangeY(this Vector3 vec, float yValue)
    {
        return new Vector3(vec.x, yValue, vec.z);
    }
    public static Vector3 ChangeZ(this Vector3 vec, float zValue)
    {
        return new Vector3(vec.x, vec.y, zValue);
    }

    public static Vector3 OffsetX(this Vector3 vec, float xValue)
    {
        return new Vector3(vec.x + xValue, vec.y, vec.z);
    }
    public static Vector3 OffsetY(this Vector3 vec, float yValue)
    {
        return new Vector3(vec.x, vec.y + yValue, vec.z);
    }
    public static Vector3 OffsetZ(this Vector3 vec, float zValue)
    {
        return new Vector3(vec.x, vec.y, vec.z + zValue);
    }

    public static Vector3 ClampX(this Vector3 vec, float min, float max)
    {
        return new Vector3(Mathf.Clamp(vec.x, min, max), vec.y, vec.z);
    }
    public static Vector3 ClampY(this Vector3 vec, float min, float max)
    {
        return new Vector3(vec.x, Mathf.Clamp(vec.y, min, max), vec.z);
    }
    public static Vector3 ClampZ(this Vector3 vec, float min, float max)
    {
        return new Vector3(vec.x, vec.y, Mathf.Clamp(vec.z, min, max));
    }

    public static Vector3 MultiplyVector(this Vector3 vec1, Vector3 vec2)
    {
        return new Vector3(vec1.x * vec2.x, vec1.y * vec2.y, vec1.z * vec2.z);
    }
    public static Vector3 MultiplyVector(Vector3 vec1, Vector3 vec2, Vector3 vec3)
    {
        return new Vector3(vec1.x * vec2.x * vec3.x, vec1.y * vec2.y * vec3.y, vec1.z * vec2.z * vec3.z);
    }
    public static Vector3 MultiplyVector(Vector3 vec1, Vector3 vec2, float value)
    {
        return new Vector3(vec1.x * vec2.x * value, vec1.y * vec2.y * value, vec1.z * vec2.z * value);
    }

    public static Vector3 NoisyVector(this Vector3 vec, float range)
    {
        return new Vector3(vec.x + Random.Range(-range, range), vec.y + Random.Range(-range, range), vec.z + Random.Range(-range, range));
    }

    public static Vector3 Invert(this Vector3 vec, bool xValue = false, bool yValue = false, bool zValue = false)
    {
        return new Vector3(vec.x * (xValue ? -1 : 1), vec.y * (yValue ? -1 : 1), vec.z * (zValue ? -1 : 1));
    }

    public static Vector3 SwapXY(this Vector3 vec)
    {
        return new Vector3(vec.y, vec.x, vec.z);
    }
    public static Vector3 SwapYZ(this Vector3 vec)
    {
        return new Vector3(vec.x, vec.z, vec.y);
    }
    public static Vector3 SwapZX(this Vector3 vec)
    {
        return new Vector3(vec.z, vec.y, vec.x);
    }

    public static void SetMaterialOpacity(GameObject go, float value)
    {
        MaterialPropertyBlock matProp = new MaterialPropertyBlock();
        matProp.SetFloat("_Treshold", value); //A save et Load en opaque ou carrément changer le shader
        go.GetComponent<MeshRenderer>().SetPropertyBlock(matProp);
    }

    public static void SetMaterialColor(GameObject go, Color value)
    {
        MaterialPropertyBlock matProp = new MaterialPropertyBlock();
        matProp.SetColor("_Color", value); //NON HDRP
        //matProp.SetColor("_BaseColor", value); //HDRP
        go?.GetComponent<MeshRenderer>()?.SetPropertyBlock(matProp);
    }

    public static float GetMaterialValue(GameObject go, string value)
    {
        return go.GetComponent<MeshRenderer>().material.GetFloat(value);
    }

    public static float TriangleFonction(float time, float period = 2, float amplitude = 1, float offset = 0)
    {
        return Mathf.Abs((((time / (period / 2)) + (offset + 1)) % (amplitude * 2)) - amplitude);
    }

    public static float NegativeInclusionTriangleFonction(float time, float period = 2, float amplitude = 1, float offset = 0)
    {
        return (((time / (period / 2)) + (offset + 1)) % (amplitude * 2)) - amplitude;
    }

    public static Vector2 xz(this Vector3 vv)
    {
        return new Vector2(vv.x, vv.z);
    }

    public static Vector2 xy(this Vector3 vv)
    {
        return new Vector2(vv.x, vv.y);
    }

    public static Vector2 yz(this Vector3 vv)
    {
        return new Vector2(vv.y, vv.z);
    }

    public static float FlatDistanceTo(this Vector3 from, Vector3 to)
    {
        Vector2 a = from.xz();
        Vector2 b = to.xz();

        return Vector2.Distance(a, b);
    }

    public static Vector3 Round(this Vector3 vector3, int decimalPlaces = 2)
    {
        float multiplier = 1;
        for (int i = 0; i < decimalPlaces; i++)
        {
            multiplier *= 10f;
        }
        return new Vector3(
            Mathf.Round(vector3.x * multiplier) / multiplier,
            Mathf.Round(vector3.y * multiplier) / multiplier,
            Mathf.Round(vector3.z * multiplier) / multiplier);
    }

    public static bool InRadiusXY(this Vector2 vec, Vector2 from, float radius)
    {
        if (vec.x > from.x + radius || vec.x < from.x - radius || vec.y > from.y + radius || vec.y < from.y - radius) return false;
        else return true;
    }

    public static bool InBoundsXY(this Vector2 vec, Vector2 from, float boundX, float boundY)
    {
        if (vec.x > from.x + boundX || vec.x < from.x - boundX || vec.y > from.y + boundY || vec.y < from.y - boundY) return false;
        else return true;
    }

    public static void Add<T>(this List<T> list, T item)
    {
        if (item != null) list.Add(item);
    }

    public static Vector3 ClampRotationX(this Vector3 rot, float min, float max)
    {
        float value = rot.x;

        if (value >= 180) value -= 360;

        return new Vector3(Mathf.Clamp(value, min, max), rot.y, rot.z);
    }

    public static Vector3 ClampRotationY(this Vector3 rot, float min, float max)
    {
        float value = rot.y;

        if (value >= 180) value -= 360;

        return new Vector3(rot.x, Mathf.Clamp(value, min, max), rot.z);
    }

    public static Vector3 ClampRotationZ(this Vector3 rot, float min, float max)
    {
        float value = rot.z;

        if (value >= 180) value -= 360;

        return new Vector3(rot.x, rot.y, Mathf.Clamp(value, min, max));
    }

    public static Vector3 QuadraticInterpolation(Vector3 a, Vector3 b, Vector3 c, float t)
    {
        Vector3 ab = Vector3.Lerp(a, b, t);
        Vector3 bc = Vector3.Lerp(b, c, t);

        return Vector3.Lerp(ab, bc, t);
    }
    public static Vector3 CubicInterpolation(Vector3 a, Vector3 b, Vector3 c, Vector3 d, float t)
    {
        Vector3 ab_bc = QuadraticInterpolation(a, b, c, t);
        Vector3 bc_cd = QuadraticInterpolation(b, c, d, t);

        return Vector3.Lerp(ab_bc, bc_cd, t);
    }

    public static Vector3Int ToIntVector(this Vector3 vec)
    {
        return new Vector3Int((int)vec.x, (int)vec.y, (int)vec.z);
    }

    public static string FormatS(this float value, string format, int valueLength = 0, char comaReplacement = ',')
    {
        int length = valueLength - Mathf.Floor(value).ToString("F0").Length;
        length = Mathf.Max(0, length);

        return new string('0', length) + value.ToString(format).Replace(',', comaReplacement);
    }

    public static string TimeFormat(this float value, char separator)
    {
        string result = "";
        float hours = Mathf.Floor(value / 3600f);
        float minutes = Mathf.Floor((value - (hours * 3600)) / 60f);
        float seconds = Mathf.RoundToInt((value % 60) - 0.5f);

        if (hours < 10 && hours > 0)
        {
            result += "0" + hours.ToString() + separator;
        }
        else if (hours > 0 || hours >= 10) result += hours.ToString() + separator;

        if (minutes < 10)
        {
            result += "0" + minutes.ToString() + separator;
        }
        else result += minutes.ToString() + separator;

        if (seconds < 10)
        {
            result += "0" + Mathf.Floor(seconds).ToString();
        }
        else result += Mathf.Floor(seconds);

        return result;
    }

    public static float Average(float v1, float v2)
    {
        return (v1 + v2) / 2f;
    }

    public static bool IsBetween(this float value, float min, float max, bool minInclusive = false, bool maxInclusive = false)
    {
        return (minInclusive ? value >= min : value > min) && (maxInclusive ? value <= max : value < max);
    }

    public static bool IsOutof(this float value, float min, float max, bool minInclusive = false, bool maxInclusive = false)
    {
        return !IsBetween(value, min, max, minInclusive, maxInclusive);
    }


    public static Stopwatch watch = new Stopwatch();
    public static void BeginProfiling()
    {
        watch.Reset();
        watch.Start();
    }

    public static float EndProfiling(bool log = true, bool pause = false)
    {
        watch.Stop();

        if (log) Debug.Log(watch.Elapsed);
        if (pause) Debug.Break();

        return (float)watch.Elapsed.TotalMilliseconds;
    }

    public static Quaternion Lerp(Quaternion p, Quaternion q, float t, bool shortWay)
    {
        if (shortWay)
        {
            float dot = Quaternion.Dot(p, q);
            if (dot < 0.0f)
                return Lerp(ScalarMultiply(p, -1.0f), q, t, true);
        }

        Quaternion r = Quaternion.identity;
        r.x = p.x * (1f - t) + q.x * (t);
        r.y = p.y * (1f - t) + q.y * (t);
        r.z = p.z * (1f - t) + q.z * (t);
        r.w = p.w * (1f - t) + q.w * (t);
        return r;
    }

    public static Quaternion Slerp(Quaternion p, Quaternion q, float t, bool shortWay)
    {
        float dot = Quaternion.Dot(p, q);
        if (shortWay)
        {
            if (dot < 0.0f)
                return Slerp(ScalarMultiply(p, -1.0f), q, t, true);
        }

        float angle = Mathf.Acos(dot);
        Quaternion first = ScalarMultiply(p, Mathf.Sin((1f - t) * angle));
        Quaternion second = ScalarMultiply(q, Mathf.Sin((t) * angle));
        float division = 1f / Mathf.Sin(angle);
        return ScalarMultiply(Add(first, second), division);
    }


    public static Quaternion ScalarMultiply(Quaternion input, float scalar)
    {
        return new Quaternion(input.x * scalar, input.y * scalar, input.z * scalar, input.w * scalar);
    }

    public static Quaternion Add(Quaternion p, Quaternion q)
    {
        return new Quaternion(p.x + q.x, p.y + q.y, p.z + q.z, p.w + q.w);
    }

    public static Vector3 BezierTangent(float t, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
    {
        float u = 1 - t;
        float uu = u * u;
        float tu = t * u;
        float tt = t * t;

        Vector3 P = p0 * 3 * uu * (-1.0f);
        P += p1 * 3 * (uu - 2 * tu);
        P += p2 * 3 * (2 * tu - tt);
        P += p3 * 3 * tt;

        // Returns the unit vector 
        return P.normalized;
    }
}
