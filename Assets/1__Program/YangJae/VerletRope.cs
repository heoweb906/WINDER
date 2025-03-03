using UnityEngine;
using UnityEngine.Serialization;

public struct VerletNode
{
    public Vector3 Position;
    public Vector3 PrevoiusPosition;
}

public class VerletRope : MonoBehaviour
{
    private VerletNode[] m_VerletNodes;
    [SerializeField] private float m_RopeLength;
    [SerializeField] private int m_NumberOfNodes;
    [SerializeField] private int m_ConstraintIterationCount;
    [SerializeField] private Vector3 m_Gravity;
    private float m_DistanceBetweenNodes;
    [SerializeField] private float m_RopeRadius;
    [SerializeField] private int m_SubSteps = 4;

    private RopeRenderer m_RopeRenderer;

    [SerializeField] private bool m_IsEndFixed = false;
    [SerializeField] private GameObject m_EndPoint; // 빈 오브젝트를 끝점으로 설정

    private void Awake()
    {
        m_VerletNodes = new VerletNode[(int)(m_NumberOfNodes)];
        m_DistanceBetweenNodes = m_RopeLength / m_NumberOfNodes;

        for (int i = 0; i < m_VerletNodes.Length; i++)
        {
            m_VerletNodes[i].Position = transform.position - new Vector3(0f, (m_DistanceBetweenNodes * i), 0f);
            m_VerletNodes[i].PrevoiusPosition = m_VerletNodes[i].Position;
        }
    }

    private void Start()
    {
        m_RopeRenderer = GetComponent<RopeRenderer>();
    }

    private void FixedUpdate()
    {
        for (int step = 0; step < m_SubSteps; step++)
        {
            CalculateNewPositions(Time.fixedDeltaTime / m_SubSteps);
            for (int i = 0; i < m_ConstraintIterationCount; i++)
            {
                FixNodeDistances();
                if (i % 2 == 0)
                    ApplyCollision();
            }
        }
        
        m_RopeRenderer.RenderRope(m_VerletNodes, m_RopeRadius);
    }

    // private void OnDrawGizmos()
    // {
    //     if (!Application.isPlaying)
    //         return;
    //
    //     for (int i = 0; i < m_VerletNodes.Length; i++)
    //     {
    //         if (i != m_VerletNodes.Length - 1)
    //         {
    //             Gizmos.color = Color.green;
    //             Gizmos.DrawLine(m_VerletNodes[i].Position, m_VerletNodes[i + 1].Position);
    //         }
    //         
    //         Gizmos.color = Color.blue;
    //         Gizmos.DrawLine(m_VerletNodes[i].Position, m_VerletNodes[i].PrevoiusPosition);
    //     }
    // }

    private void CalculateNewPositions(float deltaTime)
    {
        Vector3 gravityStep = m_Gravity * (deltaTime * deltaTime);

        for (int i = 0; i < m_VerletNodes.Length; i++)
        {
            var currNode = m_VerletNodes[i];
            var newPreviousPosition = currNode.Position;

            var newPosition = (2 * currNode.Position) - currNode.PrevoiusPosition + gravityStep;

            Vector3 direction = newPosition - currNode.Position;
            float distance = direction.magnitude;
            direction.Normalize();

            if (Physics.SphereCast(currNode.Position, m_RopeRadius, direction, out RaycastHit hit, distance))
            {
                newPosition = hit.point + hit.normal * m_RopeRadius;
            }

            m_VerletNodes[i].PrevoiusPosition = newPreviousPosition;
            m_VerletNodes[i].Position = newPosition;
        }
    }

    private void FixNodeDistances()
{
    m_VerletNodes[0].Position = transform.position; // 첫 번째 노드 고정

    for (int i = 0; i < m_VerletNodes.Length - 1; i++)
    {
        var n1 = m_VerletNodes[i];
        var n2 = m_VerletNodes[i + 1];

        var d1 = n1.Position - n2.Position;
        var d2 = d1.magnitude;
        var d3 = (d2 - m_DistanceBetweenNodes) / d2;

        m_VerletNodes[i].Position -= (d1 * (0.5f * d3));
        m_VerletNodes[i + 1].Position += (d1 * (0.5f * d3));
    }

    if (m_IsEndFixed && m_EndPoint != null)
    {
        m_VerletNodes[m_VerletNodes.Length - 1].Position = m_EndPoint.transform.position;
    }
}

    private void ApplyCollision()
    {
        for (int i = 0; i < m_VerletNodes.Length; i++)
        {
            ResolveCollision(ref m_VerletNodes[i]);
        }
        ResolveSelfCollision();
    }

    private void ResolveCollision(ref VerletNode node)
    {
        var colliders = Physics.OverlapSphere(node.Position, m_RopeRadius);

        foreach (var col in colliders)
        {
            if (col.isTrigger) continue;

            Vector3 closestPoint = col.ClosestPoint(node.Position);
            float distance = Vector3.Distance(node.Position, closestPoint);

            if (distance < m_RopeRadius)
            {
                Vector3 penetrationNormal = (node.Position - closestPoint).normalized;
                float penetrationDepth = m_RopeRadius - distance;

                node.Position += penetrationNormal * penetrationDepth * 1.01f;
            }
        }
    }

    private void ResolveSelfCollision()
{
    for (int i = 0; i < m_VerletNodes.Length; i++)
    {
        for (int j = i + 2; j < m_VerletNodes.Length; j++) // 인접 노드는 제외
        {
            float distance = Vector3.Distance(m_VerletNodes[i].Position, m_VerletNodes[j].Position);

            if (distance < m_RopeRadius * 2)
            {
                Vector3 direction = (m_VerletNodes[i].Position - m_VerletNodes[j].Position).normalized;
                float penetrationDepth = (m_RopeRadius * 2) - distance;

                m_VerletNodes[i].Position += direction * (penetrationDepth * 0.5f);
                m_VerletNodes[j].Position -= direction * (penetrationDepth * 0.5f);
            }
        }
    }
}

    public int GetNodeCount()
    {
        return m_VerletNodes.Length;
    }
}
