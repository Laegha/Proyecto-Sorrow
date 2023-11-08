using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CrossReferenceHelper : MonoBehaviour
{
    public class ReferenceArguments
    {
        public string objectName;
        public string componentName;
        public string componentSubclass;
        public string lookingFor;
    }

    [Serializable]
    public class FunctionReferenceArguments : ReferenceArguments
    {
        public string[] arguments;
        public string[] argumentTypes;
        public object[] convertedArguments;
    }

    [Serializable]
    public class FieldReferenceArguments : ReferenceArguments
    {
        public string fieldType;
        public string newValue;
        public object convertedValue;
    }

    [SerializeField] FunctionReferenceArguments[] functionRA;
    [SerializeField] FieldReferenceArguments[] fieldRA;

    void Awake()
    {
        foreach (var r in functionRA)
            foreach (var (a, t) in r.arguments.Zip(r.argumentTypes, (a, t) => (a, t)))
                r.convertedArguments.Append(Convert.ChangeType(a, Type.GetType(t)));

        foreach (var r in fieldRA)
            r.convertedValue = Convert.ChangeType(r.newValue, Type.GetType(r.fieldType));
    }

    public void CallFunction(int index)
    {
        var component = ComponentGetter(functionRA[index]);
        var type = TypeGetter(component, fieldRA[index]);
        type.GetMethod(functionRA[index].lookingFor).Invoke(component, functionRA[index].convertedArguments);
    }

    public void ModifyFieldValue(int index)
    {
        var component = ComponentGetter(fieldRA[index]);
        var type = TypeGetter(component, fieldRA[index]);
        type.GetField(fieldRA[index].lookingFor).SetValue(component, fieldRA[index].convertedValue);
    }

    Component ComponentGetter(ReferenceArguments ra)
        => GameObject.Find(ra.objectName).GetComponent(ra.componentName);

    Type TypeGetter(Component component, ReferenceArguments ra)
    {
        if (ra.componentSubclass != string.Empty && ra.componentSubclass != null)
            return Type.GetType(ra.componentSubclass);
        
        return component.GetType();
    }
}
