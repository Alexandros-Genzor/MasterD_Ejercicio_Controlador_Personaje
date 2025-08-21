using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    #region CAMERA
    [Header("-- CAMERA --")]
    [LabelOverride("Camera")] public Camera cam;
    [LabelOverride("Speed")] public float speed;
    [LabelOverride("Vertical Sensitivity")] public float vSens;
    [LabelOverride("Horizontal Sensitivity")] public float hSens;

    [LabelOverride("Vertical Rotation")] public float vRot;
    [LabelOverride("Horizontal Rotation")] public float hRot;

    [LabelOverride("Camera Target")] public GameObject tgt;
    [LabelOverride("Axis Target")] public GameObject axisTgt;

    [Header("-- CHARACTER --")] 
    [LabelOverride("Minimum Health")] public int minHealth;
    [LabelOverride("Maximum Health")] public int maxHealth;

    private int _health;
    public int Health { get { return _health; } set { _health = Mathf.Clamp(value, minHealth, maxHealth); } }

    [LabelOverride("Got Damaged?")] public bool gotDmg;
    
    #endregion
    
    #region LIFECYCLE_FUNCTIONS
    // Start is called before the first frame update
    void Start()
    {
        
        
    }

    // Update is called once per frame
    void Update()
    {
        
        
    }
    
    #endregion
    
}
