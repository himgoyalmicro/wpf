// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using MS.Utility;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using MS.Internal;
using System.Reflection;
using System.ComponentModel;
using System.Globalization;

using BuildInfo=MS.Internal.PresentationFramework.BuildInfo;

//---------------------------------------------------------------------------
//
//
//
// Description: ColumnDefinitionCollection implementation. ColumnDefinition implemenation.
//
// Specs:
//      Grid : http://avalon/layout/Specs/Grid.mht
//      Size Sharing: http://avalon/layout/Specs/Size%20Information%20Sharing.doc
//
// Note:        This source file is auto-generated from:
//                  \wcp\Framework\Ms\Utility\GridContentElementCollection.th
//                  \wcp\Framework\Ms\Utility\GridContentElementCollection.tb
//                  \wcp\Framework\Ms\Utility\ColumnDefinitionCollection.ti
//
//
//
//

//
//---------------------------------------------------------------------------

#pragma warning disable 1634, 1691  // suppressing PreSharp warnings

namespace System.Windows.Controls
{
    /// <summary>
    /// A ColumnDefinitionCollection is an ordered, strongly typed, non-sparse
    /// collection of ColumnDefinitions.
    /// </summary>
    /// <remarks>
    /// ColumnDefinitionCollection provides public access for ColumnDefinitions
    /// reading and manipulation.
    /// </remarks>
    
    [TypeConverter(typeof(System.Windows.Controls.ColumnDefinitionsConverter))]
    public sealed class ColumnDefinitionCollection : IList<ColumnDefinition> , IList
    {
        //------------------------------------------------------
        //
        //  Constructors
        //
        //------------------------------------------------------

        #region Constructors

        /// <summary>
        ///     Default ctor.
        /// </summary>
        internal ColumnDefinitionCollection(Grid owner)
        {
            _owner = owner;
            PrivateOnModified();
            if (!string.IsNullOrEmpty(_definitions))
            {
                var rows = _definitions.Split(',');
                foreach (var row in rows)
                {
                    Add(new ColumnDefinition { Width = GridLengthConverter.FromString(row, CultureInfo.InvariantCulture) });
                }
            }
        }

        public ColumnDefinitionCollection(string definitions)
        {
            _definitions = definitions;
            // if (!string.IsNullOrEmpty(definitions))
            // {
            //     var rows = definitions.Split(',');
            //     foreach (var row in rows)
            //     {
            //         var value = new ColumnDefinition { Width = GridLengthConverter.FromString(row, CultureInfo.InvariantCulture) };
            //         PrivateValidateValueForAddition(value);
            //         PrivateInsert(_size, value);
            //     }
            // }
        }

        #endregion Constructors

        //------------------------------------------------------
        //
        //  Public Methods
        //
        //------------------------------------------------------

        #region Public Methods

        /// <summary>
        ///     <see cref="ICollection.CopyTo"/>
        /// </summary>
        void ICollection.CopyTo(Array array, int index)
        {
            ArgumentNullException.ThrowIfNull(array);
            if (array.Rank != 1)
            {
                throw new ArgumentException(SR.GridCollection_DestArrayInvalidRank);
            }
            if (index < 0)
            {
                throw new ArgumentOutOfRangeException(SR.Format(SR.GridCollection_DestArrayInvalidLowerBound, "index"));
            }
            if (array.Length - index < _size)
            {
                throw new ArgumentException(SR.Format(SR.GridCollection_DestArrayInvalidLength, "array"));
            }

            if (_size > 0)
            {
                Debug.Assert(_items != null);
                Array.Copy(_items, 0, array, index, _size);
            }
        }

        /// <summary>
        ///     <see cref="ICollection<T>.CopyTo"/>
        /// </summary>
        public void CopyTo(ColumnDefinition[] array, int index) //  void ICollection<T>.CopyTo(T[] array, int arrayIndex)
        {
            ArgumentNullException.ThrowIfNull(array);
            if (index < 0)
            {
                throw new ArgumentOutOfRangeException(SR.Format(SR.GridCollection_DestArrayInvalidLowerBound, "index"));
            }
            if (array.Length - index < _size)
            {
                throw new ArgumentException(SR.Format(SR.GridCollection_DestArrayInvalidLength, "array"));
            }

            if (_size > 0)
            {
                Debug.Assert(_items != null);
                Array.Copy(_items, 0, array, index, _size);
            }
        }

