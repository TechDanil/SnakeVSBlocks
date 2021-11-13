using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    [SerializeField] private Vector2Int _obstacleHitSizeRange; 
    [SerializeField] private TMP_Text _view;

    private int _obstacleHitSize;

    private void Start()
    {
        _obstacleHitSize = Random.Range(_obstacleHitSizeRange.x, _obstacleHitSizeRange.y);
        _view.text = _obstacleHitSize.ToString();
    }

    public int Hit()
    {
        Destroy(gameObject);
        return _obstacleHitSize;
    }

}
