using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SolutionQ2 : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Solution start");
        var spherePosition = new Vector3(1f, 1f, 1f);
        
        var A = new Vector3(-3f, -3f, -7f);
        var B = new Vector3(3f, -3f, -1f);
        var C = new Vector3(-3f, 3f, -1f);
        var trianglePosition = (A + B + C) / 3;
        Debug.Log(trianglePosition);

        var contactNormal = (spherePosition - trianglePosition).normalized;
        Debug.Log(contactNormal);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
