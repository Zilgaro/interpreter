namespace Compilers {
    public class BinOp : VisitableNode {
        private VisitableNode left, right;
        private Token op;

        public BinOp(VisitableNode left, Token op, VisitableNode right) {
            this.left = left;
            this.op = op;
            this.right = right;
        }

        public override void accept(Visitor v) {
            v.visit(this);
        }

        public Token getOp() {return this.op;}

        public VisitableNode getLeft() {return this.left;}

        public VisitableNode getRight() {return this.right;}
    }
}
