using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private CharacterController _controller;
    private Vector3 _forward, _strafe, _vertical;
    private Vector3 _direction;
        
    private float _forwarSpeed = 5f; 
    private float _strafeSpeed = 5f;

    
    private float _gravity;
    private float _jumpSpeed;
    private float _maxJumpHeight = 2f;
    private float _timeToMaxHeight = 0.5f;    
    private void Start()
    {
        _controller = GetComponent<CharacterController>();
        _gravity = (-2 * _maxJumpHeight) / (_timeToMaxHeight * _timeToMaxHeight);
        _jumpSpeed = (2 * _maxJumpHeight) / _timeToMaxHeight;
    }

    private void FixedUpdate()
    {
        float forwardInput = Input.GetAxis("Vertical");
        float strafeInput = Input.GetAxis("Horizontal");

        _forward = forwardInput * _forwarSpeed * transform.forward;
        _strafe = strafeInput * _strafeSpeed * transform.right;
        
        _vertical += _gravity * Time.deltaTime * Vector3.up;
        
        // verification to say it's touching the ground
        if (_controller.isGrounded)
        {
            _vertical = Vector3.down;
        }
        
        // UP
        if (Input.GetKeyDown(KeyCode.Space) && _controller.isGrounded)
        {
            _vertical = _jumpSpeed * Vector3.up;
        }

        // check to say if the player is hitting the ceiling
        if (_vertical.y > 0 &&(_controller.collisionFlags & CollisionFlags.Above) !=0)
        {
            _vertical = Vector3.zero;
        }
        
        Vector3 finalVelocity = _forward + _strafe + _vertical;
        _controller.Move(finalVelocity * Time.deltaTime);
    }
}
