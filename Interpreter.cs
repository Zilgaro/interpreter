using System;
using System.Collections;

namespace Compilers {
    class Interpreter {
        public Hashtable globalST; // Global scope symbol table
        private Parser parser;
        public Interpreter(Parser parser) {
            this.parser = parser;
            this.globalST = new Hashtable();
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
            Console.WriteLine("VISITING: " + v);
            if (v == TokenValues.INTEGER) {
                return this.visitNum(node);
            } else if (v == TokenValues.MULTIPLY || v == TokenValues.MINUS || v == TokenValues.PLUS || v ==  TokenValues.DIVISION) {
                return this.visitBinOp(node);
            } else if (v == TokenValues.ASSIGN) {
                this.visitAssign(node);
                return 0;
            } else if (v == TokenValues.ID) {
                return this.visitVar(node);
            } else {
                throw new InterpreterException("syntax error");
            }
        }

        public void visitProgram(AST node) {
            for (int i = 0; i < node.GetChildren().Length; i++) {
                visit(node.GetChildren()[i]);
            }
        }

        public void visitNoOp(AST node) {}

        public void visitAssign(AST node) {
            String name = node.GetLeft().GetToken().GetValue();
            globalST.Add(name, visit(node.GetRight()));
        }

        public int visitVar(AST node) {
            String name = node.GetToken().GetValue();
            AST value = (AST) globalST[name];
            if (value == null) {
                throw new InterpreterException(string.Format("Error accessing {0}", name));
            } else {
                Console.WriteLine("We here: " + Int32.Parse(value.GetToken().GetValue()));
                return Int32.Parse(value.GetToken().GetValue());
            }
        }

        public int interpret() {
            AST tree = this.parser.parse();
            var result = this.visit(tree);

            return result; 
        }
    }
}