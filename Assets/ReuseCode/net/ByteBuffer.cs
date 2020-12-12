using UnityEngine;
using System.Collections;
using System.IO;
using System.Text;
using System;


namespace Zby
{
    public class ByteBuffer
    {
        MemoryStream stream = null;
        BinaryWriter writer = null;
        BinaryReader reader = null;

        public ByteBuffer()
        {
            stream = new MemoryStream();
            writer = new BinaryWriter(stream);
        }

        public ByteBuffer(byte[] data)
        {
            if (data != null)
            {
                stream = new MemoryStream(data);
                reader = new BinaryReader(stream);
            }
            else
            {
                stream = new MemoryStream();
                writer = new BinaryWriter(stream);
            }
        }
        public ByteBuffer(Stream read_stream) {
            if (read_stream != null) {
                //stream = new MemoryStream(data);
                reader = new BinaryReader(read_stream);
            } else {
                stream = new MemoryStream();
                writer = new BinaryWriter(stream);
            }
        }
        public void Close()
        {
            if (writer != null) writer.Close();
            if (reader != null) reader.Close();

            stream.Close();
            writer = null;
            reader = null;
            stream = null;
        }

        public ByteBuffer WriteByte(byte v)
        {
            writer.Write(v);
            return this;
        }

        public ByteBuffer WriteInt(int v)
        {
            writer.Write((int)v);
            return this;
        }
        public ByteBuffer WriteUInt(uint v)
        {
            writer.Write((uint)v);
            return this;
        }
        public ByteBuffer WriteShort(short v)
        {
            writer.Write((short)v);
            return this;
        }
        public ByteBuffer WriteUShort(ushort v)
        {
            writer.Write((ushort)v);
            return this;
        }

        public ByteBuffer WriteLong(long v)
        {
            writer.Write((long)v);
            return this;
        }

        public ByteBuffer WriteFloat(float v)
        {
            byte[] temp = BitConverter.GetBytes(v);
            Array.Reverse(temp);
            writer.Write(BitConverter.ToSingle(temp, 0));
            return this;
        }

        public ByteBuffer WriteDouble(double v)
        {
            byte[] temp = BitConverter.GetBytes(v);
            Array.Reverse(temp);
            writer.Write(BitConverter.ToDouble(temp, 0));
            return this;
        }

        public ByteBuffer WriteString(string v)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(v);
            this.WriteUShort((ushort)bytes.Length);
            writer.Write(bytes);
            return this;
        }

        public ByteBuffer WriteBytes(byte[] v)
        {
            this.WriteInt((int)v.Length);
            writer.Write(v);
            return this;
        }

        public ByteBuffer AppendBytes(byte[] v)
        {
            writer.Write(v);
            return this;
        }



        public byte ReadByte()
        {
            return reader.ReadByte();
        }

        public int ReadInt()
        {
            return (int)reader.ReadInt32();
        }
        public uint ReadUInt()
        {
            return (uint)reader.ReadUInt32();
        }

        public short ReadShort()
        {
            return (short)reader.ReadInt16();
        }

        public ushort ReadUShort()
        {
            return (ushort)reader.ReadUInt16();
        }


        public long ReadLong()
        {
            return (long)reader.ReadInt64();
        }

        public float ReadFloat()
        {
            byte[] temp = BitConverter.GetBytes(reader.ReadSingle());
            Array.Reverse(temp);
            return BitConverter.ToSingle(temp, 0);
        }

        public double ReadDouble()
        {
            byte[] temp = BitConverter.GetBytes(reader.ReadDouble());
            Array.Reverse(temp);
            return BitConverter.ToDouble(temp, 0);
        }

        public string ReadString()
        {
            ushort len = ReadUShort();
            byte[] buffer = new byte[len];
            buffer = reader.ReadBytes(len);
            return Encoding.UTF8.GetString(buffer);
        }

        public byte[] ReadBytes()
        {
            int len = ReadInt();
            return reader.ReadBytes(len);
        }
        public byte[] ReadLeftBytes()
        {
            long left = stream.Length - stream.Position;
            return reader.ReadBytes((int)left);
        }

        public byte[] ToBytes()
        {
            writer.Flush();
            return stream.ToArray();
        }

        public void Flush()
        {
            writer.Flush();
        }
    }
}