        /// <summary>
        ///     <see cref="IList.Add"/>
        /// </summary>
        /// <remarks>
        ///     <para>Adding <c>null</c> as <paramref name="value"/> is prohibited.</para>
        ///     <para><paramref name="value"/> must be of ColumnDefinition type.</para>
        /// </remarks>
        int IList.Add(object value)
        {
            PrivateVerifyWriteAccess();
            PrivateValidateValueForAddition(value);
            PrivateInsert(_size, value as ColumnDefinition);
            return (_size - 1);
        }

        /// <summary>
        ///     <see cref="ICollection<T>.Add"/>
        /// </summary>
        public void Add(ColumnDefinition value) //  void ICollection<T>.Add(T item)
        {
            PrivateVerifyWriteAccess();
            PrivateValidateValueForAddition(value);
            PrivateInsert(_size, value);
        }

        /// <summary>
        ///     <see cref="ICollection<T>.Clear"/>
        /// </summary>
        public void Clear() //  void ICollection<T>.Clear();
        {
            PrivateVerifyWriteAccess();
            PrivateOnModified();

            for (int i = 0; i < _size; ++i)
            {
                Debug.Assert(   _items[i] != null
                            &&  _items[i].Parent == _owner );

                PrivateDisconnectChild(_items[i]);
                _items[i] = null;
            }
            _size = 0;
        }

        /// <summary>
        ///     <see cref="IList.Contains"/>
        /// </summary>
        bool IList.Contains(object value)
        {
            ColumnDefinition item = value as ColumnDefinition;
            if (    item != null
                &&  item.Parent == _owner  )
            {
                Debug.Assert(_items[item.Index] == item);
                return (true);
            }

            return (false);
        }

        /// <summary>
        ///     <see cref="ICollection<T>.Contains"/>
        /// </summary>
        public bool Contains(ColumnDefinition value)    //  bool ICollection<T>.Contains(T item)
        {
            if (    value != null
                &&  value.Parent == _owner  )
            {
                Debug.Assert(_items[value.Index] == value);
                return (true);
            }

            return (false);
        }

        /// <summary>
        ///     <see cref="IList.IndexOf"/>
        /// </summary>
        int IList.IndexOf(object value)
        {
            return (this.IndexOf(value as ColumnDefinition));
        }

        /// <summary>
        ///     <see cref="IList<T>.IndexOf"/>
        /// </summary>
        public int IndexOf(ColumnDefinition value)  //  int IList<T>.IndexOf(T item);
        {
            if (    value == null
                ||  value.Parent != _owner )
            {
                return (-1);
            }
            else
            {
                return value.Index;
            }
        }

        /// <summary>
        ///     <see cref="IList.Insert"/>
        /// </summary>
        /// <remarks>
        ///     <paramref name="value"/> must be of ColumnDefinition type.
        /// </remarks>
        void IList.Insert(int index, object value)
        {
            PrivateVerifyWriteAccess();
            if (index < 0 || index > _size)
            {
                throw new ArgumentOutOfRangeException(SR.TableCollectionOutOfRange);
            }
            PrivateValidateValueForAddition(value);
            PrivateInsert(index, value as ColumnDefinition);
        }

        /// <summary>
        ///     <see cref="IList<T>.Insert"/>
        /// </summary>
        public void Insert(int index, ColumnDefinition value)   //  void IList<T>.Insert(int index, T item)
        {
            PrivateVerifyWriteAccess();
            if (index < 0 || index > _size)
            {
                throw new ArgumentOutOfRangeException(SR.TableCollectionOutOfRange);
            }
            PrivateValidateValueForAddition(value);
            PrivateInsert(index, value);
        }

        /// <summary>
        ///     <see cref="IList.Remove"/>
        /// </summary>
        /// <remarks>
        ///     <paramref name="value"/> must be of ColumnDefinition type.
        /// </remarks>
        void IList.Remove(object value)
        {
            PrivateVerifyWriteAccess();
            bool found = PrivateValidateValueForRemoval(value);
            if (found)
            {
                PrivateRemove(value as ColumnDefinition);
            }
        }

        /// <summary>
        ///     <see cref="ICollection<T>.Remove"/>
        /// </summary>
        public bool Remove(ColumnDefinition value)  //  bool ICollection<T>.Remove(T item)
        {
            bool found = PrivateValidateValueForRemoval(value);
            if (found)
            {
                PrivateRemove(value as ColumnDefinition);
            }
            return (found);
        }

