using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Plug : MonoBehaviour
{
    public enum PlugType
    {
        Single,
        Double,
        Triple
    }

    [Header("References")]
    [SerializeField] private GameObject cableEnd;
    [SerializeField] private GameObject cableStart;
    [SerializeField] private GameObject plug;
    [SerializeField] private LineRenderer cableRenderer;
    [SerializeField] private GameObject socketManagerObj;
    public PlugType plugType;

    //private Queue<float> _times = new Queue<float>();
    
    private Vector3 cableStartPos;
    private Vector3 cableEndPos;

    private SocketManager socketManager;

    [Header("Settings")]
    [SerializeField] private Vector2 _checkSize;

    private bool _isDragging = false;
    private bool _isPlugged = false;
    private int _pluggedIndex = -1;

    private LayerMask _targetLayer;
    private LayerMask _plugLayer;
    //private Queue<BoxCollider2D> _disableQueue = new Queue<BoxCollider2D>();

    private Collider2D _collider;
    private Collider2D _colliderPlug;

    void Start()
    {
        _targetLayer = LayerMask.GetMask("Socket");
        _plugLayer = LayerMask.GetMask("Plug");
        socketManager = socketManagerObj.GetComponent<SocketManager>();
        _colliderPlug = plug.GetComponent<Collider2D>();
        _collider = GetComponent<Collider2D>();
        cableStartPos = cableStart.transform.position;
        cableEndPos = cableEnd.transform.position;
        cableRenderer.SetPosition(0, cableStartPos);
        cableRenderer.SetPosition(1, cableEndPos);
    }

    void Update()
    {
        if (_isDragging && !_isPlugged)
        {
            Vector2 mouseWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = mouseWorld;
            CheckPlugging();
        }
        
        cableEndPos = cableEnd.transform.position;
        
        cableRenderer.SetPosition(1, cableEndPos);

        /*if (_times.Count > 0)
        {
            if (Time.time - 1f >= _times.Peek())
            {
                _disableQueue.Dequeue().enabled = true;
                _times.Dequeue();
            }
        }*/
    }

    private void CheckPlugging()
    {
        Vector2 hitPos = plugType == PlugType.Double ? (Vector2) transform.position + new Vector2(-0.5f, 0f) : transform.position;
        Collider2D socketHit = Physics2D.OverlapCircle(hitPos, 0.1f, _targetLayer);

        if (socketHit)
        {
            int index = socketHit.gameObject.GetComponent<Socket>().index;
            bool success;
            switch (plugType)
            {
                case PlugType.Single:
                    success = socketManager.placeSingleSocket(index);
                    break;
                case PlugType.Double:
                    success = socketManager.placeDoubleSocket(index);
                    break;
                case PlugType.Triple:
                    success = socketManager.placeTripleSocket(index);
                    break;
                default:
                    success = false;
                    break;
            }

            if (success)
            {
                _pluggedIndex = index;
                Vector3 offset = plugType == PlugType.Double ? new Vector3(0.5f, 0.33f, 0f) : new Vector3(0f, 0.33f, 0f);
                transform.position = socketHit.transform.position + offset;
                _isDragging = false;
                _isPlugged = true;
                plug.GetComponent<BoxCollider2D>().enabled = false;
                //_collider = socketHit.gameObject.GetComponent<Collider2D>();
                //_collider.enabled = false;
                socketManager.CheckSuccess();
            }
        }
        else
        {
            if (_pluggedIndex != -1)
            {
                /*_isPlugged = false;
                _disableQueue.Append(_collider);
                _collider.enabled = false;
                _times.Append(Time.time);
                switch (plugType)
                {
                    case PlugType.Single:
                        socketManager.RemoveSingleSocket(_pluggedIndex);
                        break;
                    case PlugType.Double:
                        socketManager.removeDoubleSocket(_pluggedIndex);
                        break;
                    case PlugType.Triple:
                        socketManager.removeTripleSocket(_pluggedIndex);
                        break;
                }
                _pluggedIndex = -1;*/
            }
        }
    }

    private void OnMouseDown()
    {
        _isDragging = true;
    }

    private void OnMouseUp()
    {
        _isDragging = false;
    }

    public bool IsPlugged()
    {
        return _isPlugged;
    }
}
