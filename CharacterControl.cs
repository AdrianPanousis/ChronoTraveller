using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterControl : MonoBehaviour
{
    //contrtols the direction the stick movement moves the user
    private Vector3 MoveDirection;
    //controls how fast the character moves and is set in the editor
    public float MovementSpeed = 0;
    //stores the camera to control the rotation
    public Camera cam;
    //stores the parent of the camera to control the position
    public GameObject camHolder;
    //stores the virtual joystick
    public VariableJoystick MoveJoystick;



    
   
    void Update()
    {
        //gets stick input data and changes the movement direction
        MoveDirection = new Vector3(MoveJoystick.Direction.x, 0, MoveJoystick.Direction.y);
        //moves the character
        transform.Translate(MoveDirection*Time.deltaTime*MovementSpeed);
        //matches the player rotation with the camera
        transform.rotation = cam.transform.rotation;
        //moves the camera parent
        camHolder.transform.position = transform.position;


    }
}
