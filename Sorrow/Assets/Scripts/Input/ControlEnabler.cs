using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlEnabler : MonoBehaviour
{
    public void RemRegControls(bool enablement) => InputManager.instance.RemRegControl(enablement);
}
