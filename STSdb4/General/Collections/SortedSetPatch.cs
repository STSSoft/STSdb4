using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Security;
using System.Threading;

namespace STSdb4.General.Collections
{
    //[Serializable, DebuggerTypeProxy(typeof(SortedSetDebugView<>)), DebuggerDisplay("Count = {Count}")]
    //public class SortedSetPatch<T> : ISet<T>, ICollection<T>, IEnumerable<T>, ICollection, IEnumerable, ISerializable, IDeserializationCallback
    //{
    //    // Fields
    //    private object _syncRoot;
    //    private IComparer<T> comparer;
    //    private const string ComparerName = "Comparer";
    //    private int count;
    //    private const string CountName = "Count";
    //    private const string EnumStartName = "EnumStarted";
    //    private const string EnumVersionName = "EnumVersion";
    //    private const string ItemsName = "Items";
    //    private static string lBoundActiveName;
    //    private static string maxName;
    //    private static string minName;
    //    private const string NodeValueName = "Item";
    //    private const string ReverseName = "Reverse";
    //    private Node root;
    //    private SerializationInfo siInfo;
    //    internal const int StackAllocThreshold = 100;
    //    private const string TreeName = "Tree";
    //    private static string uBoundActiveName;
    //    private int version;
    //    private const string VersionName = "Version";

    //    // Methods
    //    static SortedSetPatch()
    //    {
    //        SortedSetPatch<T>.minName = "Min";
    //        SortedSetPatch<T>.maxName = "Max";
    //        SortedSetPatch<T>.lBoundActiveName = "lBoundActive";
    //        SortedSetPatch<T>.uBoundActiveName = "uBoundActive";
    //    }

    //    public SortedSetPatch()
    //    {
    //        this.comparer = Comparer<T>.Default;
    //    }

    //    public SortedSetPatch(IComparer<T> comparer)
    //    {
    //        if (comparer == null)
    //        {
    //            this.comparer = Comparer<T>.Default;
    //        }
    //        else
    //        {
    //            this.comparer = comparer;
    //        }
    //    }

    //    public SortedSetPatch(IEnumerable<T> collection)
    //        : this(collection, Comparer<T>.Default)
    //    {
    //    }

    //    public SortedSetPatch(IEnumerable<T> collection, IComparer<T> comparer)
    //        : this(comparer)
    //    {
    //        if (collection == null)
    //        {
    //            throw new ArgumentNullException("collection");
    //        }
    //        SortedSetPatch<T> set = collection as SortedSetPatch<T>;
    //        SortedSetPatch<T> set2 = collection as TreeSubSet;
    //        if (((set != null) && (set2 == null)) && SortedSetPatch<T>.AreComparersEqual((SortedSetPatch<T>)this, set))
    //        {
    //            if (set.Count == 0)
    //            {
    //                this.count = 0;
    //                this.version = 0;
    //                this.root = null;
    //            }
    //            else
    //            {
    //                Stack<Node> stack = new Stack<Node>((2 * SortedSetPatch<T>.log2(set.Count)) + 2);
    //                Stack<Node> stack2 = new Stack<Node>((2 * SortedSetPatch<T>.log2(set.Count)) + 2);
    //                Node root = set.root;
    //                Node item = (root != null) ? new Node(root.Item, root.IsRed) : null;
    //                this.root = item;
    //                while (root != null)
    //                {
    //                    stack.Push(root);
    //                    stack2.Push(item);
    //                    item.Left = (root.Left != null) ? new Node(root.Left.Item, root.Left.IsRed) : null;
    //                    root = root.Left;
    //                    item = item.Left;
    //                }
    //                while (stack.Count != 0)
    //                {
    //                    root = stack.Pop();
    //                    item = stack2.Pop();
    //                    Node right = root.Right;
    //                    Node left = null;
    //                    if (right != null)
    //                    {
    //                        left = new Node(right.Item, right.IsRed);
    //                    }
    //                    item.Right = left;
    //                    while (right != null)
    //                    {
    //                        stack.Push(right);
    //                        stack2.Push(left);
    //                        left.Left = (right.Left != null) ? new Node(right.Left.Item, right.Left.IsRed) : null;
    //                        right = right.Left;
    //                        left = left.Left;
    //                    }
    //                }
    //                this.count = set.count;
    //                this.version = 0;
    //            }
    //        }
    //        else
    //        {
    //            List<T> list = new List<T>(collection);
    //            list.Sort(this.comparer);
    //            for (int i = 1; i < list.Count; i++)
    //            {
    //                if (comparer.Compare(list[i], list[i - 1]) == 0)
    //                {
    //                    list.RemoveAt(i);
    //                    i--;
    //                }
    //            }
    //            this.root = SortedSetPatch<T>.ConstructRootFromSortedArray(list.ToArray(), 0, list.Count - 1, null);
    //            this.count = list.Count;
    //            this.version = 0;
    //        }
    //    }

    //    protected SortedSetPatch(SerializationInfo info, StreamingContext context)
    //    {
    //        this.siInfo = info;
    //    }

    //    public bool Add(T item)
    //    {
    //        return this.AddIfNotPresent(item);
    //    }

    //    private void AddAllElements(IEnumerable<T> collection)
    //    {
    //        foreach (T local in collection)
    //        {
    //            if (!this.Contains(local))
    //            {
    //                this.Add(local);
    //            }
    //        }
    //    }

    //    internal virtual bool AddIfNotPresent(T item)
    //    {
    //        if (this.root == null)
    //        {
    //            this.root = new Node(item, false);
    //            this.count = 1;
    //            this.version++;
    //            return true;
    //        }
    //        Node root = this.root;
    //        Node node = null;
    //        Node grandParent = null;
    //        Node greatGrandParent = null;
    //        this.version++;
    //        int num = 0;
    //        while (root != null)
    //        {
    //            num = this.comparer.Compare(item, root.Item);
    //            if (num == 0)
    //            {
    //                this.root.IsRed = false;
    //                return false;
    //            }
    //            if (SortedSetPatch<T>.Is4Node(root))
    //            {
    //                SortedSetPatch<T>.Split4Node(root);
    //                if (SortedSetPatch<T>.IsRed(node))
    //                {
    //                    this.InsertionBalance(root, ref node, grandParent, greatGrandParent);
    //                }
    //            }
    //            greatGrandParent = grandParent;
    //            grandParent = node;
    //            node = root;
    //            root = (num < 0) ? root.Left : root.Right;
    //        }
    //        Node current = new Node(item);
    //        if (num > 0)
    //        {
    //            node.Right = current;
    //        }
    //        else
    //        {
    //            node.Left = current;
    //        }
    //        if (node.IsRed)
    //        {
    //            this.InsertionBalance(current, ref node, grandParent, greatGrandParent);
    //        }
    //        this.root.IsRed = false;
    //        this.count++;
    //        return true;
    //    }

    //    private static bool AreComparersEqual(SortedSetPatch<T> set1, SortedSetPatch<T> set2)
    //    {
    //        return set1.Comparer.Equals(set2.Comparer);
    //    }

    //    internal virtual bool BreadthFirstTreeWalk(TreeWalkPredicate<T> action)
    //    {
    //        if (this.root != null)
    //        {
    //            List<Node> list = new List<Node> {
    //                this.root
    //            };
    //            while (list.Count != 0)
    //            {
    //                Node node = list[0];
    //                list.RemoveAt(0);
    //                if (!action(node))
    //                {
    //                    return false;
    //                }
    //                if (node.Left != null)
    //                {
    //                    list.Add(node.Left);
    //                }
    //                if (node.Right != null)
    //                {
    //                    list.Add(node.Right);
    //                }
    //            }
    //        }
    //        return true;
    //    }

    //    [SecurityCritical]
    //    private unsafe ElementCount CheckUniqueAndUnfoundElements(IEnumerable<T> other, bool returnIfUnfound)
    //    {
    //        ElementCount count;
    //        if (this.Count != 0)
    //        {
    //            BitHelper helper;
    //            int length = BitHelper.ToIntArrayLength(this.Count);
    //            //if (length <= 100)
    //            //{
    //            //    int* bitArrayPtr = (int*) stackalloc byte[(((IntPtr) length) * 4)];
    //            //    helper = new BitHelper(bitArrayPtr, length);
    //            //int* bitArrayPtr = stackalloc int[length];
    //            //}
    //            //else
    //            {
    //                int[] bitArray = new int[length];
    //                helper = new BitHelper(bitArray, length);
    //            }
    //            int num4 = 0;
    //            int num5 = 0;
    //            foreach (T local in other)
    //            {
    //                int bitPosition = this.InternalIndexOf(local);
    //                if (bitPosition >= 0)
    //                {
    //                    if (!helper.IsMarked(bitPosition))
    //                    {
    //                        helper.MarkBit(bitPosition);
    //                        num5++;
    //                    }
    //                }
    //                else
    //                {
    //                    num4++;
    //                    if (returnIfUnfound)
    //                    {
    //                        break;
    //                    }
    //                }
    //            }
    //            count.uniqueCount = num5;
    //            count.unfoundCount = num4;
    //            return count;
    //        }
    //        int num = 0;
    //        using (IEnumerator<T> enumerator = other.GetEnumerator())
    //        {
    //            while (enumerator.MoveNext())
    //            {
    //                T current = enumerator.Current;
    //                num++;
    //                goto Label_0039;
    //            }
    //        }
    //    Label_0039:
    //        count.uniqueCount = 0;
    //        count.unfoundCount = num;
    //        return count;
    //    }

    //    public virtual void Clear()
    //    {
    //        this.root = null;
    //        this.count = 0;
    //        this.version++;
    //    }

    //    public void ConstructFromSortedArray(SortedSetPatch<T> set, T[] array, int index, int count)
    //    {
    //        set.root = SortedSetPatch<T>.ConstructRootFromSortedArray(array, index, index + count - 1, null);
    //        set.count = count;
    //        set.version++; //this.version = 0; 
    //    }

    //    public void ConstructFromSortedArray(T[] array, int index, int count)
    //    {
    //        this.root = ConstructRootFromSortedArray(array, index, index + count - 1, null);
    //        this.count = count;
    //        this.version++; //this.version = 0; 
    //    }

    //    private static Node ConstructRootFromSortedArray(T[] arr, int startIndex, int endIndex, Node redNode)
    //    {
    //        int num = (endIndex - startIndex) + 1;
    //        if (num == 0)
    //        {
    //            return null;
    //        }
    //        Node node = null;
    //        switch (num)
    //        {
    //            case 1:
    //                node = new Node(arr[startIndex], false);
    //                if (redNode != null)
    //                {
    //                    node.Left = redNode;
    //                }
    //                return node;

    //            case 2:
    //                node = new Node(arr[startIndex], false)
    //                {
    //                    Right = new Node(arr[endIndex], false)
    //                };
    //                node.Right.IsRed = true;
    //                if (redNode != null)
    //                {
    //                    node.Left = redNode;
    //                }
    //                return node;

