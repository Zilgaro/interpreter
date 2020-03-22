namespace Compilers {
    public interface Visitor {
        void visit (Root root);
        int visit (Num num);
        void visit (Assign assign);  
        int visit (BinOp binOp);
        int visit (Var var);

        void visit(VarDecl varDecl);

        void visit (TypeNode typeNode);

        void visit (Print print);
    }
}