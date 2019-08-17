using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class ThrowDart : MonoBehaviour
{
    // Start is called before the first frame update
    Vector3 startpos, endpos, direction,angle;
    public float touchtimestart, touchtimeend, timeinterval;
    public Text Score;
    public GameObject Board;
    private float dragDistance;
    int points = 0;
    int t = 0;
    int[] scores = { 20, 1, 18, 4, 13, 6, 10, 15, 2, 17, 3, 19, 7, 16, 8, 11, 14, 9, 12, 5 };
    int[] f = { -700, 700 };
    private System.Random rand = new System.Random();

    // Start is called before the first frame update
    [Range(0.05f, 1f)]
    public float throwforce = 0.3f;
    void Start()
    {
        dragDistance = Screen.height * 15 / 100; //dragDistance is 15% height of the screen
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount == 1) // user is touching the screen with a single touch
        {
            Touch touch = Input.GetTouch(0); // get the touch
            if (touch.phase == TouchPhase.Began) //check for the first touch
            {
                startpos = touch.position;
                endpos = touch.position;
                touchtimestart = Time.time;
            }
            else if (touch.phase == TouchPhase.Moved) // update the last position based on where they moved
            {
                endpos = touch.position;
            }
            else if (touch.phase == TouchPhase.Ended) //check if the finger is removed from the screen
            {
                endpos = touch.position;
                touchtimeend = Time.time;
                timeinterval = touchtimeend - touchtimestart;
                direction = endpos - startpos;
                angle = new Vector3(0, direction.x /150, 0);

                //Check if drag distance is greater than 20% of the screen height
                if (Mathf.Abs(endpos.x - startpos.x) > dragDistance || Mathf.Abs(endpos.y - startpos.y) > dragDistance)
                {//It's a drag
                 //check if the drag is vertical or horizontal
                    if (Mathf.Abs(endpos.x - startpos.x) < Mathf.Abs(endpos.y - startpos.y))
                    {
                        if (endpos.y > startpos.y)  //If the movement was up
                        {   //Up swipe
                            transform.Rotate(angle );

                            GetComponent<Rigidbody>().AddForce(direction.x/ timeinterval * throwforce,- 850, direction.y / timeinterval * throwforce);

                        }
                    }
                }
            }
        }
    }



private void OnCollisionEnter()
    {
        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;

        CountPoints();
    }
    void CountPoints() {
        Vector2 d;
    d.x = transform.position.x;
        d.y = transform.position.y;
       
        float a = Vector2.Angle(Vector2.up, d) ;// Calculate points based on sector dart hits
        if ((d.x < 0) && (a > 9))
            a = 360 - a;
        float b = (a + 9) / 18;
        int i = Mathf.FloorToInt(b);
        
        int result = scores[i];
        /*float startAngle = 0 * Mathf.Rad2Deg;            SECTOR v1

       float endAngle = 9 *Mathf.Rad2Deg;
       float radius = 26.2f;

       float polarradius = (float)Mathf.Sqrt((d.x * d.x) + (d.y * d.y));
       float Angle = (float)Mathf.Atan(d.y / d.x) * Mathf.Rad2Deg;

       if (Angle >= startAngle && Angle <= endAngle && polarradius < radius)
           Debug.Log("HIT");
       else

    Debug.Log("MISS");*/
        float dis = Vector2.Distance(Vector2.zero, d) / Board.transform.localScale.z; //Calculate points multiplier and bullseye from distance
        if(dis<.905f)
        {
            result = 50;
        }
        if(dis>.905f && dis<2.02f)
        {
            result = 25;
        }
        if(dis<12.72f && dis>11.33f)
        {
            result *= 3;
        }
        if(dis<20.47f && dis>19.05f)
        {
            result *= 2;
        }
        if(dis>20.47f)
        {
            result = 0;
        }
        Score.text = result.ToString();



    }



}
