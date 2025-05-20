using UnityEngine;
using IG.NodeSystem;
using IG.Controller;
using System.Collections;

namespace IG.Command
{
    /// <summary>
    /// Represents a command for handling node click actions, including
    /// execution and undo functionality.
    /// </summary>
    /// 노드 클릭할 때 어떤 동작을 실행할지 정의
    public class NodeClickCommand : ICommand 
    {
        private readonly GameObject _clickedNode; // 유저가 클릭한 노드 오브젝트
        private readonly ScoreManager _scoreManager; // 플레이어 점수 & 이동 횟수 관리

        public NodeClickCommand(GameObject clickedNode, ScoreManager scoreManager)
        {
            _clickedNode = clickedNode;
            _scoreManager = scoreManager;
        }

        /// <summary>
        /// Executes the command, triggering the node's click behavior
        /// </summary>
        public IEnumerator Execute()
        {
            yield return _clickedNode.GetComponent<Node>().NodeClicked();
            
            // Update player's move count.
            _scoreManager.PlayerMoves++;
        }

        public void Undo()
        {
            // TODO 
            //_scoreManager.PlayerMoves--;
        }
    }
}

