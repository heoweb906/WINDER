using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;

#if (UNITY_EDITOR_WIN)

namespace RayFire
{
    
    public class ShatterBatch
    {
        Transform       sourceTm;
        List<Renderer>  sourceRends;
        Transform       targetRoot;
        List<Transform> targetFrags;

        
        // Preview Target scale/color
        // Fragment to target location
        // Load target frag properties
    }
    
    public class RFEngine
    {
        public List<Mesh>                         uMeshes     = new List<Mesh>();
        public List<Utils.Mesh>                   rfMeshes    = new List<Utils.Mesh>();
        public List<Renderer>                     renderers   = new List<Renderer>();
        public Dictionary<GameObject, GameObject> oldNewGoMap = new Dictionary<GameObject, GameObject>();
        public GameObject                         RootGO;

        /// /////////////////////////////////
        /// Constructor
        /// /////////////////////////////////
        
        public RFEngine()
        {
            uMeshes     = new List<Mesh>();
            rfMeshes    = new List<Utils.Mesh>();
            renderers   = new List<Renderer>();
            oldNewGoMap = new Dictionary<GameObject, GameObject>();
        }

        public void SetEngineData (GameObject go, Transform[] slicePlanes)
        {
            Clear();
            AddFromGO (go, slicePlanes);
        }
        
        RFEngine(GameObject go, Transform[] slicePlanes)
        {
            AddFromGO (go, slicePlanes);
        }
        
        void AddFromGO(GameObject go, Transform[] excluding)
        {
            Clear();
            Dictionary<Transform, bool> dictionary = new Dictionary<Transform, bool>();
            if (excluding != null)
            {
                for (int index1 = 0; index1 < excluding.Length; ++index1)
                {
                    Transform[] componentsInChildren = excluding[index1].gameObject.GetComponentsInChildren<Transform>();
                    if (dictionary.ContainsKey (excluding[index1]) == false)
                        dictionary.Add (excluding[index1], false);
                    
                    for (int index2 = 0; index2 < componentsInChildren.Length; ++index2)
                        if (dictionary.ContainsKey (componentsInChildren[index2]) == false)
                            dictionary.Add (componentsInChildren[index2], false);
                }
            }
            Renderer[] componentsInChildren1 = go.GetComponentsInChildren<Renderer>();
            for (int index = 0; index < componentsInChildren1.Length; ++index)
                if (dictionary.ContainsKey (componentsInChildren1[index].transform) == false)
                    Add (componentsInChildren1[index]);
        }
        
        void Add(Renderer rdr)
        {
            Mesh um = null;
            if (rdr.GetType() == typeof(MeshRenderer))
                um = rdr.gameObject.GetComponent<MeshFilter>().sharedMesh;
            else if (rdr.GetType() == typeof(SkinnedMeshRenderer))
                um = ((SkinnedMeshRenderer)rdr).sharedMesh;
            if (um.vertexCount <= 0)
                return;
            uMeshes.Add (um);
            rfMeshes.Add (new Utils.Mesh (um));
            renderers.Add (rdr);
        }

        void Clear()
        {
            uMeshes.Clear();
            renderers.Clear();
            rfMeshes.Clear();
            oldNewGoMap.Clear();
        }
        
        /// /////////////////////////////////
        /// Methods
        /// /////////////////////////////////
        
        Utils.Mesh[][] GetBakedWorldTransformMeshes()
        {
            Utils.Mesh[][] worldTransformMeshes = new Utils.Mesh[uMeshes.Count][];
            for (int index = 0; index < uMeshes.Count; ++index)
            {
                worldTransformMeshes[index]    = new Utils.Mesh[1];
                worldTransformMeshes[index][0] = new Utils.Mesh (uMeshes[index]);
                if (IsSkinnedMeshRenderer (index) == false)
                    worldTransformMeshes[index][0].Transform (GetWorldMatrix (index));
                else
                    worldTransformMeshes[index][0].TransformByBones (uMeshes[index].bindposes, ((SkinnedMeshRenderer)renderers[index]).bones, false);
            }
            return worldTransformMeshes;
        }

        void UnBakeWorldTransform(Utils.Mesh[][] bakedMeshes)
        {
            for (int index1 = 0; index1 < bakedMeshes.Length; ++index1)
            {
                for (int index2 = 0; index2 < bakedMeshes[index1].Length; ++index2)
                {
                    if (IsSkinnedMeshRenderer (index1) == false)
                        bakedMeshes[index1][index2].Transform (GetWorldMatrix (index1).inverse);
                    else
                        bakedMeshes[index1][index2].TransformByBones (uMeshes[index1].bindposes, ((SkinnedMeshRenderer)renderers[index1]).bones, true);
                }
            }
        }

        bool IsSkinnedMeshRenderer(int i)
        {
            return renderers[i].GetType() == typeof(SkinnedMeshRenderer);
        }

        Matrix4x4 GetWorldMatrix(int i)
        {
            return renderers[i].transform.localToWorldMatrix;
        }

