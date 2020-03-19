using System.Collections;
using System;
namespace Compilers {
    class AstVisitor : Visitor {

        private Hashtable globalST;
        public AstVisitor() {
            globalST = new Hashtable();
        }

        public void visit(Root root) {
            root.getChild(0).accept(this);
        }

        public void visit(Assign assign) {

        }

        public void visit(Num num) {

        }

        public void visit(Var var) {

        }

        public void visit(BinOp binOp) {

        }
    }
}