    //            case 3:
    //                node = new Node(arr[startIndex + 1], false)
    //                {
    //                    Left = new Node(arr[startIndex], false),
    //                    Right = new Node(arr[endIndex], false)
    //                };
    //                if (redNode != null)
    //                {
    //                    node.Left.Left = redNode;
    //                }
    //                return node;
    //        }
    //        int index = (startIndex + endIndex) / 2;
    //        node = new Node(arr[index], false)
    //        {
    //            Left = SortedSetPatch<T>.ConstructRootFromSortedArray(arr, startIndex, index - 1, redNode)
    //        };
    //        if ((num % 2) == 0)
    //        {
    //            node.Right = SortedSetPatch<T>.ConstructRootFromSortedArray(arr, index + 2, endIndex, new Node(arr[index + 1], true));
    //            return node;
    //        }
    //        node.Right = SortedSetPatch<T>.ConstructRootFromSortedArray(arr, index + 1, endIndex, null);
    //        return node;
    //    }

    //    public virtual bool Contains(T item)
    //    {
    //        return (this.FindNode(item) != null);
    //    }

    //    public bool TryGetValue(T item, out T result)
    //    {
    //        Node node = FindNode(item);

    //        if (node != null)
    //        {
    //            result = node.Item;
    //            return true;
    //        }

    //        result = default(T);
    //        return false;
    //    }

    //    public bool Replace(T item, Func<T, T, T> onExist = null)
    //    {
    //        if (this.root == null)
    //        {
    //            this.root = new Node(item, false);
    //            this.count = 1;
    //            this.version++;
    //            return false;
    //        }

    //        SortedSetPatch<T>.Node root = this.root;
    //        SortedSetPatch<T>.Node node = null;
    //        SortedSetPatch<T>.Node grandParent = null;
    //        SortedSetPatch<T>.Node greatGrandParent = null;
    //        this.version++;
    //        int cmp = 0;
    //        var comparer = this.comparer;

    //        while (root != null)
    //        {
    //            cmp = comparer.Compare(item, root.Item);
    //            if (cmp == 0)
    //            {
    //                this.root.IsRed = false;
    //                root.Item = onExist != null ? onExist(root.Item, item) : item;
    //                return true;
    //            }

    //            if (SortedSetPatch<T>.Is4Node(root))
    //            {
    //                SortedSetPatch<T>.Split4Node(root);
    //                if (SortedSetPatch<T>.IsRed(node))
    //                    this.InsertionBalance(root, ref node, grandParent, greatGrandParent);
    //            }

    //            greatGrandParent = grandParent;
    //            grandParent = node;
    //            node = root;
    //            root = (cmp < 0) ? root.Left : root.Right;
    //        }

    //        SortedSetPatch<T>.Node current = new SortedSetPatch<T>.Node(item);
    //        if (cmp > 0)
    //            node.Right = current;
    //        else
    //            node.Left = current;

    //        if (node.IsRed)
    //            this.InsertionBalance(current, ref node, grandParent, greatGrandParent);

    //        this.root.IsRed = false;
    //        this.count++;
    //        return false;
    //    }

    //    private bool ContainsAllElements(IEnumerable<T> collection)
    //    {
    //        foreach (T local in collection)
    //        {
    //            if (!this.Contains(local))
    //            {
    //                return false;
    //            }
    //        }
    //        return true;
    //    }

    //    public void CopyTo(T[] array)
    //    {
    //        this.CopyTo(array, 0, this.Count);
    //    }

    //    public void CopyTo(T[] array, int index)
    //    {
    //        this.CopyTo(array, index, this.Count);
    //    }

    //    public void CopyTo(T[] array, int index, int count)
    //    {
    //        if (array == null)
    //        {
    //            ThrowHelper.ThrowArgumentNullException(ExceptionArgument.array);
    //        }
    //        if (index < 0)
    //        {
    //            ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.index);
    //        }
    //        if (count < 0)
    //        {
    //            throw new ArgumentOutOfRangeException("count", SR.GetString("ArgumentOutOfRange_NeedNonNegNum"));
    //        }
    //        if ((index > array.Length) || (count > (array.Length - index)))
    //        {
    //            throw new ArgumentException(SR.GetString("Arg_ArrayPlusOffTooSmall"));
    //        }
    //        count += index;
    //        this.InOrderTreeWalk(delegate(Node node)
    //        {
    //            if (index >= count)
    //            {
    //                return false;
    //            }
    //            array[index++] = node.Item;
    //            return true;
    //        });
    //    }

    //    public static IEqualityComparer<SortedSetPatch<T>> CreateSetComparer()
    //    {
    //        return new SortedSetEqualityComparer<T>();
    //    }

    //    public static IEqualityComparer<SortedSetPatch<T>> CreateSetComparer(IEqualityComparer<T> memberEqualityComparer)
    //    {
    //        return new SortedSetEqualityComparer<T>(memberEqualityComparer);
    //    }

    //    internal virtual bool DoRemove(T item)
    //    {
    //        if (this.root == null)
    //        {
    //            return false;
    //        }
    //        this.version++;
    //        Node root = this.root;
    //        Node parent = null;
    //        Node node3 = null;
    //        Node match = null;
    //        Node parentOfMatch = null;
    //        bool flag = false;
    //        while (root != null)
    //        {
    //            if (SortedSetPatch<T>.Is2Node(root))
    //            {
    //                if (parent == null)
    //                {
    //                    root.IsRed = true;
    //                }
    //                else
    //                {
    //                    Node sibling = SortedSetPatch<T>.GetSibling(root, parent);
    //                    if (sibling.IsRed)
    //                    {
    //                        if (parent.Right == sibling)
    //                        {
    //                            SortedSetPatch<T>.RotateLeft(parent);
    //                        }
    //                        else
    //                        {
    //                            SortedSetPatch<T>.RotateRight(parent);
    //                        }
    //                        parent.IsRed = true;
    //                        sibling.IsRed = false;
    //                        this.ReplaceChildOfNodeOrRoot(node3, parent, sibling);
    //                        node3 = sibling;
    //                        if (parent == match)
    //                        {
    //                            parentOfMatch = sibling;
    //                        }
    //                        sibling = (parent.Left == root) ? parent.Right : parent.Left;
    //                    }
    //                    if (SortedSetPatch<T>.Is2Node(sibling))
    //                    {
    //                        SortedSetPatch<T>.Merge2Nodes(parent, root, sibling);
    //                    }
    //                    else
    //                    {
    //                        TreeRotation rotation = SortedSetPatch<T>.RotationNeeded(parent, root, sibling);
    //                        Node newChild = null;
    //                        switch (rotation)
    //                        {
    //                            case TreeRotation.LeftRotation:
    //                                sibling.Right.IsRed = false;
    //                                newChild = SortedSetPatch<T>.RotateLeft(parent);
    //                                break;

    //                            case TreeRotation.RightRotation:
    //                                sibling.Left.IsRed = false;
    //                                newChild = SortedSetPatch<T>.RotateRight(parent);
    //                                break;

    //                            case TreeRotation.RightLeftRotation:
    //                                newChild = SortedSetPatch<T>.RotateRightLeft(parent);
    //                                break;

    //                            case TreeRotation.LeftRightRotation:
    //                                newChild = SortedSetPatch<T>.RotateLeftRight(parent);
    //                                break;
    //                        }
    //                        newChild.IsRed = parent.IsRed;
    //                        parent.IsRed = false;
    //                        root.IsRed = true;
    //                        this.ReplaceChildOfNodeOrRoot(node3, parent, newChild);
    //                        if (parent == match)
    //                        {
    //                            parentOfMatch = newChild;
    //                        }
    //                        node3 = newChild;
    //                    }
    //                }
    //            }
    //            int num = flag ? -1 : this.comparer.Compare(item, root.Item);
    //            if (num == 0)
    //            {
    //                flag = true;
    //                match = root;
    //                parentOfMatch = parent;
    //            }
    //            node3 = parent;
    //            parent = root;
    //            if (num < 0)
    //            {
    //                root = root.Left;
    //            }
    //            else
    //            {
    //                root = root.Right;
    //            }
    //        }
    //        if (match != null)
    //        {
    //            this.ReplaceNode(match, parentOfMatch, parent, node3);
    //            this.count--;
    //        }
    //        if (this.root != null)
    //        {
    //            this.root.IsRed = false;
    //        }
    //        return flag;
    //    }

    //    public void ExceptWith(IEnumerable<T> other)
    //    {
    //        if (other == null)
    //        {
    //            throw new ArgumentNullException("other");
    //        }
    //        if (this.count != 0)
    //        {
    //            if (other == this)
    //            {
    //                this.Clear();
    //            }
    //            else
    //            {
    //                SortedSetPatch<T> set = other as SortedSetPatch<T>;
    //                if ((set != null) && SortedSetPatch<T>.AreComparersEqual((SortedSetPatch<T>)this, set))
    //                {
    //                    if ((this.comparer.Compare(set.Max, this.Min) >= 0) && (this.comparer.Compare(set.Min, this.Max) <= 0))
    //                    {
    //                        T min = this.Min;
    //                        T max = this.Max;
    //                        foreach (T local3 in other)
    //                        {
    //                            if (this.comparer.Compare(local3, min) >= 0)
    //                            {
    //                                if (this.comparer.Compare(local3, max) > 0)
    //                                {
    //                                    break;
    //                                }
    //                                this.Remove(local3);
    //                            }
    //                        }
    //                    }
    //                }
    //                else
    //                {
    //                    this.RemoveAllElements(other);
    //                }
    //            }
    //        }
    //    }

    //    internal virtual Node FindNode(T item)
    //    {
    //        int num;
    //        for (Node node = this.root; node != null; node = (num < 0) ? node.Left : node.Right)
    //        {
    //            num = this.comparer.Compare(item, node.Item);
    //            if (num == 0)
    //            {
    //                return node;
    //            }
    //        }
    //        return null;
    //    }

    //    public bool FindPrev(T item, out T prevItem)
    //    {
    //        int num;
    //        Node prev = null;

    //        for (Node node = root; node != null; node = (num < 0) ? node.Left : node.Right)
    //        {
    //            num = comparer.Compare(item, node.Item);

    //            if (num == 0)
    //            {
    //                prevItem = node.Item;
    //                return true;
    //            }

    //            if (num > 0)
    //                prev = node;
    //        }

    //        if (prev != null)
    //        {
    //            prevItem = prev.Item;
    //            return true;
    //        }

    //        prevItem = default(T);
    //        return false;
    //    }

    //    public bool FindNext(T item, out T nextItem)
    //    {
    //        int num;
    //        Node next = null;

    //        for (Node node = root; node != null; node = (num < 0) ? node.Left : node.Right)
    //        {
    //            num = comparer.Compare(item, node.Item);

    //            if (num == 0)
    //            {
    //                nextItem = node.Item;
    //                return true;
    //            }

    //            if (num < 0)
    //                next = node;
    //        }

    //        if (next != null)
    //        {
    //            nextItem = next.Item;
    //            return true;
    //        }

    //        nextItem = default(T);
    //        return false;
    //    }

    //    public bool FindBefore(T item, out T beforeItem)
    //    {
    //        Node prev = null;

