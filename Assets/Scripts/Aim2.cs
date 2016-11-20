using UnityEngine;
using System.Collections;

public class Aim2 : MonoBehaviour
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
        int a, c;
        a = 0;
        while (a == 0)
        {
            a = Random.Range(-6, 6);
        }
        c = 0;
        while (c == 0)
        {
            c = Random.Range(-6, 6);
        }
        Instantiate(obj, new Vector3(a, 2, c), Quaternion.identity);

        Debug.Log(timer);
        GameObject.Find("manager").GetComponent<data_write>().write_time(timer);
        Destroy(gameObject);
		rend.material.color = Color.white;

    }
}