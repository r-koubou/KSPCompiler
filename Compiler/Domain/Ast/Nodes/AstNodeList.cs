using System.Collections;
using System.Collections.Generic;

namespace KSPCompiler.Domain.Ast.Nodes
{
    /// <summary>
    /// List representation of a node.
    /// </summary>
    public class AstNodeList<TNode> : IEnumerable<TNode> where TNode : class, IAstNode
    {
        private readonly List<TNode> nodeList = new();

        /// <summary>
        /// Source node.
        /// </summary>
        public IAstNode Parent { get; }

        /// <summary>
        /// Child nodes
        /// </summary>
        public IReadOnlyCollection<TNode> Nodes => new List<TNode>( nodeList );

        /// <summary>
        /// The number of child nodes. 0 if there are no child nodes.
        /// </summary>
        public int Count => nodeList.Count;

        /// <summary>
        /// true if there are no child nodes otherwise false.
        /// </summary>
        public bool Empty => nodeList.Count == 0;

        /// <summary>
        /// Ctor
        /// </summary>
        public AstNodeList() : this( NullAstNode.Instance ) {}

        /// <summary>
        /// Ctor
        /// </summary>
        public AstNodeList( IAstNode parent )
        {
            Parent = parent;
        }

        #region List controller
        public void AddRange( IList<TNode> list )
        {
            foreach( var n in list )
            {
                n.Parent = this.Parent;
                nodeList.Add( n );
            }
        }

        public void AddRange( AstNodeList<TNode> list )
            => AddRange( list.nodeList );

        public void Add( TNode node )
        {
            node.Parent = this.Parent;
            nodeList.Add( node );
        }

        public void Insert( int index, TNode node )
        {
            nodeList.Insert( index, node );
            node.Parent = this.Parent;
        }

        public void Remove( TNode node )
        {
            if( nodeList.Remove( node ) )
            {
                node.Parent = NullAstNode.Instance;
            }
        }

        public void Put( int index, TNode node )
        {
            nodeList[ index ] = node;
            node.Parent = this.Parent;
        }

        public void Clear()
        {
            foreach( var n in Nodes )
            {
                n.Parent = NullAstNode.Instance;
            }
            nodeList.Clear();
        }

        public bool Contains( TNode node )
            => nodeList.Contains( node );
        #endregion List controller

        #region Operator
        public TNode this [ int index ]
        {
            get => nodeList[ index ];
            set
            {
                var org  = nodeList[ index ];

                nodeList.RemoveAt( index );
                nodeList.Insert( index, value );

                org.Parent   = NullAstNode.Instance;
                value.Parent = this.Parent;
            }
        }
        #endregion Operator

        #region IEnumerable
        IEnumerator<TNode> IEnumerable<TNode>.GetEnumerator()
        {
            return Nodes.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return Nodes.GetEnumerator();
        }
        #endregion IEnumerable

    }
}
