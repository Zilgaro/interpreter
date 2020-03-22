namespace Compilers {
    public class Str : VisitableNode {
        private Token token;
        private string value;

        public Str(Token t) {
            this.token = t;
            this.value = t.GetValue();
        }

        public override void accept(Visitor v) => v.visit(this);

        public string getValue() {return this.value;}

    }
}