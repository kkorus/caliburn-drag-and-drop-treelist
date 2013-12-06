using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Data.Entity;
using System.Windows.Input;
using System.Windows.Media;
using AutoMapper;
using Caliburn.Micro;
using CaliburnApp.Domain;
using CaliburnApp.Domain.Entities;
using CaliburnApp.UI.Extensions;
using CaliburnApp.Domain.Contracts;

namespace CaliburnApp.UI.ViewModels
{
    public class TreeListViewModel : PropertyChangedBase
    {
        public class Node : PropertyChangedBase
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public ObservableCollection<Node> Childs { get; set; }
            public TreeListViewModel TreeList;

            private Node _parent;
            public Node Parent
            {
                get { return _parent; }
                set
                {
                    if (value != _parent)
                    {
                        Node oldParent = _parent;
                        _parent = value;

                        if (oldParent != null)
                        {
                            oldParent.Childs.Remove(this);
                        }
                        _parent.Childs.Add(this);
                    }
                }
            }

            public Node(int id, string name)
            {
                Id = id;
                Name = name;
                Childs = new ObservableCollection<Node>();
            }

            /// <summary>
            /// Cuts this instance.
            /// </summary>
            public void Cut()
            {
                TreeList.CutNode(this);
            }

            /// <summary>
            /// Copies this instance.
            /// </summary>
            public void Copy()
            {
                TreeList.CopyNode(this);
            }

            /// <summary>
            /// Pastes this instance.
            /// </summary>
            public void Paste()
            {
                TreeList.PasteNode(this);
            }

            /// <summary>
            /// Determines whether this instance can paste.
            /// </summary>
            /// <returns></returns>
            public bool CanPaste
            {
                get { return TreeList.NodeClipBoard != null; }
            }
        }

        private Point _startPoint;
        private readonly IBusinessValueObjectsService _businessObjectsService;
        public ObservableCollection<Node> Nodes { get; set; }
        public Node NodeClipBoard { get; set; }
        public bool IsCopying { get; set; }

        private string _textContent { get; set; }
        public string TextContent
        {
            get
            {
                return _textContent;
            }
            set
            {
                _textContent = value;
                NotifyOfPropertyChange(() => TextContent);
            }
        }

        public TreeListViewModel(IBusinessValueObjectsService businessObjectsRepository)
        {
            Nodes = new ObservableCollection<Node>();

            _businessObjectsService = businessObjectsRepository;

            SetData();
            // SetDummyData();
        }

        private void SetDummyData()
        {
            var root1 = new Node(1, "Parent 1");
            (new Node(2, "Child 1 1")).Parent = root1;
            (new Node(3, "Child 1 2")).Parent = root1;
            (new Node(4, "Child 1 3")).Parent = root1;
            (new Node(5, "Child 1 4")).Parent = root1;
            (new Node(6, "Child 1 5")).Parent = root1;
            root1.Childs.ToList().ForEach(x => x.TreeList = this);
            root1.TreeList = this;
            Nodes.Add(root1);

            var root2 = new Node(7, "Parent 2");
            (new Node(8, "Child 2 1")).Parent = root2;
            (new Node(9, "Child 2 2")).Parent = root2;
            (new Node(10, "Child 2 3")).Parent = root2;
            (new Node(11, "Child 2 4")).Parent = root2;
            (new Node(12, "Child 2 5")).Parent = root2;
            root2.Childs.ToList().ForEach(x => x.TreeList = this);
            root2.TreeList = this;
            Nodes.Add(root2);
        }

        private void SetData()
        {
            var objects = _businessObjectsService.Items();
            var root = Mapper.Map<ObservableCollection<Node>>(objects).First();
            root.FlattenHierarchy(x => x.Childs).ToList().ForEach(x => x.TreeList = this);
            Nodes = new ObservableCollection<Node>() { root };

            //foreach (var obj in objects)
            //{
            //    var root = Mapper.Map<Node>(obj);
            //    root.Childs.AddRange(Mapper.Map<ObservableCollection<Node>>(obj.Children).ToList());
            //    if(obj.Parent != null)
            //    result.Add(root);
            //}
            //var result = new List<Node>();
            //Map(objects, result);

            //var dictionaryItems = _dictionaryItemRepository.Items().Include(x => x.Items).ToList();
            //foreach (var dictionary in dictionaryItems)
            //{
            //    var root = Mapper.Map<Node>(dictionary);
            //    root.TreeList = this;
            //    foreach (var item in dictionary.Items)
            //    {
            //        var node = Mapper.Map<Node>(item);
            //        node.Parent = root; 
            //        node.TreeList = this;
            //    }
            //    Nodes.Add(root);
            //}
        }

