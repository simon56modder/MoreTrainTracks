using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ColossalFramework;
using UnityEngine;
using System.IO;
using ColossalFramework.UI;
using ColossalFramework.Steamworks;
using ColossalFramework.Globalization;
using ColossalFramework.IO;
using ColossalFramework.Steamworks;
using ColossalFramework.Plugins;
using ICities;
using UnityEngine;

namespace MoreTrainTracks
{
    public class Container : MonoBehaviour
    {
        public bool ClonedPrefab;
        public NetInfo.Lane[] oLanes;
        public Rect window = new Rect(Screen.width - 305, Screen.height - 300, 300, 134);
        public bool showWindow = false, illuminated = false, cable = true, move = false;
        public int texture = 0;
        public string[] mastsCodeNames = new string[] { "PROPS_TITLE[RailwayPowerline]:0", "CableMast" };

        private NetInfo concretePrefab, noCablePrefab, noCableBridge, noCableElevated, noCableSlope, noCableConcrete, noConcreteBridge, noConcreteElevated, noConcreteSlope, AbandonnedPrefab, AbandonnedPrefab2, TramTracks, noCableTram, IlluminatedPrefab, IlluminatedNoCable/*, RoadTramPrefab*/;
        public NetInfo originalPrefab;


        public void Awake()
        {
            DontDestroyOnLoad(this);
        }

        public void OnLevelWasLoaded(int level)
        {
            if (level == 6)
            {
                ClonedPrefab = false;
            }
        }

        public void Update()
        {
            try
            {
                if (ToolsModifierControl.GetCurrentTool<ToolBase>() is NetTool)
                {
                    if (IsTrainTool(ToolsModifierControl.GetCurrentTool<NetTool>().m_prefab))
                    {
                        showWindow = true;
                        SetTool();
                    }
                    else
                        showWindow = false;
                }
                else if (ToolsModifierControl.GetCurrentTool<ToolBase>().GetType().Name == "NetToolFine")
                {
                    if (IsTrainTool((ToolsModifierControl.GetCurrentTool<ToolBase>() as NetTool).m_prefab))
                    {
                        showWindow = true;
                        SetTool();
                    }
                    else
                        showWindow = false;
                }
                else
                    showWindow = false;
            }
            catch { }
            if (ClonedPrefab) return;
            try
            {
                GameObject.Find("Public Transport").GetComponent<NetCollection>();
            }
            catch (Exception)
            {
                return;
            }
            ClonePrefab();
            ClonedPrefab = true;
        }


