using UnityEngine.Networking;

namespace Network {
    public class BytesMessage : MessageBase
    {
        public byte[] value;

        public BytesMessage()
        {
        }

        public BytesMessage(byte[] bytes)
        {
            this.value = bytes;
        }

        public override void Deserialize(NetworkReader reader)
        {
            this.value = reader.ReadBytesAndSize();
        }

        public override void Serialize(NetworkWriter writer)
        {
            writer.WriteBytesFull(this.value);
        }
    }
}