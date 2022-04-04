using DefaultNamespace;
using UnityEngine;

namespace BML.Scripts
{
    public class ConveyorBelt : MonoBehaviour
    {
        [SerializeField] private GameObject boxPrefab;
        [SerializeField] private float yOffset = 2f;
        [SerializeField] private float xOffset = 2f;
        [SerializeField] private float moveSpeed = 5f;

        public void SpawnBox()
        {
            Vector3 playerPos = Camera.main.gameObject.transform.position;
            Vector3 spawnPos = new Vector3(transform.position.x + xOffset, transform.position.y + yOffset, playerPos.z);
            GameObject box = GameObject.Instantiate(boxPrefab, spawnPos, Quaternion.identity);
            box.GetComponent<MoveOverTime>().SetMove(moveSpeed, transform.forward);
        }
    }
}