        private void ClonePrefab()
        {
            originalPrefab = Resources.FindObjectsOfTypeAll<NetInfo>().FirstOrDefault(netinfo => netinfo.name == "Train Track");
            /* 
            var originalPrefab =
                Resources.FindObjectsOfTypeAll<NetInfo>().
                FirstOrDefault(netInfo => netInfo.name == "Train Station Track");
            if (originalPrefab == null)
            {
                Debug.LogError("MoreTrainTracks - Train track not found");
                return;
            } */
            // var noCablePrefab = ClonePrefab(originalPrefab, "NoCable Train Track");
            concretePrefab = ClonePrefab(originalPrefab, "ConcreteTrain Track");
            noCablePrefab = ClonePrefab(originalPrefab, "NoCable Train Track");
            // var noCableTunnel = ClonePrefab(Resources.FindObjectsOfTypeAll<NetInfo>().FirstOrDefault(netInfo => netInfo.name == "Train Track Tunnel"), "NoCable TrainTunnel");
            noCableBridge = ClonePrefab(Resources.FindObjectsOfTypeAll<NetInfo>().FirstOrDefault(netInfo => netInfo.name == "Train Track Bridge"), "NoCable TrainBridge");
            noCableElevated = ClonePrefab(Resources.FindObjectsOfTypeAll<NetInfo>().FirstOrDefault(netInfo => netInfo.name == "Train Track Elevated"), "NoCable TrainElevat");
            noCableSlope = ClonePrefab(Resources.FindObjectsOfTypeAll<NetInfo>().FirstOrDefault(netInfo => netInfo.name == "Train Track Slope"), "NoCable TrainSlope");
            noCableConcrete = ClonePrefab(originalPrefab, "NoCableConcreteTrac");
            noConcreteBridge = ClonePrefab(Resources.FindObjectsOfTypeAll<NetInfo>().FirstOrDefault(netInfo => netInfo.name == "Train Track Bridge"), "NoConcr TrainBridge");
            noConcreteElevated = ClonePrefab(Resources.FindObjectsOfTypeAll<NetInfo>().FirstOrDefault(netInfo => netInfo.name == "Train Track Elevated"), "NoConcr TrainElevat");
            noConcreteSlope = ClonePrefab(Resources.FindObjectsOfTypeAll<NetInfo>().FirstOrDefault(netInfo => netInfo.name == "Train Track Slope"), "NoConcr TrainSlope");
            AbandonnedPrefab = ClonePrefab(originalPrefab, "Abandonned Track");
            AbandonnedPrefab2 = ClonePrefab(originalPrefab, "Abandonned Track 2");
            TramTracks = ClonePrefab(originalPrefab, "Tram Tracks");
            noCableTram = ClonePrefab(originalPrefab, "TramTracksNoCable");
            IlluminatedPrefab = ClonePrefab(originalPrefab, "Illuminated Tracks");
            IlluminatedNoCable = ClonePrefab(originalPrefab, "IlluminNoCable");
            // RoadTramPrefab = ClonePrefab(Resources.FindObjectsOfTypeAll<NetInfo>().FirstOrDefault(netInfo => netInfo.name == "Large Oneway"), "Tram tracks on road");

            concretePrefab.m_availableIn = ItemClass.Availability.None;
            noCablePrefab.m_availableIn = ItemClass.Availability.None;
            noCableBridge.m_availableIn = ItemClass.Availability.None;
            noCableElevated.m_availableIn = ItemClass.Availability.None;
            noCableSlope.m_availableIn = ItemClass.Availability.None;
            noCableConcrete.m_availableIn = ItemClass.Availability.None;
            noConcreteBridge.m_availableIn = ItemClass.Availability.None;
            noConcreteElevated.m_availableIn = ItemClass.Availability.None;
            noConcreteSlope.m_availableIn = ItemClass.Availability.None;
            AbandonnedPrefab.m_availableIn = ItemClass.Availability.None;
            AbandonnedPrefab2.m_availableIn = ItemClass.Availability.None;
            TramTracks.m_availableIn = ItemClass.Availability.None;
            noCableTram.m_availableIn = ItemClass.Availability.None;
            IlluminatedPrefab.m_availableIn = ItemClass.Availability.None;
            IlluminatedNoCable.m_availableIn = ItemClass.Availability.None;

           // RoadTramPrefab.m_availableIn = ItemClass.Availability.None;

            if (concretePrefab != null)
            {
                later(() =>
                {
                    concretePrefab = ConcreteVersion(concretePrefab);
                    noCableConcrete = ConcreteVersion(noCableConcrete);

                    // noCableConcrete.m_segments = new NetInfo.Segment[] { noCableConcrete.m_segments[0], noCableConcrete.m_segments[1] };

                    AbandonnedPrefab.m_segments = new NetInfo.Segment[] { AbandonnedPrefab.m_segments[0], AbandonnedPrefab.m_segments[1] };
                    AbandonnedPrefab2.m_segments = new NetInfo.Segment[] { AbandonnedPrefab2.m_segments[0] };

                    noCablePrefab = NoCableVersion(noCablePrefab);
                    noCableConcrete = NoCableVersion(noCableConcrete);

                    Debug.Log("Wi-fi antenna tex dimensions : " + FindProp("PROPS_TITLE[RailwayPowerline]:0").m_material.mainTexture.height + "x" + FindProp("PROPS_TITLE[RailwayPowerline]:0").m_material.mainTexture.width);
                    Debug.Log("Wi-fi antenna lod tex dimensions : " + FindProp("PROPS_TITLE[RailwayPowerline]:0").m_lodMaterial.mainTexture.height + "x" + FindProp("PROPS_TITLE[RailwayPowerline]:0").m_lodMaterial.mainTexture.height);


                    TramTracks = TramVersion(TramTracks, 1);
                    noCableTram = TramVersion(NoCableVersion(noCableTram), 2);
                   // RoadTramPrefab = TramVersion(RoadTramPrefab, 3);
                    IlluminatedPrefab = Illuminated(IlluminatedPrefab, 0);
                    IlluminatedNoCable = Illuminated(IlluminatedNoCable, 1);

                    if (noCablePrefab != null)
                    {
                        var AInoCablePrefab = noCablePrefab.GetComponent<TrainTrackAI>();
                        AInoCablePrefab.m_slopeInfo = SlopeNoCable(noCableSlope);
                        AInoCablePrefab.m_elevatedInfo = BridgeElevatedNoCable(noCableElevated);
                        AInoCablePrefab.m_bridgeInfo = BridgeElevatedNoCable(noCableBridge);

                        #region Texture and Mesh changes tests
                        /*
                        Texture2D tex = new UnityEngine.Texture2D(47, 47);
                        tex.LoadImage(File.ReadAllBytes(string.Concat(Util.AssemblyDirectory, "/Segment0_template_d.png")));
                      // Texture2D lodTex = new UnityEngine.Texture2D(47, 47);
                      // lodTex.LoadImage(File.ReadAllBytes(string.Concat(Util.AssemblyDirectory, "/Segment0_template_lod_d.png")));
                        
                        AInoCablePrefab.m_elevatedInfo.m_segments[0].m_mesh = Instantiate<Mesh>(AInoCablePrefab.m_elevatedInfo.m_segments[0].m_mesh);
                        AInoCablePrefab.m_elevatedInfo.m_segments[0].m_segmentMesh = Instantiate<Mesh>(AInoCablePrefab.m_elevatedInfo.m_segments[0].m_segmentMesh);
                        AInoCablePrefab.m_elevatedInfo.m_segments[0].m_lodMesh = Instantiate<Mesh>(AInoCablePrefab.m_elevatedInfo.m_segments[0].m_lodMesh);
                        AInoCablePrefab.m_elevatedInfo.m_segments[0].m_mesh = Util.LoadMesh(string.Concat(Util.AssemblyDirectory, "/Model_segTest.obj"), "ETST ");
                        AInoCablePrefab.m_elevatedInfo.m_segments[0].m_segmentMesh = Util.LoadMesh(string.Concat(Util.AssemblyDirectory, "/Model_segTest.obj"), "ETST ");
                        AInoCablePrefab.m_elevatedInfo.m_segments[0].m_lodMesh = Util.LoadMesh(string.Concat(Util.AssemblyDirectory, "/Model_segTest.obj"), "ETST ");


                        replaceTexture(AInoCablePrefab.m_elevatedInfo, tex, true, false);
                        
                        AInoCablePrefab.m_elevatedInfo.m_segments[0].m_material = Instantiate<Material>(AInoCablePrefab.m_elevatedInfo.m_segments[0].m_material);
                        AInoCablePrefab.m_elevatedInfo.m_segments[0].m_segmentMaterial = Instantiate<Material>(AInoCablePrefab.m_elevatedInfo.m_segments[0].m_segmentMaterial);
                        AInoCablePrefab.m_elevatedInfo.m_segments[0].m_lodMaterial = Instantiate<Material>(AInoCablePrefab.m_elevatedInfo.m_segments[0].m_lodMaterial);
                        AInoCablePrefab.m_elevatedInfo.m_segments[0].m_segmentMaterial.mainTexture = tex;
                        AInoCablePrefab.m_elevatedInfo.m_segments[0].m_material.mainTexture = tex;
                        AInoCablePrefab.m_elevatedInfo.m_segments[0].m_lodMaterial.mainTexture = lodTex; 
                        try
                        {
                            Debug.Log("Texture width and height m_material : " + AInoCablePrefab.m_elevatedInfo.m_segments[0].m_material.mainTexture.width);
                            Debug.Log("Texture height and height m_material : " + AInoCablePrefab.m_elevatedInfo.m_segments[0].m_material.mainTexture.height);
                        }
                        catch { }
                        try
                        {
                            Debug.Log("Texture width and height m_lodMaterial : " + AInoCablePrefab.m_elevatedInfo.m_segments[0].m_lodMaterial.mainTexture.width);
                            Debug.Log("Texture height and height m_lodMaterial : " + AInoCablePrefab.m_elevatedInfo.m_segments[0].m_lodMaterial.mainTexture.height);
                        }
                        catch { }
                        try
                        {
                            Debug.Log("Texture width and height m_segmentMaterial : " + AInoCablePrefab.m_elevatedInfo.m_segments[0].m_segmentMaterial.mainTexture.width);
                            Debug.Log("Texture height and height m_segmentMaterial : " + AInoCablePrefab.m_elevatedInfo.m_segments[0].m_segmentMaterial.mainTexture.height);
                        }
                        catch { }
                        Debug.Log(AInoCablePrefab.m_elevatedInfo.m_segments[0].m_material.mainTexture.width); 

                        AInoCablePrefab.m_bridgeInfo.m_segments[0].m_mesh = Instantiate<Mesh>(AInoCablePrefab.m_bridgeInfo.m_segments[0].m_mesh);
                        AInoCablePrefab.m_bridgeInfo.m_segments[0].m_segmentMesh = Instantiate<Mesh>(AInoCablePrefab.m_bridgeInfo.m_segments[0].m_segmentMesh);
                        AInoCablePrefab.m_bridgeInfo.m_segments[0].m_lodMesh = Instantiate<Mesh>(AInoCablePrefab.m_bridgeInfo.m_segments[0].m_lodMesh);
                        AInoCablePrefab.m_bridgeInfo.m_segments[0].m_mesh = Util.LoadMesh(string.Concat(Util.AssemblyDirectory, "/Model_segTest.obj"), "ETST ");
                        AInoCablePrefab.m_bridgeInfo.m_segments[0].m_segmentMesh = Util.LoadMesh(string.Concat(Util.AssemblyDirectory, "/Model_segTest.obj"), "ETST ");
                        AInoCablePrefab.m_bridgeInfo.m_segments[0].m_lodMesh = Util.LoadMesh(string.Concat(Util.AssemblyDirectory, "/Model_segTest.obj"), "ETST ");

                        replaceTexture(AInoCablePrefab.m_bridgeInfo, tex, true, false);
                         
                        AInoCablePrefab.m_bridgeInfo.m_segments[0].m_material = Instantiate<Material>(AInoCablePrefab.m_bridgeInfo.m_segments[0].m_material);
                        AInoCablePrefab.m_bridgeInfo.m_segments[0].m_segmentMaterial = Instantiate<Material>(AInoCablePrefab.m_bridgeInfo.m_segments[0].m_segmentMaterial);
                        AInoCablePrefab.m_bridgeInfo.m_segments[0].m_lodMaterial = Instantiate<Material>(AInoCablePrefab.m_bridgeInfo.m_segments[0].m_lodMaterial);
                        AInoCablePrefab.m_bridgeInfo.m_segments[0].m_segmentMaterial.mainTexture = tex;
                        AInoCablePrefab.m_bridgeInfo.m_segments[0].m_material.mainTexture = tex;
                        AInoCablePrefab.m_bridgeInfo.m_segments[0].m_lodMaterial.mainTexture = lodTex;  */
                        #endregion
                    }
                    if (noCableConcrete != null)
                    {
                        var AInoCableConcrete = noCableConcrete.GetComponent<TrainTrackAI>();
                        AInoCableConcrete.m_slopeInfo = SlopeNoCable(noConcreteSlope, true);
                        AInoCableConcrete.m_elevatedInfo = BridgeElevatedNoCable(noConcreteElevated, true);
                        AInoCableConcrete.m_bridgeInfo = BridgeElevatedNoCable(noConcreteBridge, true);
                    }

                    AbandonnedPrefab = Abandonned(AbandonnedPrefab);
                    AbandonnedPrefab2 = Abandonned(AbandonnedPrefab2);

                    // ChangePowerlineMast("CableMast");
                });
            }
        }

