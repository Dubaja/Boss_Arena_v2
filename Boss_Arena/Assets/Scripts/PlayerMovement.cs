using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
 	
 	public float moveSpeed = 5f;
 	Rigidbody2D rb;
 	public Camera cam;

 	Vector2 movement;
 	Vector2 mousePos;

    void Start(){
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        //Debug.Log("jebo mamu");
        mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
    }

    void FixedUpdate(){
    	rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);

    	Vector2 lookDir = mousePos - rb.position;
    	float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f;
    	rb.rotation = angle;
    }
}
