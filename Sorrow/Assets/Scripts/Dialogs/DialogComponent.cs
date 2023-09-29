using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

/*
#if UNITY_EDITOR
using UnityEditor;
#endif
*/

[Serializable]
public class DialogComponent
{
    [Serializable]
    public enum DialogComponentType
    {
        Dialog,
        Timeline,
        Divider
    }

    public DialogComponentType type;
    public int from;
    public int to;
    public PlayableAsset timeline;

/*
#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(DialogComponent))]
    public class DialogComponentEditor : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);
            property.FindPropertyRelative("type").enumValueIndex = (int)(DialogComponentType)EditorGUI.EnumPopup(new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight), (DialogComponentType)property.FindPropertyRelative("type").enumValueIndex);
            switch ((DialogComponentType)property.FindPropertyRelative("type").enumValueIndex)
            {
                case DialogComponentType.Dialog:
                    property.FindPropertyRelative("from").intValue = EditorGUI.IntField(new Rect(position.x, position.y + EditorGUIUtility.singleLineHeight, position.width, EditorGUIUtility.singleLineHeight), "From", property.FindPropertyRelative("from").intValue);
                    property.FindPropertyRelative("to").intValue = EditorGUI.IntField(new Rect(position.x, position.y + EditorGUIUtility.singleLineHeight * 2, position.width, EditorGUIUtility.singleLineHeight), "To", property.FindPropertyRelative("to").intValue);
                    break;
                case DialogComponentType.Timeline:
                    property.FindPropertyRelative("timeline").objectReferenceValue = EditorGUI.ObjectField(new Rect(position.x, position.y + EditorGUIUtility.singleLineHeight, position.width, EditorGUIUtility.singleLineHeight), "Timeline", property.FindPropertyRelative("timeline").objectReferenceValue, typeof(PlayableAsset), false);
                    break;
                case DialogComponentType.Divider:
                    break;
            }
            // Adjust height for the next property
            position.min = new Vector2(position.x, position.y + EditorGUIUtility.singleLineHeight * 3);
        }
    }
#endif
*/
}