        void OnGUI()
        {
            if (showWindow)
                window = GUI.Window(537973489, window, Window, "MoreTrainTracks");
        }

        void Window(int id = 537973489)
        {
            if (move)
            {
                GUI.DragWindow(new Rect(0, 0, 300, 100));
                GUI.Label(new Rect(100, 45, 200, 30), "<size=20>Move window!</size>");
            }
            else
            {
                texture = GUI.Toolbar(new Rect(5, 28, 290, 32), texture, new string[] { "Gravel", "Concrete", "Grass" });
                cable = GUI.Toggle(new Rect(5, 65, 140, 30), cable, "Overhead cable");
                illuminated = GUI.Toggle(new Rect(150, 65, 130, 30), illuminated, "Illumination");
                if (GUI.changed/*GUI.Button(new Rect(10, 100, 190, 28), "Apply")*/)
                {
                    SetTool();
                }
            }
            move = GUI.Toggle(new Rect(198, 100, 100, 28), move, "Move window");
        }


        private NetInfo TramVersion(NetInfo source, int version)
        {
            NetInfo n = source;
            if (version == 1)
            {
                n.m_segments = new NetInfo.Segment[] { n.m_segments[1], n.m_segments[2] };
                // lane 1 has cable masts, lane 0 has not. both have ends.
                /* for (int i = 0; i < n.m_lanes[1].m_laneProps.m_props.ToList().Count; i++)
                {
                    if (!(n.m_lanes[0].m_laneProps.m_props[0].m_prop == n.m_lanes[1].m_laneProps.m_props[i].m_prop || n.m_lanes[0].m_laneProps.m_props[1].m_prop == n.m_lanes[1].m_laneProps.m_props[i].m_prop))
                        n.m_lanes[1].m_laneProps.m_props[i].m_prop = findProp("CableMast");
                } */

                n.m_createGravel = false;
                n.m_clipTerrain = false;
                n.m_clipSegmentEnds = false;
            }
            if (version == 2)
            {
                n.m_segments = new NetInfo.Segment[] { n.m_segments[1] };
                n.m_createGravel = false;
                n.m_clipTerrain = false;
                n.m_clipSegmentEnds = false;
            }
            if (version == 3)
            {
                Debug.Log("This thing has " + n.m_lanes.ToList().Count + " lanes !");
                for (int i = 0; i < n.m_lanes.ToList().Count; i++)
                {
                    Debug.Log("Lane " + (i + 1) + " (" + i + ") : vehicle type is " + n.m_lanes[i].m_vehicleType.ToString() + ", type " + n.m_lanes[i].m_laneType + ".");
                }

                foreach (NetLaneProps.Prop prop in n.m_lanes[1].m_laneProps.m_props)
                {
                    prop.m_prop = FindProp("PROPS_TITLE[Railway Crossing Very Long]:0");
                }


                n.m_segments = new NetInfo.Segment[] { n.m_segments[0], originalPrefab.m_segments[1], originalPrefab.m_segments[2] };
                n.m_lanes[9].m_vehicleType = VehicleInfo.VehicleType.Train;
                n.m_lanes[8].m_vehicleType = VehicleInfo.VehicleType.Train;
                n.m_lanes[9].m_laneProps = new NetLaneProps();
                n.m_lanes[9].m_laneProps.m_props = originalPrefab.m_lanes[1].m_laneProps.m_props;
                n.m_lanes[8].m_laneProps = new NetLaneProps();
                n.m_lanes[8].m_laneProps.m_props = originalPrefab.m_lanes[0].m_laneProps.m_props;

                n.m_nodes = new NetInfo.Node[] { n.m_nodes[0], originalPrefab.m_nodes[0], originalPrefab.m_nodes[1] };
                // n.m_nodes[n.m_nodes.Count()] = originalPrefab.m_nodes[0];
                //  n.m_nodes[n.m_nodes.Count() + 1] = originalPrefab.m_nodes[1];
                //   n.m_nodes[n.m_nodes.Count() + 2] = originalPrefab.m_nodes[2];
            }
            return n;
        }

