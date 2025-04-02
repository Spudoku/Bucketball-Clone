using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
[RequireComponent(typeof(Trajectory))]

// click and drag to aim
// interacts with an Aimer to help the player
public class Ball : MonoBehaviour
{
    [SerializeField] float sensitivity;         // how much the power scales per unit the mouse is dragged
    [SerializeField] float maxPower;
    [SerializeField] List<Material> materials;  // stores all valid materials

    Trajectory trajPredictor;
    Camera cam;

    Rigidbody2D rb;

    bool isLaunched;
    private Vector2 trajectory;
    //bool isDragged;
    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        rb = GetComponent<Rigidbody2D>();
        isLaunched = false;
        trajectory = new();

        trajPredictor = GetComponent<Trajectory>();
    }

    void Update()
    {
        // only launch once
        if (!isLaunched)
        {
            rb.gravityScale = 0;

            // detect when mouse is held down
            if (Input.GetButton("Fire1"))
            {
                // prepare a trajectory
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
    }


    public IEnumerator Yay()
    {
        rb.linearVelocity = new();
        yield return new WaitForSeconds(1.5f);
        Explode();
    }
    // Get rid of the ball
    private void Explode()
    {
        // particle effects

        // make the ball dissapear
        Destroy(gameObject);
    }

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
}
