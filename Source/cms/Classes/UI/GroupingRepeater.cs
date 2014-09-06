﻿using System;
using System.Collections;
using System.ComponentModel;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Csbs.Web.UI
{
    /// <summary>
    ///     Summary description for GroupingRepeater.
    ///     When another group is found, render an additional template.
    /// </summary>
    /// <remarks>
    ///     This control has a bug, which makes it unable to use postbacking controls
    /// </remarks>
    [ParseChildren(true)]
    public class GroupingRepeater : Control,
        INamingContainer
    {
        private IComparer _comparer;
        private object _dataSource;
        private ITemplate _groupTemplate;
        private ITemplate _itemTemplate;

        public IComparer Comparer
        {
            get { return _comparer; }
            set { _comparer = value; }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual object DataSource
        {
            get { return _dataSource; }
            set { _dataSource = value; }
        }

        [PersistenceMode(PersistenceMode.InnerProperty)]
        [TemplateContainer(typeof (RepeaterItem))]
        public ITemplate GroupTemplate
        {
            get { return _groupTemplate; }
            set { _groupTemplate = value; }
        }

        [PersistenceMode(PersistenceMode.InnerProperty)]
        [TemplateContainer(typeof (RepeaterItem))]
        public ITemplate ItemTemplate
        {
            get { return _itemTemplate; }
            set { _itemTemplate = value; }
        }

        public event RepeaterItemEventHandler ItemCreated;
        public event RepeaterItemEventHandler ItemDataBound;
        public event RepeaterCommandEventHandler ItemCommand;

        protected void OnItemDataBound(RepeaterItemEventArgs e)
        {
            if (ItemDataBound != null)
                ItemDataBound(this, e);
        }

        protected void OnItemCreated(RepeaterItemEventArgs e)
        {
            if (ItemCreated != null)
                ItemCreated(this, e);
        }

        protected void OnItemCommand(RepeaterCommandEventArgs e)
        {
            if (ItemCommand != null)
                ItemCommand(this, e);
        }

        public override void DataBind()
        {
            // Controls with a data-source property perform their 
            // custom data binding by overriding DataBind to
            // evaluate any data-binding expressions on the control    
            // itself.
            base.OnDataBinding(EventArgs.Empty);

            // Reset the control's state.
            Controls.Clear();
            ClearChildViewState();

            // Create the control hierarchy using the data source.
            CreateControlHierarchy(true);
            ChildControlsCreated = true;

            TrackViewState();
        }

        protected override void CreateChildControls()
        {
            Controls.Clear();

            if (ViewState["ItemCount"] != null)
            {
                // Create the control hierarchy using the view state, 
                // not the data source.
                CreateControlHierarchy(false);
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="useDataSource">True to create the hierarchy from the DataSource, False to create it from the ViewState.</param>
        private void CreateControlHierarchy(bool useDataSource)
        {
            IEnumerable dataSource = null;
            int count = -1;

            if (useDataSource == false)
            {
                // ViewState must have a non-null value for ItemCount because this is checked 
                //  by CreateChildControls.
                count = (int) ViewState["ItemCount"];
                if (count != -1)
                {
                    dataSource = new DummyDataSource(count);
                }
            }
            else
            {
                dataSource = GetResolvedDataSource(_dataSource, null);
            }

            if (dataSource != null)
            {
                int index = 0;
                count = 0;
                object lastValue = null;
                foreach (object dataItem in dataSource)
                {
                    bool insertGroupRow;
                    if (useDataSource)
                    {
                        insertGroupRow = (_comparer.Compare(lastValue, dataItem) != 0);
                    }
                    else
                    {
                        insertGroupRow = (bool) ViewState["gotaGroup" + index];
                    }

                    if (insertGroupRow)
                    {
                        CreateItem(index, _groupTemplate, ListItemType.Separator, useDataSource, dataItem);
                        //count++;
                    }

                    CreateItem(index, _itemTemplate, ListItemType.Item, useDataSource, dataItem);
                    lastValue = dataItem;

                    if (useDataSource)
                    {
                        ViewState["gotaGroup" + index] = insertGroupRow;
                    }

                    count++;
                    index++;
                }
            }

            if (useDataSource)
            {
                // Save the number of items contained for use in round trips.
                ViewState["ItemCount"] = ((dataSource != null) ? count : -1);
            }
        }

        private RepeaterItem CreateItem(int itemIndex, ITemplate template, ListItemType itemType, bool dataBind, object dataItem)
        {
            var item = new RepeaterItem(itemIndex, itemType);

            var e = new RepeaterItemEventArgs(item);

            template.InstantiateIn(item);

            if (dataBind)
            {
                item.DataItem = dataItem;
            }
            OnItemCreated(e);
            Controls.Add(item);

            if (dataBind)
            {
                item.DataBind();
                OnItemDataBound(e);

                item.DataItem = null;
            }

            return item;
        }

        protected override bool OnBubbleEvent(object source, EventArgs e)
        {
            // Handle events raised by children by overriding OnBubbleEvent.

            bool handled = false;

            if (e is CommandEventArgs)
            {
                var ce = (RepeaterCommandEventArgs) e;

                OnItemCommand(ce);
                handled = true;
            }

            return handled;
        }

        /// <summary>
        ///     The following code was obtained by using Reflector(tm).
        ///     It tries to transform any object into something IEnumerable
        /// </summary>
        /// <param name="dataSource"></param>
        /// <param name="dataMember"></param>
        /// <returns></returns>
        internal static IEnumerable GetResolvedDataSource(object dataSource, string dataMember)
        {
            if (dataSource == null)
            {
                return null;
            }
            var source1 = dataSource as IListSource;
            if (source1 != null)
            {
                IList list1 = source1.GetList();
                if (!source1.ContainsListCollection)
                {
                    return list1;
                }
                if ((list1 != null) && (list1 is ITypedList))
                {
                    var list2 = (ITypedList) list1;
                    PropertyDescriptorCollection collection1 = list2.GetItemProperties(new PropertyDescriptor[0]);
                    if ((collection1 == null) || (collection1.Count == 0))
                    {
                        throw new HttpException("ListSource_Without_DataMembers");
                    }
                    PropertyDescriptor descriptor1 = null;
                    if ((dataMember == null) || (dataMember.Length == 0))
                    {
                        descriptor1 = collection1[0];
                    }
                    else
                    {
                        descriptor1 = collection1.Find(dataMember, true);
                    }
                    if (descriptor1 != null)
                    {
                        object obj1 = list1[0];
                        object obj2 = descriptor1.GetValue(obj1);
                        if ((obj2 != null) && (obj2 is IEnumerable))
                        {
                            return ((IEnumerable) obj2);
                        }
                    }
                    throw new HttpException("ListSource_Missing_DataMember");
                }
            }
            if (dataSource is IEnumerable)
            {
                return ((IEnumerable) dataSource);
            }
            return null;
        }
    }

    internal sealed class DummyDataSource : ICollection
    {
        private readonly int dataItemCount;

        public DummyDataSource(int dataItemCount)
        {
            this.dataItemCount = dataItemCount;
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public int Count
        {
            get { return dataItemCount; }
        }

        public bool IsSynchronized
        {
            get { return false; }
        }

        public object SyncRoot
        {
            get { return this; }
        }

        public void CopyTo(Array array, int index)
        {
            for (IEnumerator e = GetEnumerator(); e.MoveNext();)
                array.SetValue(e.Current, index++);
        }

        public IEnumerator GetEnumerator()
        {
            return new BasicDataSourceEnumerator(dataItemCount);
        }

        private class BasicDataSourceEnumerator : IEnumerator
        {
            private readonly int count;
            private int index;

            public BasicDataSourceEnumerator(int count)
            {
                this.count = count;
                index = -1;
            }

            public object Current
            {
                get { return null; }
            }

            public bool MoveNext()
            {
                index++;
                return index < count;
            }

            public void Reset()
            {
                index = -1;
            }
        }
    }
}