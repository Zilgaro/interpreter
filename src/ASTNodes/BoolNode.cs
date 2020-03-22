namespace Compilers {
    public class BoolNode : VisitableNode {
        private Token token;
        private string value;

        public BoolNode(Token t) {
            this.token = t;
            this.value = t.GetValue();
        }

        public override void accept(Visitor v) => v.visit(this);

        public string getValue() {return this.value;}

    }
}
