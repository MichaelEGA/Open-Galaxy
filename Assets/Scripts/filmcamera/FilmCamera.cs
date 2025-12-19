using UnityEngine;

public class FilmCamera : MonoBehaviour
{
    public Scene scene;
    public GameObject filmCameraGO;
    public GameObject cockpitCameraGO;
    public GameObject moveGO;
    public GameObject rotateGO;
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
    public float rotateSpeed = 6;

    //Cockpit Camera
    public bool cockpitActivated;
    public GameObject targetShipCameraGO;
    public GameObject cockpitGO;
    public LayerMask savedLayerMask;

    //Other Effects
    public bool blackbars = false;
    public bool blackbarsActive = false;
    
    //Shake camera
    public bool shakeCamera = false;
    public float shakeRate = 1f;
    public float shakeStrength = 0.5f;
    public float smoothTime = 0.1f;
    
    //Pan and zoom
    public float moveSpeed;
    public bool moveActive = false;
    public string moveAxis;

    //Rotation
    public float secondaryRotationSpeed;
    public bool rotateActive = false;
    public string rotationAxis;

    void FixedUpdate()
    {
        FilmCameraFunctions.RunFilmCamera(this);
    }
}
