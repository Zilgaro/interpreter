namespace Compilers {
    public class ReadNode : VisitableNode {
        
        private VisitableNode node;
        public ReadNode(VisitableNode n) {
            node = n;
        }

        public override void accept(Visitor v) => v.visit(this);

        public VisitableNode GetNode() {return this.node;}

    }
}