        private NetInfo Illuminated(NetInfo source, int version)
        {
            NetInfo n = source;
            NetLaneProps props = Instantiate(n.m_lanes[1].m_laneProps);

            for (int i = 0; i < props.m_props.ToList().Count; i++)
            {
                if (!(n.m_lanes[0].m_laneProps.m_props[0].m_prop == props.m_props[i].m_prop || n.m_lanes[0].m_laneProps.m_props[1].m_prop == props.m_props[i].m_prop))
                    props.m_props[i].m_prop = FindProp("Street Lamp #2");
            }

            n.m_lanes[version].m_laneProps = props;
            if (version == 1)
            {
                n.m_segments = new NetInfo.Segment[] { n.m_segments[0], n.m_segments[1] };
                n.m_nodes = new NetInfo.Node[] { n.m_nodes[0], n.m_nodes[1], n.m_nodes[2] };
            }
            return n;
        }

        private NetInfo NoCableVersion(NetInfo source)
        {
            NetInfo n = source;
            n.m_lanes[1].m_laneProps = n.m_lanes[0].m_laneProps;
            n.m_segments = new NetInfo.Segment[] { n.m_segments[0], n.m_segments[1] };
            n.m_nodes = new NetInfo.Node[] { n.m_nodes[0], n.m_nodes[1], n.m_nodes[2] };
            return n;
        }

