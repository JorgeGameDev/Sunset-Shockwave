using UnityEngine;
using System.Collections;

/// Calculates the pixel scale requireed for pixel perfect art.
public class OrtographicCalculator : MonoBehaviour
{
    // Variables required for the calculator.
    [Header("Properties")]
    public int tileSize;

    private Camera _mainCamera;
    private float screenWidth;
    private float screenHeight;

    // Use this for initialization
    void Start()
    {
        // Gets the main camera.
        _mainCamera = Camera.main;

        // Gets the screen info based on the screen.
        screenWidth = Screen.width;
        screenHeight = Screen.height;

        // Used to update the camera size.
        UpdateCameraSize(tileSize);
    }

    // Debug Tools used to change the camera size.
    void Update()
    {
        if(Input.GetKeyUp(KeyCode.Alpha1))
        {
            UpdateCameraSize(tileSize);
        }

        if (Input.GetKeyUp(KeyCode.Alpha2))
        {
            UpdateCameraSize(tileSize * 2);
        }

        if (Input.GetKeyUp(KeyCode.Alpha3))
        {
            UpdateCameraSize(tileSize * 4);
        }
    }

    // Function used to update the camera size at runtime.s
    void UpdateCameraSize(int tileSize)
    {
        // Calculates the ortographic size for the camera.
        float screenSize = ((screenWidth / screenHeight) * 2);
        float ortoSize = (screenWidth / ((screenSize) * tileSize));
        _mainCamera.orthographicSize = ortoSize;
    }
}