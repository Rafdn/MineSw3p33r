using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineSceneMovement : MonoBehaviour
{
    private CharacterController cc;
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; 
        cc = GetComponent<CharacterController>();
    }

    
    void Update()
    {
        
        
        float moveSpeed = 20.0f;
        float dt = Time.deltaTime;
        float dy =  0;
        if(Input.GetKey(KeyCode.Space))
        {
            dy = moveSpeed * dt;
        }
        if(Input.GetKey(KeyCode.LeftShift))
        {
            dy -= moveSpeed * dt;
        }
        float dx = Input.GetAxis("Horizontal") * dt * moveSpeed;
        float dz= Input.GetAxis("Vertical") * dt * moveSpeed;
           
        cc.Move(transform.TransformDirection(new Vector3(dx, dy,dz)));
    }
}
