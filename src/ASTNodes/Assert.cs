namespace Compilers {
    public class Assert : VisitableNode {
        
        private VisitableNode node;
        public Assert(VisitableNode n) {
            node = n;
        }

        public override void accept(Visitor v) => v.visit(this);

        public VisitableNode GetNode() {return this.node;}

    }
}