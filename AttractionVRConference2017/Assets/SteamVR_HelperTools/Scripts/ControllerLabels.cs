using UnityEngine;
using System.Collections;
using System;

public class ControllerLabels : MonoBehaviour
{
    [System.Serializable]
    public class Label
    {
        [HideInInspector]
        public GameObject labelGameObject;
 
        public enum Controller { Left, Right }
        [Header("Controller Properties")]
        [Tooltip("The controller you wish to place label")]
        public Controller controller;

        public enum ViveButton { GripLeft, GripRight, TriggerRight, TriggerLeft, MenuButton, TrackPad, TrackPadDown, TrackPadUp, TrackPadLeft, TrackPadRight }
        [Tooltip("The button on the controller this label describes")]
        public ViveButton button;
        [HideInInspector]
        public string buttonName;
        
        [HideInInspector]
        public TextMesh textMesh;
        [Header("Label Properties")]
        [Tooltip("Hide the label")]
        public bool hideLabel;
        [Tooltip("Text on this label")]
        public string text;
        [Tooltip("Size of the text")]
        public float textSize;
        [Tooltip("Color of the text")]
        public Color textColor;
        [Tooltip("The shift in position from the default position. Each button has a different default position. Best to set this while in Play Mode.")]
        public Vector3 shiftPosition;
        [HideInInspector]
        public Vector3 originPosition;

        [HideInInspector]
        public LineRenderer lineRenderer;
        [HideInInspector]
        public Material lineMaterial;
        [Header("Line Properties")]
        [Tooltip("Hide the label")]
        public bool hideLine;
        [Tooltip("Width of the line")]
        public float lineWidth;
        [Tooltip("Color of the line")]
        public Color lineColor;

    }

    [Header("Wands")]
    [Tooltip("The left controller from the SteamVR CameraRig")]
    public GameObject wandLeft;
    [Tooltip("The right controller from the SteamVR CameraRig")]
    public GameObject wandRight;
    [Header("Font")]
    [Tooltip("Font of the labels")]
    public Font font;
    [Tooltip("Material of the labels")]
    public Material fontMaterial;

    [Header("Globals")]
    public bool useGlobalTextSize;
    [Tooltip("If this is anything other than 0.0 than all labels will use this text size. Otherwise they use their own setting")]
    public float textSize = 0.2f;
    public bool useGlobalTextColor;
    [Tooltip("If this is anything other than 0.0 than all labels will use this line width. Otherwise they use their own setting")]
    public Color textColor = Color.white;
    [Space(10)]
    public bool useGlobalLineWidth;
    [Tooltip("If this is anything other than 0.0 than all labels will use this line width. Otherwise they use their own setting")]
    public float lineWidth = 0.003f;
    public bool useGlobalLineColor;
    [Tooltip("If this is anything other than 0.0 than all labels will use this line width. Otherwise they use their own setting")]
    public Color lineColor = Color.white;


    [Header("Labels")]
    public Label[] labels;

    private Vector3 scaleCameraRig;
    private Vector3 lrShiftRight = new Vector3(0.014f, 0.0f, 0.0f);
    private Vector3 lrShiftLeft = new Vector3(0.0f, 0.0f, 0.014f);
    private Vector3 lrShiftUp = new Vector3(0.0f, 0.014f, 0.0f);
    private Vector3 lrShiftDown = new Vector3(0.0f, -0.014f, 0.0f);


    void Start()
    {
        scaleCameraRig = wandLeft.transform.parent.lossyScale;


        foreach (Label label in labels)
        {
            label.labelGameObject = new GameObject();
            label.labelGameObject.name = label.controller.ToString() + " - " + label.button.ToString();
            label.labelGameObject.transform.parent = this.transform;

            label.labelGameObject.AddComponent<MeshRenderer>();
            label.labelGameObject.GetComponent<MeshRenderer>().material = fontMaterial;

            label.textMesh = label.labelGameObject.AddComponent<TextMesh>();
            label.textMesh.text = label.text;
            label.textMesh.font = font;
            if (useGlobalTextColor) label.textMesh.color = textColor;
            else label.textMesh.color = label.textColor;
            if (useGlobalTextSize) label.textMesh.characterSize = textSize / font.fontSize;
            else label.textMesh.characterSize = label.textSize / font.fontSize;

            label.lineMaterial = new Material(Shader.Find("Standard"));
            label.lineMaterial.EnableKeyword("_EMISSION");
            if (useGlobalLineColor)
            {
                label.lineMaterial.SetColor("_Color", lineColor);
                label.lineMaterial.SetColor("_EmissionColor", lineColor);
            }
            else
            {
                label.lineMaterial.SetColor("_Color", label.lineColor);
                label.lineMaterial.SetColor("_EmissionColor", label.lineColor);
            }

            label.lineRenderer = label.labelGameObject.AddComponent<LineRenderer>();
            label.lineRenderer.material = label.lineMaterial;
            label.lineRenderer.SetColors(label.lineColor, label.lineColor);

            if (useGlobalLineWidth) label.lineRenderer.SetWidth(lineWidth, lineWidth);
            else label.lineRenderer.SetWidth(label.lineWidth, label.lineWidth);

            SetButtonInfo(label);

        }
    }

