using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    Rigidbody2D rb;
    [SerializeField]Vector3 startingPosition;
    private Transform transform;
    private bool hasDbjump = false;
    private float horizontal;
    private bool dashing = false;
    private bool isFacingRight = true;
    private float speed = 8f;
    private float jumpingPower = 6f;
    private float dashPower = 10f;
    private bool onGround = false;
    private bool touchingWall = false;
    bool[] abilities={false,//walk left
    false,//jump
    false,//dash
    false,//double jump
    false,//pause
    false,//credits
    };

    // Start is called before the first frame update
    void Start()
    {
     rb=GetComponent<Rigidbody2D>();
     transform=GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
    if (dashing)return;//disable input while dashing

    horizontal = Input.GetAxisRaw("Horizontal");

        if (isFacingRight && horizontal < 0f || !isFacingRight && horizontal > 0f)
        {
            //actually you can turn left so you feel bad cuz you can't go there
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    if (!abilities[1])return;//exit if you didn't unlock that
    if (Input.GetButtonDown("Jump") && (onGround||hasDbjump))
    {
        rb.velocity = new (rb.velocity.x, jumpingPower);
        if (!onGround){//consume dbjump
            hasDbjump=false;
            return;
        }
        onGround = false;
    }
    if(!abilities[2])return;//exit if you didn't unlock that
    if (Input.GetKey("Shift"))
    {
        dash();
    }
    }
    private void OnCollisionEnter2D(Collision2D other) {
        switch (other.gameObject.tag) {
            case "ground":
                onGround = true;
                if (abilities[3]) hasDbjump = true;//recharge dbjump
                break;
            case "wall":
                touchingWall = true;
                break;
            case "hazard"://all things that hurt
                die();
                break;
            case "left":
                unlock(0);
                break;
            case "jump":
                unlock(1);
                break;
            case "dash":
                unlock(2);
                break;
            case "dbjump":
                unlock(3);
                break;
            case "ui.pause":
                unlock(4);
                break;
            case "ui.creds":
                unlock(5);
                break;
            default:
                break;
        }
    }
    private void OnCollisionExit2D(Collision2D other) {
        touchingWall = false;
    }
    private void FixedUpdate() {
        if (touchingWall||(!isFacingRight&&!abilities[0]))return;//don't go left if you didn't unlock that
        rb.velocity = new(horizontal * speed, rb.velocity.y);
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
}