        private NetInfo BridgeElevatedNoCable(NetInfo source, bool concrete = false)
        {
            NetInfo n = source;
            n.m_lanes[1].m_laneProps = n.m_lanes[0].m_laneProps;
            n.m_segments = new NetInfo.Segment[] { n.m_segments[0], n.m_segments[1] };
            n.m_nodes = new NetInfo.Node[] { n.m_nodes[0], n.m_nodes[1] };
            if (concrete)
            {
                n.m_createGravel = false;
                n.m_createPavement = true;
            }
            return n;
        }

        private NetInfo ConcreteVersion(NetInfo source)
        {
            NetInfo n = source;
            n.m_createGravel = false;
            n.m_createPavement = true;
            return n;
        }

        private NetInfo Abandonned(NetInfo source)
        {
            NetInfo n = source;
            foreach (NetInfo.Lane lane in n.m_lanes)
            {
                lane.m_laneProps = null;
            }
            n.m_nodes = new NetInfo.Node[] { n.m_nodes[0], n.m_nodes[1], n.m_nodes[2] };
            n.m_maxHeight = 0;
            n.m_minHeight = 0;
            n.m_createGravel = false;
            return n;
        }

        private NetInfo SlopeNoCable(NetInfo source, bool concrete = false)
        {
            NetInfo n = source;
            n.m_lanes[1].m_laneProps = n.m_lanes[0].m_laneProps;
            n.m_segments = new NetInfo.Segment[] { n.m_segments[0], n.m_segments[1], n.m_segments[3] };
            n.m_nodes = new NetInfo.Node[] { n.m_nodes[0], n.m_nodes[1], n.m_nodes[3] };
            if (concrete)
            {
                n.m_createGravel = false;
                n.m_createPavement = true;
            }
            return n;
        }

