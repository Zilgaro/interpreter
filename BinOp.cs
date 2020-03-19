namespace Compilers {
    public class BinOp : VisitableNode {
        private VisitableNode left, right;
        private Token op;
        public void accept(Visitor v) {
            v.visit(this);
        }

        public Token getOp() {return this.op;}

        public VisitableNode getLeft() {return this.left;}

        public VisitableNode getRight() {return this.right;}

    }
}
