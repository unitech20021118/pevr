using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LightSetting : MonoBehaviour
{
    AddLightReources AddedLight;
    public GameObject Slide1, Slide2;
    AddLightInforma informe;
    public GameObject StatePanel;
	// Use this for initialization
	void Start () {
        Slide1.GetComponent<Slider>().value = 3;
        Slide2.GetComponent<Slider>().value = 1f;
        
	}
	
    void Update()
    {
        Slide1.transform.FindChild("Text1").GetComponent<Text>().text = Slide1.GetComponent<Slider>().value.ToString();
        Slide2.transform.FindChild("Text2").GetComponent<Text>().text = Slide2.GetComponent<Slider>().value.ToString();

        ISstateLight();
	}
    public void SetCurrentLight(AddLightReources d, ActionInforma info)
    {
        AddedLight= d;
        AddedLight.z = Slide1.GetComponent<Slider>().value;
        AddedLight.x = Slide2.GetComponent<Slider>().value;
        informe = (AddLightInforma)info;
        informe.z = Slide1.GetComponent<Slider>().value.ToString();
        informe.x = Slide2.GetComponent<Slider>().value.ToString();
    }
    /// <summary>
    /// to close lightsetting(!state)
    /// </summary>
    public void ISstateLight()
    {

    }
}
