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
            int x, z;
             x = 0;
             while (x == 0)
                x = Random.Range(-6, 6);
             z = 0;
             while (z == 0)
                z = Random.Range(-6, 6);
            Destroy(hit.collider.gameObject);
                isDelect = false;
                Instantiate(obj, new Vector3(x, 2, z), Quaternion.identity);
        }
    }
}