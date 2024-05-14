using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCRandomPatrol : MonoBehaviour
{
    [Header("RandomPatrol parameters")]
    public float speed;
    public float minPatrolTime; // tiempo de patrulla
    public float maxPatrolTime;
    public float minWaitTime; // tiempo de espera
    public float maxWaitTime;

    Animator animator;
    Rigidbody2D rigidBody;

    Vector2 direction;
    Vector2 initialPosicition;

    private void Awake() //Awake una función antes de star paea evitar error al ejecutar varios start
    
        {
        animator = GetComponent<Animator>();
        rigidBody = GetComponent<Rigidbody2D>();

        initialPosicition = transform.position;

        direction = RandomDirection(); // direcciones aleatorias
        animator.SetFloat("Horizontal", direction.x); // animaciones de movimiento en x para la derecha(1) izquierda (-1)
        animator.SetFloat("Vertical", direction.y); // animaciones de movimiento en y para arriba(1) para abajo(-1)
    }

    IEnumerator Patrol()
    {
        direction = RandomDirection(); // direcciones aleatorias
        Animations(); // animación en cuanto a la dirección
        yield return new WaitForSeconds(Random.Range(minPatrolTime, maxPatrolTime)); // tiempo de animación de random patrulla

        direction = Vector2.zero; // andar y detenerse
        Animations(); // animaciones andar y detenerse
        yield return new WaitForSeconds(Random.Range(minWaitTime, maxWaitTime)); // tiempo de animación de random espera

        StartCoroutine(Patrol());
    }

    private Vector2 RandomDirection()
    {
        int x = Random.Range(0, 8); // rango de 8 direcciones y puede ser de cuatro 4 y solo moveria arriba, abajo, izquierda y derecha

        return x switch // en el parametro x, devolver el swith de 8 direcciones
        {
            0 => Vector2.up,
            1 => Vector2.down,
            2 => Vector2.left,
            3 => Vector2.right, // este quedaria como default _
            4 => new Vector2(1, 1), // al borrar estos cuatro direcciones de new
            5 => new Vector2(1, -1),
            6 => new Vector2(-1, 1),
            _ => new Vector2(-1, -1),
        };
    }

    public void FacePlayer(Vector2 playerPost)
    {
        float x = playerPost.x - transform.position.x;
        float y = playerPost.y - transform.position.y;

        if(Mathf.Abs(x) >  Mathf.Abs(y)) // Npc mire en las 4 direcciones
        {
            if (x > 0) direction = Vector2.right;
            else direction = Vector2.left;
        }
        else
        {
            if (y > 0) direction = Vector2.up;
            else direction = Vector2.down;
        }

        animator.SetFloat("Horizontal", x); // gira el personaje hacia la dirección requerida
        animator.SetFloat("Vertical", y); // gira el personaje hacia la dirección requerida
        direction = Vector2.zero;
        Animations();
    }

    private void Animations() // animaciones para el movimiento de los diferentes ejes Horizontal y Vertical
    {
        if (direction.magnitude != 0) // si la dirección del idle en el persona es diferente a 0 mantiene su postura de idle en cuanto a la dirección
        {
            animator.SetFloat("Horizontal", direction.x); // animaciones de movimiento en x para la derecha(1) izquierda (-1)
            animator.SetFloat("Vertical", direction.y); // animaciones de movimiento en y para arriba(1) para abajo(-1)
            animator.Play("Run"); // diferente de cero el personaje se desplaza, animación creada en blend tree de unity
        }
        else animator.Play("Idle"); // al no desplazarse queda estático

        rigidBody.velocity = direction.normalized * speed;
    }

    public void StopBehaviour() // sobre escribir la funcion
    {
        StopAllCoroutines();
        direction = Vector2.zero;
        Animations();
    }

    public void ContinueBehaviour()
    {
        StartCoroutine(Patrol());
    }

    public void ResetPosition()  // Reinicie la posición
    {
        transform.position = initialPosicition;
        
    }
}