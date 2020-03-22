using System.Collections.Generic;
namespace Compilers {
    public class ForNode : VisitableNode {
        private Var counter;
        private VisitableNode precon;
        private List<VisitableNode> statements;
        private Token op;

        public ForNode(Var counter, VisitableNode precon, List<VisitableNode> statements) {
            this.counter = counter;
            this.precon = precon;
            this.statements = statements;
        }

        public override void accept(Visitor v) {
            v.visit(this);
        }

        public Var getCounter() {return this.counter;}

        public VisitableNode getPrecon() {return this.precon;}

        public List<VisitableNode> getStatements() {return this.statements;}
    }
}
