using UnityEngine;
using System.Collections;

public class Aim : MonoBehaviour
{   
	public GameObject obj, left, right;
    private Renderer rend;
    private float timer=0;
    void Start()
    {
        rend = GetComponent<Renderer>();

		left = GameObject.Find ("Left_Arrow");
		right = GameObject.Find ("Right_Arrow");
		float x, y, z, sum1, sum2;
		x = left.transform.position.x - obj.transform.position.x;
		y = left.transform.position.y - obj.transform.position.y;
		z = left.transform.position.z - obj.transform.position.z;
		sum1 = x * x + y * y + z * z;
		x = right.transform.position.x - obj.transform.position.x;
		y = right.transform.position.y - obj.transform.position.y;
		z = right.transform.position.z - obj.transform.position.z;
		sum2 = x * x + y * y + z * z;
		if (sum1 > sum2) {
			left.SetActive (false); 
		} else {
			right.SetActive (false);
		}

    }

    void Update()
    {
        timer += Time.deltaTime;
    }
    public void OnGazeEnter()
    {
        rend.material.color = Color.red;
		left.SetActive(false); 
		right.SetActive(false); 
    }

    public void OnGazeExit()
    {
        rend.material.color = Color.white;


		float x, y, z, sum1, sum2;
		x = left.transform.position.x - obj.transform.position.x;
		y = left.transform.position.y - obj.transform.position.y;
		z = left.transform.position.z - obj.transform.position.z;
		sum1 = x * x + y * y + z * z;
		x = right.transform.position.x - obj.transform.position.x;
		y = right.transform.position.y - obj.transform.position.y;
		z = right.transform.position.z - obj.transform.position.z;
		sum2 = x * x + y * y + z * z;
		if (sum1 < sum2) {
			left.SetActive (true); 
		} else {
			right.SetActive(true);
		}
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

		left.SetActive (true);
		right.SetActive(true);
    }
}