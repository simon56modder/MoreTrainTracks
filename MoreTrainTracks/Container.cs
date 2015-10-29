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
        public Rect window = new Rect(Screen.width - 305, Screen.height - 300, 300, 134);
        public bool showWindow = false, illuminated = false, cable = true, move = false;
        public int texture = 0;

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
            concretePrefab = ClonePrefab(originalPrefab, "ConcreteTrain Track");
            noCablePrefab = ClonePrefab(originalPrefab, "NoCable Train Track");
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


            if (concretePrefab != null)
            {
                later(() =>
                {
                    concretePrefab = ConcreteVersion(concretePrefab);
                    noCableConcrete = ConcreteVersion(noCableConcrete);



                    AbandonnedPrefab.m_segments = new NetInfo.Segment[] { AbandonnedPrefab.m_segments[0], AbandonnedPrefab.m_segments[1] };
                    AbandonnedPrefab2.m_segments = new NetInfo.Segment[] { AbandonnedPrefab2.m_segments[0] };

                    noCablePrefab = NoCableVersion(noCablePrefab);
                    noCableConcrete = NoCableVersion(noCableConcrete);


                    TramTracks = TramVersion(TramTracks, 1);
                    noCableTram = TramVersion(NoCableVersion(noCableTram), 2);
                    IlluminatedPrefab = Illuminated(IlluminatedPrefab, 0);
                    IlluminatedNoCable = Illuminated(IlluminatedNoCable, 1);

                    if (noCablePrefab != null)
                    {
                        var AInoCablePrefab = noCablePrefab.GetComponent<TrainTrackAI>();
                        AInoCablePrefab.m_slopeInfo = SlopeNoCable(noCableSlope);
                        AInoCablePrefab.m_elevatedInfo = BridgeElevatedNoCable(noCableElevated);
                        AInoCablePrefab.m_bridgeInfo = BridgeElevatedNoCable(noCableBridge);
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


                n.m_segments = new NetInfo.Segment[] { n.m_segments[0], originalPrefab.m_segments[1], originalPrefab.m_segments[2] };
                n.m_lanes[9].m_vehicleType = VehicleInfo.VehicleType.Train;
                n.m_lanes[8].m_vehicleType = VehicleInfo.VehicleType.Train;
                n.m_lanes[9].m_laneProps = new NetLaneProps();
                n.m_lanes[9].m_laneProps.m_props = originalPrefab.m_lanes[1].m_laneProps.m_props;
                n.m_lanes[8].m_laneProps = new NetLaneProps();
                n.m_lanes[8].m_laneProps.m_props = originalPrefab.m_lanes[0].m_laneProps.m_props;

                n.m_nodes = new NetInfo.Node[] { n.m_nodes[0], originalPrefab.m_nodes[0], originalPrefab.m_nodes[1] };
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
