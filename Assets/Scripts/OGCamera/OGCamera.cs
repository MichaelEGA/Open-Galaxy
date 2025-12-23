using UnityEngine;

public class OGCamera : MonoBehaviour
{
    //Scene reference
    public Scene scene;

    //Cameras and camera related gameobjects
    public GameObject mainCamera;
    public GameObject cockpitCamera;
    public GameObject starfieldCamera;
    public GameObject planetCamera;
    public GameObject moveGO;
    public GameObject rotateGO;
    public GameObject cockpitAnchor;

    //Key vaules
    public string targetName; //Target ship
    public GameObject targetShip;
    public Vector3 position; //Position and Rotation
    public Quaternion rotation;
    
    //Cockpit Values
    public bool cockpitActivated; //Cockpits
    public GameObject targetShipCameraGO;
    public GameObject cockpitGO;
    public LayerMask savedLayerMask;
    public Vector3 cockpitPosition; //Cockpit move values
    public Quaternion cockpitRotation;
    public float smoothThrust;
    public float movementTime; 
    public float randomisationX;
    public float randomisationY;
    public float x;
    public float y;
    public float z;

    //OG camera mode
    public string mode = "gamemode";

    //Game Mode values
    public string viewType = "thirdperson";
    public float toggleGameCameraPressTime;
    public float hitTime;
    public bool shipHit;

    //Film Mode values
    public string shotType = "freelook";
    public bool staticShotTaken = false;
    public bool looking = false;
    public float moveSpeed; //Pan and zoom
    public bool moveActive = false;
    public string moveAxis;
    public float secondaryRotationSpeed; //Rotation
    public bool rotateActive = false;
    public string rotationAxis;
    public bool shakeCamera = false; //Shake camera
    public float shakeRate = 1f;
    public float shakeStrength = 0.5f;

    void FixedUpdate()
    {
        OGCameraFunctions.RunCamera(this);
    }
}
