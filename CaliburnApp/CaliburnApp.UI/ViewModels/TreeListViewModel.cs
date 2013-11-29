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

namespace CaliburnApp.UI.ViewModels
{
    public class TreeListViewModel : PropertyChangedBase
    {
        public class Node
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public ObservableCollection<Node> Childs { get; set; }

            public Node(int id, string name)
            {
                Id = id;
                Name = name;
                Childs = new ObservableCollection<Node>();
            }

            /// <summary>
            /// to delete ?
            /// </summary>
            /// <param name="child"></param>
            public void AddChild(Node child)
            {
                child.Parent = this;
            }

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
                        else
                        {
                            _parent = value;
                        }

                        _parent.Childs.Add(this);
                    }
                }
            }
        }

        private Point _startPoint;
        private TreeListViewModel _parent;
        private readonly IRepository<Dictionary> _dictionaryItemRepository;
        public ObservableCollection<Node> Nodes { get; set; }
        public TreeViewItem SelectedItem { get; set; }
        public TreeViewItem PasteTo { get; set; }
        public bool IsCopying { get; set; }

        public TreeListViewModel(IRepository<Dictionary> repository)
        {
            Nodes = new ObservableCollection<Node>();
            _dictionaryItemRepository = repository;

            SetData();
            //SetDummyData();
        }

        private void SetDummyData()
        {
            // data retrived from database
            var root1 = new Node(1, "Parent 1");
            (new Node(2, "Child 1 1")).Parent = root1;
            (new Node(3, "Child 1 2")).Parent = root1;
            (new Node(4, "Child 1 3")).Parent = root1;
            (new Node(5, "Child 1 4")).Parent = root1;
            (new Node(6, "Child 1 5")).Parent = root1;
            Nodes.Add(root1);

            var root2 = new Node(7, "Parent 2");
            (new Node(8, "Child 2 1")).Parent = root2;
            (new Node(9, "Child 2 2")).Parent = root2;
            (new Node(10, "Child 2 3")).Parent = root2;
            (new Node(11, "Child 2 4")).Parent = root2;
            (new Node(12, "Child 2 5")).Parent = root2;
            Nodes.Add(root2);
        }

        private void SetData()
        {
            var dictionaryItems = _dictionaryItemRepository.Items().Include(x => x.Items).ToList();
            foreach (var dictionary in dictionaryItems)
            {
                var root = Mapper.Map<Node>(dictionary);
                foreach (var item in dictionary.Items)
                {
                    Mapper.Map<Node>(item).Parent = root;
                }
                Nodes.Add(root);
            }
        }

        private bool isSelected;
        public bool IsSelected
        {
            get { return isSelected; }
            set
            {
                if (value != isSelected)
                {
                    isSelected = value;
                    NotifyOfPropertyChange(() => IsSelected);
                }
            }
        }

        private bool isExpanded;
        public bool IsExpanded
        {
            get { return isExpanded; }
            set
            {
                if (value != isExpanded)
                {
                    isExpanded = value;
                    NotifyOfPropertyChange(() => IsExpanded);
                }

                if (isExpanded && _parent != null && !_parent.IsExpanded)
                    _parent.IsExpanded = true;
            }
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
                if(!IsCopying)
                    SelectedItem = treeViewItem;
                else
                {
                    PasteTo = treeViewItem;
                }

                treeViewItem.Focus();
                e.Handled = true;
            }
        }

        public void NodeDragEnter(object sender, DragEventArgs e)
        {
            if (!e.Data.GetDataPresent(typeof(Node)) ||
                sender != e.Source)
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

                var dropTarget = treeViewItem.Header as Node;
                if ((node == dropTarget) || dropTarget == null || node == null)
                    return;

                node.Parent = dropTarget;
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

        public void Copy(object sender, RoutedEventArgs args)
        {
            IsCopying = true;
        }

        public void Cut()
        {

        }

        public void Paste()
        {

        }

        public bool IsPasteEnabled()
        {
            return false;
        }

        // Helper to search up the VisualTree
        private static T FindAnchestor<T>(DependencyObject current)
            where T : DependencyObject
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
