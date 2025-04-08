//using UnityEngine;
//using System.Collections.Generic;

//public class ObstacleHider : MonoBehaviour
//{
//    [Tooltip("���θ��� ���̾� (Building ���̾ ���Խ�Ű��)")]
//    public LayerMask buildingMask;

//    private Transform target; // �÷��̾�
//    private List<Renderer> hiddenRenderers = new List<Renderer>();

//    private void Awake()
//    {
//        // �÷��̾� ��ü ã��
//        GameObject player = GameAssistManager.Instance.GetPlayer();
//        if (player != null)
//        {
//            target = player.transform;
//        }
//        else
//        {
//            Debug.LogError("ObstacleHider: �÷��̾ ã�� �� �����ϴ�.");
//        }
//    }

//    private void LateUpdate()
//    {
//        if (target == null) return;

//        // ���� �����ӿ��� ��Ȱ��ȭ�� �������� �ٽ� �ѱ�
//        foreach (Renderer renderer in hiddenRenderers)
//        {
//            if (renderer != null)
//                renderer.enabled = true;
//        }
//        hiddenRenderers.Clear();

//        // ī�޶� �� �÷��̾� ����� �Ÿ� ���
//        Vector3 direction = target.position - transform.position;
//        float distance = direction.magnitude;

//        // ��ֹ� ���� (Building ���̾)
//        RaycastHit[] hits = Physics.RaycastAll(transform.position, direction.normalized, distance, buildingMask);

//        foreach (RaycastHit hit in hits)
//        {
//            GameObject hitObject = hit.collider.gameObject;

//            // ������ ������Ʈ�� Building ���̾����� �ٽ� �� �� üũ (����)
//            if (hitObject.layer == LayerMask.NameToLayer("Building"))
//            {
//                // �ڽ� + �ڽĵ��� ��� Renderer ��������
//                Renderer[] renderers = hitObject.GetComponentsInChildren<Renderer>();
//                foreach (Renderer r in renderers)
//                {
//                    if (r != null && r.enabled)
//                    {
//                        r.enabled = false;
//                        hiddenRenderers.Add(r);
//                    }
//                }
//            }
//        }
//    }
//}



using UnityEngine;
using System.Collections.Generic;

public class ObstacleHider : MonoBehaviour
{
    [Tooltip("���θ��� ���̾� (Building ���̾ ���Խ�Ű��)")]
    public LayerMask buildingMask;

    private Transform target;

    private Dictionary<Renderer, List<Color>> originalColors = new Dictionary<Renderer, List<Color>>();
    private List<Renderer> transparentRenderers = new List<Renderer>();

    [Range(0f, 1f)] public float transparentAlpha = 0.2f;

    private void Awake()
    {
        GameObject player = GameAssistManager.Instance.GetPlayer();
        if (player != null)
        {
            target = player.transform;
        }
        else
        {
            Debug.LogError("ObstacleHider: �÷��̾ ã�� �� �����ϴ�.");
        }
    }

    private void LateUpdate()
    {
        if (target == null) return;

        // ���� ������ ����
        foreach (Renderer renderer in transparentRenderers)
        {
            if (renderer != null && originalColors.ContainsKey(renderer))
            {
                Material[] materials = renderer.materials;
                List<Color> originalColorList = originalColors[renderer];

                for (int i = 0; i < materials.Length; i++)
                {
                    if (i < originalColorList.Count)
                    {
                        Color color = originalColorList[i];
                        materials[i].color = color;
                        SetMaterialToOpaque(materials[i]);
                    }
                }
            }
        }

        transparentRenderers.Clear();
        originalColors.Clear();


        float backOffset = 2f;
        Vector3 rayOrigin = transform.position - transform.forward * backOffset;

        Vector3 direction = target.position - rayOrigin;
        float distance = direction.magnitude;

        RaycastHit[] hits = Physics.RaycastAll(rayOrigin, direction.normalized, distance, buildingMask);

        foreach (RaycastHit hit in hits)
        {
            GameObject hitObject = hit.collider.gameObject;

            if (hitObject.layer == LayerMask.NameToLayer("Building"))
            {
                Renderer[] renderers = hitObject.GetComponentsInChildren<Renderer>();
                foreach (Renderer r in renderers)
                {
                    if (r != null && r.enabled)
                    {
                        Material[] materials = r.materials;
                        List<Color> colorBackup = new List<Color>();

                        foreach (Material mat in materials)
                        {
                            colorBackup.Add(mat.color);
                            SetMaterialToTransparent(mat);
                            Color c = mat.color;
                            c.a = transparentAlpha;
                            mat.color = c;
                        }

                        if (!originalColors.ContainsKey(r))
                        {
                            originalColors.Add(r, colorBackup);
                            transparentRenderers.Add(r);
                        }
                    }
                }
            }
        }
    }

    // ���׸����� �����ϰ� ����� (���� ����)
    private void SetMaterialToTransparent(Material mat)
    {
        mat.SetFloat("_Mode", 3);
        mat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
        mat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        mat.SetInt("_ZWrite", 0);
        mat.DisableKeyword("_ALPHATEST_ON");
        mat.EnableKeyword("_ALPHABLEND_ON");
        mat.DisableKeyword("_ALPHAPREMULTIPLY_ON");
        mat.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Transparent;
    }

    // ������� �������ϰ� ����
    private void SetMaterialToOpaque(Material mat)
    {
        mat.SetFloat("_Mode", 0);
        mat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
        mat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
        mat.SetInt("_ZWrite", 1);
        mat.EnableKeyword("_ALPHATEST_ON");
        mat.DisableKeyword("_ALPHABLEND_ON");
        mat.DisableKeyword("_ALPHAPREMULTIPLY_ON");
        mat.renderQueue = -1;
    }
}
