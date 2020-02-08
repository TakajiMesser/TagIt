using System.Collections.Generic;
using System.IO;
using TagIt.WPF.Models.Videos;

namespace TagIt.WPF.Models.Explorers
{
    /*public class PathNode
    {
        private Dictionary<string, PathNode> _childrenByID = new Dictionary<string, PathNode>();

        public string Name { get; }
        public string ID { get; }
        public string Path { get; }
        public List<IVideo> Videos { get; } = new List<IVideo>();

        public PathNode Parent { get; private set; }
        public IEnumerable<PathNode> Children => _childrenByID.Values;

        public PathNode(string path)
        {
            Name = new DirectoryInfo(path).Name;
            Path = path;
        }

        public void AddChild(PathNode node)
        {
            _childrenByID.Add(node.ID, node);
            node.Parent = this;
        }

        public PathNode GetChild(string id) => _childrenByID[id];

        public void ClearChildren() => _childrenByID.Clear();
    }*/
}
