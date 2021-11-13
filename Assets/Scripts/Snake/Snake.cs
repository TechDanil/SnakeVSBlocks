using Assets.Scripts.GameKeeper;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(TailGenerator))]
[RequireComponent(typeof(PlayerInput))]
public class Snake : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private int _tailSize;
    [SerializeField] private float _tailSpringiness;
    [SerializeField] private SnakeHead _head;

    private PlayerInput _playerInput;
    private List<Segment> _tail;
    private TailGenerator _tailGenerator;

    public event UnityAction<bool> IsDeadthOccured;
    public event UnityAction<int> SizeUpdated;

    private bool _isAlive;

    private void Awake()
    {
        _tailGenerator = GetComponent<TailGenerator>();
        _playerInput = GetComponent<PlayerInput>();
    }

    private void OnEnable()
    {
        _head.BlockCollided += OnBlockCollided;
        _head.BonusCollected += OnBonusCollected;
        _head.ObstacleHit += OnObstacleHit;
    }

    private void Start()
    {
        _tail = _tailGenerator.Generate(_tailSize);

        SizeUpdated?.Invoke(_tail.Count);
    }

    private void OnDisable()
    {
        _head.BlockCollided -= OnBlockCollided;
        _head.BonusCollected -= OnBonusCollected;
        _head.ObstacleHit -= OnObstacleHit;
    }

    private void FixedUpdate()
    {
        if (IsAlive())
        {
            Move(_head.transform.position + _head.transform.up * _speed * Time.fixedDeltaTime);
            _head.transform.up = _playerInput.GetDirectionToClick(_head.transform.position);
        }
    }

    private void Move(Vector3 nextPosition)
    {
        Vector3 previousPosition = _head.transform.position;

        foreach (var segment in _tail)
        {
            Vector3 tempPosition = segment.transform.position;
            segment.transform.position = Vector2.Lerp(segment.transform.position, previousPosition, _tailSpringiness * Time.fixedDeltaTime);
            previousPosition = tempPosition;
        }

        _head.Move(nextPosition);
    }

    private void OnBlockCollided()
    {
        if (_tail.Count > 0)
        {
            Segment deletedSegment = _tail[_tail.Count - 1];
            _tail.Remove(deletedSegment);
            Destroy(deletedSegment.gameObject);
            SizeUpdated?.Invoke(_tail.Count);
        }
    }

    private void OnBonusCollected(int bonusSize)
    {
        _tail.AddRange(_tailGenerator.Generate(bonusSize));
        SizeUpdated?.Invoke(_tail.Count);
    }

    private void OnObstacleHit(int hitSize)
    {
        List<Segment> segmentsToDestroy = new List<Segment>();
        Segment deletedSegment;

        for (int i = hitSize; i > 0; i--)
        {
            deletedSegment = _tail[i];
            segmentsToDestroy.Add(deletedSegment);
        }

        for (int i = hitSize; i > 0; i--)
        {
            _tail.RemoveAt(i);
        }

        foreach (var segment in segmentsToDestroy)
        {
            Destroy(segment.gameObject);
        }

        SizeUpdated?.Invoke(_tail.Count);
    }

    private bool IsAlive()
    {
        if (_tail.Any())
        {
            return _isAlive = true;
        }

        IsDeadthOccured?.Invoke(_isAlive);

        return _isAlive = false;
    }
}