        /// <summary>
        ///     <see cref="IList<T>.RemoveAt"/>
        ///     <seealso cref="IList.RemoveAt"/>
        /// </summary>
        public void RemoveAt(int index) //  void IList.RemoveAt(int index); void IList<T>.RemoveAt(int index)
        {
            PrivateVerifyWriteAccess();
            if (index < 0 || index >= _size)
            {
                throw new ArgumentOutOfRangeException(SR.TableCollectionOutOfRange);
            }
            PrivateRemove(_items[index]);
        }

        /// <summary>
        ///     Removes a range of ColumnDefinitions from the ColumnDefinitionCollection.
        /// </summary>
        /// <param name="index">
        ///     The zero-based index of the range of ColumnDefinitions to remove.
        /// </param>
        /// <param name="count">
        ///     The number of ColumnDefinitions to remove.
        /// </param>
        public void RemoveRange(int index, int count)
        {
            PrivateVerifyWriteAccess();
            if (index < 0 || index >= _size)
            {
                throw new ArgumentOutOfRangeException(SR.TableCollectionOutOfRange);
            }
            if (count < 0)
            {
                throw new ArgumentOutOfRangeException(SR.TableCollectionCountNeedNonNegNum);
            }
            if (_size - index < count)
            {
                throw new ArgumentException(SR.TableCollectionRangeOutOfRange);
            }

            PrivateOnModified();

            if (count > 0)
            {
                for (int i = index + count - 1; i >= index; --i)
                {
                    Debug.Assert(   _items[i] != null
                                &&  _items[i].Parent == _owner );

                    PrivateDisconnectChild(_items[i]);
                }

                _size -= count;
                for (int i = index; i < _size; ++i)
                {
                    Debug.Assert(   _items[i + count] != null
                                &&  _items[i + count].Parent == _owner );

                    _items[i] = _items[i + count];
                    _items[i].Index = i;
                    _items[i + count] = null;
                }
            }
        }

        /// <summary>
        ///     <see cref="IEnumerable.GetEnumerator"/>
        /// </summary>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return (new Enumerator(this));
        }

        /// <summary>
        ///     <see cref="IEnumerable<T>.GetEnumerator"/>
        /// </summary>
        IEnumerator<ColumnDefinition> IEnumerable<ColumnDefinition>.GetEnumerator()
        {
            return (new Enumerator(this));
        }

        #endregion Public Methods

        //------------------------------------------------------
        //
        //  Public Properties
        //
        //------------------------------------------------------

        #region Public Properties

        /// <summary>
        ///     <see cref="ICollection<T>.Count"/>
        ///     <seealso cref="ICollection.Count"/>
        /// </summary>
        public int Count    //  int ICollection.Count {get;}; int ICollection<T>.Count {get;}
        {
            get { return (_size); }
        }

        /// <summary>
        ///     <see cref="IList.IsFixedSize"/>
        /// </summary>
        bool IList.IsFixedSize
        {
            get
            {
                return (    _owner.MeasureOverrideInProgress
                        ||  _owner.ArrangeOverrideInProgress    );
            }
        }

        /// <summary>
        ///     <see cref="ICollection<T>.IsReadOnly"/>
        ///     <seealso cref="IList.IsReadOnly"/>
        /// </summary>
        public bool IsReadOnly  //  bool IList.IsReadOnly {get;}; bool ICollection<T>.IsReadOnly {get;}
        {
            get
            {
                return (    _owner.MeasureOverrideInProgress
                        ||  _owner.ArrangeOverrideInProgress    );
            }
        }

        /// <summary>
        ///     <see cref="ICollection.IsSynchronized"/>
        /// </summary>
        public bool IsSynchronized  //  bool IColleciton.IsSynchronized {get;};
        {
            get { return (false); }
        }

        /// <summary>
        ///     <see cref="ICollection.SyncRoot"/>
        /// </summary>
        public object SyncRoot  //  object ICollection.SyncRoot {get;};
        {
            get { return (this); }
        }

