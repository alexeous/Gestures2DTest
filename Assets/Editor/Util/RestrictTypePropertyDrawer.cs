using UnityEditor;
using UnityEngine;
using Util;

namespace Editor.Util
{
    [CustomPropertyDrawer(typeof(RestrictType))]
    public class RestrictTypePropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var desiredType = ((RestrictType)attribute).Type;

            if (property.isArray)
            {
                var arrayType = fieldInfo.FieldType.GetGenericTypeDefinition().MakeGenericType(desiredType);

                EditorGUI.ObjectField(position, property, arrayType, label);
                return;
            }

            EditorGUI.ObjectField(position, property, desiredType, label);
        }
    }
}