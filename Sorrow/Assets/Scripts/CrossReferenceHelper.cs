using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CrossReferenceHelper : MonoBehaviour
{
    [Serializable]
    public struct ReferenceArguments
    {
        public string objectName;
        public string componentName;
        public string functionName;
        public string[] arguments;
        public string[] argumentTypes;
        public object[] convertedArguments;
    }

    [SerializeField] ReferenceArguments[] referenceArguments;

    void Awake()
    {
        foreach (var r in referenceArguments)
            foreach (var (a, t) in r.arguments.Zip(r.argumentTypes, (a, t) => (a, t)))
                r.convertedArguments.Append(Convert.ChangeType(a, Type.GetType(t)));
    }

    public void Call(int index)
    {
        var component = GameObject.Find(referenceArguments[index].objectName).GetComponent(referenceArguments[index].componentName);
        component.GetType().GetMethod(referenceArguments[index].functionName).Invoke(component, referenceArguments[index].convertedArguments);
    }
}