        Material[] GetMaterials(int i)
        {
            return renderers[i].sharedMaterials;
        }

        string GetGroupName(int i)
        {
            return renderers[i].name;
        }

        Matrix4x4 CloneHierarchy(GameObject originalObj, bool inheritScale)
        {
            // Create instance
            RootGO      = Object.Instantiate (originalObj);
            RootGO.name = RootGO.name.Remove (RootGO.name.Length - 7, 7);
            
            // Set position and rotation
            RootGO.transform.position = originalObj.transform.position;
            RootGO.transform.rotation = originalObj.transform.rotation;
            
            Transform[] originalTms = originalObj.GetComponentsInChildren<Transform>();
            Transform[] instanceTms = RootGO.GetComponentsInChildren<Transform>();
            
            // Destroy renderers and meshfilters. Collect old new go map
            oldNewGoMap.Clear();
            for (int index = 0; index < originalTms.Length; ++index)
            {
                // Destroy renderer on instance
                Renderer renderer = instanceTms[index].gameObject.GetComponent<Renderer>();
                if (renderer != null)
                    Object.DestroyImmediate (renderer);
                
                // Destroy meshfilter on instance
                MeshFilter meshFIlter = instanceTms[index].gameObject.GetComponent<MeshFilter>();
                if (meshFIlter != null)
                    Object.DestroyImmediate (meshFIlter);
                
                // Set normal scale for roots TODO
                // if (inheritScale == false) instanceTms[index].localScale = new Vector3(1, 1, 1);
                
                // Collect oldNewMap
                oldNewGoMap.Add (originalTms[index].gameObject, instanceTms[index].gameObject);
            }
            
            // Scale matrix for fragments rescale
            Matrix4x4 origScaleMat = Matrix4x4.identity;
            
            // Set normal scale to fragments roots
            if (inheritScale == false)
            {
                // Set normal scale. This will offset fragment roots
                RootGO.transform.localScale = new Vector3(1, 1, 1);
                
                // Set fragments root position back by corresponding object in original hierarchy
                Transform[] originalChildren = originalObj.GetComponentsInChildren<Transform>();
                for (int i = 0; i < originalChildren.Length; i++)
                    if (originalChildren[i].parent != null && originalChildren[i].parent.gameObject == originalObj)
                        oldNewGoMap[originalChildren[i].gameObject].transform.position = originalChildren[i].transform.position;
                
                // Set matrix for fragments scale
                origScaleMat.SetTRS(new Vector3(0, 0, 0), Quaternion.identity, originalObj.transform.lossyScale);
                
                return origScaleMat;
            }           
           
            // Inherit scale from original roots
            RootGO.transform.localScale = originalObj.transform.lossyScale;
            
            return origScaleMat;
        }

        // Get scale matrix
        Matrix4x4 GetScaleMatrix(GameObject originalObj, bool inheritScale)
        {
            // Scale matrix for fragments rescale
            Matrix4x4 origScaleMat = Matrix4x4.identity;
            
            // Set normal scale to fragments roots
            if (inheritScale == false)
            {
                // Set matrix for fragments scale
                origScaleMat.SetTRS(new Vector3(0, 0, 0), Quaternion.identity, originalObj.transform.lossyScale);
                return origScaleMat;
            }           
           
            // Inherit scale from original roots
            RootGO.transform.localScale = originalObj.transform.lossyScale;
            return origScaleMat;
        }
        
        public GameObject BuildHierarchy(GameObject ShatterGO, int meshID)
        {
            GetChain(ShatterGO, renderers[meshID].gameObject, ref oldNewGoMap, ref RootGO);
            if (IsSkinnedMeshRenderer(meshID) == true)
            {
                SkinnedMeshRenderer renderer = (SkinnedMeshRenderer) renderers[meshID];
                GetChain(ShatterGO, renderer.rootBone.gameObject, ref oldNewGoMap, ref RootGO);
                foreach (Component bone in renderer.bones)
                    GetChain(ShatterGO, bone.gameObject, ref oldNewGoMap, ref RootGO);
            }
            return oldNewGoMap[renderers[meshID].gameObject];
        }
        
        private static void GetChain(GameObject Top, GameObject Bot, ref Dictionary<GameObject, GameObject> oldNewGo, ref GameObject RootGO)
        {
            GameObject gameObject1 = null;
            GameObject gameObject2;
            while (true)
            {
                if (oldNewGo.ContainsKey(Bot) == true)
                {
                    gameObject2 = oldNewGo[Bot];
                }
                else
                {
                    gameObject2                         = new GameObject(Bot.name);
                    gameObject2.transform.localPosition = Bot.transform.localPosition;
                    gameObject2.transform.localRotation = Bot.transform.localRotation;
                    gameObject2.transform.localScale    = Bot.transform.localScale;
                    oldNewGo.Add(Bot, gameObject2);
                }
                if (gameObject1 != null)
                    gameObject1.transform.SetParent(gameObject2.transform, false);
                if (!(Bot == Top))
                {
                    gameObject1 = gameObject2;
                    Bot         = Bot.transform.parent.gameObject;
                }
                else
                    break;
            }
            RootGO = gameObject2;
        }