        public void NodeMouseDoubleClick(object sender, RoutedEventArgs e)
        {
            var tree = sender as TreeView;
            var node = tree.SelectedItem as Node;
            TextContent = string.Format("SOME KIND OF TEXT: {0}", node.Name);
        }

        public void NodePreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _startPoint = e.GetPosition(null);
        }

        public void NodePreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            var treeViewItem = FindAnchestor<TreeViewItem>((DependencyObject)e.OriginalSource);
            if (treeViewItem != null)
            {
                treeViewItem.Focus();
                e.Handled = true;
            }

            var tree = sender as TreeView;
            var node = tree.SelectedItem as Node;
            node.NotifyOfPropertyChange(() => node.CanPaste);
        }

        public void NodeDragEnter(object sender, DragEventArgs e)
        {
            if (!e.Data.GetDataPresent(typeof(Node)) || sender != e.Source)
            {
                e.Effects = DragDropEffects.None;
            }
        }

        public void NodeDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(Node)))
            {
                var node = e.Data.GetData(typeof(Node)) as Node;
                var treeViewItem = FindAnchestor<TreeViewItem>((DependencyObject)e.OriginalSource);

                if (treeViewItem != null)
                {
                    var dropTarget = treeViewItem.Header as Node;
                    if ((node == dropTarget) || dropTarget == null || node == null)
                        return;

                    node.Parent = dropTarget;
                }
            }
        }

        public void NodeMouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                var mousePos = e.GetPosition(null);
                var diff = _startPoint - mousePos;

                if (Math.Abs(diff.X) > SystemParameters.MinimumHorizontalDragDistance
                    || Math.Abs(diff.Y) > SystemParameters.MinimumVerticalDragDistance)
                {
                    var treeView = sender as TreeView;
                    var treeViewItem = FindAnchestor<TreeViewItem>((DependencyObject)e.OriginalSource);

                    if (treeView == null || treeViewItem == null)
                        return;

                    var node = treeView.SelectedItem as Node;
                    if (node == null)
                        return;

                    var dragData = new DataObject(node);
                    DragDrop.DoDragDrop(treeViewItem, dragData, DragDropEffects.Move);
                }
            }
        }

        /// <summary>
        /// Cuts the node.
        /// </summary>
        /// <param name="node">The node.</param>
        public void CutNode(Node node)
        {
            NodeClipBoard = node;
            IsCopying = false;

            if (node.Parent != null)
            {
                node.Parent.Childs.Remove(node);
            }
            else
            {
                Nodes.Remove(node);
            }
        }

        /// <summary>
        /// Copies the node.
        /// </summary>
        /// <param name="node">The node.</param>
        public void CopyNode(Node node)
        {
            NodeClipBoard = node;
            IsCopying = true;
        }

        /// <summary>
        /// Pastes the node.
        /// </summary>
        /// <param name="node">The node.</param>
        public void PasteNode(Node node)
        {
            node.Childs.Add(NodeClipBoard);
            _businessObjectsService.ChangeParent(NodeClipBoard.Id, node.Id, IsCopying);
            NodeClipBoard = null;
        }

        /// <summary>
        /// Finds the anchestor.
        /// Helper to search up the VisualTree
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="current">The current.</param>
        /// <returns></returns>
        private static T FindAnchestor<T>(DependencyObject current) where T : DependencyObject
        {
            do
            {
                if (current is T)
                {
                    return (T)current;
                }
                current = VisualTreeHelper.GetParent(current);
            }
            while (current != null);
            return null;
        }

    }
}
