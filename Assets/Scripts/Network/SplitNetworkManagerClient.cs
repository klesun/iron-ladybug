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
using System.Collections.Generic;

namespace Network {
    /**
     * will use this to do the transport stuff of separation of client code from server code
     */
    public class SplitNetworkManagerClient : NetworkManager {
    
        float lastSyncAt = 0;
        V2 lastMousePos = new V2(); 

        private List<NetworkConnection> serverConnections = new List<NetworkConnection>();

        public override void OnClientConnect(NetworkConnection conn)
        {
            //new NetworkClient(conn).RegisterHandler(0, msg => Debug.Log("message 23 came to client " + msg));
            Debug.Log("pizda you are connected to server " + conn + " " + conn.GetHashCode());
            serverConnections.Add(conn);
            
            var client = new NetworkClient(conn);
            client.RegisterHandler(MsgType.Highest + 1, msg => Debug.Log("message 123 came to client " + msg));
            base.OnClientConnect(conn);
            Tls.Inst().timeout.Real(5, () => {
                Debug.Log("sending 23 message to server");
                client.SendUnreliable(MsgType.Highest + 1, new StringMessage("pizda please be nice to me"));
            });
        }

        void OnGUI()
        {            
            var e = Event.current;
            var msgs = new List<Msg>();
            // GetKeyDown - to filter OS key auto-repeat
            if ((e.type == EventType.KeyDown || e.type == EventType.MouseDown) && Input.GetKeyDown(e.keyCode)) {
                msgs.Add(new Msg{
                    type = Msg.EType.KeyDown,
                    keyCode = e.keyCode,
                });
            } else if (e.type == EventType.KeyUp || e.type == EventType.MouseUp) {
                msgs.Add(new Msg{
                    type = Msg.EType.KeyUp,
                    keyCode = e.keyCode,
                });
            } else if (e.type == EventType.Repaint || e.type == EventType.Layout) {
                // ignored if mouse position did not change
            } else {
                Debug.Log("unhandled event - " + e.type + " " + e);
            }
            msgs.ForEach((msg) => {
                serverConnections.ForEach(serv => {
                    var dataStr = JsonConvert.SerializeObject(msg);
                    new NetworkClient(serv).SendUnreliable(MsgType.Highest + 1, new StringMessage(dataStr));
                });
            });
        }

        void Update ()
        {
            var msgs = new List<Msg>();
            var mouseDelta = new V2{
                x = Input.GetAxis("Mouse X"), 
                y = Input.GetAxis("Mouse Y")
            };
            if (mouseDelta.toStd().magnitude > 0.000001f) {
                msgs.Add(new Msg{
                    type = Msg.EType.MouseMove,
                    mouseDelta = mouseDelta,
                });
            }
            lastMousePos = mouseDelta;
            if (Time.fixedTime - lastSyncAt > 2.5f) {
                lastSyncAt = Time.fixedTime;
                msgs.Add(new Msg{
                    type = Msg.EType.Sync,
                });
            }
            msgs.ForEach((msg) => {
                serverConnections.ForEach(serv => {
                    var dataStr = JsonConvert.SerializeObject(msg);
                    new NetworkClient(serv).SendUnreliable(MsgType.Highest + 1, new StringMessage(dataStr));
                });
            });
        }
    }

}