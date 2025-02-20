using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Movimentacao : MonoBehaviour
{
    public Animator animator;
    float input_x = 0;
    float input_y = 0;
    public float speed = 2.5f;
    public bool isWalking = false;

    Rigidbody2D rb;
    Vector2 movement = Vector2.zero;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        isWalking = false;
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        input_x = Input.GetAxisRaw("Horizontal");
        input_y = Input.GetAxisRaw("Vertical");
        isWalking = (input_x != 0 || input_y != 0);

        movement = new Vector2(input_x, input_y);
        if(isWalking)
        {
            animator.SetFloat("input_x", input_x);
            animator.SetFloat("input_y", input_y);
            if(Input.GetAxis("Horizontal") < 0)
            {
                transform.eulerAngles = new Vector3(0f, 180f, 0f);
            }
            else if(Input.GetAxis("Horizontal")  > 0)
            {
                transform.eulerAngles = new Vector3(0f, 0f, 0f);
            }
        }

        animator.SetBool("isWalking", isWalking);
    }

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement.normalized * speed * Time.fixedDeltaTime);
    }
}
