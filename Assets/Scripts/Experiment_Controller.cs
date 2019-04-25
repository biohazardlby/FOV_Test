using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Experiment_Controller : MonoBehaviour
{
    public GameObject camera;
    [Header("FOV")]
    public GameObject half_Sphere_prefab;
    [Header("Output")]
    public string initials = "";
    GameObject half_Sphere_Right_GO;
    GameObject half_Sphere_Left_GO;
    GameObject sphere_roll_GO;
    float rotate_rate = 10;
    float slow_rate = 2.5f;
    float roll_rate = 5f;
    float slow_roll_rate = 2.5f;
    StreamWriter writer;

    void Start()
    {
        sphere_roll_GO = new GameObject("sphere_roll_GO");
        sphere_roll_GO.transform.SetParent(camera.transform);
        half_Sphere_Right_GO = GameObject.Instantiate(half_Sphere_prefab, sphere_roll_GO.transform);
        half_Sphere_Left_GO = GameObject.Instantiate(half_Sphere_prefab, sphere_roll_GO.transform);
        half_Sphere_Left_GO.transform.Rotate(0, 180, 0);
        string file_name = initials + "_" + System.DateTime.Now.ToString("HH-mm") + ".txt";
        string path = Path.Combine(Application.dataPath, "data");
        Debug.Log(path);
        Directory.CreateDirectory(path);
        writer = File.CreateText(Path.Combine(path, file_name));
        writer.Write("Roll_Degree" +
            "\t" + "Left_Degree" +
            "\t" + "Right_Degree" + "\n");
    }

    // Update is called once per frame
    void Update()
    {
        bool hold_shift = false;
        if (Input.GetKey(KeyCode.LeftShift))
        {
            hold_shift = true;
        }
        if (hold_shift)
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                Vector3 cur_rot = sphere_roll_GO.transform.rotation.eulerAngles;
                if (cur_rot.z % 15 == 0)
                {
                    cur_rot.z += 15;
                }
                else
                {
                    if (cur_rot.z > 0)
                    {
                        cur_rot.z = Mathf.Ceil((cur_rot.z + 1) / 15.0f) * 15;
                    }
                    else if (cur_rot.z < 0)
                    {
                        cur_rot.z = Mathf.Floor((cur_rot.z-1) / 15.0f) * 15;
                    }
                }
                sphere_roll_GO.transform.rotation = Quaternion.Euler(cur_rot);
                hold_shift = true;
            } else if (Input.GetKeyDown(KeyCode.E))
            {
                Vector3 cur_rot = sphere_roll_GO.transform.rotation.eulerAngles;
                if (cur_rot.z % 15 == 0)
                {
                    cur_rot.z -= 15;
                }
                else
                {
                    if (cur_rot.z > 0)
                    {
                        cur_rot.z = Mathf.Floor((cur_rot.z-1) / 15.0f) * 15;
                    }
                    else if (cur_rot.z < 0)
                    {
                        cur_rot.z = Mathf.Ceil((cur_rot.z+1) / 15.0f) * 15;
                    }
                }
                sphere_roll_GO.transform.rotation = Quaternion.Euler(cur_rot);
                hold_shift = true;
            }
        }
        else
        {
            if (Input.GetKey(KeyCode.Q))
            {
                if (Input.GetKey(KeyCode.LeftControl))
                {
                    sphere_roll_GO.transform.Rotate(0, 0, Time.deltaTime * slow_roll_rate);
                }
                else
                {
                    sphere_roll_GO.transform.Rotate(0, 0, Time.deltaTime * roll_rate);
                }
            }
            else if (Input.GetKey(KeyCode.E))
            {
                if (Input.GetKey(KeyCode.LeftControl))
                {
                    sphere_roll_GO.transform.Rotate(0, 0, -Time.deltaTime * slow_roll_rate);
                }
                else
                {
                    sphere_roll_GO.transform.Rotate(0, 0, -Time.deltaTime * roll_rate);
                }
            }
        }

        if (Input.GetKey(KeyCode.A))
        {
            if (Input.GetKey(KeyCode.LeftControl))
            {
                half_Sphere_Left_GO.transform.Rotate(0, -Time.deltaTime * slow_rate, 0);
            }
            else
            {
                half_Sphere_Left_GO.transform.Rotate(0, -Time.deltaTime * rotate_rate, 0);
            }
        }
        if (Input.GetKey(KeyCode.D))
        {
            if (Input.GetKey(KeyCode.LeftControl))
            {
                half_Sphere_Left_GO.transform.Rotate(0, Time.deltaTime * slow_rate, 0);
            }
            else
            {
                half_Sphere_Left_GO.transform.Rotate(0, Time.deltaTime * rotate_rate, 0);
            }
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            if (Input.GetKey(KeyCode.LeftControl))
            {
                half_Sphere_Right_GO.transform.Rotate(0, -Time.deltaTime * slow_rate, 0);
            }
            else
            {
                half_Sphere_Right_GO.transform.Rotate(0, -Time.deltaTime * rotate_rate, 0);
            }
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            if (Input.GetKey(KeyCode.LeftControl))
            {
                half_Sphere_Right_GO.transform.Rotate(0, Time.deltaTime * slow_rate, 0);
            }
            else
            {
                half_Sphere_Right_GO.transform.Rotate(0, Time.deltaTime * rotate_rate, 0);
            }
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            flash_sphere();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            writer.Write(sphere_roll_GO.transform.localRotation.eulerAngles.z + "\t"
                    + (180 -half_Sphere_Left_GO.transform.localRotation.eulerAngles.y) + "\t"
                    + half_Sphere_Right_GO.transform.localRotation.eulerAngles.y + "\n"
                );
        }
    }
    void flash_sphere()
    {
        Renderer rend1 = half_Sphere_Left_GO.GetComponent<Renderer>();
        Renderer rend2 = half_Sphere_Right_GO.GetComponent<Renderer>();
        wait_n_changeColor(0.25f, 2, rend1);
        wait_n_changeColor(0.25f, 2, rend2);
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
        StartCoroutine(wait_n_changeColor_Helper(t, cur + 1, tar, !black, rend));
    }
    private void OnApplicationQuit()
    {
        writer.Close();
    }
}
