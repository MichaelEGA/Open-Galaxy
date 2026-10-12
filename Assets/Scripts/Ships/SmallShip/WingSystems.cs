using UnityEngine;

public class WingSystems : MonoBehaviour
{
    [Header("Key References")]
    public Ship ship;

    [Header("Wing Functions")]
    public bool wingsOpen = true;
    
    [Header("Wing GameObjects")]
    public Transform[] wings;
    public GameObject wing01;
    public GameObject wing02;
    public GameObject wing03;
    public GameObject wing04;

    [Header("Wing Positions")]
    public GameObject wing01_open;
    public GameObject wing01_closed;
    public GameObject wing02_open;
    public GameObject wing02_closed;
    public GameObject wing03_open;
    public GameObject wing03_closed;
    public GameObject wing04_open;
    public GameObject wing04_closed;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ship = GetComponent<Ship>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
