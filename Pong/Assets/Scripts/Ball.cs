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
    public AudioClip scoreRetro;
    public AudioClip hitWallRetro;
    public AudioClip hitRacketRetro;

    //vfx
    public ParticleSystem particleSytem;
    public GameObject playerLeft;
    public GameObject playerRight;
    public Vector3 racketPunchAmount;
    public Vector3 ballPunchAmount;
    public Color hitWallColor;
    public GameObject cam;
    public Vector3 camShakeAmount;
    public Vector3 scorePunchAmount;

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
        if (col.gameObject.tag == "Wall")
        {
            iTween.PunchScale(this.gameObject, ballPunchAmount, 0.5f);
            audioSource.PlayOneShot(hitWall); 
            return;
        }
        if (col.gameObject.tag == "WallHorizontalRight") 
        {
           // audioSource.PlayOneShot(hitWallRetro);
            return;
        }
        //Hit left or right wall?
        else if (col.gameObject.tag == "WallLeft")
        {
            iTween.PunchScale(this.gameObject, ballPunchAmount, 0.5f);
            //play sound
            audioSource.PlayOneShot(score, 0.5f);
            //play particle
            particleSytem.transform.position = this.transform.position;
            particleSytem.Play();
            //screenshake
            iTween.ShakePosition(cam, camShakeAmount, 0.5f);
            //reset pos
            transform.position = startPos;
            //reset direction
            ydir = 0;
            //point for player right
            scoreRight++;
            scoreRightText.text = scoreRight.ToString();
            //punch score
            iTween.PunchScale(scoreRightGO, scorePunchAmount, 0.5f);
        }
        else if (col.gameObject.tag == "WallRight")
        {
            //play sound
           // audioSource.PlayOneShot(scoreRetro);
            //reset pos
            transform.position = startPos;
            //reset direction
            ydir = 0;
            //point for player left
            scoreLeft++;
            scoreLeftText.text = scoreLeft.ToString();
            
        }

        //Hit a Racket?
        else if (col.gameObject.name == "PlayerLeft")
        {
            iTween.PunchScale(this.gameObject, ballPunchAmount, 0.5f);
            //play sound
            audioSource.PlayOneShot(hitRacket);
            //punch
            iTween.PunchScale(playerLeft, racketPunchAmount, 0.5f);
            //change dir
            xdir = 1;
            // Calculate hit Factor
            ydir = hitFactor(transform.position, col.transform.position, col.collider.bounds.size.y);
        }
        else if (col.gameObject.name == "PlayerRight")
        {
            //audioSource.PlayOneShot(hitRacketRetro);
            // iTween.PunchScale(playerRight, racketPunchAmount, 0.5f);
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

}
