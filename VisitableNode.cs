namespace Compilers {
    public abstract class VisitableNode {
        /*private Token token;
        private AST[] children;
        public AST(Token token, AST[] children) {
            this.token = token;
            this.children = children;
        }

        public Token GetToken() {
            return this.token;
        }

        public AST[] GetChildren() {
            return this.children;
        }

        public AST GetLeft() {
            return this.children[0];
        }
        public AST GetRight() {
            return this.children[1];
        }
        */
        public abstract void accept(Visitor v);
    }
}