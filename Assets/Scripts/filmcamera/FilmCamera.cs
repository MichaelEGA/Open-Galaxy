using UnityEngine;

public class FilmCamera : MonoBehaviour
{
    public Scene scene;
    public GameObject filmCamera;

    public Vector3 position;
    public Quaternion rotation;

    //Film Camera Modes
    public string mode = "freelook";

    //Free look mode values
    public bool looking = false;
    public float movementSpeed = 10f;
    public float fastMovementSpeed = 100f;
    public float freeLookSensitivity = 3f;

    //Look at ship static
    public string targetName;
    public GameObject targetShip;

    //Other Effects
    public bool blackbars = false;

    void FixedUpdate()
    {
        FilmCameraFunctions.RunFilmCamera(this);
    }
}
