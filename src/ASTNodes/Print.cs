namespace Compilers {
    public class Print : VisitableNode {
        
        private VisitableNode node;
        public Print(VisitableNode n) {
            node = n;
        }

        public override void accept(Visitor v) => v.visit(this);

        public VisitableNode GetNode() {return this.node;}

    }
}
