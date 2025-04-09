using UnityEngine;
using UnityEngine.Rendering;

namespace FlatKit {
public static class RendererFeatureUtils {
    public static void SetKeyword(Material material, string keyword, bool enabled)
     {
            if (material.shader != null)
            {
                try
                {
                    var localKeyword = new LocalKeyword(material.shader, keyword);
                    material.SetKeyword(localKeyword, enabled);
                }
                catch
                {
                    // LocalKeyword 생성 실패 시 fallback
                    if (enabled)
                        material.EnableKeyword(keyword);
                    else
                        material.DisableKeyword(keyword);
                }
            }
            else
            {
                if (enabled)
                    material.EnableKeyword(keyword);
                else
                    material.DisableKeyword(keyword);
            }
        }
}
}






//if (material.shader != null)
//{
//    material.SetKeyword(new LocalKeyword(material.shader, keyword), enabled);
//}
//else
//{
//    if (enabled)
//    {
//        material.EnableKeyword(keyword);
//    }
//    else
//    {
//        material.DisableKeyword(keyword);
//    }
//}