    //        int num;
    //        for (Node node = root; node != null; node = (num < 0) ? node.Left : node.Right)
    //        {
    //            num = comparer.Compare(item, node.Item);

    //            if (num == 0)
    //            {
    //                if (node.Left != null)
    //                {
    //                    Node tmp = node.Left;

    //                    while (tmp != null)
    //                    {
    //                        prev = tmp;
    //                        tmp = tmp.Right;
    //                    }
    //                }

    //                break;
    //            }

    //            if (num > 0)
    //                prev = node;
    //        }

    //        if (prev != null)
    //        {
    //            beforeItem = prev.Item;
    //            return true;
    //        }

    //        beforeItem = default(T);
    //        return false;
    //    }

    //    public bool FindAfter(T item, out T afterItem)
    //    {
    //        Node next = null;

    //        int num;
    //        for (Node node = root; node != null; node = (num < 0) ? node.Left : node.Right)
    //        {
    //            num = comparer.Compare(item, node.Item);

    //            if (num == 0)
    //            {
    //                if (node.Right != null)
    //                {
    //                    Node tmp = node.Right;

    //                    while (tmp != null)
    //                    {
    //                        next = tmp;
    //                        tmp = tmp.Left;
    //                    }
    //                }

    //                break;
    //            }

    //            if (num < 0)
    //                next = node;
    //        }

    //        if (next != null)
    //        {
    //            afterItem = next.Item;
    //            return true;
    //        }

    //        afterItem = default(T);
    //        return false;
    //    }

    //    public void ChangeFirstItem(T newItem)
    //    {
    //        Node node = this.root;
    //        while (node.Left != null) node = node.Left;
    //        if (comparer.Compare(node.Item, newItem) < 0)
    //            throw new ArgumentException("The new item must be lowest one");
    //        node.Item = newItem;
    //    }

    //    public void ChangeLastItem(T newItem)
    //    {
    //        Node node = this.root;
    //        while (node.Right != null) node = node.Right;
    //        if (comparer.Compare(node.Item, newItem) > 0)
    //            throw new ArgumentException("The new item must be highest one");
    //        node.Item = newItem;
    //    }

    //    internal Node FindRange(T from, T to)
    //    {
    //        return this.FindRange(from, to, true, true);
    //    }

    //    internal Node FindRange(T from, T to, bool lowerBoundActive, bool upperBoundActive)
    //    {
    //        Node root = this.root;
    //        while (root != null)
    //        {
    //            if (lowerBoundActive && (this.comparer.Compare(from, root.Item) > 0))
    //            {
    //                root = root.Right;
    //            }
    //            else
    //            {
    //                if (upperBoundActive && (this.comparer.Compare(to, root.Item) < 0))
    //                {
    //                    root = root.Left;
    //                    continue;
    //                }
    //                return root;
    //            }
    //        }
    //        return null;
    //    }

    //    public Enumerator GetEnumerator()
    //    {
    //        return new Enumerator((SortedSetPatch<T>)this);
    //    }

    //    protected virtual void GetObjectData(SerializationInfo info, StreamingContext context)
    //    {
    //        if (info == null)
    //        {
    //            ThrowHelper.ThrowArgumentNullException(ExceptionArgument.info);
    //        }
    //        info.AddValue("Count", this.count);
    //        info.AddValue("Comparer", this.comparer, typeof(IComparer<T>));
    //        info.AddValue("Version", this.version);
    //        if (this.root != null)
    //        {
    //            T[] array = new T[this.Count];
    //            this.CopyTo(array, 0);
    //            info.AddValue("Items", array, typeof(T[]));
    //        }
    //    }

    //    private static Node GetSibling(Node node, Node parent)
    //    {
    //        if (parent.Left == node)
    //        {
    //            return parent.Right;
    //        }
    //        return parent.Left;
    //    }

    //    public virtual SortedSetPatch<T> GetViewBetween(T lowerValue, T upperValue)
    //    {
    //        if (this.Comparer.Compare(lowerValue, upperValue) > 0)
    //        {
    //            throw new ArgumentException("lowerBound is greater than upperBound");
    //        }
    //        return new TreeSubSet((SortedSetPatch<T>)this, lowerValue, upperValue, true, true);
    //    }

    //    public SortedSetPatch<T> GetViewBetween(T lowerValue, T upperValue, bool lowerBoundActive, bool upperBoundActive)
    //    {
    //        if (lowerBoundActive && upperBoundActive)
    //        {
    //            if (comparer.Compare(lowerValue, upperValue) > 0)
    //                throw new ArgumentException("lowerValue > upperValue");
    //        }

    //        return new TreeSubSet((SortedSetPatch<T>)this, lowerValue, upperValue, lowerBoundActive, upperBoundActive);
    //    }

    //    internal bool InOrderTreeWalk(TreeWalkPredicate<T> action)
    //    {
    //        return this.InOrderTreeWalk(action, false);
    //    }

    //    internal virtual bool InOrderTreeWalk(TreeWalkPredicate<T> action, bool reverse)
    //    {
    //        if (this.root != null)
    //        {
    //            Stack<Node> stack = new Stack<Node>(2 * SortedSetPatch<T>.log2(this.Count + 1));
    //            Node root = this.root;
    //            while (root != null)
    //            {
    //                stack.Push(root);
    //                root = reverse ? root.Right : root.Left;
    //            }
    //            while (stack.Count != 0)
    //            {
    //                root = stack.Pop();
    //                if (!action(root))
    //                {
    //                    return false;
    //                }
    //                for (Node node2 = reverse ? root.Left : root.Right; node2 != null; node2 = reverse ? node2.Right : node2.Left)
    //                {
    //                    stack.Push(node2);
    //                }
    //            }
    //        }
    //        return true;
    //    }

    //    private void InsertionBalance(Node current, ref Node parent, Node grandParent, Node greatGrandParent)
    //    {
    //        Node node;
    //        bool flag = grandParent.Right == parent;
    //        bool flag2 = parent.Right == current;
    //        if (flag == flag2)
    //        {
    //            node = flag2 ? SortedSetPatch<T>.RotateLeft(grandParent) : SortedSetPatch<T>.RotateRight(grandParent);
    //        }
    //        else
    //        {
    //            node = flag2 ? SortedSetPatch<T>.RotateLeftRight(grandParent) : SortedSetPatch<T>.RotateRightLeft(grandParent);
    //            parent = greatGrandParent;
    //        }
    //        grandParent.IsRed = true;
    //        node.IsRed = false;
    //        this.ReplaceChildOfNodeOrRoot(greatGrandParent, grandParent, node);
    //    }

    //    internal virtual int InternalIndexOf(T item)
    //    {
    //        int num2;
    //        Node root = this.root;
    //        for (int i = 0; root != null; i = (num2 < 0) ? ((2 * i) + 1) : ((2 * i) + 2))
    //        {
    //            num2 = this.comparer.Compare(item, root.Item);
    //            if (num2 == 0)
    //            {
    //                return i;
    //            }
    //            root = (num2 < 0) ? root.Left : root.Right;
    //        }
    //        return -1;
    //    }

    //    public virtual void IntersectWith(IEnumerable<T> other)
    //    {
    //        if (other == null)
    //        {
    //            throw new ArgumentNullException("other");
    //        }
    //        if (this.Count != 0)
    //        {
    //            SortedSetPatch<T> set = other as SortedSetPatch<T>;
    //            TreeSubSet set2 = this as TreeSubSet;
    //            if (set2 != null)
    //            {
    //                this.VersionCheck();
    //            }
    //            if (((set == null) || (set2 != null)) || !SortedSetPatch<T>.AreComparersEqual((SortedSetPatch<T>)this, set))
    //            {
    //                this.IntersectWithEnumerable(other);
    //            }
    //            else
    //            {
    //                T[] arr = new T[this.Count];
    //                int num = 0;
    //                Enumerator enumerator = this.GetEnumerator();
    //                Enumerator enumerator2 = set.GetEnumerator();
    //                bool flag = !enumerator.MoveNext();
    //                bool flag2 = !enumerator2.MoveNext();
    //                T max = this.Max;
    //                T min = this.Min;
    //                while ((!flag && !flag2) && (this.Comparer.Compare(enumerator2.Current, max) <= 0))
    //                {
    //                    int num2 = this.Comparer.Compare(enumerator.Current, enumerator2.Current);
    //                    if (num2 < 0)
    //                    {
    //                        flag = !enumerator.MoveNext();
    //                    }
    //                    else
    //                    {
    //                        if (num2 == 0)
    //                        {
    //                            arr[num++] = enumerator2.Current;
    //                            flag = !enumerator.MoveNext();
    //                            flag2 = !enumerator2.MoveNext();
    //                            continue;
    //                        }
    //                        flag2 = !enumerator2.MoveNext();
    //                    }
    //                }
    //                this.root = null;
    //                this.root = SortedSetPatch<T>.ConstructRootFromSortedArray(arr, 0, num - 1, null);
    //                this.count = num;
    //                this.version++;
    //            }
    //        }
    //    }

    //    internal virtual void IntersectWithEnumerable(IEnumerable<T> other)
    //    {
    //        List<T> collection = new List<T>(this.Count);
    //        foreach (T local in other)
    //        {
    //            if (this.Contains(local))
    //            {
    //                collection.Add(local);
    //                this.Remove(local);
    //            }
    //        }
    //        this.Clear();
    //        this.AddAllElements(collection);
    //    }

    //    private static bool Is2Node(Node node)
    //    {
    //        return ((SortedSetPatch<T>.IsBlack(node) && SortedSetPatch<T>.IsNullOrBlack(node.Left)) && SortedSetPatch<T>.IsNullOrBlack(node.Right));
    //    }

    //    private static bool Is4Node(Node node)
    //    {
    //        return (SortedSetPatch<T>.IsRed(node.Left) && SortedSetPatch<T>.IsRed(node.Right));
    //    }

    //    private static bool IsBlack(Node node)
    //    {
    //        return ((node != null) && !node.IsRed);
    //    }

    //    private static bool IsNullOrBlack(Node node)
    //    {
    //        if (node != null)
    //        {
    //            return !node.IsRed;
    //        }
    //        return true;
    //    }

    //    public bool IsProperSubsetOf(IEnumerable<T> other)
    //    {
    //        if (other == null)
    //        {
    //            throw new ArgumentNullException("other");
    //        }
    //        if ((other is ICollection) && (this.Count == 0))
    //        {
    //            return ((other as ICollection).Count > 0);
    //        }
    //        SortedSetPatch<T> set = other as SortedSetPatch<T>;
    //        if ((set != null) && SortedSetPatch<T>.AreComparersEqual((SortedSetPatch<T>)this, set))
    //        {
    //            if (this.Count >= set.Count)
    //            {
    //                return false;
    //            }
    //            return this.IsSubsetOfSortedSetWithSameEC(set);
    //        }
    //        ElementCount count = this.CheckUniqueAndUnfoundElements(other, false);
    //        return ((count.uniqueCount == this.Count) && (count.unfoundCount > 0));
    //    }

