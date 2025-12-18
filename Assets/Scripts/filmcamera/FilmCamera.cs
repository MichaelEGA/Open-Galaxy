using UnityEngine;

public class FilmCamera : MonoBehaviour
{
    public Scene scene;
    public GameObject filmCamera;
    public bool active;
    public Vector3 position;
    public Quaternion rotation;

    //Film Camera Modes
    public string mode = "freelook";

    //Static shot
    public bool staticShotTaken = false;

    //Free look mode values
    public bool looking = false;
    public float movementSpeed = 10f;
    public float fastMovementSpeed = 100f;
    public float freeLookSensitivity = 3f;

    //Look at ship
    public string targetName;
    public GameObject targetShip;
    public float followSpeed = 16;
    public float rotationSpeed = 6;

    //Other Effects
    public bool blackbars = false;
    public bool blackbarsActive = false;
    public bool shakeCamera = false;
    public float shakeRate = 1f;
    public float shakeStrength = 0.5f;
    public float smoothTime = 0.1f;
    public Vector3 currentVelocity = Vector3.zero;
    public Vector3 originalPosition = Vector3.zero;

    void FixedUpdate()
    {
        FilmCameraFunctions.RunFilmCamera(this);
    }
}
