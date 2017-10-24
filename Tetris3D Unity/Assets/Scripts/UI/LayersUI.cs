using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UI {

    public class LayersUI : MonoBehaviour {

        [SerializeField]
        private GridLayoutGroup grid;
        [SerializeField]
        private GameObject layerImagePrefab;

        private int curRowIndex;

        private void Start() {
            Level.Instance.OnHeightChanged += UpdateGrid;
        }

        private void OnDestroy() {
            if(Level.Instance != null)
                Level.Instance.OnHeightChanged -= UpdateGrid;
        }

        private void UpdateGrid() {
             if (Level.Instance.HighestRow > curRowIndex)
                AddImageLayerToGrid();
            else if (Level.Instance.HighestRow < curRowIndex)
                RemoveImageLayerFromGrid();
        }

        private void AddImageLayerToGrid() {
            GameObject layer = Instantiate(layerImagePrefab, grid.transform);
            layer.GetComponent<Image>().color = Level.Instance.GetCorrespondingRowColor(Level.Instance.HighestRow - 1);
            curRowIndex++;
        }

        private void RemoveImageLayerFromGrid() {
            if (grid.transform.childCount > 0)
                Destroy(grid.transform.GetChild(0).gameObject);
            curRowIndex--;
        }
    }
}