    void Update()
    {
        if (scaleCameraRig != wandLeft.transform.parent.lossyScale)
        {
            scaleCameraRig = wandLeft.transform.parent.localScale;
            OnValidate();
        }

        foreach (Label label in labels)
        {
            if (!label.hideLabel) label.labelGameObject.SetActive(true);
            else label.labelGameObject.SetActive(false);
        }

        if (wandLeft.activeSelf)
        {
            if (wandLeft.GetComponent<SteamVR_TrackedObject>().index != SteamVR_TrackedObject.EIndex.None)
            {
                foreach (Label label in labels)
                {
                    if (label.controller == Label.Controller.Left) SetLabelLocation(label, label.shiftPosition + label.originPosition, wandLeft);
                }
            }
        }

        if (wandRight.activeSelf)
        {
            if (wandRight.GetComponent<SteamVR_TrackedObject>().index != SteamVR_TrackedObject.EIndex.None)
            {
                foreach (Label label in labels)
                {
                    if (label.controller == Label.Controller.Right) SetLabelLocation(label, label.shiftPosition + label.originPosition, wandRight);
                }
            }
        }

    }

    void OnValidate()
    {
        try
        {
            foreach (Label label in labels)
            {
                label.labelGameObject.name = label.controller.ToString() + " - " + label.button.ToString();

                label.labelGameObject.GetComponent<MeshRenderer>().material = fontMaterial;

                label.textMesh.text = label.text;
                label.textMesh.font = font;
                if (useGlobalTextColor) label.textMesh.color = textColor;
                else label.textMesh.color = label.textColor;
                if (useGlobalTextSize) label.textMesh.characterSize = textSize / font.fontSize;
                else label.textMesh.characterSize = label.textSize / font.fontSize;

                label.lineRenderer.SetColors(label.lineColor, label.lineColor);
                if (useGlobalLineColor)
                {
                    label.lineMaterial.SetColor("_Color", lineColor);
                    label.lineMaterial.SetColor("_EmissionColor", lineColor);
                }
                else
                {
                    label.lineMaterial.SetColor("_Color", label.lineColor);
                    label.lineMaterial.SetColor("_EmissionColor", label.lineColor);
                }

                if (useGlobalLineWidth) label.lineRenderer.SetWidth(lineWidth, lineWidth);
                else label.lineRenderer.SetWidth(label.lineWidth, label.lineWidth);

                SetButtonInfo(label);

            }
        }
        catch (System.NullReferenceException) { }
    }

    private void SetLabelLocation(Label label, Vector3 menuObjPos, GameObject wand)
    {
        Vector3 wandPos = wand.transform.position;
        Quaternion wandRot = wand.transform.rotation * Quaternion.Euler(95, 0, 0);

        label.labelGameObject.transform.position = wandPos;
        label.labelGameObject.transform.Translate(menuObjPos);
        label.labelGameObject.transform.rotation = wandRot;

        if (!label.hideLine)
        {
            label.labelGameObject.GetComponent<LineRenderer>().SetVertexCount(2);
            label.labelGameObject.GetComponent<LineRenderer>().SetPosition(0, label.labelGameObject.transform.position);

            try
            {
                Vector3 lineEnd = wand.transform.GetChild(0).transform.Find(label.buttonName).transform.GetChild(0).transform.position;

                switch (label.button)
                {
                    case Label.ViveButton.TrackPadUp:
                        lineEnd += Vector3.Scale(wandRot * lrShiftUp, scaleCameraRig);
                        break;
                    case Label.ViveButton.TrackPadDown:
                        lineEnd += Vector3.Scale(wandRot * lrShiftDown, scaleCameraRig);
                        break;
                    case Label.ViveButton.TrackPadRight:
                        lineEnd += Vector3.Scale(wandRot * lrShiftRight, scaleCameraRig);
                        break;
                    case Label.ViveButton.TrackPadLeft:
                        lineEnd += Vector3.Scale(wandRot * lrShiftLeft, scaleCameraRig);
                        break;
                    default:
                        break;
                }

                label.labelGameObject.GetComponent<LineRenderer>().SetPosition(1, lineEnd);

            } catch (System.NullReferenceException) { }

        }
    }