        GameObject GetGroupGO(int i)
        {
            return oldNewGoMap[renderers[i].gameObject];
        }

        Transform[] GetBonesTNS(int i)
        {
            Transform[] bonesTns = null;
            if (IsSkinnedMeshRenderer (i))
            {
                Transform[] bones = ((SkinnedMeshRenderer)this.renderers[i]).bones;
                bonesTns = new Transform[bones.Length];
                for (int index = 0; index < bones.Length; ++index)
                    bonesTns[index] = this.oldNewGoMap[bones[index].gameObject].transform;
            }
            return bonesTns;
        }

        Transform GetRootBone(int i)
        {
            Transform rootBone = (Transform)null;
            if (this.IsSkinnedMeshRenderer (i))
                rootBone = this.oldNewGoMap[((SkinnedMeshRenderer)this.renderers[i]).rootBone.gameObject].transform;
            return rootBone;
        }

        void SynchronizeAnimTime(GameObject ShatterGO)
        {
            Animation[] componentsInChildren1 = ShatterGO.GetComponentsInChildren<Animation>();
            for (int index1 = 0; index1 < componentsInChildren1.Length; ++index1)
            {
                if (oldNewGoMap.ContainsKey (componentsInChildren1[index1].gameObject))
                {
                    Animation animation1 = componentsInChildren1[index1];
                    Object.DestroyImmediate (oldNewGoMap[animation1.gameObject].GetComponent<Animation>());
                    Animation animation2 = oldNewGoMap[componentsInChildren1[index1].gameObject].AddComponent<Animation>();
                    animation2.clip = animation1.clip;
                    List<float> floatList = new List<float>();
                    foreach (AnimationState animationState in animation1)
                    {
                        floatList.Add (animationState.time);
                        animation2.AddClip (animationState.clip, animationState.name);
                    }
                    int index2 = 0;
                    foreach (AnimationState animationState in animation2)
                    {
                        animationState.time = floatList[index2];
                        ++index2;
                    }
                    animation2.playAutomatically = animation1.playAutomatically;
                    animation2.animatePhysics    = animation1.animatePhysics;
                    animation2.cullingType       = animation1.cullingType;
                    animation2.Play();
                }
            }
            Animator[] componentsInChildren2 = ShatterGO.GetComponentsInChildren<Animator>();
            for (int index3 = 0; index3 < componentsInChildren2.Length; ++index3)
            {
                if (oldNewGoMap.ContainsKey (componentsInChildren2[index3].gameObject))
                {
                    Animator animator  = componentsInChildren2[index3];
                    Animator component = oldNewGoMap[componentsInChildren2[index3].gameObject].GetComponent<Animator>();
                    for (int index4 = 0; index4 < animator.layerCount; ++index4)
                    {
                        AnimatorStateInfo animatorStateInfo = animator.GetCurrentAnimatorStateInfo (index4);
                        component.Play (animatorStateInfo.fullPathHash, index4, animatorStateInfo.normalizedTime);
                    }
                }
            }
        }
        
        Utils.MeshMaps[] GetMaps()
        {
            Utils.MeshMaps[] maps = new Utils.MeshMaps[this.uMeshes.Count];
            for (int index = 0; index < this.uMeshes.Count; ++index)
            {
                maps[index] = new Utils.MeshMaps();
                maps[index].SetNormals(this.uMeshes[index].normals);
                maps[index].SetTexCoords (this.uMeshes[index]);
                maps[index].SetVertexColors(this.uMeshes[index].colors);
            }
            return maps;
        }

        static Utils.MeshMaps[][] ComputeMaps(RFEngine origMeshInfo, Utils.Mesh[][] frags, Matrix4x4 normalMat, float uvScale, Vector2 uvAreaBegin, Vector2 uvAreaEnd, Color innerColor, bool smoothInner) 
        {
            Utils.Mesh[]     origMeshes = origMeshInfo.rfMeshes.ToArray();
            Utils.MeshMaps[] origMaps = origMeshInfo.GetMaps();
            float            num   = Math.Max(normalMat.lossyScale.x, Math.Max(normalMat.lossyScale.y, normalMat.lossyScale.z));
            uvScale = ((double) num == 0.0 
                ? 0.0f 
                : 1f / num) * uvScale;
            Utils.MeshMaps[][] maps2 = new Utils.MeshMaps[frags.Length][];
            for (int index1 = 0; index1 < frags.Length; ++index1)
            {
                maps2[index1] = new Utils.MeshMaps[frags[index1].Length];
                for (int index2 = 0; index2 < frags[index1].Length; ++index2)
                {
                    maps2[index1][index2] = new Utils.MeshMaps();
                    maps2[index1][index2].BuildBary(origMeshes[index1], frags[index1][index2]);

                    if (!origMeshInfo.IsSkinnedMeshRenderer(index1))
                    {
                        maps2[index1][index2].ComputeNormals(origMaps[index1], origMeshes[index1], frags[index1][index2], (Matrix4x4[])null, (Transform[])null, smoothInner);
                    }
                    else
                    {
                        maps2[index1][index2].ComputeNormals(origMaps[index1], origMeshes[index1], frags[index1][index2], origMeshInfo.uMeshes[index1].bindposes, ((SkinnedMeshRenderer)origMeshInfo.renderers[index1]).bones, smoothInner);
                    }

                    maps2[index1][index2].ComputeTexCoords(origMaps[index1], origMeshes[index1], frags[index1][index2], uvScale, uvAreaBegin, uvAreaEnd);
                    maps2[index1][index2].ComputeVertexColors(origMaps[index1], origMeshes[index1], frags[index1][index2], innerColor);
                }
            }
            return maps2;
        }

