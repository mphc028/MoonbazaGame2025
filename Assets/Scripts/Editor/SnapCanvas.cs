using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

public class SnapUIToPixels : EditorWindow
{
    [MenuItem("Tools/Snap UI To Pixels")]
    public static void ShowWindow()
    {
        GetWindow<SnapUIToPixels>("Snap UI To Pixels");
    }

    private int referenceWidth = 640;
    private int referenceHeight = 360;

    void OnGUI()
    {
        GUILayout.Label("Snap all UI elements in Canvas to pixel grid", EditorStyles.boldLabel);

        referenceWidth = EditorGUILayout.IntField("Reference Width", referenceWidth);
        referenceHeight = EditorGUILayout.IntField("Reference Height", referenceHeight);

        if (GUILayout.Button("Snap Selected Canvas"))
        {
            SnapSelectedCanvas();
        }
    }

    void SnapSelectedCanvas()
    {
        foreach (GameObject go in Selection.gameObjects)
        {
            Canvas canvas = go.GetComponent<Canvas>();
            if (canvas == null)
            {
                Debug.LogWarning(go.name + " has no Canvas component.");
                continue;
            }

            RectTransform[] rects = canvas.GetComponentsInChildren<RectTransform>(true);

            foreach (RectTransform rt in rects)
            {
                Undo.RecordObject(rt, "Snap UI Position");
                Vector2 pos = rt.anchoredPosition;

                pos.x = Mathf.Round(pos.x);
                pos.y = Mathf.Round(pos.y);

                rt.anchoredPosition = pos;
                EditorUtility.SetDirty(rt);
            }
        }

        Debug.Log("Snapped all UI elements in selected Canvas(es) to pixel grid.");
    }
}