    //    public bool IsProperSupersetOf(IEnumerable<T> other)
    //    {
    //        if (other == null)
    //        {
    //            throw new ArgumentNullException("other");
    //        }
    //        if (this.Count == 0)
    //        {
    //            return false;
    //        }
    //        if ((other is ICollection) && ((other as ICollection).Count == 0))
    //        {
    //            return true;
    //        }
    //        SortedSetPatch<T> set = other as SortedSetPatch<T>;
    //        if ((set != null) && SortedSetPatch<T>.AreComparersEqual(set, (SortedSetPatch<T>)this))
    //        {
    //            if (set.Count >= this.Count)
    //            {
    //                return false;
    //            }
    //            SortedSetPatch<T> viewBetween = this.GetViewBetween(set.Min, set.Max);
    //            foreach (T local in set)
    //            {
    //                if (!viewBetween.Contains(local))
    //                {
    //                    return false;
    //                }
    //            }
    //            return true;
    //        }
    //        ElementCount count = this.CheckUniqueAndUnfoundElements(other, true);
    //        return ((count.uniqueCount < this.Count) && (count.unfoundCount == 0));
    //    }

    //    private static bool IsRed(Node node)
    //    {
    //        return ((node != null) && node.IsRed);
    //    }

    //    public bool IsSubsetOf(IEnumerable<T> other)
    //    {
    //        if (other == null)
    //        {
    //            throw new ArgumentNullException("other");
    //        }
    //        if (this.Count == 0)
    //        {
    //            return true;
    //        }
    //        SortedSetPatch<T> set = other as SortedSetPatch<T>;
    //        if ((set != null) && SortedSetPatch<T>.AreComparersEqual((SortedSetPatch<T>)this, set))
    //        {
    //            if (this.Count > set.Count)
    //            {
    //                return false;
    //            }
    //            return this.IsSubsetOfSortedSetWithSameEC(set);
    //        }
    //        ElementCount count = this.CheckUniqueAndUnfoundElements(other, false);
    //        return ((count.uniqueCount == this.Count) && (count.unfoundCount >= 0));
    //    }

    //    private bool IsSubsetOfSortedSetWithSameEC(SortedSetPatch<T> asSorted)
    //    {
    //        SortedSetPatch<T> viewBetween = asSorted.GetViewBetween(this.Min, this.Max);
    //        foreach (T local in this)
    //        {
    //            if (!viewBetween.Contains(local))
    //            {
    //                return false;
    //            }
    //        }
    //        return true;
    //    }

    //    public bool IsSupersetOf(IEnumerable<T> other)
    //    {
    //        if (other == null)
    //        {
    //            throw new ArgumentNullException("other");
    //        }
    //        if (!(other is ICollection) || ((other as ICollection).Count != 0))
    //        {
    //            SortedSetPatch<T> set = other as SortedSetPatch<T>;
    //            if ((set == null) || !SortedSetPatch<T>.AreComparersEqual((SortedSetPatch<T>)this, set))
    //            {
    //                return this.ContainsAllElements(other);
    //            }
    //            if (this.Count < set.Count)
    //            {
    //                return false;
    //            }
    //            SortedSetPatch<T> viewBetween = this.GetViewBetween(set.Min, set.Max);
    //            foreach (T local in set)
    //            {
    //                if (!viewBetween.Contains(local))
    //                {
    //                    return false;
    //                }
    //            }
    //        }
    //        return true;
    //    }

    //    internal virtual bool IsWithinRange(T item)
    //    {
    //        return true;
    //    }

    //    private static int log2(int value)
    //    {
    //        int num = 0;
    //        while (value > 0)
    //        {
    //            num++;
    //            value = value >> 1;
    //        }
    //        return num;
    //    }

    //    private static void Merge2Nodes(Node parent, Node child1, Node child2)
    //    {
    //        parent.IsRed = false;
    //        child1.IsRed = true;
    //        child2.IsRed = true;
    //    }

    //    protected virtual void OnDeserialization(object sender)
    //    {
    //        if (this.comparer == null)
    //        {
    //            if (this.siInfo == null)
    //            {
    //                ThrowHelper.ThrowSerializationException(ExceptionResource.Serialization_InvalidOnDeser);
    //            }
    //            this.comparer = (IComparer<T>)this.siInfo.GetValue("Comparer", typeof(IComparer<T>));
    //            int num = this.siInfo.GetInt32("Count");
    //            if (num != 0)
    //            {
    //                T[] localArray = (T[])this.siInfo.GetValue("Items", typeof(T[]));
    //                if (localArray == null)
    //                {
    //                    ThrowHelper.ThrowSerializationException(ExceptionResource.Serialization_MissingValues);
    //                }
    //                for (int i = 0; i < localArray.Length; i++)
    //                {
    //                    this.Add(localArray[i]);
    //                }
    //            }
    //            this.version = this.siInfo.GetInt32("Version");
    //            if (this.count != num)
    //            {
    //                ThrowHelper.ThrowSerializationException(ExceptionResource.Serialization_MismatchedCount);
    //            }
    //            this.siInfo = null;
    //        }
    //    }

    //    public bool Overlaps(IEnumerable<T> other)
    //    {
    //        if (other == null)
    //        {
    //            throw new ArgumentNullException("other");
    //        }
    //        if (this.Count != 0)
    //        {
    //            if ((other is ICollection<T>) && ((other as ICollection<T>).Count == 0))
    //            {
    //                return false;
    //            }
    //            SortedSetPatch<T> set = other as SortedSetPatch<T>;
    //            if (((set != null) && SortedSetPatch<T>.AreComparersEqual((SortedSetPatch<T>)this, set)) && ((this.comparer.Compare(this.Min, set.Max) > 0) || (this.comparer.Compare(this.Max, set.Min) < 0)))
    //            {
    //                return false;
    //            }
    //            foreach (T local in other)
    //            {
    //                if (this.Contains(local))
    //                {
    //                    return true;
    //                }
    //            }
    //        }
    //        return false;
    //    }

    //    public bool Remove(T item)
    //    {
    //        return this.DoRemove(item);
    //    }

    //    public bool Remove(T fromKey, T toKey)
    //    {
    //        int cmp = Comparer.Compare(fromKey, toKey);
    //        if (cmp > 0)
    //            throw new ArgumentException();

    //        if (cmp == 0)
    //            return Remove(fromKey);

    //        T[] arr = new T[Count];
    //        CopyTo(arr);

    //        int from = Array.BinarySearch(arr, fromKey, Comparer);
    //        int fromIdx = from;
    //        if (fromIdx < 0)
    //        {
    //            fromIdx = ~fromIdx;
    //            if (fromIdx == Count)
    //                return false;
    //        }

    //        int to = Array.BinarySearch(arr, fromIdx, Count - fromIdx, toKey, Comparer);
    //        int toIdx = to;
    //        if (toIdx < 0)
    //        {
    //            if (from == to)
    //                return false;

    //            toIdx = ~toIdx - 1;
    //        }

    //        int count = toIdx - fromIdx + 1;
    //        if (count == 0)
    //            return false;

    //        Array.Copy(arr, toIdx + 1, arr, fromIdx, Count - (toIdx + 1));
    //        ConstructFromSortedArray(arr, 0, Count - count);

    //        return true;
    //    }

    //    private void RemoveAllElements(IEnumerable<T> collection)
    //    {
    //        T min = this.Min;
    //        T max = this.Max;
    //        foreach (T local3 in collection)
    //        {
    //            if (((this.comparer.Compare(local3, min) >= 0) && (this.comparer.Compare(local3, max) <= 0)) && this.Contains(local3))
    //            {
    //                this.Remove(local3);
    //            }
    //        }
    //    }

    //    public int RemoveWhere(Predicate<T> match)
    //    {
    //        if (match == null)
    //        {
    //            throw new ArgumentNullException("match");
    //        }
    //        List<T> matches = new List<T>(this.Count);
    //        this.BreadthFirstTreeWalk(delegate(Node n)
    //        {
    //            if (match(n.Item))
    //            {
    //                matches.Add(n.Item);
    //            }
    //            return true;
    //        });
    //        int num = 0;
    //        for (int i = matches.Count - 1; i >= 0; i--)
    //        {
    //            if (this.Remove(matches[i]))
    //            {
    //                num++;
    //            }
    //        }
    //        return num;
    //    }

    //    private void ReplaceChildOfNodeOrRoot(Node parent, Node child, Node newChild)
    //    {
    //        if (parent != null)
    //        {
    //            if (parent.Left == child)
    //            {
    //                parent.Left = newChild;
    //            }
    //            else
    //            {
    //                parent.Right = newChild;
    //            }
    //        }
    //        else
    //        {
    //            this.root = newChild;
    //        }
    //    }

    //    private void ReplaceNode(Node match, Node parentOfMatch, Node succesor, Node parentOfSuccesor)
    //    {
    //        if (succesor == match)
    //        {
    //            succesor = match.Left;
    //        }
    //        else
    //        {
    //            if (succesor.Right != null)
    //            {
    //                succesor.Right.IsRed = false;
    //            }
    //            if (parentOfSuccesor != match)
    //            {
    //                parentOfSuccesor.Left = succesor.Right;
    //                succesor.Right = match.Right;
    //            }
    //            succesor.Left = match.Left;
    //        }
    //        if (succesor != null)
    //        {
    //            succesor.IsRed = match.IsRed;
    //        }
    //        this.ReplaceChildOfNodeOrRoot(parentOfMatch, match, succesor);
    //    }

    //    public IEnumerable<T> Reverse()
    //    {
    //        Enumerator iteratorVariable0 = new Enumerator((SortedSetPatch<T>)this, true);
    //        while (true)
    //        {
    //            if (!iteratorVariable0.MoveNext())
    //            {
    //                yield break;
    //            }
    //            yield return iteratorVariable0.Current;
    //        }
    //    }

    //    private static Node RotateLeft(Node node)
    //    {
    //        Node right = node.Right;
    //        node.Right = right.Left;
    //        right.Left = node;
    //        return right;
    //    }

    //    private static Node RotateLeftRight(Node node)
    //    {
    //        Node left = node.Left;
    //        Node right = left.Right;
    //        node.Left = right.Right;
    //        right.Right = node;
    //        left.Right = right.Left;
    //        right.Left = left;
    //        return right;
    //    }

    //    private static Node RotateRight(Node node)
    //    {
    //        Node left = node.Left;
    //        node.Left = left.Right;
    //        left.Right = node;
    //        return left;
    //    }

    //    private static Node RotateRightLeft(Node node)
    //    {
    //        Node right = node.Right;
    //        Node left = right.Left;
    //        node.Right = left.Left;
    //        left.Left = node;
    //        right.Left = left.Right;
    //        left.Right = right;
    //        return left;
    //    }

    //    private static TreeRotation RotationNeeded(Node parent, Node current, Node sibling)
    //    {
    //        if (SortedSetPatch<T>.IsRed(sibling.Left))
    //        {
    //            if (parent.Left == current)
    //            {
    //                return TreeRotation.RightLeftRotation;
    //            }
    //            return TreeRotation.RightRotation;
    //        }
    //        if (parent.Left == current)
    //        {
    //            return TreeRotation.LeftRotation;
    //        }
    //        return TreeRotation.LeftRightRotation;
    //    }

