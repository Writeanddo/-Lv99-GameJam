using System;
using System.Collections;
using UnityEngine;

public class HealthSystem : MonoBehaviour, IDamageable
{
    //[field:] serve pra deixar variaveis que tem esse { get; set; } no final, visiveis no editor;
    [field: SerializeField] public float CurrentHealth { get; set; }
    [field: SerializeField] public float MaxHealth { get; set; }
    public bool IsDie { get; set; }

    //diz que esse carinha alterou a vida (dano ou cura), e passa a vida atual
    public event Action<float, float> OnChangeHealth;
    //avisa que o gameObject foi morto
    public event Action<IDamageable> OnDie;
    public event Action<Vector3> OnTakeDamage;
    public event Action OnHeal;

    [SerializeField] private bool destroyOnDie;

    [SerializeField] private int enemyXp;
    public Collider2D collider2d;

    private Animator anim;
    public SpriteRenderer spriteTemp;
    public Color coloralpha;

    public Color colorBlue;

    private void Start()
    {
        anim = GetComponent<Animator>();
        collider2d = GetComponent<Collider2D>();
        /*if (gameObject.CompareTag("Player"))
            CurrentHealth = TreeController.Instance.CurrentHealth;*/
        OnChangeHealth?.Invoke(CurrentHealth, MaxHealth);
    }

    private void OnEnable()
    {
        CurrentHealth = MaxHealth;
        collider2d.enabled = true;
    }

    /// <summary>
    /// Tomar dano
    /// </summary>
    /// <param name="damage">dano</param>
    public void TakeDamage(Vector3 direction, float damage)
    {
        print("Tomou Dano");
        FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/Player/Hit", GetComponent<Transform>().position);

        if (damage <= 0)
            return;

        StartCoroutine(IEInvencibleHeart());
        CurrentHealth -= damage;

        if (CurrentHealth < 0)
        {
            Die();
            anim.SetBool("isDie", true);
            return;
        }

        OnChangeHealth?.Invoke(CurrentHealth, MaxHealth);
        OnTakeDamage?.Invoke(direction);
    }

    public void TakeDamageOxygen(float damage)
    {
        if (damage <= 0)
            return;

        StopCoroutine(IEInvencibleHeartOxygen());
        StartCoroutine(IEInvencibleHeartOxygen());

        CurrentHealth -= damage;

        if (CurrentHealth < 0)
        {
            Die();
            anim.SetBool("isDie", true);
            return;
        }

        OnChangeHealth?.Invoke(CurrentHealth, MaxHealth);
        OnTakeDamage?.Invoke(Vector3.zero);
    }

    private IEnumerator IEInvencibleHeart()
    {
        gameObject.layer = LayerMask.NameToLayer("invencivelPlayer");

        for (var i = 0; i < 21; i++) // alterado de "5" para "20" (Alyson)
        {
            spriteTemp.color = Color.clear;
            yield return new WaitForSeconds(0.05f);

            spriteTemp.color = coloralpha;
            yield return new WaitForSeconds(0.05f);

        }
        spriteTemp.color = new Color(spriteTemp.color.r, spriteTemp.color.g, spriteTemp.color.b, 1f);

        this.gameObject.layer = LayerMask.NameToLayer("Player");

    }

    private IEnumerator IEInvencibleHeartOxygen()
    {

        for (var i = 0; i < 2; i++)
        {
            spriteTemp.color = colorBlue;
            yield return new WaitForSeconds(0.05f);

            spriteTemp.color = coloralpha;
            yield return new WaitForSeconds(0.05f);

        }

        spriteTemp.color = Color.white;
        spriteTemp.color = new(spriteTemp.color.r, spriteTemp.color.g, spriteTemp.color.b, 1f);
    }

    public void Die()
    {
        if (IsDie)
            return;
        IsDie = true;

        gameObject.layer = LayerMask.NameToLayer("invencivelPlayer");

        OnDie?.Invoke(this);

        GameManager.Instance.GameOver(false);


        if (destroyOnDie)
        {
        }//evita que o player seja destruido
         //gameObject.SetActive(false);


    }

    public void SetDieAnimation()
    {
        gameObject.SetActive(false);
    }



    /// <summary>
    /// Para curar
    /// </summary>
    /// <param name="amount">quantidade</param>
    public void Heal(float amount)
    {
        if (amount <= 0)
            return;

        CurrentHealth += amount;

        if (CurrentHealth > MaxHealth)
            CurrentHealth = MaxHealth;

        OnChangeHealth?.Invoke(CurrentHealth, MaxHealth);
        OnHeal?.Invoke();
    }
}
