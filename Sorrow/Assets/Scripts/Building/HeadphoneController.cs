using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeadphoneController : MonoBehaviour
{
    public static List<RithmInteractable> rithmInteractables = new List<RithmInteractable>();
    [SerializeField] Image headphonesFilter;
    bool headphonesOn = false;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
            SwitchState();
    }
    void SwitchState()
    {
        headphonesOn = !headphonesOn;
        headphonesFilter.gameObject.SetActive(headphonesOn);
        foreach (RithmInteractable rithmInteractable in rithmInteractables)
            rithmInteractable.SwitchCurrState(headphonesOn);
    }
}
