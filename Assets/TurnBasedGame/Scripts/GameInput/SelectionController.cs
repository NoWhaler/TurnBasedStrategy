using System;
using System.Collections.Generic;
using TurnBasedGame.Scripts.Enum;
using TurnBasedGame.Scripts.Managers;
using UnityEngine;
using UnityEngine.EventSystems;

namespace TurnBasedGame.Scripts.GameInput
{
    public class SelectionController : MonoBehaviour
    {
        [SerializeField] private MouseInput _mouseInput;
        [SerializeField] private LayerMask _tileLayerMask;
        [SerializeField] private LayerMask _unitLayerMask;

        private Ray _ray;
        private Camera _camera;

        private BattleTile _selectedTile;

        private Unit _selectedUnit; 
        
        [Header("")]
        
        private SelectionManager _selectionManager;
        private TurnManager _turnManager;
        private PathfinderManager _pathfinderManager;
        private BattleGrid _battleGrid;

        public static bool IsPutPhaseInputEnded;

        [field: SerializeField] public List<BattleTile> ShortestPath { get; set; } = new List<BattleTile>();
        public event Action<List<BattleTile>, Unit> OnSelectedTileToMove; 

        private void Awake()
        {
            _mouseInput = FindObjectOfType<MouseInput>();
            _camera = FindObjectOfType<Camera>();
            _selectionManager = FindObjectOfType<SelectionManager>();
            _battleGrid = FindObjectOfType<BattleGrid>();
            _turnManager = FindObjectOfType<TurnManager>();
            _pathfinderManager = FindObjectOfType<PathfinderManager>();
        }

        private void OnEnable()
        {
            _mouseInput.OnLeftMouseDownToSelect += Select;
            _mouseInput.OnRightMouseClickToDeselect += Deselect;
        }

        private void OnDisable()
        {
            _mouseInput.OnLeftMouseDownToSelect -= Select;
            _mouseInput.OnRightMouseClickToDeselect -= Deselect;
        }


        public void EnterUnit(Unit unit)
        {
            
        }

        public void ExitUnit()
        {
            
            
        }

        public void EnterTile(BattleTile tile)
        {
            _ray = _camera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(_ray, out var hit, _tileLayerMask))
            {
                _selectedTile = hit.collider.GetComponent<BattleTile>();
                if (_selectedTile != null && _selectedTile == tile) 
                {
                    if (_turnManager.ClosestTiles.Contains(_selectedTile) && !_turnManager.IsMoving)
                    {
                        ShortestPath = _pathfinderManager.CalculateShortestPath(_selectedTile);
                        _pathfinderManager.HighLightShortestPath(ShortestPath);
                    }
                }
            }
        }

        public void ExitTile()
        {
            if (_selectedTile != null && !_turnManager.IsMoving)
            {
                _selectedTile = null;
                _pathfinderManager.HighlightGreen(ShortestPath);
                ShortestPath.Clear();
            }
        }

        private void Select()
        {
            _ray = _camera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(_ray, out var hit, _tileLayerMask))
            {
                _selectedTile = hit.collider.GetComponent<BattleTile>();
                if (_selectedTile != null  && !IsPutPhaseInputEnded && _battleGrid.PlayerPlacementTiles.Contains(_selectedTile))
                {
                    if (_selectedTile.IsEmpty)
                    {
                        if (_selectionManager.Unit != null && _selectionManager.Unit.IsSelected)
                        {
                            _selectionManager.Unit.gameObject.SetActive(true);
                            if (_selectionManager.Unit.BattleTile == null)
                            {
                                _selectionManager.Unit.BattleTile = _selectedTile;
                            }
                            else if (_selectionManager.Unit.BattleTile != null)
                            {
                                _selectionManager.Unit.BattleTile.IsEmpty = true;
                                _selectionManager.Unit.BattleTile = _selectedTile;
                            }
                            _selectionManager.Unit.transform.position = _selectedTile.transform.position + new Vector3(0f, 0.5f, 0f);
                            _selectedTile.Unit = _selectionManager.Unit;
                            _selectedTile.IsEmpty = false;
                            _selectionManager.Unit.IsSelected = false;
                            _selectionManager.Unit = null;
                        }
                    }
                }
                
                else if (_selectedTile != null 
                         && IsPutPhaseInputEnded 
                         && _turnManager.ClosestTiles.Contains(_selectedTile) && !_turnManager.IsMoving)
                {
                    OnSelectedTileToMove?.Invoke(ShortestPath, _selectedTile.Unit);
                }
            }
            
            if (Physics.Raycast(_ray, out var hitUnit, _unitLayerMask))
            {
                _selectedUnit = hitUnit.collider.GetComponent<Unit>();
                if (_selectedUnit != null 
                    && IsPutPhaseInputEnded
                    && _selectedUnit.UnitFractionType == UnitFractionType.Enemy)
                {

                    foreach (var tile in _turnManager.ClosestTiles)
                    {
                        if (tile.Unit == _selectedUnit)
                        {
                            Debug.Log($"Attack this Unit - {_selectedUnit.name}");
                        }
                    }
                }
            }
        }

        private void Deselect()
        {
            _ray = _camera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(_ray, out var hit, _unitLayerMask))
            {
                _selectedUnit = hit.collider.GetComponent<Unit>();
                if (_selectedUnit != null && !IsPutPhaseInputEnded)
                {
                    _selectedUnit.Position = Vector3.zero;
                    _selectedUnit.gameObject.SetActive(false);
                    _selectedUnit.BattleTile.IsEmpty = true;
                    _selectedUnit.BattleTile = null;
                }
            }
        }
    }
}