    //    public bool SetEquals(IEnumerable<T> other)
    //    {
    //        if (other == null)
    //        {
    //            throw new ArgumentNullException("other");
    //        }
    //        SortedSetPatch<T> set = other as SortedSetPatch<T>;
    //        if ((set != null) && SortedSetPatch<T>.AreComparersEqual((SortedSetPatch<T>)this, set))
    //        {
    //            IEnumerator<T> enumerator = this.GetEnumerator();
    //            IEnumerator<T> enumerator2 = set.GetEnumerator();
    //            bool flag = !enumerator.MoveNext();
    //            bool flag2 = !enumerator2.MoveNext();
    //            while (!flag && !flag2)
    //            {
    //                if (this.Comparer.Compare(enumerator.Current, enumerator2.Current) != 0)
    //                {
    //                    return false;
    //                }
    //                flag = !enumerator.MoveNext();
    //                flag2 = !enumerator2.MoveNext();
    //            }
    //            return (flag && flag2);
    //        }
    //        ElementCount count = this.CheckUniqueAndUnfoundElements(other, true);
    //        return ((count.uniqueCount == this.Count) && (count.unfoundCount == 0));
    //    }

    //    internal static bool SortedSetEquals(SortedSetPatch<T> set1, SortedSetPatch<T> set2, IComparer<T> comparer)
    //    {
    //        if (set1 == null)
    //        {
    //            return (set2 == null);
    //        }
    //        if (set2 == null)
    //        {
    //            return false;
    //        }
    //        if (SortedSetPatch<T>.AreComparersEqual(set1, set2))
    //        {
    //            if (set1.Count != set2.Count)
    //            {
    //                return false;
    //            }
    //            return set1.SetEquals(set2);
    //        }
    //        bool flag = false;
    //        foreach (T local in set1)
    //        {
    //            flag = false;
    //            foreach (T local2 in set2)
    //            {
    //                if (comparer.Compare(local, local2) == 0)
    //                {
    //                    flag = true;
    //                    break;
    //                }
    //            }
    //            if (!flag)
    //            {
    //                return false;
    //            }
    //        }
    //        return true;
    //    }

    //    private static void Split4Node(Node node)
    //    {
    //        node.IsRed = true;
    //        node.Left.IsRed = false;
    //        node.Right.IsRed = false;
    //    }

    //    public SortedSetPatch<T> Split(int count)
    //    {
    //        T[] array = new T[Count];
    //        this.CopyTo(array);

    //        SortedSetPatch<T> right = new SortedSetPatch<T>(this.Comparer);

    //        ConstructFromSortedArray(this, array, 0, array.Length - count);
    //        ConstructFromSortedArray(right, array, array.Length - count, count);

    //        return right;
    //    }

    //    public void SymmetricExceptWith(IEnumerable<T> other)
    //    {
    //        if (other == null)
    //        {
    //            throw new ArgumentNullException("other");
    //        }
    //        if (this.Count == 0)
    //        {
    //            this.UnionWith(other);
    //        }
    //        else if (other == this)
    //        {
    //            this.Clear();
    //        }
    //        else
    //        {
    //            SortedSetPatch<T> set = other as SortedSetPatch<T>;
    //            if ((set != null) && SortedSetPatch<T>.AreComparersEqual((SortedSetPatch<T>)this, set))
    //            {
    //                this.SymmetricExceptWithSameEC(set);
    //            }
    //            else
    //            {
    //                T[] array = new List<T>(other).ToArray();
    //                Array.Sort<T>(array, this.Comparer);
    //                this.SymmetricExceptWithSameEC(array);
    //            }
    //        }
    //    }

    //    internal void SymmetricExceptWithSameEC(ISet<T> other)
    //    {
    //        foreach (T local in other)
    //        {
    //            if (this.Contains(local))
    //            {
    //                this.Remove(local);
    //            }
    //            else
    //            {
    //                this.Add(local);
    //            }
    //        }
    //    }

    //    internal void SymmetricExceptWithSameEC(T[] other)
    //    {
    //        if (other.Length != 0)
    //        {
    //            T y = other[0];
    //            for (int i = 0; i < other.Length; i++)
    //            {
    //                while (((i < other.Length) && (i != 0)) && (this.comparer.Compare(other[i], y) == 0))
    //                {
    //                    i++;
    //                }
    //                if (i >= other.Length)
    //                {
    //                    return;
    //                }
    //                if (this.Contains(other[i]))
    //                {
    //                    this.Remove(other[i]);
    //                }
    //                else
    //                {
    //                    this.Add(other[i]);
    //                }
    //                y = other[i];
    //            }
    //        }
    //    }

    //    void ICollection<T>.Add(T item)
    //    {
    //        this.AddIfNotPresent(item);
    //    }

    //    IEnumerator<T> IEnumerable<T>.GetEnumerator()
    //    {
    //        return new Enumerator((SortedSetPatch<T>)this);
    //    }

    //    void ICollection.CopyTo(Array array, int index)
    //    {
    //        if (array == null)
    //        {
    //            ThrowHelper.ThrowArgumentNullException(ExceptionArgument.array);
    //        }
    //        if (array.Rank != 1)
    //        {
    //            ThrowHelper.ThrowArgumentException(ExceptionResource.Arg_RankMultiDimNotSupported);
    //        }
    //        if (array.GetLowerBound(0) != 0)
    //        {
    //            ThrowHelper.ThrowArgumentException(ExceptionResource.Arg_NonZeroLowerBound);
    //        }
    //        if (index < 0)
    //        {
    //            ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.arrayIndex, ExceptionResource.ArgumentOutOfRange_NeedNonNegNum);
    //        }
    //        if ((array.Length - index) < this.Count)
    //        {
    //            ThrowHelper.ThrowArgumentException(ExceptionResource.Arg_ArrayPlusOffTooSmall);
    //        }
    //        T[] localArray = array as T[];
    //        if (localArray != null)
    //        {
    //            this.CopyTo(localArray, index);
    //        }
    //        else
    //        {
    //            TreeWalkPredicate<T> action = null;
    //            object[] objects = array as object[];
    //            if (objects == null)
    //            {
    //                ThrowHelper.ThrowArgumentException(ExceptionResource.Argument_InvalidArrayType);
    //            }
    //            try
    //            {
    //                if (action == null)
    //                {
    //                    action = delegate(Node node)
    //                    {
    //                        objects[index++] = node.Item;
    //                        return true;
    //                    };
    //                }
    //                this.InOrderTreeWalk(action);
    //            }
    //            catch (ArrayTypeMismatchException)
    //            {
    //                ThrowHelper.ThrowArgumentException(ExceptionResource.Argument_InvalidArrayType);
    //            }
    //        }
    //    }

    //    IEnumerator IEnumerable.GetEnumerator()
    //    {
    //        return new Enumerator((SortedSetPatch<T>)this);
    //    }

    //    void IDeserializationCallback.OnDeserialization(object sender)
    //    {
    //        this.OnDeserialization(sender);
    //    }

    //    void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
    //    {
    //        this.GetObjectData(info, context);
    //    }

    //    internal T[] ToArray()
    //    {
    //        T[] array = new T[this.Count];
    //        this.CopyTo(array);
    //        return array;
    //    }

    //    public void UnionWith(IEnumerable<T> other)
    //    {
    //        if (other == null)
    //        {
    //            throw new ArgumentNullException("other");
    //        }
    //        SortedSetPatch<T> collection = other as SortedSetPatch<T>;
    //        TreeSubSet set2 = this as TreeSubSet;
    //        if (set2 != null)
    //        {
    //            this.VersionCheck();
    //        }
    //        if (((collection != null) && (set2 == null)) && (this.count == 0))
    //        {
    //            SortedSetPatch<T> set3 = new SortedSetPatch<T>(collection, this.comparer);
    //            this.root = set3.root;
    //            this.count = set3.count;
    //            this.version++;
    //        }
    //        else if (((collection == null) || (set2 != null)) || (!SortedSetPatch<T>.AreComparersEqual((SortedSetPatch<T>)this, collection) || (collection.Count <= (this.Count / 2))))
    //        {
    //            this.AddAllElements(other);
    //        }
    //        else
    //        {
    //            T[] arr = new T[collection.Count + this.Count];
    //            int num = 0;
    //            Enumerator enumerator = this.GetEnumerator();
    //            Enumerator enumerator2 = collection.GetEnumerator();
    //            bool flag = !enumerator.MoveNext();
    //            bool flag2 = !enumerator2.MoveNext();
    //            while (!flag && !flag2)
    //            {
    //                int num2 = this.Comparer.Compare(enumerator.Current, enumerator2.Current);
    //                if (num2 < 0)
    //                {
    //                    arr[num++] = enumerator.Current;
    //                    flag = !enumerator.MoveNext();
    //                }
    //                else
    //                {
    //                    if (num2 == 0)
    //                    {
    //                        arr[num++] = enumerator2.Current;
    //                        flag = !enumerator.MoveNext();
    //                        flag2 = !enumerator2.MoveNext();
    //                        continue;
    //                    }
    //                    arr[num++] = enumerator2.Current;
    //                    flag2 = !enumerator2.MoveNext();
    //                }
    //            }
    //            if (!flag || !flag2)
    //            {
    //                Enumerator enumerator3 = flag ? enumerator2 : enumerator;
    //                do
    //                {
    //                    arr[num++] = enumerator3.Current;
    //                }
    //                while (enumerator3.MoveNext());
    //            }
    //            this.root = null;
    //            this.root = SortedSetPatch<T>.ConstructRootFromSortedArray(arr, 0, num - 1, null);
    //            this.count = num;
    //            this.version++;
    //        }
    //    }

    //    internal void UpdateVersion()
    //    {
    //        this.version++;
    //    }

    //    internal virtual void VersionCheck()
    //    {
    //    }

    //    // Properties
    //    public IComparer<T> Comparer
    //    {
    //        get
    //        {
    //            return this.comparer;
    //        }
    //    }

    //    public int Count
    //    {
    //        get
    //        {
    //            this.VersionCheck();
    //            return this.count;
    //        }
    //    }

    //    public T Max
    //    {
    //        get
    //        {
    //            T ret = default(T);
    //            this.InOrderTreeWalk(delegate(Node n)
    //            {
    //                ret = n.Item;
    //                return false;
    //            }, true);
    //            return ret;
    //        }
    //    }

    //    public T Min
    //    {
    //        get
    //        {
    //            T ret = default(T);
    //            this.InOrderTreeWalk(delegate(Node n)
    //            {
    //                ret = n.Item;
    //                return false;
    //            });
    //            return ret;
    //        }
    //    }

    //    bool ICollection<T>.IsReadOnly
    //    {
    //        get
    //        {
    //            return false;
    //        }
    //    }

    //    bool ICollection.IsSynchronized
    //    {
    //        get
    //        {
    //            return false;
    //        }
    //    }

