using IG.Level;
using IG.NodeSystem;
using UnityEngine;

namespace IG.Controller 
{
    /// <summary>
    /// Defines Grid manager which will initialize all required nodes
    /// </summary>
    public interface IGrid
    {
        public Transform GridTransform {get; set;}
        // 회로 연결 여부 검증용 클래스
        public CircuitValidation CircuitValidation {get; set;}
        public LevelConfig LevelConfig {get; set;} // Level data will be used from this config
        Node GetNodeAt(int row, int column); // 노드 리턴
    }
}
