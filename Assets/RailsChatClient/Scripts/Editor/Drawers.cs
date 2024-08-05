using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace RailsChat
{
    [CustomPropertyDrawer(typeof(ChannelSO))]
    public class ChannelSOPropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var channelSO = property.objectReferenceValue as ChannelSO;

            EditorGUI.BeginProperty(position, label, property);

            float singleLineHeight = EditorGUIUtility.singleLineHeight;
            Rect fieldRect = new Rect(position.x, position.y, position.width, singleLineHeight);

            if (channelSO != null)
            {
                GUI.enabled = true;
                channelSO.ChannelName = EditorGUI.TextField(fieldRect, "Name: ",channelSO.ChannelName);
                fieldRect.y += singleLineHeight;
                GUI.enabled = false;
                EditorGUI.EnumPopup(fieldRect, "Status: ", channelSO.Status);
                GUI.enabled = true;
            }
            else
            {
                GUILayout.BeginHorizontal();
                EditorGUILayout.LabelField(property.displayName);
                EditorGUILayout.ObjectField(property, typeof(ChannelSO), GUIContent.none);
                GUILayout.EndHorizontal();

            }

            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return property.objectReferenceValue == null ? EditorGUIUtility.singleLineHeight * 2 : EditorGUIUtility.singleLineHeight * 2;
        }
    }

    [CustomPropertyDrawer(typeof(System.Collections.Generic.List<ChannelSO>))]
    public class GenericListPropertyDrawer : PropertyDrawer
    {
        private ReorderableList reorderableList;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            // Initialize the ReorderableList if not done yet
            if (reorderableList == null)
            {
                // Find the serialized property representing the list
                SerializedProperty listProperty = property.FindPropertyRelative("Array");
                reorderableList = new ReorderableList(property.serializedObject, listProperty, true, true, true, true);

                reorderableList.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
                {
                    SerializedProperty element = reorderableList.serializedProperty.GetArrayElementAtIndex(index);
                    EditorGUI.PropertyField(rect, element, GUIContent.none);
                };

                reorderableList.drawHeaderCallback = (Rect rect) =>
                {
                    EditorGUI.LabelField(rect, property.displayName);
                };

                reorderableList.onAddCallback = (ReorderableList list) =>
                {
                    list.serializedProperty.InsertArrayElementAtIndex(list.serializedProperty.arraySize);
                    SerializedProperty newElement = list.serializedProperty.GetArrayElementAtIndex(list.serializedProperty.arraySize - 1);
                    newElement.objectReferenceValue = null;
                    property.serializedObject.ApplyModifiedProperties();
                };

                reorderableList.onRemoveCallback = (ReorderableList list) =>
                {
                    list.serializedProperty.DeleteArrayElementAtIndex(list.index);
                    property.serializedObject.ApplyModifiedProperties();
                };
            }

            // Draw the ReorderableList
            reorderableList.DoList(position);
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            // Calculate the height of the ReorderableList
            if (reorderableList != null)
            {
                float elementHeight = reorderableList.elementHeight;
                return elementHeight * reorderableList.serializedProperty.arraySize + reorderableList.headerHeight;
            }

            return base.GetPropertyHeight(property, label);
        }
    }
}