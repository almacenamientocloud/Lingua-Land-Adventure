using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f; //Variable de velocidad a la cual se va a mover el personaje
    Vector2 direction; //Variable de dirección a la cual se va a mover el personaje en cordenadas x -> horizontal , y -> vertical 

    Rigidbody2D rigidBody;// se crea en unity para darle físicas al personaje
    Animator animator; // se crea en unity para darle el movimiento al personaje por medio de Animator
    SpriteRenderer spriteRenderer; // jugador parpadee

    bool isAttacking;

    bool invincible;
    float invincibilityTime = 1.2f; // 0.6 / 0.1 = 6 pasa rapido y no lo vemos
    float blinkTime = 0.1f;

    bool uncontrollable; // al recibir un golpe se puede controlar el jugador sin que se pare
    float KnockbackStrength = 2f;
    float KnockbachTime = 0.3f;

    GameManager gameManager;  // instancia de gamemMnager

    List<BasicInteration> basicInteractionList = new List<BasicInteration>(); // sobre escribir la interácción

    private void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();//usa el rigibody
        animator = GetComponent<Animator>(); //usa el animator
        spriteRenderer = GetComponent<SpriteRenderer>(); // usa el spriteRenderer
        gameManager = FindObjectOfType<GameManager>(); // usa el gameManager
    }

    private void FixedUpdate()
    {
        if(!uncontrollable)
        {
            rigidBody.velocity = direction * speed;//asigna la velocidad con relacion a la dirección y la velocidad
        }
    }

    private void Update()
    {
        Inputs();// interacciones del jugador
        Animations();//función de la animación
    }

    private void Inputs()// función de movimientos del player como RUN y ATTACK
    {
        if (isAttacking || uncontrollable) return;
        direction = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;//usa los valores de los inputs horizontal y vertical y los normaliza para que no esté mas rapido de un lado que del otro.

        if (Input.GetKeyDown(KeyCode.Space)) // si el usuario presiona la tecla usuario
        {
            if (basicInteractionList != null)
            {
                Vector2 playerFacing = new Vector2(animator.GetFloat("Horizontal"), animator.GetFloat("Vertical"));
                bool interactionSucess = false;

                foreach (BasicInteration basicInteraction in basicInteractionList)
                {
                    if (interactionSucess) return;
                    if (basicInteraction.Interact(playerFacing, transform.position))
                    {
                        interactionSucess = true;
                    }
                }

                if (!interactionSucess) // interactuar con el objeto
                {
                    Attack();
                }
               
            }

            else
            {
                Attack();
            }
        }
    }

    private void Attack()
    {
        animator.Play("Attack"); // animación de ataque creado en el blend tree de unity
        isAttacking = true;
        AttackAnimDirection();
    }

    private void Animations()//animaciones para el movimiento de los diferentes ejes Horizontal y Vertical
    {
        if (isAttacking || Time.timeScale == 0) return; // el jugador cuando interactua con el cartel se detine el movimiento

        if (direction.magnitude != 0) // si la dirección del idle en el persona es diferente a 0 mantiene su postura de idle en cuanto a la dirección
        {
            animator.SetFloat("Horizontal", direction.x); //animaciones de movimiento en x para la derecha(1) izquierda (-1)
            animator.SetFloat("Vertical", direction.y); //animaciones de movimiento en y para arriba(1) para abajo(-1)
            animator.Play("Run");//diferente de cero el personaje se desplaza, animación creada en blend tree de unity
        }
        else animator.Play("Idle");//al no desplazarse queda estático
    }

    private void AttackAnimDirection() // corregir la dirección del movimiento de la espada con el jugador en los ejes x - y
    {
        direction.x = animator.GetFloat("Horizontal");
        direction.y = animator.GetFloat("Vertical");

        if (Mathf.Abs(direction.y) > Mathf.Abs(direction.x))
        {
            direction.x = 0;
        }
        else
        {
            direction.y = 0;
        }
        direction = direction.normalized;

        animator.SetFloat("Horizontal", direction.x);
        animator.SetFloat("Vertical", direction.y);

        direction = Vector2.zero;
    }

    private void EndAttack() // evento para controlar las animaciones de la espada attack, es agregado en animaciones de unity, en add event 
    {
        isAttacking = false;
    }

    private void OnTriggerEnter2D(Collider2D collision) // Colisión con el objeto del corazón
    {
        if (collision.CompareTag("MaxHpUp"))
        {
            Destroy(collision.gameObject); // al tocarlo se destruye el corazón
            gameManager.IncreaseMaxHP();
        }
        else if (collision.CompareTag("Heal") && gameManager.CanHeal()) // si puede tomar más o no
        {
            Destroy(collision.gameObject);
            gameManager.UpdateCurrentHP(4);
        }
        else if (collision.CompareTag("Interaction")) // interación con el jugador
        {
            basicInteractionList.Add(collision.GetComponent<BasicInteration>());
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Interaction")) // interación con el jugador
        {
            basicInteractionList.Remove(collision.GetComponent<BasicInteration>());
        }
    }

    private void OnCollisionEnter2D(Collision2D collision) // jugador colisione con el objeto enemigo
    {
        if (collision.transform.CompareTag("Enemy") && !invincible)// colisión con el arma hacia el enemigo
        {
            gameManager.UpdateCurrentHP(-2);
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
        uncontrollable = true;
        direction = Vector2.zero;
        rigidBody.velocity = (transform.position - hitPosition).normalized * KnockbackStrength;
        yield return new WaitForSeconds(KnockbachTime);
        rigidBody.velocity = Vector3.zero; // se puede Vector2.zero; juego en 2d
        uncontrollable = false;
    }
}      