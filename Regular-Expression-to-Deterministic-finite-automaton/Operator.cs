using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnaryOperation = System.Func<string, string>;
using BinaryOperation = System.Func<string, string, string>;

namespace dfa
{
    internal class Operator
    {
        public static Dictionary<string, byte> Arities = new Dictionary<string, byte>
            {
                { "|", 2 },
                { ".", 2 },
                { "*", 1 }
            };

        public static Dictionary<string, Delegate> Operations = new Dictionary<string, Delegate>
            {
                { "|", new BinaryOperation(Concatenate) },
                { ".", new BinaryOperation(Alternate) },
                { "*", new UnaryOperation(Stelate) }
            };

        public static string Concatenate(string lhs, string rhs)
        {
            return lhs + rhs;
        }

        public static string Alternate(string lhs, string rhs)
        {
            return "";
        }

        public static string Stelate(string lhs)
        {
            return "";
        }


        public string Label;
        public byte Arity;
        public Delegate Operation;

        public Operator(string label)
        {
            Label = label;
            Arity = Operator.Arities[label];
            Operation = Operator.Operations[label];
        }

        public string Execute(string lhs, string rhs)
        {
            if (Arity == 2 && Operation is BinaryOperation binaryOp)
            {
                return binaryOp(lhs, rhs);
            }
            throw new InvalidOperationException("Operation is not binary.");
        }

        public string Execute(string lhs)
        {
            if (Arity == 1 && Operation is UnaryOperation unaryOp)
            {
                return unaryOp(lhs);
            }
            throw new InvalidOperationException("Operation is not unary.");
        }
    }
}

