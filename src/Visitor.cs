namespace Compilers {
    public interface Visitor {
        void visit (Root root);
        int visit (Num num);
        void visit (Assign assign);  
        dynamic visit (BinOp binOp);
        dynamic visit (Var var);

        void visit(VarDecl varDecl);

        void visit (TypeNode typeNode);

        void visit (Print print);

        string visit(Str str);

        bool visit(BoolNode boolNode);

        void visit (Assert assert);

        void visit(ReadNode readNode);

        void visit(ForNode forNode);
    }
}