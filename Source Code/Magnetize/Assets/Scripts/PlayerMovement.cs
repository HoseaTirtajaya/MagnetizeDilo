using System.Collections;
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

    // Start is called before the first frame update
    void Start()
    {
       rb2D = this.gameObject.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
       //Move the object
       rb2D.velocity = -transform.up * moveSpeed;

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
                isPulled = true;

                //Angular velocity
                rb2D.angularVelocity = -rotateSpeed / distance;
                isPulled = true;
            }
        }

        if (Input.GetKeyUp(KeyCode.Z))
        {
            //float distance = Vector2.Distance(transform.position, hookedTower.transform.position);
            isPulled = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Wall")
        {
            this.gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Goal")
        {
            Debug.Log("Level Clear");
        }
    }
    public void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Tower")
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
}
