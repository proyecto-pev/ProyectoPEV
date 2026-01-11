using UnityEngine;

[RequireComponent(typeof(Outline))]
public class OutlineDistanceController : MonoBehaviour
{
    [Header("Configuración de distancia")]
    [SerializeField] private float distanciaMaxima = 10f;
    [SerializeField] private float distanciaMinima = 3f;
    [SerializeField] private bool usarFrustumCulling = true;

    [Header("Referencias")]
    [SerializeField] private Transform jugadorTransform;
    [SerializeField] private Outline outlineComponent;

    private Camera camaraPrincipal;
    private bool outlineEstadoAnterior = true;

    void Start()
    {
        // Obtener referencias
        if (outlineComponent == null)
        {
            outlineComponent = GetComponent<Outline>();
        }

        // Buscar cámara principal
        camaraPrincipal = Camera.main;

        // Buscar jugador si no está asignado
        if (jugadorTransform == null)
        {
            GameObject jugador = GameObject.FindGameObjectWithTag("Player");
            if (jugador != null)
            {
                jugadorTransform = jugador.transform;
            }
            else
            {
                jugador = GameObject.Find("Player");
                if (jugador != null)
                {
                    jugadorTransform = jugador.transform;
                }
            }
        }

        if (jugadorTransform == null)
        {
            Debug.LogWarning("No se encontró al jugador. Desactivando control por distancia.", this);
            enabled = false;
        }

        // Guardar estado inicial
        outlineEstadoAnterior = outlineComponent.enabled;
    }

    void Update()
    {
        if (jugadorTransform == null || outlineComponent == null) return;

        // Calcular distancia al jugador
        float distancia = Vector3.Distance(transform.position, jugadorTransform.position);

        // Verificar si está dentro del frustum
        bool estaEnCamara = true;
        if (usarFrustumCulling && camaraPrincipal != null)
        {
            estaEnCamara = EstaDentroDelFrustum();
        }

        // Determinar si el outline debe estar activo
        bool outlineDebeEstarActivo = estaEnCamara &&
                                     distancia >= distanciaMinima &&
                                     distancia <= distanciaMaxima;

        // Actualizar solo si hay cambio
        if (outlineDebeEstarActivo != outlineEstadoAnterior)
        {
            outlineComponent.enabled = outlineDebeEstarActivo;
            outlineEstadoAnterior = outlineDebeEstarActivo;
        }
    }

    bool EstaDentroDelFrustum()
    {
        if (camaraPrincipal == null) return true;

        // Obtener los planos del frustum
        Plane[] planos = GeometryUtility.CalculateFrustumPlanes(camaraPrincipal);

        // Obtener el bounds del objeto
        Renderer renderer = GetComponent<Renderer>();
        if (renderer != null)
        {
            return GeometryUtility.TestPlanesAABB(planos, renderer.bounds);
        }

        // Si no hay renderer, usar posición del objeto
        return GeometryUtility.TestPlanesAABB(planos, new Bounds(transform.position, Vector3.one));
    }

    // Métodos públicos para control externo
    public void ForzarActivacion()
    {
        outlineComponent.enabled = true;
        outlineEstadoAnterior = true;
    }

    public void ForzarDesactivacion()
    {
        outlineComponent.enabled = false;
        outlineEstadoAnterior = false;
    }

    public void ConfigurarDistancias(float min, float max)
    {
        distanciaMinima = min;
        distanciaMaxima = max;
    }
}