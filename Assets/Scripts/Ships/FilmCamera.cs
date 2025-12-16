using UnityEngine;

public class FilmCamera : MonoBehaviour
{
    public Scene scene;
    public GameObject mainCamera;

    public bool filmCameraPrepared;

    //Free look values
    public bool looking = false;
    public float movementSpeed = 10f;
    public float fastMovementSpeed = 100f;
    public float freeLookSensitivity = 3f;

    // Update is called once per frame
    void Update()
    {
        FilmCameraFunctions.PrepareFilmCamera(this);
    }

    void FixedUpdate()
    {
        FilmCameraFunctions.FreeMoveCamera(this);
    }
}
