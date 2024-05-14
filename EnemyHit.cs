using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHit : MonoBehaviour
{
    EnemyHealth enemyHealth; // Referencia
    Animator animator;

    private void Start()
    {
        enemyHealth = GetComponentInParent<EnemyHealth>(); // componenetes del padre
        animator = GetComponent<Animator>();
    }

    public void Defeat()
    {
        animator.Play("Death");
    }

    private void Hide() // Para ocultar el spriteRenderer del objeto (enemigo)
    {
        enemyHealth.HideEnemy(); // llamamos a HideEnemy        
    }

    private void Destroy() // Destruir el enemigo (objeto)
    {
        Destroy(enemyHealth.gameObject);
    }
    
            
}
