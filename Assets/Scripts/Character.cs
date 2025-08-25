using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Character : MonoBehaviour
{
    #region FIELDS & ATTRIBS
    // Camera
    [Header("-- CAMERA --")]
    [LabelOverride("Camera")] public Camera cam;
    [LabelOverride("Vertical Sensitivity")] public float vSens;
    [LabelOverride("Horizontal Sensitivity")] public float hSens;

    private float _vRot;
    private float _hRot;

    [LabelOverride("Camera Target")] public GameObject tgt;
    [LabelOverride("Axis Target")] public GameObject axisTgt;

    [LabelOverride("Camera Trailing")] public float t;
    
    [Tooltip("Sets the minimum (X) and maximum (Y) vertical rotation limits for the camera.")]
    [SerializeField] [LabelOverride("Set Camera Angle Limits")] private Vector2 angleLimits = new Vector2(-90, 90);

    [LabelOverride("Camera Target Offset")] public Vector3 camOffset;

    // Character
    [Header("-- CHARACTER --")] 
    [LabelOverride("Character Speed")] public float speed;
    private float _finalSpeed;
    
    [LabelOverride("Minimum Health")] public int minHealth = 0;
    [LabelOverride("Maximum Health")] public int maxHealth = 100;

    [LabelOverride("Is Third Person Default?")] public bool isThirdPerson = true;

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
        CamControl();

        if (gotDmg)
        {
            AlterHealth(-10);
            gotDmg = false;
            
        }
        
        if (Input.GetKeyDown(KeyCode.V))
            isThirdPerson = !isThirdPerson;
        
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
        
        transform.position += Vector3.ClampMagnitude((fwd + rgt), 1) * _finalSpeed * Time.deltaTime;

        transform.forward = Vector3.Slerp(transform.forward, camFwd, 0.05f);

    }

    public void CameraControlThirdPerson()
    {
        axisTgt.transform.eulerAngles = new Vector3(_vRot, _hRot, 0);
        
        cam.transform.position =
            Vector3.Lerp(cam.transform.position, tgt.transform.position, Mathf.SmoothStep(0, 1, t));
        
        cam.transform.LookAt(axisTgt.transform.position);

    }

    private void CameraControlFirstPerson()
    {
        cam.transform.position = axisTgt.transform.position + camOffset;
        
        cam.transform.localRotation = Quaternion.Euler(_vRot, _hRot, 0);
        
    }

    private void CamControl()
    {
        _hRot += Input.GetAxis("Mouse X") * hSens;
        _vRot -= Input.GetAxis("Mouse Y") * vSens;
        
        _vRot = Mathf.Clamp(_vRot, angleLimits.x, angleLimits.y);
        
        if (isThirdPerson)
            CameraControlThirdPerson();
        
        else
            CameraControlFirstPerson();
        
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
