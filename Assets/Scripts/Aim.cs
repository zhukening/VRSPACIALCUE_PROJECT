using UnityEngine;
using System.Collections;
/* interaction class for gaze triggered objects */


public class Aim : MonoBehaviour
{
    public VRStandardAssets.Utils.VRInteractiveItem m_InteractiveItem;

    private Renderer rend;
    private float timer=0;

    private GameObject Manager;

    void Start()
    {
        rend = GetComponent<Renderer>();
    }

    void Update()
    {
        timer += Time.deltaTime;
    }

    private void OnEnable()
    {
        m_InteractiveItem.OnOver += OnGazeEnter;
        m_InteractiveItem.OnOut += OnGazeExit;
        m_InteractiveItem.OnClick += OnGazeTrigger;
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
        rend.material.color = Color.white; // fixes bug where new cube appears red

        // update Target manager that we have been hit
        GameObject.Find("Manager").GetComponent <TargetManager>().SpawnNextTarget(timer);

        // CODE Moved to TargetManager.cs
        //GameObject newTarget = (GameObject) Instantiate(obj, new Vector3(Random.Range(-5, 5), Random.Range(0, 4), Random.Range(-5, 5)), Quaternion.identity);  // needs to be replaced with a preprogrammed table per participant
        //Debug.Log(timer); // print out time taken to find box
        //GameObject.Find("manager").GetComponent<data_write>().write_time(timer); // writes the time taken to file - to be replaced by array update

        Destroy(gameObject); // self destruct
    }
}