        // Get bounds by renderers
        static Bounds GetRendererBounds(List<Renderer> list)
        {
            // Only one bound
            if (list.Count == 1)
                return list[0].bounds;
            
            // New bound
            Bounds bound = new Bounds();
            
            // Basic bounds min and max values
            float minX = list[0].bounds.min.x - 0.01f;
            float minY = list[0].bounds.min.y - 0.01f;
            float minZ = list[0].bounds.min.z - 0.01f;
            float maxX = list[0].bounds.max.x + 0.01f;
            float maxY = list[0].bounds.max.y + 0.01f;
            float maxZ = list[0].bounds.max.z + 0.01f;
            
            for (int i = 1; i < list.Count; i++)
            {
                if (list[i].bounds.min.x < minX) minX = list[i].bounds.min.x;
                if (list[i].bounds.min.y < minY) minY = list[i].bounds.min.y;
                if (list[i].bounds.min.z < minZ) minZ = list[i].bounds.min.z;
                if (list[i].bounds.max.x > maxX) maxX = list[i].bounds.max.x;
                if (list[i].bounds.max.y > maxY) maxY = list[i].bounds.max.y;
                if (list[i].bounds.max.z > maxZ) maxZ = list[i].bounds.max.z;
            }
            
            // Get center
            bound.center = new Vector3((maxX - minX) / 2f, (maxY - minY) / 2f, (maxZ - minZ) / 2f);

            // Get min and max vectors
            bound.min = new Vector3(minX, minY, minZ);
            bound.max = new Vector3(maxX, maxY, maxZ);

            return bound;
        }
        
        /// /////////////////////////////////
        /// Shatter script
        /// /////////////////////////////////
        
