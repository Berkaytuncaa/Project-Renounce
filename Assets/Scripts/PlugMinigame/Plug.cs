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
    [SerializeField] private GameObject plug;
    [SerializeField] private LineRenderer cableRenderer;
    [SerializeField] private GameObject socketManagerObj;
    public PlugType plugType;

    private float _time;

    private SocketManager socketManager;

    [Header("Settings")]
    [SerializeField] private Vector2 _checkSize;

    private bool _isDragging = false;
    private bool _isPlugged = false;
    private int _pluggedIndex = -1;

    private LayerMask _targetLayer;
    private LayerMask _plugLayer;

    private Collider2D _collider;

    void Start()
    {
        _targetLayer = LayerMask.GetMask("Sockets");
        _plugLayer = LayerMask.GetMask("Plugs");
        socketManager = socketManagerObj.GetComponent<SocketManager>();
        _collider = plug.GetComponent<Collider2D>();
    }

    void Update()
    {
        if (_isDragging)
        {
            Vector2 mouseWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = mouseWorld;
            CheckPlugging();
        }
    }

    private void CheckPlugging()
    {
        Collider2D socketHit = Physics2D.OverlapCircle(transform.position, 0.3f, _targetLayer);

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
                Vector3 offset = plugType == PlugType.Double ? new Vector3(0.5f, 0f, 0f) : Vector3.zero;
                transform.position = socketHit.transform.position + offset;
                _isDragging = false;
                _isPlugged = true;
                _collider = socketHit.gameObject.GetComponent<Collider2D>();
                _collider.enabled = false;
                socketManager.CheckSuccess();
            }
        }
        else
        {
            if (_pluggedIndex != -1)
            {
                _isPlugged = false;
                _collider.enabled = false;
                _time = Time.time;
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
                _pluggedIndex = -1;
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
