using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Ball : MonoBehaviour
{
   public float speed;
   private Rigidbody2D rb;
   float ydir = 0;
   float xdir = 0;
   Vector2 startPos = new Vector2 (0,0);

   //Score
   public GameObject scoreLeftGO;
   public GameObject scoreRightGO;
   TextMeshProUGUI scoreLeftText;
   TextMeshProUGUI scoreRightText;
   int scoreLeft = 0;
   int scoreRight = 0;

    //Sound
    public AudioClip score;
    public AudioClip hitWall;
    public AudioClip hitRacket;
    AudioSource audioSource;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        scoreLeftText = scoreLeftGO.GetComponent<TextMeshProUGUI>();
        scoreRightText = scoreRightGO.GetComponent<TextMeshProUGUI>();
        audioSource = GetComponent<AudioSource>();
        //initial pos
        transform.position = startPos;
        //initial velocity
        rb.velocity = new Vector2(1,0) * speed;
    }

    void OnCollisionEnter2D(Collision2D col) 
    {
        //Hit a Wall?
        if (col.gameObject.tag == "Wall"){audioSource.PlayOneShot(hitWall); return;}
        //Hit left or right wall?
        else if (col.gameObject.tag == "WallLeft") {
            ResetBall();
            //point for player right
            scoreRight++;
            scoreRightText.text = scoreRight.ToString(); 
        }
        else if (col.gameObject.tag == "WallRight") {
            ResetBall();
            //point for player left
            scoreLeft++;
            scoreLeftText.text = scoreLeft.ToString(); 
        }
        //Hit a Racket?
        else if (col.gameObject.name == "PlayerLeft") {
            //play sound
            audioSource.PlayOneShot(hitRacket);
            //change dir
            xdir = 1; 
            // Calculate hit Factor
            ydir = hitFactor(transform.position, col.transform.position, col.collider.bounds.size.y); 
        } 
        else if (col.gameObject.name == "PlayerRight") {
            audioSource.PlayOneShot(hitRacket);
            xdir = -1; 
            // Calculate hit Factor
            ydir = hitFactor(transform.position, col.transform.position, col.collider.bounds.size.y); 
        }

        // Calculate new direction, make length=1 via .normalized
        Vector2 dir = new Vector2(xdir, ydir).normalized;
        // Set Velocity with dir * speed
        rb.velocity = dir * speed;
    }

    float hitFactor(Vector2 ballPos, Vector2 racketPos, float racketHeight) 
    {
        // ascii art:
        // ||  1 <- at the top of the racket
        // ||
        // ||  0 <- at the middle of the racket
        // ||
        // || -1 <- at the bottom of the racket
        return (ballPos.y - racketPos.y) / racketHeight;
    }

    void ResetBall()
    {
            
          //play sound
          audioSource.PlayOneShot(score);
          //reset pos
          transform.position = startPos; 
          //reset direction
          ydir = 0;
    }
}
