using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
[RequireComponent(typeof(Trajectory))]

// click and drag to aim
// interacts with an Aimer to help the player

// Represents all ball behaviors

public class Ball : MonoBehaviour
{
    [SerializeField] float sensitivity;         // how much the power scales per unit the mouse is dragged
    [SerializeField] float maxPower;

    [SerializeField] AudioSource bounceSFX;
    [SerializeField] AudioSource wheeSFX;

    Trajectory trajPredictor;
    Camera cam;

    Rigidbody2D rb;

    bool isLaunched;
    private Vector2 trajectory;



    private bool playSound = false;

    ParticleSystem particles;
    //bool isDragged;
    // Start is called before the first frame update
    void Start()
    {
        // initializing!
        cam = Camera.main;
        rb = GetComponent<Rigidbody2D>();
        isLaunched = false;
        trajectory = new();

        trajPredictor = GetComponent<Trajectory>();

        particles = GetComponent<ParticleSystem>();

        // // preload sound clip
        // if (bounceSFX != null && bounceSFX.clip != null)
        // {
        //     bounceSFX.PlayOneShot(bounceSFX.clip, 0); // Preload clip silently
        // }
    }

    void Update()
    {
        // only launch once; outcome should always be either a win (in the bucket and correct color)
        // or loss (hit screen edge, in bucket but wrong color)
        if (!isLaunched)
        {
            rb.gravityScale = 0;

            // detect when mouse is held down
            if (Input.GetButton("Fire1"))
            {
                // prepare a trajectory
                trajPredictor.isVisible = true;
                trajectory = CalcTrajectory();
            }

            if (Input.GetButtonUp("Fire1"))
            {
                // launch the ball!
                Launch(trajectory);
            }
        }

        // restart the level if the ball is touching the edge of the screen
        if (IsTouchingScreenEdge())
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        if (playSound)
        {
            if (bounceSFX != null)
            {
                bounceSFX.pitch = Mathf.Clamp(rb.linearVelocity.magnitude / maxPower, 0.5f, 1.25f);
                bounceSFX.PlayOneShot(bounceSFX.clip);
            }

            playSound = false;
        }
    }


    // get rid of the ball
    // prepare to advance to next level
    public IEnumerator Yay()
    {
        //rb.linearVelocity = new();
        particles.Play();
        yield return new WaitForSeconds(1.5f);
        Explode();
    }

    // Get rid of the ball
    // play effects
    private void Explode()
    {
        // particle effects

        // make the ball dissapear
        Destroy(gameObject);
    }

    // return a Vector for initial velocity of the ball
    private Vector2 CalcTrajectory()
    {

        Vector3 mousePos = Input.mousePosition;
        mousePos.z = -cam.transform.position.z;
        mousePos = cam.ScreenToWorldPoint(mousePos);

        //Debug.Log($"Mouse position: {mousePos}");

        Vector3 pos = transform.position;
        Vector3 traj = Vector3.ClampMagnitude((-(mousePos - pos)) * sensitivity, maxPower);
        //Debug.Log($"Trajectory: {traj}");
        trajPredictor.CalcTrajectory(traj);
        return traj;
    }

    // passes a velocity vector to the ball
    private void Launch(Vector2 trajectory)
    {
        rb.gravityScale = 1.0f;
        rb.linearVelocity = trajectory;
        isLaunched = true;

        // play sound effect?
        if (wheeSFX != null)
        {
            wheeSFX.Play();
        }
    }

    private bool IsTouchingScreenEdge()
    {

        // following code modified slightly from Beck on unity forums
        // https://discussions.unity.com/t/how-to-detect-screen-edge-in-unity/459224/2
        Vector3 pos = cam.WorldToViewportPoint(transform.position);

        if (pos.x < 0.0) return true;
        if (1.0 < pos.x) return true;
        if (pos.y < 0.0) return true;
        if (1.0 < pos.y) return true;
        return false;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        playSound = true;
    }
}
