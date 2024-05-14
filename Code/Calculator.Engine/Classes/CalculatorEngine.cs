using Calculator.Engine.Dto;
using Calculator.Engine.Enums;
using Calculator.Engine.Exceptions;
using Calculator.Engine.Interfaces;

namespace Calculator.Engine.Classes;

public class CalculatorEngine : ICalculatorEngine
{
    public ExpressionResultDto Calculate(ExpressionDto expression)
    {
        return expression.Operator switch
        {
            Operators.Addition => PerformArithmeticOperation(expression, (a, b) => a + b),
            Operators.Subtraction => PerformArithmeticOperation(expression, (a, b) => a - b),
            Operators.Division => PerformArithmeticOperation(expression, (a, b) => a / b),
            Operators.Multiplication => PerformArithmeticOperation(expression, (a, b) => a * b),
            _ => throw new ComputeException(message: $"Unknown operation: {expression.Operator}"),
        };
    }

    public delegate double ArithmeticOperation(double a, double b);

    public ExpressionResultDto PerformArithmeticOperation(ExpressionDto expressionDto, ArithmeticOperation operation)
    {
        try
        {
            var result = operation(expressionDto.FirstOperand ?? 1.0, expressionDto.SecondOperand ?? 1.0);
            return new ExpressionResultDto(result, expressionDto);
        }
        catch (Exception ex) {
            throw new ComputeException(message: $"Error performing operation: {expressionDto.Operator}, for operand: {expressionDto.FirstOperand} and {expressionDto.SecondOperand}. \n{ex.Message}");
        }
    }
}
