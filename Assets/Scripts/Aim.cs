using UnityEngine;
using System.Collections;

public class Aim : MonoBehaviour
{   
    public GameObject obj;

    private Renderer rend;
    private float timer=0;
    void Start()
    {
        rend = GetComponent<Renderer>();
    }

    void Update()
    {
        timer += Time.deltaTime;
    }
    public void OnGazeEnter()
    {
        rend.material.color = Color.red;
    }

    public void OnGazeExit()
    {
        rend.material.color = Color.white;
    }

    public void OnGazeTrigger()
    {
        Instantiate(obj, new Vector3(Random.Range(-5, 5), Random.Range(0, 4), Random.Range(-5, 5)), Quaternion.identity);
        Debug.Log(timer);
        GameObject.Find("manager").GetComponent<data_write>().write_time(timer);
        Destroy(gameObject);
    }
}