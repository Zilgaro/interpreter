namespace Compilers {
    public class VarDecl : VisitableNode {
        private Var varNode;
        private TypeNode typeNode;
        private Assign assign;
        public VarDecl(Var v, TypeNode t) {
            varNode = v;
            typeNode = t;
            assign = null;
        }

        public VarDecl(Var var, TypeNode type, Assign a) {
            varNode = var;
            typeNode = type;
            assign = a;
        }

        public override void accept(Visitor v) => v.visit(this);

        public Var GetVarNode() {
            return varNode;
        }

        public TypeNode GetTypeNode() {
            return typeNode;
        }

        public Assign GetAssign() {
            return assign;
        }
    }

}