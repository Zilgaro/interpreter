namespace Compilers {
    public class NoOp : VisitableNode {
        public NoOp() {}

        public override void accept(Visitor v) {
            return;
        }
    }
}