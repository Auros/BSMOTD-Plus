using BS_Utils.Utilities;
using System.Linq;
using UnityEngine;

namespace BSMOTD_Plus
{
    class Assistant : MonoBehaviour
    {
        private static Assistant _instance;
        public static Assistant Instance
        {
            get
            {
                if (_instance == null)
                {
                    var go = new GameObject();
                    _instance = go.AddComponent<Assistant>();
                    DontDestroyOnLoad(go);
                    
                }
                return _instance;
            }
        }

        private BMPFlowCoordinator _bmp;


        public void SummonUI()
        {
            _mainFlowCoordinator = MainFlowCoordinator;
            if (_bmp == null)
            {
                _bmp = new GameObject().AddComponent<BMPFlowCoordinator>();
                _bmp._mainFlowCoordinator = MainFlowCoordinator;
            }
            _mainFlowCoordinator.InvokeMethod("PresentFlowCoordinator", new object[] { _bmp, null, false, false });
            //return _bmp;
        }

        public static void PlayDespacito()
        {
            Logger.log.Info("'no'");
        }
        private MainFlowCoordinator _mainFlowCoordinator;
        private MainFlowCoordinator MainFlowCoordinator
        {
            get
            {
                if (_mainFlowCoordinator == null)
                    _mainFlowCoordinator = Resources.FindObjectsOfTypeAll<MainFlowCoordinator>().FirstOrDefault();
                return _mainFlowCoordinator;
            }
        }
    }
}
