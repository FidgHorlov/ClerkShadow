using ClerkShadow.Levels;
using UnityEditor;
using UnityEngine;

namespace ClerkShadow.Editor
{
    [CustomEditor(typeof(LevelManager))]
    public class LevelManagerEditor : UnityEditor.Editor
    {
        private int _currentLevel;
        
        private LevelManager _levelManager;
        private LevelManager LevelManager => _levelManager ??= (LevelManager) target;


        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (GUILayout.Button("Delete progress"))
            {
                LevelManager.ResetLevelEditor();
            }

            EditorGUILayout.BeginHorizontal();
            _currentLevel = EditorGUILayout.IntField("Target level", _currentLevel);
            if (GUILayout.Button("Set current level value"))
            {
                LevelManager.SetCustomLevelEditor(_currentLevel);
            }
            EditorGUILayout.EndHorizontal();

            if (GUILayout.Button("Get current level "))
            {
                _currentLevel = LevelManager.GetCurrentLevelEditor();
            }
        }
    }
}
