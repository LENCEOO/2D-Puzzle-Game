using System.Collections;

namespace IG.Command
{
    public interface ICommand // 명령 패턴을 구현
    {
        IEnumerator Execute(); // 동작 실행
        void Undo(); // 실행 취소
    }
}