    //    object ICollection.SyncRoot
    //    {
    //        get
    //        {
    //            if (this._syncRoot == null)
    //            {
    //                Interlocked.CompareExchange(ref this._syncRoot, new object(), null);
    //            }
    //            return this._syncRoot;
    //        }
    //    }

    //    //[CompilerGenerated]
    //    //private sealed class <Reverse>d__12 : IEnumerable<T>, IEnumerable, IEnumerator<T>, IEnumerator, IDisposable

    //    [StructLayout(LayoutKind.Sequential)]
    //    internal struct ElementCount
    //    {
    //        internal int uniqueCount;
    //        internal int unfoundCount;
    //    }

    //    [Serializable, StructLayout(LayoutKind.Sequential)]
    //    public struct Enumerator : IEnumerator<T>, IDisposable, IEnumerator, ISerializable, IDeserializationCallback
    //    {
    //        private SortedSetPatch<T> tree;
    //        private int version;
    //        private Stack<SortedSetPatch<T>.Node> stack;
    //        private SortedSetPatch<T>.Node current;
    //        private static SortedSetPatch<T>.Node dummyNode;
    //        private bool reverse;
    //        private SerializationInfo siInfo;
    //        internal Enumerator(SortedSetPatch<T> set)
    //        {
    //            this.tree = set;
    //            this.tree.VersionCheck();
    //            this.version = this.tree.version;
    //            this.stack = new Stack<SortedSetPatch<T>.Node>(2 * SortedSetPatch<T>.log2(set.Count + 1));
    //            this.current = null;
    //            this.reverse = false;
    //            this.siInfo = null;
    //            this.Intialize();
    //        }

    //        internal Enumerator(SortedSetPatch<T> set, bool reverse)
    //        {
    //            this.tree = set;
    //            this.tree.VersionCheck();
    //            this.version = this.tree.version;
    //            this.stack = new Stack<SortedSetPatch<T>.Node>(2 * SortedSetPatch<T>.log2(set.Count + 1));
    //            this.current = null;
    //            this.reverse = reverse;
    //            this.siInfo = null;
    //            this.Intialize();
    //        }

    //        private Enumerator(SerializationInfo info, StreamingContext context)
    //        {
    //            this.tree = null;
    //            this.version = -1;
    //            this.current = null;
    //            this.reverse = false;
    //            this.stack = null;
    //            this.siInfo = info;
    //        }

    //        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
    //        {
    //            this.GetObjectData(info, context);
    //        }

    //        private void GetObjectData(SerializationInfo info, StreamingContext context)
    //        {
    //            if (info == null)
    //            {
    //                ThrowHelper.ThrowArgumentNullException(ExceptionArgument.info);
    //            }
    //            info.AddValue("Tree", this.tree, typeof(SortedSetPatch<T>));
    //            info.AddValue("EnumVersion", this.version);
    //            info.AddValue("Reverse", this.reverse);
    //            info.AddValue("EnumStarted", !this.NotStartedOrEnded);
    //            info.AddValue("Item", (this.current == null) ? SortedSetPatch<T>.Enumerator.dummyNode.Item : this.current.Item, typeof(T));
    //        }

    //        void IDeserializationCallback.OnDeserialization(object sender)
    //        {
    //            this.OnDeserialization(sender);
    //        }

    //        private void OnDeserialization(object sender)
    //        {
    //            if (this.siInfo == null)
    //            {
    //                ThrowHelper.ThrowSerializationException(ExceptionResource.Serialization_InvalidOnDeser);
    //            }
    //            this.tree = (SortedSetPatch<T>)this.siInfo.GetValue("Tree", typeof(SortedSetPatch<T>));
    //            this.version = this.siInfo.GetInt32("EnumVersion");
    //            this.reverse = this.siInfo.GetBoolean("Reverse");
    //            bool boolean = this.siInfo.GetBoolean("EnumStarted");
    //            this.stack = new Stack<SortedSetPatch<T>.Node>(2 * SortedSetPatch<T>.log2(this.tree.Count + 1));
    //            this.current = null;
    //            if (boolean)
    //            {
    //                T y = (T)this.siInfo.GetValue("Item", typeof(T));
    //                this.Intialize();
    //                while (this.MoveNext())
    //                {
    //                    if (this.tree.Comparer.Compare(this.Current, y) == 0)
    //                    {
    //                        return;
    //                    }
    //                }
    //            }
    //        }

    //        private void Intialize()
    //        {
    //            this.current = null;
    //            SortedSetPatch<T>.Node root = this.tree.root;
    //            SortedSetPatch<T>.Node node2 = null;
    //            SortedSetPatch<T>.Node node3 = null;
    //            while (root != null)
    //            {
    //                node2 = this.reverse ? root.Right : root.Left;
    //                node3 = this.reverse ? root.Left : root.Right;
    //                if (this.tree.IsWithinRange(root.Item))
    //                {
    //                    this.stack.Push(root);
    //                    root = node2;
    //                }
    //                else
    //                {
    //                    if ((node2 == null) || !this.tree.IsWithinRange(node2.Item))
    //                    {
    //                        root = node3;
    //                        continue;
    //                    }
    //                    root = node2;
    //                }
    //            }
    //        }

    //        public bool MoveNext()
    //        {
    //            this.tree.VersionCheck();
    //            if (this.version != this.tree.version)
    //            {
    //                ThrowHelper.ThrowInvalidOperationException(ExceptionResource.InvalidOperation_EnumFailedVersion);
    //            }
    //            if (this.stack.Count == 0)
    //            {
    //                this.current = null;
    //                return false;
    //            }
    //            this.current = this.stack.Pop();
    //            SortedSetPatch<T>.Node item = this.reverse ? this.current.Left : this.current.Right;
    //            SortedSetPatch<T>.Node node2 = null;
    //            SortedSetPatch<T>.Node node3 = null;
    //            while (item != null)
    //            {
    //                node2 = this.reverse ? item.Right : item.Left;
    //                node3 = this.reverse ? item.Left : item.Right;
    //                if (this.tree.IsWithinRange(item.Item))
    //                {
    //                    this.stack.Push(item);
    //                    item = node2;
    //                }
    //                else
    //                {
    //                    if ((node3 == null) || !this.tree.IsWithinRange(node3.Item))
    //                    {
    //                        item = node2;
    //                        continue;
    //                    }
    //                    item = node3;
    //                }
    //            }
    //            return true;
    //        }

    //        public void Dispose()
    //        {
    //        }

    //        public T Current
    //        {
    //            get
    //            {
    //                if (this.current != null)
    //                {
    //                    return this.current.Item;
    //                }
    //                return default(T);
    //            }
    //        }
    //        object IEnumerator.Current
    //        {
    //            get
    //            {
    //                if (this.current == null)
    //                {
    //                    ThrowHelper.ThrowInvalidOperationException(ExceptionResource.InvalidOperation_EnumOpCantHappen);
    //                }
    //                return this.current.Item;
    //            }
    //        }
    //        internal bool NotStartedOrEnded
    //        {
    //            get
    //            {
    //                return (this.current == null);
    //            }
    //        }
    //        internal void Reset()
    //        {
    //            if (this.version != this.tree.version)
    //            {
    //                ThrowHelper.ThrowInvalidOperationException(ExceptionResource.InvalidOperation_EnumFailedVersion);
    //            }
    //            this.stack.Clear();
    //            this.Intialize();
    //        }

    //        void IEnumerator.Reset()
    //        {
    //            this.Reset();
    //        }

    //        static Enumerator()
    //        {
    //            SortedSetPatch<T>.Enumerator.dummyNode = new SortedSetPatch<T>.Node(default(T));
    //        }
    //    }

    //    public class Node
    //    {
    //        // Fields
    //        public bool IsRed;
    //        public T Item;
    //        public SortedSetPatch<T>.Node Left;
    //        public SortedSetPatch<T>.Node Right;

    //        // Methods
    //        public Node(T item)
    //        {
    //            this.Item = item;
    //            this.IsRed = true;
    //        }

    //        public Node(T item, bool isRed)
    //        {
    //            this.Item = item;
    //            this.IsRed = isRed;
    //        }
    //    }

    //    [Serializable]
    //    internal sealed class TreeSubSet : SortedSetPatch<T>, ISerializable, IDeserializationCallback
    //    {
    //        // Fields
    //        private bool lBoundActive;
    //        private T max;
    //        private T min;
    //        private bool uBoundActive;
    //        private SortedSetPatch<T> underlying;

    //        // Methods
    //        private TreeSubSet()
    //        {
    //            base.comparer = null;
    //        }

    //        private TreeSubSet(SerializationInfo info, StreamingContext context)
    //        {
    //            base.siInfo = info;
    //            this.OnDeserializationImpl(info);
    //        }

    //        public TreeSubSet(SortedSetPatch<T> Underlying, T Min, T Max, bool lowerBoundActive, bool upperBoundActive)
    //            : base(Underlying.Comparer)
    //        {
    //            this.underlying = Underlying;
    //            this.min = Min;
    //            this.max = Max;
    //            this.lBoundActive = lowerBoundActive;
    //            this.uBoundActive = upperBoundActive;
    //            base.root = this.underlying.FindRange(this.min, this.max, this.lBoundActive, this.uBoundActive);
    //            base.count = 0;
    //            base.version = -1;
    //            this.VersionCheckImpl();
    //        }

    //        internal override bool AddIfNotPresent(T item)
    //        {
    //            if (!this.IsWithinRange(item))
    //            {
    //                ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.collection);
    //            }
    //            bool flag = this.underlying.AddIfNotPresent(item);
    //            this.VersionCheck();
    //            return flag;
    //        }

    //        internal override bool BreadthFirstTreeWalk(TreeWalkPredicate<T> action)
    //        {
    //            this.VersionCheck();
    //            if (base.root != null)
    //            {
    //                List<SortedSetPatch<T>.Node> list = new List<SortedSetPatch<T>.Node> {
    //                    base.root
    //                };
    //                while (list.Count != 0)
    //                {
    //                    SortedSetPatch<T>.Node node = list[0];
    //                    list.RemoveAt(0);
    //                    if (this.IsWithinRange(node.Item) && !action(node))
    //                    {
    //                        return false;
    //                    }
    //                    if ((node.Left != null) && (!this.lBoundActive || (base.Comparer.Compare(this.min, node.Item) < 0)))
    //                    {
    //                        list.Add(node.Left);
    //                    }
    //                    if ((node.Right != null) && (!this.uBoundActive || (base.Comparer.Compare(this.max, node.Item) > 0)))
    //                    {
    //                        list.Add(node.Right);
    //                    }
    //                }
    //            }
    //            return true;
    //        }

    //        public override void Clear()
    //        {
    //            List<T> toRemove;
    //            if (base.count != 0)
    //            {
    //                toRemove = new List<T>();
    //                this.BreadthFirstTreeWalk(delegate(SortedSetPatch<T>.Node n)
    //                {
    //                    toRemove.Add(n.Item);
    //                    return true;
    //                });
    //                while (toRemove.Count != 0)
    //                {
    //                    this.underlying.Remove(toRemove[toRemove.Count - 1]);
    //                    toRemove.RemoveAt(toRemove.Count - 1);
    //                }
    //                base.root = null;
    //                base.count = 0;
    //                base.version = this.underlying.version;
    //            }
    //        }