        public static void Fragment(RayfireShatter sh, RFEngine engineData)
        {
            // Start countdown
            System.Diagnostics.Stopwatch stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Start();
            
            // Set engine data
            engineData.SetEngineData (sh.gameObject, sh.slice.sliceList.ToArray());
            
            // Set global bounds by all renderers TODO if Custom/Hex frag type
            sh.bound = GetRendererBounds (engineData.renderers);
            
            // Bake World Transform & get RFMeshes Array
            Utils.Mesh[][] fragMeshes = engineData.GetBakedWorldTransformMeshes();
            
            // Apply Extra TN. Use original object if there is no references
            Matrix4x4 biasTN = Matrix4x4.identity;
            biasTN.SetTRS(new Vector3(), sh.CenterDir, new Vector3(1, 1, 1));
            biasTN = biasTN.inverse;
            Utils.Mesh.Transform(fragMeshes, biasTN);
            
            // Normalize meshes Verts to range 0.0f - 1.0f
            Matrix4x4 normalMat = Utils.Mesh.GetNormMat(fragMeshes, out Vector3 aabbMin, out Vector3 aabbMax);
            Utils.Mesh.Transform(fragMeshes, normalMat);

            // Separate not connected elements
            if (sh.advanced.separate == true)
                fragMeshes = Utils.Mesh.Separate(fragMeshes, true);
            
            // PreCap holes on every element and set not capped open edges array
            bool[][] openEdgeFlags = new bool[fragMeshes.Length][];
            Utils.Mesh.CheckOpenEdges(fragMeshes, openEdgeFlags, sh.advanced.inputPrecap);
            
            // Surface for cut
            SetSliceType (sh.advanced.sliceType, fragMeshes, openEdgeFlags);
            
            // Prepare Slice Data Parameters
            Vector3 aabbCentroid    = (aabbMin + aabbMax) * 0.5f;
            Vector3 aabbSize        = aabbMax - aabbMin;
            Vector3 cutAabbCentroid = new Vector3();
            Vector3 cutAabbSize     = new Vector3();
            
            // Set Custom fragmentation bounding box data TODO set as centerBias object for correct bb
            bool cutAABB = sh.advanced.aabbEnable == true && sh.advanced.aabbObject != null && sh.advanced.aabbObject.gameObject.activeSelf;
            if (cutAABB == true)
            {
                Matrix4x4 aabbGizmoMAT = biasTN * sh.advanced.aabbObject.localToWorldMatrix;
                cutAabbCentroid = aabbGizmoMAT.GetPosition();
                cutAabbSize     = aabbGizmoMAT.lossyScale;

                // Convert bbox coordinates to lib coordinates
                Utils.SliceData.FixAABB (ref cutAabbCentroid, ref cutAabbSize, normalMat);
            }
            
            // Fix Bias Pos
            Vector3 biasPos = new Vector3(float.NaN, float.NaN, float.NaN);
            if (sh.CenterBias != null)
                biasPos = normalMat.MultiplyPoint(biasTN * sh.CenterPos);

            // Create Slice Data  
            Utils.SliceData sliceData = new Utils.SliceData();
            
            // Adjusted axis scale for Splinter and Slabs frag types
            Vector3 axisScale = AxisScale(sh);
            
            // Set fragmentation bounding box
            sliceData.SetAABB(fragMeshes, axisScale, aabbCentroid, aabbSize, cutAABB, cutAabbCentroid, cutAabbSize, sh.advanced.aabbSeparate);       

            // Chose Fragmentation Type and Set Parameters
            SetFragType (sh, sliceData, normalMat, biasTN, aabbMax, biasPos);

            // Custom cell fragmentation state. 100% by default. Can be used for partial per frame fragmentation
            for (int i = 0; i < sliceData.GetNumCells(); i++)
                sliceData.Enable (i, true);
            
            // Create Unity Mesh objects
            System.Diagnostics.Stopwatch sliceStopWatch = new System.Diagnostics.Stopwatch();
            sliceStopWatch.Start();
            
            
            // Fragment for all types except Decompose type
            sh.advanced.minFaceFilter = 0;
            if (sh.type != FragType.Decompose)
                fragMeshes = Utils.Mesh.Slice(sliceData, sh.advanced.combine, openEdgeFlags, sh.advanced.elementSizeThreshold * 0.01f, sh.advanced.minFaceFilter);

            sliceStopWatch.Stop();
            
            // Get inner sub id TODO add support for other renderers, not only first renderer materials
            int innerSubId = GetInnerSubId(sh.material.iMat, engineData.GetMaterials(0));
            
            // Build SubMeshes
            Utils.Mesh.BuildSubMeshes(fragMeshes, engineData.uMeshes, innerSubId);

            // Undo Normalize
            Utils.Mesh.Transform(fragMeshes, normalMat.inverse);

            // Undo Extra TN
            if (sh.CenterBias != null)
                Utils.Mesh.Transform(fragMeshes, biasTN.inverse);

            // Undo World Transform
            engineData.UnBakeWorldTransform(fragMeshes);

            // Restore Maps
            Utils.MeshMaps[][] fragMaps = ComputeMaps(engineData, fragMeshes, normalMat, sh.material.MappingScale, sh.material.UvRegionMin, sh.material.UvRegionMax, sh.material.Color, sh.advanced.smooth);
           
            // Centerize
            Vector3[][] centroids = Utils.Mesh.Centerize(fragMeshes);
            
            System.Diagnostics.Stopwatch hierarchySW = new System.Diagnostics.Stopwatch();
            hierarchySW.Start();
            
            // Instantiate original object hierarchy
            Matrix4x4 origScaleMat = Matrix4x4.identity;
            if (sh.advanced.hierarchy == HierarchyType.Instantiate)
            {
                origScaleMat = engineData.CloneHierarchy (sh.gameObject, sh.advanced.inheritScale);
            }
            else if (sh.advanced.hierarchy == HierarchyType.Build)
            {
                origScaleMat = engineData.CloneHierarchy(sh.gameObject, sh.advanced.inheritScale);
            }
            else if (sh.advanced.hierarchy == HierarchyType.Flatten)
            {
                origScaleMat = engineData.CloneHierarchy(sh.gameObject, sh.advanced.inheritScale);
            }
            hierarchySW.Stop();
            
            // Build Unity Objects Hierarchy
            sh.skinnedMeshesOrigScale = new List<Tuple<Mesh, Matrix4x4[]>>();
          
            // Set scale
            if(sh.advanced.inheritScale == false)
                Utils.Mesh.Transform(fragMeshes, origScaleMat);
            
            // Create Unity Mesh objects
            CreateFragments (sh, engineData, fragMeshes, fragMaps, centroids, origScaleMat);
            
            if (sh.skinnedMeshesOrigScale.Count == 0)
                sh.skinnedMeshesOrigScale = null;

            if (engineData.RootGO != null)
            {           
                sh.go      =  engineData.RootGO;
                Object.DestroyImmediate(sh.go.GetComponent<RayfireShatter>());            
            }
            
            engineData.SynchronizeAnimTime(sh.gameObject);

            stopWatch.Stop();
            // Debug.Log("Slice Time == " + sliceStopWatch.Elapsed.TotalMilliseconds + " ms.,  " + 
            //          "Total Time == " + stopWatch.Elapsed.TotalMilliseconds + " ms.,  " + 
            //          "CloneHierarchy Time == " + hierarchySW.Elapsed.TotalMilliseconds + " ms.");
        }

