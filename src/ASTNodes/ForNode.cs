using System.Collections.Generic;
namespace Compilers {
    public class ForNode : VisitableNode {
        private Var counter;
        private VisitableNode from, to;
        private List<VisitableNode> statements;
        
        public ForNode(Var counter, VisitableNode from, VisitableNode to, List<VisitableNode> statements) {
            this.counter = counter;
            this.from = from;
            this.to = to;
            this.statements = statements;
        }

        public override void accept(Visitor v) {
            v.visit(this);
        }

        public Var getCounter() {return this.counter;}

        public VisitableNode getFrom() {return this.from;}
        public VisitableNode getTo() {return this.to;}
        public List<VisitableNode> getStatements() {return this.statements;}
    }
}
