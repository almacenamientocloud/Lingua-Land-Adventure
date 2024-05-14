using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

public class CameraController : MonoBehaviour
{
    Transform player;// trnasform del jugador, playerMovement

    public float yDistance = 6f; // distancia del jugador de la camara tomamos la mitad del yMovement
    public float yMovement = 12f; //resolución de la camara en verticar una vez sale el recorrido de la camara por los cuadritos de pantalla números de Sprites

    public float xDistance = 11f; //distancia del jugador de la camara tomamos la mitad del xMovement
    public float xMovement = 22f; //resolución de la camara en horizontal una vez sale el recorrido de la camara por los cuadritos de pantalla números de Sprites

    //public Vector3 cameraOrigen; // Posición de la camara antes de iniciar la transición
    public Vector3 cameraDestination; // El nuevo lugar de destino de la posición del jugador, cordenadas.

    public float movementTime = 0.5f; //la velocidad con la que se desplaza la camara
    public bool isMoving; //variable de movimiento de control para la camara

    EntitySceneControl entitySceneControl;

    void Start()
    {
        player = FindObjectOfType<PlayerMovement>().transform;
        entitySceneControl = FindObjectOfType<EntitySceneControl>();
        entitySceneControl.ActiveAllEntitiesScene(transform.position);
    }

    void Update()
    {
        if (!isMoving)
        {
            if (player.position.y - transform.position.y >= yDistance)// si el jugador menos la posición de la camara en el eje y es mayor o igual cambia de mapa al desplazarse por arriba
            {
                cameraDestination += new Vector3(0, yMovement, 0);
                StartCoroutine(MoveCamera());
            }
            else if (transform.position.y - player.position.y >= yDistance)// si la posición de la camara menos la posición del jugador en el eje y es mayor o igual cambia de mapa al desplazarse por abajo
            {
                cameraDestination -= new Vector3(0, yMovement, 0);
                StartCoroutine(MoveCamera());
            }
            else if (player.position.x - transform.position.x >= xDistance)// si el jugador menos la posición de la camara en el eje x es mayor o igual cambia de mapa al desplazarse por la derecha
            {
                cameraDestination += new Vector3(xMovement, 0, 0);
                StartCoroutine(MoveCamera());
            }
            else if (transform.position.x - player.position.x >= xDistance)// si la posición de la camara menos la posición del jugador en el eje x es mayor o igual cambia de mapa al desplazarse por la izquierda
            {
                cameraDestination -= new Vector3(xMovement, 0, 0);
                StartCoroutine(MoveCamera());
            }
        }  
        
    }
    IEnumerator MoveCamera() // ayuda al motor en cuanto al frame
    {
        isMoving = true; // al empezar la Coroutine la camara se esta moviendo
        var currentPos = transform.position; // variable con la posicón actual de la camara
        var t = 0f; // camara esta en el origen

        entitySceneControl.StopAllEntitiesScene(currentPos); // Psición la cual la camara parte

        while (t < 1) // camara esta en el destino
        {
            t += Time.deltaTime / movementTime; // Velocidad del movimiento que le pongamos en movementTime (0.5f)
            transform.position = Vector3.Lerp(currentPos, cameraDestination, t); // la camara se mueve de la posición inicial a la posición final que es la que le correspode ( 24 cuadritos en x y 12 cuadritos en y)
            transform.position = new Vector3(transform.position.x, transform.position.y, currentPos.z); // Se mueve por cada nodo o cuadrito dependiendo dem su eje x o y (mantener el valor -10 para no perder la camara en z-10)
            yield return null; // Simplemente espera hasta el próximo frame de pantalla antes de continuar.
        }
        isMoving = false; // la camara no se esta moviendo

        entitySceneControl.ResetPositionEntitiesScene(currentPos);  // Reinicia la enemigo
        entitySceneControl.ActiveAllEntitiesScene(cameraDestination); // Activa la camara al estar con los enemigos
    }  
}