        // Chose Fragmentation Type and Set Parameters
        static void SetFragType(RayfireShatter sh, Utils.SliceData sd, Matrix4x4 normalMat, Matrix4x4 biasTN, Vector3 aabbMax, Vector3 biasPos)
        {
            switch (sh.type)
            {
                case FragType.Voronoi:
                {
                    sd.GenRandomPoints(sh.voronoi.Amount, sh.advanced.Seed);
                    sd.ApplyCenterBias(biasPos, sh.voronoi.centerBias);
                    sd.BuildCells();
                    break;
                }
                case FragType.Splinters:
                {
                    sd.GenRandomPoints(sh.splinters.Amount, sh.advanced.Seed);
                    sd.ApplyCenterBias(biasPos, sh.splinters.centerBias);
                    sd.BuildCells();
                    break;
                }
                case FragType.Slabs:
                {
                    sd.GenRandomPoints(sh.slabs.Amount, sh.advanced.Seed);
                    sd.ApplyCenterBias(biasPos, sh.slabs.centerBias);
                    sd.BuildCells();
                    break;
                }
                case FragType.Radial:
                {
                    Vector3 aabbAbsSize = normalMat.inverse * aabbMax;
                    float radiusExtraScale = Mathf.Max(aabbAbsSize.x, aabbAbsSize.y, aabbAbsSize.z);
                    sd.GenRadialPoints(
                        sh.advanced.Seed, 
                        sh.radial.rays, 
                        sh.radial.rings, 
                        sh.radial.radius / (radiusExtraScale * sh.radial.rings), 
                        sh.radial.divergence * 2.0f, 
                        sh.radial.twist / 90.0f, 
                        (int)sh.radial.centerAxis, 
                        biasPos, 
                        sh.radial.focus, 
                        sh.radial.randomRings * 0.01f, 
                        sh.radial.randomRays * 0.01f);
                    sd.BuildCells();
                    break;
                }
                case FragType.Hexagon:
                {
                    List<Vector3> customPnt        = RFHexagon.GetHexPointCLoudV2 (sh.hexagon, sh.CenterPos, sh.CenterDir, sh.bound);
                    List<Vector3> customPointsList = new List<Vector3>();
                    for(int i = 0; i < customPnt.Count; i++)
                        customPointsList.Add(normalMat.MultiplyPoint(biasTN * customPnt[i]));
                    sd.SetCustomPoints(customPointsList);
                    float centerBias = 0;
                    sd.ApplyCenterBias(biasPos, centerBias);
                    sd.BuildCells();
                    break;
                }
                case FragType.Custom:
                {
                    List<Vector3> customPnt        = RFCustom.GetCustomPointCLoud (sh.custom, sh.transform, sh.advanced.Seed, sh.bound);
                    List<Vector3> customPointsList = new List<Vector3>();
                    for(int i = 0; i < customPnt.Count; i++)
                        customPointsList.Add(normalMat.MultiplyPoint(biasTN * customPnt[i]));
                    sd.SetCustomPoints(customPointsList);
                    float centerBias = 0;
                    sd.ApplyCenterBias(biasPos, centerBias);
                    sd.BuildCells();
                    break;
                }
                case FragType.Slices:
                {
                    // Set slice data by transforms. TODO Use in Shatter
                    sd.AddPlanes(sh.slice.sliceList.ToArray(), new Vector3(0, 1, 0), normalMat);
                    
                    // Set slice data by vector arrays. TODO Use for Rigid
                    //Vector3[] pos = null;
                    //Vector3[] norm = null;
                    //RFSlice.SetSliceData(sh.slice, sh.transform, ref pos, ref norm);
                    //sd.AddPlanes(pos, norm, normalMat);
                    
                    break;
                }
                case FragType.Bricks:
                {
                    sd.GenBricks(
                        sh.bricks.Size,
                        sh.bricks.Num, 
                        sh.bricks.SizeVariation, 
                        sh.bricks.SizeOffset, 
                        sh.bricks.SplitState,
                        sh.bricks.SplitPro);
                    break;
                }
                case FragType.Voxels:
                {
                    sd.GenBricks(
                        sh.voxels.Size,
                        sh.bricks.SplitState, // has 0 vectorint
                        Vector3.zero, 
                        Vector3.zero, 
                        sh.voxels.SplitState,
                        Vector3.zero);
                    break;
                }
                case FragType.Tets:
                {
                    // TODO
                    Debug.Log ("Tetrahedron fragmentation is not supported yet by V2 Beta engine");
                    break; 
                }
            }
        }