    //        public override bool Contains(T item)
    //        {
    //            this.VersionCheck();
    //            return base.Contains(item);
    //        }

    //        internal override bool DoRemove(T item)
    //        {
    //            if (!this.IsWithinRange(item))
    //            {
    //                return false;
    //            }
    //            bool flag = this.underlying.Remove(item);
    //            this.VersionCheck();
    //            return flag;
    //        }

    //        internal override SortedSetPatch<T>.Node FindNode(T item)
    //        {
    //            if (!this.IsWithinRange(item))
    //            {
    //                return null;
    //            }
    //            this.VersionCheck();
    //            return base.FindNode(item);
    //        }

    //        protected override void GetObjectData(SerializationInfo info, StreamingContext context)
    //        {
    //            if (info == null)
    //            {
    //                ThrowHelper.ThrowArgumentNullException(ExceptionArgument.info);
    //            }
    //            info.AddValue(SortedSetPatch<T>.maxName, this.max, typeof(T));
    //            info.AddValue(SortedSetPatch<T>.minName, this.min, typeof(T));
    //            info.AddValue(SortedSetPatch<T>.lBoundActiveName, this.lBoundActive);
    //            info.AddValue(SortedSetPatch<T>.uBoundActiveName, this.uBoundActive);
    //            base.GetObjectData(info, context);
    //        }

    //        public override SortedSetPatch<T> GetViewBetween(T lowerValue, T upperValue)
    //        {
    //            if (this.lBoundActive && (base.Comparer.Compare(this.min, lowerValue) > 0))
    //            {
    //                throw new ArgumentOutOfRangeException("lowerValue");
    //            }
    //            if (this.uBoundActive && (base.Comparer.Compare(this.max, upperValue) < 0))
    //            {
    //                throw new ArgumentOutOfRangeException("upperValue");
    //            }
    //            return (SortedSetPatch<T>.TreeSubSet)this.underlying.GetViewBetween(lowerValue, upperValue);
    //        }

    //        internal override bool InOrderTreeWalk(TreeWalkPredicate<T> action, bool reverse)
    //        {
    //            this.VersionCheck();
    //            if (base.root != null)
    //            {
    //                Stack<SortedSetPatch<T>.Node> stack = new Stack<SortedSetPatch<T>.Node>(2 * SortedSetPatch<T>.log2(base.count + 1));
    //                SortedSetPatch<T>.Node root = base.root;
    //                while (root != null)
    //                {
    //                    if (this.IsWithinRange(root.Item))
    //                    {
    //                        stack.Push(root);
    //                        root = reverse ? root.Right : root.Left;
    //                    }
    //                    else
    //                    {
    //                        if (this.lBoundActive && (base.Comparer.Compare(this.min, root.Item) > 0))
    //                        {
    //                            root = root.Right;
    //                            continue;
    //                        }
    //                        root = root.Left;
    //                    }
    //                }
    //                while (stack.Count != 0)
    //                {
    //                    root = stack.Pop();
    //                    if (!action(root))
    //                    {
    //                        return false;
    //                    }
    //                    SortedSetPatch<T>.Node item = reverse ? root.Left : root.Right;
    //                    while (item != null)
    //                    {
    //                        if (this.IsWithinRange(item.Item))
    //                        {
    //                            stack.Push(item);
    //                            item = reverse ? item.Right : item.Left;
    //                        }
    //                        else
    //                        {
    //                            if (this.lBoundActive && (base.Comparer.Compare(this.min, item.Item) > 0))
    //                            {
    //                                item = item.Right;
    //                                continue;
    //                            }
    //                            item = item.Left;
    //                        }
    //                    }
    //                }
    //            }
    //            return true;
    //        }

    //        internal override int InternalIndexOf(T item)
    //        {
    //            int num = -1;
    //            foreach (T local in this)
    //            {
    //                num++;
    //                if (base.Comparer.Compare(item, local) == 0)
    //                {
    //                    return num;
    //                }
    //            }
    //            return -1;
    //        }

    //        internal override void IntersectWithEnumerable(IEnumerable<T> other)
    //        {
    //            List<T> collection = new List<T>(base.Count);
    //            foreach (T local in other)
    //            {
    //                if (this.Contains(local))
    //                {
    //                    collection.Add(local);
    //                    base.Remove(local);
    //                }
    //            }
    //            this.Clear();
    //            base.AddAllElements(collection);
    //        }

    //        internal override bool IsWithinRange(T item)
    //        {
    //            int num = this.lBoundActive ? base.Comparer.Compare(this.min, item) : -1;
    //            if (num > 0)
    //            {
    //                return false;
    //            }
    //            num = this.uBoundActive ? base.Comparer.Compare(this.max, item) : 1;
    //            if (num < 0)
    //            {
    //                return false;
    //            }
    //            return true;
    //        }

    //        protected override void OnDeserialization(object sender)
    //        {
    //            this.OnDeserializationImpl(sender);
    //        }

    //        private void OnDeserializationImpl(object sender)
    //        {
    //            if (base.siInfo == null)
    //            {
    //                ThrowHelper.ThrowSerializationException(ExceptionResource.Serialization_InvalidOnDeser);
    //            }
    //            base.comparer = (IComparer<T>)base.siInfo.GetValue("Comparer", typeof(IComparer<T>));
    //            int num = base.siInfo.GetInt32("Count");
    //            this.max = (T)base.siInfo.GetValue(SortedSetPatch<T>.maxName, typeof(T));
    //            this.min = (T)base.siInfo.GetValue(SortedSetPatch<T>.minName, typeof(T));
    //            this.lBoundActive = base.siInfo.GetBoolean(SortedSetPatch<T>.lBoundActiveName);
    //            this.uBoundActive = base.siInfo.GetBoolean(SortedSetPatch<T>.uBoundActiveName);
    //            this.underlying = new SortedSetPatch<T>();
    //            if (num != 0)
    //            {
    //                T[] localArray = (T[])base.siInfo.GetValue("Items", typeof(T[]));
    //                if (localArray == null)
    //                {
    //                    ThrowHelper.ThrowSerializationException(ExceptionResource.Serialization_MissingValues);
    //                }
    //                for (int i = 0; i < localArray.Length; i++)
    //                {
    //                    this.underlying.Add(localArray[i]);
    //                }
    //            }
    //            this.underlying.version = base.siInfo.GetInt32("Version");
    //            base.count = this.underlying.count;
    //            base.version = this.underlying.version - 1;
    //            this.VersionCheck();
    //            if (base.count != num)
    //            {
    //                ThrowHelper.ThrowSerializationException(ExceptionResource.Serialization_MismatchedCount);
    //            }
    //            base.siInfo = null;
    //        }

    //        void IDeserializationCallback.OnDeserialization(object sender)
    //        {
    //        }

    //        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
    //        {
    //            this.GetObjectData(info, context);
    //        }

    //        internal override void VersionCheck()
    //        {
    //            this.VersionCheckImpl();
    //        }

    //        private void VersionCheckImpl()
    //        {
    //            TreeWalkPredicate<T> action = null;
    //            if (base.version != this.underlying.version)
    //            {
    //                base.root = this.underlying.FindRange(this.min, this.max, this.lBoundActive, this.uBoundActive);
    //                base.version = this.underlying.version;
    //                base.count = 0;
    //                if (action == null)
    //                {
    //                    action = delegate(SortedSetPatch<T>.Node n)
    //                    {
    //                        base.count++;
    //                        return true;
    //                    };
    //                }
    //                base.InOrderTreeWalk(action);
    //            }
    //        }
    //    }


    //}

    //#region System.Collections.Generic References

    //internal delegate bool TreeWalkPredicate<T>(SortedSetPatch<T>.Node node);

    //internal class SortedSetDebugView<T>
    //{
    //    // Fields
    //    private SortedSetPatch<T> set;

    //    // Methods
    //    public SortedSetDebugView(SortedSetPatch<T> set)
    //    {
    //        if (set == null)
    //        {
    //            throw new ArgumentNullException("set");
    //        }
    //        this.set = set;
    //    }

    //    // Properties
    //    [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
    //    public T[] Items
    //    {
    //        get
    //        {
    //            return this.set.ToArray();
    //        }
    //    }
    //}

    //internal enum TreeRotation
    //{
    //    LeftRightRotation = 4,
    //    LeftRotation = 1,
    //    RightLeftRotation = 3,
    //    RightRotation = 2
    //}

    //internal class SortedSetEqualityComparer<T> : IEqualityComparer<SortedSetPatch<T>>
    //{
    //    // Fields
    //    private IComparer<T> comparer;
    //    private IEqualityComparer<T> e_comparer;

    //    // Methods
    //    public SortedSetEqualityComparer()
    //        : this(null, null)
    //    {
    //    }

    //    public SortedSetEqualityComparer(IComparer<T> comparer)
    //        : this(comparer, null)
    //    {
    //    }

    //    public SortedSetEqualityComparer(IEqualityComparer<T> memberEqualityComparer)
    //        : this(null, memberEqualityComparer)
    //    {
    //    }

    //    public SortedSetEqualityComparer(IComparer<T> comparer, IEqualityComparer<T> memberEqualityComparer)
    //    {
    //        if (comparer == null)
    //        {
    //            this.comparer = Comparer<T>.Default;
    //        }
    //        else
    //        {
    //            this.comparer = comparer;
    //        }
    //        if (memberEqualityComparer == null)
    //        {
    //            this.e_comparer = EqualityComparer<T>.Default;
    //        }
    //        else
    //        {
    //            this.e_comparer = memberEqualityComparer;
    //        }
    //    }

    //    public override bool Equals(object obj)
    //    {
    //        SortedSetEqualityComparer<T> comparer = obj as SortedSetEqualityComparer<T>;
    //        if (comparer == null)
    //        {
    //            return false;
    //        }
    //        return (this.comparer == comparer.comparer);
    //    }

    //    public bool Equals(SortedSetPatch<T> x, SortedSetPatch<T> y)
    //    {
    //        return SortedSetPatch<T>.SortedSetEquals(x, y, this.comparer);
    //    }

    //    public override int GetHashCode()
    //    {
    //        return (this.comparer.GetHashCode() ^ this.e_comparer.GetHashCode());
    //    }

    //    public int GetHashCode(SortedSetPatch<T> obj)
    //    {
    //        int num = 0;
    //        if (obj != null)
    //        {
    //            foreach (T local in obj)
    //            {
    //                num ^= this.e_comparer.GetHashCode(local) & 0x7fffffff;
    //            }
    //        }
    //        return num;
    //    }
    //}

    //internal class BitHelper
    //{
    //    // Fields
    //    private const byte IntSize = 0x20;
    //    private int[] m_array;
    //    private unsafe int* m_arrayPtr;
    //    private int m_length;
    //    private const byte MarkedBitFlag = 1;
    //    private bool useStackAlloc;

