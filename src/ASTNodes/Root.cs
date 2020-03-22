using System.Collections.Generic;
using System.Collections;

namespace Compilers {
    public class Root : VisitableNode {
        private List<VisitableNode> children;

        public Root() {
            this.children = new List<VisitableNode>();
        }
        public override void accept(Visitor v) => v.visit(this);

        public VisitableNode getChild(int index) {
            return children[index];
        }

        public List<VisitableNode> getChildren() {
            return children;
        }

        public void addChild(VisitableNode c) {
            this.children.Add(c);
        }

        public void setChildren(List<VisitableNode> children) {
            this.children = children;
        }
    }
}