using UnityEditor;
using UnityEngine;
using static Sequence;

[CustomPropertyDrawer(typeof(SequenceAction))]
public class SequenceActionDrawer : PropertyDrawer
{
    private const float VerticalSpace = 6f; // Adjust the value to control the vertical space
    private readonly Color audioPanelColor = new Color(0.8f, 0.8f, 1f); // Color for the panel when ActionType is Audio

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        SerializedProperty actionType = property.FindPropertyRelative("actionType");
        ActionType selectedActionType = (ActionType)actionType.enumValueIndex;

        Rect currentFieldPosition = new Rect(position.x, position.y, position.width, 0f);

        // Change panel color for ActionType element if it is set to ActionType.Audio
        Color originalBackgroundColor = GUI.backgroundColor;
        if (selectedActionType == ActionType.Audio)
        {
            GUI.backgroundColor = ConvertToColor("#DFFF3C");
            EditorGUI.DrawRect(position, GetLighterColor("#DFFF3C"));
        }
        
        if (selectedActionType == ActionType.Animation)
        {
            GUI.backgroundColor = ConvertToColor("#55D867");
            EditorGUI.DrawRect(position, GetLighterColor("#55D867"));
        }

        if (selectedActionType == ActionType.GameObject)
        {
            GUI.backgroundColor = ConvertToColor("#FF913C");
            EditorGUI.DrawRect(position, GetLighterColor("#FF913C"));
        }

        if (selectedActionType == ActionType.Camera)
        {
            GUI.backgroundColor = ConvertToColor("#3C91FF"); ;
            EditorGUI.DrawRect(position, GetLighterColor("#3C91FF"));
        }

        ShowPropertyField(actionType, ref currentFieldPosition, label);

        // Reset panel color
        GUI.backgroundColor = originalBackgroundColor;

        switch (selectedActionType)
        {
            case ActionType.Animation:
                ShowPropertyField(property.FindPropertyRelative("animationClip"), ref currentFieldPosition);
                ShowPropertyField(property.FindPropertyRelative("waitForCompletion"), ref currentFieldPosition);
                break;
            case ActionType.Audio:
                ShowPropertyField(property.FindPropertyRelative("audioClip"), ref currentFieldPosition);
                ShowPropertyField(property.FindPropertyRelative("audioDelay"), ref currentFieldPosition);
                ShowPropertyField(property.FindPropertyRelative("waitForCompletion"), ref currentFieldPosition);
                break;
            case ActionType.GameObject:
                ShowPropertyField(property.FindPropertyRelative("gameObject"), ref currentFieldPosition);
                ShowPropertyField(property.FindPropertyRelative("enable"), ref currentFieldPosition);
                break;
            case ActionType.Camera:
                ShowPropertyField(property.FindPropertyRelative("cameraPosition"), ref currentFieldPosition);
                ShowPropertyField(property.FindPropertyRelative("cameraMovementDuration"), ref currentFieldPosition);
                ShowPropertyField(property.FindPropertyRelative("cameraAnimationTime"), ref currentFieldPosition);
                ShowPropertyField(property.FindPropertyRelative("cameraAudioTime"), ref currentFieldPosition);
                break;
        }

        EditorGUI.EndProperty();
    }

    private void ShowPropertyField(SerializedProperty property, ref Rect position, GUIContent label = null)
    {
        float fieldHeight = EditorGUI.GetPropertyHeight(property);
        Rect fieldPosition = new Rect(position.x, position.yMax + VerticalSpace, position.width, fieldHeight);
        position = fieldPosition;
        EditorGUI.PropertyField(fieldPosition, property, label);
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        SerializedProperty actionType = property.FindPropertyRelative("actionType");
        ActionType selectedActionType = (ActionType)actionType.enumValueIndex;

        float totalHeight = EditorGUI.GetPropertyHeight(actionType);

        switch (selectedActionType)
        {
            case ActionType.Animation:
                totalHeight += VerticalSpace;
                totalHeight += EditorGUI.GetPropertyHeight(property.FindPropertyRelative("animationClip")) + VerticalSpace;
                totalHeight += EditorGUI.GetPropertyHeight(property.FindPropertyRelative("waitForCompletion")) + VerticalSpace;
                break;
            case ActionType.Audio:
                totalHeight += VerticalSpace;
                totalHeight += EditorGUI.GetPropertyHeight(property.FindPropertyRelative("audioClip")) + VerticalSpace;
                totalHeight += EditorGUI.GetPropertyHeight(property.FindPropertyRelative("audioDelay")) + VerticalSpace;
                totalHeight += EditorGUI.GetPropertyHeight(property.FindPropertyRelative("waitForCompletion")) + VerticalSpace;
                break;
            case ActionType.GameObject:
                totalHeight += VerticalSpace;
                totalHeight += EditorGUI.GetPropertyHeight(property.FindPropertyRelative("gameObject")) + VerticalSpace;
                totalHeight += EditorGUI.GetPropertyHeight(property.FindPropertyRelative("enable")) + VerticalSpace;
                break;
            case ActionType.Camera:
                totalHeight += VerticalSpace;
                totalHeight += EditorGUI.GetPropertyHeight(property.FindPropertyRelative("cameraPosition")) + VerticalSpace;
                totalHeight += EditorGUI.GetPropertyHeight(property.FindPropertyRelative("cameraMovementDuration")) + VerticalSpace;
                totalHeight += EditorGUI.GetPropertyHeight(property.FindPropertyRelative("cameraAnimationTime")) + VerticalSpace;
                totalHeight += EditorGUI.GetPropertyHeight(property.FindPropertyRelative("cameraAudioTime")) + VerticalSpace;
                break;
        }

        return totalHeight;
    }
    public static Color ConvertToColor(string hexCode)
    {
        Color color;

        if (ColorUtility.TryParseHtmlString(hexCode, out color))
        {
            return color;
        }

        // If parsing fails, return a default color (e.g., white)
        return Color.white;
    }

    public static Color GetDarkerColor(string hexCode)
    {
        Color color;

        if (ColorUtility.TryParseHtmlString(hexCode, out color))
        {
            // Calculate the darker color by multiplying the RGB values by 0.75 (25%)
            color.r *= 0.75f;
            color.g *= 0.75f;
            color.b *= 0.75f;

            return color;
        }

        // If parsing fails, return a default color (e.g., white)
        return Color.white;
    }
    public static Color GetLighterColor(string hexCode)
    {
        Color color;

        if (ColorUtility.TryParseHtmlString(hexCode, out color))
        {
            // Calculate the lighter color by adding 25% of the difference between the current value and 1.0
            color.r += (1f - color.r) * 0.25f;
            color.g += (1f - color.g) * 0.25f;
            color.b += (1f - color.b) * 0.25f;

            return color;
        }

        // If parsing fails, return a default color (e.g., white)
        return Color.white;
    }
}