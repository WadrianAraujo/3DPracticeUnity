using System;
using UnityEngine;

public class FirstPersonCamera : MonoBehaviour
{
    public Transform characterBody;
    public Transform characterHead;

    public float sensitivityX = 0.5f;
    public float sensitivityY = 0.5f;

    public float smoothCoefx = 0.005f;
    public float smoothCoefy = 0.005f;
    
    private float _smoothRotX = 0;
    private float _smoothRotY = 0;
    
    private float _angleYMin = -90;
    private float _angleYMax = 90;
    
    private float _rotationX = 0;
    private float _rotationY = 0;


    private void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        float verticalDelta = Input.GetAxisRaw("Mouse Y") * sensitivityY;
        float horizontalDelta = Input.GetAxisRaw("Mouse X") * sensitivityX;
        
        _rotationX += horizontalDelta * Time.deltaTime;
        _rotationY += verticalDelta * Time.deltaTime;

        //makes the camera rotate more smoothly
        smoothCoefx = Mathf.Lerp(_smoothRotX, horizontalDelta, smoothCoefx);
        smoothCoefx = Mathf.Lerp(_smoothRotY, verticalDelta, smoothCoefy);

        _rotationX += _smoothRotX;
        _rotationY += _smoothRotY;
        
        //limits camera rotation on the y-axis
        _rotationY = Mathf.Clamp(_rotationY, _angleYMin, _angleYMax);
        
        //makes the player move towards the camera
        characterBody.localEulerAngles = new Vector3(0, _rotationX, 0);
        
        //move the camera as the mouse moves
        transform.localEulerAngles = new Vector3(-_rotationY, _rotationX, 0);
    }
    

    private void LateUpdate()
    {
        // moves to the camera position for the player's head
        transform.position = characterHead.position;
    }
}