        /*
        public NetInfo ChangePowerlineMast(NetInfo info, PropInfo prop)
        {
            NetInfo n = info;
            foreach (NetInfo.Lane lane in info.m_lanes)
            {
                NetLaneProps.Prop[] props = lane.m_laneProps.m_props;
                lane.m_laneProps = new NetLaneProps() { m_props = props };
                foreach (NetLaneProps.Prop p in lane.m_laneProps.m_props)
                {
                    if (mastsCodeNames.ToList().Contains(p.m_prop.GetLocalizedTitle()))
                    {
                        p.m_prop = prop;
                        p.m_finalProp = prop;
                    }
                }
            }
            return n;
        }
        public void ChangePowerlineMast(string name)
        {
            PropInfo CableMast = FindProp("PROPS_TITLE[RailwayPowerline]:0");
            CableMast.m_mesh = Util.LoadMesh(string.Concat(Util.AssemblyDirectory, "/CableMast.obj"), "CableMastObj");
            CableMast.m_lodMesh = Util.LoadMesh(string.Concat(Util.AssemblyDirectory, "/CableMast_lod.obj"), "CableMastLod");
            replaceTexture(CableMast, string.Concat(Util.AssemblyDirectory, "/CableMast_d.png"), string.Concat(Util.AssemblyDirectory, "/CableMast_lod_d.png"));
        }

        public void replaceTexture(NetInfo ni, Texture tex, bool segments, bool nodes)
        {
            if (segments)
            {
                Material mat = new Material(ni.m_segments[0].m_material);
                mat.shader = ni.m_segments[0].m_material.shader;
                mat.SetTexture("_MainTex", tex);
                for (int i = 0; i < ni.m_segments.Length; ++i)
                {
                    ni.m_segments[i].m_material = mat;
                    ni.m_segments[i].m_lodRenderDistance = 2500;
                }
            }
            if (nodes)
            {
                Material mat = new Material(ni.m_nodes[0].m_material);
                mat.shader = ni.m_nodes[0].m_material.shader;
                mat.SetTexture("_MainTex", tex);
                for (int i = 0; i < ni.m_nodes.Length; ++i)
                {
                    ni.m_nodes[i].m_material = mat;
                    ni.m_nodes[i].m_lodRenderDistance = 2500;
                }
            }
        }
        public void replaceTexture(PropInfo prop, string path, string pathLOD)
        {
            
            Material mat = new Material(prop.m_material);
            Texture2D tex = new Texture2D(2, 2);
            tex.LoadImage(File.ReadAllBytes(path));
            mat.shader = prop.m_material.shader;
            mat.SetTexture("_MainTex", tex);
            prop.m_material = mat; 
            Material mat = new Material(prop.m_material);
            try
            {
                mat.GetTexture("_MainTex");
                Export(mat.GetTexture("_MainTex"), "Prop_MainTex");
                Debug.Log("Can get _MainTex !");
            }
            catch
            {
                Debug.Log("Can't get _MainTex !");
            }
            try
            {
                mat.GetTexture("_xyz");
                Export(mat.GetTexture("_xyz"), "Prop_xyz");
                Debug.Log("Can get _xyz !");
            }
            catch
            {
                Debug.Log("Can't get _xyz !");
            }
            try
            {
                mat.GetTexture("_aci");
                Export(mat.GetTexture("_aci"), "Prop_aci");
                Debug.Log("Can get _aci !");
            }
            catch
            {
                Debug.Log("Can't get _aci !");
            }

            
            Material matLod = new Material(prop.m_lodMaterial);
            Texture2D texLod = new Texture2D(2, 2);
            texLod.LoadImage(File.ReadAllBytes(pathLOD));
            matLod.shader = prop.m_lodMaterial.shader;
            matLod.SetTexture("_MainTex", tex);
            prop.m_lodMaterial = mat; 
        }

        void Export(Texture tex, string Name)
        {
            byte[] b = ((Texture2D)tex).EncodeToPNG();
            File.WriteAllBytes(@"C:\Users\Simon\Desktop\FICHIERS\Skylines\Modding\MoreTrainTracks\railwayMastStock\" + Name + ".png", b);
        }

        public static string AssemblyDirectory
        {
            get
            {
                var pluginManager = PluginManager.instance;
                var plugins = pluginManager.GetPluginsInfo();

                foreach (var item in plugins)
                {
                    try
                    {
                        var instances = item.GetInstances<IUserMod>();
                        if (!(instances.FirstOrDefault() is Mod))
                        {
                            continue;
                        }
                        return item.modPath;
                    }
                    catch { }
                }
                throw new Exception("Failed to find MoreTrainTracks assembly!");

            }
        }
*/

