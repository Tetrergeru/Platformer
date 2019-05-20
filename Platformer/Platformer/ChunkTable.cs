using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Math;

namespace Platformer
{
    class ChunkTable<T> : IEnumerable<T>
    {
        private List<Node> elems = new List<Node>();
        private Dictionary<(int x, int y), List<Node>> table = new Dictionary<(int x, int y), List<Node>>();
        public Dictionary<(int x, int y), List<Node>> validTable = new Dictionary<(int x, int y), List<Node>>();
        private Func<T, IRectangle> func;
        private (double x, double y) chunkSize;
        
        public class Node
        {
            public IntRectangle rectangle;
            public IRectangle r;
            public T value;
        }

        public ChunkTable(Func<T, IRectangle> func, (double x, double y) chunkSize)
        {
            this.chunkSize = chunkSize;
            this.func = func;
        }

        public void Add(T value)
        {
            var rect = GetRctangle(value);
            Node node = new Node {  value = value, rectangle = rect.rectangle, r = rect.r};
            elems.Add(node);

            for (int i = node.rectangle.X1; i <= node.rectangle.X2; ++i)
                for (int j = node.rectangle.Y1; j <= node.rectangle.Y2; ++j)
                    AddFromKey((i, j), node);
        }

        public bool Remove(T value)
        {
            Node node = elems.Find(n => n.value.Equals(value));
            if (node == null)
                return false;

            elems.Remove(node);
            for (int i = node.rectangle.X1; i <= node.rectangle.X2; ++i)
                for (int j = node.rectangle.Y1; j <= node.rectangle.Y2; ++j)
                    RemoveFromKey((i, j), node);
            return true;
        }

        private (IntRectangle rectangle, IRectangle r) GetRctangle(T value)
        {
            IRectangle rect = func(value);
            return (Scale(rect), rect);
        }

        private IntRectangle Scale(IRectangle rectangle)
        {
            return new IntRectangle
            {
                X1 = (int)Floor(rectangle.X / chunkSize.x),
                Y1 = (int)Floor(rectangle.Y / chunkSize.y),
                X2 = (int)Floor(rectangle.X2 / chunkSize.x),
                Y2 = (int)Floor(rectangle.Y2 / chunkSize.y),
            };
        }

        private void MoveNode(Node node)
        {
            var rect = GetRctangle(node.value);

            IntRectangle newR = rect.rectangle;
            IntRectangle lastR = node.rectangle;

            node.rectangle = newR;
            node.r = rect.r;
            
            if (newR.X1 == lastR.X1 &&
                newR.X2 == lastR.X2 &&
                newR.Y1 == lastR.Y1 &&
                newR.Y2 == lastR.Y2)
                return;

            for (int i = lastR.X1; i <= lastR.X2; ++i)
                if (i < newR.X1 || i > newR.X2)
                    for (int j = lastR.Y1; j <= lastR.Y2; ++j)
                        RemoveFromKey((i, j), node);

            for (int j = lastR.Y1; j <= lastR.Y2; ++j)
                if (j < newR.Y1 || j > newR.Y2)
                    for (int i = lastR.X1; i <= lastR.X2; ++i)
                        if (!(i < newR.X1 || i > newR.X2))
                            RemoveFromKey((i, j), node);

            for (int i = newR.X1; i <= newR.X2; ++i)
                if (i < lastR.X1 || i > lastR.X2)
                    for (int j = newR.Y1; j <= newR.Y2; ++j)
                        AddFromKey((i, j), node);

            for (int j = newR.Y1; j <= newR.Y2; ++j)
                if (j < lastR.Y1 || j > lastR.Y2)
                    for (int i = newR.X1; i <= newR.X2; ++i)
                        if (!(i < lastR.X1 || i > lastR.X2))
                            AddFromKey((i, j), node);
        }

        private void RemoveFromKey((int, int) key, Node node)
        {
            List<Node> nodes = table[key];
            nodes.Remove(node);
            
            if (nodes.Count == 1)
                validTable.Remove(key);
            if (nodes.Count == 0)
                table.Remove(key);
        }

        private void AddFromKey((int, int) key, Node node)
        {
            if (!table.ContainsKey(key))
                table.Add(key, new List<Node>());
            List<Node> nodes = table[key];
            nodes.Add(node);
            if (nodes.Count == 2)
                validTable.Add(key, nodes);
        }

        public void Refresh()
        {
            foreach(Node node in elems)
                MoveNode(node);
        }

        public IEnumerable<(T, T)> Collisions()
        { 
            foreach (var pair in table)
                    for(int i = 0; i < pair.Value.Count; i++)
                        for(int j = i + 1; j < pair.Value.Count; j++)
                        {
                            var node1 = pair.Value[i];
                            var node2 = pair.Value[j];
                            if (node1 != node2
                                && !((node1.rectangle.X1 < pair.Key.x && node2.rectangle.X1 < pair.Key.x) || (node1.rectangle.Y1 < pair.Key.y && node2.rectangle.Y1 < pair.Key.y))
                                && IsCollision(node1, node2))
                                yield return (node1.value, node2.value);
                        }
            yield break;
        }

        private bool IsCollision(Node n1, Node n2)
        {
            return !(
                n1.r.X2 < n2.r.X ||
                n2.r.X2 < n1.r.X ||
                n1.r.Y2 < n2.r.Y ||
                n2.r.Y2 < n1.r.Y);
        }

        public IEnumerator GetEnumerator()
        {
            return elems.Select(n => n.value).GetEnumerator();
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return elems.Select(n => n.value).GetEnumerator();
        }
    }

    class IntRectangle
    {
        public int X1 { get; set; }
        public int Y1 { get; set; }
        public int X2 { get; set; }
        public int Y2 { get; set; }
    }
}
