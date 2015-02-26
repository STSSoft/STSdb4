using STSdb4.General.IO;
using STSdb4.WaterfallTree;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STSdb4.Storage
{
    //public class DirectoryHeap : IHeap
    //{
    //    private AtomicFile systemFile;
    //    private Dictionary<long, Block> map = new Dictionary<long, Block>();
    //    private List<Block> forDelete = new List<Block>();
    //    private byte[] tag;
    //    private long maxHandle;
    //    private readonly object SyncRoot = new object();

    //    public string Directory {get; private set;}

    //    public DirectoryHeap(string directory)
    //    {
    //        if (String.IsNullOrEmpty(directory))
    //            directory = System.IO.Directory.GetCurrentDirectory();
    //        else if (!System.IO.Directory.Exists(directory))
    //            System.IO.Directory.CreateDirectory(directory);

    //        Directory = directory;

    //        systemFile = new AtomicFile(Path.Combine(directory, "system.stsdb4"));
    //        byte[] buffer = systemFile.Read();

    //        if (buffer.Length > 0)
    //        {
    //            using (MemoryStream ms = new MemoryStream(buffer))
    //                DeserializeSystem(ms);
    //        }
    //    }

    //    private string GetFileName(long handle, int version)
    //    {
    //        return Path.Combine(Directory, String.Format("{0}.{1}", handle, version));
    //    }

    //    private Stream CreateFile(long handle, int version, int size)
    //    {
    //        return new OptimizedFileStream(GetFileName(handle, version), FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite, size);
    //    }

    //    private Stream OpenFile(long handle, int version, int size)
    //    {
    //        return new OptimizedFileStream(GetFileName(handle, version), FileMode.Open, FileAccess.Read, FileShare.Read, size);
    //    }

    //    private void SerializeSystem(Stream stream)
    //    {
    //        BinaryWriter writer = new BinaryWriter(stream);

    //        writer.Write(maxHandle);

    //        //write map
    //        writer.Write(map.Count);
    //        foreach (var kv in map)
    //            kv.Value.Serialize(writer);

    //        //write tag
    //        var tag = Tag;
    //        writer.Write(tag != null);
    //        if (tag != null)
    //        {
    //            writer.Write(tag.Length);
    //            writer.Write(tag, 0, tag.Length);
    //        }
    //    }

    //    private void DeserializeSystem(Stream stream)
    //    {
    //        BinaryReader reader = new BinaryReader(stream);

    //        maxHandle = reader.ReadInt64();

    //        //read map
    //        map.Clear();
    //        for (int i = reader.ReadInt32(); i > 0; i--)
    //        {
    //            Block box = Block.Deserialize(reader);
    //            map[box.Handle] = box;
    //        }

    //        //read tag
    //        Tag = reader.ReadBoolean() ? reader.ReadBytes(reader.ReadInt32()) : null;
    //    }

    //    #region IHeap

    //    public long ObtainNewHandle()
    //    {
    //        lock (SyncRoot)
    //            return maxHandle++;
    //    }

    //    public void Release(long handle)
    //    {
    //        lock (SyncRoot)
    //        {
    //            Block block;
    //            if (!map.TryGetValue(handle, out block))
    //                return; //throw new ArgumentException("handle");

    //            if (block.Stream != null)
    //            {
    //                block.Stream.Close();
    //                block.Stream = null;
    //            }

    //            map.Remove(handle);
    //            forDelete.Add(block);
    //        }
    //    }

    //    public bool Exists(long handle)
    //    {
    //        lock (SyncRoot)
    //            return map.ContainsKey(handle);
    //    }

    //    public void Write(long handle, byte[] buffer, int index, int count)
    //    {
    //        lock (SyncRoot)
    //        {
    //            Block block;
    //            if (!map.TryGetValue(handle, out block))
    //                map[handle] = block = new Block(handle, 0, count, null);
    //            else
    //            {
    //                block.Stream.Close();
    //                block.Version++;
    //                block.Size = count;
    //            }

    //            block.Stream = CreateFile(handle, block.Version, count);
    //            block.Stream.Write(buffer, index, count);
    //            block.Stream.Flush();
    //        }
    //    }

    //    public byte[] Read(long handle)
    //    {
    //        lock (SyncRoot)
    //        {
    //            Block block;
    //            if (!map.TryGetValue(handle, out block))
    //                throw new ArgumentException("Invalid handle.");

    //            if (block.Stream == null)
    //                block.Stream = OpenFile(handle, block.Version, block.Size);

    //            byte[] buffer = new byte[block.Size];
    //            block.Stream.Seek(0, SeekOrigin.Begin);
    //            int readed = block.Stream.Read(buffer, 0, buffer.Length);

    //            if (readed != buffer.Length)
    //                throw new IOException(); //should never happen

    //            return buffer;
    //        }
    //    }

    //    public void Commit()
    //    {
    //        lock (SyncRoot)
    //        {
    //            //atomic write system data
    //            using (MemoryStream ms = new MemoryStream())
    //            {
    //                SerializeSystem(ms);
    //                byte[] buffer = ms.GetBuffer();
    //                systemFile.Write(buffer, 0, (int)ms.Length);
    //            }

    //            //delete old blocks
    //            if (forDelete.Count > 0)
    //            {
    //                foreach (var block in forDelete)
    //                    File.Delete(GetFileName(block.Handle, block.Version));

    //                forDelete.Clear();
    //            }
    //        }
    //    }

    //    public void Close()
    //    {
    //        lock (SyncRoot)
    //        {
    //            foreach (var kv in map)
    //            {
    //                var block = kv.Value;

    //                if (block.Stream != null)
    //                {
    //                    block.Stream.Close();
    //                    block.Stream = null;
    //                }
    //            }

    //            systemFile.Close();
    //        }
    //    }

    //    public byte[] Tag
    //    {
    //        get
    //        {
    //            lock (SyncRoot)
    //                return tag;
    //        }
    //        set
    //        {
    //            lock (SyncRoot)
    //                tag = value;
    //        }
    //    }

    //    public long DataSize
    //    {
    //        get
    //        {
    //            lock (SyncRoot)
    //                return map.Sum(kv => kv.Value.Size);
    //        }
    //    }

    //    public long Size
    //    {
    //        get
    //        {
    //            lock (SyncRoot)
    //                return DataSize + forDelete.Sum(x => x.Size) + systemFile.Length;
    //        }
    //    }

    //    #endregion

    //    private class Block
    //    {
    //        public long Handle;
    //        public int Version;
    //        public int Size;

    //        public Stream Stream;

    //        private Block()
    //        {
    //        }

    //        public Block(long handle, int version, int size, Stream stream)
    //        {
    //            Handle = handle;
    //            Version = version;
    //            Size = size;
    //            Stream = stream;
    //        }

    //        public void Serialize(BinaryWriter writer)
    //        {
    //            writer.Write(Handle);
    //            writer.Write(Version);
    //            writer.Write(Size);
    //        }

    //        public static Block Deserialize(BinaryReader reader)
    //        {
    //            Block block = new Block();

    //            block.Handle = reader.ReadInt64();
    //            block.Version = reader.ReadInt32();
    //            block.Size = reader.ReadInt32();
    //            block.Stream = null;

    //            return block;
    //        }
    //    }
    //}
}
