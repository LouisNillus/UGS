using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UGS_M_Camera : UGS_Module
{


    public int minFOV;
    public int maxFOV;

    Camera cam;
    float camZ;

    public Coroutine shakeRoutine;

    private void Start()
    {
        cam = Camera.main;
    }

    private void Update()
    {
        camZ = cam.transform.position.z;

        Debug.DrawLine(cam.transform.position, Methods.ZDistanceMousePos(0), Color.cyan);
    }

    public void Zoom(ZoomType type, int step, bool zoomToPointer = true, bool unzoomToCenter = true)
    {

        if (type == ZoomType.In && cam.fieldOfView == minFOV) return;
        if (type == ZoomType.Out && cam.fieldOfView == maxFOV) return;

        Vector3 mousePos = Methods.ZDistanceMousePos(0);

        mousePos.x = Mathf.Clamp(mousePos.x, grid.cells[0,0].position.x, grid.cells[grid.dimensions.x - 1, 0].position.x);
        mousePos.y = Mathf.Clamp(mousePos.y, grid.cells[0,0].position.y, grid.cells[0, grid.dimensions.y - 1].position.y);

        float zoomProgression = Mathf.InverseLerp(maxFOV, minFOV, cam.fieldOfView);
        float unzoomProgression = Mathf.InverseLerp(minFOV, maxFOV, cam.fieldOfView);

        Vector3 pos = cam.transform.position;

        if (type == ZoomType.In && zoomToPointer)
        {
            pos = Vector3.Lerp(cam.transform.position, mousePos, zoomProgression).ChangeZ(camZ);
        }
        else if(type == ZoomType.Out && unzoomToCenter)
        {
            pos = Vector3.Lerp(cam.transform.position, grid.GetFocusPoint(GridFocusPoint.Middle), unzoomProgression).ChangeZ(camZ);

        }

        Camera.main.transform.position = pos;

        Camera.main.fieldOfView += (type == ZoomType.Out ? step : -step);
        ClampCameraFOV();
    }
    
    public void ClampCameraFOV()
    {
        Camera.main.fieldOfView = Mathf.Clamp(Camera.main.fieldOfView, minFOV, maxFOV);
    }

    public IEnumerator Shake(float duration, float magnitude)
    {
        Vector3 initPos = Camera.main.transform.localPosition;

        float t = 0f;

        while (t < duration)
        {
            float xShake = Random.Range(-1f, 1f) * magnitude;
            float yShake = Random.Range(-1f, 1f) * magnitude;

            Camera.main.transform.localPosition = initPos + new Vector3(xShake, yShake, 0);
            yield return null;

            t += Time.deltaTime;
        }

        Camera.main.transform.localPosition = initPos;
    }
}

public enum ZoomType {In, Out}
