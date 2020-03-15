namespace Compilers {
    class AST {
        private Token token;
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

    }
}