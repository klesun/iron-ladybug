using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.GameLogic;
using GameLogic;
using Newtonsoft.Json;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;
using UnityEngine.Serialization;
using Util;
using Util.Controls;
using Util.Shorthands;
using Object = System.Object;

namespace Network {

    class RemotePlayer {
        public NetworkClient client;
        public byte chanId;
        public HeroControl hero;
    }

    /**
     * will use this to do the transport stuff of separation of client code from server code
     */
    public class SplitNetworkManagerServer : NetworkManager {

        public CamStreaming camStreaming;

        private readonly bool USE_SPLIT_SCREEN = false;

        private List<RemotePlayer> clientConnections = new List<RemotePlayer>();
        float lastFrameSentAt = 0;
        List<Action> lateUpdates = new List<Action>();

        public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
        {
            var config = new ConnectionConfig();
            var chanId = config.AddChannel(QosType.UnreliableFragmented);
            // trying to set it big enough to pass an image in a message.
            // probably I'll have to fragment them, after all..
            var client = new NetworkClient(conn);
            client.Configure(config, 1);

            var playerObj = Instantiate(Sa.Inst().playerRef.gameObject);
            playerObj.SetActive(true);
            var hero = playerObj.GetComponent<HeroControl>();
            clientConnections.Add(new RemotePlayer {
                client = client,
                hero = hero,
                chanId = chanId,
            });
            foreach (var cam in playerObj.GetComponentsInChildren<Camera>()) {
                if (USE_SPLIT_SCREEN) {
                    if (clientConnections.Count > 1) {
                        cam.rect = new Rect(0,0,0.5f,1);
                    } else {
                        cam.rect = new Rect(0.5f,0,0.5f,1);
                    }
                } else {
                    cam.targetTexture = camStreaming.render;
                }
            }
            var input = new RemotePlayerInput();
            hero.SetInput(input);
            hero.output.Add(msg => {
                var dataStr = JsonConvert.SerializeObject(msg);
                client.SendUnreliable(MsgType.Highest + 1, new StringMessage(dataStr));
            });
            client.RegisterHandler(MsgType.Highest + 1, msg => {
                var str = msg.reader.ReadString();
                try {
                    var msgData = JsonConvert.DeserializeObject<Msg> (str);
                    input.HandleEvent(msgData);
                } catch (Exception exc) {
                    Debug.Log("Could not parse JSON message - " + exc.Message + " - " + str);
                }
            });
            base.OnServerAddPlayer(conn, playerControllerId);
        }

        private void SendHeroState(RemotePlayer remote)
        {
            var msg = new Msg {
                type = Msg.EType.State,
                state = new HeroState {
                    hp = remote.hero.npc.Health,
                    hpMax = NpcControl.START_HP,
                    mpFactor = remote.hero.npc.GetMpFactor(),
                    spells = new SpellBook(remote.hero).GetNames(),
                },
            };
            var dataStr = JsonConvert.SerializeObject(msg);
            remote.client.SendUnreliable(MsgType.Highest + 1, new StringMessage(dataStr));
        }

        private void Update()
        {
            if (Time.fixedTime - lastFrameSentAt > 0.05f) {
                lastFrameSentAt = Time.fixedTime;
                clientConnections.ForEach(remote => {
                    var bytes = camStreaming.GetFrameImgBytes();
                    remote.client.SendByChannel(MsgType.Highest + 2, new BytesMessage(bytes), remote.chanId);
                    SendHeroState(remote);
                });
            }
        }
    }
}