using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateAroundMine : MonoBehaviour
{
    [Tooltip("Object to be rotated")]
    public GameObject objectRotated = null;
    [Tooltip("Object to rotate around")]
    public GameObject objectAround = null;
    [Tooltip("Rotation speed")]
    public float rotationSpeed = 0;

    // Radius of the circle whose center is objectAround
    private float radius = 0;

    // Coordinates of objectAround
    private float xAround = 0;
    private float yAround = 0;
    private float zAround = 0;

    // Coordinates of objectRotated
    private float xRotated = 0;
    private float yRotated = 0;
    private float zRotated = 0;

    // Variable to control whether the data has been set
    // private bool dataSet = false;

    void SetData()
    {
        radius = Vector2.Distance(objectRotated.transform.position, objectAround.transform.position);

        xAround = objectAround.transform.position.x;
        yAround = objectAround.transform.position.y;
        zAround = objectAround.transform.position.z;

        xRotated = objectRotated.transform.position.x;
        yRotated = objectRotated.transform.position.y;
        zRotated = objectRotated.transform.position.z;
    }

    void Rotate()
    {

        // Top of the coord system
        if (yRotated >= yAround || xRotated == xAround - radius)
        {
            if (xRotated <= xAround + radius)
            {
                xRotated += rotationSpeed;

                if (Mathf.Pow(radius, 2.0f) > Mathf.Pow(xRotated - xAround, 2.0f))
                {
                    yRotated = yAround + Mathf.Sqrt(Mathf.Pow(radius, 2.0f) - Mathf.Pow((xRotated - xAround), 2.0f));
                }
                else
                {
                    yRotated = yAround;
                }

                objectRotated.transform.position = new Vector3(xRotated, yRotated, zRotated);
            }
            else if (xRotated > xAround + radius)
            {
                xRotated = xAround + radius;
                yRotated = yAround + Mathf.Sqrt(Mathf.Pow(radius, 2.0f) - Mathf.Pow((xRotated - xAround), 2.0f));

                objectRotated.transform.position = new Vector3(xRotated, yRotated, zRotated);
            }
        }
        // Bottom of the coord system
        else if (yRotated < yAround || xRotated == xAround + radius)
        {
            if (xRotated >= xAround - radius)
            {
                xRotated -= rotationSpeed;

                if (Mathf.Pow(radius, 2.0f) > Mathf.Pow(xRotated - xAround, 2.0f))
                {
                    yRotated = yAround + Mathf.Sqrt(Mathf.Pow(radius, 2.0f) - Mathf.Pow((xRotated - xAround), 2.0f));
                }
                else
                {
                    yRotated = yAround;
                }

                objectRotated.transform.position = new Vector3(xRotated, yRotated, zRotated);
            }
            else if (xRotated < xAround - radius)
            {
                xRotated = xAround - radius;
                yRotated = yAround - Mathf.Sqrt(Mathf.Pow(radius, 2.0f) - Mathf.Pow((xRotated - xAround), 2.0f));

                objectRotated.transform.position = new Vector3(xRotated, yRotated, zRotated);
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        SetData();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(radius);
        // Debug.Log(xAround + "," + yAround + "," + zAround);
        Debug.Log(xRotated + "," + yRotated + "," + zRotated);

        Rotate();
    }
}
