using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SensitivitySlider : MonoBehaviour
{
	Slider slider;
	public Cam cam;
    // Start is called before the first frame update
    void Start()
    {
		slider = GetComponent<Slider>();
    }

    // Update is called once per frame
    void Update()
    {
		cam.ChangeSense(slider.value);
    }
}
