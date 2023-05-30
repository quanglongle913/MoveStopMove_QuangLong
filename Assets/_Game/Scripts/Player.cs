using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    [SerializeField] private FloatingJoystick floatingJoystick;
    [SerializeField] private float moveSpeed = 5.0f;
    [SerializeField] private Rigidbody rigidbody;
    private float horizontal;
    private float vertical;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void FixedUpdate()
    {
        horizontal = floatingJoystick.Horizontal;
        vertical = floatingJoystick.Vertical;
        if (Mathf.Abs(horizontal) >= 0.03 || Mathf.Abs(vertical) >= 0.03)
        {
            Vector3 _Direction = new Vector3(horizontal * moveSpeed * Time.fixedDeltaTime, rigidbody.velocity.y, vertical * moveSpeed * Time.fixedDeltaTime);
            Vector3 TargetPoint = new Vector3(rigidbody.position.x + _Direction.x, rigidbody.position.y, rigidbody.position.z + _Direction.z);
            RotateTowards(this.gameObject, _Direction);
            transform.position = TargetPoint;
            ChangeAnim("Run");
        }
        else if (horizontal == 0 || vertical == 0)
        {
            ChangeAnim("Idle");
        }
    }
}
