using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntitySceneControl : MonoBehaviour
{
    public void StopAllEntitiesScene(Vector3 scenePosition) // Detener a los enemigos en una determinada escena en su posición
    {
        GameObject scene = FindSceneByPosition(scenePosition);

        EnemyHealth[] enemyHealthArray = scene.GetComponentsInChildren<EnemyHealth>(); // Es donde se va a derivar todos los enemigos Hijos de primer nivel
        NPCRandomPatrol[] NPCArray = scene.GetComponentsInChildren<NPCRandomPatrol>();
        foreach (EnemyHealth enemyHealth in enemyHealthArray) // busacar cuales tienen el script de EnemyHealth
        {
            enemyHealth.StopBehaviour(); // Comportamiento del enemigo para detenerlo
        }
        foreach (NPCRandomPatrol npc in NPCArray) // busacar cuales tienen el script de EnemyHealth
        {
            npc.StopBehaviour(); // Comportamiento del enemigo para detenerlo
        }
    }

    public void ActiveAllEntitiesScene(Vector3 scenePosition) // Activar a los enemigos en una determinada escena en su posición
    {
        GameObject scene = FindSceneByPosition(scenePosition);

        EnemyHealth[] enemyHealthArray = scene.GetComponentsInChildren<EnemyHealth>(); // Es donde se va a derivar todos los enemigos Hijos de primer nivel
        NPCRandomPatrol[] NPCArray = scene.GetComponentsInChildren<NPCRandomPatrol>();
        foreach (EnemyHealth enemyHealth in enemyHealthArray) // busacar cuales tienen el script de EnemyHealth
        {
            enemyHealth.ContinueBehaviour(); // Comportamiento del enemigo
        }
        foreach (NPCRandomPatrol npc in NPCArray) // busacar cuales tienen el script de EnemyHealth
        {
            npc.ContinueBehaviour(); // Comportamiento del enemigo 
        }
    }

    public void ResetPositionEntitiesScene(Vector3 scenePosition) // Activar a los enemigos en una determinada escena en su posición
    {
        GameObject scene = FindSceneByPosition(scenePosition);

        EnemyHealth[] enemyHealthArray = scene.GetComponentsInChildren<EnemyHealth>(); // Es donde se va a derivar todos los enemigos Hijos de primer nivel
        NPCRandomPatrol[] NPCArray = scene.GetComponentsInChildren<NPCRandomPatrol>();
        foreach (EnemyHealth enemyHealth in enemyHealthArray) // busacar cuales tienen el script de EnemyHealth
        {
            enemyHealth.ResetPosition(); // Comportamiento del enemigo
        }
        foreach (NPCRandomPatrol npc in NPCArray) // busacar cuales tienen el script de EnemyHealth
        {
            npc.ResetPosition(); // Comportamiento del enemigo 
        }

    }

    private GameObject FindSceneByPosition(Vector3 scenePosition)
    {
        foreach(Transform child in transform) // Buscar los hijos del propio objeto que es Rooms ( Town - Snow - Fields )
        {
            if (scenePosition.x == child.transform.position.x 
                && scenePosition.y == child.transform.position.y) // comprobamos las cordenadas de x - y
            {
                return child.gameObject;
            }
        }

        return null;
    }
}
