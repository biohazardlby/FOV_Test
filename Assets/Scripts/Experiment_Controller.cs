using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Experiment_Controller : MonoBehaviour
{
    public GameObject camera;
    [Header("Checkerboard")]
    public bool activate_checkerboard;
    public GameObject checkerboard_prefab;
    public GameObject controller;
    [Header("FOV")]
    public bool activate_FOV;
    public GameObject half_Sphere_prefab;

    GameObject half_Sphere_Right;
    GameObject half_Sphere_Left;
    float rotate_rate = 10;
    float slow_rate = 2.5f;
    
    void Start()
    {
        if (activate_checkerboard)
        {
            GameObject CB_GO = GameObject.Instantiate(checkerboard_prefab);
            Checker_Ctrl CB_Mono = CB_GO.GetComponent<Checker_Ctrl>();
            CB_Mono.controller_trans = controller.transform;
        }
        if (activate_FOV)
        {
            half_Sphere_Right = GameObject.Instantiate(half_Sphere_prefab,camera.transform);
            half_Sphere_Left = GameObject.Instantiate(half_Sphere_prefab,camera.transform);
            half_Sphere_Left.transform.Rotate(0, 180, 0);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.A))
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                half_Sphere_Left.transform.Rotate(0, -Time.deltaTime * slow_rate, 0);
            }
            else
            {
                half_Sphere_Left.transform.Rotate(0, -Time.deltaTime * rotate_rate, 0);
            }
        }
        if (Input.GetKey(KeyCode.D))
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                half_Sphere_Left.transform.Rotate(0, Time.deltaTime * slow_rate, 0);
            }
            else
            {
                half_Sphere_Left.transform.Rotate(0, Time.deltaTime * rotate_rate, 0);
            }
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                half_Sphere_Right.transform.Rotate(0, -Time.deltaTime * slow_rate, 0);
            }
            else
            {
                half_Sphere_Right.transform.Rotate(0, -Time.deltaTime * rotate_rate, 0);
            }
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                half_Sphere_Right.transform.Rotate(0, Time.deltaTime * slow_rate, 0);
            }
            else
            {
                half_Sphere_Right.transform.Rotate(0, Time.deltaTime * rotate_rate, 0);
            }
        }
        if (Input.GetKeyDown(KeyCode.F)){
            flash_sphere();
        }
    }
    void flash_sphere()
    {
        Renderer rend1 = half_Sphere_Left.GetComponent<Renderer>();
        Renderer rend2 = half_Sphere_Right.GetComponent<Renderer>();
        wait_n_changeColor(0.5f, 4, rend1);
        wait_n_changeColor(0.5f, 4, rend2);
    }
    void wait_n_changeColor(float t, int times, Renderer rend)
    {
        StartCoroutine(wait_n_changeColor_Helper(t, 0, times, false, rend));
    }
    IEnumerator wait_n_changeColor_Helper(float t, int cur, int tar, bool black, Renderer rend)
    {
        if (black)
        {
            rend.material.SetColor("_Color", new Color(0, 0, 0));
        }
        else
        {
            rend.material.SetColor("_Color", new Color(1, 0, 0));
        }
        if (cur == tar)
        {
            rend.material.SetColor("_Color", new Color(1, 1, 1));
            yield break;
        }
        yield return new WaitForSeconds(t);
        StartCoroutine(wait_n_changeColor_Helper(t, cur+1, tar, !black, rend));
    }
}