        /// <summary>
        ///     <see cref="IList.this"/>
        /// </summary>
        /// <remarks>
        ///     <para>Setting <c>null</c> as <paramref name="value"/> is prohibited.</para>
        ///     <para><paramref name="value"/> must be of ColumnDefinition type.</para>
        /// </remarks>
        object IList.this[int index]
        {
            get
            {
                if (index < 0 || index >= _size)
                {
                    throw new ArgumentOutOfRangeException(SR.TableCollectionOutOfRange);
                }
                return (_items[index]);
            }
            set
            {
                PrivateVerifyWriteAccess();
                PrivateValidateValueForAddition(value);
                if (index < 0 || index >= _size)
                {
                    throw new ArgumentOutOfRangeException(SR.TableCollectionOutOfRange);
                }
                PrivateDisconnectChild(_items[index]);
                PrivateConnectChild(index, value as ColumnDefinition);
            }
        }

        /// <summary>
        ///     <see cref="IList<T>.Item"/>
        /// </summary>
        public ColumnDefinition this[int index] //  T IList<T>.this[int index] {get; set;}
        {
            get
            {
                if (index < 0 || index >= _size)
                {
                    throw new ArgumentOutOfRangeException(SR.TableCollectionOutOfRange);
                }
                return ((ColumnDefinition)_items[index]);
            }
            set
            {
                PrivateVerifyWriteAccess();
                PrivateValidateValueForAddition(value);
                if (index < 0 || index >= _size)
                {
                    throw new ArgumentOutOfRangeException(SR.TableCollectionOutOfRange);
                }
                PrivateDisconnectChild(_items[index]);
                PrivateConnectChild(index, value);
            }
        }

        #endregion Public Properties

        //------------------------------------------------------
        //
        //  Internal Methods
        //
        //------------------------------------------------------

        #region Internal Methods

        /// <summary>
        ///     Frees un-used memory.
        /// </summary>
        internal void InternalTrimToSize()
        {
            PrivateSetCapacity(_size);
        }

        #endregion Internal Methods

        //------------------------------------------------------
        //
        //  Internal Properties
        //
        //------------------------------------------------------

        #region Internal Properties

        /// <summary>
        ///     Internal version of Count.
        /// </summary>
        internal int InternalCount
        {
            get { return (_size);   }
        }

        /// <summary>
        ///     Internal accessor to items array.
        /// </summary>
        internal DefinitionBase[] InternalItems
        {
            get {   return (_items);    }
        }

        #endregion Internal Properties

        //------------------------------------------------------
        //
        //  Private Methods
        //
        //------------------------------------------------------

        #region Private Methods

        /// <summary>
        ///     Throws if the collection is in readonly state.
        /// </summary>
        private void PrivateVerifyWriteAccess()
        {
            if (this.IsReadOnly)
            {
                throw new InvalidOperationException(SR.Format(SR.GridCollection_CannotModifyReadOnly, "ColumnDefinitionCollection"));
            }
        }

        /// <summary>
        ///     Throws if value is not something the collection expects.
        /// </summary>
        private void PrivateValidateValueForAddition(object value)
        {
            ArgumentNullException.ThrowIfNull(value);

            ColumnDefinition item = value as ColumnDefinition;

            if (item == null)
            {
                throw new ArgumentException(SR.Format(SR.GridCollection_MustBeCertainType, "ColumnDefinitionCollection", "ColumnDefinition"));
            }

            if (item.Parent != null)
            {
                throw new ArgumentException(SR.Format(SR.GridCollection_InOtherCollection, "value", "ColumnDefinitionCollection"));
            }
        }

        /// <summary>
        ///     Throws if value is not something the collection expects.
        ///     Returns true if value belongs to the collection.
        /// </summary>
        private bool PrivateValidateValueForRemoval(object value)
        {
            ArgumentNullException.ThrowIfNull(value);

            ColumnDefinition item = value as ColumnDefinition;

            if (item == null)
            {
                throw new ArgumentException(SR.Format(SR.GridCollection_MustBeCertainType, "ColumnDefinitionCollection", "ColumnDefinition"));
            }

            return (item.Parent == _owner);
        }

        /// <summary>
        ///     Sets the specified DefinitionBase at the specified index;
        ///     Connects the value to the model tree;
        ///     Notifies the DefinitionBase about the event.
        /// </summary>
        /// <remarks>
        ///     Note that the function requires that _item[index] == null and
        ///     it also requires that the passed in value is not included into another ColumnDefinitionCollection.
        /// </remarks>
        private void PrivateConnectChild(int index, DefinitionBase value)
        {
            Debug.Assert(value != null && value.Index == -1);
            Debug.Assert(_items[index] == null);

            // add the value into collection's array
            _items[index] = value;
            value.Index = index;

            _owner.AddLogicalChild(value);
            value.OnEnterParentTree();
        }