        private void later(Action a)
        {
            Singleton<LoadingManager>.instance.QueueLoadingAction(InCoroutine(a));
        }

        private static IEnumerator InCoroutine(Action a)
        {
            a();
            yield break;
        }

        NetInfo ClonePrefab(NetInfo originalPrefab, string newName)
        {
            var instance = Instantiate(originalPrefab.gameObject);
            instance.name = newName;
            instance.transform.SetParent(transform);
            instance.transform.localPosition = new Vector3(-7500, -7500, -7500);
            var newPrefab = instance.GetComponent<NetInfo>();
            instance.SetActive(false);

            newPrefab.m_prefabInitialized = false;
            newPrefab.m_netAI = null;

            MethodInfo initMethod = typeof(NetCollection).GetMethod("InitializePrefabs", BindingFlags.Static | BindingFlags.NonPublic);
            Singleton<LoadingManager>.instance.QueueLoadingAction((IEnumerator)
            initMethod.Invoke(null, new object[] { newName, new[] { newPrefab }, new string[] { null } }));

            newPrefab.m_prefabInitialized = false;

            return newPrefab;
        }

        PropInfo FindProp(string name)
        {
            foreach (PropInfo prop in Resources.FindObjectsOfTypeAll<PropInfo>())
            {
                if (prop.GetLocalizedTitle() == name)
                    return prop;
            }
            return null;
        }

