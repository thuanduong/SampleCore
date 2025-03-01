using Cysharp.Threading.Tasks;
using Google.Protobuf;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public partial class ProtobufMessageParser : IMessageParser
{
    private readonly byte[] nextByteBuffer = new byte[1];
    private readonly List<byte> sizeByteList = new List<byte>();

    public IMessage Parse(byte[] rawMessage)
    {
        return null;
    }

    public byte[] ToByteArray(IMessage message)
    {
        return null;
    }


    public async UniTask<(int size, byte[] sizeByteArray)> GetNextMessageSize(Stream stream)
    {
        sizeByteList.Clear();
        byte newByte = 0;
        do
        {
            newByte = await ReceiveNextByte(stream);
            sizeByteList.Add(newByte);
        } while ((newByte & 0x80) != 0);

        var sizeByteArray = sizeByteList.ToArray();
        var codedInputStream = new CodedInputStream(sizeByteArray);

        return (size: (int)codedInputStream.ReadUInt32(), sizeByteArray);
    }

    private async UniTask<byte> ReceiveNextByte(Stream stream)
    {
        var length = 0;
        while (length == 0)
        {
            length = await stream.ReadAsync(nextByteBuffer, 0, nextByteBuffer.Length)
                                 .ConfigureAwait(false);
        }

        return nextByteBuffer[0];
    }
}
