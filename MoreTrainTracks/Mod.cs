using ICities;
using UnityEngine;

namespace MoreTrainTracks
{
    public class Mod : LoadingExtensionBase, IUserMod
    {

        public static Container Container;

        public string Name
        {
            get { return "More Train Tracks"; }
        }

        public string Description
        {
            get { return "[PRE-APLHA] Adds variations of train tracks"; }
        }

        public override void OnCreated(ILoading loading)
        {
            base.OnCreated(loading);

            if (Container == null) {
                Container = new GameObject("MoreTrainTracks").AddComponent<Container>();
            }
        }

        public override void OnReleased()
        {
            base.OnReleased();
            if (Container != null)
            {
                Object.Destroy(Container.gameObject);
            }
        }
    }
}