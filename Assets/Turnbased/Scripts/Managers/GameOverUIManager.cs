using System;
using Photon.Pun;
using TMPro;
using Turnbased.Scripts.Player;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Turnbased.Scripts.Managers
{
    public class GameOverUIManager : MonoBehaviour
    {
        public TMP_Text text;
        public GameObject holder;
        // Start is called before the first frame update
        void Start()
        {
            Invoke(nameof(Register),1f);
        }

        void Register()
        {
            GetOpponentPlayer()._damagable.OnPlayerDead += OnOpponentDead;
            GetMyPlayer()._damagable.OnPlayerDead += OnPlayerDead;

            GetMyPlayer().OnPlayerLeft += OnPlayerLeft;
            GetOpponentPlayer().OnPlayerLeft += OnOpponentLeft;
        }

        private void OnOpponentLeft()
        {
            OnOpponentDead();
            PhotonNetwork.LeaveRoom();
        }

        private void OnPlayerLeft()
        {
            OnPlayerDead();
            PhotonNetwork.LeaveRoom();
        }

        private void OnOpponentDead()
        {
            holder.SetActive(true);
            text.text = "YOU WON";
        }

        private void OnPlayerDead()
        {
            holder.SetActive(true);
            text.text = "YOU LOST";
        }

        public void Back()
        {
            SceneManager.LoadScene(0);
        }


        Unit GetOpponentPlayer()
        {
            GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
            Unit unit=null;
            foreach (var pla in players)
            {
                if (!pla.GetComponent<PhotonView>().IsMine)
                {
                    return pla.GetComponent<Unit>();
                }
            }

            return null;
        }

        Unit GetMyPlayer()
        {
            GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
            foreach (var pla in players)
            {
                if (pla.GetComponent<PhotonView>().IsMine)
                {
                    return pla.GetComponent<Unit>();
                }
            }

            return null;
        }
        
    }
}
