using UnityEngine;

public class OGCamera : MonoBehaviour
{
    public Scene scene;
    public GameObject mainCamera;
    public GameObject cockpitCamera;
    public GameObject starfieldCamera;
    public GameObject planetCamera;
    public GameObject moveGO;
    public GameObject rotateGO;
    public Vector3 position;
    public Quaternion rotation;

    //Film camera mode
    public string mode = "gamemode";

    //Game Mode
    public string viewType = "thirdperson";
    public float toggleGameCameraPressTime;

    //Film Camera Shot Type
    public string shotType = "freelook";

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
    
    //Pan and zoom
    public float moveSpeed;
    public bool moveActive = false;
    public string moveAxis;

    //Rotation
    public float secondaryRotationSpeed;
    public bool rotateActive = false;
    public string rotationAxis;

    //Shake and hit
    public float hitTime;
    public bool shipHit;

    void FixedUpdate()
    {
        OGCameraFunctions.RunCamera(this);
    }
}
