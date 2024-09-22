using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Player : MonoBehaviour
{
    Rigidbody2D rb;
    Animator animator;
    [SerializeField]Vector3 startingPosition;
    [SerializeField]GameObject panel;
    private bool hasKey = false;
    private Transform transform;
    private bool hasDbjump = false;
    private float horizontal;
    private bool dashing = false;
    private bool isFacingRight = true;
    [SerializeField]private float speed = 8f;
    [SerializeField] private float coyoteTime = 0.9f; // Adjust as needed
    private float coyoteTimeCounter; 
    
    [SerializeField]private float jumpingPower = 12f;
    [SerializeField]private float dashPower = 10f;
    private bool onGround = false;
    [SerializeField]bool[] abilities={false,//walk left
    false,//jump
    false,//dash
    false,//double jump
    false,//pause
    false,//credits
    };

    // Start is called before the first frame update
    void Start()
    {
     animator=GetComponent<Animator>();
     rb=GetComponent<Rigidbody2D>();
     transform=GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
    if (dashing)return;//disable input while dashing
    coyoteTimeCounter = onGround?coyoteTime:coyoteTimeCounter - Time.deltaTime;
    horizontal = Input.GetAxisRaw("Horizontal");
    animator.SetBool("walking",horizontal!=0&&onGround);

        if (isFacingRight && horizontal < 0f || !isFacingRight && horizontal > 0f)
        {
            //actually you can turn left so you feel bad cuz you can't go there
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    if (!abilities[1])return;//exit if you didn't unlock that
    if ((Input.GetButtonDown("Jump")||Input.GetKey(KeyCode.W)) && (onGround||hasDbjump))
    {
        rb.velocity = new (rb.velocity.x, jumpingPower);
        if (!onGround){//consume dbjump
            hasDbjump=false;
            return;
        }
        animator.SetBool("jumping",true);
        animator.SetBool("walking",false);
        onGround = false;
    }
    if(!abilities[2])return;//exit if you didn't unlock that
    if (Input.GetKey(KeyCode.S))
    {
        dash();
    }
    if(!abilities[4])return;//exit if you didn't unlock that
    if (Input.GetKey(KeyCode.P))
    {
        pause();
    }
    }
    private void OnCollisionEnter2D(Collision2D other) {
        switch (other.gameObject.tag) {
            case "ground":
                onGround = true;
                animator.SetBool("jumping",false);
                if (abilities[3]) hasDbjump = true;//recharge dbjump
                break;
            case "hazard"://all things that hurt
                die();
                break;
            case "left":
                unlock(0);
                Destroy(other.gameObject);
                break;
            case "key":
                hasKey = true;
                Destroy(other.gameObject);
                break;
            case "win":
                if (hasKey)
                    SceneManager.LoadScene("win");
                break;
            case "jump":
                unlock(1);
                Destroy(other.gameObject);
                break;
            case "dash":
                unlock(2);
                Destroy(other.gameObject);
                break;
            case "dbjump":
                unlock(3);
                Destroy(other.gameObject);
                break;
            case "ui.pause":
                unlock(4);
                Destroy(other.gameObject);
                break;
            case "ui.creds":
                unlock(5);
                Destroy(other.gameObject);
                break;
            default:
                break;
        }
    }

    private void FixedUpdate() {
        if (!isFacingRight&&!abilities[0])return;//don't go left if you didn't unlock that
        // Only allow horizontal movement if on the ground or within coyote time
    if (onGround || coyoteTimeCounter > 0)
    {
        rb.velocity = new(horizontal * speed, rb.velocity.y);
    }
    }
    private void die(){
        if (dashing) return;
        teleportToStart();
    }
    private void teleportToStart(){
        transform.position = startingPosition;
        rb.velocity = new(0f,0f);
    }
    private void dash(){
        dashing=true;
        rb.velocity=new(isFacingRight?dashPower:-1*dashPower,0f);
        Invoke("endDash",1f);
    }
    private void endDash(){
        dashing=false;
        rb.velocity=new(0f,0f);
    }
    private void unlock(int i){
        abilities[i]=true;
    }
    private void pause(){
        Time.timeScale=0;
        if (!abilities[5])return;
        panel.GetComponent<panelScript>().lockCreds();
    }
}
