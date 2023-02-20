using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "CCData", menuName = "Scriptable Object/CC Data", order = int.MaxValue)]
public class CCData : ScriptableObject
{
    [SerializeField]
    private string ccName;
    public string CCName { get { return ccName; } }

    [SerializeField]
    private Sprite icon;
    public Sprite Icon { get { return icon; } }
}
