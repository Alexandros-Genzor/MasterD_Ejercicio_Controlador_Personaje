using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
    private float _finalSpeed;
    
    [LabelOverride("Minimum Health")] public int minHealth = 0;
    [LabelOverride("Maximum Health")] public int maxHealth = 100;

    private int _health;
    public int Health { get { return _health; } set { _health = Mathf.Clamp(value, minHealth, maxHealth); } }

    [LabelOverride("Got Damaged?")] public bool gotDmg;
    [LabelOverride("Run Speed Multiplier")] public float runMult;
    [LabelOverride("Crouch Speed Multiplier")] public float crouchMult;

    private bool _IsCrouching;
    private bool _isRunning;
    
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

    private void OnCollisionEnter(Collision other)
    {
        
        
    }

    #endregion

    public void AlterHealth(int healthChange)
    {
        Health += healthChange;
        
    }

    public void Movement()
    {
        Vector3 camFwd = cam.transform.forward;
        camFwd.y = 0;
        
        _IsCrouching = Input.GetKey(KeyCode.LeftControl);
        _isRunning = Input.GetKey(KeyCode.LeftShift);
        
        transform.GetChild(0).localScale = new Vector3(1, (_IsCrouching ? 0.5f : 1), 1);
        _finalSpeed = speed * SpeedVariations(_IsCrouching, _isRunning);

        Vector3 fwd = transform.forward * Input.GetAxis("Vertical");
        Vector3 rgt = transform.right * Input.GetAxis("Horizontal");

        Vector3 dir = Vector3.ClampMagnitude((fwd + rgt), 1);
        
        transform.position += dir * _finalSpeed * Time.deltaTime;

        transform.forward = Vector3.Slerp(transform.forward, camFwd, 0.05f);
        
        Debug.Log(dir.magnitude);

    }

    public void CameraControl()
    {
        cam.transform.position =
            Vector3.Lerp(cam.transform.position, tgt.transform.position, Mathf.SmoothStep(0, 1, t));

        // vRot += Input.GetAxis("Mouse Y") * vSens;
        hRot += Input.GetAxis("Mouse X") * hSens;
        
        axisTgt.transform.eulerAngles = new Vector3(0, hRot, 0);
        
        cam.transform.LookAt(axisTgt.transform.position);

    }
    
    private float SpeedVariations(bool crouch, bool run)
    {
        if (crouch)
            return crouchMult;
        
        else if (run)
            return runMult;

        else
            return 1;

    }
    
}
