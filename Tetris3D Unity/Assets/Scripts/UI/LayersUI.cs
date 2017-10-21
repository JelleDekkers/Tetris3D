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

        private int curLayers;

        private void Start() {
            Level.Instance.OnHeightChanged += UpdateGrid;
        }

        private void OnDestroy() {
            Level.Instance.OnHeightChanged -= UpdateGrid;
        }

        private void UpdateGrid() {
             if (Level.Instance.HighestLayer > curLayers)
                AddImageLayerToGrid();
            else if (Level.Instance.HighestLayer < curLayers)
                RemoveImageLayerFromGrid();
        }

        private void AddImageLayerToGrid() {
            GameObject layer = Instantiate(layerImagePrefab, grid.transform);
            layer.GetComponent<Image>().color = Level.Instance.GetCorrespondingRowColor(Level.Instance.HighestLayer - 1);
            curLayers++;
        }

        private void RemoveImageLayerFromGrid() {
            if (grid.transform.childCount > 0)
                Destroy(grid.transform.GetChild(0).gameObject);
            curLayers--;
        }
    }
}