using UnityEngine;
using System.Collections;
/* interaction class for gaze triggered objects */


public class Aim : MonoBehaviour
{
    public VRStandardAssets.Utils.VRInteractiveItem m_InteractiveItem;
    public int GazeTriggerTime = 1;

    private Renderer rend;
    private float timer=0;

    private GameObject Manager;
    bool staring; // to trigger on stare

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
        StartCoroutine(Stare()); // trigger the gaze timer
    }

    public void OnGazeExit()
    {
        rend.material.color = Color.white;
        staring = false;
    }

    public void OnGazeTrigger()
    {
        rend.material.color = Color.white; // fixes bug where new cube appears red

        // update Target manager that we have been hit
        GameObject.Find("Manager").GetComponent <TargetManager>().SpawnNextTarget(timer);

        Destroy(gameObject); // self destruct
    }

    IEnumerator Stare()
    {
        staring = true;

        yield return new WaitForSeconds(GazeTriggerTime);
        // are we still staring after the timer
        if (staring)
        {
            OnGazeTrigger();
        }

    }
}