        /*
        public PropInfo CreatePropInfo(string name)
        {
            string meshPath = "/" + name + ".obj";
            string meshPathLod = "/" + name + "_lod.obj";
            string texturePath = "/" + name + "_d.png";
            string texturePathLod = "/" + name + "_lod_d.png";
            Texture2D texture = new Texture2D(3,3), textureLod = new Texture2D(3,3);
            texture.LoadImage(File.ReadAllBytes(string.Concat(Util.AssemblyDirectory, texturePath)));
            textureLod.LoadImage(File.ReadAllBytes(string.Concat(Util.AssemblyDirectory, texturePathLod)));
            PropInfo prop = Instantiate(FindProp("Wi-Fi Antenna"));
            prop.m_mesh = Util.LoadMesh(string.Concat(Util.AssemblyDirectory, meshPath), name);
            prop.m_lodMesh = Util.LoadMesh(string.Concat(Util.AssemblyDirectory, meshPathLod), name + "_LOD");
            prop.m_material.SetTexture("_MainTex", texture);
            prop.m_lodMaterial.SetTexture("_MainTex", textureLod);
            return prop;
        } */

        public bool IsTrainTool(NetInfo info)
        {
            if (info == originalPrefab || info == concretePrefab || info == noCablePrefab || info == noCableBridge || info == noCableElevated || info == noCableSlope || info == noCableConcrete || info == noConcreteBridge || info == noConcreteElevated || info == noConcreteSlope || info == AbandonnedPrefab || info == AbandonnedPrefab2 || info == TramTracks || info == noCableTram || info == IlluminatedPrefab || info == IlluminatedNoCable)
                return true;
            return false;
        }

        void SetTool(int v = 0)
        {
            NetTool netTool = ToolsModifierControl.SetTool<NetTool>();
            if (v == 1)
            {
                netTool.m_prefab = AbandonnedPrefab;
                return;
            }
            if (v == 2)
            {
                netTool.m_prefab = AbandonnedPrefab2;
                return;
            }

            if (illuminated)
            {
                if (cable)
                {
                    if (texture == 0)
                        netTool.m_prefab = IlluminatedPrefab;
                }
                else
                {
                    if (texture == 0)
                        netTool.m_prefab = IlluminatedNoCable;
                }
            }
            else
            {
                if (cable)
                {
                    if (texture == 0)
                        netTool.m_prefab = originalPrefab;
                    else if (texture == 1)
                        netTool.m_prefab = concretePrefab;
                    else if (texture == 2)
                        netTool.m_prefab = TramTracks;
                }
                else
                {
                    if (texture == 0)
                        netTool.m_prefab = noCablePrefab;
                    else if (texture == 1)
                        netTool.m_prefab = noCableConcrete;
                    else if (texture == 2)
                        netTool.m_prefab = noCableTram;
                }
            }
        }
    }
}