        // Surface for cut
        static void SetSliceType(SliceType slice, Utils.Mesh[][] fragMeshes, bool[][] openEdgeFlags)
        {
            if (slice != SliceType.Hybrid)
                for (int i = 0; i < fragMeshes.Length; i++)
                    for (int j = 0; j < fragMeshes[i].Length; j++)
                        openEdgeFlags[i][j] = slice == SliceType.ForcedCut;
        }
        
        // Create Unity Mesh objects
        static void CreateFragments(RayfireShatter sh, RFEngine engineData, Utils.Mesh[][] fragMeshes, Utils.MeshMaps[][] fragMaps, Vector3[][] centroids, Matrix4x4 origScaleMat)
        {
            // Clear last fragments list
            sh.fragmentsLast.Clear();
            
            // Collect main root to fragment roots
            sh.rootChildList.Add (engineData.RootGO.transform);

            for (int i = 0; i < fragMeshes.Length; i++)
            {
                // Create local fragment root
                GameObject fragsGroup = engineData.GetGroupGO(i);
                fragsGroup.name += "_root";

                // Get bones transforms
                Transform[] bones = engineData.GetBonesTNS(i);

                for (int j = 0; j < fragMeshes[i].Length; j++)
                {
                    // Get fragment location
                    Utils.Mesh.FragmentLocation loc = fragMeshes[i][j].GetFragLocation();
                   
                    // Skip inner fragments by inner filter
                    if (sh.advanced.inner == true && loc == Utils.Mesh.FragmentLocation.Inner)
                        continue;

                    // Create fragment object
                    GameObject fragGo = new GameObject(engineData.GetGroupName(i) + "_sh_" + j);
                    
                    // Set root as parent
                    fragGo.transform.SetParent(fragsGroup.transform, false);
                    
                    // Collect to last fragments
                    sh.fragmentsLast.Add (fragGo);
                    
                    // Create fragment mesh
                    Mesh frag = new Mesh();
                    frag.name         = fragGo.name;
                    frag.indexFormat  = fragMeshes[i][j].GetNumTri() * 3 < ushort.MaxValue ? UnityEngine.Rendering.IndexFormat.UInt16 : UnityEngine.Rendering.IndexFormat.UInt32;
                    frag.subMeshCount = fragMeshes[i][j].GetNumSubMeshes();
                    frag.vertices     = fragMeshes[i][j].GetVerts();
                    
                    // Set triangles
                    for (int subMesh = 0; subMesh < fragMeshes[i][j].GetNumSubMeshes(); subMesh++)
                        frag.SetTriangles(fragMeshes[i][j].GetSubTris(subMesh), subMesh);
                    
                    // Set bone data
                    frag.boneWeights = fragMeshes[i][j].GetSkinData();

                    // Set UV data
                    fragMaps[i][j].GetMaps(frag);
                    
                    // Add shell
                    if (sh.shell.enable == true)
                        frag = RFShell.AddShell (fragMeshes[i][j], frag, sh.shell.bridge, sh.shell.submesh, sh.shell.thickness);
                    
                    // Mesh filter fragments
                    if(bones == null)   
                    {
                        // Add meshfilter
                        MeshFilter mf = fragGo.AddComponent<MeshFilter>();
                        mf.sharedMesh = frag;
                        
                        // Add renderer and materials
                        fragGo.AddComponent<MeshRenderer>().sharedMaterials = GetCorrectMaterials(engineData.GetMaterials(i), fragMeshes[i][j], sh.material.iMat);
      
                        // Set scale
                        if (sh.advanced.inheritScale == true)
                            fragGo.transform.localPosition = centroids[i][j];
                        else
                            fragGo.transform.localPosition = origScaleMat * centroids[i][j];
                    }
                    
                    // Skinned mesh fragments
                    else               
                    {
                        Matrix4x4 centroidMat = Matrix4x4.Translate(centroids[i][j]);
                        Matrix4x4[] bindPoses = new Matrix4x4[engineData.uMeshes[i].bindposes.Length];
                        Matrix4x4[] bindPosesForScale = new Matrix4x4[engineData.uMeshes[i].bindposes.Length];
                        for (int ii = 0; ii < bindPoses.Length; ii++)
                        {
                            bindPoses[ii] = engineData.uMeshes[i].bindposes[ii] * centroidMat;
                            bindPosesForScale[ii] = bindPoses[ii];
                        }
                        frag.bindposes = bindPoses;
                        sh.skinnedMeshesOrigScale.Add(Tuple.Create(frag, bindPosesForScale));
                        SkinnedMeshRenderer smr = fragGo.AddComponent<SkinnedMeshRenderer>();
                        smr.sharedMaterials = GetCorrectMaterials(engineData.GetMaterials(i), fragMeshes[i][j], sh.material.iMat);
                        smr.bones           = bones;
                        smr.rootBone        = engineData.GetRootBone(i);
                        smr.sharedMesh      = frag;
                    }
                }        
            }
            
            // Collect to all fragments
            sh.fragmentsAll.AddRange (sh.fragmentsLast);
        }
        
