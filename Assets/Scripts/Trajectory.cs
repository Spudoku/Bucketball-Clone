using System.Collections.Generic;
using UnityEngine;

public class Trajectory : MonoBehaviour
{

    [SerializeField] GameObject pointer;    // object that shows a point in trajectory
    [SerializeField] int steps;             // number of objects shown. Should be relatively low (~10-15)
    [SerializeField] float interval;        // time interval between steps, in seconds

    private List<GameObject> pointers = new List<GameObject>();

    public bool isVisible;

    void Start()
    {
        isVisible = false;

        float scale = 0.5f;

        // create pointers
        for (int i = 0; i < steps; i++)
        {
            GameObject newPointer = Instantiate(pointer);
            pointer.transform.localScale = new(scale, scale);
            scale *= 0.9f;
            pointers.Add(newPointer);
        }
    }

    void Update()
    {
        foreach (GameObject p in pointers)
        {
            p.SetActive(isVisible);
        }
    }


    // calculate predicted trajectory given an initial starting speed
    public void CalcTrajectory(Vector3 traj)
    {
        // V0 = traj
        float x = traj.x;
        float y = traj.y;
        float speed = traj.magnitude;
        float theta = Mathf.Atan2(y, x);

        float t = 0.0001f;
        for (int i = 0; i < pointers.Count; i++)
        {
            GameObject p = pointers[i];
            float px = speed * Mathf.Cos(theta) * t + transform.position.x;
            float py = speed * Mathf.Sin(theta) * t + 0.5f * Physics.gravity.y * t * t;

            p.transform.position = new(px, py);

            t += interval;


        }
    }


}
