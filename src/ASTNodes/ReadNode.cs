namespace Compilers {
    public class ReadNode : VisitableNode {
        
        private Var node;
        public ReadNode(Var n) {
            node = n;
        }

        public override void accept(Visitor v) => v.visit(this);

        public Var GetNode() {return this.node;}

    }
}
