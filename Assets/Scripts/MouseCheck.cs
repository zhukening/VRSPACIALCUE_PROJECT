using UnityEngine;
using System.Collections;

public class MouseCheck : MonoBehaviour
{
    RaycastHit hit;
    bool isDelect = false;
    public GameObject obj;

    void Start()
    {

    }

    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                isDelect = true;
            }
        }
    }

    void OnGUI()
    {
        if (isDelect)
        {          
                Destroy(hit.collider.gameObject);
                isDelect = false;
                Instantiate(obj, new Vector3(Random.Range(-5, 5), Random.Range(0, 4), Random.Range(-5, 5)), Quaternion.identity);
        }
    }
}