using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;

public class DrawMeshFull : MonoBehaviour
{

    public static DrawMeshFull Instance { get; private set; }

    [SerializeField] private Material drawMeshMaterial;

    private GameObject lastGameObject;
    private int lastSortingOrder;
    private Mesh mesh;
    private MeshLineBuilder meshBuilder;

    private Vector3 lastMouseWorldPosition;
    private float lineThickness = 1f;
    private Color lineColor = Color.green;

    private List<GameObject> allDrawnObjects = new List<GameObject>();


    [SerializeField] private Material drawLineMaterial;
    [SerializeField] private float minDistance = 0.01f;

    private LineRenderer currentLine;
    private List<Vector3> points = new();

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        if (!IsPointerOverUI())
        {
            Vector3 mouseWorldPosition = GetMouseWorldPosition();

            if (Input.GetMouseButtonDown(0))
            {
                CreateNewLine();
                AddPoint(mouseWorldPosition);
            }

            if (Input.GetMouseButton(0))
            {
                if (Vector3.Distance(lastMouseWorldPosition, mouseWorldPosition) > minDistance)
                {
                    AddPoint(Vector3.Lerp(lastMouseWorldPosition, mouseWorldPosition, 0.5f)); // smoother
                    lastMouseWorldPosition = mouseWorldPosition;
                }
            }
        }
    }

    static int order = 1;
    private void CreateNewLine()
    {
        GameObject lineObj = new GameObject("LineRendererStroke");
        LineRenderer lr = lineObj.AddComponent<LineRenderer>();
        Material mat = new Material(drawLineMaterial);
        lr.material = mat;
        lr.widthMultiplier = lineThickness;
        lr.numCornerVertices = 8;
        lr.numCapVertices = 8;
        lr.positionCount = 0;
        lr.useWorldSpace = true;
        lr.alignment = LineAlignment.View;
        lr.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        lr.receiveShadows = false;
        lr.textureMode = LineTextureMode.Stretch;
        lr.gameObject.layer = 6;

        lr.sortingOrder = order++;
        lr.sortingLayerName = "Foreground"; // or any layer ABOVE the one used by your sprite
        

        currentLine = lr;
        points.Clear();
        allDrawnObjects.Add(lineObj);

        lastMouseWorldPosition = GetMouseWorldPosition(); 
    }

    private void AddPoint(Vector3 point)
    {
        points.Add(point);
        currentLine.positionCount = points.Count;
        currentLine.SetPositions(points.ToArray());
    }
    private void CreateMeshObject()
    {
        lastGameObject = new GameObject("DrawMeshSingle", typeof(MeshFilter), typeof(MeshRenderer));
        lastSortingOrder++;
        lastGameObject.GetComponent<MeshRenderer>().sortingOrder = lastSortingOrder;

        allDrawnObjects.Add(lastGameObject);
    }

    public void SetThickness(float lineThickness)
    {
        this.lineThickness = lineThickness;
        meshBuilder?.SetThickness(lineThickness);
    }

    public void SetColor(Color lineColor)
    {
        this.lineColor = lineColor;
    }

    public static Vector3 GetMouseWorldPosition()
    {
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        worldPosition.z = 0f;
        return worldPosition;
    }

    public static bool IsPointerOverUI()
    {
        return EventSystem.current != null && EventSystem.current.IsPointerOverGameObject();
    }
    public void ClearAllLines()
    {
        foreach (var obj in allDrawnObjects)
        {
            Destroy(obj);
        }
        order = 1;
        allDrawnObjects.Clear();
    }
}
