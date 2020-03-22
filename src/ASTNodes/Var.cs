namespace Compilers {
public class Var : VisitableNode{
    private Token token;
    private string value;
    public Var(Token token) {
        this.token = token;
        this.value = token.GetValue();
    }

    public string GetValue() {
        return value;
    }

    public Token GetToken() {
        return token;
    }

        public override void accept(Visitor v) => v.visit(this);

        public dynamic getClass() {
        return this.GetType();
    }
}
}