        /// <summary>
        ///     Notifies the DefinitionBase about the event;
        ///     Disconnects the value from the model tree;
        ///     Sets the DefinitionBase's slot in the collection's array to null.
        /// </summary>
        private void PrivateDisconnectChild(DefinitionBase value)
        {
            Debug.Assert(value != null);

            value.OnExitParentTree();

            // remove the value from collection's array
            _items[value.Index] = null;
            value.Index = -1;

            _owner.RemoveLogicalChild(value);
        }

        /// <summary>
        ///     PrivateInsert inserts specified DefinitionBase into the
        ///     ColumnDefinitionCollection at the specified index. Index is allowed
        ///     to be equal to the current size of the collection. When index
        ///     is equal to size, PrivateInsert effectively performs Add
        ///     operation.
        /// </summary>
        private void PrivateInsert(int index, DefinitionBase value)
        {
            PrivateOnModified();

            if (_items == null)
            {
                PrivateSetCapacity(c_defaultCapacity);
            }
            else if (_size == _items.Length)
            {
                PrivateSetCapacity(Math.Max(_items.Length * 2, c_defaultCapacity));
            }

            for (int i = _size - 1; i >= index; --i)
            {
                Debug.Assert(   _items[i] != null
                            &&  _items[i].Parent == _owner );

                _items[i + 1] = _items[i];
                _items[i].Index = i + 1;
            }

            _items[index] = null;

            _size++;
            PrivateConnectChild(index, value);
        }

        /// <summary>
        ///     Removes specified DefinitionBase from the ColumnDefinitionCollection.
        /// </summary>
        private void PrivateRemove(DefinitionBase value)
        {
            Debug.Assert(   _items[value.Index] == value
                        &&  value.Parent == _owner );

            PrivateOnModified();

            int index = value.Index;

            PrivateDisconnectChild(value);

            --_size;

            for (int i = index; i < _size; ++i)
            {
                Debug.Assert(   _items[i + 1] != null
                            &&  _items[i + 1].Parent == _owner    );

                _items[i] = _items[i + 1];
                _items[i].Index = i;
            }

            _items[_size] = null;
        }

        /// <summary>
        ///     Updates version of the ColumnDefinitionCollection.
        ///     Norifies owner grid about the change.
        /// </summary>
        private void PrivateOnModified()
        {
            _version++;
            _owner.ColumnDefinitionCollectionDirty = true;
            _owner.Invalidate();
        }

        /// <summary>
        ///     Handles internal strorage capacity.
        /// </summary>
        private void PrivateSetCapacity(int value)
        {
            Debug.Assert(value >= _size);

            if (value <= 0)
            {
                _items = null;
            }
            else if (_items == null || value != _items.Length)
            {
                ColumnDefinition[] newItems = new ColumnDefinition[value];
                if (_size > 0)
                {
                    Array.Copy(_items, 0, newItems, 0, _size);
                }
                _items = newItems;
            }
        }

        #endregion Private Methods

        //------------------------------------------------------
        //
        //  Private Fields
        //
        //------------------------------------------------------

        #region Private Fields
        private readonly Grid _owner;      //  owner of the collection
        private DefinitionBase[] _items;            //  storage of items
        private int _size;                          //  size of the collection
        private int _version;                       //  version tracks updates in the collection
        private const int c_defaultCapacity = 4;    //  default capacity of the collection
        private string _definitions;
        #endregion Private Fields

        //------------------------------------------------------
        //
        //  Private Structures / Classes
        //
        //------------------------------------------------------

        #region Private Structures Classes

        /// <summary>
        ///     Dual purpose IEnumerator / IEnumerator<T> implementation.
        /// </summary>
        /// <remarks>
        ///     Enumerator can be initialized with null as a collection reference.
        ///     If the case Enumerator behaves as empty enumerator.
        /// </remarks>
        internal struct Enumerator : IEnumerator<ColumnDefinition>, IEnumerator
        {
            /// <summary>
            ///     Default ctor.
            /// </summary>
            internal Enumerator(ColumnDefinitionCollection collection)
            {
                _collection = collection;
                _index = -1;
                _version = _collection != null ? _collection._version : -1;
                _currentElement = collection;
            }

