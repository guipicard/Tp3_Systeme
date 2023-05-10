using UnityEngine;

namespace TP3.Script.PlayerStateMachine
{
    public class Player_Tree : PlayerState
    {
        private Ray m_MouseRay;
        private RaycastHit m_HitInfo;
        private Camera m_MainCamera = Camera.main;
        private int tries;

        public Player_Tree(global::PlayerStateMachine stateMachine) : base(stateMachine)
        {
            tries = 0;
        }

        public override void UpdateExecute()
        {
            if (Input.GetMouseButtonDown(0))
            {
                m_MouseRay = m_MainCamera.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(m_MouseRay, out m_HitInfo))
                {
                    GameObject target = m_HitInfo.collider.gameObject;
                    if (target.layer == 8 && !target.GetComponent<CapsuleCollider>().isTrigger)
                    {
                        target.GetComponent<CapsuleCollider>().isTrigger = true;
                        if (Random.Range(0, 6 - tries) == 1)
                        {
                            ResetBananaTrees();
                            Debug.Log("win");
                            // WIn
                            LevelManager.instance.CollectItemAction?.Invoke(Inventory.ItemType.Banana);
                            LevelManager.instance.EndLevelAction?.Invoke(true);
                            _StateMachine.SetState(new Player_Jumping(_StateMachine));
                        }
                        else
                        {
                            Debug.Log("Nope");
                            tries++;
                        }
                    }
                }
            }

            if (tries >= 3)
            {
                // Lose
                ResetBananaTrees();
                Debug.Log("Lose");
                LevelManager.instance.EndLevelAction?.Invoke(false);
                _StateMachine.SetState(new Player_Idle(_StateMachine));
            }
        }

        public override void FixedUpdateExecute()
        {
        }

        private void ResetBananaTrees()
        {
            foreach (var tree in GameObject.FindGameObjectsWithTag("Banana"))
            {
                tree.GetComponent<CapsuleCollider>().isTrigger = false;
            }
        }
    }
}