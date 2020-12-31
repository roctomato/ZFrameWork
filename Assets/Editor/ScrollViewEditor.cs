using UnityEngine;
using UnityEngine.UI;
using UnityEditor.AnimatedValues;
using LR;

namespace UnityEditor.UI
{
    [CustomEditor(typeof(ScrollView), true)]
    [CanEditMultipleObjects]
    public class ScrollViewEditor : ScrollRectEditor
    {
        SerializedProperty padding;
        SerializedProperty cellTemplate;
        SerializedProperty maxWidth;
        SerializedProperty cellWidth;
        SerializedProperty cellHeight;
        SerializedProperty maxCreateCountPerFrame;
        SerializedProperty disableDragIfFits;
        SerializedProperty isResizeCell;
        SerializedProperty notDataObject;

        string m_StrInvalidWarning;

        protected override void OnEnable()
        {
            base.OnEnable();
            padding = serializedObject.FindProperty("padding");
            cellTemplate = serializedObject.FindProperty("cellTemplate");
            maxWidth = serializedObject.FindProperty("maxWidth");
            cellWidth = serializedObject.FindProperty("cellWidth");
            cellHeight = serializedObject.FindProperty("cellHeight");
            maxCreateCountPerFrame = serializedObject.FindProperty("maxCreateCountPerFrame");
            disableDragIfFits = serializedObject.FindProperty("disableDragIfFits");
            isResizeCell = serializedObject.FindProperty("isResizeCell");
            notDataObject = serializedObject.FindProperty("notDataObject");
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            ScrollViewPropertyGUI();
            ScrollViewShowGUI();
            serializedObject.ApplyModifiedProperties();
        }

        void ScrollViewPropertyGUI()
        {
            EditorGUILayout.BeginVertical();
            EditorGUILayout.PropertyField(padding, true);
            EditorGUILayout.PropertyField(cellTemplate);
            EditorGUILayout.PropertyField(maxWidth);
            EditorGUILayout.PropertyField(cellWidth);
            EditorGUILayout.PropertyField(cellHeight);
            EditorGUILayout.PropertyField(maxCreateCountPerFrame);
            EditorGUILayout.PropertyField(disableDragIfFits);
            EditorGUILayout.PropertyField(isResizeCell);
            EditorGUILayout.PropertyField(notDataObject);
            EditorGUILayout.EndVertical();
        }

        void ScrollViewShowGUI()
        {
            EditorGUILayout.Space();

            if (!Application.isPlaying)
            {
                EditorGUILayout.Space();
                EditorGUILayout.LabelField("-----Editor mode-----");

                ScrollView scrollView = target as ScrollView;
                int tempValue = EditorGUILayout.IntField("SizeInEditor", scrollView.CellCount);
                if (tempValue != scrollView.CellCount)
                {
                    m_StrInvalidWarning = string.Empty;

                    try
                    {
                        scrollView.Init(tempValue);
                    }
                    catch (System.Exception e)
                    {
                        m_StrInvalidWarning = e.ToString();
                    }
                }

                if (!string.IsNullOrEmpty(m_StrInvalidWarning))
                {
                    EditorGUILayout.HelpBox(m_StrInvalidWarning, MessageType.Error);
                }
            }
        }
    }
}
