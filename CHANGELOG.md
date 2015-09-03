---------------------------------
STSdb 4.0 version history:
---------------------------------
ver. 4.0.8 (2015-09-03)
 - changed: SortedSetExtensions now generates the same expressions for .NET and Mono. (thanks to corminlu from GitHub)
 - changed: DecimalExtensions now generates the same expressions for .NET and Mono.
 - improved: FindNext, FindAfter, FindBefore, FindPrev methods of SortedSet.

 The Mono Project has adopted the .NET implementations for some of their libraries.

ver. 4.0.7 (2015-01-13)
 - bug fixed: SortedSetExtension.ConstructFromSortedArray finally works under Mono

 Even after the bug fix, there are problems under Mono. Simple insert of 10 000 000 records with random keys randomly fails with mono runtime error:

//---
 Stacktrace:

  at <unknown> <0xffffffff>
  at (wrapper managed-to-native) object.__icall_wrapper_mono_gc_alloc_obj (intptr,intptr) <IL 0x0000e, 0xffffffff>
  at (wrapper alloc) object.AllocSmall (intptr) <IL 0x00047, 0xffffffff>
  at STSdb4.Database.XTable`2<long, STSdb4.General.Collections.Tick>.set_Item (long,STSdb4.General.Collections.Tick) [0x00002] in /home/chris/Desktop/STSdb4/STSdb4/Database/XTable.cs:36
  at STSdb4.GettingStarted.Program.Example () [0x0007b] in /home/chris/Desktop/STSdb4/STSdb4.GettingStarted/Program.cs:77
  at STSdb4.GettingStarted.Program.Main (string[]) [0x00001] in /home/chris/Desktop/STSdb4/STSdb4.GettingStarted/Program.cs:28
  at (wrapper runtime-invoke) <Module>.runtime_invoke_void_object (object,intptr,intptr,intptr) <IL 0x00050, 0xffffffff>

Native stacktrace:

	/usr/bin/mono() [0x4b73d8]
	/usr/bin/mono() [0x50f13b]
	/usr/bin/mono() [0x423d22]
	/lib/x86_64-linux-gnu/libpthread.so.0(+0x10340) [0x7f2a24df9340]
	/usr/bin/mono() [0x5fb127]
	/usr/bin/mono() [0x5fc703]
	/usr/bin/mono() [0x5d86ff]
	/usr/bin/mono() [0x5ddefe]
	/usr/bin/mono() [0x5de3b9]
	/usr/bin/mono() [0x5f4719]
	/usr/bin/mono() [0x5f47e4]
	[0x41c3f133]

Debug info from gdb:

Could not attach to process.  If your uid matches the uid of the target
process, check the setting of /proc/sys/kernel/yama/ptrace_scope, or try
again as the root user.  For more details, see /etc/sysctl.d/10-ptrace.conf
ptrace: Operation not permitted.
No threads.

=================================================================
Got a SIGSEGV while executing native code. This usually indicates
a fatal error in the mono runtime or one of the native libraries 
used by your application.
=================================================================
//---

This error in mono runtime is already reported in bugzilla: https://bugzilla.xamarin.com/show_bug.cgi?id=25751
We hope that it will be fixed soon.

ver. 4.0.6 (2014-12-16)
 - bug fixed: Persist<T> and DataPersist<T> do not work with AllowNull.All flag. This bug does not affect the default database behaviour (which creates the persist instances with AllowNull.OnlyMembers flag).
 - bug fixed: XFile does not Read correctly after open.
 - bug fixed: SortedSetExtension.ConstructFromSortedArray finally works under Mono without the slow workaround.
 - bug fixed: DecimalExtensions.GetDigits now works under Mono.
 - bug fixed: SlotsBuilder.BuildType now works under Mono.
 - other: the STSdb4 is built under .NET 4.5 Framework.

 The database should now work under Mono.

ver. 4.0.5 (2014-08-05)
 - bug fixed: GetDigits() method.

ver. 4.0.4 (2014-06-25)
 - incompatible change: Persist<T> and DataPersist<T> serialization logic is changed for structures, GUID and nullable types. In case the user uses such types, STSdb 4.0.4 is not compatible with 4.0.3. If the user uses classes or primitive types, 4.0.4 is compatible with 4.0.3.
 - added: TimeSpan support. TimeSpan is now a primitive STSdb 4.0 type.
 - improved: GetDigits() methods - this affects all floating-point delta compressions.
 - changed: default StorageEngine settings are changed to:
        public int INTERNAL_NODE_MIN_BRANCHES = 2;
        public int INTERNAL_NODE_MAX_BRANCHES = 5; //old value 4;
        public int INTERNAL_NODE_MAX_OPERATIONS_IN_ROOT = 8 * 1024;
        public int INTERNAL_NODE_MIN_OPERATIONS = 32 * 1024; //old value 64 * 1024;
        public int INTERNAL_NODE_MAX_OPERATIONS = 64 * 1024; //old value 128 * 1024;
        public int LEAF_NODE_MIN_RECORDS = 8 * 1024; //old value 16 * 1024;
        public int LEAF_NODE_MAX_RECORDS = 64 * 1024; //old value 128 * 1024;
   Default StorageEngine.CacheSize is now 64; //old value 32
   These settings make the database a little more ballanced. The overall speed is slightly improved, while the RAM usage is decreased with ~14%.
 - bug fixed: Persist<T> and DataPersist<T> do not generate proper code when the type is KeyValuePair or nullable type.
 - bug fixed: ValueToString<T> and DataToString<T> does not work in partial To/From conversion mode.

ver. 4.0.3 (2004-04-11)
 - bug fixed: memory usage issue (thanks: NaimingYao & mafshin) The W-tree leaf nodes were always being created with internally fixed capacity (100000 records by default). This caused high memory usage when the database stored medium or large size objects. The default settings still assume that the stored objects will be with relatively small size, but the user now can control LEAF_NODE_MAX_RECORDS and all other basic W-tree properties via the StorageEngine fields. For large objects we recommend reducing the internal/leaf nodes capacity:
    public int INTERNAL_NODE_MIN_BRANCHES = 2; //default values
    public int INTERNAL_NODE_MAX_BRANCHES = 4;
    public int INTERNAL_NODE_MAX_OPERATIONS_IN_ROOT = 8 * 1024;
    public int INTERNAL_NODE_MIN_OPERATIONS = 64 * 1024;
    public int INTERNAL_NODE_MAX_OPERATIONS = 128 * 1024;
    public int LEAF_NODE_MIN_RECORDS = 16 * 1024;
    public int LEAF_NODE_MAX_RECORDS = 128 * 1024;
 (See the new Memory Usage topic in STSdb developer's guide for details.)
 - renamed: StorageEngine properties:
 	* MIN_BRANCHES to INTERNAL_NODE_MIN_BRANCHES
	* MAX_BRANCHES to INTERNAL_NODE_MAX_BRANCHES
	* MAX_OPERATIONS_IN_ROOT to INTERNAL_NODE_MAX_OPERATIONS_IN_ROOT
	* MIN_OPERATIONS to INTERNAL_NODE_MIN_OPERATIONS
	* MAX_OPERATIONS to INTERNAL_NODE_MAX_OPERATIONS
 - added: StorageEngine properties:
	* LEAF_NODE_MIN_RECORDS
	* LEAF_NODE_MAX_RECORDS
 - added: Heap constructor with fileName parameter (thanks: mafshin):
	var engine = STSdb.FromHeap(new Heap("test.stsdb4", true));
 - updated: STSdb developer's guide to 4.0.3 API

ver. 4.0.2 (2014-03-19)
 - changed: XFile is now thread-safe.
 - improved: XFile.Write() method.
 - improved: caching of XFile.Length performance.
 - bug fixed: XFile.Read() throws exception when trying to read beyond the end of the file.
 - bug fixed: XFile.Read() in some cases does not initialize the input buffer array.
 - bug fixed: one of the CompareOption constructor ignores the sortOrder parameter.
 - renamed: IStorageEngine.OpenXTable(string name, DataType keyDataType, DataType recordDataType) to OpenXTablePortable
 - renamed: class XTable : ITable<IData, IData> to class XTablePortable : ITable<IData, IData>
 - renamed: IHeap.ObtainHandle() to ObtainNewHandle()
 - added: IHeap.DataSize property
 - added: IStorageEngine.Heap property
 - removed: IStorageEngine.DatabaseSize property
 - removed: bool useCompression and AllocationStrategy strategy parameters from STSdb factory methods
 - removed: the following members are removed from the IHeap interface:
	* GetUsedSpace() method
	* GetLatest() method
	* UseCompression property
	* CurrentVersion property
 - documentation: Developer's guide is strongly updated (all main concepts finally explained).
 - new: RemoteHeap and HeapServer now available (experimental).

ver. 4.0.1 (2014-01-16)
 - bug fixed: default KeyComparer & KeyEqualityComparer of a table are not created when the user previosly creates a table with record type equal to the key type of the second table (thanks: unruledboy). This bug affects the TypeEngine class which is responsible for holding the default comparers & persist classes for each type T, no matter key or record type.
 - bug fixed: OpenXTable() not workinkg with Guid keys (thanks: BilikTamas79)
 - bug fixed: in some cases table.Exists() does not work correctly (thanks: chinesefox)
 - bug fixed: in some cases, after delete all keys, table.Backward() does not work correctly
 - added: advanced OpenXTablePortable() method. The user can now really write custom transformers. This methods allows "encoding" the friendly user types to more compact portable internal data. The technique can be used for complete object transformation, partial object serialization etc. 

ver. 4.0 (2013-11-08)
 - INCOMPATIBLE CHANGE: STSdb 4.0 data format is not compatible with W4.0 RC3 build3.
 - new: IData technology - the database can now work directly with the user type. All generated .NET expressions - for compare, serialize, deserialize etc. have been rewritten to work directly with the user types. 
		No more object transformations are needed. This improves the database performance (especially for complex types). Note that in this case the database file may depends from the user types - the engine serializes their full names to rebuild the expressions on engine load.
		There are two main scenarios of using the database:
		* If you want to achieve the maximum performance of STSdb 4.0, use OpenXIndex<TKey, TRecord>. We recommended this way if the user types can be distributed with the database file or if the application uses the database as embedded.
		* If you want to keep the database completely independent from your types, use the OpenXIndexPortable<TKey, TRecord>() - with the automatically generated transformers.
 - new: database scheme - the IStorageEngine now supports Delete, Rename, Exists for tables.
 - new: nullable types supported
 - new: IDescriptor now available on each table & file (a small meta-info)
 - new: the user now has the possibility to replace the entire TKey/TRecord generated logic - for compare, serialize & deserialize etc.:
		* KeyType & RecordType
		* KeyComparer & KeyEqualityComparer
		* KeyPersist & RecordPersist
		* KeyIndexerPersist & RecordIndexerPersist (for compression mode)
		The database now supports custom comparers, persists etc. The custom logic must be provided immediately after the storage engine was opened, through the table descriptor properties.
 - improved: heap allocation engine; solved database file size. The file is also automatically truncated on engine.Commit().
 - improved: strongly improved delta compression & decompression speed - this affects all integer/float compressions.
 - improved: speed and size with byte[] key/records.
 - improved: engine.Commit()
 - bug fixed: OpenXIndex<TKey, TRecord>() does not work with byte[]
 - bug fixed: in some rare cases Forward()/Backward() methods does not return the records in ascending/descending order
 - bug fixed: the user cannot read 0 bytes from a XFile 
 - changed: system & data files are merged into single database file - STSdb.FromFile() now takes one filename parameter.
 - changed: XIndex is renamed to XTable
 - changed: XTable is now thread-safe
 - changed: OpenXTable & OpenXFile now return the same instance for the same name (the previous version always returned new instances).
 - changed: Heap allocation strategies:
		* AllocationStrategy.FromTheCurrentBlock (default)
		* AllocationStrategy.FromTheBeginning - even more effective in size, but affects the performance
 - removed: compressKeys & compressRecords flags. For primitive and linear types the engine uses compression mode; for more complicated types an uncompressed mode is used.
 - removed: Flush() method is no longer available. The tables and files are now automatically flushed on engine.Commit()
 - removed: ~5K lines formal source code (the library is less than 300kb)
 - other: The database now is even more powerfull and intuitive for use. For example:

    using (IStorageEngine engine = STSdb.FromFile("test.stsdb4"))
    {
        ITable<int, Tick> table = engine.OpenXTable<int, Tick>("table");

        for (int i = 0; i < 100000; i++)
            table[i] = new Tick();

        engine.Commit();
    }

	//some record type
	public class Tick
    {
        public string Symbol { get; set; }
        public DateTime Timestamp { get; set; }
        public double Bid { get; set; }
        public double Ask { get; set; }
        public long Volume { get; set; }
        public string Provider { get; set; }
    }

 - other: We also redesigned some of the .NET expressions and now they can be used as general tools, independently from the database. For example, there are public:
		* class Comparer<T> - which generates expressions for fast compare of T instances;
		* class EqualityComparer<T> - which generates expressions for fast equality comparer of T instances;
		* class Persist<T> - which generates expressions for fast serialization/deserialization of T instances.
		The last class - Persist<T>, outperforms on both size and speed the .NET BinaryFormatter and the C# port of Google's Protocol Buffers.
		We will describe these tools in an STSLabs article.
 - other: added small headers to most of the internal data formats (including the compressions). This should avoid future incompatibilities with the next minor releases.
 - other: starting STSdb 4.0 Developers Guide.
 - other: STSdb W4.0 is now just STSdb 4.0

Overall performance improvements:
		* write speed on sequential keys: +15%
		* read speed on sequential keys: +8%
		* read speed on random keys: +5%
		* database size on random keys: -15% (with the default allocation strategy)

ver. W4.0 RC3 build3 (2013-07-17)
 - bug fixed: StorageEngine.Commit() does not flush the system & user data (thanks: unruledboy)
 - changed: STSdb.FromStream() is now public

ver. W4.0 RC3 build2 (2013-07-11)
 - INCOMPATIBLE CHANGE: STSdb W4.0 RC3 build2 data format is not compatible with W4.0 RC3.
 - bug fixed: KeyDescriptor default comparers flag makes incompatible IIndex<TKey, TRecord> table with the appropriate IIndex<IData, IData> table.
 - bug fixed: Dictionary<byte[], TRecord> is not created with the appropriate IEqualityComparer instance.
 - bug fixed: the engine throws NotSupportedException when a XIndex<byte[], byte[]> is opened.
 - improved: Dictionary<TKey, TRecord> serialization/deserilization, when TKey is byte[] or string.
 - added: support of Guid type in TKey & TRecord.

Supported types for TKey:

1.	Primitive STSdb types – Boolean, Char, SByte, Byte, Int16, UInt16, Int32, UInt32, Int64, UInt64, Single, Double, Decimal, DateTime, String, byte[];
2.	Enums;
3.	Guid;
4.	Classes (with public default constructor) and structures, containing public read/write properties or fields with types from [1-3];
5.	Classes (with public default constructor) and structures, containing public read/write properties or fields with types from [1-4];
6.	Other types that can be transformed to one of the IData successors, available in STSdb4 - Data<TSlot0, TSlot1, ...>

Supported types for TRecord:

1.	Primitive STSdb types – Boolean, Char, SByte, Byte, Int16, UInt16, Int32, UInt32, Int64, UInt64, Single, Double, Decimal, DateTime, String, byte[];
2.	Enums;
3.	Guid;
4.	T[], List<T>, KeyValuePair<K, V> and Dictionary<K, V>, where T, K and V are types from [1-3];
5.	Classes (with public default constructor) and structures, containing public read/write properties or fields with types from [1-4];
6.	Classes (with public default constructor) and structures, containing public read/write properties or fields with types from [1-5];
7.	Classes (with public default constructor) and structures, containing public read/write properties or fields with types T[], List<T> or KeyValuePair<K, V>, where T, K and V are types from [1-6];
8.	Classes (with public default constructor) and structures, containing public read/write properties or fields with types Dictionary<K, V>, where K is type from [1-3] and V is type from [1-7];
9.	Other types that can be transformed to one of the IData successors, available in STSdb4 - Data<TSlot0, TSlot1, ...>

ver. W4.0 RC3 (2013-07-09)
- INCOMPATIBLE CHANGE: STSdb W4.0 RC3 data format is not compatible with W4.0 RC2. We preserve the right to improve the internal data format until final W4.0 release reached.
- added: support of T[], List<T>, Dictionary<K, V>, KeyValuePair<K, V> and enums in XIndex<TKey, TRecord> record types.

Supported types for TKey:

1.	Primitive STSdb types – Boolean, Char, SByte, Byte, Int16, UInt16, Int32, UInt32, Int64, UInt64, Single, Double, Decimal, DateTime, String, byte[];
2.	Enums;
3.	Classes (with public default constructor) and structures, containing public read/write properties or fields with types from [1-2];
4.	Classes (with public default constructor) and structures, containing public read/write properties or fields with types from [1-3];
5.	Other types that can be transformed to one of the IData successors, available in STSdb4 - Data<TSlot0, TSlot1, ...>

Supported types for TRecord:

1.	Primitive STSdb types – Boolean, Char, SByte, Byte, Int16, UInt16, Int32, UInt32, Int64, UInt64, Single, Double, Decimal, DateTime, String, byte[];
2.	Enums;
3.	T[], List<T>, KeyValuePair<K, V> and Dictionary<K, V>, where T, K and V are types from [1-2];
4.	Classes (with public default constructor) and structures, containing public read/write properties or fields with types from [1-3];
5.	Classes (with public default constructor) and structures, containing public read/write properties or fields with types from [1-4];
6.	Classes (with public default constructor) and structures, containing public read/write properties or fields with types T[], List<T> or KeyValuePair<K, V>, where T, K and V are types from [1-5];
7.	Classes (with public default constructor) and structures, containing public read/write properties or fields with types Dictionary<K, V>, where K is type from [1-2] and V is type from [1-5];
8.	Other types that can be transformed to one of the IData successors, available in STSdb4 - Data<TSlot0, TSlot1, ...>

Examples:

    public class Key
    {
        public string A { get; set; }
        public int B { get; set; }
        public SubKey C { get; set; }
        public byte[] D { get; set; }
    }

    public class SubKey
    {
        public float A { get; set; }
        public float B { get; set; }
    }

    public class Record
    {
        public string A { get; set; }
        public double B { get; set; }
        public List<int> List { get; set; }
        public Dictionary<int, string> Map { get; set; }
        public long[] Array { get; set; }
        public KeyValuePair<int, string> KV { get; set; }
    }

    public class Items
    {
        public List<List<long>> List { get; set; }
        Dictionary<int, Record> Records { get; set; }
        KeyValuePair<int[], double[]> KV { get; set; }
        public int[][][] Array { get; set; }
    }

	var table1 = engine.OpenXIndex<Key, byte[]>("table1");
	var table2 = engine.OpenXIndex<int, Record>("table2");
	var table3 = engine.OpenXIndex<Key, Items>("table3");

You can combine and nest almost any types. The IData engine automatically generates the appropriate STSdb environment code for compare, serialization, deserialization etc.

- changed: IStorageEngine interface

old:
    public interface IStorageEngine : IDisposable
    {
		...
        IIndex<IData, IData> OpenXIndex(DataType keyType, DataType recordType, params string[] path);
        IIndex<IData, IData> OpenXIndex(DataType keyType, DataType recordType, bool compressKeys, bool compressRecords, params string[] path);
		...
        IIndex<object[], object[]> OpenXIndexPrimitive(DataType keyType, DataType recordType, params string[] path);
        IIndex<object[], object[]> OpenXIndexPrimitive(DataType keyType, DataType recordType, bool compressKeys, bool compressRecords, params string[] path);
		...
    }

new:
    public interface IStorageEngine : IDisposable
    {
		...
        IIndex<IData, IData> OpenXIndex(DataType[] keySlotes, DataType[] recordSlotes, bool compressKeys, bool compressRecords, params string[] path);
        IIndex<IData, IData> OpenXIndex(DataType[] keySlotes, DataType[] recordSlotes, params string[] path);
		...
        IIndex<object[], object[]> OpenXIndexPrimitive(DataType[] keySlotes, DataType[] recordSlotes, bool compressKeys, bool compressRecords, params string[] path);
        IIndex<object[], object[]> OpenXIndexPrimitive(DataType[] keySlotes, DataType[] recordSlotes, params string[] path);
		...
    }

The change affects only non-generic XIndex API. Each key or record slot is described with its own DataType.

old:
	IIndex<IData, IData> table = engine.OpenXIndex(DataType.Slotes(DataType.DateTime, DataType.Int32), DataType.Slotes(DataType.String, DataType.Double, DataType.Double), "table");
new:
	IIndex<IData, IData> table = engine.OpenXIndex(new DataType[] { DataType.DateTime, DataType.Int32 }, new DataType[] { DataType.String, DataType.Double, DataType.Double }, "table");

- changed: IDataTransformer<T> interface:

old:
    public interface IDataTransformer<T>
    {
        T FromIData(IData data);
        IData ToIData(T item);
        DataType DataType { get; }
    }

new:
    public interface IDataTransformer<T>
    {
        IData ToIData(T item);
        T FromIData(IData data);
        DataType[] SlotTypes { get; }
    }

ver. W4.0 RC2 (2013-06-19)
- INCOMPATIBLE CHANGE: STSdb W4.0 RC2 data format is not compatible with W4.0 RC. If you want to upgrate you should make some csv-export/import from RC to RC2. We preserve the right to improve the internal data format until final W4.0 release reached.
- new: Client/Server is now available.
	//On the client side, a client connection is opened with:
	using (IStorageEngine engine = STSdb.FromNetwork(host, port))
    {
    }

	//On the server side, the server is started with:
	using (IStorageEngine engine = STSdb.FromFile("stsdb4.sys", "stsdb4.dat"))
    {
        var server = STSdb.CreateServer(engine, port);
        server.Start();

        //server is ready for connections

        server.Stop();
    }
	The created server instance will listen on the specified port and receive/send data from/to the clients.
- new: DataType class is now available. DataType is responsible for describing key & record IData types for the non generic XIndex tables (IIndex<IData, IData> tables):
	* old:
	    XIndex<IData, IData> table = engine.OpenXIndex(typeof(Data<int>), typeof(Data<string>), "table");
	* new:
		IIndex<IData, IData> table = engine.OpenXIndex(DataType.Int32, DataType.String, "table");
	* old:
		XIndex<IData, IData> table = engine.OpenXIndex(typeof(Data<long>), typeof(Data<string, DateTime, double, double, long, string>), "table");
	* new:
	    DataType keyType = DataType.Int64;
		DataType recordType = DataType.Slotes(
			DataType.String,
			DataType.DateTime,
			DataType.Double,
			DataType.Double,
			DataType.Int64,
			DataType.String
		);
		IIndex<IData, IData> table = engine.OpenXIndex(keyType, recordType, "table");
	For now RC2 supports only primitive DataTypes and DataType.Slotes() of primitive types. In the final release the database will support List<T>, T[] and Dictionary<K, V>
- new: IIndex<TKey, TRecord> now supports public read/write fields
- changed: StorageEngine is now created via STSdb factory methods:
	* IStorageEngine STSdb.FromFile(systemFile, dataFile)
	* IStorageEngine STSdb.FromMemory()
	* IStorageEngine STSdb.FromNetwork(host, port)
- changed: IStorageEngine interface now works with IIndex<TKey, TRecord> interface, instead of XIndex<TKey, TRecord>
- changed: all DataTransformers are now generated with expressions (no more CodeDom)
- bug fixed: XIndex Forward/Backward methods don't work correctly if one of the hasFrom/hasTo flags is false (thanks: unruledboy)

ver. W4.0 RC (2013-05-18)

The main change in the new version is the STSdb core. STSdb 4.0 is based on WaterfallTree™ - a new patented data indexing structure.
W-tree effectively solves one of the most fundamental problems in the database world - speed degradation on random keys in real-time indexing.
W-tree allows fast random key inserts while keeping an outstanding performance on sequential keys.

There are lots of changes and improvements. We will keep our website updated with the new features and technologies.


