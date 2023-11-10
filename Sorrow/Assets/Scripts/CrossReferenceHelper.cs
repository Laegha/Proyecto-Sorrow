using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

public class CrossReferenceHelper : MonoBehaviour
{
    public class ReferenceArguments
    {
        public string objectName;
        public string componentName;
        public string componentSubclassNamespace;
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
        public bool isProperty;
        public string newValue;
        public string fieldType;
        public object convertedValue;
    }

    [SerializeField] FunctionReferenceArguments[] functionRA;
    [SerializeField] FieldReferenceArguments[] fieldRA;

    void Awake()
    {
        foreach (var r in functionRA)
        {
            r.convertedArguments = new object[r.arguments.Length];
            foreach (var (a, t) in r.arguments.Zip(r.argumentTypes, (a, t) => (a, t)))
                r.convertedArguments.Append(Convert.ChangeType(a, Type.GetType(t)));
        }

        foreach (var r in fieldRA)
            r.convertedValue = Convert.ChangeType(r.newValue, Type.GetType(r.fieldType));
    }

    public void CallFunction(int index)
    {
        var component = ComponentGetter(functionRA[index]);
        var type = TypeGetter(component, functionRA[index]);
        type.GetMethod(functionRA[index].lookingFor).Invoke(component, functionRA[index].convertedArguments);
    }

    public void ModifyFieldValue(int index)
    {
        var component = ComponentGetter(fieldRA[index]);
        var type = TypeGetter(component, fieldRA[index]);
        print(component);
        print(type);
        print(fieldRA[index].lookingFor);
        print(fieldRA[index].convertedValue);
        if (fieldRA[index].isProperty)
        {
            var property = type.GetProperty(fieldRA[index].lookingFor);
            property.SetValue(component, fieldRA[index].convertedValue);
            return;
        }
        
        var field = type.GetField(fieldRA[index].lookingFor);
        field.SetValue(component, fieldRA[index].convertedValue);
    }

    Component ComponentGetter(ReferenceArguments ra)
        => GameObject.Find(ra.objectName).GetComponent(ra.componentName);

    Type TypeGetter(Component component, ReferenceArguments ra)
    {
        if (ra.componentSubclass != string.Empty && ra.componentSubclass != null)
            return Assembly.Load(ra.componentSubclassNamespace).GetType(ra.componentSubclass);
        
        return component.GetType();
    }
}
