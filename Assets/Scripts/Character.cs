using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    #region CAMERA
    [Header("-- CAMERA --")]
    [LabelOverride("Camera")] public Camera cam;
    [LabelOverride("Vertical Sensitivity")] public float vSens;
    [LabelOverride("Horizontal Sensitivity")] public float hSens;

    [LabelOverride("Vertical Rotation")] public float vRot;
    [LabelOverride("Horizontal Rotation")] public float hRot;

    [LabelOverride("Camera Target")] public GameObject tgt;
    [LabelOverride("Axis Target")] public GameObject axisTgt;

    [LabelOverride("Camera Trailing")] public float t;

    [Header("-- CHARACTER --")] 
    [LabelOverride("Character Speed")] public float speed;
    
    [LabelOverride("Minimum Health")] public int minHealth = 0;
    [LabelOverride("Maximum Health")] public int maxHealth = 100;

    private int _health;
    public int Health { get { return _health; } set { _health = Mathf.Clamp(value, minHealth, maxHealth); } }

    [LabelOverride("Got Damaged?")] public bool gotDmg;
    
    #endregion
    
    #region LIFECYCLE_FUNCTIONS
    // Start is called before the first frame update
    void Start()
    {
        // idk why declare both min and max health here when both are public and serialized :/
        _health = maxHealth;
        
        Debug.Log(cam);

        // automatically sets camera as the main one in scene if no camera reference has been dragged to the component.
        if (cam == null)
            cam = Camera.main;

    }

    // Update is called once per frame
    void Update()
    {
        Movement();
        CameraControl();

        if (gotDmg)
        {
            AlterHealth(-10);
            gotDmg = false;
            
        }
        
    }
    
    #endregion

    public void AlterHealth(int healthChange)
    {
        Health += healthChange;
        
    }

    public void Movement()
    {
        transform.position += transform.forward * Input.GetAxis("Vertical") * speed * Time.deltaTime;
        transform.position += transform.right * Input.GetAxis("Horizontal") * speed * Time.deltaTime;
        
    }

    public void CameraControl()
    {
        cam.transform.position =
            Vector3.Lerp(cam.transform.position, tgt.transform.position, Mathf.SmoothStep(0, 1, t));

        // vRot += Input.GetAxis("Mouse Y") * vSens;
        hRot += Input.GetAxis("Mouse X") * hSens;
        
        axisTgt.transform.eulerAngles = new Vector3(0, hRot, 0);
        
        cam.transform.LookAt(transform.position);

    }
    
}
