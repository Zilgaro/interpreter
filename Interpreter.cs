using System;
namespace Compilers {
    class Interpreter {
        private Parser parser;
        public Interpreter(Parser parser) {
            this.parser = parser;
        } 

        public int visitNum(AST node) {
            if (node.GetToken().GetTokenValueType() == TokenValues.INTEGER) {
                return Int32.Parse(node.GetToken().GetValue());
            } else {
                throw(new InterpreterException("Invalid syntax"));
            }
        }

        public int visitBinOp(AST node) {
            int left = this.visit(node.GetChildren()[0]);
            int right = this.visit(node.GetChildren()[1]);   

            switch (node.GetToken().GetTokenValueType()) {
                case TokenValues.PLUS:
                    return left + right;
                case TokenValues.MINUS:
                    return left - right;
                case TokenValues.MULTIPLY:
                    return left * right;
                case TokenValues.DIVISION:
                    return left / right;
                default:
                    throw(new InterpreterException("Invalid syntax"));
            }
        }

        public int visit (AST node) {
            TokenValues v = node.GetToken().GetTokenValueType();
            if (v == TokenValues.INTEGER) {
                return this.visitNum(node);
            } else if (v == TokenValues.MULTIPLY || v == TokenValues.MINUS || v == TokenValues.PLUS || v ==  TokenValues.DIVISION) {
                return this.visitBinOp(node);
            } else {
                throw new InterpreterException("syntax error");
            }
        }

        public int interpret() {
            AST tree = this.parser.parse();
            var result = this.visit(tree);

            return result; 
        }
    }
}