        // Adjusted axis scale for Splinter and Slabs frag types
        static Vector3 AxisScale(RayfireShatter sh)
        {
            Vector3 axisScale = Vector3.one;
            if (sh.type == FragType.Splinters)
            {
                float stretch = Mathf.Min (1.0f, Mathf.Max (0.005f, Mathf.Pow (1.0f - sh.splinters.strength, 1.5f)));
                if (sh.splinters.axis == AxisType.XRed)
                    axisScale.x = stretch;
                else if (sh.splinters.axis == AxisType.YGreen)
                    axisScale.y = stretch;
                else if (sh.splinters.axis == AxisType.ZBlue)
                    axisScale.z = stretch;
            }
            else if (sh.type == FragType.Slabs)
            {
                float stretch = Mathf.Min (1.0f, Mathf.Max (0.005f, Mathf.Pow (1.0f - sh.slabs.strength, 1.5f)));
                if (sh.slabs.axis == AxisType.XRed)
                    axisScale.y = axisScale.z = stretch;
                else if (sh.slabs.axis == AxisType.YGreen)
                    axisScale.x = axisScale.z = stretch;
                else if (sh.slabs.axis == AxisType.ZBlue)
                    axisScale.x = axisScale.y = stretch;
            }
            return axisScale;
        }
        
        // Delete fragments
        public static void Delete(RayfireShatter sh)
        {
            if (sh.go != null)
            {
                Object.DestroyImmediate(sh.go);
            }

            if(sh.skinnedMeshesOrigScale != null)
            {
                sh.skinnedMeshesOrigScale = null;
            }
        }

        // Get inner sub id in case of inner material usage
        static int GetInnerSubId(Material innerMaterial, Material[] materials)
        {
            // Inner surface should have custom inner material
            if (innerMaterial != null)
            {
                // Object already has Inner material applied to one of the submesh
                if (materials.Contains (innerMaterial) == true)
                    for (int i = 0; i < materials.Length; i++)
                        if (innerMaterial == materials[i])
                            return i;
                
                // Object has no inner material applied
                return -1;
            }
            
            // Apply first material to inner surface
            return 0;
        }

        // Set materials to fragments
        static Material[] GetCorrectMaterials(Material[] materials, Utils.Mesh rfMesh, Material innerMaterial)
        {
            List<Material> correctMaterials = new List<Material>();
            for (int ii = 0; ii < rfMesh.GetNumSubMeshes(); ii++)
            {
                int origSubID = rfMesh.GetSubMeshID(ii);
                if (origSubID >= 0) // if == -1 then its a inner faces new submesh
                {
                    correctMaterials.Add(materials[origSubID]);
                }
                if (origSubID == -1) // if == -1 then its a inner faces new submesh
                {
                    correctMaterials.Add(innerMaterial);
                }
            }
            return correctMaterials.ToArray();
        }

        /*
        // Update is called once per frame
        void SetComponentsUpdateScalePreview(ComplexTest ct)
        {

            if (ct.go != null)
            {
                float scaleTmp = ct.FragsScale * 0.01f;

                var mfs = ct.go.GetComponentsInChildren<MeshFilter>();
                for (int i = 0; i < mfs.Length; i++)
                {
                    mfs[i].gameObject.transform.localScale = new Vector3 (scaleTmp, scaleTmp, scaleTmp);
                }

                if (ct.skinnedMeshesOrigScale != null)
                {
                    Matrix4x4 scaleMat = Matrix4x4.Scale (new Vector3 (scaleTmp, scaleTmp, scaleTmp));
                    for (int i = 0; i < ct.skinnedMeshesOrigScale.Count; i++)
                    {
                        var origScale = ct.skinnedMeshesOrigScale[i].Item2;
                        if (ct.skinnedMeshesOrigScale[i].Item1.bindposes.Length == origScale.Length)
                        {
                            Matrix4x4[] scaledBPs = new Matrix4x4[origScale.Length];
                            for (int j = 0; j < origScale.Length; j++)
                            {
                                scaledBPs[j] = origScale[j] * scaleMat;
                            }
                            ct.skinnedMeshesOrigScale[i].Item1.bindposes = scaledBPs;
                        }
                    }
                }

                var rdrs = ct.GetComponentsInChildren<Renderer>();
                for (int i = 0; i < rdrs.Length; i++)
                {
                    rdrs[i].enabled = false;
                }

                var clds = ct.GetComponentsInChildren<Collider>();
                for (int i = 0; i < clds.Length; i++)
                {
                    clds[i].enabled = false;
                }
            }
            else
            {
                var rdrs = ct.GetComponentsInChildren<Renderer>();
                for (int i = 0; i < rdrs.Length; i++)
                {
                    rdrs[i].enabled = true;
                }
            }
        }
        */
    }
}

// Static dummy class for not supported platforms
#else
namespace RayFire
{
    public class RFEngine
    {
        public List<Renderer>                     renderers;

        public static void Fragment(RayfireShatter sh, RFEngine engineData){}
    }
}

#endif