    private void SetButtonInfo(Label label)
    { 
        switch (label.button)
        {
            case Label.ViveButton.GripLeft:
                label.textMesh.anchor = TextAnchor.UpperRight;
                label.textMesh.alignment = TextAlignment.Right;
                label.originPosition = new Vector3(-0.05f, -0.13f, 0.03f);
                label.originPosition = Vector3.Scale(label.originPosition, scaleCameraRig);
                label.buttonName = "lgrip";
                break;

            case Label.ViveButton.GripRight:
                label.textMesh.anchor = TextAnchor.UpperLeft;
                label.textMesh.alignment = TextAlignment.Left;
                label.originPosition = new Vector3(0.05f, -0.13f, 0.03f);
                label.originPosition = Vector3.Scale(label.originPosition, scaleCameraRig);
                label.buttonName = "rgrip";
                break;

            case Label.ViveButton.TriggerLeft:
                label.textMesh.anchor = TextAnchor.UpperRight;
                label.textMesh.alignment = TextAlignment.Right;
                label.originPosition = new Vector3(-0.046f, -0.08f, 0.052f);
                label.originPosition = Vector3.Scale(label.originPosition, scaleCameraRig);
                label.buttonName = "trigger";
                break;

            case Label.ViveButton.TriggerRight:
                label.textMesh.anchor = TextAnchor.UpperLeft;
                label.textMesh.alignment = TextAlignment.Left;
                label.originPosition = new Vector3(0.046f, -0.08f, 0.052f);
                label.originPosition = Vector3.Scale(label.originPosition, scaleCameraRig);
                label.buttonName = "trigger";
                break;

            case Label.ViveButton.MenuButton:
                label.textMesh.anchor = TextAnchor.LowerCenter;
                label.textMesh.alignment = TextAlignment.Center;
                label.originPosition = new Vector3(0.0f, -0.0008f, -0.01f);
                label.originPosition = Vector3.Scale(label.originPosition, scaleCameraRig);
                label.buttonName = "button";
                break;

            case Label.ViveButton.TrackPad:
                label.textMesh.anchor = TextAnchor.MiddleCenter;
                label.textMesh.alignment = TextAlignment.Center;
                label.originPosition = new Vector3(0.0f, -0.052f, -0.01f);
                label.originPosition = Vector3.Scale(label.originPosition, scaleCameraRig);
                label.buttonName = "trackpad";
                break;

            case Label.ViveButton.TrackPadDown:
                label.textMesh.anchor = TextAnchor.UpperCenter;
                label.textMesh.alignment = TextAlignment.Center;
                label.originPosition = new Vector3(0.0f, -0.074f, -0.02f);
                label.originPosition = Vector3.Scale(label.originPosition, scaleCameraRig);
                label.buttonName = "trackpad";
                break;

            case Label.ViveButton.TrackPadUp:
                label.textMesh.anchor = TextAnchor.LowerCenter;
                label.textMesh.alignment = TextAlignment.Center;
                label.originPosition = new Vector3(0.0f, -0.03f, -0.02f);
                label.originPosition = Vector3.Scale(label.originPosition, scaleCameraRig);
                label.buttonName = "trackpad";
                break;

            case Label.ViveButton.TrackPadLeft:
                label.textMesh.anchor = TextAnchor.MiddleRight;
                label.textMesh.alignment = TextAlignment.Right;
                label.originPosition = new Vector3(-0.04f, -0.054f, -0.02f);
                label.originPosition = Vector3.Scale(label.originPosition, scaleCameraRig);
                label.buttonName = "trackpad";
                break;

            case Label.ViveButton.TrackPadRight:
                label.textMesh.anchor = TextAnchor.MiddleLeft;
                label.textMesh.alignment = TextAlignment.Left;
                label.originPosition = new Vector3(0.04f, -0.054f, -0.02f);
                label.originPosition = Vector3.Scale(label.originPosition, scaleCameraRig);
                label.buttonName = "trackpad";
                break;

            default:
                break;

        }
    }

    public void HideAllLabels()
    {
        foreach (Label label in labels)
        {
            label.hideLabel = true;
        }
    }

    public void ShowAllLabels()
    {
        foreach (Label label in labels)
        {
            label.hideLabel = false;
        }
    }
}
