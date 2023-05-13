using UnityEngine;

namespace TP3.Script.PlayerStateMachine
{
    public class Player_Tree : PlayerState
    {
        private Ray m_MouseRay;
        private RaycastHit m_HitInfo;
        private Camera m_MainCamera = Camera.main;
        private int tries;

        public Player_Tree(PlayerStateMachine stateMachine) : base(stateMachine)
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
                        m_Transform.LookAt(m_HitInfo.collider.transform.position);
                        target.GetComponent<CapsuleCollider>().isTrigger = true;
                        if (Random.Range(0, 6 - tries) == 1)
                        {
                            ResetBananaTrees();
                            // WIn
                            LevelManager.CollectItemAction?.Invoke(Inventory.ItemType.Banana);
                            LevelManager.EndLevelAction?.Invoke(true);
                            SaveGame.GetInstance().SaveData();
                            _StateMachine.SetState(new Player_Jumping(_StateMachine));
                        }
                        else
                        {
                            AudioManager.instance.PlaySound(SoundClip.WrongTree, 1.0f);
                            tries++;
                        }
                    }
                }
            }

            if (tries >= 3)
            {
                // Lose
                ResetBananaTrees();
                LevelManager.EndLevelAction?.Invoke(false);
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