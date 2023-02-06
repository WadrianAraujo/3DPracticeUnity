using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private CharacterController _controller;
    private Vector3 _forward, _strafe, _vertical;

    private float _forwarSpeed = 5f; 
    private float _strafeSpeed = 5f; 
    private void Start()
    {
        _controller = GetComponent<CharacterController>();
    }

    private void FixedUpdate()
    {
        float forwardInput = Input.GetAxis("Vertical");
        float strafeInput = Input.GetAxis("Horizontal");

        _forward = forwardInput * _forwarSpeed * transform.forward;
        Vector3 finalVelocity = _forward + _strafe + _vertical;
        _controller.Move(finalVelocity * Time.deltaTime);
    }
}
