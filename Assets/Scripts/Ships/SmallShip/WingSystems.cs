using UnityEngine;

public class WingSystems : MonoBehaviour
{
    public Ship ship; 
    public bool wingsOpen = true;
    public Transform[] wings;
    public GameObject wing01;
    public GameObject wing02;
    public GameObject wing03;
    public GameObject wing04;
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
