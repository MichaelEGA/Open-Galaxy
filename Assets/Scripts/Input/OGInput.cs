using UnityEngine;

public class OGInput : MonoBehaviour
{
    public Scene scene;
    public OGSettings settings;
    public MissionManager missionManager;

    //General input settings
    public bool keyboardAndMouse = true;
    public bool invertUpDown;
    public bool invertLeftRight;

    //General ship inputs
    public float rollInput;
    public float pitchInput;
    public float turnInput;
    public float thrustInput;

    public bool powerToShields;
    public bool powerToEngine;
    public bool powerToLasers;
    public bool resetPowerLevels;
    public bool getNextTarget;
    public bool getNextEnemy;
    public bool getClosestEnemy;
    public bool selectTargetInFront;
    public bool fireWeapon;
    public bool rapidFire;
    public bool toggleWeapons;
    public bool toggleWeaponNumber;
    public bool matchSpeed;
    public bool focusCamera;
    public bool fireCounterMeasures;

    //Choice node inputs
    public bool choiceUp;
    public bool choiceDown;
    public bool choiceRight;
    public bool choiceLeft;

    //Mouse specific values
    public Vector2 targetMouseInput;
    public Vector2 currentMouseInput;
    public float lerpSpeed = 2.5f;

    //Controller specific values
    public float controllerThrust;
    public float smoothedPitch;
    public float smoothedRoll;
    public float smoothedTurn;
    public float controllerPitch;
    public float controllerRoll;
    public float controllerTurn;
    public float controllerSensitivity;

    // Update is called once per frame
    void Update()
    {
        OGInputFunctions.UpdateInput(this);
    }
}
