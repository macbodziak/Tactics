using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] [Tooltip("The distance of the camera from the terrain Point at Start of game")]
     float distance = 20.0f;
    [SerializeField] [Tooltip("The angle of the camera to the terrain at Start of game")] 
    float cameraAngle = 50.0f;
    [SerializeField] float speed = 1.0f;
    [SerializeField] float rotationSpeed = 1.0f;
    [SerializeField] float zoomSpeed = 1.0f;
    [SerializeField] float zoomMin = 8.0f;
    [SerializeField] float zoomMax = 30.0f;
    float deltaX;
    float deltaZ;
    float maxX;
    float maxZ;

    int rotDir;
    [SerializeField] Camera cam;
    // Vector3 lookAtPos;
    bool isRotating = false;

    private void Start()
    {
        rotDir = 0;
        SetUpStartPosition();
        SetClampValues();
        // SetlookAtPos();
    }
    void Update()
    {
        HandpeInput();
    }

    void SetClampValues()
    {
        maxX = Graph.instance.width * Graph.instance.nodeInterval;
        maxZ = Graph.instance.height * Graph.instance.nodeInterval;
    }

    // void SetlookAtPos()
    // {
    //     lookAtPos = transform.parent.position;
    // }
    void HandpeInput()
    {
        HandleLinearMovementInput();
        HandleRotationInput();
        HandleZoomInput();
    }

    void HandleLinearMovementInput()
    {
        deltaX = speed * Input.GetAxis("Horizontal") * Time.deltaTime;
        deltaZ = speed * Input.GetAxis("Vertical") * Time.deltaTime;

        if (deltaX == 0.0f && deltaZ == 0.0f)
        {
            return;
        }

        Vector3 pos = transform.localPosition;

        switch (rotDir)
        {
            case 0:
                pos.x += deltaX;
                pos.z += deltaZ;
                break;
            case 1:
                pos.x += deltaZ;
                pos.z -= deltaX;
                break;
            case 2:
                pos.x -= deltaX;
                pos.z -= deltaZ;
                break;
            case 3:
                pos.x -= deltaZ;
                pos.z += deltaX;
                break;
        }

        pos.x = Mathf.Clamp(pos.x, 0f, maxX);
        pos.z = Mathf.Clamp(pos.z, 0f, maxZ);

        transform.localPosition = pos;
    }

    void HandleRotationInput()
    {
        if (isRotating)
        {
            return;
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            StartCoroutine("RotateLeftCoroutine");
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            StartCoroutine("RotateRightCoroutine");
        }
    }

    void HandleZoomInput()
    {
        if (Input.GetKey(KeyCode.Insert))
        {
            ZoomIn();
        }
        else if (Input.GetKey(KeyCode.Delete))
        {
            ZoomOut();
        }
    }
    IEnumerator RotateRightCoroutine()
    {
        float angle = 0.0f;
        isRotating = true;
        while (angle < 90.0f)
        {
            cam.transform.RotateAround(transform.position, Vector3.up, rotationSpeed * Time.deltaTime);
            angle += rotationSpeed * Time.deltaTime;
            yield return null;
        }
        rotDir = (rotDir + 1) % 4;
        isRotating = false;
        yield return null;
    }

    IEnumerator RotateLeftCoroutine()
    {
        float angle = 0.0f;
        isRotating = true;
        while (angle < 90.0f)
        {
            cam.transform.RotateAround(transform.position, Vector3.up, -rotationSpeed * Time.deltaTime);
            angle += rotationSpeed * Time.deltaTime;
            yield return null;
        }
        rotDir = (rotDir + 3) % 4;
        isRotating = false;
        yield return null;
    }

    void SetUpStartPosition()
    {
        float camAngle = cameraAngle * Mathf.Deg2Rad;
        float z = -distance * Mathf.Cos(camAngle);
        float y = distance * Mathf.Sin(camAngle);
        Debug.Log("Mathf.Sin(camAngle) " + Mathf.Sin(camAngle));
        float x = 0.0f;

        cam.transform.localPosition = new Vector3(x, y, z);
    }



    void ZoomIn()
    {
        Vector3 MinPoint = transform.position - cam.transform.forward.normalized * zoomMin;
        cam.transform.position = Vector3.MoveTowards(cam.transform.position, MinPoint, zoomSpeed * Time.deltaTime);

        float dist = Vector3.Distance(transform.position, cam.transform.position);
        Debug.Log("distance: " + dist);
        // if (dist > zoomMin)
        // {
        //     cam.transform.position = newPos;
        // }
    }

    void ZoomOut()
    {
        Vector3 MaxPoint = transform.position - cam.transform.forward.normalized * zoomMax;
        cam.transform.position = Vector3.MoveTowards(cam.transform.position, MaxPoint, zoomSpeed * Time.deltaTime);
        
        float dist = Vector3.Distance(transform.position, cam.transform.position);
        Debug.Log("distance: " + dist);
    }
}