            /// <summary>
            ///     <see cref="IEnumerator<T>.MoveNext"/>
            ///     <seealso cref="IEnumerator.MoveNext"/>
            /// </summary>
            public bool MoveNext()
            {
                if (_collection == null)
                {
                    // empty enumerator case.
                    return (false);
                }

                PrivateValidate();

                if (_index < (_collection._size - 1))
                {
                    _index++;
                    _currentElement = _collection[_index];
                    return (true);
                }
                else
                {
                    _currentElement = _collection;
                    _index = _collection._size;
                    return (false);
                }
            }


            /// <summary>
            ///     <see cref="IEnumerator.Current"/>
            /// </summary>
            object IEnumerator.Current
            {
                get
                {
                    //  note: (_currentElement == _collection) works for empty enumerator too.
                    //  if the case then EnumeratorNotStarted will be thrown.
                    if (_currentElement == _collection)
                    {
                        if (_index == -1)
                        {
                            #pragma warning suppress 6503 // IEnumerator.Current is documented to throw this exception
                            throw new InvalidOperationException(SR.EnumeratorNotStarted);
                        }
                        else
                        {
                            #pragma warning suppress 6503 // IEnumerator.Current is documented to throw this exception
                            throw new InvalidOperationException(SR.EnumeratorReachedEnd);
                        }
                    }
                    return (_currentElement);
                }
            }

            /// <summary>
            ///     <see cref="IEnumerator<T>.Current"/>
            /// </summary>
            public ColumnDefinition Current
            {
                get
                {
                    //  note: (_currentElement == _collection) works for empty enumerator too.
                    //  if the case then EnumeratorNotStarted will be thrown.
                    if (_currentElement == _collection)
                    {
                        if (_index == -1)
                        {
                            #pragma warning suppress 6503 // IEnumerator.Current is documented to throw this exception
                            throw new InvalidOperationException(SR.EnumeratorNotStarted);
                        }
                        else
                        {
                            #pragma warning suppress 6503 // IEnumerator.Current is documented to throw this exception
                            throw new InvalidOperationException(SR.EnumeratorReachedEnd);
                        }
                    }
                    return ((ColumnDefinition)_currentElement);
                }
            }

            /// <summary>
            ///     <see cref="IEnumerator.Reset"/>
            /// </summary>
            public void Reset()
            {
                if (_collection == null)
                {
                    //  empty enumerator case.
                    //  it is Ok to just return here without checking if
                    //  the enumerator is disposed as long as empty enumerator
                    //  is internal only.
                    return;
                }

                PrivateValidate();
                _currentElement = _collection;
                _index = -1;
            }

            /// <summary>
            ///     <see cref="IDisposable.Dispose"/>
            /// </summary>
            public void Dispose()
            {
                _currentElement = null;
            }

            /// <summary>
            ///     Validates that
            ///     enumerator is not disposed;
            ///     enumerator is still in sync with collection;
            /// </summary>
            private void PrivateValidate()
            {
                if (_currentElement == null)
                {
                    throw new InvalidOperationException(SR.EnumeratorCollectionDisposed);
                }
                if (_version != _collection._version)
                {
                    throw new InvalidOperationException(SR.EnumeratorVersionChanged);
                }
            }

            private ColumnDefinitionCollection _collection;              //  the collection to be enumerated
            private int _index;                         //  current element index
            private int _version;                       //  the snapshot of collection's version at the time of creation
            private object _currentElement;             //  multipurpose:
                                                        //  points to the collection object when enumerator is either before start or after end
                                                        //  points to the current element while in the process of enumeration
                                                        //  is null if disposed
        }

