using UnityEngine;
using UnityEngine.SceneManagement;

public class SocketManager : MonoBehaviour
{
    public bool[] socketsOccupied = new bool[6];
    private int plugCount = 0;
    public GameObject[] sockets;
    
    // KULAĞI TAMAMEN TERSTEN TUTTUM ÇOK SİKLEMEYİN
    bool getSocketOccupied(int i)
    {
        return socketsOccupied[i];
    }

    public bool placeSingleSocket(int i)
    {
        if (!getSocketOccupied(i))
        {
            socketsOccupied[i] = true;
            plugCount++;
            return true;
        }
        return false;
    }

    public void RemoveSingleSocket(int i)
    {
        socketsOccupied[i] = false;
        plugCount--;
    }

    public bool placeDoubleSocket(int i)
    {
        if (i % 3 == 2)
        {
            if (!getSocketOccupied(i))
            {
                socketsOccupied[i] = true;
                plugCount++;
                return true;
            }
        }
        else
        {
            if (!getSocketOccupied(i) && !getSocketOccupied(i + 1))
            {
                socketsOccupied[i] = true;
                socketsOccupied[i + 1] = true;
                plugCount++;
                return true;
            }
        }

        return false;
    }

    public void removeDoubleSocket(int i)
    {
        if (i % 3 != 2)
        {
            socketsOccupied[i+1] = false;
        }
        socketsOccupied[i] = false;
        plugCount--;
    }

    public bool placeTripleSocket(int i)
    {
        if (i % 3 == 2)
        {
            if (!getSocketOccupied(i) && !getSocketOccupied(i - 1))
            {
                socketsOccupied[i] = true;
                socketsOccupied[i - 1] = true;
                plugCount++;
                return true;
            }
        }
        else if (i % 3 == 1)
        {
            if (!getSocketOccupied(i) && !getSocketOccupied(i + 1) && !getSocketOccupied(i - 1))
            {
                socketsOccupied[i] = true;
                socketsOccupied[i + 1] = true;
                socketsOccupied[i - 1] = true;
                plugCount++;
                return true;
            }
        }
        else
        {
            if (!getSocketOccupied(i) && !getSocketOccupied(i + 1))
            {
                socketsOccupied[i] = true;
                socketsOccupied[i + 1] = true;
                plugCount++;
                return true;
            }
        }
        return false;
    }

    public void removeTripleSocket(int i)
    {
        if (i % 3 != 2)
        {
            socketsOccupied[i + 1] = false;
        }
        if (i % 3 != 0)
        {
            socketsOccupied[i - 1] = false;
        }
        socketsOccupied[i] = false;
        
        plugCount--;
    }

    // Call this explicitly after calling place functions.
    public void CheckSuccess()
    {
        if (plugCount >= 5)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
        Debug.Log(plugCount);
    }


    public void ReplayClicked()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
