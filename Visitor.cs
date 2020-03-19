namespace Compilers {
    public interface Visitor {
        void visit (Root root);
        void visit (Num num);
        void visit (Assign assign);  
        void visit (BinOp binOp);
        void visit (Var var);


    }
}