    //    // Methods
    //    [SecurityCritical]
    //    internal unsafe BitHelper(int* bitArrayPtr, int length)
    //    {
    //        this.m_arrayPtr = bitArrayPtr;
    //        this.m_length = length;
    //        this.useStackAlloc = true;
    //    }

    //    internal BitHelper(int[] bitArray, int length)
    //    {
    //        this.m_array = bitArray;
    //        this.m_length = length;
    //    }

    //    [SecurityCritical]
    //    internal unsafe bool IsMarked(int bitPosition)
    //    {
    //        if (this.useStackAlloc)
    //        {
    //            int num = bitPosition / 0x20;
    //            return (((num < this.m_length) && (num >= 0)) && ((this.m_arrayPtr[num] & (((int)1) << (bitPosition % 0x20))) != 0));
    //        }
    //        int index = bitPosition / 0x20;
    //        return (((index < this.m_length) && (index >= 0)) && ((this.m_array[index] & (((int)1) << (bitPosition % 0x20))) != 0));
    //    }

    //    [SecurityCritical]
    //    internal unsafe void MarkBit(int bitPosition)
    //    {
    //        if (this.useStackAlloc)
    //        {
    //            int num = bitPosition / 0x20;
    //            if ((num < this.m_length) && (num >= 0))
    //            {
    //                int* numPtr1 = this.m_arrayPtr + num;
    //                numPtr1[0] |= ((int)1) << (bitPosition % 0x20);
    //            }
    //        }
    //        else
    //        {
    //            int index = bitPosition / 0x20;
    //            if ((index < this.m_length) && (index >= 0))
    //            {
    //                this.m_array[index] |= ((int)1) << (bitPosition % 0x20);
    //            }
    //        }
    //    }

    //    internal static int ToIntArrayLength(int n)
    //    {
    //        if (n <= 0)
    //        {
    //            return 0;
    //        }
    //        return (((n - 1) / 0x20) + 1);
    //    }
    //}

    //#endregion

    //#region System References

    //internal static class ThrowHelper
    //{
    //    // Methods
    //    internal static string GetArgumentName(ExceptionArgument argument)
    //    {
    //        switch (argument)
    //        {
    //            case ExceptionArgument.obj:
    //                return "obj";

    //            case ExceptionArgument.dictionary:
    //                return "dictionary";

    //            case ExceptionArgument.array:
    //                return "array";

    //            case ExceptionArgument.info:
    //                return "info";

    //            case ExceptionArgument.key:
    //                return "key";

    //            case ExceptionArgument.collection:
    //                return "collection";

    //            case ExceptionArgument.match:
    //                return "match";

    //            case ExceptionArgument.converter:
    //                return "converter";

    //            case ExceptionArgument.queue:
    //                return "queue";

    //            case ExceptionArgument.stack:
    //                return "stack";

    //            case ExceptionArgument.capacity:
    //                return "capacity";

    //            case ExceptionArgument.index:
    //                return "index";

    //            case ExceptionArgument.startIndex:
    //                return "startIndex";

    //            case ExceptionArgument.value:
    //                return "value";

    //            case ExceptionArgument.count:
    //                return "count";

    //            case ExceptionArgument.arrayIndex:
    //                return "arrayIndex";

    //            case ExceptionArgument.item:
    //                return "item";
    //        }
    //        return string.Empty;
    //    }

    //    internal static string GetResourceName(ExceptionResource resource)
    //    {
    //        switch (resource)
    //        {
    //            case ExceptionResource.Argument_ImplementIComparable:
    //                return "Argument_ImplementIComparable";

    //            case ExceptionResource.ArgumentOutOfRange_NeedNonNegNum:
    //                return "ArgumentOutOfRange_NeedNonNegNum";

    //            case ExceptionResource.ArgumentOutOfRange_NeedNonNegNumRequired:
    //                return "ArgumentOutOfRange_NeedNonNegNumRequired";

    //            case ExceptionResource.Arg_ArrayPlusOffTooSmall:
    //                return "Arg_ArrayPlusOffTooSmall";

    //            case ExceptionResource.Argument_AddingDuplicate:
    //                return "Argument_AddingDuplicate";

    //            case ExceptionResource.Serialization_InvalidOnDeser:
    //                return "Serialization_InvalidOnDeser";

    //            case ExceptionResource.Serialization_MismatchedCount:
    //                return "Serialization_MismatchedCount";

    //            case ExceptionResource.Serialization_MissingValues:
    //                return "Serialization_MissingValues";

    //            case ExceptionResource.Arg_RankMultiDimNotSupported:
    //                return "Arg_MultiRank";

    //            case ExceptionResource.Arg_NonZeroLowerBound:
    //                return "Arg_NonZeroLowerBound";

    //            case ExceptionResource.Argument_InvalidArrayType:
    //                return "Invalid_Array_Type";

    //            case ExceptionResource.NotSupported_KeyCollectionSet:
    //                return "NotSupported_KeyCollectionSet";

    //            case ExceptionResource.ArgumentOutOfRange_SmallCapacity:
    //                return "ArgumentOutOfRange_SmallCapacity";

    //            case ExceptionResource.ArgumentOutOfRange_Index:
    //                return "ArgumentOutOfRange_Index";

    //            case ExceptionResource.Argument_InvalidOffLen:
    //                return "Argument_InvalidOffLen";

    //            case ExceptionResource.InvalidOperation_CannotRemoveFromStackOrQueue:
    //                return "InvalidOperation_CannotRemoveFromStackOrQueue";

    //            case ExceptionResource.InvalidOperation_EmptyCollection:
    //                return "InvalidOperation_EmptyCollection";

    //            case ExceptionResource.InvalidOperation_EmptyQueue:
    //                return "InvalidOperation_EmptyQueue";

    //            case ExceptionResource.InvalidOperation_EnumOpCantHappen:
    //                return "InvalidOperation_EnumOpCantHappen";

    //            case ExceptionResource.InvalidOperation_EnumFailedVersion:
    //                return "InvalidOperation_EnumFailedVersion";

    //            case ExceptionResource.InvalidOperation_EmptyStack:
    //                return "InvalidOperation_EmptyStack";

    //            case ExceptionResource.InvalidOperation_EnumNotStarted:
    //                return "InvalidOperation_EnumNotStarted";

    //            case ExceptionResource.InvalidOperation_EnumEnded:
    //                return "InvalidOperation_EnumEnded";

    //            case ExceptionResource.NotSupported_SortedListNestedWrite:
    //                return "NotSupported_SortedListNestedWrite";

    //            case ExceptionResource.NotSupported_ValueCollectionSet:
    //                return "NotSupported_ValueCollectionSet";
    //        }
    //        return string.Empty;
    //    }

    //    internal static void IfNullAndNullsAreIllegalThenThrow<T>(object value, ExceptionArgument argName)
    //    {
    //        if ((value == null) && (default(T) != null))
    //        {
    //            ThrowArgumentNullException(argName);
    //        }
    //    }

    //    internal static void ThrowArgumentException(ExceptionResource resource)
    //    {
    //        throw new ArgumentException(SR.GetString(GetResourceName(resource)));
    //    }

    //    internal static void ThrowArgumentNullException(ExceptionArgument argument)
    //    {
    //        throw new ArgumentNullException(GetArgumentName(argument));
    //    }

    //    internal static void ThrowArgumentOutOfRangeException(ExceptionArgument argument)
    //    {
    //        throw new ArgumentOutOfRangeException(GetArgumentName(argument));
    //    }

    //    internal static void ThrowArgumentOutOfRangeException(ExceptionArgument argument, ExceptionResource resource)
    //    {
    //        throw new ArgumentOutOfRangeException(GetArgumentName(argument), SR.GetString(GetResourceName(resource)));
    //    }

    //    internal static void ThrowInvalidOperationException(ExceptionResource resource)
    //    {
    //        throw new InvalidOperationException(SR.GetString(GetResourceName(resource)));
    //    }

    //    internal static void ThrowKeyNotFoundException()
    //    {
    //        throw new KeyNotFoundException();
    //    }

    //    internal static void ThrowNotSupportedException(ExceptionResource resource)
    //    {
    //        throw new NotSupportedException(SR.GetString(GetResourceName(resource)));
    //    }

    //    internal static void ThrowSerializationException(ExceptionResource resource)
    //    {
    //        throw new SerializationException(SR.GetString(GetResourceName(resource)));
    //    }

    //    internal static void ThrowWrongKeyTypeArgumentException(object key, Type targetType)
    //    {
    //        throw new ArgumentException(SR.GetString("Arg_WrongType", new object[] { key, targetType }), "key");
    //    }

    //    internal static void ThrowWrongValueTypeArgumentException(object value, Type targetType)
    //    {
    //        throw new ArgumentException(SR.GetString("Arg_WrongType", new object[] { value, targetType }), "value");
    //    }
    //}

    //internal enum ExceptionArgument
    //{
    //    obj,
    //    dictionary,
    //    array,
    //    info,
    //    key,
    //    collection,
    //    match,
    //    converter,
    //    queue,
    //    stack,
    //    capacity,
    //    index,
    //    startIndex,
    //    value,
    //    count,
    //    arrayIndex,
    //    item
    //}

    //internal enum ExceptionResource
    //{
    //    Argument_ImplementIComparable,
    //    ArgumentOutOfRange_NeedNonNegNum,
    //    ArgumentOutOfRange_NeedNonNegNumRequired,
    //    Arg_ArrayPlusOffTooSmall,
    //    Argument_AddingDuplicate,
    //    Serialization_InvalidOnDeser,
    //    Serialization_MismatchedCount,
    //    Serialization_MissingValues,
    //    Arg_RankMultiDimNotSupported,
    //    Arg_NonZeroLowerBound,
    //    Argument_InvalidArrayType,
    //    NotSupported_KeyCollectionSet,
    //    ArgumentOutOfRange_SmallCapacity,
    //    ArgumentOutOfRange_Index,
    //    Argument_InvalidOffLen,
    //    NotSupported_ReadOnlyCollection,
    //    InvalidOperation_CannotRemoveFromStackOrQueue,
    //    InvalidOperation_EmptyCollection,
    //    InvalidOperation_EmptyQueue,
    //    InvalidOperation_EnumOpCantHappen,
    //    InvalidOperation_EnumFailedVersion,
    //    InvalidOperation_EmptyStack,
    //    InvalidOperation_EnumNotStarted,
    //    InvalidOperation_EnumEnded,
    //    NotSupported_SortedListNestedWrite,
    //    NotSupported_ValueCollectionSet
    //}

    ///// <summary>
    ///// Stub
    ///// </summary>
    //internal sealed class SR
    //{
    //    public static string GetString(string name)
    //    {
    //        return String.Empty;
    //    }

    //    public static string GetString(string name, out bool usedFallback)
    //    {
    //        usedFallback = false;
    //        return GetString(name);
    //    }

    //    public static string GetString(string name, params object[] args)
    //    {
    //        return String.Empty;
    //    }
    //}

    //#endregion
}