using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.GameLogic;
using Newtonsoft.Json;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;
using Util;
using Util.Controls;
using Util.Shorthands;
using Object = System.Object;

namespace Network {
    /**
     * will use this to do the transport stuff of separation of client code from server code
     */
    public class SplitNetworkManagerServer : NetworkManager {

        private List<NetworkConnection> clientConnections = new List<NetworkConnection>();

        public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
        {
            //new NetworkClient(conn).RegisterHandler(0, msg => Debug.Log("message 23 came to server " + msg));
            Debug.Log("zhopa player connected " + playerControllerId + " " + conn + " " + conn.GetHashCode());
            clientConnections.Add(conn);
            
            var playerObj = Instantiate(Sa.Inst().playerRef.gameObject);
            playerObj.SetActive(true);
            var hero = playerObj.GetComponent<HeroControl>();
            foreach (var cam in playerObj.GetComponentsInChildren<Camera>()) {
                Debug.Log("zhop camera " + cam);
                if (clientConnections.Count > 1) {
                    cam.rect = new Rect(0,0,0.5f,1);
                } else {
                    cam.rect = new Rect(0.5f,0,0.5f,1);
                }
            }
            var input = new RemotePlayerInput();
            hero.SetInput(input);
            new NetworkClient(conn).RegisterHandler(MsgType.Highest + 1, msg => {
                var str = msg.reader.ReadString();
                Debug.Log("message 123 came to server " + Time.fixedTime + " " + str + " " + msg.GetType());
                try {
                    var msgData = JsonConvert.DeserializeObject<Msg> (str);
                    input.HandleEvent(msgData);
                } catch (Exception exc) {
                    Debug.Log("Could not parse JSON message - " + exc.Message);
                }
            });
            base.OnServerAddPlayer(conn, playerControllerId);
            Tls.Inst().timeout.Real(5, () => {
                Debug.Log("sending 123 message to client");
                //new NetworkClient(conn).SendUnreliable(MsgType.Highest + 1, new StringMessage("zhopa welcome to server"));
            });
        }
    }
}