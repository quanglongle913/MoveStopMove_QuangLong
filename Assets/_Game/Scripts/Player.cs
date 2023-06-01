using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Player : Character
{
    [SerializeField] private FloatingJoystick floatingJoystick;
    [SerializeField] private float moveSpeed = 5.0f;
    [SerializeField] private Rigidbody rigidbody;
    [SerializeField] private GameObject cylinder;
    [SerializeField] private GameObject weaponInHand;
    [SerializeField] private GameObject weaponAttack;
    private float horizontal;
    private float vertical;
    // Start is called before the first frame update
    void Start()
    {
        weaponAttack.SetActive(false);
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
        if (cylinder != null)
        {
            cylinder.transform.localScale = new Vector3(AttackRange * 2, 0.001f, AttackRange * 2);
        }
        horizontal = floatingJoystick.Horizontal;
        vertical = floatingJoystick.Vertical;
        if (Mathf.Abs(horizontal) >= 0.03 || Mathf.Abs(vertical) >= 0.03)
        {
            Vector3 _Direction = new Vector3(horizontal * moveSpeed * Time.fixedDeltaTime, rigidbody.velocity.y, vertical * moveSpeed * Time.fixedDeltaTime);
            Vector3 TargetPoint = new Vector3(rigidbody.position.x + _Direction.x, rigidbody.position.y, rigidbody.position.z + _Direction.z);
            RotateTowards(this.gameObject, _Direction);
            if (!isWall(LayerMask.GetMask(Constant.LAYOUT_WALL)))
            {
                transform.position = TargetPoint;
                ChangeAnim("Run");
            }
            
        }
        else if (horizontal == 0 || vertical == 0)
        {
            ChangeAnim("Idle");
        }
        EnableCircleAttack(cubesInsideZone, true);
        EnableCircleAttack(cubesOutsideZone, false);
        foreach (Collider hitcollider in cubesInsideZone)
        {
            if (hitcollider.GetComponent<BotAI>())
            {
                weaponAttack.transform.position = weaponInHand.transform.position;
                weaponAttack.SetActive(true);
                weaponInHand.SetActive(false);
                //Vector3 direction = (transform.position - hitcollider.GetComponent<BotAI>().gameObject.transform.position).normalized;
                return;
            }
            else
            {
                weaponAttack.SetActive(false);
                weaponInHand.SetActive(true);
            }
        }
    }
    private void EnableCircleAttack(Collider[] colliders, bool enable)
    {

        foreach (Collider hitcollider in colliders)
        {
            if (hitcollider.GetComponent<BotAI>())
            {
                hitcollider.GetComponent<BotAI>().CircleAttack.SetActive(enable);
            }
        }
    }
    private IEnumerator coroutineAttack(float time)
    {
        //ChangeAnim("Attack");
        weaponAttack.transform.position = weaponInHand.transform.position;
        weaponAttack.SetActive(true);
        weaponInHand.SetActive(false);
        yield return new WaitForSeconds(time);
        //ChangeAnim("Idle");
    }
    private void DetectionCharacter(Collider[] colliders)
    {
        foreach (Collider hitcollider in colliders)
        {
            if (hitcollider.GetComponent<BotAI>())
            {
                //Debug.Log(hitcollider.gameObject.name);
            }
        }
    }
}

