//using UnityEngine;
//using System.Collections.Generic;

//public class ObstacleHider : MonoBehaviour
//{
//    [Tooltip("가로막는 레이어 (Building 레이어만 포함시키기)")]
//    public LayerMask buildingMask;

//    private Transform target; // 플레이어
//    private List<Renderer> hiddenRenderers = new List<Renderer>();

//    private void Awake()
//    {
//        // 플레이어 객체 찾기
//        GameObject player = GameAssistManager.Instance.GetPlayer();
//        if (player != null)
//        {
//            target = player.transform;
//        }
//        else
//        {
//            Debug.LogError("ObstacleHider: 플레이어를 찾을 수 없습니다.");
//        }
//    }

//    private void LateUpdate()
//    {
//        if (target == null) return;

//        // 이전 프레임에서 비활성화한 렌더러들 다시 켜기
//        foreach (Renderer renderer in hiddenRenderers)
//        {
//            if (renderer != null)
//                renderer.enabled = true;
//        }
//        hiddenRenderers.Clear();

//        // 카메라 → 플레이어 방향과 거리 계산
//        Vector3 direction = target.position - transform.position;
//        float distance = direction.magnitude;

//        // 장애물 감지 (Building 레이어만)
//        RaycastHit[] hits = Physics.RaycastAll(transform.position, direction.normalized, distance, buildingMask);

//        foreach (RaycastHit hit in hits)
//        {
//            GameObject hitObject = hit.collider.gameObject;

//            // 감지된 오브젝트가 Building 레이어인지 다시 한 번 체크 (안전)
//            if (hitObject.layer == LayerMask.NameToLayer("Building"))
//            {
//                // 자신 + 자식들의 모든 Renderer 꺼버리기
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
    [Tooltip("가로막는 레이어 (Building 레이어만 포함시키기)")]
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
            Debug.LogError("ObstacleHider: 플레이어를 찾을 수 없습니다.");
        }
    }

    private void LateUpdate()
    {
        if (target == null) return;

        // 기존 렌더러 복원
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

    // 머테리얼을 투명하게 만들기 (알파 블렌딩)
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

    // 원래대로 불투명하게 복구
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
