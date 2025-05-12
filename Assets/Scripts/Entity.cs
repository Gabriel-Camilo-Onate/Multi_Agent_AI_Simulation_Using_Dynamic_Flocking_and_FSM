using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    [Header("General")]
    [SerializeField] protected int _team;
    public int team { get { return _team; } }
    [SerializeField] protected int _life;
    [SerializeField] protected int _maxLife;

    [Header("Field Of View")]
    [SerializeField] protected bool _gizmosOnOff;
    public bool gizmosOnOff { get { return _gizmosOnOff; } }

    [Header("Attack")]
    [SerializeField] protected List<Transform> _targetsToAttack=new List<Transform>();
    [SerializeField] protected int _attackDamage;
    [SerializeField] protected float _attacksPerSecond;

    [Header("Movement")]
    [SerializeField] protected Vector3 _velocity;
    public Vector3 velocity { get { return _velocity; } }
    [SerializeField] protected float _speed;
    [SerializeField] protected float _maxForce;

    protected virtual void Start()
    {
        FOVManager.instance.SuscribeToViewableEntities(_team, this);
    }
    public virtual void SetTargetToAttack(Transform target, bool isInSight)
    {
        if(isInSight)
        {
            if (!_targetsToAttack.Contains(target))
                _targetsToAttack.Add(target);
        }
        else
        {
            if (_targetsToAttack.Contains(target))
                _targetsToAttack.Remove(target);
        }
    }
    public virtual bool IsMinion()
    {
        return true;
    } 
}
