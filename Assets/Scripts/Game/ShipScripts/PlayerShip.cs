using UnityEngine;
using System.Collections;
using System;

public class PlayerShip : Ship
{
    private float defaultHealth;
    public static PlayerShip Instance;
    public Animator animator;
    public event Action OnPlayerDestroyed;

    public float DefaultHealth => defaultHealth;

    void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
    }
    
    protected override void CustomStart()
    {
        defaultHealth = health;
    }
    
    protected override void Move()
    {
        if(moveDirection.magnitude > 0)
        {
            rigidBody.velocity = moveDirection * moveSpeed;
        } else {
            rigidBody.velocity -= rigidBody.velocity * friction;
        }
    }
    
    void Update()
    {
        moveDirection = new Vector2(Input.GetAxis("Horizontal"),Input.GetAxis("Vertical")).normalized;

        Vector2 GetMousePos()
        {
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = Camera.main.nearClipPlane;
            Vector3 Worldpos = Camera.main.ScreenToWorldPoint(mousePos);
            Vector2 Worldpos2D = new Vector2(Worldpos.x,Worldpos.y);
            return Worldpos2D;
        }

        Vector2 shootDirection = (GetMousePos() - new Vector2(transform.position.x,transform.position.y)).normalized;
        transform.eulerAngles = new Vector3(0, 0, -90 + Mathf.Atan2(shootDirection.y, shootDirection.x)*180/Mathf.PI);

        if(Input.GetMouseButton(0))
        {
            if(canShoot)
            {
                StartCoroutine(Shoot(shootDirection,shootForce));
                animator.SetBool("isShooting",true);
            } else {
                animator.SetBool("isShooting",false);
            }
        }
    }

    protected override void DestroyShip()
    {   
        if (OnPlayerDestroyed != null)
        {
            OnPlayerDestroyed.Invoke();
        }
        base.DestroyShip();
    }
}