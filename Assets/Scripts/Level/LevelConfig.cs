using UnityEngine;
using System.Linq;

namespace IG.Level
{
    /// <summary>
    /// Defines the structure and settings for each level.
    /// </summary>
    [CreateAssetMenu(fileName = "Level", menuName = "IG/Level")]
    public class LevelConfig : ScriptableObject // 에디터에서 데이터를 저장할 수 있는 템플릿 객체
    {
        public enum GridType // 그리드 형태 (Square만 지원)
        {
            Square = 4,
            Hexagonal = 6
        }

        public enum NodeType // 노드 종류
        {
            WiFiNode,
            ComputerNode,
            CableNode,
            EmptyNode
        }

        [System.Serializable]
        public class NodeData
        {
            public GameObject nodePrefab; // Prefab for the node
            public NodeType nodeType;
            [Range(0, 5)] // Range is 0-5 to support 4 rotations for Square and 6 for Hexagonal
            public int initialRotation; // Number of rotations (0~5)
        }

        // Level Grid type (read only) - Only Square is working at the moment
        public GridType gridType {get;} = GridType.Square; // 읽기 전용
        public float nodeSize = 100f; // 노드 크기
        public float spacing = 5f; // 노드 사이 간격

        /// <summary>
        /// Minimum number of moves for a perfect score
        /// </summary>
        public int minMoves;
        /// <summary>
        /// Maximum number of moves after which the score will be 1
        /// </summary>
        public int maxMoves;
        // rows, columns : 맵의 크기
        public int rows;
        public int columns; 
        public int TotalComputers {get; private set;}
        public NodeData[] grid;

        private void OnValidate()
        {
            ValidateGridSize();

            for (int i = 0; i < grid.Length; i++)
            {
                if (grid[i] == null)
                {
                    Debug.Log($"{name} {i}th element is null");
                }
            }
        }

        public void Initialize(Grid nodeParentGrid)
        {
            ValidateGridSize();

            nodeParentGrid.cellSize = new Vector2(nodeSize, nodeSize);
            nodeParentGrid.cellGap = new Vector2(spacing, spacing);
        }

        // TODO Grid size, row, column, Required nodes, other validations
        private void ValidateGridSize()
        {
            var gridSize = grid.Length;
            if (gridSize <= 0)
            {
                Debug.LogError($"{name} Grid size must be greater than 0.");
            }

            if (rows == 0 || columns == 0 || !gridSize.Equals(rows * columns))
            {
                Debug.LogError($"{name} Grid does not have valid rows or colums or Grid size is not valid");
            }

            UpdateTotalComputerSize();
        }

        private void UpdateTotalComputerSize() 
        {
            TotalComputers = (from NodeData item in grid
                                  where item.nodeType.Equals(NodeType.ComputerNode)
                                  select item).Count();
        }

        public void SetGridElement(int row, int column, NodeData element)
        {
            if (row >= 0 && row < rows && column >= 0 && column < columns)
            {
                int index = row * columns + column;
                grid[index] = element;
            }
            else
            {
                Debug.LogWarning($"{name} Grid position out of bounds");
            }
        }

        public NodeData GetGridElement(int row, int column)
        {
            if (grid.Length < 2 || row < 0 || row >= rows || column < 0 || column >= columns)
            {
                Debug.LogWarning($"{name} Grid position out of bounds");
                return null;
            }

            int index = row * columns + column;
            return grid[index];
        }
    }
}