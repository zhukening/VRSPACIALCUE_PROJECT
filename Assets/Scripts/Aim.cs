using UnityEngine;
using System.Collections;
/* interaction class for gaze triggered objects */


public class Aim : MonoBehaviour
{
    public GameObject obj;
    private GameObject Player;
    public VRStandardAssets.Utils.VRInteractiveItem m_InteractiveItem;

    private Renderer rend;
    private float timer=0;

    void Start()
    {
        rend = GetComponent<Renderer>();
        Player = GameObject.Find("Player");
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
        Debug.Log("gaze entered");
    }

    public void OnGazeExit()
    {
        rend.material.color = Color.white;
    }

    public void OnGazeTrigger()
    {
        GameObject newTarget = (GameObject) Instantiate(obj, new Vector3(Random.Range(-5, 5), Random.Range(0, 4), Random.Range(-5, 5)), Quaternion.identity);  // needs to be replaced with a preprogrammed table per participant
        Debug.Log(timer); 
        GameObject.Find("manager").GetComponent<data_write>().write_time(timer);
        Player.GetComponent<FeedbackMain>().target = newTarget; // update the targeting system with new target
        Destroy(gameObject);
    }
}