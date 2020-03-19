namespace Compilers {
    public class Root : VisitableNode {
        private VisitableNode[] children;

        public Root() {
            this.children = new VisitableNode[2];
        }
        public void accept(Visitor v) {
            v.visit(this);
        }

        public VisitableNode getChild(int index) {
            return children[index];
        }
    }
}