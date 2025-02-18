using UnityEngine;
using System.Linq;

public class mazeCell : MonoBehaviour
{
    [SerializeField]
    private GameObject _leftWall;

    [SerializeField]
    private GameObject _rightWall;

    [SerializeField]
    private GameObject _frontWall;

    [SerializeField]
    private GameObject _backWall;

    [SerializeField]
    private GameObject _unVisitedtBlock;

    public bool isVisited { get; private set; }

    public void Visited()
    {
        isVisited = true;
        _unVisitedtBlock.SetActive(false);
    }

    public void clearLeftWalls()
    {
        _leftWall.SetActive(false);

    }

    public void clearRightWalls()
    {
        _rightWall.SetActive(false);

    }

    public void clearFrontWalls()
    {
        _frontWall.SetActive(false);

    }

    public void clearBackWalls()
    {
        _backWall.SetActive(false);

    }

}
