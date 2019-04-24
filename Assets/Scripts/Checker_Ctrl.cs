using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checker_Ctrl : MonoBehaviour
{
    public Transform controller_trans;
    bool following = true;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (following)
        {
            transform.position = controller_trans.position;
            Vector3 eulerRotation = new Vector3(controller_trans.transform.eulerAngles.x, controller_trans.transform.eulerAngles.y, controller_trans.transform.eulerAngles.z);

            transform.rotation = Quaternion.Euler(eulerRotation);
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            toggle_follow();
        }
    }
    void toggle_follow()
    {
        following = !following;
    }
}
