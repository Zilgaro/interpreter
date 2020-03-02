using System;

namespace Compilers
{
    class Token {
        private TokenValues type;
        //char cannot be null in C# so let's use string
        private string value;
        public Token(TokenValues type, string value) {
            this.type = type;
            this.value = value;
        }

        public TokenValues GetTokenValueType() {
            return this.type;
        }

        public string GetValue() {
            return this.value;
        }


        public override string ToString() {
            return $"Token({this.type}, {this.value})";
        }
    }
}