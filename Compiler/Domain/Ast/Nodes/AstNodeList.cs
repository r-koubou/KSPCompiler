using System.Collections;
using System.Collections.Generic;

namespace KSPCompiler.Domain.Ast.Node
{
    /// <summary>
    /// List representation of a node.
    /// </summary>
    public class AstNodeList<TNode> : IEnumerable<TNode> where TNode : class, IAstNode
    {
        /// <summary>
        /// Source node.
        /// </summary>
        public IAstNode Parent { get; }

        /// <summary>
        /// Child nodes
        /// </summary>
        public List<TNode> Nodes { get; } = new();

        /// <summary>
        /// The number of child nodes. 0 if there are no child nodes.
        /// </summary>
        public int Count => Nodes.Count;

        /// <summary>
        /// true if there are no child nodes otherwise false.
        /// </summary>
        public bool Empty => Nodes.Count == 0;

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
        public void AddRange( List<TNode> list )
        {
            foreach( var n in list )
            {
                n.Parent = this.Parent;
                Nodes.Add( n );
            }
        }

        public void AddRange( AstNodeList<TNode> list )
            => AddRange( list.Nodes );

        public void Add( TNode node )
        {
            node.Parent = this.Parent;
            Nodes.Add( node );
        }

        public void Insert( int index, TNode node )
        {
            Nodes.Insert( index, node );
            node.Parent = this.Parent;
        }

        public void Remove( TNode node )
        {
            if( Nodes.Remove( node ) )
            {
                node.Parent = NullAstNode.Instance;
            }
        }

        public void Clear()
        {
            foreach( var n in Nodes )
            {
                n.Parent = NullAstNode.Instance;
            }
            Nodes.Clear();
        }

        public bool Contains( TNode node )
            => Nodes.Contains( node );
        #endregion List controller

        #region Operator
        public TNode this [ int index ]
        {
            get
            {
                return this.Nodes[ index ];
            }
            set
            {
                var list = Nodes;
                var org  = list[ index ];

                list.RemoveAt( index );
                list.Insert( index, value );

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
