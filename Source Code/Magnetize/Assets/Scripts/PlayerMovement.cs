﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb2D;
    public float moveSpeed = 5f;
    public float pullForce = 100f;
    public float rotateSpeed = 360f;
    private GameObject closestTower;
    private GameObject hookedTower;
    private bool isPulled = false;
    private UIControllerScript uiControl;
    private AudioSource myAudio;
    private bool isCrashed = false;
    private bool isGreen1 = false;
    private bool isGreen2 = false;
    private GameObject dummy;
    //private GameObject dummy2;
    private Vector3 startPosition;


    // Start is called before the first frame update
    void Start()
    {
       startPosition = this.transform.localPosition;
       rb2D = this.gameObject.GetComponent<Rigidbody2D>();
       myAudio = this.gameObject.GetComponent<AudioSource>();
       uiControl = GameObject.Find("Canvas").GetComponent<UIControllerScript>();
       
    }

    // Update is called once per frame
    void Update()
    {
         //Debug.Log(rb2D.angularVelocity);
        //Debug.Log(rb2D.velocity);
        rb2D.velocity = -transform.up * moveSpeed;

        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

            RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);
            if (hit.collider.name == "TowerBottom") // green1
            {
                Debug.Log("Tower Bottom");
                hookedTower = hit.collider.gameObject;
                if (isGreen2)
                {
                    isGreen2 = false;

                }
                    hookedTower.gameObject.GetComponent<SpriteRenderer>().color = Color.green;
                    isGreen1 = true;

            }
            else if (hit.collider.name == "TowerTop") //green2
            {
                Debug.Log("Tower Top");
                hookedTower = hit.collider.gameObject;
                if (isGreen1)
                {
                    isGreen2 = false;
                } 
                    hookedTower.gameObject.GetComponent<SpriteRenderer>().color = Color.green;
                    isGreen1 = true;
            }
        }

        if(!isGreen1)
        {
            dummy = GameObject.Find("TowerBottom");
            dummy.GetComponent<SpriteRenderer>().color = Color.white;
        }
        else if(!isGreen2)
        {
            dummy = GameObject.Find("TowerTop");
            dummy.GetComponent<SpriteRenderer>().color = Color.white;
        }

        if (Input.GetKey(KeyCode.Z) && !isPulled)
        {
            if (closestTower != null && hookedTower == null)
            {
                hookedTower = closestTower;
            }
            if (hookedTower)
            {
                float distance = Vector2.Distance(transform.position, hookedTower.transform.position);

                //Gravitation toward tower
                Vector3 pullDirection = (hookedTower.transform.position - transform.position).normalized;
                float newPullForce = Mathf.Clamp(pullForce / distance, 20, 50);
                rb2D.AddForce(pullDirection * newPullForce);

                //Angular velocity
                rb2D.angularVelocity = -rotateSpeed / distance;
                isPulled = true;
            }
            if (isCrashed)
            {
                if (!myAudio.isPlaying)
                {
                    restartPosition();
                }
                else
                {
                    //Move the object
                    rb2D.velocity = -transform.up * moveSpeed;
                    rb2D.angularVelocity = 0f;
                }
            }
        }

        if (Input.GetKeyUp(KeyCode.Z))
        {
            rb2D.angularVelocity -= 100;
            isPulled = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Wall")
        {
            //this.gameObject.SetActive(false);
            if (!isCrashed)
            {
                //Play SFX
                myAudio.Play();
                rb2D.velocity = new Vector3(0f, 0f, 0f);
                rb2D.angularVelocity = 0f;
                isCrashed = true;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Goal")
        {
            Debug.Log("Level Clear");
            uiControl.endGame();
        }
    }
    public void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Tower" && hookedTower == null)
        {
            closestTower = collision.gameObject;

            //Change tower color back to green as indicator
            collision.gameObject.GetComponent<SpriteRenderer>().color = Color.green;
        }
    }
    public void OnTriggerExit2D(Collider2D collision)
    {
        if (isPulled) return;

        if (collision.gameObject.tag == "Tower")
        {
            closestTower = null;

            //Change tower color back to normal
            collision.gameObject.GetComponent<SpriteRenderer>().color = Color.white;
        }
    }
    public void restartPosition()
    {
        //Set to start position
        this.transform.position = startPosition;

        //Restart rotation
        this.transform.rotation = Quaternion.Euler(0f, 0f, 90f);

        //Set isCrashed to false
        isCrashed = false;

        if (closestTower)
        {
            closestTower.GetComponent<SpriteRenderer>().color = Color.white;
            closestTower = null;
        }

    }
}
