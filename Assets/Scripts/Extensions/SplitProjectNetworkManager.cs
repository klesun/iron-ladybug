using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;
using Util;
using Util.Shorthands;
using Object = System.Object;

namespace Extensions {
    /**
     * will use this to do the transport stuff of separation of client code from server code
     */
    public class SplitProjectNetworkManager : NetworkManager {
    
        float lastSyncAt = 0;

        private List<NetworkConnection> serverConnections = new List<NetworkConnection>();

        public override void OnClientConnect(NetworkConnection conn)
        {
            serverConnections.Add(conn);
            var client = new NetworkClient(conn);
            client.RegisterHandler(MsgType.Highest + 1, msg => Debug.Log("message 123 came to client " + msg));
            //new NetworkClient(conn).RegisterHandler(0, msg => Debug.Log("message 23 came to client " + msg));
            Debug.Log("pizda you are connected to server" + conn);
            base.OnClientConnect(conn);
            Tls.Inst().timeout.Real(5, () => {
                Debug.Log("sending 23 message to server");
                client.SendUnreliable(MsgType.Highest + 1, new StringMessage("pizda please be nice to me"));
            });
        }
        
        public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
        {
            new NetworkClient(conn).RegisterHandler(MsgType.Highest + 1, msg => {
                var str = msg.reader.ReadString();
                Debug.Log("message 123 came to server " + Time.fixedTime + " " + str + " " + msg.GetType());
            });
            //new NetworkClient(conn).RegisterHandler(0, msg => Debug.Log("message 23 came to server " + msg));
            Debug.Log("zhopa player connected " + playerControllerId + " " + conn);
            base.OnServerAddPlayer(conn, playerControllerId);
            Tls.Inst().timeout.Real(5, () => {
                Debug.Log("sending 123 message to client");
                //new NetworkClient(conn).SendUnreliable(MsgType.Highest + 1, new StringMessage("zhopa welcome to server"));
            });
        }

        void OnGUI()
        {            
            var e = Event.current;
            var msgOpt = new Opt<object>(false, null);
            // GetKeyDown - to filter OS key auto-repeat
            if (e.type == EventType.KeyDown && Input.GetKeyDown(e.keyCode)) {
                msgOpt = new Opt<object>(true, new Msg{
                    type = "KeyDown",
                    data = e.keyCode,
                });
            } else if (e.type == EventType.KeyUp) {
                msgOpt = new Opt<object>(true, new Msg{
                    type = "KeyUp",
                    data = e.keyCode,
                });
            }
            msgOpt.get = (msg) => {
                serverConnections.ForEach(serv => {
                    string dataStr = JsonConvert.SerializeObject(msg);
                    new NetworkClient(serv).SendUnreliable(MsgType.Highest + 1, new StringMessage(dataStr));
                });
            };
        }

        void Update ()
        {
            if (Time.fixedTime - lastSyncAt > 2.5f) {
                lastSyncAt = Time.fixedTime;
                serverConnections.ForEach(serv => {
                    string dataStr = JsonConvert.SerializeObject(new {
                        type = "sync",
                    });
                    new NetworkClient(serv).SendUnreliable(MsgType.Highest + 1, new StringMessage(dataStr));
                });
            }
        }
    }
    
    class Msg {
        public string type;
        public Object data;
        public readonly float time = Time.fixedTime; 
    }
}