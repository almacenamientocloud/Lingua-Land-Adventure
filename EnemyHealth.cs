using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour // Nos permite hacer funciones como la de Start-Update-OncollisionEnter y más
{   
    [Header("EnemyHP parameters")]
    public int maxHp = 5;
    public int hp = 5;

    protected bool invincible; // pasamos de bool a protected para poder acceder desde el hijo, variables protegidas que accede el padre y el hijo
    protected float invincibilityTime = 0.6f; // 0.6 / 0.1 = 6 pasa rapido y no lo vemos
    protected float blinkTime = 0.1f;

    public float KnockbackStrength = 2f;
    protected float KnockbachTime = 0.3f;

    protected Rigidbody2D rigidBody;
    protected SpriteRenderer spriteRenderer;
    protected EnemyHit enemyHit; // Referencia al script

    protected Vector2 initialPosicition; // Reiniciar la posición del enemigo

    public virtual void Awake() //Awake una función antes de star paea evitar error al ejecutar varios start
    {
        rigidBody = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        enemyHit = GetComponentInChildren<EnemyHit>(); // Se toma desde el hijo

        initialPosicition = transform.position; // iniciar la posición de enemigo 

        maxHp = hp; // lo ideaql es hp = maxHp; maxHp = hp;
    }

    void OnTriggerEnter2D(Collider2D collision) // por eso la espada es de tipo trigger para no chocar con los muros
    {
        if (collision.CompareTag("Weapon")&& !invincible)// colisión con el arma hacia el enemigo
        {
            hp--; //menos uno de vida
            if (hp <= 0) // llega a cero
            {
                enemyHit.Defeat();
            }
            StopBehaviour();
            StartCoroutine(Invincibility());
            StartCoroutine(KnockBack(collision.transform.position));
        }
    }

    IEnumerator Invincibility()
    {
        invincible = true;
        float auxTime = invincibilityTime;

        while (auxTime > 0)
        {
            yield return new WaitForSeconds(blinkTime); // esperar el tiempo del blink
            auxTime -= blinkTime;
            spriteRenderer.enabled = !spriteRenderer.enabled; // si esta activo se desactiva
        }

        spriteRenderer.enabled = true;
        invincible = false;
    }

    IEnumerator KnockBack(Vector3 hitPosition)
    {
        if (KnockbackStrength <= 0)
        {
            if(hp >  0) ContinueBehaviour();
            yield break;
        }

        rigidBody.velocity = (transform.position - hitPosition).normalized * KnockbackStrength;
        yield return new WaitForSeconds(KnockbachTime);
        rigidBody.velocity = Vector3.zero; // se puede Vector2.zero; juego en 2d
        yield return new WaitForSeconds(KnockbachTime); // enemigo se detine al recibir un golpe por un tiempo
        if (hp > 0) ContinueBehaviour();
    }

    public void HideEnemy()
    {
        StopAllCoroutines(); // para las courrutinas para que el enemigo deje de parpadear.
        rigidBody.velocity = Vector3.zero;
        spriteRenderer.enabled = false;
    }

    public virtual void StopBehaviour() { } // acceder del hijo para parar el movimiento

    public virtual void ContinueBehaviour() { } // para continuar

    public virtual void ResetPosition()  // Reinicie la posición
    {
        transform.position = initialPosicition;
        hp = maxHp; // Reinicia la vida del Enemy
    }
}
