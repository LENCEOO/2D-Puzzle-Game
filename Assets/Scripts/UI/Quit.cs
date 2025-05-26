using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace IG.UI
{
    public class Quit : MonoBehaviour
    {
        public void QuitAndReset()
        {
            // 저장된 모든 데이터 삭제
            PlayerPrefs.DeleteAll();
            PlayerPrefs.SetInt("CurrentLevel", 1);
            PlayerPrefs.SetInt("LastUnlockedLevel", 1);
            PlayerPrefs.Save();

#if UNITY_EDITOR
            // 에디터에서는 Play 모드 종료
            EditorApplication.isPlaying = false;
#else
            // 빌드된 게임에서는 앱 종료
            Application.Quit();
#endif
        }
    }
}

