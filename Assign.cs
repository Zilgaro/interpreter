namespace Compilers {
    public class Assign : VisitableNode {
        private Var left;
        private VisitableNode right;

        private Token token;
        public override void accept(Visitor v) {
            v.visit(this);
        }

        public Assign(Var left, Token token, VisitableNode right) {
            this.left = left;
            this.right = right;
            this.token = token;
        }

        public Token GetToken() {
            return token;
        }

        public Var GetLeft() {
            return this.left;
        }

        public VisitableNode GetRight() {
            return this.right;
        }
    }
}