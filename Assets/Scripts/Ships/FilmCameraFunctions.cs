using UnityEngine;

public class FilmCameraFunctions : MonoBehaviour
{
    //This prepares the film camera
    public static void PrepareFilmCamera(FilmCamera filmCamera)
    {
        if (filmCamera.filmCameraPrepared == false)
        {
            SetMainCamera(filmCamera);
            filmCamera.filmCameraPrepared = true;
        }

        SceneFunctions.IdentifyAsMainCamera(filmCamera);
        ParentFilmCameraToScene(filmCamera);
    }

    public static void ParentFilmCameraToScene(FilmCamera filmCamera)
    {
        filmCamera.scene = SceneFunctions.GetScene();
        filmCamera.transform.parent = filmCamera.transform;
    }


    //This gets the main camera
    public static void SetMainCamera(FilmCamera filmCamera)
    {
        GameObject mainCamera = GameObject.Find("Main Camera");
        mainCamera.transform.parent = filmCamera.transform;
        filmCamera.mainCamera = mainCamera;
        SceneFunctions.ActivateFollowCamera(false);
    }

    //This allows the player to move the camera freely
    public static void FreeMoveCamera(FilmCamera filmCamera)
    {
        var currentMoveSpeed = Input.GetKey(KeyCode.LeftShift) ? filmCamera.fastMovementSpeed : filmCamera.movementSpeed;

        if (Input.GetKey(KeyCode.W))
            filmCamera.transform.position += filmCamera.transform.forward * currentMoveSpeed * Time.deltaTime;
        if (Input.GetKey(KeyCode.S))
            filmCamera.transform.position += - filmCamera.transform.forward * currentMoveSpeed * Time.deltaTime;
        if (Input.GetKey(KeyCode.A))
            filmCamera.transform.position += -filmCamera.transform.right * currentMoveSpeed * Time.deltaTime;
        if (Input.GetKey(KeyCode.D))
            filmCamera.transform.position += filmCamera.transform.right * currentMoveSpeed * Time.deltaTime;
        if (Input.GetKey(KeyCode.Q)) // Move up in world space
            filmCamera.transform.position += Vector3.up * currentMoveSpeed * Time.deltaTime;
        if (Input.GetKey(KeyCode.E)) // Move down in world space
            filmCamera.transform.position += -Vector3.up * currentMoveSpeed * Time.deltaTime;

        // 2. Rotation (Free Look)
        if (Input.GetKeyDown(KeyCode.Mouse1)) // Right-click to start looking
        {
            filmCamera.looking = true;
        }
        else if (Input.GetKeyUp(KeyCode.Mouse1)) // Right-click release to stop
        {
            filmCamera.looking = false;
        }

        if (filmCamera.looking)
        {
            float newRotationX = filmCamera.transform.localEulerAngles.y + Input.GetAxis("Mouse X") * filmCamera.freeLookSensitivity;
            // Negative sign for Y-axis to invert, matching standard camera controls
            float newRotationY = filmCamera.transform.localEulerAngles.x - Input.GetAxis("Mouse Y") * filmCamera.freeLookSensitivity;

            // Apply rotation, clamping the vertical look to prevent flipping
            // Note: This simple implementation is just for horizontal/vertical look
            filmCamera.transform.localEulerAngles = new Vector3(newRotationY, newRotationX, 0f);
        }
    }
}
