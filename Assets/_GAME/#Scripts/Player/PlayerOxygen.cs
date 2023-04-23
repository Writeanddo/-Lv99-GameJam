using UnityEngine;
using System.Collections;
using System;

public class PlayerOxygen : MonoBehaviour
{
    public event Action<float, float> OnUpdateOxygen;

    [SerializeField] private float maxOxygen = 1;
    [SerializeField] private float percentToRemove = 0.001f;

    [SerializeField] private float damage = 0.001f;

    private IDamageable damageable;
    private float _currentOxygen;

    private bool waiting;

    private void Awake()
    {
        damageable = GetComponent<IDamageable>();
    }

    private void Start()
    {
        _currentOxygen = maxOxygen;

        StartCoroutine(RemovingOxygen());
    }

    private IEnumerator RemovingOxygen()
    {
        while(_currentOxygen > 0)
        {
            _currentOxygen -= percentToRemove;

            yield return new WaitForSeconds(0.01f);
           
            OnUpdateOxygen?.Invoke(_currentOxygen, maxOxygen);
        }
        
        while(damageable.IsDie == false)
        {
            if(_currentOxygen <= 0)
            {
                damageable.TakeDamageOxygen(damage);
                yield return new WaitForSeconds(0.01f);

                OnUpdateOxygen?.Invoke(_currentOxygen, maxOxygen);
            }
        }

        StartCoroutine(RemovingOxygen());
    }

    public void RemoveOxygen(float oxygen)
    {
        _currentOxygen -= oxygen;

        _currentOxygen -= percentToRemove;
    }

    public void AddOxygen(float oxygen)
    {
        _currentOxygen += oxygen;

        if(_currentOxygen >= maxOxygen)
            _currentOxygen = maxOxygen;

        _currentOxygen -= percentToRemove;
    }

    public void Block()
    {
        waiting = true;
    }

    public void RemoveBlock()
    {
        waiting = false;
    }
}
