using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Caliburn.Micro;

namespace CaliburnApp.UI.ViewModels
{
    public class Node
    {
        public int Id { get; set; }
        public int? ParentId { get; set; }
        public Node Parent { get; set; }
        public string Name { get; set; }
        public ObservableCollection<Node> Childs { get; set; }

        public Node(int id, string name, int? parentId = null)
        {
            Id = id;
            Name = name;
            ParentId = parentId;
            Childs = new ObservableCollection<Node>();
        }
    }

    public class TreeListViewModel : PropertyChangedBase
    {
        private Point startPoint;
        private TreeListViewModel parent;
        public ObservableCollection<Node> Nodes { get; set; }

        public TreeListViewModel()
        {
            SetDummyData();
        }

        private void SetDummyData()
        {
            // data retrived from database
            Nodes = new ObservableCollection<Node>();

            var root1 = new Node(1, "Parent 1");
            root1.Childs.Add(new Node(2, "Child 1", 1));
            root1.Childs.Add(new Node(3, "Child 2", 1));
            root1.Childs.Add(new Node(4, "Child 3", 1));
            root1.Childs.Add(new Node(5, "Child 4", 1));
            root1.Childs.Add(new Node(6, "Child 5", 1));
            Nodes.Add(root1);

            var root2 = new Node(7, "Parent 2");
            root2.Childs.Add(new Node(8, "Child 1", 7));
            root2.Childs.Add(new Node(9, "Child 2", 7));
            root2.Childs.Add(new Node(10, "Child 3", 7));
            root2.Childs.Add(new Node(11, "Child 4", 7));
            root2.Childs.Add(new Node(12, "Child 5", 7));
            Nodes.Add(root2);
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

                if (isExpanded && parent != null && !parent.IsExpanded)
                    parent.IsExpanded = true;

                //lazy loading of children
                //if (!childrenLoaded)
                //{
                //    this.Children.Remove(emptyChild);
                //    foreach (var directory in this.info.EnumerateDirectories())
                //    {
                //        children.Add(new FolderViewModel(directory, this));
                //    }
                //    childrenLoaded = true;
                //}
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

                var treeViewItem =
                    FindAnchestor<TreeViewItem>((DependencyObject)e.OriginalSource);

                var dropTarget = treeViewItem.Header as Node;

                if (dropTarget == null || node == null)
                    return;

                node.Parent = dropTarget;
            }
        }

        public void MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                var mousePos = e.GetPosition(null);
                var diff = startPoint - mousePos;

                if (Math.Abs(diff.X) > SystemParameters.MinimumHorizontalDragDistance
                    || Math.Abs(diff.Y) > SystemParameters.MinimumVerticalDragDistance)
                {
                    var treeView = sender as TreeView;
                    var treeViewItem =
                        FindAnchestor<TreeViewItem>((DependencyObject)e.OriginalSource);

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

        public void PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            startPoint = e.GetPosition(null);
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
