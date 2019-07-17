using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectStayOnMousePosition : MonoBehaviour
{

    void Update()
    {

        Vector3 mouse = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0f);
        mouse = Camera.main.ScreenToWorldPoint(mouse);

        this.transform.position = new Vector3(mouse.x, mouse.y, mouse.z);
    }
}
