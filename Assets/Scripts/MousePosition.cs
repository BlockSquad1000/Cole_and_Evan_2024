using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MousePosition : MonoBehaviour
{
    Transform mouseTransform;
    public Vector3 mousePos;

    // Start is called before the first frame update
    void Start()
    {
        mouseTransform = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        mousePos = Input.mousePosition;

        Ray cameToNavRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;


    }
}