        #endregion Private Structures Classes
    }

    /// <summary>
    ///     ColumnDefinition is a FrameworkContentElement used by Grid
    ///     to hold column / row specific properties.
    /// </summary>
    public class ColumnDefinition : DefinitionBase
    {
        //------------------------------------------------------
        //
        //  Constructors
        //
        //------------------------------------------------------

        #region Constructors

        /// <summary>
        ///     Default ctor.
        /// </summary>
        public ColumnDefinition()
            : base(DefinitionBase.ThisIsColumnDefinition)
        {
        }

        #endregion Constructors

        //------------------------------------------------------
        //
        //  Public Properties
        //
        //------------------------------------------------------

        #region Public Properties

        /// <summary>
        ///     Sets specified Width value for the ColumnDefinition.
        ///     Returns current Width value for the ColumnDefinition.
        /// </summary>
        public GridLength Width
        {
            get { return (base.UserSizeValueCache); }
            set { SetValue(WidthProperty, value); }
        }

        /// <summary>
        ///     Sets specified MinWidth value for the ColumnDefinition.
        ///     Returns current MinWidth value for the ColumnDefinition.
        /// </summary>
        [TypeConverter(typeof(LengthConverter))]
        public double MinWidth
        {
            get { return (base.UserMinSizeValueCache); }
            set { SetValue(MinWidthProperty, value); }
        }

        /// <summary>
        ///     Sets specified MaxWidth value for the ColumnDefinition.
        ///     Returns current MaxWidth value for the ColumnDefinition.
        /// </summary>
        [TypeConverter(typeof(LengthConverter))]
        public double MaxWidth
        {
            get { return (base.UserMaxSizeValueCache); }
            set { SetValue(MaxWidthProperty, value); }
        }

        /// <summary>
        ///     Returns calculated device independent pixel value of Width for the ColumnDefinition.
        /// </summary>
        public double ActualWidth
        {
            get
            {
                double value = 0.0;

                if (base.InParentLogicalTree)
                {
                    value = ((Grid)base.Parent).GetFinalColumnDefinitionWidth(base.Index);
                }

                return (value);
            }
        }

        /// <summary>
        ///     Returns calculated device independent pixel value of the ColumnDefinition's offset
        ///     in the coordinate system of the owning grid.
        /// </summary>
        public double Offset
        {
            get
            {
                double value = 0.0;

                if (base.Index != 0)
                {
                    value = base.FinalOffset;
                }

                return (value);
            }
        }

        #endregion Public Properties

        //------------------------------------------------------
        //
        //  Dynamic Properties
        //
        //------------------------------------------------------

        #region Dynamic Properties

        /// <summary>
        /// Width property.
        /// </summary>
        [MS.Internal.PresentationFramework.CommonDependencyProperty]
        public static readonly DependencyProperty WidthProperty =
                DependencyProperty.Register(
                        "Width",
                        typeof(GridLength),
                        typeof(ColumnDefinition),
                        new FrameworkPropertyMetadata(
                                new GridLength(1.0, GridUnitType.Star),
                                new PropertyChangedCallback(OnUserSizePropertyChanged)),
                        new ValidateValueCallback(IsUserSizePropertyValueValid));

        /// <summary>
        /// MinWidth property.
        /// </summary>
        [MS.Internal.PresentationFramework.CommonDependencyProperty]
        [TypeConverter($"System.Windows.LengthConverter, PresentationFramework, Version={BuildInfo.WCP_VERSION}, Culture=neutral, PublicKeyToken={BuildInfo.WCP_PUBLIC_KEY_TOKEN}, Custom=null")]
        public static readonly DependencyProperty MinWidthProperty =
                DependencyProperty.Register(
                        "MinWidth",
                        typeof(double),
                        typeof(ColumnDefinition),
                        new FrameworkPropertyMetadata(
                                0d,
                                new PropertyChangedCallback(OnUserMinSizePropertyChanged)),
                        new ValidateValueCallback(IsUserMinSizePropertyValueValid));

        /// <summary>
        /// MaxWidth property.
        /// </summary>
        [MS.Internal.PresentationFramework.CommonDependencyProperty]
        [TypeConverter($"System.Windows.LengthConverter, PresentationFramework, Version={BuildInfo.WCP_VERSION}, Culture=neutral, PublicKeyToken={BuildInfo.WCP_PUBLIC_KEY_TOKEN}, Custom=null")]
        public static readonly DependencyProperty MaxWidthProperty =
                DependencyProperty.Register(
                        "MaxWidth",
                        typeof(double),
                        typeof(ColumnDefinition),
                        new FrameworkPropertyMetadata(
                                Double.PositiveInfinity,
                                new PropertyChangedCallback(OnUserMaxSizePropertyChanged)),
                        new ValidateValueCallback(IsUserMaxSizePropertyValueValid));

        #endregion Dynamic Properties
    }
}


