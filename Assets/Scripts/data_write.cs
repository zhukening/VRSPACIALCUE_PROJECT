using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class data_write : MonoBehaviour {

    private int num = 0;
    private int num2 = 0;
    private string all_name = null;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (num2 < num)
        {
            all_name = null;
            for (int i = 1; i <= num; i++)
            {
                string get_name = "data" + i;
                all_name = all_name + "DATA"+i+"------"+PlayerPrefs.GetFloat(get_name) + "\n";
                PlayerPrefs.SetString("all_data", all_name);          
            }
            num2 += 1;
        }
  	
	}

    public void write_time(float timer)
    {
        num += 1;
        string set_name = "data" + num;
        PlayerPrefs.SetFloat(set_name, timer);
    }
}
