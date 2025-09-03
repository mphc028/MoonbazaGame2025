using UnityEngine;

[CreateAssetMenu(fileName = "Resource", menuName = "Scriptable Objects/Resource")]
public class Resource : ScriptableObject
{
    [Header("Resource Info")]
    public int id;
    public string resourceName;
    public Sprite icon;
}
