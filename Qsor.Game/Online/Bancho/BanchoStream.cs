using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Qsor.Game.Online.Bancho
{
    public interface ISerializer
    {
        void ReadFromStream(BanchoStreamReader sr);
        void WriteToStream(BanchoStreamWriter sw);
    }

    public interface IPacket : ISerializer
    {
        PacketId Id { get; }
    }
    
    public class BanchoStreamWriter : BinaryWriter
    {
        public BanchoStreamWriter(Stream s) : base(s, Encoding.UTF8)
        {
        }

        public long Length => BaseStream.Length;

        public static BanchoStreamWriter New() => new BanchoStreamWriter(new MemoryStream());

        public void Write(ISerializer seri)
        {
            seri.WriteToStream(this);
        }

        public void Write(IPacket packet)
        {
            using (var x = New()) {
                base.Write((short) packet.Id);
                base.Write((byte) 0);
                
                x.Write((ISerializer) packet);
                
                base.Write((int) x.Length);
                Write(x);

                x.Close();
            }
        }

        public void Write(IEnumerable<IPacket> packetList)
        {
            foreach (var p in packetList)
                Write(p);
        }

        public void Write(BinaryWriter w)
        {
            w.BaseStream.Position = 0;
            w.BaseStream.CopyTo(BaseStream);
        }

        public void Write(string value, bool nullable)
        {
            if (value == null && nullable)
            {
                base.Write((byte) 0);
            }
            else
            {
                base.Write((byte) 0x0b);
                base.Write(value + "");
            }
        }

        public override void Write(string value)
        {
            Write((byte) 0x0b);
            base.Write(value);
        }

        public override void Write(byte[] buff)
        {
            var length = buff.Length;
            Write(length);
            if (length > 0)
                base.Write(buff);
        }

        public void Write(List<int> list)
        {
            var count = (short) list.Count;
            Write(count);
            for (var i = 0; i < count; i++)
                Write(list[i]);
        }

        public void WriteRawBuffer(byte[] buff)
        {
            base.Write(buff);
        }

        public void WriteRawString(string value)
        {
            WriteRawBuffer(Encoding.UTF8.GetBytes(value));
        }
        
        public byte[] ToArray() => ((MemoryStream) BaseStream).ToArray();
    }

    public class BanchoStreamReader : BinaryReader
    {
        public BanchoStreamReader(Stream s) : base(s, Encoding.UTF8)
        {
        }

        public override string ReadString()
            => ReadByte() == 0x00 ? "" : base.ReadString();

        public byte[] ReadBytes()
        {
            var len = ReadInt32();
            return len > 0 ? base.ReadBytes(len) : len < 0 ? null : new byte[0];
        }

        public List<int> ReadInt32List()
        {
            var count = ReadInt16();
            if (count < 0)
                return new List<int>();
            var outList = new List<int>(count);
            for (var i = 0; i < count; i++)
                outList.Add(ReadInt32());
            return outList;
        }

        public T ReadPacket<T>() where T : IPacket, new()
        {
            var packet = new T();
            ReadInt16();
            ReadByte();
            var rawPacketData = ReadBytes();
            using (var x = BanchoStreamWriter.New())
            {
                x.WriteRawBuffer(rawPacketData);
                x.BaseStream.Position = 0;
                packet.ReadFromStream(new BanchoStreamReader(x.BaseStream));
            }

            return packet;
        }

        public T ReadData<T>() where T : ISerializer, new()
        {
            var data = new T();
            data.ReadFromStream(this);
            return data;
        }

        public byte[] ReadToEnd()
        {
            var x = new List<byte>();
            while (BaseStream.Position != BaseStream.Length)
                x.Add(ReadByte());

            return x.ToArray();
        }
    }
}
